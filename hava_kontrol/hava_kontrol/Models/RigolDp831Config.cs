namespace hava_kontrol.Models
{
    /// <summary>
    /// RIGOL DP831 Programlanabilir Lineer DC Güç Kaynağı konfigürasyon sabitleri.
    /// RS-232 / Seri Port üzerinden SCPI protokolü ile haberleşir.
    /// 
    /// Model: DP831 (3 Kanal)
    ///   Kanal 1: 0V - 8V / 0A - 5A (Pozitif)
    ///   Kanal 2: 0V - +30V / 0A - 2A (Pozitif)
    ///   Kanal 3: 0V - -30V / 0A - 2A (Negatif Polarite)
    /// 
    /// Bağlantı: PC (RS-232 veya USB-Seri) → DP831
    /// Protokol: SCPI (Standard Commands for Programmable Instruments)
    /// Terminator: LF (0x0A, "\n")
    /// </summary>
    public static class RigolDp831Config
    {
        // ==========================================
        // SERİ PORT AYARLARI
        // ==========================================

        /// <summary>Varsayılan COM port adı.</summary>
        public const string DefaultComPort = "COM4";

        /// <summary>Rigol DP800 varsayılan baud rate (9600).</summary>
        public const int DefaultBaudRate = 9600;

        /// <summary>Veri bitleri.</summary>
        public const int DataBits = 8;

        /// <summary>Okuma timeout (ms).</summary>
        public const int ReadTimeoutMs = 2000;

        /// <summary>Yazma timeout (ms).</summary>
        public const int WriteTimeoutMs = 1000;

        /// <summary>RS-232 satır sonlandırıcı (RIGOL DP800 SCPI: LF "\n").</summary>
        public const string LineTerminator = "\n";

        // ==========================================
        // KANAL AYARLARI VE LİMİTLERİ
        // ==========================================

        public const int ChannelCount = 3;

        // CH1 (8V / 5A)
        public const float Ch1MinVoltage = 0.0f;
        public const float Ch1MaxVoltage = 8.4f;
        public const float Ch1RatedVoltage = 8.0f;
        public const float Ch1MinCurrent = 0.0f;
        public const float Ch1MaxCurrent = 5.3f;
        public const float Ch1RatedCurrent = 5.0f;

        // CH2 (+30V / 2A)
        public const float Ch2MinVoltage = 0.0f;
        public const float Ch2MaxVoltage = 32.0f;
        public const float Ch2RatedVoltage = 30.0f;
        public const float Ch2MinCurrent = 0.0f;
        public const float Ch2MaxCurrent = 2.1f;
        public const float Ch2RatedCurrent = 2.0f;

        // CH3 (-30V / 2A - Negatif Polarite)
        public const float Ch3MinVoltage = -32.0f;
        public const float Ch3MaxVoltage = 0.0f;
        public const float Ch3RatedVoltage = -30.0f;
        public const float Ch3MinCurrent = 0.0f;
        public const float Ch3MaxCurrent = 2.1f;
        public const float Ch3RatedCurrent = 2.0f;

        // ==========================================
        // SCPI KOMUTLARI
        // ==========================================

        /// <summary>Cihaz kimliği sorgulama.</summary>
        public const string CmdIdentify = "*IDN?";

        /// <summary>Cihazı fabrika ayarlarına sıfırlama.</summary>
        public const string CmdReset = "*RST";

        /// <summary>Hata durum kaydı temizleme.</summary>
        public const string CmdClear = "*CLS";

        /// <summary>Kanal voltaj ve akımını tek seferde ayarlar: :APPLy CH{ch},{volt},{curr}</summary>
        public static string CmdApply(int channel, float voltage, float current) =>
            $":APPLy CH{channel},{voltage.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)},{current.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)}";

        /// <summary>Kanal ayarlarını sorgular: :APPLy? CH{ch}</summary>
        public static string CmdApplyQuery(int channel) => $":APPLy? CH{channel}";

        /// <summary>Kanal voltajı ayarlar: :SOURce{ch}:VOLTage {volt}</summary>
        public static string CmdSetVoltage(int channel, float voltage) =>
            $":SOURce{channel}:VOLTage {voltage.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)}";

        /// <summary>Kanal akımı ayarlar: :SOURce{ch}:CURRent {curr}</summary>
        public static string CmdSetCurrent(int channel, float current) =>
            $":SOURce{channel}:CURRent {current.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)}";

        /// <summary>Kanal voltaj ayarını sorgular.</summary>
        public static string CmdGetVoltage(int channel) => $":SOURce{channel}:VOLTage?";

        /// <summary>Kanal akım ayarını sorgular.</summary>
        public static string CmdGetCurrent(int channel) => $":SOURce{channel}:CURRent?";

        /// <summary>Kanal çıkışını açar/kapatır: :OUTPut CH{ch},{ON|OFF}</summary>
        public static string CmdSetOutput(int channel, bool on) =>
            $":OUTPut CH{channel},{(on ? "ON" : "OFF")}";

        /// <summary>Kanal çıkış durumunu sorgular: :OUTPut? CH{ch}</summary>
        public static string CmdGetOutput(int channel) => $":OUTPut? CH{channel}";

        /// <summary>Kanalın gerçek voltaj, akım ve gücünü tek sorguda okur: :MEASure:ALL? CH{ch}</summary>
        public static string CmdMeasureAll(int channel) => $":MEASure:ALL? CH{channel}";
    }
}
