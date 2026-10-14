namespace hava_kontrol.Models
{
    /// <summary>
    /// PLC Modbus TCP konfigürasyon sabitleri.
    /// Python config.py karşılığı.
    /// 
    /// Tüm register adresleri main_program.st ile birebir eşleşir.
    /// Delta AS228P PLC, D register'ları = Modbus Holding Register adresleri.
    /// </summary>
    public static class PlcConfig
    {
        // ==========================================
        // HAVA KONTROL - PLC CONFIG
        // ==========================================

        public const string PlcIp = "192.168.1.5";
        public const int PlcPort = 502;

        // Modbus TCP Unit ID
        public const byte PlcSlaveId = 1;

        // Valf sayısı (Valf 1 - 8: Y0.0 - Y0.7)
        public const int ValveCount = 8;

        // V-OVG Egzoz / Tahliye Valfleri (VALF 13, 14, 15, 16)
        public static readonly int[] AuxValveNumbers = { 13, 14, 15, 16 };

        // MFC sayısı
        public const int MfcCount = 4;

        // V-OVG sayısı
        public const int VovgCount = 4;

        // ==========================================
        // VALF REGISTER'LARI (Valf 1-8 ve 13-16)
        // ==========================================

        // D100-D107: Valf 1-8 Komut (0=KAPAT, 1=AÇ)
        public const ushort ValveCommandStart = 100;
        // D200-D207: Valf 1-8 Durum
        public const ushort ValveStatusStart = 200;

        // D112-D115: Valf 13-16 Komut (0=KAPAT, 1=AÇ) -> Formül: 100 + valveNumber - 1
        // D212-D215: Valf 13-16 Durum (0=KAPALI, 1=AÇIK) -> Formül: 200 + valveNumber - 1

        public static bool IsValidValveNumber(int valveNumber)
        {
            return (valveNumber >= 1 && valveNumber <= ValveCount) ||
                   (valveNumber >= 13 && valveNumber <= 16);
        }

        public static ushort GetValveCommandRegister(int valveNumber)
        {
            if (IsValidValveNumber(valveNumber))
                return (ushort)(ValveCommandStart + valveNumber - 1);
            throw new ArgumentOutOfRangeException(nameof(valveNumber), $"Geçersiz valf numarası: {valveNumber}");
        }

        public static ushort GetValveStatusRegister(int valveNumber)
        {
            if (IsValidValveNumber(valveNumber))
                return (ushort)(ValveStatusStart + valveNumber - 1);
            throw new ArgumentOutOfRangeException(nameof(valveNumber), $"Geçersiz valf numarası: {valveNumber}");
        }

        // ==========================================
        // GÜÇ ÇIKIŞLARI (POWER ON/OFF: Y0.8 - Y0.11)
        // ==========================================

        // Güç çıkış sayısı (Çıkış 8, 9, 10, 11)
        public const int PowerOutputCount = 4;
        public const int PowerOutputStartNumber = 8;
        // D108-D111: Güç Komut (0=KAPAT/OFF, 1=AÇ/ON)
        public const ushort PowerCommandStart = 108;
        // D208-D211: Güç Durum
        public const ushort PowerStatusStart = 208;

        // ==========================================
        // GENEL KONTROL REGISTER'LARI
        // ==========================================

        // D10: Interlock register
        public const ushort InterlockRegister = 10;

        // D300: Genel Güç Komutu (0=OFF, 1=ON)
        public const ushort GeneralPowerCommand = 300;
        // D301: PC Heartbeat counter
        public const ushort HeartbeatRegister = 301;

        // ==========================================
        // GENEL DURUM REGISTER'LARI
        // ==========================================

        // D400: Genel Güç Durumu
        public const ushort GeneralPowerStatus = 400;
        // D450: Çevrim tamamlandı flag
        public const ushort CycleCompleteFlag = 450;
        // D451: Tüm MFC'ler iletişimde flag
        public const ushort AllMfcCommOk = 451;
        // D452: PC bağlı (watchdog status, 1=bağlı, 0=koptu)
        public const ushort WatchdogStatus = 452;
        // D453: Alarm durumu
        public const ushort AlarmStatus = 453;
        // D455: Haberleşme state machine durumu
        public const ushort CommState = 455;

        // ==========================================
        // MFC KOMUT REGISTER'LARI
        // ==========================================
        //
        // Her MFC için:
        //   Base+0 : Setpoint Source (0/1) — PLC validation flag
        //   Base+1 : Flow ON/OFF (0/1)
        //   Base+2 : Setpoint HIGH word (IEEE-754 float32)
        //   Base+3 : Setpoint LOW word
        //
        // MFC1: D310-D313  (Slave 1)
        // MFC2: D320-D323  (Slave 2)
        // MFC3: D330-D333  (Slave 3)
        // MFC4: D340-D343  (Slave 6)

        public static readonly ushort[] MfcCommandBase = { 0, 310, 320, 330, 340 }; // [0] unused, [1]=MFC1, ...

        // Komut offset'leri (base'e göre)
        public const int MfcOffsetSetpointSource = 0;   // D3x0
        public const int MfcOffsetFlowOnOff = 1;         // D3x1
        public const int MfcOffsetSetpointHigh = 2;      // D3x2
        public const int MfcOffsetSetpointLow = 3;       // D3x3

        // ==========================================
        // MFC DURUM REGISTER'LARI
        // ==========================================
        //
        // Her MFC için:
        //   Base+0 : İletişim durumu (0=yok, 1=OK)
        //   Base+4 : Actual Flow HIGH word
        //   Base+5 : Actual Flow LOW word
        //   Base+6 : Hata kodu
        //
        // MFC1: D410, D414-D415, D416
        // MFC2: D420, D424-D425, D426
        // MFC3: D430, D434-D435, D436
        // MFC4: D440, D444-D445, D446

        public static readonly ushort[] MfcStatusBase = { 0, 410, 420, 430, 440 }; // [0] unused

        // Durum offset'leri (base'e göre)
        public const int MfcOffsetCommStatus = 0;        // D4x0
        public const int MfcOffsetActualFlowHigh = 4;    // D4x4
        public const int MfcOffsetActualFlowLow = 5;     // D4x5
        public const int MfcOffsetErrorCode = 6;         // D4x6
        // ==========================================
        // V-OVG REGISTER'LARI (PLC COM1 - Eurotherm)
        // ==========================================
        //
        // 4 Adet V-OVG Cihazı:
        //   V-OVG 1: Temp Slave 3, Flow Slave 4
        //   V-OVG 2: Temp Slave 5, Flow Slave 6
        //   V-OVG 3: Temp Slave 7, Flow Slave 8
        //   V-OVG 4: Temp Slave 9, Flow Slave 10
        //
        // Komut Register'ları (PC yazar, PLC okur)
        //   V-OVG 1: D350 (Temp SP), D351 (Flow SP), D352 (Güç)
        //   V-OVG 2: D360 (Temp SP), D361 (Flow SP), D362 (Güç)
        //   V-OVG 3: D370 (Temp SP), D371 (Flow SP), D372 (Güç)
        //   V-OVG 4: D380 (Temp SP), D381 (Flow SP), D382 (Güç)
        public static readonly ushort[] VovgCommandBase = { 0, 350, 360, 370, 380 }; // [0] unused, [1]=V-OVG 1...

        public const int VovgOffsetTempCommandSetpoint = 0; // Base+0: Sıcaklık SP (x10)
        public const int VovgOffsetFlowCommandSetpoint = 1; // Base+1: Akış SP (sccm)
        public const int VovgOffsetPowerCommand = 2;        // Base+2: Güç/Mod (0=Kapalı, 1=Açık)

        // Geriye dönük uyumluluk sabitleri (V-OVG 1)
        public const ushort VovgTempCommandSetpoint = 350;
        public const ushort VovgFlowCommandSetpoint = 351;
        public const ushort VovgPowerCommand = 352;

        // Durum Register'ları (PLC yazar, PC okur) - Her cihaz için 14 register
        //   V-OVG 1: D470 .. D483 (Base: 470)
        //   V-OVG 2: D600 .. D613 (Base: 600)
        //   V-OVG 3: D620 .. D633 (Base: 620)
        //   V-OVG 4: D640 .. D653 (Base: 640)
        public static readonly ushort[] VovgStatusBase = { 0, 470, 600, 620, 640 }; // [0] unused, [1]=V-OVG 1...
        public const ushort VovgStatusCount = 14;

        // Standart Durum Offset'leri (Her cihazın kendi Base adresine göre)
        public const int VovgOffsetTempActualPV = 0;        // Base+0: Sıcaklık PV (x10)
        public const int VovgOffsetTempTargetSP = 1;        // Base+1: Sıcaklık Target SP (x10)
        public const int VovgOffsetTempManualOut = 2;       // Base+2: Sıcaklık Manuel Out
        public const int VovgOffsetTempOutputPower = 3;     // Base+3: Sıcaklık Çıkış Gücü (% x10)
        public const int VovgOffsetTempWorkingSP = 4;       // Base+4: Sıcaklık WSP (x10)

        public const int VovgOffsetFlowActualPV = 5;        // Base+5: Akış PV (sccm)
        public const int VovgOffsetFlowTargetSP = 6;        // Base+6: Akış Target SP (sccm)
        public const int VovgOffsetFlowManualOut = 7;       // Base+7: Akış Manuel Out
        public const int VovgOffsetFlowOutputPower = 8;     // Base+8: Akış Çıkış Gücü (% x10)
        public const int VovgOffsetFlowWorkingSP = 9;       // Base+9: Akış WSP (sccm)

        public const int VovgOffsetTempCommStatus = 10;     // Base+10: Sıcaklık İletişim (1=OK, 0=Yok)
        public const int VovgOffsetTempAlarmStatus = 11;    // Base+11: Sıcaklık Alarm (1=Alarm)
        public const int VovgOffsetFlowCommStatus = 12;     // Base+12: Akış İletişim (1=OK, 0=Yok)
        public const int VovgOffsetFlowAlarmStatus = 13;    // Base+13: Akış Alarm (1=Alarm)

        // Geriye dönük uyumluluk sabitleri (V-OVG 1: Base 470)
        public const ushort VovgTempActualPV = 470;
        public const ushort VovgTempTargetSP = 471;
        public const ushort VovgTempManualOut = 472;
        public const ushort VovgTempOutputPower = 473;
        public const ushort VovgTempWorkingSP = 474;

        public const ushort VovgFlowActualPV = 475;
        public const ushort VovgFlowTargetSP = 476;
        public const ushort VovgFlowManualOut = 477;
        public const ushort VovgFlowOutputPower = 478;
        public const ushort VovgFlowWorkingSP = 479;

        public const ushort VovgTempCommStatus = 480;
        public const ushort VovgTempAlarmStatus = 481;
        public const ushort VovgFlowCommStatus = 482;
        public const ushort VovgFlowAlarmStatus = 483;
    }
}

