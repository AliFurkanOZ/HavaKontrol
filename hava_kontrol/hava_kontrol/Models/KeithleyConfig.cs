namespace hava_kontrol.Models
{
    /// <summary>
    /// Keithley 6517B Electrometer konfigürasyon sabitleri.
    /// RS-232 üzerinden SCPI protokolü ile haberleşir.
    /// 
    /// Bağlantı: PC COM port → RS-232 → Keithley 6517B
    /// Protokol: SCPI 1991.0 (RS-232)
    /// Kablo: Düz (straight-through), DB-9
    /// </summary>
    public static class KeithleyConfig
    {
        // ==========================================
        // SERİ PORT AYARLARI
        // ==========================================

        /// <summary>Varsayılan COM port adı.</summary>
        public const string DefaultComPort = "COM3";

        /// <summary>Baud rate (cihaz ayarıyla eşleşmeli).</summary>
        public const int BaudRate = 9600;

        /// <summary>Veri bitleri.</summary>
        public const int DataBits = 8;

        /// <summary>Okuma timeout (ms).</summary>
        public const int ReadTimeoutMs = 3000;

        /// <summary>Yazma timeout (ms).</summary>
        public const int WriteTimeoutMs = 2000;

        // ==========================================
        // SCPI KOMUTLARI
        // ==========================================

        /// <summary>Cihaz tanımlama.</summary>
        public const string CmdIdentify = "*IDN?";

        /// <summary>Cihaz sıfırlama.</summary>
        public const string CmdReset = "*RST";

        /// <summary>Tüm hata kuyruğunu temizle.</summary>
        public const string CmdClearStatus = "*CLS";

        // --- Ölçüm Fonksiyonu Seçimi ---
        public const string CmdSetFuncVoltage = ":SENS:FUNC 'VOLT'";
        public const string CmdSetFuncCurrent = ":SENS:FUNC 'CURR'";
        public const string CmdSetFuncCharge = ":SENS:FUNC 'CHAR'";

        // --- Ölçüm Okuma ---
        public const string CmdMeasureVoltage = ":MEAS:VOLT?";
        public const string CmdMeasureCurrent = ":MEAS:CURR?";
        public const string CmdMeasureCharge = ":MEAS:CHAR?";
        public const string CmdRead = ":READ?";
        public const string CmdFetch = ":FETC?";

        // --- Dahili Voltaj Kaynağı ---
        public const string CmdSetVoltage = ":SOUR:VOLT ";         // + değer (V)
        public const string CmdVoltageOn = ":SOUR:VOLT:STAT ON";
        public const string CmdVoltageOff = ":SOUR:VOLT:STAT OFF";
        public const string CmdGetVoltage = ":SOUR:VOLT?";
        public const string CmdGetVoltageState = ":SOUR:VOLT:STAT?";

        // --- Zero Check ---
        public const string CmdZeroCheckOn = ":SYST:ZCH ON";
        public const string CmdZeroCheckOff = ":SYST:ZCH OFF";

        // ==========================================
        // LİMİTLER
        // ==========================================

        /// <summary>Dahili voltaj kaynağı minimum (V).</summary>
        public const float VoltageSourceMin = -1000f;

        /// <summary>Dahili voltaj kaynağı maksimum (V).</summary>
        public const float VoltageSourceMax = 1000f;

        /// <summary>Komut satır sonu karakteri (LF).</summary>
        public const string LineTerminator = "\n";
    }
}
