namespace hava_kontrol.Models
{
    /// <summary>
    /// Keysight B2902B Precision Source/Measure Unit (SMU) SCPI konfigürasyon ve komut sabitleri.
    /// USB (USB-TMC / VISA) veya Seri Port (RS-232 / Virtual COM) üzerinden SCPI protokolü ile haberleşir.
    /// 
    /// Cihaz Özellikleri:
    ///   - 2 Bağımsız Kanal (Ch1, Ch2)
    ///   - Kaynak: ±210 V DC / ±3.03 A DC (Darbeli: 10.5 A)
    ///   - Ölçüm: Akım (pA/nA/µA/mA/A), Voltaj (µV/mV/V), Direnç (mΩ/Ω/kΩ/MΩ/GΩ)
    ///   - Koruma (Compliance): Aşırı akım/voltaj sınır koruması
    /// </summary>
    public static class KeysightB2902bConfig
    {
        // ==========================================
        // BAĞLANTI AYARLARI
        // ==========================================

        /// <summary>Varsayılan USB otomatik tespit etiketi.</summary>
        public const string DefaultUsbEndpoint = "USB (Keysight Otomatik)";

        /// <summary>Varsayılan COM port adı (Seri bağlantı için).</summary>
        public const string DefaultComPort = "COM3";

        /// <summary>Varsayılan baud rate (Seri port için).</summary>
        public const int DefaultBaudRate = 9600;

        /// <summary>Veri bitleri.</summary>
        public const int DataBits = 8;

        /// <summary>Okuma zaman aşımı (ms).</summary>
        public const int ReadTimeoutMs = 3000;

        /// <summary>Yazma zaman aşımı (ms).</summary>
        public const int WriteTimeoutMs = 2000;

        /// <summary>Komut satır sonu karakteri (LF).</summary>
        public const string LineTerminator = "\n";

        // ==========================================
        // GENEL SCPI KOMUTLARI
        // ==========================================

        /// <summary>Cihaz tanımlama (*IDN?).</summary>
        public const string CmdIdentify = "*IDN?";

        /// <summary>Cihazı fabrika/güvenli başlangıç durumuna sıfırlama.</summary>
        public const string CmdReset = "*RST";

        /// <summary>Hata kuyruğunu ve durum bayraklarını temizleme.</summary>
        public const string CmdClearStatus = "*CLS";

        /// <summary>Sistem hata kuyruğunu sorgulama.</summary>
        public const string CmdGetError = ":SYST:ERR?";

        /// <summary>Uzaktan kontrol moduna geçiş.</summary>
        public const string CmdRemote = ":SYST:REM";

        /// <summary>Yerel (ön panel) kontrol moduna dönüş.</summary>
        public const string CmdLocal = ":SYST:LOC";

        /// <summary>Tetikleme ve devam eden ölçümleri derhal iptal eder (IDLE durumuna döndürür).</summary>
        public static string CmdAbort(int? channel = null) =>
            channel.HasValue ? $":ABOR (@{channel.Value})" : ":ABOR";

        // ==========================================
        // ÇIKIŞ KONTROLÜ (OUTPUT)
        // ==========================================

        public static string CmdSetOutput(int channel, bool on) =>
            on ? $":OUTP{channel}:STAT ON" : $":ABOR (@{channel});:OUTP{channel}:STAT OFF";

        public static string CmdGetOutput(int channel) =>
            $":OUTP{channel}:STAT?";

        /// <summary>Çıkış kapalıyken terminal durumunu ayarlar (HIZ: Yüksek Empedans / Anında Röle Açma, NORM: Normal, ZERO: 0V).</summary>
        public static string CmdSetOutputOffMode(int channel, string mode = "HIZ") =>
            $":OUTP{channel}:OFF:MODE {mode}";

        /// <summary>Ölçüm başlatıldığında çıkışın otomatik açılmasını devre dışı bırakmak için (OFF).</summary>
        public static string CmdSetAutoOutputOn(int channel, bool on) =>
            $":OUTP{channel}:ON:AUTO {(on ? "ON" : "OFF")}";

        /// <summary>Tetikleme bittiğinde çıkışın otomatik kapanması kontrolü.</summary>
        public static string CmdSetAutoOutputOff(int channel, bool on) =>
            $":OUTP{channel}:OFF:AUTO {(on ? "ON" : "OFF")}";

        // ==========================================
        // ENTEGRASYON VE HIZ AYARLARI (NPLC)
        // ==========================================

