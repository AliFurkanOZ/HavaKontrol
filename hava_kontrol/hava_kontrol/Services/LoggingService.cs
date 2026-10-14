using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using hava_kontrol.Models;

namespace hava_kontrol.Services
{
    /// <summary>
    /// Yüksek performanslı, thread-safe, çift katmanlı (disk streaming + UI ring buffer) olay ve hareket günlüğü servisi.
    /// 8-9+ saatlik kesintisiz çalışma senaryolarında sıfır veri kaybı ve kontrollü bellek tüketimi sağlar.
    /// </summary>
    public class LoggingService : IDisposable
    {
        private static readonly Lazy<LoggingService> _instance = new(() => new LoggingService());
        public static LoggingService Instance => _instance.Value;

        private readonly object _uiLock = new();
        private const int MaxUiLogEntries = 1000;

        public ObservableCollection<LogEntry> Entries { get; } = new();

        // Asenkron disk yazıcı kuyruğu
        private readonly BlockingCollection<LogEntry> _logQueue = new(new ConcurrentQueue<LogEntry>());
        private readonly CancellationTokenSource _cts = new();
        private readonly Task _writerTask;

        private readonly string _logsDirectory;
        private readonly string _sessionCsvPath;
        private readonly string _dailyLogPath;

        private long _totalLoggedCount;
        public long TotalLoggedCount => Interlocked.Read(ref _totalLoggedCount);

        public string LogsDirectory => _logsDirectory;
        public string CurrentSessionFilePath => _sessionCsvPath;

        public LoggingService()
        {
            try
            {
                _logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                Directory.CreateDirectory(_logsDirectory);

                string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string dateStamp = DateTime.Now.ToString("yyyy-MM-dd");

                _sessionCsvPath = Path.Combine(_logsDirectory, $"Session_{timeStamp}.csv");
                _dailyLogPath = Path.Combine(_logsDirectory, $"GasFlow_{dateStamp}.log");

                // CSV Başlığını oluştur (UTF-8 BOM ile Excel uyumlu)
                if (!File.Exists(_sessionCsvPath))
                {
                    File.WriteAllText(_sessionCsvPath, "\"Tarih\",\"Saat (ms)\",\"Seviye\",\"Kategori\",\"Kaynak\",\"Mesaj\",\"Detay\"\r\n", new UTF8Encoding(true));
                }

                // Günlük log dosyasına oturum başlangıç ayracı koy
                File.AppendAllText(_dailyLogPath, $"\r\n================================================================================\r\n" +
                                                  $"OTURUM BAŞLADI: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}\r\n" +
                                                  $"================================================================================\r\n", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoggingService başlatma hatası: {ex.Message}");
                _logsDirectory = AppDomain.CurrentDomain.BaseDirectory;
                _sessionCsvPath = Path.Combine(_logsDirectory, "fallback_session.csv");
                _dailyLogPath = Path.Combine(_logsDirectory, "fallback_log.log");
            }

            // Arka plan yazıcı görevini başlat
            _writerTask = Task.Run(() => ProcessLogQueue(_cts.Token));

            // Uygulama kapanırken son kuyruğu boşaltma
            AppDomain.CurrentDomain.ProcessExit += (_, _) => Dispose();
        }

        public void Log(LogSeverity severity, string source, string message, string? category = null, string? details = null)
        {
            var entry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Severity = severity,
                Source = source,
                Category = category ?? source,
                Message = message,
                Details = details,
                IsAcknowledged = severity == LogSeverity.Info || severity == LogSeverity.Action
            };

            Interlocked.Increment(ref _totalLoggedCount);

            // 1. Asenkron disk kuyruğuna ekle (Milisaniyeden kısa sürer, çağıranı asla bekletmez)
            if (!_logQueue.IsAddingCompleted)
            {
                try
                {
                    _logQueue.Add(entry);
                }
                catch (InvalidOperationException) { }
            }

            // 2. UI Ring Buffer'a ekle (Hafif ve kontrollü)
            DispatchToUi(entry);
        }

        public void Action(string source, string message, string? category = null, string? details = null) =>
            Log(LogSeverity.Action, source, message, category, details);

        public void Info(string source, string message, string? category = null, string? details = null) =>
            Log(LogSeverity.Info, source, message, category, details);

        public void Warning(string source, string message, string? category = null, string? details = null) =>
            Log(LogSeverity.Warning, source, message, category, details);

        public void Alarm(string source, string message, string? category = null, string? details = null) =>
            Log(LogSeverity.Alarm, source, message, category, details);

        private void DispatchToUi(LogEntry entry)
        {
            var app = Application.Current;
            if (app != null && !app.Dispatcher.HasShutdownStarted)
            {
                app.Dispatcher.BeginInvoke(() =>
                {
                    lock (_uiLock)
                    {
                        Entries.Insert(0, entry);
                        while (Entries.Count > MaxUiLogEntries)
                        {
                            Entries.RemoveAt(Entries.Count - 1);
                        }
                    }
                });
            }
        }

        public void AcknowledgeAllAlarms()
        {
            lock (_uiLock)
            {
                foreach (var entry in Entries)
                {
                    entry.IsAcknowledged = true;
                }
            }
            Action("KULLANICI", "Tüm bekleyen alarmlar kullanıcı tarafından onaylandı.");
        }

        public void ClearUiLogs()
        {
            lock (_uiLock)
            {
                Entries.Clear();
            }
            Info("SİSTEM", "Kullanıcı arayüzü log listesi temizlendi (Diskteki oturum logları korunmaktadır).");
        }

