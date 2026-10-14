using System.Net.Sockets;
using NModbus;
using hava_kontrol.Models;

namespace hava_kontrol.Services
{
    /// <summary>
    /// PLC Modbus TCP bağlantı, valf kontrol ve MFC proxy servisi.
    /// 
    /// PC artık MFC'lerle doğrudan konuşmaz.
    /// Tüm MFC komutları PLC D register'larına yazılır,
    /// PLC MODRWE ile MFC'lere iletir.
    /// MFC verileri PLC D register'larından okunur.
    /// 
    /// Thread-safe olarak tasarlanmıştır.
    /// NModbus 3.x kullanır (ModbusFactory API).
    /// </summary>
    public class PlcService : IDisposable
    {
        private TcpClient? _tcpClient;
        private IModbusMaster? _master;
        private readonly ModbusFactory _factory = new();
        private readonly object _lock = new();
        private bool _disposed;

        // Heartbeat sayacı — her yazışta artar
        private ushort _heartbeatCounter;

        // ==========================================
        // BAĞLANTI
        // ==========================================

        public bool Connect()
        {
            lock (_lock)
            {
                try
                {
                    Disconnect();

                    _tcpClient = new TcpClient();
                    _tcpClient.ReceiveTimeout = 3000;
                    _tcpClient.SendTimeout = 3000;

                    // Timeout'lu bağlantı (3 saniye) - UI thread'i bloke etmez
                    var connectTask = _tcpClient.ConnectAsync(PlcConfig.PlcIp, PlcConfig.PlcPort);
                    if (!connectTask.Wait(3000))
                    {
                        // Timeout - bağlantı sağlanamadı
                        _tcpClient?.Dispose();
                        _tcpClient = null;
                        _ = connectTask.ContinueWith(t => { _ = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);
                        return false;
                    }

                    _master = _factory.CreateMaster(_tcpClient);

                    return true;
                }
                catch
                {
                    _tcpClient?.Dispose();
                    _tcpClient = null;
                    _master = null;
                    return false;
                }
            }
        }

        public void Disconnect()
        {
            lock (_lock)
            {
                _master?.Dispose();
                _master = null;

                if (_tcpClient != null)
                {
                    try { _tcpClient.Close(); } catch { }
                    _tcpClient.Dispose();
                    _tcpClient = null;
                }
            }
        }

        public bool IsConnected
        {
            get
            {
                lock (_lock)
                {
                    return _tcpClient?.Connected == true && _master != null;
                }
            }
        }

        // ==========================================
        // REGISTER OKUMA / YAZMA
        // ==========================================

        public ushort ReadD(ushort address)
        {
            lock (_lock)
            {
                EnsureConnected();

                ushort[] result = _master!.ReadHoldingRegisters(
                    PlcConfig.PlcSlaveId,
                    address,
                    1);

                return result[0];
            }
        }

        public ushort[] ReadMultipleD(ushort startAddress, ushort count)
        {
            lock (_lock)
            {
                EnsureConnected();

                return _master!.ReadHoldingRegisters(
                    PlcConfig.PlcSlaveId,
                    startAddress,
                    count);
            }
        }

        public void WriteD(ushort address, ushort value)
        {
            lock (_lock)
            {
                EnsureConnected();

                _master!.WriteSingleRegister(
                    PlcConfig.PlcSlaveId,
                    address,
                    value);
            }
        }

        public void WriteMultipleD(ushort startAddress, ushort[] values)
        {
            lock (_lock)
            {
                EnsureConnected();

                _master!.WriteMultipleRegisters(
                    PlcConfig.PlcSlaveId,
                    startAddress,
                    values);
            }
        }

        // ==========================================
        // VALF ADRESLERİ
        // ==========================================

        private static void ValidateValve(int valveNumber)
        {
            if (!PlcConfig.IsValidValveNumber(valveNumber))
                throw new ArgumentOutOfRangeException(
                    nameof(valveNumber),
                    $"Geçersiz valf numarası: {valveNumber}");
        }

        public static ushort ValveCommandAddress(int valveNumber)
        {
            ValidateValve(valveNumber);
            return PlcConfig.GetValveCommandRegister(valveNumber);
        }

        public static ushort ValveStatusAddress(int valveNumber)
        {
            ValidateValve(valveNumber);
            return PlcConfig.GetValveStatusRegister(valveNumber);
        }

        // ==========================================
        // VALF AÇ / KAPAT
        // ==========================================

        public void ValveOn(int valveNumber)
        {
            ushort address = ValveCommandAddress(valveNumber);
            WriteD(address, 1);
        }

        public void ValveOff(int valveNumber)
        {
            ushort address = ValveCommandAddress(valveNumber);
            WriteD(address, 0);
        }

        public ushort ValveCommand(int valveNumber)
        {
            ushort address = ValveCommandAddress(valveNumber);
            return ReadD(address);
        }

        public ushort ValveStatus(int valveNumber)
        {
            ushort address = ValveStatusAddress(valveNumber);
            return ReadD(address);
        }

        // ==========================================
        // GÜÇ ÇIKIŞLARI (POWER ON/OFF - Y0.8 - Y0.11)
        // ==========================================

        private static void ValidatePowerOutput(int outputNumber)
        {
            int min = PlcConfig.PowerOutputStartNumber;
            int max = min + PlcConfig.PowerOutputCount - 1;
            if (outputNumber < min || outputNumber > max)
                throw new ArgumentOutOfRangeException(
                    nameof(outputNumber),
                    $"Geçersiz güç çıkış numarası: {outputNumber} ({min}-{max} olmalı)");
        }

        public static ushort PowerCommandAddress(int outputNumber)
        {
            ValidatePowerOutput(outputNumber);
            return (ushort)(PlcConfig.PowerCommandStart + (outputNumber - PlcConfig.PowerOutputStartNumber));
        }

        public static ushort PowerStatusAddress(int outputNumber)
        {
            ValidatePowerOutput(outputNumber);
            return (ushort)(PlcConfig.PowerStatusStart + (outputNumber - PlcConfig.PowerOutputStartNumber));
        }

        public void PowerOutputOn(int outputNumber)
        {
            ushort address = PowerCommandAddress(outputNumber);
            WriteD(address, 1);
        }

        public void PowerOutputOff(int outputNumber)
        {
            ushort address = PowerCommandAddress(outputNumber);
            WriteD(address, 0);
        }

        public ushort PowerOutputCommand(int outputNumber)
        {
            ushort address = PowerCommandAddress(outputNumber);
            return ReadD(address);
        }

        public ushort PowerOutputStatus(int outputNumber)
        {
            ushort address = PowerStatusAddress(outputNumber);
            return ReadD(address);
        }

        // ==========================================
        // INTERLOCK
        // ==========================================

        public ushort GetInterlock()
        {
            return ReadD(PlcConfig.InterlockRegister);
        }

        // ==========================================
        // GENEL GÜÇ
        // ==========================================

        /// <summary>
        /// PLC D300'e genel güç komutu yazar.
        /// PLC programı bu değere göre D400 durumunu günceller
        /// ve 0 ise tüm MFC flow'larını kapatır.
        /// </summary>
        public void WriteGeneralPower(bool enabled)
        {
            WriteD(PlcConfig.GeneralPowerCommand, (ushort)(enabled ? 1 : 0));
        }

        /// <summary>
        /// PLC D400'den genel güç durumunu okur.
        /// </summary>
        public bool ReadGeneralPowerStatus()
        {
            return ReadD(PlcConfig.GeneralPowerStatus) != 0;
        }

        // ==========================================
        // HEARTBEAT
        // ==========================================

        /// <summary>
        /// PLC D301'e heartbeat sayacı yazar.
        /// PLC watchdog bu değerin değişip değişmediğini izler.
        /// 3000 scan boyunca değişmezse PLC, PC bağlantısını
        /// kopuk sayar ve tüm MFC'leri kapatır.
        /// </summary>
        public void SendHeartbeat()
        {
            unchecked { _heartbeatCounter++; }
            WriteD(PlcConfig.HeartbeatRegister, _heartbeatCounter);
        }

        /// <summary>
        /// D452: PLC tarafından güncellenen watchdog durumu.
        /// 1 = PC bağlı (heartbeat geliyor), 0 = PC koptu.
        /// </summary>
        public bool ReadWatchdogStatus()
        {
            return ReadD(PlcConfig.WatchdogStatus) != 0;
        }

        // ==========================================
        // PLC ALARM
        // ==========================================

        /// <summary>
        /// D453: PLC alarm durumu.
        /// 0 = normal, 1 = alarm (MFC hatası, interlock ihlali, vb.)
        /// </summary>
        public ushort ReadAlarmStatus()
        {
            return ReadD(PlcConfig.AlarmStatus);
        }

        // ==========================================
        // MFC YARDIMCI
        // ==========================================

        private static void ValidateMfc(int mfcNumber)
        {
            if (mfcNumber < 1 || mfcNumber > PlcConfig.MfcCount)
                throw new ArgumentOutOfRangeException(
                    nameof(mfcNumber),
                    "MFC numarası 1, 2, 3 veya 4 olmalıdır.");
        }

        private static ushort MfcCmdAddr(int mfcNumber, int offset)
        {
            return (ushort)(PlcConfig.MfcCommandBase[mfcNumber] + offset);
        }

        private static ushort MfcStatusAddr(int mfcNumber, int offset)
        {
            return (ushort)(PlcConfig.MfcStatusBase[mfcNumber] + offset);
        }

        // ==========================================
        // MFC KOMUTLARI (PC → PLC → MFC)
        // ==========================================

        /// <summary>
        /// MFC setpoint source'u ayarlar. D3x0'a yazar.
        /// 1 = Dijital (Modbus üzerinden setpoint), 0 = Analog.
        /// PLC bu değeri komut doğrulama bölümünde kontrol eder.
        /// </summary>
        public void MfcSetSetpointSource(int mfcNumber, ushort value)
        {
            ValidateMfc(mfcNumber);
            WriteD(MfcCmdAddr(mfcNumber, PlcConfig.MfcOffsetSetpointSource), value);
        }

        /// <summary>
        /// MFC flow ON/OFF komutu. D3x1'e yazar.
        /// PLC bu değeri MFC'nin 40011 register'ına iletir.
        /// </summary>
        public void MfcSetFlowOnOff(int mfcNumber, bool enabled)
        {
            ValidateMfc(mfcNumber);
            WriteD(MfcCmdAddr(mfcNumber, PlcConfig.MfcOffsetFlowOnOff), (ushort)(enabled ? 1 : 0));
        }

        /// <summary>
        /// MFC setpoint yazar. Float değeri HIGH/LOW word'lere çevirir
        /// ve D3x2 + D3x3'e yazar.
        /// PLC bu değerleri MFC'nin 40013-40014 register'larına iletir.
        /// </summary>
        public void MfcWriteSetpoint(int mfcNumber, float sccm)
        {
            ValidateMfc(mfcNumber);

            if (sccm < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(sccm), "Setpoint 0 SCCM'den küçük olamaz.");

            if (sccm > 2000)
                throw new ArgumentOutOfRangeException(
                    nameof(sccm), "Setpoint 2000 SCCM'den büyük olamaz.");

            var (high, low) = FloatConverter.FloatToWords(sccm);

            // D3x2 = HIGH, D3x3 = LOW — tek seferde yaz
            ushort startAddr = MfcCmdAddr(mfcNumber, PlcConfig.MfcOffsetSetpointHigh);
            WriteMultipleD(startAddr, new ushort[] { high, low });
        }

        // ==========================================
        // MFC DURUM OKUMA (MFC → PLC → PC)
        // ==========================================

        /// <summary>
        /// MFC actual flow okur. D4x4 (HIGH) ve D4x5 (LOW) register'larından
        /// IEEE-754 float32 değer oluşturur.
        /// PLC bu değerleri MFC'nin 40001-40002 register'larından okuyarak günceller.
        /// </summary>
        public float MfcReadActualFlow(int mfcNumber)
        {
            ValidateMfc(mfcNumber);

            ushort addrHigh = MfcStatusAddr(mfcNumber, PlcConfig.MfcOffsetActualFlowHigh);
            ushort[] data = ReadMultipleD(addrHigh, 2);

            // Standart byte order: AB CD (high word first)
            float value = FloatConverter.WordsToFloat(data[0], data[1]);

            // Eğer standart order 0 veriyorsa ve ham register'larda veri varsa,
            // word-swap (CD AB) dene — bazı Modbus cihazlar ters sıra kullanır.
            if (value == 0.0f && (data[0] != 0 || data[1] != 0))
            {
                float swapped = FloatConverter.WordsToFloat(data[1], data[0]);
                if (!float.IsNaN(swapped) && !float.IsInfinity(swapped))
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[MFC{mfcNumber}] Word-swap algılandı: " +
                        $"0x{data[0]:X4} 0x{data[1]:X4} → {swapped:F1} SCCM");
                    return swapped;
                }
            }

