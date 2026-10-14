using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using hava_kontrol.Models;

namespace hava_kontrol.Services
{
    /// <summary>
    /// RIGOL DP831 3-kanallı Programlanabilir DC Güç Kaynağı SCPI haberleşme servisi.
    /// RS-232 / Seri Port üzerinden çalışır.
    /// 
    /// Kanallar:
    ///   CH1: 0V - 8V / 0A - 5A (Pozitif)
    ///   CH2: 0V - +30V / 0A - 2A (Pozitif)
    ///   CH3: 0V - -30V / 0A - 2A (Negatif)
    /// 
    /// Thread-safe (lock ile korunur).
    /// </summary>
    public class RigolDp831Service : IDisposable
    {
        private SerialPort? _port;
        private VisaSession? _visa;
        private readonly object _lock = new();
        private bool _disposed;

        public bool IsConnected => (_port?.IsOpen == true) || (_visa?.IsOpen == true);
        public bool IsVisaConnection => _visa?.IsOpen == true;
        public string ConnectionEndpoint { get; private set; } = string.Empty;
        public string ComPort { get; private set; } = RigolDp831Config.DefaultComPort;
        public int BaudRate { get; private set; } = RigolDp831Config.DefaultBaudRate;
        public string DeviceIdentifier { get; private set; } = string.Empty;
        public string LastError { get; private set; } = string.Empty;

        // ======================================
        // BAĞLANTI YÖNETİMİ
        // ======================================

        /// <summary>
        /// Belirtilen COM port veya USB-VISA kaynağı üzerinden Rigol DP831'e bağlanır.
        /// </summary>
        public bool Connect(string? endpoint = null, int baudRate = RigolDp831Config.DefaultBaudRate)
        {
            lock (_lock)
            {
                try
                {
                    LastError = string.Empty;
                    Disconnect();

                    endpoint = string.IsNullOrWhiteSpace(endpoint) ? "USB (Otomatik)" : endpoint.Trim();
                    ConnectionEndpoint = endpoint;

                    if (endpoint.StartsWith("USB", StringComparison.OrdinalIgnoreCase))
                    {
                        // VISA USB-TMC Bağlantısı
                        string visaResource = endpoint;
                        if (endpoint.Equals("USB", StringComparison.OrdinalIgnoreCase) ||
                            endpoint.Contains("Otomatik", StringComparison.OrdinalIgnoreCase))
                        {
                            var usbList = VisaSession.FindUsbInstruments();
                            if (usbList.Count == 0)
                            {
                                LastError = "Bağlı USB DP831 cihazı bulunamadı. Lütfen USB kablosunu kontrol edin.";
                                return false;
                            }
                            // 0x1AB1 = Rigol Technologies Vendor ID
                            visaResource = usbList.FirstOrDefault(u => u.Contains("0x1AB1", StringComparison.OrdinalIgnoreCase) || u.Contains("DP8", StringComparison.OrdinalIgnoreCase)) ?? usbList[0];
                        }

                        _visa = new VisaSession();
                        if (!_visa.Open(visaResource, RigolDp831Config.ReadTimeoutMs))
                        {
                            LastError = $"VISA üzerinden bağlanılamadı ({visaResource}).";
                            Disconnect();
                            return false;
                        }

                        DeviceIdentifier = SendQuery(RigolDp831Config.CmdIdentify);
                        if (string.IsNullOrWhiteSpace(DeviceIdentifier) ||
                            (!DeviceIdentifier.Contains("RIGOL", StringComparison.OrdinalIgnoreCase) &&
                             !DeviceIdentifier.Contains("DP8", StringComparison.OrdinalIgnoreCase)))
                        {
                            LastError = string.IsNullOrWhiteSpace(DeviceIdentifier)
                                ? "Cihazdan kimlik yanıtı alınamadı (*IDN? zaman aşımı)."
                                : $"Bilinmeyen cihaz yanıtı: {DeviceIdentifier}";
                            Disconnect();
                            return false;
                        }

                        // Uzaktan kontrol modunu aktif et
                        SendCommand(":SYSTem:REMote");
                        Debug.WriteLine($"[DP831 USB-VISA] Başarıyla bağlandı: {DeviceIdentifier} ({visaResource})");
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
                            DataBits = RigolDp831Config.DataBits,
                            Parity = Parity.None,
                            StopBits = StopBits.One,
                            Handshake = Handshake.None,
                            DtrEnable = true,
                            RtsEnable = true,
                            ReadTimeout = RigolDp831Config.ReadTimeoutMs,
                            WriteTimeout = RigolDp831Config.WriteTimeoutMs,
                            NewLine = RigolDp831Config.LineTerminator
                        };

                        _port.Open();

                        // Cihazı tanımla (*IDN?)
                        DeviceIdentifier = SendQuery(RigolDp831Config.CmdIdentify);
                        if (string.IsNullOrWhiteSpace(DeviceIdentifier) ||
                            (!DeviceIdentifier.Contains("RIGOL", StringComparison.OrdinalIgnoreCase) &&
                             !DeviceIdentifier.Contains("DP8", StringComparison.OrdinalIgnoreCase)))
                        {
                            LastError = string.IsNullOrWhiteSpace(DeviceIdentifier)
                                ? "Seri port açıldı fakat cihazdan yanıt alınamadı (*IDN? zaman aşımı). Baud rate ve kabloyu kontrol edin."
                                : $"Bilinmeyen cihaz yanıtı: {DeviceIdentifier}";
                            Disconnect();
                            return false;
                        }

                        // Uzaktan kontrol modunu aktif et
                        SendCommand(":SYSTem:REMote");
                        Debug.WriteLine($"[DP831 RS232] Başarıyla bağlandı: {DeviceIdentifier}");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    Debug.WriteLine($"[DP831] Bağlantı hatası: {ex.Message}");
                    Disconnect();
                    return false;
                }
            }
        }

