using System;

namespace hava_kontrol.Models
{
    public enum LogSeverity
    {
        Info,
        Action,
        Warning,
        Alarm
    }

    /// <summary>
    /// Sistem olay, hareket ve alarm günlüğü kaydı.
    /// Milisaniye hassasiyeti ve kategori desteği içerir.
    /// </summary>
    public class LogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public LogSeverity Severity { get; set; } = LogSeverity.Info;
        public string Source { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public bool IsAcknowledged { get; set; }

        public string FormattedTime => Timestamp.ToString("HH:mm:ss.fff");
        public string FormattedTimeShort => Timestamp.ToString("HH:mm:ss");
        public string FormattedDate => Timestamp.ToString("yyyy-MM-dd");
        public string TimestampFull => Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");

        public string SeverityText => Severity switch
        {
            LogSeverity.Action => "İŞLEM",
            LogSeverity.Alarm => "ALARM",
            LogSeverity.Warning => "UYARI",
            _ => "BİLGİ"
        };

        public string BadgeBackground => Severity switch
        {
            LogSeverity.Action => "#E0F2FE",
            LogSeverity.Alarm => "#FEE2E2",
            LogSeverity.Warning => "#FEF3C7",
            _ => "#EFF6FF"
        };

        public string BadgeForeground => Severity switch
        {
            LogSeverity.Action => "#0284C7",
            LogSeverity.Alarm => "#DC2626",
            LogSeverity.Warning => "#D97706",
            _ => "#2563EB"
        };
    }
}
