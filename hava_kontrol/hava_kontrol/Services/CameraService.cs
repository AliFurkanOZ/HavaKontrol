using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using hava_kontrol.Models;
using OpenCvSharp;
using CvRect = OpenCvSharp.Rect;

namespace hava_kontrol.Services
{
    /// <summary>
    /// V-OVG kameralarını yöneten, DirectShow ve simülasyon tabanlı video yakalama servisi.
    /// Her V-OVG için bağımsız arka plan döngüsünde akış sağlar, UI thread'ini bloke etmez.
    /// </summary>
    public class CameraService : IDisposable
    {
        private static CameraService? _instance;
        public static CameraService Instance => _instance ??= new CameraService();

        private readonly ConcurrentDictionary<int, CameraStreamSession> _activeSessions = new();
        private readonly CameraConfig _config;
        private bool _disposed;

        public CameraConfig Config => _config;

        public CameraService()
        {
            _config = CameraConfig.Load();
        }

        /// <summary>
        /// Sistemdeki kullanılabilir tüm fiziksel ve simüle kameraları listeler.
        /// </summary>
        public List<CameraDeviceInfo> GetAvailableCameras()
        {
            var cameras = new List<CameraDeviceInfo>();

            // 1. Simülasyon seçeneği her zaman ilk sırada
            cameras.Add(CameraDeviceInfo.CreateSimulation());

            // 2. Windows WMI ile algılanan donanım adlarını al
            var pnpNames = new List<string>();
            try
            {
                using var searcher = new ManagementObjectSearcher(
                    "SELECT Name, DeviceID FROM Win32_PnPEntity WHERE PNPClass = 'Camera' OR PNPClass = 'Image'");
                foreach (ManagementObject obj in searcher.Get())
                {
                    string? name = obj["Name"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        pnpNames.Add(name);
                    }
                }
            }
            catch
            {
                // WMI erişilemezse DirectShow indeksleri kullanılır
            }

            // Aktif olarak yayında olan aygıt ID'lerini tespit et
            var activeDeviceIds = new HashSet<int>(
                _activeSessions.Values
                    .Where(s => s.IsRunning && s.DeviceId >= 0)
                    .Select(s => s.DeviceId)
            );

            // 3. DirectShow ve MediaFoundation aygıtlarını tara (0..5 arası kontrol)
            for (int i = 0; i < 6; i++)
            {
                try
                {
                    // Eğer bu aygıt zaten aktif bir oturumda açıksa ve yayın yapıyorsa,
                    // donanımı test etmek için tekrar açmayın!
                    // Tekrar açmaya çalışmak çalışan DirectShow akışını bozar veya durdurur.
                    if (activeDeviceIds.Contains(i))
                    {
                        string friendlyName = i < pnpNames.Count
                            ? $"Kamera {i} — {pnpNames[i]}"
                            : $"Kamera {i} (Video Aygıtı #{i})";

                        cameras.Add(new CameraDeviceInfo
                        {
                            DeviceId = i,
                            Name = friendlyName,
                            IsSimulation = false
                        });
                        continue;
                    }

                    bool opened = false;
                    try
                    {
                        using var testCap = new VideoCapture(i, VideoCaptureAPIs.DSHOW);
                        opened = testCap.IsOpened();
                    }
                    catch { }

                    if (!opened)
                    {
                        try
                        {
                            using var testCapMsmf = new VideoCapture(i, VideoCaptureAPIs.MSMF);
                            opened = testCapMsmf.IsOpened();
                        }
                        catch { }
                    }

                    if (opened)
                    {
                        string friendlyName = i < pnpNames.Count
                            ? $"Kamera {i} — {pnpNames[i]}"
                            : $"Kamera {i} (Video Aygıtı #{i})";

                        cameras.Add(new CameraDeviceInfo
                        {
                            DeviceId = i,
                            Name = friendlyName,
                            IsSimulation = false
                        });
                    }
                }
                catch
                {
                    // İlgili indeks açılamazsa devam et
                }
            }

            return cameras;
        }

        /// <summary>
        /// Belirli bir V-OVG ünitesi için kamera akışını başlatır.
        /// </summary>
        public void StartStream(
            int vovgNumber,
            int deviceId,
            Func<(float Temp, float Flow, string Status)> telemetryProvider,
            Action<BitmapSource> onFrame,
            Action<string> onStatusChanged,
            Action<string?> onError)
        {
            StopStream(vovgNumber);

            var session = new CameraStreamSession(
                vovgNumber,
                deviceId,
                _config,
                telemetryProvider,
                onFrame,
                onStatusChanged,
                onError);

            _activeSessions[vovgNumber] = session;
            session.Start();
        }