            return value;
        }

        /// <summary>
        /// MFC iletişim durumunu okur. D4x0.
        /// 0 = MFC ile henüz başarılı iletişim yok, 1 = OK.
        /// </summary>
        public bool MfcReadCommStatus(int mfcNumber)
        {
            ValidateMfc(mfcNumber);
            return ReadD(MfcStatusAddr(mfcNumber, PlcConfig.MfcOffsetCommStatus)) != 0;
        }

        /// <summary>
        /// MFC hata kodunu okur. D4x6.
        /// 0 = hata yok.
        /// 1001 = geçersiz setpoint source değeri
        /// 1002 = geçersiz flow on/off değeri
        /// 1101 = negatif setpoint
        /// 2001 = MFC iletişim yok
        /// </summary>
        public ushort MfcReadErrorCode(int mfcNumber)
        {
            ValidateMfc(mfcNumber);
            return ReadD(MfcStatusAddr(mfcNumber, PlcConfig.MfcOffsetErrorCode));
        }

        /// <summary>
        /// MFC flow ON/OFF durumunu PLC komut register'ından okur. D3x1.
        /// Bu, PC'nin yazdığı son değerdir (PLC'nin MFC'ye ilettiği).
        /// </summary>
        public bool MfcReadFlowOnOff(int mfcNumber)
        {
            ValidateMfc(mfcNumber);
            return ReadD(MfcCmdAddr(mfcNumber, PlcConfig.MfcOffsetFlowOnOff)) != 0;
        }

