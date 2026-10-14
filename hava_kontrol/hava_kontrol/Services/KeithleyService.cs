using System.Diagnostics;
using System.IO.Ports;
using hava_kontrol.Models;

namespace hava_kontrol.Services
{
    /// <summary>
    /// Keithley 6517B Electrometer RS-232 SCPI haberleşme servisi.
    /// 
    /// Bağlantı: PC COM port → RS-232 (DB-9, düz kablo) → Keithley 6517B
    /// Protokol: SCPI 1991.0
    /// 
    /// Özellikler:
    ///   - Voltaj / Akım / Yük ölçümü
    ///   - Dahili voltaj kaynağı kontrolü (±1000V)
    ///   - Zero check yönetimi
    ///   - Thread-safe (lock ile korunur)
    /// </summary>
    public class KeithleyService : IDisposable
    {
        private SerialPort? _port;
        private readonly object _lock = new();
        private bool _disposed;

        public bool IsConnected => _port?.IsOpen == true;
        public string ComPort { get; private set; } = KeithleyConfig.DefaultComPort;

        // ======================================
        // BAĞLANTI
        // ======================================

        /// <summary>
        /// Belirtilen COM port üzerinden Keithley 6517B'ye bağlanır.
        /// </summary>
        public bool Connect(string? comPort = null)
        {
            lock (_lock)
            {
                try
                {
                    Disconnect();

                    ComPort = comPort ?? KeithleyConfig.DefaultComPort;

                    _port = new SerialPort(ComPort)
                    {
                        BaudRate = KeithleyConfig.BaudRate,
                        DataBits = KeithleyConfig.DataBits,
                        Parity = Parity.None,
                        StopBits = StopBits.One,
                        Handshake = Handshake.None,
                        ReadTimeout = KeithleyConfig.ReadTimeoutMs,
                        WriteTimeout = KeithleyConfig.WriteTimeoutMs,
                        NewLine = KeithleyConfig.LineTerminator
                    };

                    _port.Open();

                    // Cihazı tanımla
                    string idn = SendQuery(KeithleyConfig.CmdIdentify);
                    Debug.WriteLine($"[Keithley] Bağlandı: {idn}");

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Keithley] Bağlantı hatası: {ex.Message}");
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
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Keithley] Disconnect hatası: {ex.Message}");
                }
            }
        }

        // ======================================
        // ÖLÇÜM FONKSİYONU
        // ======================================

        /// <summary>Ölçüm fonksiyonunu ayarlar (VOLT, CURR, CHAR).</summary>
        public void SetFunction(MeasureFunction func)
        {
            string cmd = func switch
            {
                MeasureFunction.Voltage => KeithleyConfig.CmdSetFuncVoltage,
                MeasureFunction.Current => KeithleyConfig.CmdSetFuncCurrent,
                MeasureFunction.Charge => KeithleyConfig.CmdSetFuncCharge,
                _ => throw new ArgumentOutOfRangeException(nameof(func))
            };

            SendCommand(cmd);
        }

        /// <summary>Aktif fonksiyona göre ölçüm yapar, sonucu döndürür.</summary>
        public double Measure(MeasureFunction func)
        {
            string cmd = func switch
            {
                MeasureFunction.Voltage => KeithleyConfig.CmdMeasureVoltage,
                MeasureFunction.Current => KeithleyConfig.CmdMeasureCurrent,
                MeasureFunction.Charge => KeithleyConfig.CmdMeasureCharge,
                _ => throw new ArgumentOutOfRangeException(nameof(func))
            };

            string response = SendQuery(cmd);
            return ParseMeasurement(response);
        }

        /// <summary>Genel :READ? komutu ile ölçüm okur.</summary>
        public double Read()
        {
            string response = SendQuery(KeithleyConfig.CmdRead);
            return ParseMeasurement(response);
        }

        // ======================================
        // DAHİLİ VOLTAJ KAYNAĞI
        // ======================================

        /// <summary>Dahili voltaj kaynağı değerini ayarlar (V).</summary>
        public void SetVoltageSource(float voltage)
        {
            if (voltage < KeithleyConfig.VoltageSourceMin || voltage > KeithleyConfig.VoltageSourceMax)
                throw new ArgumentOutOfRangeException(nameof(voltage),
                    $"Voltaj {KeithleyConfig.VoltageSourceMin} ile {KeithleyConfig.VoltageSourceMax} V arasında olmalıdır.");

            SendCommand($"{KeithleyConfig.CmdSetVoltage}{voltage:F1}");
        }

        /// <summary>Dahili voltaj kaynağını açar.</summary>
        public void VoltageSourceOn()
        {
            SendCommand(KeithleyConfig.CmdVoltageOn);
        }

        /// <summary>Dahili voltaj kaynağını kapatır.</summary>
        public void VoltageSourceOff()
        {
            SendCommand(KeithleyConfig.CmdVoltageOff);
        }

        /// <summary>Dahili voltaj kaynağı durumunu sorgular.</summary>
        public bool IsVoltageSourceOn()
        {
            string response = SendQuery(KeithleyConfig.CmdGetVoltageState);
            return response.Trim() == "1" || response.Trim().Equals("ON", StringComparison.OrdinalIgnoreCase);
        }

        // ======================================
        // ZERO CHECK
        // ======================================

        public void ZeroCheckOn() => SendCommand(KeithleyConfig.CmdZeroCheckOn);
        public void ZeroCheckOff() => SendCommand(KeithleyConfig.CmdZeroCheckOff);

        // ======================================
        // RESET
        // ======================================

        /// <summary>Cihazı fabrika ayarlarına sıfırlar.</summary>
        public void Reset()
        {
            SendCommand(KeithleyConfig.CmdReset);
            SendCommand(KeithleyConfig.CmdClearStatus);
        }

        // ======================================
        // DÜŞÜK SEVİYE SERİ PORT I/O
        // ======================================

        /// <summary>Komut gönderir (yanıt beklenmez).</summary>
        private void SendCommand(string command)
        {
            lock (_lock)
            {
                if (_port?.IsOpen != true)
                    throw new InvalidOperationException("Keithley bağlı değil.");

                _port.DiscardInBuffer();
                _port.WriteLine(command);
                Debug.WriteLine($"[Keithley] TX: {command}");
            }
        }

        /// <summary>Komut gönderir ve yanıt okur.</summary>
        private string SendQuery(string command)
        {
            lock (_lock)
            {
                if (_port?.IsOpen != true)
                    throw new InvalidOperationException("Keithley bağlı değil.");

                _port.DiscardInBuffer();
                _port.WriteLine(command);
                Debug.WriteLine($"[Keithley] TX: {command}");

                string response = _port.ReadLine().Trim();
                Debug.WriteLine($"[Keithley] RX: {response}");

                return response;
            }
        }

        /// <summary>
        /// Keithley ölçüm yanıtını parse eder.
        /// Format: "+1.234567E+02NVDC" veya sadece sayı.
        /// </summary>
        private static double ParseMeasurement(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return double.NaN;

            // Virgülle ayrılmış veri olabilir (ör: değer, zaman, durum)
            string[] parts = response.Split(',');
            string valueStr = parts[0].Trim();

            // Sayısal olmayan son karakterleri kırp (NVDC, NADC, NC vb.)
            int endIdx = valueStr.Length;
            while (endIdx > 0 && !char.IsDigit(valueStr[endIdx - 1]) &&
                   valueStr[endIdx - 1] != '+' && valueStr[endIdx - 1] != '-' &&
                   valueStr[endIdx - 1] != '.' && valueStr[endIdx - 1] != 'E' &&
                   valueStr[endIdx - 1] != 'e')
            {
                endIdx--;
            }

            valueStr = valueStr[..endIdx];

            if (double.TryParse(valueStr, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }

            return double.NaN;
        }

        // ======================================
        // DISPOSE
        // ======================================

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

    /// <summary>Keithley 6517B ölçüm fonksiyonları.</summary>
    public enum MeasureFunction
    {
        Voltage,
        Current,
        Charge
    }
}
