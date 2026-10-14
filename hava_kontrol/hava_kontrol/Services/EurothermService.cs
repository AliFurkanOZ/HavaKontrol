using System.IO.Ports;
using NModbus;
using NModbus.Serial;
using hava_kontrol.Models;

namespace hava_kontrol.Services
{
    /// <summary>
    /// PC → RS485 (COM12) → Eurotherm 3200 kontrolcü Modbus RTU iletişim servisi.
    /// 
    /// V-OVG cihazındaki 2 adet Eurotherm 3200 kontrolcüye (Flow + Temperature)
    /// doğrudan PC'den RS485 üzerinden Modbus RTU ile bağlanır.
    /// PLC'den bağımsızdır — ayrı bir seri port kullanır.
    /// 
    /// Desteklenen Modbus fonksiyonları:
    ///   - FC 03: Read Holding Registers
    ///   - FC 06: Write Single Register
    /// 
    /// Thread-safe olarak tasarlanmıştır (lock mekanizması).
    /// NModbus 3.x kullanır (ModbusFactory + SerialPortAdapter).
    /// </summary>
    public class EurothermService : IDisposable
    {
        private SerialPort? _serialPort;
        private IModbusMaster? _master;
        private readonly ModbusFactory _factory = new();
        private readonly object _lock = new();
        private bool _disposed;

        // ==========================================
        // BAĞLANTI
        // ==========================================

        /// <summary>
        /// RS485 seri port bağlantısını açar ve Modbus RTU master oluşturur.
        /// </summary>
        public bool Connect(string? portOverride = null)
        {
            lock (_lock)
            {
                try
                {
                    Disconnect();

                    string portName = portOverride ?? EurothermConfig.ComPort;

                    _serialPort = new SerialPort
                    {
                        PortName = portName,
                        BaudRate = EurothermConfig.BaudRate,
                        DataBits = EurothermConfig.DataBits,
                        Parity = EurothermConfig.Parity,
                        StopBits = EurothermConfig.StopBits,
                        ReadTimeout = EurothermConfig.ReadTimeoutMs,
                        WriteTimeout = EurothermConfig.WriteTimeoutMs,
                        // RS485 half-duplex: RTS kontrolü
                        Handshake = Handshake.None,
                        RtsEnable = false,
                        DtrEnable = false
                    };

                    _serialPort.Open();
                    _serialPort.DiscardInBuffer();
                    _serialPort.DiscardOutBuffer();

                    var adapter = new SerialPortAdapter(_serialPort);
                    _master = _factory.CreateRtuMaster(adapter);

                    // Modbus RTU transport ayarları
                    _master.Transport.ReadTimeout = EurothermConfig.ReadTimeoutMs;
                    _master.Transport.WriteTimeout = EurothermConfig.WriteTimeoutMs;
                    _master.Transport.Retries = 1;

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"Eurotherm RS485 bağlantı hatası: {ex.Message}");
                    Disconnect();
                    return false;
                }
            }
        }

        /// <summary>
        /// Seri port bağlantısını kapatır.
        /// </summary>
        public void Disconnect()
        {
            lock (_lock)
            {
                _master?.Dispose();
                _master = null;

                if (_serialPort != null)
                {
                    try
                    {
                        if (_serialPort.IsOpen)
                            _serialPort.Close();
                    }
                    catch { /* close hatalarını yut */ }

                    _serialPort.Dispose();
                    _serialPort = null;
                }
            }
        }

