using System;

namespace hava_kontrol.Models
{
    /// <summary>
    /// Sistemdeki bir video yakalama aygıtının veya simülasyon kaynağının modelidir.
    /// </summary>
    public class CameraDeviceInfo
    {
        /// <summary>
        /// DirectShow aygıt indeksi (0, 1, 2...). Simülasyon için -1.
        /// </summary>
        public int DeviceId { get; set; }

        /// <summary>
        /// Kullanıcıya gösterilen aygıt adı (örn: "Kamera 0 - Surface Camera", "Simülasyon Modu").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Cihaz donanım kimliği veya PnP ID (opsiyonel).
        /// </summary>
        public string DevicePath { get; set; } = string.Empty;

        /// <summary>
        /// Fiziksel kamera yerine sentetik test görüntüsü üretip üretmediği.
        /// </summary>
        public bool IsSimulation { get; set; }

        public override string ToString() => Name;

        public static CameraDeviceInfo CreateSimulation(string label = "🔬 Simülasyon Modu (Test Görüntüsü)")
        {
            return new CameraDeviceInfo
            {
                DeviceId = -1,
                Name = label,
                IsSimulation = true
            };
        }
    }
}