        /// <summary>
        /// MFC setpoint'i PLC komut register'larından okur. D3x2 + D3x3.
        /// Bu, PC'nin yazdığı son değerdir.
        /// </summary>
        public float MfcReadSetpoint(int mfcNumber)
        {
            ValidateMfc(mfcNumber);

            ushort addrHigh = MfcCmdAddr(mfcNumber, PlcConfig.MfcOffsetSetpointHigh);
            ushort[] data = ReadMultipleD(addrHigh, 2);

            return FloatConverter.WordsToFloat(data[0], data[1]);
        }

        // ==========================================
        // DİAGNOSTİK — State Machine & Ham Register
        // ==========================================

        /// <summary>
        /// D455 (state machine durumu) okur.
        /// 0→10→20→...→170→0 döngüsünde dönüyorsa state machine çalışıyor demektir.
        /// </summary>
        public ushort ReadCommState()
        {
            return ReadD(PlcConfig.CommState);
        }

        /// <summary>
        /// D455'e (state machine) yazarak PLC'deki MODRW komutunu tetikler.
        /// PC kontrollü state cycling: SM101 cascade sorununu bypass eder.
        /// </summary>
        public void WriteCommState(ushort state)
        {
            WriteD(PlcConfig.CommState, state);
        }

        /// <summary>
        /// MFC actual flow register'larının HAM değerlerini okur (D4x4, D4x5).
        /// Float dönüşümü yapmadan, ham ushort olarak döndürür.
        /// MODRW'nin gerçekten veri okuyup okumadığını anlamak için kullanılır.
        /// </summary>
        public (ushort raw0, ushort raw1) MfcReadActualFlowRaw(int mfcNumber)
        {
            ValidateMfc(mfcNumber);
            ushort addrHigh = MfcStatusAddr(mfcNumber, PlcConfig.MfcOffsetActualFlowHigh);
            ushort[] data = ReadMultipleD(addrHigh, 2);
            return (data[0], data[1]);
        }