        /// <summary>
        /// Bağlantı durumu.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                lock (_lock)
                {
                    return _serialPort?.IsOpen == true && _master != null;
                }
            }
        }

        // ==========================================
        // REGISTER OKUMA (FC 03)
        // ==========================================

        /// <summary>
        /// Tek register okur (FC 03).
        /// </summary>
        public ushort ReadRegister(byte slaveId, ushort address)
        {
            lock (_lock)
            {
                EnsureConnected();
                ushort[] result = _master!.ReadHoldingRegisters(slaveId, address, 1);
                return result[0];
            }
        }

        /// <summary>
        /// Birden fazla ardışık register okur (FC 03).
        /// </summary>
        public ushort[] ReadRegisters(byte slaveId, ushort startAddress, ushort count)
        {
            lock (_lock)
            {
                EnsureConnected();
                return _master!.ReadHoldingRegisters(slaveId, startAddress, count);
            }
        }

        // ==========================================
        // REGISTER YAZMA (FC 06)
        // ==========================================

        /// <summary>
        /// Tek register yazar (FC 06).
        /// </summary>
        public void WriteRegister(byte slaveId, ushort address, ushort value)
        {
            lock (_lock)
            {
                EnsureConnected();
                _master!.WriteSingleRegister(slaveId, address, value);
            }
        }

        // ==========================================
        // YÜKSEK SEVİYE OKUMA
        // ==========================================

        /// <summary>
        /// Process Variable okur ve scaling uygular.
        /// Flow: DecimalPoints=0 → doğrudan değer.
        /// Temperature: DecimalPoints=1 → /10 (örn: 992 → 99.2°C).
        /// </summary>
        public float ReadPV(byte slaveId, int decimalPoints)
        {
            ushort raw = ReadRegister(slaveId, EurothermConfig.RegPV);
            // Eurotherm signed integer kullanır
            short signed = (short)raw;
            return EurothermConfig.RawToScaled(signed, decimalPoints);
        }

        /// <summary>
        /// Çalışma setpoint'i okur (ramp etkisi dahil).
        /// </summary>
        public float ReadWorkingSetpoint(byte slaveId, int decimalPoints)
        {
            ushort raw = ReadRegister(slaveId, EurothermConfig.RegWorkingSetpoint);
            short signed = (short)raw;
            return EurothermConfig.RawToScaled(signed, decimalPoints);
        }

        /// <summary>
        /// Aktif SP1 değerini okur.
        /// </summary>
        public float ReadSetpoint1(byte slaveId, int decimalPoints)
        {
            ushort raw = ReadRegister(slaveId, EurothermConfig.RegSP1);
            short signed = (short)raw;
            return EurothermConfig.RawToScaled(signed, decimalPoints);
        }

        /// <summary>
        /// Aktif çıkış gücünü (%) okur.
        /// </summary>
        public float ReadOutputPower(byte slaveId, int decimalPoints)
        {
            ushort raw = ReadRegister(slaveId, EurothermConfig.RegActiveOutput);
            short signed = (short)raw;
            return EurothermConfig.RawToScaled(signed, decimalPoints);
        }

        /// <summary>
        /// Manuel/Auto mod durumunu okur.
        /// 0 = Auto, 1 = Manual.
        /// </summary>
        public bool IsManualMode(byte slaveId)
        {
            ushort raw = ReadRegister(slaveId, EurothermConfig.RegManualMode);
            return raw != 0;
        }

        /// <summary>
        /// PV-SP hata farkını okur.
        /// </summary>
        public float ReadError(byte slaveId, int decimalPoints)
        {
            ushort raw = ReadRegister(slaveId, EurothermConfig.RegError);
            short signed = (short)raw;
            return EurothermConfig.RawToScaled(signed, decimalPoints);
        }

        /// <summary>
        /// Alarm 1 çıkış durumunu okur.
        /// </summary>
        public bool ReadAlarm1Status(byte slaveId)
        {
            ushort raw = ReadRegister(slaveId, EurothermConfig.RegAlarm1Out);
            return raw != 0;
        }

        /// <summary>
        /// Sensör kırık durumunu okur.
        /// </summary>
        public bool ReadSensorBreak(byte slaveId)
        {
            ushort raw = ReadRegister(slaveId, EurothermConfig.RegSensorBreak);
            return raw != 0;
        }

        // ==========================================
        // YÜKSEK SEVİYE YAZMA
        // ==========================================

        /// <summary>
        /// SP1 setpoint yazar. Scaling uygular.
        /// Flow: değer doğrudan yazılır.
        /// Temperature: ×10 (örn: 75.5°C → 755).
        /// </summary>
        public void WriteSetpoint1(byte slaveId, float value, int decimalPoints)
        {
            ushort raw = EurothermConfig.ScaledToRaw(value, decimalPoints);
            WriteRegister(slaveId, EurothermConfig.RegSP1, raw);
        }

        /// <summary>
        /// Manuel/Auto mod ayarlar.
        /// 0 = Auto, 1 = Manual.
        /// </summary>
        public void SetManualMode(byte slaveId, bool manual)
        {
            WriteRegister(slaveId, EurothermConfig.RegManualMode, (ushort)(manual ? 1 : 0));
        }

        /// <summary>
        /// Manuel çıkış değeri yazar (%).
        /// </summary>
        public void WriteManualOutput(byte slaveId, float percent, int decimalPoints)
        {
            ushort raw = EurothermConfig.ScaledToRaw(percent, decimalPoints);
            WriteRegister(slaveId, EurothermConfig.RegManualOutput, raw);
        }

        // ==========================================
        // YARDIMCI
        // ==========================================

        private void EnsureConnected()
        {
            if (_master == null || _serialPort?.IsOpen != true)
                throw new InvalidOperationException(
                    "Eurotherm RS485 bağlantısı yok. Önce Connect() çağrılmalıdır.");
        }

        // ==========================================
        // DISPOSE
        // ==========================================

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
