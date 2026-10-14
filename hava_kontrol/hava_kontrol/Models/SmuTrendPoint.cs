using System;

namespace hava_kontrol.Models
{
    /// <summary>
    /// Keysight B2902B SMU için zaman damgalı V, I, R ölçüm noktası.
    /// Gerçek zamanlı grafik ve telemetri için kullanılır.
    /// </summary>
    public struct SmuTrendPoint
    {
        public DateTime Timestamp { get; set; }
        public int Channel { get; set; }
        public double Voltage { get; set; }     // V
        public double Current { get; set; }     // A
        public double Resistance { get; set; }  // Ω
        public bool IsOutputOn { get; set; }

        public SmuTrendPoint(DateTime timestamp, int channel, double voltage, double current, double resistance, bool isOutputOn = true)
        {
            Timestamp = timestamp;
            Channel = channel;
            Voltage = voltage;
            Current = current;
            Resistance = resistance;
            IsOutputOn = isOutputOn;
        }
    }
}
