using System;

namespace hava_kontrol.Models
{
    /// <summary>
    /// MFC 1..4 için zaman serisi ölçüm ve hedef (setpoint) veri noktası.
    /// </summary>
    public struct MfcTrendPoint
    {
        public DateTime Timestamp { get; set; }
        public float Mfc1Actual { get; set; }
        public float Mfc1Setpoint { get; set; }
        public float Mfc2Actual { get; set; }
        public float Mfc2Setpoint { get; set; }
        public float Mfc3Actual { get; set; }
        public float Mfc3Setpoint { get; set; }
        public float Mfc4Actual { get; set; }
        public float Mfc4Setpoint { get; set; }

        public MfcTrendPoint(
            DateTime timestamp,
            float mfc1Actual, float mfc1Setpoint,
            float mfc2Actual, float mfc2Setpoint,
            float mfc3Actual, float mfc3Setpoint,
            float mfc4Actual, float mfc4Setpoint)
        {
            Timestamp = timestamp;
            Mfc1Actual = mfc1Actual;
            Mfc1Setpoint = mfc1Setpoint;
            Mfc2Actual = mfc2Actual;
            Mfc2Setpoint = mfc2Setpoint;
            Mfc3Actual = mfc3Actual;
            Mfc3Setpoint = mfc3Setpoint;
            Mfc4Actual = mfc4Actual;
            Mfc4Setpoint = mfc4Setpoint;
        }
    }
}
