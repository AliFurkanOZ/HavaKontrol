using System;

namespace hava_kontrol.Models
{
    /// <summary>
    /// V-OVG Buhar Fırını için zaman serisi Sıcaklık (PV/SP) ve Taşıyıcı Gaz Akışı (PV/SP) veri noktası.
    /// </summary>
    public struct VovgTrendPoint
    {
        public DateTime Timestamp { get; set; }
        public float TempPV { get; set; }
        public float TempSetpoint { get; set; }
        public float FlowPV { get; set; }
        public float FlowSetpoint { get; set; }

        public VovgTrendPoint(DateTime timestamp, float tempPv, float tempSetpoint, float flowPv, float flowSetpoint)
        {
            Timestamp = timestamp;
            TempPV = tempPv;
            TempSetpoint = tempSetpoint;
            FlowPV = flowPv;
            FlowSetpoint = flowSetpoint;
        }
    }
}