        /// <summary>
        /// Belirli bir V-OVG ünitesinin kamera akışını durdurur.
        /// </summary>
        public void StopStream(int vovgNumber)
        {
            if (_activeSessions.TryRemove(vovgNumber, out var session))
            {
                session.Stop();
                session.Dispose();
            }
        }

        /// <summary>
        /// Belirli bir V-OVG ünitesinin yayında olup olmadığını bildirir.
        /// </summary>
        public bool IsStreaming(int vovgNumber)
        {
            return _activeSessions.TryGetValue(vovgNumber, out var session) && session.IsRunning;
        }

        /// <summary>
        /// Anlık görüntü (Snapshot) çeker ve diske kaydeder.
        /// </summary>
        public string? CaptureSnapshot(int vovgNumber)
        {
            if (_activeSessions.TryGetValue(vovgNumber, out var session))
            {
                return session.SaveSnapshot();
            }
            return null;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            foreach (var kvp in _activeSessions)
            {
                kvp.Value.Stop();
                kvp.Value.Dispose();
            }
            _activeSessions.Clear();
        }

        // ====================================================================
        // İÇ SINIF: CameraStreamSession
        // ====================================================================
        private class CameraStreamSession : IDisposable
        {
            private readonly int _vovgNumber;
            private readonly int _deviceId;
            private readonly CameraConfig _config;
            private readonly Func<(float Temp, float Flow, string Status)> _telemetryProvider;
            private readonly Action<BitmapSource> _onFrame;
            private readonly Action<string> _onStatusChanged;
            private readonly Action<string?> _onError;

            private CancellationTokenSource? _cts;
            private Task? _streamTask;
            private Mat? _latestMat;
            private readonly object _matLock = new();

            public bool IsRunning { get; private set; }
            public int DeviceId => _deviceId;

            public CameraStreamSession(
                int vovgNumber,
                int deviceId,
                CameraConfig config,
                Func<(float Temp, float Flow, string Status)> telemetryProvider,
                Action<BitmapSource> onFrame,
                Action<string> onStatusChanged,
                Action<string?> onError)
            {
                _vovgNumber = vovgNumber;
                _deviceId = deviceId;
                _config = config;
                _telemetryProvider = telemetryProvider;
                _onFrame = onFrame;
                _onStatusChanged = onStatusChanged;
                _onError = onError;
            }

            public void Start()
            {
                if (IsRunning) return;
                IsRunning = true;
                _cts = new CancellationTokenSource();
                var token = _cts.Token;

                _streamTask = Task.Run(() =>
                {
                    if (_deviceId == -1)
                    {
                        RunSimulationLoop(token);
                    }
                    else
                    {
                        RunHardwareLoop(token);
                    }
                }, token);
            }

            public void Stop()
            {
                IsRunning = false;
                try
                {
                    _cts?.Cancel();
                    _streamTask?.Wait(800);
                }
                catch
                {
                    // TaskCancellation normal
                }
                finally
                {
                    _onStatusChanged("DURDURULDU");
                }
            }

