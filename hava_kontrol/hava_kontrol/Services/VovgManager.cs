using hava_kontrol.Models;

namespace hava_kontrol.Services
{
    /// <summary>
    /// V-OVG üst seviye yönetim katmanı — PLC Modbus TCP üzerinden.
    /// 4 Adet V-OVG cihazını (toplam 8 adet Eurotherm kontrolcü) yönetir.
    /// 
    /// Mimari:
    ///   PC → Modbus TCP (192.168.1.5:502) → PLC (Delta AS228P)
    ///                                         └── COM1 RS-485 (19200, 8-N-1)
    ///                                               ├── V-OVG 1: Eurotherm #3 (Temp) + #4 (Flow)
    ///                                               ├── V-OVG 2: Eurotherm #5 (Temp) + #6 (Flow)
    ///                                               ├── V-OVG 3: Eurotherm #7 (Temp) + #8 (Flow)
    ///                                               └── V-OVG 4: Eurotherm #9 (Temp) + #10 (Flow)
    /// 
    /// PC doğrudan seri porta bağlanmaz.
    /// Tüm V-OVG komut ve durumları PLC D-register'ları üzerinden aktarılır.
    /// MfcManager pattern'i ile birebir tutarlıdır.
    /// </summary>
    public class VovgManager : IDisposable
    {
        private readonly PlcService _plcService;
        private bool _disposed;

        public VovgManager(PlcService plcService)
        {
            _plcService = plcService ?? throw new ArgumentNullException(nameof(plcService));
        }

        // ============================================================
        // BAĞLANTI (PlcService üzerinden)
        // ============================================================

        /// <summary>
        /// V-OVG bağlantısı PLC bağlantısına bağlıdır.
        /// </summary>
        public bool IsConnected => _plcService.IsConnected;

        public bool Connect()
        {
            return _plcService.IsConnected;
        }

        public void Disconnect()
        {
            // PLC bağlantısı MainViewModel tarafından yönetilir
        }

        // ============================================================
        // FLOW CONTROLLER (RCXX — Slave 4, 6, 8, 10)
        // ============================================================

        /// <summary>
        /// Belirli bir V-OVG cihazına Flow setpoint yazar (sccm).
        /// 0-250 sccm sınır kontrolü yapar.
        /// </summary>
        public void WriteFlowSetpoint(int vovgNumber, float sccm)
        {
            ValidateVovgNumber(vovgNumber);

            if (sccm < EurothermConfig.FlowMinSetpoint)
                throw new ArgumentOutOfRangeException(
                    nameof(sccm),
                    $"Flow setpoint {EurothermConfig.FlowMinSetpoint} sccm'den küçük olamaz.");

            if (sccm > EurothermConfig.FlowMaxSetpoint)
                throw new ArgumentOutOfRangeException(
                    nameof(sccm),
                    $"Flow setpoint {EurothermConfig.FlowMaxSetpoint} sccm'den büyük olamaz.");

            _plcService.WriteVovgFlowSetpoint(vovgNumber, sccm);
        }

        /// <summary>
        /// V-OVG 1 için Flow setpoint yazar (geriye dönük uyumluluk).
        /// </summary>
        public void WriteFlowSetpoint(float sccm)
        {
            WriteFlowSetpoint(1, sccm);
        }

        // ============================================================
        // TEMPERATURE CONTROLLER (RRXX — Slave 3, 5, 7, 9)
        // ============================================================

        /// <summary>
        /// Belirli bir V-OVG cihazına Temperature setpoint yazar (°C).
        /// 0-100 °C sınır kontrolü yapar.
        /// </summary>
        public void WriteTemperatureSetpoint(int vovgNumber, float celsius)
        {
            ValidateVovgNumber(vovgNumber);

            if (celsius < EurothermConfig.TempMinSetpoint)
                throw new ArgumentOutOfRangeException(
                    nameof(celsius),
                    $"Sıcaklık setpoint {EurothermConfig.TempMinSetpoint}°C'den küçük olamaz.");

            if (celsius > EurothermConfig.TempMaxSetpoint)
                throw new ArgumentOutOfRangeException(
                    nameof(celsius),
                    $"Sıcaklık setpoint {EurothermConfig.TempMaxSetpoint}°C'den büyük olamaz.");

            _plcService.WriteVovgTemperatureSetpoint(vovgNumber, celsius);
        }

