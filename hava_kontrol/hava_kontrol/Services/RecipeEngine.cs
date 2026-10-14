using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using hava_kontrol.Models;

namespace hava_kontrol.Services
{
    public enum RecipeState
    {
        Idle,
        Running,
        Paused,
        Completed,
        Cancelled
    }

    /// <summary>
    /// Çok kanallı ve ürüne özel kuyruklu Reçete Motoru.
    /// Operatörün oluşturduğu adımlı otomasyon sekanslarını yönetir.
    /// Cihaz bazlı asenkron kuyruk ve sıralı sekans modlarını tam destekler.
    /// WPF veri bağlaması için INotifyPropertyChanged uygular.
    /// </summary>
    public class RecipeEngine : IDisposable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (Application.Current?.Dispatcher.CheckAccess() == false)
            {
                Application.Current.Dispatcher.Invoke(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
            }
            else
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private readonly DispatcherTimer _timer;
        private bool _disposed;

        // Cihaz bazlı aktif adımlar (DeviceKey -> Active RecipeStep)
        private readonly Dictionary<string, RecipeStep> _activeSteps = new();
        // Cihaz bazlı bekleyen kuyruklar (DeviceKey -> Queue<RecipeStep>)
        private readonly Dictionary<string, Queue<RecipeStep>> _deviceQueues = new();

        // Sıralı mod için mevcut adım indeksi
        private int _sequentialStepIndex = -1;

        // ======================================
        // MOTOR DURUM ÖZELLİKLERİ (NOTIFY)
        // ======================================

        private Recipe? _currentRecipe;
        public Recipe? CurrentRecipe
        {
            get => _currentRecipe;
            private set
            {
                _currentRecipe = value;
                OnPropertyChanged(nameof(CurrentRecipe));
                OnPropertyChanged(nameof(StepProgressText));
            }
        }

        private RecipeState _state = RecipeState.Idle;
        public RecipeState State
        {
            get => _state;
            private set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
                OnPropertyChanged(nameof(IsRunning));
                OnPropertyChanged(nameof(IsPaused));
                OnPropertyChanged(nameof(IsManualLocked));
                OnPropertyChanged(nameof(StepProgressText));
            }
        }

        public bool IsRunning => State == RecipeState.Running;
        public bool IsPaused => State == RecipeState.Paused;
        public bool IsManualLocked => State == RecipeState.Running || State == RecipeState.Paused;

        private int _totalElapsedSeconds = 0;
        public int TotalElapsedSeconds
        {
            get => _totalElapsedSeconds;
            private set
            {
                _totalElapsedSeconds = value;
                OnPropertyChanged(nameof(TotalElapsedSeconds));
                OnPropertyChanged(nameof(StepProgressText));
            }
        }

        public string StepProgressText
        {
            get
            {
                if (_currentRecipe == null || _currentRecipe.Steps.Count == 0)
                    return "Reçete Yok";

                int completedCount = _currentRecipe.Steps.Count(s => s.Status == RecipeStepStatus.Completed);
                int totalCount = _currentRecipe.Steps.Count;

                if (State == RecipeState.Running)
                {
                    int activeCount = _activeSteps.Count;
                    return $"Yürütülüyor: {completedCount}/{totalCount} Adım · {activeCount} Aktif Ürün · {TotalElapsedSeconds / 60:D2}:{TotalElapsedSeconds % 60:D2}";
                }
                else if (State == RecipeState.Paused)
                {
                    return $"Duraklatıldı: {completedCount}/{totalCount} Adım · {TotalElapsedSeconds / 60:D2}:{TotalElapsedSeconds % 60:D2}";
                }
                else if (State == RecipeState.Completed)
                {
                    return $"Tamamlandı ({totalCount} Adım · {TotalElapsedSeconds / 60:D2}:{TotalElapsedSeconds % 60:D2})";
                }
                else if (State == RecipeState.Cancelled)
                {
                    return "İptal Edildi (Varsayılan Ayarlar Aktif)";
                }

                return $"{totalCount} Adım Hazır";
            }
        }

        // ======================================
        // DONANIM ETKİLEŞİM DELEGATLERİ
        // ======================================

        public Action<int, float>? OnSetMfcSetpoint { get; set; }
        public Action<int, bool>? OnSetMfcFlow { get; set; }

        public Action<int, float>? OnSetVovgTemp { get; set; }
        public Action<int, float>? OnSetVovgFlow { get; set; }

        public Action<int, bool>? OnSetValve { get; set; }
        public Action<int, bool>? OnSetPowerOutput { get; set; }

