using System;

namespace hava_kontrol.Models
{
    /// <summary>
    /// Trend grafiği için zaman damgalı ölçüm noktası.
    /// </summary>
    public struct TrendDataPoint
    {
        public DateTime Timestamp { get; set; }
        public float ConcentrationPpm { get; set; }
        public double ElectrometerValue { get; set; }
        public float TemperatureC { get; set; }
        public float TotalFlowSccm { get; set; }

        public TrendDataPoint(DateTime timestamp, float ppm, double electrometer, float tempC, float totalFlow)
        {
            Timestamp = timestamp;
            ConcentrationPpm = ppm;
            ElectrometerValue = electrometer;
            TemperatureC = tempC;
            TotalFlowSccm = totalFlow;
        }
    }
}
