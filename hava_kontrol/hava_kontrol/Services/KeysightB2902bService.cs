using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using hava_kontrol.Models;

namespace hava_kontrol.Services
{
    /// <summary>
    /// Keysight B2902B Precision Source/Measure Unit (SMU) haberleşme servisi.
    /// USB (USB-TMC / VISA) ve RS-232 / Sanal COM port üzerinden SCPI komutları ile tam kontrol sağlar.
    /// 
    /// Özellikler:
    ///   - 2 Bağımsız SMU Kanalı (Ch1, Ch2)
    ///   - USB-TMC VISA otomatik cihaz tespiti (*IDN? filtreleme)
    ///   - Gerilim / Akım kaynağı ayarlama ve koruma (Compliance) yönetimi
    ///   - Yüksek hassasiyetli Akım, Gerilim ve Direnç ölçümleri
    ///   - Bağımsız çıkış kontrolü (:OUTP1:STAT ON/OFF, :OUTP2:STAT ON/OFF)
    ///   - Thread-safe (lock ile korunur)
    /// </summary>
    public class KeysightB2902bService : IDisposable
    {
        private SerialPort? _port;
        private VisaSession? _visa;
        private readonly object _lock = new();
        private bool _disposed;
        private int _consecutiveFailures;
        private readonly bool[] _cachedOutput = new bool[3];

        public bool IsConnected => (_port?.IsOpen == true) || (_visa?.IsOpen == true);
        public bool IsVisaConnection => _visa?.IsOpen == true;
        public string ConnectionEndpoint { get; private set; } = string.Empty;
        public string ComPort { get; private set; } = KeysightB2902bConfig.DefaultComPort;
        public int BaudRate { get; private set; } = KeysightB2902bConfig.DefaultBaudRate;
        public string DeviceIdentifier { get; private set; } = string.Empty;
        public string LastError { get; private set; } = string.Empty;

        /// <summary>
        /// Bağlı USB cihazları arasından Keysight B2900 SMU adresini tespit eder.
        /// </summary>
        public static string? FindKeysightUsbResource()
        {
            var usbList = VisaSession.FindUsbInstruments();
            if (usbList.Count == 0) return null;

            // 1. Keysight / Agilent USB Vendor ID'leri: 0x0957, 0x2A8D veya B290
            string? candidate = usbList.FirstOrDefault(u =>
                u.Contains("0x0957", StringComparison.OrdinalIgnoreCase) ||
                u.Contains("0x2A8D", StringComparison.OrdinalIgnoreCase) ||
                u.Contains("B290", StringComparison.OrdinalIgnoreCase));

            if (candidate != null) return candidate;

            // 2. Doğrulama: Rigol olmayan cihazları *IDN? ile tara
            foreach (var res in usbList)
            {
                if (res.Contains("0x1AB1", StringComparison.OrdinalIgnoreCase)) continue;
                using var probe = new VisaSession();
                if (probe.Open(res, 800))
                {
                    string idn = probe.Query(KeysightB2902bConfig.CmdIdentify);
                    if (IsKeysightIdentifier(idn))
                        return res;
                }
            }

            return null;
        }

        // ======================================
        // BAĞLANTI YÖNETİMİ
        // ======================================