        public Action<int, float, float, bool>? OnSetPowerSupplyChannel { get; set; }
        public Action<int, bool, ElectrometerMeasureMode, float, float, bool>? OnSetElectrometerChannel { get; set; }

        /// <summary>
        /// İptal edildiğinde tüm donanımı güvenli varsayılana (Default State) çeken delegat.
        /// </summary>
        public Action? OnResetToDefault { get; set; }

        public Action? OnStateChanged { get; set; }

        public RecipeEngine()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
        }

        public void LoadRecipe(Recipe recipe)
        {
            if (IsRunning)
                throw new InvalidOperationException("Reçete çalışırken yeni reçete yüklenemez.");

            _activeSteps.Clear();
            _deviceQueues.Clear();
            _sequentialStepIndex = -1;
            TotalElapsedSeconds = 0;

            recipe.ResetRuntime();
            CurrentRecipe = recipe;
            State = RecipeState.Idle;

            OnStateChanged?.Invoke();
            LoggingService.Instance.Info("REÇETE", $"Reçete yüklendi: '{recipe.Name}' ({recipe.Steps.Count} adım - {recipe.ExecutionModeText})");
        }

        public void Start()
        {
            if (_currentRecipe == null || _currentRecipe.Steps.Count == 0)
                throw new InvalidOperationException("Yüklü veya adımı olan bir reçete bulunamadı.");

            _activeSteps.Clear();
            _deviceQueues.Clear();
            TotalElapsedSeconds = 0;
            _currentRecipe.ResetRuntime();

            State = RecipeState.Running;

            if (_currentRecipe.ExecutionMode == RecipeExecutionMode.Sequential)
            {
                _sequentialStepIndex = 0;
                var firstStep = _currentRecipe.Steps[0];
                firstStep.Status = RecipeStepStatus.Running;
                _activeSteps[firstStep.DeviceKey] = firstStep;
                ApplyStep(firstStep);
            }
            else
            {
                // ÜRÜNE ÖZEL KUYRUK (PerDeviceQueue):
                // Adımları cihaz anahtarlarına göre kuyruklara yerleştir
                foreach (var step in _currentRecipe.Steps)
                {
                    string key = step.DeviceKey;
                    if (!_deviceQueues.ContainsKey(key))
                        _deviceQueues[key] = new Queue<RecipeStep>();

                    _deviceQueues[key].Enqueue(step);
                }

                // Her cihazın ilk adımını başlat
                foreach (var kvp in _deviceQueues.ToList())
                {
                    string key = kvp.Key;
                    var queue = kvp.Value;
                    if (queue.Count > 0)
                    {
                        var step = queue.Dequeue();
                        step.Status = RecipeStepStatus.Running;
                        _activeSteps[key] = step;
                        ApplyStep(step);
                    }
                }
            }

            _timer.Start();
            OnStateChanged?.Invoke();
            LoggingService.Instance.Info("REÇETE", $"Reçete başlatıldı: '{_currentRecipe.Name}' ({_activeSteps.Count} cihaz aktif)");
        }

        public void Pause()
        {
            if (State == RecipeState.Running)
            {
                _timer.Stop();
                State = RecipeState.Paused;

                foreach (var step in _activeSteps.Values)
                {
                    step.Status = RecipeStepStatus.Paused;
                }

                OnStateChanged?.Invoke();
                LoggingService.Instance.Warning("REÇETE", $"Reçete duraklatıldı. Cihaz ayarları korundu. ({StepProgressText})");
            }
        }

        public void Resume()
        {
            if (State == RecipeState.Paused)
            {
                State = RecipeState.Running;

                foreach (var step in _activeSteps.Values)
                {
                    step.Status = RecipeStepStatus.Running;
                }

                _timer.Start();
                OnStateChanged?.Invoke();
                LoggingService.Instance.Info("REÇETE", "Reçete kaldığı yerden devam ettiriliyor.");
            }
        }

        public void Cancel()
        {
            if (State == RecipeState.Running || State == RecipeState.Paused)
            {
                _timer.Stop();
                State = RecipeState.Cancelled;

                foreach (var step in _activeSteps.Values)
                {
                    step.Status = RecipeStepStatus.Cancelled;
                }

                // Bekleyen adımları da iptal olarak işaretle
                if (_currentRecipe != null)
                {
                    foreach (var step in _currentRecipe.Steps)
                    {
                        if (step.Status == RecipeStepStatus.Pending || step.Status == RecipeStepStatus.Running || step.Status == RecipeStepStatus.Paused)
                        {
                            step.Status = RecipeStepStatus.Cancelled;
                        }
                    }
                }

                _activeSteps.Clear();
                _deviceQueues.Clear();

                // TÜM DONANIMI DEFAULT GÜVENLİ HALE GETİR
                try
                {
                    OnResetToDefault?.Invoke();
                }
                catch (Exception ex)
                {
                    LoggingService.Instance.Alarm("REÇETE", $"Reçete iptal edilirken varsayılana sıfırlama hatası: {ex.Message}");
                }

                OnStateChanged?.Invoke();
                LoggingService.Instance.Warning("REÇETE", "Reçete iptal edildi. Tüm sistem parametreleri varsayılan güvenli duruma getirildi.");
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (State != RecipeState.Running || _currentRecipe == null)
                return;

            TotalElapsedSeconds++;

            if (_currentRecipe.ExecutionMode == RecipeExecutionMode.Sequential)
            {
                ProcessSequentialTick();
            }
            else
            {
                ProcessPerDeviceQueueTick();
            }

            OnStateChanged?.Invoke();
        }

        private void ProcessPerDeviceQueueTick()
        {
            var finishedSteps = new List<RecipeStep>();

            // Aktif adımların süresini ilerlet
            foreach (var kvp in _activeSteps.ToList())
            {
                string devKey = kvp.Key;
                var step = kvp.Value;

                step.ElapsedSeconds++;

                if (step.ElapsedSeconds >= step.DurationSeconds)
                {
                    step.Status = RecipeStepStatus.Completed;
                    finishedSteps.Add(step);

                    LoggingService.Instance.Action("REÇETE", $"Adım #{step.StepNumber} tamamlandı: {step.DeviceDisplayName} - {step.Name} (Bitiş Aksiyonu: {step.EndActionText})", category: "REÇETE");
                }
            }

            // Biten adımların yerine aynı cihazın sıradaki adımını başlat ("ürüne özel beklemeli")
            foreach (var finishedStep in finishedSteps)
            {
                string devKey = finishedStep.DeviceKey;
                _activeSteps.Remove(devKey);

                // Eğer adım bitiminde kapatma/sıfırlama seçilmişse derhal kapat
                if (finishedStep.EndAction == RecipeStepEndAction.TurnOff)
                {
                    TurnOffDevice(finishedStep);
                }

                if (_deviceQueues.TryGetValue(devKey, out var queue) && queue.Count > 0)
                {
                    var nextStep = queue.Dequeue();
                    nextStep.Status = RecipeStepStatus.Running;
                    _activeSteps[devKey] = nextStep;
                    ApplyStep(nextStep);

                    LoggingService.Instance.Action("REÇETE", $"Sıradaki adım başlatılıyor: {nextStep.DeviceDisplayName} - Adım #{nextStep.StepNumber} ({nextStep.Name}) · Parametreler: {nextStep.ParametersSummary}", category: "REÇETE");
                }
            }

            // Eğer tüm adımlar bittiyse ve aktif adım kalmadıysa
            if (_activeSteps.Count == 0)
            {
                _timer.Stop();
                State = RecipeState.Completed;
                LoggingService.Instance.Info("REÇETE", $"Reçete başarıyla tamamlandı: '{_currentRecipe!.Name}' (Toplam Süre: {TotalElapsedSeconds} sn)");
            }
        }

        private void ProcessSequentialTick()
        {
            if (_sequentialStepIndex < 0 || _sequentialStepIndex >= _currentRecipe!.Steps.Count)
            {
                _timer.Stop();
                State = RecipeState.Completed;
                return;
            }

            var currentStep = _currentRecipe.Steps[_sequentialStepIndex];
            currentStep.ElapsedSeconds++;

            if (currentStep.ElapsedSeconds >= currentStep.DurationSeconds)
            {
                currentStep.Status = RecipeStepStatus.Completed;
                LoggingService.Instance.Action("REÇETE", $"Adım #{currentStep.StepNumber} tamamlandı: {currentStep.Name} (Bitiş Aksiyonu: {currentStep.EndActionText})", category: "REÇETE");

                // Eğer kapatma seçildiyse kapat
                if (currentStep.EndAction == RecipeStepEndAction.TurnOff)
                {
                    TurnOffDevice(currentStep);
                }

                // Sonraki adıma geç
                if (_sequentialStepIndex + 1 < _currentRecipe.Steps.Count)
                {
                    _sequentialStepIndex++;
                    var nextStep = _currentRecipe.Steps[_sequentialStepIndex];
                    nextStep.Status = RecipeStepStatus.Running;
                    _activeSteps.Clear();
                    _activeSteps[nextStep.DeviceKey] = nextStep;
                    ApplyStep(nextStep);

                    LoggingService.Instance.Action("REÇETE", $"Sıradaki adım başlatılıyor: Adım #{nextStep.StepNumber} ({nextStep.Name}) · Cihaz: {nextStep.DeviceDisplayName} · Parametreler: {nextStep.ParametersSummary}", category: "REÇETE");
                }
                else
                {
                    // Reçete bitti
                    _timer.Stop();
                    State = RecipeState.Completed;
                    LoggingService.Instance.Info("REÇETE", $"Reçete başarıyla tamamlandı: '{_currentRecipe.Name}' (Toplam Süre: {TotalElapsedSeconds} sn)", category: "REÇETE");
                }
            }
        }

        /// <summary>
        /// Adım süresi bittiğinde veya TurnOff seçildiğinde ilgili cihazı güvenli duruma getirir / kapatır.
        /// </summary>
        private void TurnOffDevice(RecipeStep step)
        {
            try
            {
                switch (step.DeviceType)
                {
                    case RecipeDeviceType.Mfc:
                        OnSetMfcSetpoint?.Invoke(step.DeviceIndex, 0f);
                        OnSetMfcFlow?.Invoke(step.DeviceIndex, false);
                        break;

                    case RecipeDeviceType.Vovg:
                        OnSetVovgFlow?.Invoke(step.DeviceIndex, 0f);
                        OnSetVovgTemp?.Invoke(step.DeviceIndex, 20f);
                        break;

                    case RecipeDeviceType.Valve:
                        OnSetValve?.Invoke(step.DeviceIndex, false);
                        break;

                    case RecipeDeviceType.PowerOutput:
                        OnSetPowerOutput?.Invoke(step.DeviceIndex, false);
                        break;

                    case RecipeDeviceType.PowerSupply:
                        OnSetPowerSupplyChannel?.Invoke(step.DeviceIndex, 0f, 0f, false);
                        break;

                    case RecipeDeviceType.Electrometer:
                        OnSetElectrometerChannel?.Invoke(step.DeviceIndex, step.SmuIsVoltageSource, step.SmuMeasureMode, 0f, 0.001f, false);
                        break;
                }

                LoggingService.Instance.Action("REÇETE", $"Adım süresi bitti ve cihaz sıfırlandı: {step.DeviceDisplayName} (#{step.StepNumber})", category: "REÇETE");
            }
            catch (Exception ex)
            {
                LoggingService.Instance.Alarm("REÇETE", $"Cihaz kapatılırken hata ({step.DeviceDisplayName}): {ex.Message}");
            }
        }

        /// <summary>
        /// Seçili adımı ilgili donanım delegatlerini çağırarak uygular.
        /// </summary>
        private void ApplyStep(RecipeStep step)
        {
            try
            {
                switch (step.DeviceType)
                {
                    case RecipeDeviceType.Mfc:
                        OnSetMfcSetpoint?.Invoke(step.DeviceIndex, step.MfcFlowSccm);
                        OnSetMfcFlow?.Invoke(step.DeviceIndex, step.MfcFlowEnabled);
                        break;

                    case RecipeDeviceType.Vovg:
                        OnSetVovgTemp?.Invoke(step.DeviceIndex, step.VovgTempC);
                        OnSetVovgFlow?.Invoke(step.DeviceIndex, step.VovgFlowSccm);
                        break;

                    case RecipeDeviceType.Valve:
                        OnSetValve?.Invoke(step.DeviceIndex, step.ValveIsOpen);
                        break;

                    case RecipeDeviceType.PowerOutput:
                        OnSetPowerOutput?.Invoke(step.DeviceIndex, step.PowerOutputIsOpen);
                        break;

                    case RecipeDeviceType.PowerSupply:
                        OnSetPowerSupplyChannel?.Invoke(step.DeviceIndex, step.PsVoltage, step.PsCurrentLimit, step.PsOutputIsOpen);
                        break;

                    case RecipeDeviceType.Electrometer:
                        OnSetElectrometerChannel?.Invoke(step.DeviceIndex, step.SmuIsVoltageSource, step.SmuMeasureMode, step.SmuSourceValue, step.SmuCompliance, step.SmuOutputIsOpen);
                        break;
                }

                LoggingService.Instance.Action("REÇETE",
                    $"Donanıma komut gönderildi: {step.DeviceDisplayName} · {step.ParametersSummary} (Hedef Süre: {step.DurationSeconds} sn)", category: "REÇETE");
            }
            catch (Exception ex)
            {
                LoggingService.Instance.Alarm("REÇETE", $"Adım #{step.StepNumber} uygulanırken donanım hatası: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _timer.Stop();
                _disposed = true;
            }
        }
    }
}
