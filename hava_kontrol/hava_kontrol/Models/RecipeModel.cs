using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace hava_kontrol.Models
{
    /// <summary>
    /// Reçetede kontrol edilebilen ürün/cihaz tipleri.
    /// </summary>
    public enum RecipeDeviceType
    {
        [Description("MFC (Kütle Akış Kontrolcü)")]
        Mfc = 0,

        [Description("V-OVG (Buharlaştırıcı Fırın)")]
        Vovg = 1,

        [Description("Solenoid Valf")]
        Valve = 2,

        [Description("PLC Güç Çıkışı")]
        PowerOutput = 3,

        [Description("RIGOL DP831 Güç Kaynağı")]
        PowerSupply = 4,

        [Description("Keysight SMU / Elektrometre")]
        Electrometer = 5
    }

    /// <summary>
    /// Reçete adımı çalışma durumu.
    /// </summary>
    public enum RecipeStepStatus
    {
        Pending,     // Bekliyor
        Running,     // Çalışıyor
        Paused,      // Duraklatıldı
        Completed,   // Tamamlandı
        Cancelled    // İptal Edildi
    }

    /// <summary>
    /// Reçete yürütme modu.
    /// </summary>
    public enum RecipeExecutionMode
    {
        /// <summary>
        /// Ürüne Özel Kuyruk (Asenkron):
        /// Aynı cihazın önceki adımı bitmeden sıradaki adımı başlamaz.
        /// Farklı cihazlar bağımsız/paralel olarak yürütülür.
        /// </summary>
        PerDeviceQueue = 0,

        /// <summary>
        /// Sıralı (Ardışık):
        /// Her adım tamamlandıktan sonra bir sonraki adıma geçilir.
        /// </summary>
        Sequential = 1
    }

    /// <summary>
    /// Adım süresi tamamlandığında ilgili ürünün alacağı durum.
    /// </summary>
    public enum RecipeStepEndAction
    {
        [Description("Mevcut Durumu Koru / Devam Et")]
        KeepState = 0,

        [Description("Cihazı Kapat / Sıfırla")]
        TurnOff = 1
    }

    /// <summary>
    /// Reçetedeki tek bir adımı temsil eder.
    /// UI ve motor ile tam reaktif haberleşme için INotifyPropertyChanged uygular.
    /// </summary>
    public class RecipeStep : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // ======================================
        // TEMEL KİMLİK & SÜRE
        // ======================================

        private int _stepNumber = 1;
        [JsonPropertyName("stepNumber")]
        public int StepNumber
        {
            get => _stepNumber;
            set => SetField(ref _stepNumber, value);
        }

        private string _name = "Adım 1";
        [JsonPropertyName("name")]
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        private RecipeDeviceType _deviceType = RecipeDeviceType.Mfc;
        [JsonPropertyName("deviceType")]
        public RecipeDeviceType DeviceType
        {
            get => _deviceType;
            set
            {
                if (SetField(ref _deviceType, value))
                {
                    OnPropertyChanged(nameof(DeviceDisplayName));
                    OnPropertyChanged(nameof(ParametersSummary));
                }
            }
        }

        private int _deviceIndex = 1;
        /// <summary>
        /// Cihaz veya kanal numarası:
        /// MFC: 1..4 | V-OVG: 1..4 | Valf: 1..8, 13..16 | Güç Çıkışı: 8..11 | DP831: 1..3 | SMU: 1..2
        /// </summary>
        [JsonPropertyName("deviceIndex")]
        public int DeviceIndex
        {
            get => _deviceIndex;
            set
            {
                if (SetField(ref _deviceIndex, value))
                {
                    OnPropertyChanged(nameof(DeviceDisplayName));
                    OnPropertyChanged(nameof(ParametersSummary));
                }
            }
        }

        private int _durationSeconds = 60;
        [JsonPropertyName("durationSeconds")]
        public int DurationSeconds
        {
            get => _durationSeconds;
            set
            {
                if (SetField(ref _durationSeconds, Math.Max(1, value)))
                {
                    OnPropertyChanged(nameof(DurationFormatted));
                    OnPropertyChanged(nameof(RemainingSeconds));
                    OnPropertyChanged(nameof(ProgressPercent));
                }
            }
        }

        private RecipeStepEndAction _endAction = RecipeStepEndAction.KeepState;
        /// <summary>
        /// Adım süresi bittiğinde bu cihazın alacağı aksiyon:
        /// KeepState (0): Mevcut durumu korur (varsayılan), bir sonraki adım için açık kalır.
        /// TurnOff (1): Bu cihazın çıkışı/akışı derhal kapatılır ve sıfırlanır.
        /// </summary>
        [JsonPropertyName("endAction")]
        public RecipeStepEndAction EndAction
        {
            get => _endAction;
            set
            {
                if (SetField(ref _endAction, value))
                {
                    OnPropertyChanged(nameof(EndActionText));
                    OnPropertyChanged(nameof(EndActionBrush));
                    OnPropertyChanged(nameof(ParametersSummary));
                }
            }
        }

        [JsonIgnore]
        public string EndActionText
        {
            get => EndAction == RecipeStepEndAction.KeepState ? "Koru (Açık)" : "Kapat / Sıfırla";
            set { }
        }

        private static readonly SolidColorBrush BrushKeepState = new(Color.FromRgb(34, 197, 94));
        private static readonly SolidColorBrush BrushTurnOff = new(Color.FromRgb(239, 68, 68));

        [JsonIgnore]
        public Brush EndActionBrush => EndAction == RecipeStepEndAction.KeepState ? BrushKeepState : BrushTurnOff;

        // ======================================
        // CİHAZA ÖZEL PARAMETRELER
        // ======================================

        // 1. MFC Parametreleri
        private float _mfcFlowSccm = 100f;
        [JsonPropertyName("mfcFlowSccm")]
        public float MfcFlowSccm
        {
            get => _mfcFlowSccm;
            set
            {
                if (SetField(ref _mfcFlowSccm, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        private bool _mfcFlowEnabled = true;
        [JsonPropertyName("mfcFlowEnabled")]
        public bool MfcFlowEnabled
        {
            get => _mfcFlowEnabled;
            set
            {
                if (SetField(ref _mfcFlowEnabled, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        // 2. V-OVG Parametreleri
        private float _vovgTempC = 25f;
        [JsonPropertyName("vovgTempC")]
        public float VovgTempC
        {
            get => _vovgTempC;
            set
            {
                if (SetField(ref _vovgTempC, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        private float _vovgFlowSccm = 50f;
        [JsonPropertyName("vovgFlowSccm")]
        public float VovgFlowSccm
        {
            get => _vovgFlowSccm;
            set
            {
                if (SetField(ref _vovgFlowSccm, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        // 3. Valf Parametreleri
        private bool _valveIsOpen = true;
        [JsonPropertyName("valveIsOpen")]
        public bool ValveIsOpen
        {
            get => _valveIsOpen;
            set
            {
                if (SetField(ref _valveIsOpen, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        // 4. PLC Güç Çıkışı Parametreleri
        private bool _powerOutputIsOpen = true;
        [JsonPropertyName("powerOutputIsOpen")]
        public bool PowerOutputIsOpen
        {
            get => _powerOutputIsOpen;
            set
            {
                if (SetField(ref _powerOutputIsOpen, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        // 5. RIGOL DP831 Güç Kaynağı Parametreleri
        private float _psVoltage = 5.0f;
        [JsonPropertyName("psVoltage")]
        public float PsVoltage
        {
            get => _psVoltage;
            set
            {
                if (SetField(ref _psVoltage, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        private float _psCurrentLimit = 1.0f;
        [JsonPropertyName("psCurrentLimit")]
        public float PsCurrentLimit
        {
            get => _psCurrentLimit;
            set
            {
                if (SetField(ref _psCurrentLimit, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        private bool _psOutputIsOpen = true;
        [JsonPropertyName("psOutputIsOpen")]
        public bool PsOutputIsOpen
        {
            get => _psOutputIsOpen;
            set
            {
                if (SetField(ref _psOutputIsOpen, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        // 6. Keysight SMU / Elektrometre Parametreleri
        private bool _smuIsVoltageSource = true;
        [JsonPropertyName("smuIsVoltageSource")]
        public bool SmuIsVoltageSource
        {
            get => _smuIsVoltageSource;
            set
            {
                if (SetField(ref _smuIsVoltageSource, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        private ElectrometerMeasureMode _smuMeasureMode = ElectrometerMeasureMode.Current;
        [JsonPropertyName("smuMeasureMode")]
        public ElectrometerMeasureMode SmuMeasureMode
        {
            get => _smuMeasureMode;
            set
            {
                if (SetField(ref _smuMeasureMode, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        private float _smuSourceValue = 1.0f; // V veya mA
        [JsonPropertyName("smuSourceValue")]
        public float SmuSourceValue
        {
            get => _smuSourceValue;
            set
            {
                if (SetField(ref _smuSourceValue, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        private float _smuCompliance = 0.05f; // Akım sınırı (A) veya gerilim sınırı (V)
        [JsonPropertyName("smuCompliance")]
        public float SmuCompliance
        {
            get => _smuCompliance;
            set
            {
                if (SetField(ref _smuCompliance, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        private bool _smuOutputIsOpen = true;
        [JsonPropertyName("smuOutputIsOpen")]
        public bool SmuOutputIsOpen
        {
            get => _smuOutputIsOpen;
            set
            {
                if (SetField(ref _smuOutputIsOpen, value))
                    OnPropertyChanged(nameof(ParametersSummary));
            }
        }

        // ======================================
        // ESKİ DESTEK VE GERİYE DÖNÜK UYUMLULUK ALANLARI
        // ======================================

        [JsonPropertyName("targetPpm")]
        public float TargetPpm { get; set; } = 100f;

        [JsonPropertyName("totalFlowSccm")]
        public float TotalFlowSccm { get; set; } = 500f;

        [JsonPropertyName("ovenTempC")]
        public float OvenTempC { get; set; } = 45f;

        [JsonPropertyName("targetMfcIndex")]
        public int TargetMfcIndex { get; set; } = 1;

        [JsonPropertyName("carrierMfcIndex")]
        public int CarrierMfcIndex { get; set; } = 2;

        [JsonPropertyName("cylinderPpm")]
        public float CylinderPpm { get; set; } = 10000f;

        [JsonPropertyName("furnaceBypass")]
        public bool FurnaceBypass { get; set; } = false;

        [JsonIgnore]
        public float CalculatedTargetMfcFlow
        {
            get
            {
                if (CylinderPpm <= 0 || TotalFlowSccm <= 0) return 0f;
                float flow = TotalFlowSccm * (TargetPpm / CylinderPpm);
                return Math.Clamp(flow, 0f, TotalFlowSccm);
            }
        }

        [JsonIgnore]
        public float CalculatedCarrierMfcFlow
        {
            get
            {
                float target = CalculatedTargetMfcFlow;
                return Math.Max(0f, TotalFlowSccm - target);
            }
        }

        // ======================================
        // ÇALIŞMA ZAMANI (RUNTIME) DURUM VE UI ALANLARI
        // ======================================

        private RecipeStepStatus _status = RecipeStepStatus.Pending;
        [JsonIgnore]
        public RecipeStepStatus Status
        {
            get => _status;
            set
            {
                if (SetField(ref _status, value))
                {
                    OnPropertyChanged(nameof(StatusText));
                    OnPropertyChanged(nameof(StatusBrush));
                    OnPropertyChanged(nameof(IsRunning));
                }
            }
        }

        private int _elapsedSeconds = 0;
        [JsonIgnore]
        public int ElapsedSeconds
        {
            get => _elapsedSeconds;
            set
            {
                if (SetField(ref _elapsedSeconds, value))
                {
                    OnPropertyChanged(nameof(RemainingSeconds));
                    OnPropertyChanged(nameof(RemainingFormatted));
                    OnPropertyChanged(nameof(ProgressPercent));
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }

        /// <summary>
        /// Tüm UI kolonlarının ve hesaplanan alanların anında tazelenmesini sağlar.
        /// </summary>
        public void NotifyAllPropertiesChanged()
        {
            OnPropertyChanged(nameof(StepNumber));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(DeviceType));
            OnPropertyChanged(nameof(DeviceIndex));
            OnPropertyChanged(nameof(DeviceDisplayName));
            OnPropertyChanged(nameof(ParametersSummary));
            OnPropertyChanged(nameof(DurationSeconds));
            OnPropertyChanged(nameof(DurationFormatted));
            OnPropertyChanged(nameof(ElapsedSeconds));
            OnPropertyChanged(nameof(RemainingSeconds));
            OnPropertyChanged(nameof(RemainingFormatted));
            OnPropertyChanged(nameof(ProgressPercent));
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(StatusText));
            OnPropertyChanged(nameof(StatusBrush));
            OnPropertyChanged(nameof(IsRunning));
            OnPropertyChanged(nameof(EndAction));
            OnPropertyChanged(nameof(EndActionText));
            OnPropertyChanged(nameof(EndActionBrush));
        }

        [JsonIgnore]
        public int RemainingSeconds
        {
            get => Math.Max(0, DurationSeconds - ElapsedSeconds);
            set { }
        }

        [JsonIgnore]
        public double ProgressPercent
        {
            get => DurationSeconds > 0
                ? Math.Clamp(((double)ElapsedSeconds / DurationSeconds) * 100.0, 0.0, 100.0)
                : 0.0;
            set { }
        }

        [JsonIgnore]
        public bool IsRunning => Status == RecipeStepStatus.Running;

        [JsonIgnore]
        public string DurationFormatted
        {
            get => $"{DurationSeconds / 60:D2}:{DurationSeconds % 60:D2}";
            set { }
        }

        [JsonIgnore]
        public string RemainingFormatted
        {
            get => $"{RemainingSeconds / 60:D2}:{RemainingSeconds % 60:D2}";
            set { }
        }

        [JsonIgnore]
        public string StatusText
        {
            get => Status switch
            {
                RecipeStepStatus.Pending => "Bekliyor",
                RecipeStepStatus.Running => $"Çalışıyor ({RemainingFormatted})",
                RecipeStepStatus.Paused => "Duraklatıldı",
                RecipeStepStatus.Completed => "Tamamlandı",
                RecipeStepStatus.Cancelled => "İptal Edildi",
                _ => "—"
            };
            set { }
        }

        private static readonly SolidColorBrush BrushPending = new(Color.FromRgb(148, 163, 184));
        private static readonly SolidColorBrush BrushRunning = new(Color.FromRgb(34, 197, 94));
        private static readonly SolidColorBrush BrushPaused = new(Color.FromRgb(245, 158, 11));
        private static readonly SolidColorBrush BrushCompleted = new(Color.FromRgb(59, 130, 246));
        private static readonly SolidColorBrush BrushCancelled = new(Color.FromRgb(239, 68, 68));

        static RecipeStep()
        {
            BrushPending.Freeze();
            BrushRunning.Freeze();
            BrushPaused.Freeze();
            BrushCompleted.Freeze();
            BrushCancelled.Freeze();
        }

        [JsonIgnore]
        public Brush StatusBrush => Status switch
        {
            RecipeStepStatus.Pending => BrushPending,
            RecipeStepStatus.Running => BrushRunning,
            RecipeStepStatus.Paused => BrushPaused,
            RecipeStepStatus.Completed => BrushCompleted,
            RecipeStepStatus.Cancelled => BrushCancelled,
            _ => BrushPending
        };

        /// <summary>
        /// Cihaz için okunabilir ekran adı (Örn: "MFC-1", "V-OVG 1", "Valf 5", "RIGOL CH1")
        /// </summary>
        [JsonIgnore]
        public string DeviceDisplayName
        {
            get => DeviceType switch
            {
                RecipeDeviceType.Mfc => $"MFC-{DeviceIndex}",
                RecipeDeviceType.Vovg => $"V-OVG {DeviceIndex}",
                RecipeDeviceType.Valve => $"Valf {DeviceIndex}",
                RecipeDeviceType.PowerOutput => $"Güç Çıkışı {DeviceIndex}",
                RecipeDeviceType.PowerSupply => $"RIGOL DP831 (CH{DeviceIndex})",
                RecipeDeviceType.Electrometer => $"SMU / Elektrometre (Kanal {DeviceIndex})",
                _ => $"Cihaz {DeviceIndex}"
            };
            set { }
        }

        /// <summary>
        /// Adım için ayarlanan parametrelerin tek satırlık özeti.
        /// </summary>
        [JsonIgnore]
        public string ParametersSummary
        {
            get
            {
                string actionSuffix = EndAction == RecipeStepEndAction.TurnOff ? " · [Bitişte: KAPAT]" : " · [Bitişte: KORU]";
                string baseSummary = DeviceType switch
                {
                    RecipeDeviceType.Mfc => $"Debi: {MfcFlowSccm:F1} sccm · Akış: {(MfcFlowEnabled ? "AÇIK" : "KAPALI")}",
                    RecipeDeviceType.Vovg => $"Sıcaklık: {VovgTempC:F1} °C · Akış: {VovgFlowSccm:F0} sccm",
                    RecipeDeviceType.Valve => $"Durum: {(ValveIsOpen ? "AÇIK (ON)" : "KAPALI (OFF)")}",
                    RecipeDeviceType.PowerOutput => $"Durum: {(PowerOutputIsOpen ? "AÇIK (ON)" : "KAPALI (OFF)")}",
                    RecipeDeviceType.PowerSupply => $"Gerilim: {PsVoltage:F2} V · Akım Limiti: {PsCurrentLimit:F2} A · Çıkış: {(PsOutputIsOpen ? "AÇIK" : "KAPALI")}",
                    RecipeDeviceType.Electrometer => $"Kaynak: {(SmuIsVoltageSource ? "V-Source" : "I-Source")} ({SmuSourceValue:F3}) · Ölçüm: {SmuMeasureMode} · Çıkış: {(SmuOutputIsOpen ? "AÇIK" : "KAPALI")}",
                    _ => "Özellikler ayarlanmadı"
                };
                return baseSummary + actionSuffix;
            }
            set { }
        }

        /// <summary>
        /// Kuyruk gruplaması için cihaz anahtarı (Örn: "Mfc_1", "Vovg_2")
        /// </summary>
        [JsonIgnore]
        public string DeviceKey => $"{DeviceType}_{DeviceIndex}";

        /// <summary>
        /// Derin kopya (Clone) oluşturur.
        /// </summary>
        public RecipeStep Clone()
        {
            return new RecipeStep
            {
                StepNumber = this.StepNumber,
                Name = this.Name,
                DeviceType = this.DeviceType,
                DeviceIndex = this.DeviceIndex,
                DurationSeconds = this.DurationSeconds,
                EndAction = this.EndAction,
                MfcFlowSccm = this.MfcFlowSccm,
                MfcFlowEnabled = this.MfcFlowEnabled,
                VovgTempC = this.VovgTempC,
                VovgFlowSccm = this.VovgFlowSccm,
                ValveIsOpen = this.ValveIsOpen,
                PowerOutputIsOpen = this.PowerOutputIsOpen,
                PsVoltage = this.PsVoltage,
                PsCurrentLimit = this.PsCurrentLimit,
                PsOutputIsOpen = this.PsOutputIsOpen,
                SmuIsVoltageSource = this.SmuIsVoltageSource,
                SmuMeasureMode = this.SmuMeasureMode,
                SmuSourceValue = this.SmuSourceValue,
                SmuCompliance = this.SmuCompliance,
                SmuOutputIsOpen = this.SmuOutputIsOpen,
                TargetPpm = this.TargetPpm,
                TotalFlowSccm = this.TotalFlowSccm,
                OvenTempC = this.OvenTempC,
                TargetMfcIndex = this.TargetMfcIndex,
                CarrierMfcIndex = this.CarrierMfcIndex,
                CylinderPpm = this.CylinderPpm,
                FurnaceBypass = this.FurnaceBypass
            };
        }

        public void ResetRuntime()
        {
            Status = RecipeStepStatus.Pending;
            ElapsedSeconds = 0;
        }
    }

    /// <summary>
    /// Otomatik sekans reçetesi modeli.
    /// </summary>
    public class Recipe : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private string _name = "Yeni Reçete";
        [JsonPropertyName("name")]
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        private string _description = string.Empty;
        [JsonPropertyName("description")]
        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }

        private RecipeExecutionMode _executionMode = RecipeExecutionMode.PerDeviceQueue;
        [JsonPropertyName("executionMode")]
        public RecipeExecutionMode ExecutionMode
        {
            get => _executionMode;
            set
            {
                if (SetField(ref _executionMode, value))
                    OnPropertyChanged(nameof(ExecutionModeText));
            }
        }

        [JsonIgnore]
        public string ExecutionModeText => ExecutionMode == RecipeExecutionMode.PerDeviceQueue
            ? "Ürüne Özel Kuyruk (Asenkron)"
            : "Ardışık Sıralı (Sequential)";

        private ObservableCollection<RecipeStep> _steps = new();
        [JsonPropertyName("steps")]
        public ObservableCollection<RecipeStep> Steps
        {
            get => _steps;
            set
            {
                if (SetField(ref _steps, value))
                {
                    OnPropertyChanged(nameof(TotalDurationSeconds));
                    OnPropertyChanged(nameof(TotalDurationFormatted));
                }
            }
        }

        [JsonIgnore]
        public int TotalDurationSeconds
        {
            get
            {
                if (Steps == null || Steps.Count == 0) return 0;
                if (ExecutionMode == RecipeExecutionMode.Sequential)
                {
                    int total = 0;
                    foreach (var s in Steps) total += s.DurationSeconds;
                    return total;
                }
                else
                {
                    // Ürüne özel kuyrukta her cihazın toplam süresinin maksimumu
                    var deviceDurations = new Dictionary<string, int>();
                    foreach (var s in Steps)
                    {
                        string key = s.DeviceKey;
                        if (!deviceDurations.ContainsKey(key))
                            deviceDurations[key] = 0;
                        deviceDurations[key] += s.DurationSeconds;
                    }
                    int max = 0;
                    foreach (var d in deviceDurations.Values)
                    {
                        if (d > max) max = d;
                    }
                    return max;
                }
            }
        }

        [JsonIgnore]
        public string TotalDurationFormatted => $"{TotalDurationSeconds / 60:D2}:{TotalDurationSeconds % 60:D2}";

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            });
        }

        public static Recipe? FromJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<Recipe>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                });
            }
            catch
            {
                return null;
            }
        }

        public void ResetRuntime()
        {
            foreach (var step in Steps)
            {
                step.ResetRuntime();
            }
        }
    }
}
