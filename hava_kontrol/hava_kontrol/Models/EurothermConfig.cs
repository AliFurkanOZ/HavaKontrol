namespace hava_kontrol.Models
{
    /// <summary>
    /// Eurotherm 3200 kontrolcü konfigürasyon sabitleri.
    /// V-OVG cihazındaki 2 adet Eurotherm 3200 kontrolcüyü tanımlar.
    /// 
    /// Kaynak: UIC clone dosyalarından çıkarılmıştır.
    ///   - 91-0023-AAA: Flow Controller (RCXX)
    ///   - 91-0024-AAA: Temperature Controller (RRXX)
    /// 
    /// Bağlantı: PC → USB-RS485 (COM12) → V-OVG RS485 bus
    /// Protokol: Modbus RTU
    /// </summary>
    public static class EurothermConfig
    {
        // ==========================================
        // SERİ PORT AYARLARI
        // ==========================================

        /// <summary>USB-RS485 converter COM port'u.</summary>
        public const string ComPort = "COM12";

        /// <summary>Baud rate (Kullanıcı tarafından 19200 olarak doğrulandı).</summary>
        public const int BaudRate = 19200;

        /// <summary>Data bits (Modbus RTU standart).</summary>
        public const int DataBits = 8;

        /// <summary>Parity (Eurotherm kılavuzu: PRTY register 12549: 0 = None, 1 = Even, 2 = Odd. UIC: 0 → None).</summary>
        public const System.IO.Ports.Parity Parity = System.IO.Ports.Parity.None;

        /// <summary>Stop bits (Modbus RTU standart).</summary>
        public const System.IO.Ports.StopBits StopBits = System.IO.Ports.StopBits.One;

        /// <summary>Modbus RTU okuma timeout (ms).</summary>
        public const int ReadTimeoutMs = 1200;

        /// <summary>Modbus RTU yazma timeout (ms).</summary>
        public const int WriteTimeoutMs = 1500;

        // ==========================================
        // SLAVE ADRESLERİ (4 Adet V-OVG Cihazı)
        // ==========================================

        // V-OVG 1: Sıcaklık = 3, Akış = 4
        public const byte Vovg1TempSlaveId = 3;
        public const byte Vovg1FlowSlaveId = 4;

        // V-OVG 2: Sıcaklık = 5, Akış = 6
        public const byte Vovg2TempSlaveId = 5;
        public const byte Vovg2FlowSlaveId = 6;

        // V-OVG 3: Sıcaklık = 7, Akış = 8
        public const byte Vovg3TempSlaveId = 7;
        public const byte Vovg3FlowSlaveId = 8;

        // V-OVG 4: Sıcaklık = 9, Akış = 10
        public const byte Vovg4TempSlaveId = 9;
        public const byte Vovg4FlowSlaveId = 10;

        // Dizi formatı (1-indexed: [1]=Vovg1, [2]=Vovg2, [3]=Vovg3, [4]=Vovg4)
        public static readonly byte[] TempSlaveIds = { 0, Vovg1TempSlaveId, Vovg2TempSlaveId, Vovg3TempSlaveId, Vovg4TempSlaveId };
        public static readonly byte[] FlowSlaveIds = { 0, Vovg1FlowSlaveId, Vovg2FlowSlaveId, Vovg3FlowSlaveId, Vovg4FlowSlaveId };

        // Geriye dönük uyumluluk için (V-OVG 1)
        public const byte TemperatureSlaveId = Vovg1TempSlaveId;
        public const byte FlowSlaveId = Vovg1FlowSlaveId;

        // ==========================================
        // REGISTER ADRESLERİ (Modbus Holding Registers)
        // ==========================================
        // UIC formatı: <Name>, <Modbus Address>, <Value>
        // FC 03 = Read, FC 06 = Write Single

        /// <summary>Process Variable — anlık ölçüm değeri. Read Only.</summary>
        public const ushort RegPV = 1;

        /// <summary>Manual çıkış değeri (%). Read/Write.</summary>
        public const ushort RegManualOutput = 3;

        /// <summary>Aktif çıkış gücü (%). Read Only.</summary>
        public const ushort RegActiveOutput = 4;

        /// <summary>Çalışma setpoint'i (ramp etkisi dahil). Read Only.</summary>
        public const ushort RegWorkingSetpoint = 5;

        /// <summary>Proportional Band (PID-P). Read/Write.</summary>
        public const ushort RegProportionalBand = 6;

        /// <summary>Integral Time (PID-I). Read/Write.</summary>
        public const ushort RegIntegralTime = 8;

        /// <summary>Derivative Time (PID-D). Read/Write.</summary>
        public const ushort RegDerivativeTime = 9;

        /// <summary>Giriş aralığı alt sınırı. Read Only.</summary>
        public const ushort RegRangeLow = 11;

        /// <summary>Giriş aralığı üst sınırı. Read Only.</summary>
        public const ushort RegRangeHigh = 12;

        /// <summary>Alarm 1 eşiği. Read/Write.</summary>
        public const ushort RegAlarm1Threshold = 13;

        /// <summary>Aktif SP seçimi (0=SP1, 1=SP2). Read/Write.</summary>
        public const ushort RegSPSelect = 15;

        /// <summary>Setpoint 1. Read/Write.</summary>
        public const ushort RegSP1 = 24;

        /// <summary>Setpoint 2. Read/Write.</summary>
        public const ushort RegSP2 = 25;

        /// <summary>Alternatif/Remote setpoint. Read/Write.</summary>
        public const ushort RegAltSP = 26;

        /// <summary>Çıkış üst limiti (%). Read/Write.</summary>
        public const ushort RegOutputHighLimit = 30;

        /// <summary>Çıkış alt limiti (%). Read/Write.</summary>
        public const ushort RegOutputLowLimit = 31;

        /// <summary>Ramp hızı (0=anında). Read/Write.</summary>
        public const ushort RegRate = 35;

        /// <summary>PV-SP hata farkı. Read Only.</summary>
        public const ushort RegError = 39;

        /// <summary>Cihaz durum word'ü. Read Only.</summary>
        public const ushort RegInstStatus = 75;

        /// <summary>Manuel mod (0=Auto, 1=Manual). Read/Write.</summary>
        public const ushort RegManualMode = 85;

        /// <summary>SP üst limiti. Read Only.</summary>
        public const ushort RegSPHighLimit = 111;

        /// <summary>SP alt limiti. Read Only.</summary>
        public const ushort RegSPLowLimit = 112;

        /// <summary>Sensör kırık durumu. Read Only.</summary>
        public const ushort RegSensorBreak = 258;

        /// <summary>Yeni alarm durumu. Read Only.</summary>
        public const ushort RegNewAlarm = 260;

        /// <summary>Autotune enable. Read/Write.</summary>
        public const ushort RegAutotuneEnable = 270;

        /// <summary>Alarm 1 çıkış durumu. Read Only.</summary>
        public const ushort RegAlarm1Out = 294;

        /// <summary>Ondalık basamak sayısı. Read Only.</summary>
        public const ushort RegDecimalPoints = 525;

        // ==========================================
        // FLOW CONTROLLER (RCXX) — Slave 4
        // ==========================================
        // UIC: INPUT.RangeLow=0, INPUT.RangeHigh=250
        //      INPUT.DecimalPoints=0, INPUT.Units=3 (Custom)
        //      SP.SPHighLimit=250, SP.SPLowLimit=0

        /// <summary>Flow setpoint minimum (sccm).</summary>
        public const float FlowMinSetpoint = 0.0f;

        /// <summary>Flow setpoint maximum (sccm).</summary>
        public const float FlowMaxSetpoint = 250.0f;

        /// <summary>
        /// Flow setpoint için PV üzerinden izin verilen maksimum fark (sccm).
        /// Cihazın alarma düşmemesi için setpoint, mevcut PV + bu değeri aşmamalıdır.
        /// Örnek: PV = 100 sccm ise maksimum SP = 138 sccm.
        /// </summary>
        public const float FlowMaxOverPV = 38.0f;

        /// <summary>
        /// Flow ondalık basamak (UIC: DecimalPoints=0).
        /// Modbus'tan gelen değer doğrudan kullanılır.
        /// </summary>
        public const int FlowDecimalPoints = 0;

        // ==========================================
        // TEMPERATURE CONTROLLER (RRXX) — Slave 3
        // ==========================================
        // UIC: INPUT.RangeLow=25, INPUT.RangeHigh=100
        //      INPUT.DecimalPoints=1, INPUT.Units=0 (°C)
        //      SP.SPHighLimit=100, SP.SPLowLimit=25

        /// <summary>Temperature setpoint minimum (°C).</summary>
        public const float TempMinSetpoint = 0.0f;

        /// <summary>Temperature setpoint maximum (°C).</summary>
        public const float TempMaxSetpoint = 100.0f;

        /// <summary>
        /// Temperature ondalık basamak (UIC: DecimalPoints=1).
        /// Modbus'tan gelen değer 10'a bölünmeli.
        /// Yazarken 10 ile çarpılmalı.
        /// Örnek: 992 → 99.2°C, 75.5°C → 755
        /// </summary>
        public const int TempDecimalPoints = 1;

        // ==========================================
        // YARDIMCI METOTLAR
        // ==========================================

        /// <summary>
        /// Modbus'tan okunan ham register değerini gerçek değere çevirir.
        /// Ondalık basamak sayısına göre bölme yapar.
        /// </summary>
        public static float RawToScaled(int rawValue, int decimalPoints)
        {
            if (decimalPoints <= 0) return rawValue;
            float divisor = (float)Math.Pow(10, decimalPoints);
            return rawValue / divisor;
        }

        /// <summary>
        /// Gerçek değeri Modbus'a yazılacak ham register değerine çevirir.
        /// Ondalık basamak sayısına göre çarpma yapar.
        /// </summary>
        public static ushort ScaledToRaw(float value, int decimalPoints)
        {
            if (decimalPoints <= 0) return (ushort)Math.Round(value);
            float multiplier = (float)Math.Pow(10, decimalPoints);
            return (ushort)Math.Round(value * multiplier);
        }
    }
}