        /// <summary>
        /// Arka planda kuyruktaki tüm kayıtları oturum CSV ve günlük LOG dosyasına yazar.
        /// </summary>
        private void ProcessLogQueue(CancellationToken ct)
        {
            StreamWriter? csvWriter = null;
            StreamWriter? logWriter = null;

            try
            {
                // FileShare.ReadWrite ile dosya açılır, böylece Excel veya başka bir program dosyayı kilitlemez
                var csvStream = new FileStream(_sessionCsvPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                csvWriter = new StreamWriter(csvStream, new UTF8Encoding(true)) { AutoFlush = false };

                var logStream = new FileStream(_dailyLogPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                logWriter = new StreamWriter(logStream, Encoding.UTF8) { AutoFlush = false };

                DateTime lastFlush = DateTime.UtcNow;

                while (!ct.IsCancellationRequested || _logQueue.Count > 0)
                {
                    if (_logQueue.TryTake(out var entry, 500, ct))
                    {
                        // CSV satırı
                        string safeMsg = entry.Message.Replace("\"", "\"\"");
                        string safeSrc = entry.Source.Replace("\"", "\"\"");
                        string safeCat = entry.Category.Replace("\"", "\"\"");
                        string safeDet = (entry.Details ?? string.Empty).Replace("\"", "\"\"");

                        csvWriter.WriteLine($"\"{entry.FormattedDate}\",\"{entry.FormattedTime}\",\"{entry.SeverityText}\",\"{safeCat}\",\"{safeSrc}\",\"{safeMsg}\",\"{safeDet}\"");

                        // Formatted Text LOG satırı
                        string line = $"[{entry.TimestampFull}] [{entry.SeverityText,-6}] [{safeSrc,-10}] {entry.Message}";
                        if (!string.IsNullOrEmpty(entry.Details))
                            line += $" | Detay: {entry.Details}";
                        logWriter.WriteLine(line);

                        // Kritik durumlarda veya belirli aralıklarla diske sifonla (flush)
                        if (entry.Severity == LogSeverity.Alarm || entry.Severity == LogSeverity.Action || (DateTime.UtcNow - lastFlush).TotalSeconds >= 2)
                        {
                            csvWriter.Flush();
                            logWriter.Flush();
                            lastFlush = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        // Boşta kalınca bekleyenleri diske yaz
                        csvWriter.Flush();
                        logWriter.Flush();
                        lastFlush = DateTime.UtcNow;
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Debug.WriteLine($"Log yazıcı hatası: {ex.Message}");
            }
            finally
            {
                try
                {
                    csvWriter?.Flush();
                    csvWriter?.Dispose();
                    logWriter?.Flush();
                    logWriter?.Dispose();
                }
                catch { }
            }
        }

        /// <summary>
        /// Tüm oturum log dosyasını (disk üzerinde saklanan tam geçmişi) istenen konuma kopyalar/indirir.
        /// </summary>
        public bool ExportFullSessionTo(string targetFilePath)
        {
            try
            {
                // Mevcut yazıcıyı flush etmek için kuyruğun geçici durulmasını bekle
                SpinWait.SpinUntil(() => _logQueue.Count == 0, 500);

                if (File.Exists(_sessionCsvPath))
                {
                    // FileShare.ReadWrite ile oku
                    using var sourceStream = new FileStream(_sessionCsvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using var targetStream = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write);
                    sourceStream.CopyTo(targetStream);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Oturum dışa aktarma hatası: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Belirli veya filtrelenmiş LogEntry koleksiyonunu CSV olarak dışa aktarır.
        /// </summary>
        public bool ExportEntriesToCsv(IEnumerable<LogEntry> entries, string filePath)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("\"Tarih\",\"Saat (ms)\",\"Seviye\",\"Kategori\",\"Kaynak\",\"Mesaj\",\"Detay\"");

                foreach (var e in entries)
                {
                    string safeMsg = e.Message.Replace("\"", "\"\"");
                    string safeSrc = e.Source.Replace("\"", "\"\"");
                    string safeCat = e.Category.Replace("\"", "\"\"");
                    string safeDet = (e.Details ?? string.Empty).Replace("\"", "\"\"");
                    sb.AppendLine($"\"{e.FormattedDate}\",\"{e.FormattedTime}\",\"{e.SeverityText}\",\"{safeCat}\",\"{safeSrc}\",\"{safeMsg}\",\"{safeDet}\"");
                }

                File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(true));
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CSV export hatası: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Geriye dönük uyumluluk için ExportToCsv
        /// </summary>
        public bool ExportToCsv(string filePath)
        {
            // Eğer tam oturum dışa aktarılabiliyorsa diskten aktar, yoksa bellektekileri aktar
            if (File.Exists(_sessionCsvPath))
            {
                return ExportFullSessionTo(filePath);
            }

            lock (_uiLock)
            {
                return ExportEntriesToCsv(Entries, filePath);
            }
        }

        /// <summary>
        /// Windows Explorer üzerinde Log klasörünü açar.
        /// </summary>
        public void OpenLogsFolder()
        {
            try
            {
                if (Directory.Exists(_logsDirectory))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = _logsDirectory,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Log klasörü açılırken hata: {ex.Message}");
            }
        }

        public void Dispose()
        {
            try
            {
                _logQueue.CompleteAdding();
                _cts.Cancel();
                _writerTask.Wait(1500);
            }
            catch { }
        }
    }
}