        /// <summary>Ölçüm hızını ve güç hattı döngüsünü ayarlar (1 NPLC = 20ms @50Hz - hızlı ve kararlı).</summary>
        public static string CmdSetNplc(int channel, double nplc = 1.0) =>
            string.Format(System.Globalization.CultureInfo.InvariantCulture, ":SENS{0}:CURR:NPLC {1:F1};:SENS{0}:VOLT:NPLC {1:F1}", channel, nplc);

        // ==========================================
        // KAYNAK AYARLARI (SOURCE)
        // ==========================================

        public static string CmdSetSourceMode(int channel, bool isVoltageMode) =>
            $":SOUR{channel}:FUNC:MODE {(isVoltageMode ? "VOLT" : "CURR")}";

        public static string CmdSetSourceVoltage(int channel, float voltage) =>
            string.Format(System.Globalization.CultureInfo.InvariantCulture, ":SOUR{0}:VOLT {1:F4}", channel, voltage);

        public static string CmdGetSourceVoltage(int channel) =>
            $":SOUR{channel}:VOLT?";

        public static string CmdSetSourceCurrent(int channel, float current) =>
            string.Format(System.Globalization.CultureInfo.InvariantCulture, ":SOUR{0}:CURR {1:E6}", channel, current);

        public static string CmdGetSourceCurrent(int channel) =>
            $":SOUR{channel}:CURR?";

        // ==========================================
        // KORUMA / COMPLIANCE (SENSE PROTECTION)
        // ==========================================

        /// <summary>Gerilim verilirken maksimum akım koruma sınırı (Compliance).</summary>
        public static string CmdSetCurrentCompliance(int channel, float maxCurrent) =>
            string.Format(System.Globalization.CultureInfo.InvariantCulture, ":SENS{0}:CURR:PROT {1:E6}", channel, maxCurrent);

        public static string CmdGetCurrentCompliance(int channel) =>
            $":SENS{channel}:CURR:PROT?";

        /// <summary>Akım verilirken maksimum gerilim koruma sınırı (Compliance).</summary>
        public static string CmdSetVoltageCompliance(int channel, float maxVoltage) =>
            string.Format(System.Globalization.CultureInfo.InvariantCulture, ":SENS{0}:VOLT:PROT {1:F3}", channel, maxVoltage);

        public static string CmdGetVoltageCompliance(int channel) =>
            $":SENS{channel}:VOLT:PROT?";

        // ==========================================
        // ÖLÇÜM SORGULARI (MEASURE)
        // ==========================================

        public static string CmdMeasureAll(int channel) =>
            $":MEAS? (@{channel})";

        public static string CmdMeasureCurrent(int channel) =>
            $":MEAS:CURR? (@{channel})";

        public static string CmdMeasureVoltage(int channel) =>
            $":MEAS:VOLT? (@{channel})";

        public static string CmdMeasureResistance(int channel) =>
            $":MEAS:RES? (@{channel})";

        // ==========================================
        // GÜVENLİK VE LİMİTLER (B2902B)
        // ==========================================

        /// <summary>B2902B gerilim kaynağı alt sınırı (-210 V).</summary>
        public const float VoltageSourceMin = -210.0f;

        /// <summary>B2902B gerilim kaynağı üst sınırı (+210 V).</summary>
        public const float VoltageSourceMax = 210.0f;

        /// <summary>B2902B DC akım kaynağı alt sınırı (-3.03 A).</summary>
        public const float CurrentSourceMin = -3.03f;

        /// <summary>B2902B DC akım kaynağı üst sınırı (+3.03 A).</summary>
        public const float CurrentSourceMax = 3.03f;

        /// <summary>Varsayılan güvenli akım koruma sınırı (50 mA).</summary>
        public const float DefaultCurrentCompliance = 0.05f;

        /// <summary>Varsayılan güvenli gerilim koruma sınırı (21 V).</summary>
        public const float DefaultVoltageCompliance = 21.0f;

        /// <summary>İnsan ve numune güvenliği için yüksek gerilim uyarı eşiği (42 V).</summary>
        public const float HighVoltageWarningThreshold = 42.0f;
    }

    /// <summary>Keysight B2902B ölçüm modları.</summary>
    public enum ElectrometerMeasureMode
    {
        Current,    // Akım Ölçümü (A)
        Voltage,    // Voltaj Ölçümü (V)
        Resistance  // Direnç Ölçümü (Ω)
    }
}