            private void RunHardwareLoop(CancellationToken token)
            {
                _onStatusChanged("BAĞLANIYOR...");
                _onError(null);

                VideoCapture? cap = null;
                try
                {
                    try
                    {
                        cap = new VideoCapture(_deviceId, VideoCaptureAPIs.DSHOW);
                    }
                    catch { }

                    if (cap == null || !cap.IsOpened())
                    {
                        cap?.Dispose();
                        try
                        {
                            cap = new VideoCapture(_deviceId, VideoCaptureAPIs.MSMF);
                        }
                        catch { }
                    }

                    if (cap == null || !cap.IsOpened())
                    {
                        _onStatusChanged("KAMERA AÇILAMADI");
                        _onError($"Kamera #{_deviceId} açılamadı. Aygıt meşgul veya takılı değil.");
                        return;
                    }

                    // İstenen çözünürlük
                    cap.Set(VideoCaptureProperties.FrameWidth, 640);
                    cap.Set(VideoCaptureProperties.FrameHeight, 480);
                    cap.Set(VideoCaptureProperties.Fps, _config.TargetFps);

                    _onStatusChanged($"CANLI ({_config.TargetFps} FPS)");

                    using var frame = new Mat();
                    int delayMs = 1000 / Math.Max(1, _config.TargetFps);
                    var stopwatch = new Stopwatch();
                    int consecutiveFailures = 0;

                    while (!token.IsCancellationRequested && cap.IsOpened())
                    {
                        stopwatch.Restart();

                        if (cap.Read(frame) && !frame.Empty())
                        {
                            consecutiveFailures = 0;

                            // Telemetri bindirme
                            if (_config.ShowTelemetryOverlay)
                            {
                                OverlayTelemetry(frame);
                            }

                            lock (_matLock)
                            {
                                _latestMat?.Dispose();
                                _latestMat = frame.Clone();
                            }

                            // BitmapSource'a çevir ve UI'a ilet
                            var bs = MatToFrozenBitmapSource(frame);
                            _onFrame(bs);
                        }
                        else
                        {
                            consecutiveFailures++;
                            if (consecutiveFailures >= 25) // ~1 saniye boyunca hiç kare alınamadıysa
                            {
                                if (!token.IsCancellationRequested)
                                {
                                    _onStatusChanged("SİNYAL KESİLDİ");
                                    _onError($"Kamera #{_deviceId} video akışı kesildi.");
                                }
                                break;
                            }
                        }

                        stopwatch.Stop();
                        int remaining = delayMs - (int)stopwatch.ElapsedMilliseconds;
                        if (remaining > 0)
                        {
                            Thread.Sleep(remaining);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!token.IsCancellationRequested)
                    {
                        _onStatusChanged("HATA OLUŞTU");
                        _onError(ex.Message);
                    }
                }
                finally
                {
                    IsRunning = false;
                    cap?.Release();
                    cap?.Dispose();
                }
            }

            private void RunSimulationLoop(CancellationToken token)
            {
                _onStatusChanged($"SİMÜLASYON ({_config.TargetFps} FPS)");
                _onError(null);

                int width = 640;
                int height = 480;
                int delayMs = 1000 / Math.Max(1, _config.TargetFps);
                var stopwatch = new Stopwatch();
                int frameCounter = 0;

                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        stopwatch.Restart();
                        frameCounter++;

                        var (temp, flow, _) = _telemetryProvider();

                        using var frame = GenerateSyntheticOvenFrame(width, height, temp, flow, frameCounter);

                        if (_config.ShowTelemetryOverlay)
                        {
                            OverlayTelemetry(frame);
                        }

                        lock (_matLock)
                        {
                            _latestMat?.Dispose();
                            _latestMat = frame.Clone();
                        }

                        var bs = MatToFrozenBitmapSource(frame);
                        _onFrame(bs);

                        stopwatch.Stop();
                        int remaining = delayMs - (int)stopwatch.ElapsedMilliseconds;
                        if (remaining > 0)
                        {
                            Thread.Sleep(remaining);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!token.IsCancellationRequested)
                    {
                        _onStatusChanged("HATA OLUŞTU");
                        _onError(ex.Message);
                    }
                }
            }

            private Mat GenerateSyntheticOvenFrame(int width, int height, float temp, float flow, int frameCount)
            {
                var mat = new Mat(height, width, MatType.CV_8UC3, new Scalar(18, 20, 26));

                // 1. Dış hazne gövdesi (Koyu metalik çerçeve)
                Cv2.Rectangle(mat, new CvRect(30, 30, width - 60, height - 60), new Scalar(38, 42, 54), -1);
                Cv2.Rectangle(mat, new CvRect(30, 30, width - 60, height - 60), new Scalar(80, 90, 110), 2);

                // 2. Quartz Cam Fırın Tüpü (Ortadaki silindir)
                int tubeX = 140;
                int tubeY = 80;
                int tubeW = width - 280;
                int tubeH = height - 160;

                // Sıcaklığa göre akkor ışıma rengi hesapla (25°C gri-mavi, 100°C parlak turuncu-kırmızı)
                float heatFactor = Math.Clamp((temp - 25f) / 75f, 0f, 1f);
                byte glowR = (byte)(40 + heatFactor * 210);
                byte glowG = (byte)(50 + heatFactor * 100);
                byte glowB = (byte)(70 - heatFactor * 50);

                Cv2.Rectangle(mat, new CvRect(tubeX, tubeY, tubeW, tubeH), new Scalar(glowB, glowG, glowR), -1);
                Cv2.Rectangle(mat, new CvRect(tubeX, tubeY, tubeW, tubeH), new Scalar(200, 220, 240), 2);

                // 3. Fırın Isıtıcı Sarmal Telleri (Spiral Rezistans)
                int coilCount = 14;
                for (int i = 0; i < coilCount; i++)
                {
                    int cx = tubeX + (i + 1) * (tubeW / (coilCount + 1));
                    Scalar coilColor = heatFactor > 0.15f
                        ? new Scalar(30, 100 + (int)(heatFactor * 120), 220 + (int)(heatFactor * 35))
                        : new Scalar(100, 110, 120);

                    Cv2.Line(mat, new OpenCvSharp.Point(cx, tubeY + 10), new OpenCvSharp.Point(cx, tubeY + tubeH - 10), coilColor, 3);
                }

                // 4. Numune Botu / Taşıyıcı Kapsül (Ortadaki numune)
                int boatW = 100;
                int boatH = 28;
                int boatX = tubeX + (tubeW - boatW) / 2;
                int boatY = tubeY + (tubeH - boatH) / 2;
                Cv2.Rectangle(mat, new CvRect(boatX, boatY, boatW, boatH), new Scalar(210, 215, 220), -1);
                Cv2.Rectangle(mat, new CvRect(boatX, boatY, boatW, boatH), new Scalar(70, 75, 80), 1);
                Cv2.PutText(mat, "NUMUNE", new OpenCvSharp.Point(boatX + 18, boatY + 18),
                    HersheyFonts.HersheySimplex, 0.45, new Scalar(30, 30, 30), 1);

                // 5. Taşıyıcı Gaz Akış Animasyonu (Partiküller soldan sağa akar)
                if (flow > 1f)
                {
                    int particleSpeed = Math.Max(2, (int)(flow / 25f));
                    for (int p = 0; p < 8; p++)
                    {
                        int px = tubeX + ((frameCount * particleSpeed + p * 45) % (tubeW - 20));
                        int py = tubeY + 30 + (p * 37) % (tubeH - 60);
                        Cv2.Circle(mat, new OpenCvSharp.Point(px, py), 3, new Scalar(255, 230, 180), -1);
                    }
                }

                // 6. Optik Hedefleme / Nişangah (Crosshair)
                int midX = width / 2;
                int midY = height / 2;
                Cv2.Line(mat, new OpenCvSharp.Point(midX - 25, midY), new OpenCvSharp.Point(midX + 25, midY), new Scalar(0, 255, 255), 1);
                Cv2.Line(mat, new OpenCvSharp.Point(midX, midY - 25), new OpenCvSharp.Point(midX, midY + 25), new Scalar(0, 255, 255), 1);
                Cv2.Circle(mat, new OpenCvSharp.Point(midX, midY), 16, new Scalar(0, 255, 255), 1);

                return mat;
            }