        // ==========================================
        // V-OVG KONTROL VE DURUM METOTLARI (PLC Üzerinden)
        // ==========================================

        private static void ValidateVovg(int vovgNumber)
        {
            if (vovgNumber < 1 || vovgNumber > PlcConfig.VovgCount)
                throw new ArgumentOutOfRangeException(
                    nameof(vovgNumber),
                    $"Geçersiz V-OVG numarası: {vovgNumber}. (1-{PlcConfig.VovgCount} arası olmalı)");
        }

        /// <summary>
        /// Belirli bir V-OVG cihazının PLC'deki 14 durum register'ını okur.
        /// V-OVG 1: D470..D483
        /// V-OVG 2: D600..D613
        /// V-OVG 3: D620..D633
        /// </summary>
        public ushort[] ReadVovgStatusRegisters(int vovgNumber)
        {
            ValidateVovg(vovgNumber);
            ushort baseAddr = PlcConfig.VovgStatusBase[vovgNumber];
            return ReadMultipleD(baseAddr, PlcConfig.VovgStatusCount);
        }

        /// <summary>
        /// V-OVG 1 durum register'larını okur (geriye dönük uyumluluk).
        /// </summary>
        public ushort[] ReadVovgStatusRegisters()
        {
            return ReadVovgStatusRegisters(1);
        }