        /// <summary>
        /// Belirtilen USB-VISA adresi veya COM port üzerinden Keysight B2902B'ye bağlanır.
        /// Endpoint boş veya "USB (Keysight Otomatik)" ise bağlı USB cihazları taranır.
        /// </summary>
        public bool Connect(string? endpoint = null, int baudRate = KeysightB2902bConfig.DefaultBaudRate)
        {
            lock (_lock)
            {
                try
                {
                    LastError = string.Empty;
                    Disconnect();

                    endpoint = string.IsNullOrWhiteSpace(endpoint) ? KeysightB2902bConfig.DefaultUsbEndpoint : endpoint.Trim();
                    ConnectionEndpoint = endpoint;

                    if (endpoint.StartsWith("USB", StringComparison.OrdinalIgnoreCase))
                    {
                        // VISA USB-TMC Bağlantısı
                        string visaResource = endpoint;
                        if (endpoint.Contains("Otomatik", StringComparison.OrdinalIgnoreCase) ||
                            endpoint.Equals("USB", StringComparison.OrdinalIgnoreCase))
                        {
                            string? found = FindKeysightUsbResource();
                            if (found == null)
                            {
                                var usbList = VisaSession.FindUsbInstruments();
                                if (usbList.Count == 0)
                                {
                                    LastError = "Bağlı USB cihazı bulunamadı. Keysight B2902B USB kablosunu kontrol edin.";
                                    return false;
                                }
                                visaResource = usbList[0];
                            }
                            else
                            {
                                visaResource = found;
                            }
                        }

                        _visa = new VisaSession();
                        if (!_visa.Open(visaResource, KeysightB2902bConfig.ReadTimeoutMs))
                        {
                            LastError = $"VISA üzerinden bağlanılamadı ({visaResource}).";
                            Disconnect();
                            return false;
                        }

                        DeviceIdentifier = SendQuery(KeysightB2902bConfig.CmdIdentify);
                        if (string.IsNullOrWhiteSpace(DeviceIdentifier))
                        {
                            LastError = "Cihazdan kimlik yanıtı alınamadı (*IDN? zaman aşımı).";
                            Disconnect();
                            return false;
                        }

                        if (!IsKeysightIdentifier(DeviceIdentifier))
                        {
                            LastError = $"Bağlanan cihaz Keysight B2900 serisi değil: {DeviceIdentifier}";
                            Disconnect();
                            return false;
                        }

                        // Uzaktan kontrol modunu aktif et
                        SendCommand(KeysightB2902bConfig.CmdRemote);

                        // KRİTİK: Keysight B2900 fabrika ayarı olarak tetikleme geldiğinde çıkışı otomatik açar (Automatic Output-ON).
                        // Kullanıcı çıkışı kapattığında ölçüm sorguları sebebiyle cihazın çıkışı kendiliğinden tekrar açmasını engellemek için
                        // hem Ch1 hem de Ch2 için Auto Output-ON ve Auto Output-OFF fonksiyonları kapatılır!
                        SendCommand(KeysightB2902bConfig.CmdSetAutoOutputOn(1, false));
                        SendCommand(KeysightB2902bConfig.CmdSetAutoOutputOn(2, false));
                        SendCommand(KeysightB2902bConfig.CmdSetAutoOutputOff(1, false));
                        SendCommand(KeysightB2902bConfig.CmdSetAutoOutputOff(2, false));

                        // Hızlı ve gecikmesiz çıkış kapatma için High-Z (HIZ) modu
                        SendCommand(KeysightB2902bConfig.CmdSetOutputOffMode(1, "HIZ"));
                        SendCommand(KeysightB2902bConfig.CmdSetOutputOffMode(2, "HIZ"));

                        // Hızlı ve kararlı entegrasyon (1 NPLC = 20ms @50Hz)
                        SendCommand(KeysightB2902bConfig.CmdSetNplc(1, 1.0));
                        SendCommand(KeysightB2902bConfig.CmdSetNplc(2, 1.0));

                        _consecutiveFailures = 0;
                        Debug.WriteLine($"[Keysight B2902B USB-VISA] Başarıyla bağlandı: {DeviceIdentifier} ({visaResource})");
                        return true;
                    }
                    else
                    {
                        // Standart Seri COM Port Bağlantısı
                        ComPort = endpoint;
                        BaudRate = baudRate;

                        _port = new SerialPort(ComPort)
                        {
                            BaudRate = BaudRate,
                            DataBits = KeysightB2902bConfig.DataBits,
                            Parity = Parity.None,
                            StopBits = StopBits.One,
                            Handshake = Handshake.None,
                            DtrEnable = true,
                            RtsEnable = true,
                            ReadTimeout = KeysightB2902bConfig.ReadTimeoutMs,
                            WriteTimeout = KeysightB2902bConfig.WriteTimeoutMs,
                            NewLine = KeysightB2902bConfig.LineTerminator
                        };

                        _port.Open();

                        // Cihazı tanımla (*IDN?)
                        DeviceIdentifier = SendQuery(KeysightB2902bConfig.CmdIdentify);
                        if (string.IsNullOrWhiteSpace(DeviceIdentifier) || !IsKeysightIdentifier(DeviceIdentifier))
                        {
                            LastError = string.IsNullOrWhiteSpace(DeviceIdentifier)
                                ? "Seri port açıldı fakat cihazdan yanıt alınamadı (*IDN? zaman aşımı)."
                                : $"Bağlanan cihaz Keysight B2900 serisi değil: {DeviceIdentifier}";
                            Disconnect();
                            return false;
                        }

                        SendCommand(KeysightB2902bConfig.CmdRemote);
                        SendCommand(KeysightB2902bConfig.CmdSetAutoOutputOn(1, false));
                        SendCommand(KeysightB2902bConfig.CmdSetAutoOutputOn(2, false));
                        SendCommand(KeysightB2902bConfig.CmdSetAutoOutputOff(1, false));
                        SendCommand(KeysightB2902bConfig.CmdSetAutoOutputOff(2, false));
                        SendCommand(KeysightB2902bConfig.CmdSetOutputOffMode(1, "HIZ"));
                        SendCommand(KeysightB2902bConfig.CmdSetOutputOffMode(2, "HIZ"));
                        SendCommand(KeysightB2902bConfig.CmdSetNplc(1, 1.0));
                        SendCommand(KeysightB2902bConfig.CmdSetNplc(2, 1.0));

                        _consecutiveFailures = 0;
                        Debug.WriteLine($"[Keysight B2902B RS232] Başarıyla bağlandı: {DeviceIdentifier}");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    Debug.WriteLine($"[Keysight B2902B] Bağlantı hatası: {ex.Message}");
                    Disconnect();
                    return false;
                }
            }
        }

        private static bool IsKeysightIdentifier(string idn)
        {
            if (string.IsNullOrWhiteSpace(idn)) return false;
            return idn.Contains("B2902", StringComparison.OrdinalIgnoreCase) ||
                   idn.Contains("B290", StringComparison.OrdinalIgnoreCase) ||
                   idn.Contains("Keysight", StringComparison.OrdinalIgnoreCase) ||
                   idn.Contains("Agilent", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Bağlantıyı kapatır ve cihazı yerel (Local) moda geçirir.</summary>
        public void Disconnect()
        {
            lock (_lock)
            {
                try
                {
                    _consecutiveFailures = 0;

                    if (IsConnected)
                    {
                        try { SendCommand(KeysightB2902bConfig.CmdLocal); } catch { }
                    }

                    if (_port?.IsOpen == true)
                    {
                        _port.Close();
                    }
                    _port?.Dispose();
                    _port = null;

                    _visa?.Dispose();
                    _visa = null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Keysight B2902B] Disconnect hatası: {ex.Message}");
                }
            }
        }

        // ======================================
        // ÇIKIŞ KONTROLÜ (ON / OFF)
        // ======================================

        /// <summary>
        /// Belirtilen kanalın çıkışını açar veya kapatır.
        /// </summary>
        public bool SetChannelOutput(int channel, bool on)
        {
            if (channel < 1 || channel > 2) return false;
            _cachedOutput[channel] = on;
            if (!on)
            {
                // Önce devam eden ölçüm veya entegrasyonu derhal iptal et (Abort)
                SendCommand(KeysightB2902bConfig.CmdAbort(channel));
            }
            return SendCommand(KeysightB2902bConfig.CmdSetOutput(channel, on));
        }

        /// <summary>
        /// Belirtilen kanalın çıkış durumunu sorgular.
        /// </summary>
        public bool GetChannelOutput(int channel)
        {
            if (channel < 1 || channel > 2) return false;
            string resp = SendQuery(KeysightB2902bConfig.CmdGetOutput(channel)).Trim().ToUpperInvariant();
            if (resp == "1" || resp == "ON")
            {
                _cachedOutput[channel] = true;
                return true;
            }
            if (resp == "0" || resp == "OFF")
            {
                _cachedOutput[channel] = false;
                return false;
            }
            // Yanıt boş veya zaman aşımı ise son bilinen durumu koru
            return _cachedOutput[channel];
        }

        /// <summary>
        /// Tüm kanalların (Ch1 ve Ch2) çıkışlarını kapatır.
        /// Acil durdurma (E-Stop) veya güvenlik kapatmalarında kullanılır.
        /// </summary>
        public bool TurnAllOutputsOff()
        {
            SendCommand(KeysightB2902bConfig.CmdAbort());
            _cachedOutput[1] = false;
            _cachedOutput[2] = false;
            bool ok = SetChannelOutput(1, false);
            ok &= SetChannelOutput(2, false);
            return ok;
        }

        // ======================================
        // KAYNAK (SOURCE) AYARLARI
        // ======================================

        /// <summary>
        /// Belirtilen kanalda gerilim kaynağı modunu, hedef gerilimi ve akım koruma sınırını (compliance) ayarlar.
        /// </summary>
        public bool SetSourceVoltage(int channel, float voltage, float complianceCurrent = KeysightB2902bConfig.DefaultCurrentCompliance)
        {
            if (channel < 1 || channel > 2) return false;

            voltage = Math.Clamp(voltage, KeysightB2902bConfig.VoltageSourceMin, KeysightB2902bConfig.VoltageSourceMax);
            complianceCurrent = Math.Clamp(complianceCurrent, 1e-9f, KeysightB2902bConfig.CurrentSourceMax);

            bool ok = SendCommand(KeysightB2902bConfig.CmdSetSourceMode(channel, isVoltageMode: true));
            ok &= SendCommand(KeysightB2902bConfig.CmdSetCurrentCompliance(channel, complianceCurrent));
            ok &= SendCommand(KeysightB2902bConfig.CmdSetSourceVoltage(channel, voltage));
            return ok;
        }

        /// <summary>
        /// Belirtilen kanalda akım kaynağı modunu, hedef akımı ve voltaj koruma sınırını ayarlar.
        /// </summary>
        public bool SetSourceCurrent(int channel, float current, float complianceVoltage = KeysightB2902bConfig.DefaultVoltageCompliance)
        {
            if (channel < 1 || channel > 2) return false;

            current = Math.Clamp(current, KeysightB2902bConfig.CurrentSourceMin, KeysightB2902bConfig.CurrentSourceMax);
            complianceVoltage = Math.Clamp(complianceVoltage, 0.1f, KeysightB2902bConfig.VoltageSourceMax);

            bool ok = SendCommand(KeysightB2902bConfig.CmdSetSourceMode(channel, isVoltageMode: false));
            ok &= SendCommand(KeysightB2902bConfig.CmdSetVoltageCompliance(channel, complianceVoltage));
            ok &= SendCommand(KeysightB2902bConfig.CmdSetSourceCurrent(channel, current));
            return ok;
        }

        /// <summary>
        /// Kanalın ayarlı kaynak gerilim değerini sorgular.
        /// </summary>
        public float GetSourceVoltage(int channel)
        {
            if (channel < 1 || channel > 2) return 0f;
            string resp = SendQuery(KeysightB2902bConfig.CmdGetSourceVoltage(channel));
            if (float.TryParse(resp, NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
                return val;
            return 0f;
        }

        // ======================================
        // ÖLÇÜM (MEASURE) FONKSİYONLARI
        // ======================================

        /// <summary>
        /// Seçili fonksiyona göre anlık spot ölçüm yapar.
        /// </summary>
        public double Measure(int channel, ElectrometerMeasureMode mode)
        {
            return mode switch
            {
                ElectrometerMeasureMode.Current => MeasureCurrent(channel),
                ElectrometerMeasureMode.Voltage => MeasureVoltage(channel),
                ElectrometerMeasureMode.Resistance => MeasureResistance(channel),
                _ => double.NaN
            };
        }

        /// <summary>Akım ölçümü sorgular (A).</summary>
        public double MeasureCurrent(int channel)
        {
            if (channel < 1 || channel > 2) return double.NaN;
            string resp = SendQuery(KeysightB2902bConfig.CmdMeasureCurrent(channel));
            return ParseMeasurement(resp);
        }

        /// <summary>Voltaj ölçümü sorgular (V).</summary>
        public double MeasureVoltage(int channel)
        {
            if (channel < 1 || channel > 2) return double.NaN;
            string resp = SendQuery(KeysightB2902bConfig.CmdMeasureVoltage(channel));
            return ParseMeasurement(resp);
        }

        /// <summary>Direnç ölçümü sorgular (Ω).</summary>
        public double MeasureResistance(int channel)
        {
            if (channel < 1 || channel > 2) return double.NaN;
            string resp = SendQuery(KeysightB2902bConfig.CmdMeasureResistance(channel));
            return ParseMeasurement(resp);
        }

        /// <summary>
        /// Belirtilen kanalda Voltaj, Akım ve Direnç değerlerini tek seferde alır.
        /// </summary>
        public (double Voltage, double Current, double Resistance) MeasureAll(int channel)
        {
            double v = MeasureVoltage(channel);
            double i = MeasureCurrent(channel);
            double r = MeasureResistance(channel);
            return (v, i, r);
        }

        /// <summary>
        /// Seçili kanalda anlık Voltaj, Akım ve Direnç snapshot değerlerini döner.
        /// Birincil modu doğrudan okur, diğer değerleri matematiksel model ve kaynak ayarlarıyla türetir.
        /// </summary>
        public (double Voltage, double Current, double Resistance) MeasureSnapshot(
            int channel,
            ElectrometerMeasureMode mode,
            float fallbackVoltage = 0f,
            float fallbackCurrent = 0f)
        {
            if (channel < 1 || channel > 2)
                return (double.NaN, double.NaN, double.NaN);

            double v = fallbackVoltage;
            double i = fallbackCurrent;
            double r = double.NaN;

            switch (mode)
            {
                case ElectrometerMeasureMode.Current:
                    i = MeasureCurrent(channel);
                    if (!double.IsNaN(i) && Math.Abs(i) > 1e-15)
                    {
                        r = Math.Abs(v / i);
                    }
                    else
                    {
                        r = double.PositiveInfinity;
                    }
                    break;

                case ElectrometerMeasureMode.Voltage:
                    v = MeasureVoltage(channel);
                    if (!double.IsNaN(v) && Math.Abs(i) > 1e-15)
                    {
                        r = Math.Abs(v / i);
                    }
                    break;

                case ElectrometerMeasureMode.Resistance:
                    r = MeasureResistance(channel);
                    if (!double.IsNaN(r) && r > 1e-6 && !double.IsInfinity(r))
                    {
                        i = v / r;
                    }
                    break;
            }

            return (v, i, r);
        }

        // ======================================
        // RESET & TEMİZLEME
        // ======================================

        /// <summary>Cihazı fabrika sıfırlamasına döndürür ve hataları temizler.</summary>
        public void Reset()
        {
            SendCommand(KeysightB2902bConfig.CmdReset);
            SendCommand(KeysightB2902bConfig.CmdClearStatus);
        }

        // ======================================
        // DÜŞÜK SEVİYE I/O & PARSING
        // ======================================

        private bool SendCommand(string command)
        {
            lock (_lock)
            {
                try
                {
                    bool ok = false;
                    if (_visa?.IsOpen == true)
                    {
                        ok = _visa.Write(command);
                    }
                    else if (_port?.IsOpen == true)
                    {
                        _port.DiscardInBuffer();
                        _port.WriteLine(command);
                        Thread.Sleep(20);
                        ok = true;
                    }

                    if (!ok)
                    {
                        _consecutiveFailures++;
                        if ((_visa != null && !_visa.IsOpen) || _consecutiveFailures >= 3)
                        {
                            Disconnect();
                            LastError = "Cihaz bağlantısı koptu (komut iletilemedi).";
                        }
                    }
                    else
                    {
                        _consecutiveFailures = 0;
                    }

                    return ok;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Keysight B2902B] Komut gönderme hatası ({command}): {ex.Message}");
                    _consecutiveFailures++;
                    if ((_visa != null && !_visa.IsOpen) || _consecutiveFailures >= 3)
                    {
                        Disconnect();
                        LastError = "Cihaz bağlantısı koptu: " + ex.Message;
                    }
                    return false;
                }
            }
        }

        private string SendQuery(string query)
        {
            lock (_lock)
            {
                try
                {
                    string response = string.Empty;
                    if (_visa?.IsOpen == true)
                    {
                        response = _visa.Query(query);
                    }
                    else if (_port?.IsOpen == true)
                    {
                        _port.DiscardInBuffer();
                        _port.WriteLine(query);
                        string line = _port.ReadLine();
                        response = line?.Trim() ?? string.Empty;
                    }

                    if (string.IsNullOrWhiteSpace(response))
                    {
                        _consecutiveFailures++;
                        // Ölümcül kopma (oturum kapandıysa) veya 3 ardışık başarısızlıkta bağlantıyı kapat
                        if ((_visa != null && !_visa.IsOpen) || _consecutiveFailures >= 3)
                        {
                            Disconnect();
                            LastError = "Cihaz bağlantısı koptu (USB kablosunu kontrol edin).";
                        }
                        return string.Empty;
                    }

                    _consecutiveFailures = 0;
                    return response;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Keysight B2902B] Sorgu hatası ({query}): {ex.Message}");
                    _consecutiveFailures++;
                    if ((_visa != null && !_visa.IsOpen) || _consecutiveFailures >= 3)
                    {
                        Disconnect();
                        LastError = "Cihaz bağlantısı koptu: " + ex.Message;
                    }
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// SCPI yanıtını (ör: "+1.234567E-09" veya virgülle ayrılmış "val,val") parse eder.
        /// </summary>
        private static double ParseMeasurement(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return double.NaN;

            string[] parts = response.Split(',');
            string valueStr = parts[0].Trim();

            // Sonda kalabilecek gereksiz alfa karakterleri kırp
            int endIdx = valueStr.Length;
            while (endIdx > 0 && !char.IsDigit(valueStr[endIdx - 1]) &&
                   valueStr[endIdx - 1] != '+' && valueStr[endIdx - 1] != '-' &&
                   valueStr[endIdx - 1] != '.' && valueStr[endIdx - 1] != 'E' &&
                   valueStr[endIdx - 1] != 'e')
            {
                endIdx--;
            }

            valueStr = valueStr[..endIdx];

            if (double.TryParse(valueStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
            {
                // Keysight overflow değeri genelde 9.91E+37 şeklindedir (aşırı açık devre veya limit)
                if (result > 1e36 || result < -1e36)
                    return double.PositiveInfinity;

                return result;
            }

            return double.NaN;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Disconnect();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