            private void OverlayTelemetry(Mat mat)
            {
                var (temp, flow, status) = _telemetryProvider();

                // Üst Şerit (Yarı saydam karanlık zemin)
                Cv2.Rectangle(mat, new CvRect(0, 0, mat.Width, 34), new Scalar(10, 14, 20), -1);

                // Sol: Fırın Adı ve Durum
                string title = $"V-OVG {_vovgNumber}  [{(IsRunning ? "CANLI" : "KAPALI")}]";
                Cv2.PutText(mat, title, new OpenCvSharp.Point(12, 22),
                    HersheyFonts.HersheySimplex, 0.55, new Scalar(0, 255, 128), 1);

                // Sağ: Canlı Telemetri Değerleri
                string metrics = $"T: {temp:F1} C  |  Q: {flow:F0} sccm";
                Cv2.PutText(mat, metrics, new OpenCvSharp.Point(mat.Width - 250, 22),
                    HersheyFonts.HersheySimplex, 0.52, new Scalar(255, 255, 255), 1);

                // Alt Şerit: Zaman Damgası
                Cv2.Rectangle(mat, new CvRect(0, mat.Height - 24, mat.Width, 24), new Scalar(10, 14, 20), -1);
                string timeStr = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}  ·  {status}";
                Cv2.PutText(mat, timeStr, new OpenCvSharp.Point(12, mat.Height - 7),
                    HersheyFonts.HersheySimplex, 0.42, new Scalar(180, 190, 200), 1);
            }

            public string? SaveSnapshot()
            {
                try
                {
                    Mat? snapshot = null;
                    lock (_matLock)
                    {
                        if (_latestMat != null && !_latestMat.Empty())
                        {
                            snapshot = _latestMat.Clone();
                        }
                    }

                    if (snapshot == null) return null;

                    string dir = _config.SnapshotDirectory;
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    string filename = $"VOVG{_vovgNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    string fullPath = Path.Combine(dir, filename);

                    snapshot.SaveImage(fullPath);
                    snapshot.Dispose();
                    return fullPath;
                }
                catch
                {
                    return null;
                }
            }

            private static BitmapSource MatToFrozenBitmapSource(Mat mat)
            {
                var bs = BitmapSource.Create(
                    mat.Width,
                    mat.Height,
                    96, 96,
                    PixelFormats.Bgr24,
                    null,
                    mat.Data,
                    (int)(mat.Step() * mat.Height),
                    (int)mat.Step()
                );
                bs.Freeze();
                return bs;
            }

            public void Dispose()
            {
                Stop();
                lock (_matLock)
                {
                    _latestMat?.Dispose();
                    _latestMat = null;
                }
                _cts?.Dispose();
            }
        }
    }
}