        /// <summary>
        /// Belirtilen V-OVG cihazına Sıcaklık setpoint yazar (D350/D360/D370: celsius * 10).
        /// </summary>
        public void WriteVovgTemperatureSetpoint(int vovgNumber, float celsius)
        {
            ValidateVovg(vovgNumber);
            short raw = (short)Math.Round(celsius * 10.0f);
            ushort addr = (ushort)(PlcConfig.VovgCommandBase[vovgNumber] + PlcConfig.VovgOffsetTempCommandSetpoint);
            WriteD(addr, (ushort)raw);
        }

        /// <summary>
        /// V-OVG 1 Sıcaklık setpoint yazar (geriye dönük uyumluluk).
        /// </summary>
        public void WriteVovgTemperatureSetpoint(float celsius)
        {
            WriteVovgTemperatureSetpoint(1, celsius);
        }

        /// <summary>
        /// Belirtilen V-OVG cihazına Akış setpoint yazar (D351/D361/D371: sccm).
        /// </summary>
        public void WriteVovgFlowSetpoint(int vovgNumber, float sccm)
        {
            ValidateVovg(vovgNumber);
            short raw = (short)Math.Round(sccm);
            ushort addr = (ushort)(PlcConfig.VovgCommandBase[vovgNumber] + PlcConfig.VovgOffsetFlowCommandSetpoint);
            WriteD(addr, (ushort)raw);
        }

        /// <summary>
        /// V-OVG 1 Akış setpoint yazar (geriye dönük uyumluluk).
        /// </summary>
        public void WriteVovgFlowSetpoint(float sccm)
        {
            WriteVovgFlowSetpoint(1, sccm);
        }

        // ==========================================
        // YARDIMCI
        // ==========================================

        private void EnsureConnected()
        {
            if (_master == null || _tcpClient?.Connected != true)
                throw new InvalidOperationException(
                    "PLC bağlantısı yok. Önce Connect() çağrılmalıdır.");
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