        /// <summary>Bağlantıyı kapatır.</summary>
        public void Disconnect()
        {
            lock (_lock)
            {
                try
                {
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
                    Debug.WriteLine($"[DP831] Disconnect hatası: {ex.Message}");
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
            if (channel < 1 || channel > 3) return false;
            return SendCommand(RigolDp831Config.CmdSetOutput(channel, on));
        }

        /// <summary>
        /// Belirtilen kanalın çıkış durumunu sorgular.
        /// </summary>
        public bool GetChannelOutput(int channel)
        {
            if (channel < 1 || channel > 3) return false;
            string resp = SendQuery(RigolDp831Config.CmdGetOutput(channel)).Trim().ToUpperInvariant();
            return resp == "ON" || resp == "1" || resp.StartsWith("ON");
        }

        /// <summary>
        /// Tüm kanalların (CH1, CH2, CH3) çıkışlarını açar veya kapatır.
        /// </summary>
        public bool SetAllOutputs(bool on)
        {
            bool ok = true;
            for (int ch = 1; ch <= 3; ch++)
            {
                ok &= SetChannelOutput(ch, on);
            }
            return ok;
        }

        // ======================================
        // VOLTAJ & AKIM AYARLAMA
        // ======================================

        private int _consecutiveReadFailures = 0;

        /// <summary>
        /// Belirtilen kanalın voltajını ayarlar.
        /// CH1: 0 - 8.4V
        /// CH2: 0 - 32V
        /// CH3: -32V - 0V (Pozitif girilirse otomatik negatife çevrilir)
        /// </summary>
        public bool SetVoltage(int channel, float voltage)
        {
            if (channel == 3 && voltage > 0)
            {
                voltage = -voltage;
            }
            voltage = ClampVoltage(channel, voltage);
            return SendCommand(RigolDp831Config.CmdSetVoltage(channel, voltage));
        }

        /// <summary>
        /// Belirtilen kanalın akımını ayarlar.
        /// CH1: 0 - 5.3A
        /// CH2: 0 - 2.1A
        /// CH3: 0 - 2.1A
        /// </summary>
        public bool SetCurrent(int channel, float current)
        {
            current = ClampCurrent(channel, current);
            return SendCommand(RigolDp831Config.CmdSetCurrent(channel, current));
        }

        /// <summary>
        /// Belirtilen kanalın voltaj ve akımını tek seferde ayarlar (:APPLy).
        /// </summary>
        public bool Apply(int channel, float voltage, float current)
        {
            if (channel == 3 && voltage > 0)
            {
                voltage = -voltage;
            }
            voltage = ClampVoltage(channel, voltage);
            current = ClampCurrent(channel, current);
            return SendCommand(RigolDp831Config.CmdApply(channel, voltage, current));
        }

        /// <summary>
        /// Belirtilen kanalın ayarlı voltaj değerini okur.
        /// </summary>
        public float GetSetVoltage(int channel)
        {
            string resp = SendQuery(RigolDp831Config.CmdGetVoltage(channel));
            if (float.TryParse(resp, NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
                return val;
            return 0f;
        }

        /// <summary>
        /// Belirtilen kanalın ayarlı akım değerini okur.
        /// </summary>
        public float GetSetCurrent(int channel)
        {
            string resp = SendQuery(RigolDp831Config.CmdGetCurrent(channel));
            if (float.TryParse(resp, NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
                return val;
            return 0f;
        }

        /// <summary>
        /// Kanalın cihaz üzerindeki ayarlı voltaj ve akım değerlerini sorgular (:APPLy? CH{ch}).
        /// </summary>
        public (float Voltage, float Current)? GetChannelSettings(int channel)
        {
            string resp = SendQuery(RigolDp831Config.CmdApplyQuery(channel));
            if (!string.IsNullOrEmpty(resp))
            {
                // Format: "CH1:8V/5A,0.000,1.000" veya CH1:8V/5A,0.000,1.000
                string clean = resp.Trim('\"', '\'', ' ', '\r', '\n');
                string[] parts = clean.Split(',');
                if (parts.Length >= 3)
                {
                    string vStr = parts[1].Trim('\"', '\'', ' ');
                    string iStr = parts[2].Trim('\"', '\'', ' ');
                    if (float.TryParse(vStr, NumberStyles.Float, CultureInfo.InvariantCulture, out float v) &&
                        float.TryParse(iStr, NumberStyles.Float, CultureInfo.InvariantCulture, out float i))
                    {
                        return (v, i);
                    }
                }
            }
            return null;
        }

        // ======================================
        // ÖLÇÜMLER (MEASURE)
        // ======================================

        /// <summary>
        /// Belirtilen kanalın gerçek voltaj, akım ve güç değerlerini okur.
        /// SCPI: :MEASure:ALL? CH{ch} -> "volt,curr,pwr"
        /// </summary>
        public (float Voltage, float Current, float Power) MeasureChannel(int channel)
        {
            string resp = SendQuery(RigolDp831Config.CmdMeasureAll(channel));
            if (!string.IsNullOrEmpty(resp))
            {
                string clean = resp.Trim('\"', '\'', ' ', '\r', '\n');
                string[] parts = clean.Split(',');
                if (parts.Length >= 3)
                {
                    float.TryParse(parts[0].Trim('\"', '\'', ' '), NumberStyles.Float, CultureInfo.InvariantCulture, out float v);
                    float.TryParse(parts[1].Trim('\"', '\'', ' '), NumberStyles.Float, CultureInfo.InvariantCulture, out float i);
                    float.TryParse(parts[2].Trim('\"', '\'', ' '), NumberStyles.Float, CultureInfo.InvariantCulture, out float p);

                    // RIGOL DP800 Firmware Underflow Düzeltmesi:
                    // DP831 donanım yazılımında, devre açıkken veya akım 0 civarında negatif gürültü aldığında
                    // dahili uint32_t(-1)/10000 underflow hatası sebebiyle akım ve güç 429496.719 döner.
                    // Kanal maksimum akımının çok üzerindeki (> 6A) veya negatif değerler 0.0A kabul edilir.
                    float maxAllowedCurrent = channel == 1
                        ? RigolDp831Config.Ch1MaxCurrent + 0.5f
                        : RigolDp831Config.Ch2MaxCurrent + 0.5f;

                    if (float.IsNaN(i) || float.IsInfinity(i) || i > maxAllowedCurrent || i < 0f)
                    {
                        i = 0f;
                    }

                    // Güç değeri de aynı underflow'dan etkilenir veya P = |V * I| olarak hesaplanır
                    float maxAllowedPower = channel == 1 ? 55.0f : 75.0f;
                    if (float.IsNaN(p) || float.IsInfinity(p) || p > maxAllowedPower || p < 0f)
                    {
                        p = Math.Abs(v * i);
                    }

                    // Voltaj kontrolü
                    float maxAllowedVoltage = channel == 1
                        ? RigolDp831Config.Ch1MaxVoltage + 1.0f
                        : 35.0f;

                    if (float.IsNaN(v) || float.IsInfinity(v) || Math.Abs(v) > maxAllowedVoltage)
                    {
                        v = 0f;
                    }

                    return (v, i, p);
                }
            }
            return (0f, 0f, 0f);
        }

        /// <summary>
        /// 3 kanalın tamamının durumunu tek bir çağrıda okur.
        /// Cihazla iletişim koptuğunda null döner.
        /// </summary>
        public Dp831ChannelStatus[]? ReadAllChannels()
        {
            if (!IsConnected) return null;

            var results = new Dp831ChannelStatus[3];
            bool anyResponseReceived = false;

            for (int ch = 1; ch <= 3; ch++)
            {
                string outResp = SendQuery(RigolDp831Config.CmdGetOutput(ch));
                if (!string.IsNullOrEmpty(outResp))
                {
                    anyResponseReceived = true;
                }

                var meas = MeasureChannel(ch);
                string cleanOut = outResp.Trim('\"', '\'', ' ', '\r', '\n').ToUpperInvariant();
                bool isOutOn = cleanOut == "ON" || cleanOut == "1" || cleanOut.StartsWith("ON");

                results[ch - 1] = new Dp831ChannelStatus
                {
                    ChannelNumber = ch,
                    IsOutputOn = isOutOn,
                    VoltageActual = meas.Voltage,
                    CurrentActual = meas.Current,
                    PowerActual = meas.Power
                };
            }

            if (!anyResponseReceived)
            {
                _consecutiveReadFailures++;
                if (_consecutiveReadFailures >= 3)
                {
                    LastError = "Cihazla iletişim koptu (zaman aşımı).";
                    Disconnect();
                    return null;
                }
            }
            else
            {
                _consecutiveReadFailures = 0;
            }

            return results;
        }

        // ======================================
        // YARDIMCI METOTLAR
        // ======================================

        private static float ClampVoltage(int channel, float voltage)
        {
            return channel switch
            {
                1 => Math.Clamp(voltage, RigolDp831Config.Ch1MinVoltage, RigolDp831Config.Ch1MaxVoltage),
                2 => Math.Clamp(voltage, RigolDp831Config.Ch2MinVoltage, RigolDp831Config.Ch2MaxVoltage),
                3 => Math.Clamp(voltage, RigolDp831Config.Ch3MinVoltage, RigolDp831Config.Ch3MaxVoltage),
                _ => voltage
            };
        }

        private static float ClampCurrent(int channel, float current)
        {
            return channel switch
            {
                1 => Math.Clamp(current, RigolDp831Config.Ch1MinCurrent, RigolDp831Config.Ch1MaxCurrent),
                2 => Math.Clamp(current, RigolDp831Config.Ch2MinCurrent, RigolDp831Config.Ch2MaxCurrent),
                3 => Math.Clamp(current, RigolDp831Config.Ch3MinCurrent, RigolDp831Config.Ch3MaxCurrent),
                _ => current
            };
        }

        private bool SendCommand(string command)
        {
            lock (_lock)
            {
                try
                {
                    if (_visa?.IsOpen == true)
                    {
                        return _visa.Write(command);
                    }

                    if (_port?.IsOpen == true)
                    {
                        _port.DiscardInBuffer();
                        _port.WriteLine(command);
                        Thread.Sleep(30);
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[DP831] Komut gönderme hatası ({command}): {ex.Message}");
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
                    if (_visa?.IsOpen == true)
                    {
                        return _visa.Query(query);
                    }

                    if (_port?.IsOpen == true)
                    {
                        _port.DiscardInBuffer();
                        _port.WriteLine(query);
                        string response = _port.ReadLine();
                        return response.Trim();
                    }

                    return string.Empty;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[DP831] Sorgu hatası ({query}): {ex.Message}");
                    return string.Empty;
                }
            }
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

    /// <summary>
    /// DP831 tek kanal durum verisi.
    /// </summary>
    public class Dp831ChannelStatus
    {
        public int ChannelNumber { get; set; }
        public bool IsOutputOn { get; set; }
        public float VoltageActual { get; set; }
        public float CurrentActual { get; set; }
        public float PowerActual { get; set; }
    }
}