        /// <summary>
        /// V-OVG 1 için Temperature setpoint yazar (geriye dönük uyumluluk).
        /// </summary>
        public void WriteTemperatureSetpoint(float celsius)
        {
            WriteTemperatureSetpoint(1, celsius);
        }

        // ============================================================
        // DURUM OKUMA (PLC D-Register'ları)
        // ============================================================

        /// <summary>
        /// Belirli bir V-OVG cihazının durum register'larını PLC'den okur.
        /// </summary>
        public Dictionary<string, object> ReadStatus(int vovgNumber)
        {
            ValidateVovgNumber(vovgNumber);

            var status = new Dictionary<string, object>();

            if (!_plcService.IsConnected)
            {
                status["flow_online"] = false;
                status["temp_online"] = false;
                return status;
            }

            try
            {
                ushort[] regs = _plcService.ReadVovgStatusRegisters(vovgNumber);

                // --- SICAKLIK KONTROLCÜ (RRXX) ---
                short tempPvRaw = (short)regs[PlcConfig.VovgOffsetTempActualPV];       // PV (x10)
                short tempOpRaw = (short)regs[PlcConfig.VovgOffsetTempOutputPower];     // Output Power (% x10)
                short tempWspRaw = (short)regs[PlcConfig.VovgOffsetTempWorkingSP];     // Working SP (x10)
                ushort tempComm = regs[PlcConfig.VovgOffsetTempCommStatus];            // İletişim (1=OK)
                ushort tempAlarm = regs[PlcConfig.VovgOffsetTempAlarmStatus];          // Alarm (1=Alarm)

                status["temp_online"] = tempComm != 0;
                status["temp_pv"] = tempPvRaw / 10.0f;
                status["temp_sp"] = tempWspRaw / 10.0f;
                status["temp_output"] = tempOpRaw / 10.0f;
                status["temp_alarm"] = tempAlarm != 0;

                // --- AKIŞ KONTROLCÜ (RCXX) ---
                short flowPvRaw = (short)regs[PlcConfig.VovgOffsetFlowActualPV];       // PV (sccm)
                short flowOpRaw = (short)regs[PlcConfig.VovgOffsetFlowOutputPower];     // Output Power (% x10)
                short flowWspRaw = (short)regs[PlcConfig.VovgOffsetFlowWorkingSP];     // Working SP (sccm)
                ushort flowComm = regs[PlcConfig.VovgOffsetFlowCommStatus];            // İletişim (1=OK)
                ushort flowAlarm = regs[PlcConfig.VovgOffsetFlowAlarmStatus];          // Alarm (1=Alarm)

                status["flow_online"] = flowComm != 0;
                status["flow_pv"] = (float)flowPvRaw;
                status["flow_sp"] = (float)flowWspRaw;
                status["flow_output"] = flowOpRaw / 10.0f;
                status["flow_alarm"] = flowAlarm != 0;
            }
            catch (Exception ex)
            {
                status["flow_online"] = false;
                status["temp_online"] = false;
                status["error"] = ex.Message;
                System.Diagnostics.Debug.WriteLine($"V-OVG {vovgNumber} PLC durum okuma hatası: {ex.Message}");
            }

            return status;
        }

        /// <summary>
        /// Tüm V-OVG cihazlarının (1..4) durum register'larını okur.
        /// MfcManager.ReadAllStatus pattern'i ile birebir tutarlıdır.
        /// </summary>
        public Dictionary<int, Dictionary<string, object>> ReadAllDevicesStatus()
        {
            var allStatus = new Dictionary<int, Dictionary<string, object>>();

            for (int i = 1; i <= PlcConfig.VovgCount; i++)
            {
                allStatus[i] = ReadStatus(i);
            }

            return allStatus;
        }

        /// <summary>
        /// V-OVG 1 durumunu okur (geriye dönük uyumluluk).
        /// </summary>
        public Dictionary<string, object> ReadAllStatus()
        {
            return ReadStatus(1);
        }

        // ============================================================
        // YARDIMCI
        // ============================================================

        private static void ValidateVovgNumber(int vovgNumber)
        {
            if (vovgNumber < 1 || vovgNumber > PlcConfig.VovgCount)
                throw new ArgumentOutOfRangeException(
                    nameof(vovgNumber),
                    $"V-OVG numarası 1 ile {PlcConfig.VovgCount} arasında olmalıdır (Gelen: {vovgNumber}).");
        }

        // ============================================================
        // DISPOSE
        // ============================================================

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
