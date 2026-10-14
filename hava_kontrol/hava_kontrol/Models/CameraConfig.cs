using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace hava_kontrol.Models
{
    /// <summary>
    /// V-OVG kamera konfigürasyonu ve kalıcılık ayarları.
    /// </summary>
    public class CameraConfig
    {
        private static readonly string ConfigPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "camera_config.json");

        /// <summary>
        /// V-OVG Numarası -> Seçili Camera DeviceId (-1 = Simülasyon, 0, 1, 2... = Donanım)
        /// </summary>
        public Dictionary<int, int> VovgCameraAssignments { get; set; } = new()
        {
            { 1, 0 },  // V-OVG 1 -> Kamera 0
            { 2, 1 },  // V-OVG 2 -> Kamera 1
            { 3, 2 },  // V-OVG 3 -> Kamera 2
            { 4, 3 }   // V-OVG 4 -> Kamera 3
        };

        /// <summary>
        /// V-OVG Numarası -> Seçili Kamera Adı (USB bağlantı noktası değişse bile aynı kamerayı tanımak için)
        /// </summary>
        public Dictionary<int, string> VovgCameraNameAssignments { get; set; } = new();

        /// <summary>
        /// Hedef yakalama kare hızı (FPS). USB veriyolunu korumak için varsayılan 25 FPS önerilir.
        /// </summary>
        public int TargetFps { get; set; } = 25;

        /// <summary>
        /// Görüntü üzerine sıcaklık, debi ve zaman damgası bilgisi bindirme durumu.
        /// </summary>
        public bool ShowTelemetryOverlay { get; set; } = true;

        /// <summary>
        /// Anlık görüntü (Snapshot) kayıt dizini.
        /// </summary>
        public string SnapshotDirectory { get; set; } = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Captures",
            "V-OVG");

        /// <summary>
        /// Ayarları diskten okur. Dosya yoksa varsayılan ayarlarla oluşturur.
        /// </summary>
        public static CameraConfig Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    var cfg = JsonSerializer.Deserialize<CameraConfig>(json);
                    if (cfg != null) return cfg;
                }
            }
            catch
            {
                // Okuma hatasında varsayılan döner
            }

            var defaultConfig = new CameraConfig();
            defaultConfig.Save();
            return defaultConfig;
        }

        /// <summary>
        /// Ayarları diske kaydeder.
        /// </summary>
        public void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, json);
            }
            catch
            {
                // Log or ignore
            }
        }
    }
}
