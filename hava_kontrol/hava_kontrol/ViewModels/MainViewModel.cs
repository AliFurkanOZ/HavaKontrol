using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using hava_kontrol.Models;
using hava_kontrol.Services;

namespace hava_kontrol.ViewModels
{
    /// <summary>
    /// Ana uygulama ViewModel.
    /// PLC + MFC + V-OVG + Keithley 6517B + RIGOL DP831 tüm kontrol mantığını yönetir.
    /// 
    /// Mimari:
    ///   PC → Modbus TCP → PLC (AS228P)
    ///                        ├── Valfler (Y0.0..Y0.8)
    ///                        └── COM2/RS485 → MFC 1,2,3,4
    ///   PC → COM12/RS485 → V-OVG
    ///                        ├── Eurotherm #4 (Flow - RCXX)
    ///                        └── Eurotherm #3 (Temp - RRXX)
    ///   PC → COM3/RS-232 → Keithley 6517B Electrometer
    ///   PC → RS-232/USB → RIGOL DP831 Programmable DC Power Supply
    /// 
    /// Tüm I/O arka planda çalışır, UI thread bloke edilmez.
    /// </summary>
    public class MainViewModel : BaseViewModel, IDisposable
    {
        private readonly PlcService _plcService;
        private readonly MfcManager _mfcManager;
        private readonly VovgManager _vovgManager;
        private readonly KeysightB2902bService _keysightService;
        private readonly RigolDp831Service _dp831Service;
        private readonly DispatcherTimer _timer;
        private bool _disposed;
        private int _keysightReconnectTicks;
        private bool _isKeysightConnecting;

        // İlk bağlantı tamamlanana kadar "KESİLDİ" göstermeyi engeller.
        private bool _plcInitialConnectDone;

        // Interlock ve Alarm durum değişim takibi
        private ushort _prevInterlock;
        private ushort _prevAlarm;

        // Timer callback sırasında yeniden girişi engeller.
        private bool _isUpdating;

        // ======================================
        // MFC STATE MACHINE
        // ======================================
        // PLC timer-based state machine (plan1111) D455'i otomatik
        // olarak döndürür: 0→10→20→...→170→0 (her state 300ms).
        // PC sadece komut register'larını yazar (D3xx),
        // durum register'larını okur (D4xx).
        // D455'e PC MÜDAHALE ETMEZ — PLC kendi yönetir.

        // ======================================
        // PLC DURUMU
        // ======================================

        private string _plcStatusText = "PLC: BAĞLANIYOR...";
        public string PlcStatusText
        {
            get => _plcStatusText;
            set => SetProperty(ref _plcStatusText, value);
        }

        private bool _isPlcConnected;
        public bool IsPlcConnected
        {
            get => _isPlcConnected;
            set
            {
                if (SetProperty(ref _isPlcConnected, value))
                    OnPropertyChanged(nameof(PlcStatusColor));
            }
        }

        public Brush PlcStatusColor => IsPlcConnected
            ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
            : new SolidColorBrush(Color.FromRgb(148, 163, 184));

        // ======================================
        // MFC BAĞLANTI DURUMU
        // (PLC bağlantısına bağlı, ayrı bağlantı yok)
        // ======================================

        private string _mfcStatusText = "MFC: BAĞLANIYOR...";
        public string MfcStatusText
        {
            get => _mfcStatusText;
            set => SetProperty(ref _mfcStatusText, value);
        }

        private bool _isMfcConnected;
        public bool IsMfcConnected
        {
            get => _isMfcConnected;
            set
            {
                if (SetProperty(ref _isMfcConnected, value))
                    OnPropertyChanged(nameof(MfcStatusColor));
            }
        }

        public Brush MfcStatusColor => IsMfcConnected
            ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
            : new SolidColorBrush(Color.FromRgb(148, 163, 184));

        // ======================================
        // INTERLOCK
        // ======================================

        private string _interlockText = "INTERLOCK: NORMAL";
        public string InterlockText
        {
            get => _interlockText;
            set => SetProperty(ref _interlockText, value);
        }

        private bool _isInterlockActive;
        public bool IsInterlockActive
        {
            get => _isInterlockActive;
            set
            {
                if (SetProperty(ref _isInterlockActive, value))
                    OnPropertyChanged(nameof(InterlockColor));
            }
        }

        public Brush InterlockColor => IsInterlockActive
            ? new SolidColorBrush(Color.FromRgb(239, 68, 68))
            : new SolidColorBrush(Color.FromRgb(34, 197, 94));

        // ======================================
        // GENEL GÜÇ
        // ======================================

        // Varsayılan: true — röle olmadığı için MFC'ler her zaman güçlü.
        // Röle bağlandığında false yapılıp kullanıcıdan açması istenebilir.
        private bool _isGeneralPowerOn = true;
        public bool IsGeneralPowerOn
        {
            get => _isGeneralPowerOn;
            set
            {
                if (SetProperty(ref _isGeneralPowerOn, value))
                {
                    // PLC'ye genel güç komutu yaz (D300)
                    _mfcManager.SetGeneralPower(value);
                    OnPropertyChanged(nameof(GeneralPowerText));
                    OnPropertyChanged(nameof(GeneralPowerColor));
                }
            }
        }

        public string GeneralPowerText => IsGeneralPowerOn ? "GENEL GÜÇ: AÇIK" : "GENEL GÜÇ: KAPALI";

        public Brush GeneralPowerColor => IsGeneralPowerOn
            ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
            : new SolidColorBrush(Color.FromRgb(148, 163, 184));

        // ======================================
        // PLC ALARM
        // ======================================

        private string _alarmText = "";
        public string AlarmText
        {
            get => _alarmText;
            set => SetProperty(ref _alarmText, value);
        }

        private bool _hasAlarm;
        public bool HasAlarm
        {
            get => _hasAlarm;
            set
            {
                if (SetProperty(ref _hasAlarm, value))
                    OnPropertyChanged(nameof(AlarmColor));
            }
        }

        public Brush AlarmColor => HasAlarm
            ? new SolidColorBrush(Color.FromRgb(239, 68, 68))
            : new SolidColorBrush(Color.FromRgb(34, 197, 94));

        // ======================================
        // WATCHDOG
        // ======================================

        private bool _isWatchdogOk;
        public bool IsWatchdogOk
        {
            get => _isWatchdogOk;
            set => SetProperty(ref _isWatchdogOk, value);
        }

        // ======================================
        // VALFLER & GÜÇ ÇIKIŞLARI & MFC'LER
        // ======================================

        public ObservableCollection<ValveViewModel> Valves { get; } = new();
        public ObservableCollection<PowerOutputViewModel> PowerOutputs { get; } = new();
        public ObservableCollection<MfcViewModel> Mfcs { get; } = new();

        // ======================================
        // V-OVG DURUMU (4 Adet Cihaz)
        // ======================================

        private string _vovgStatusText = "V-OVG: BAĞLI DEĞİL";
        public string VovgStatusText
        {
            get => _vovgStatusText;
            set => SetProperty(ref _vovgStatusText, value);
        }

        public ObservableCollection<VovgViewModel> VovgDevices { get; } = new();
        public VovgViewModel Vovg => VovgDevices.Count > 0 ? VovgDevices[0] : null!;
        public VovgViewModel? Vovg1 => VovgDevices.Count > 0 ? VovgDevices[0] : null;
        public VovgViewModel? Vovg2 => VovgDevices.Count > 1 ? VovgDevices[1] : null;
        public VovgViewModel? Vovg3 => VovgDevices.Count > 2 ? VovgDevices[2] : null;
        public VovgViewModel? Vovg4 => VovgDevices.Count > 3 ? VovgDevices[3] : null;

        // Fırın Tahliye Vanaları (VALF 13, 14, 15, 16)
        public ObservableCollection<ValveViewModel> AuxiliaryValves { get; } = new();
        public ValveViewModel? Valve13 => AuxiliaryValves.Count > 0 ? AuxiliaryValves[0] : null;
        public ValveViewModel? Valve14 => AuxiliaryValves.Count > 1 ? AuxiliaryValves[1] : null;
        public ValveViewModel? Valve15 => AuxiliaryValves.Count > 2 ? AuxiliaryValves[2] : null;
        public ValveViewModel? Valve16 => AuxiliaryValves.Count > 3 ? AuxiliaryValves[3] : null;

        /// <summary>Tüm valfler (Valf 1..8 ve 13..16) tek koleksiyon görünümü</summary>
        public IEnumerable<ValveViewModel> AllValves => Valves.Concat(AuxiliaryValves);

        // KEYSIGHT B2902B ELEKTROMETRE DURUMU
        // ======================================

        private string _keithleyStatusText = "KEYSIGHT: BAĞLI DEĞİL";
        public string KeithleyStatusText
        {
            get => _keithleyStatusText;
            set => SetProperty(ref _keithleyStatusText, value);
        }

        public ElectrometerViewModel Electrometer { get; }

        // ======================================
        // RIGOL DP831 GÜÇ KAYNAĞI DURUMU
        // ======================================

        private string _dp831StatusText = "DP831: BAĞLI DEĞİL";
        public string Dp831StatusText
        {
            get => _dp831StatusText;
            set
            {
                if (SetProperty(ref _dp831StatusText, value))
                    OnPropertyChanged(nameof(GpdStatusText));
            }
        }

        public string GpdStatusText => Dp831StatusText;

        public PowerSupplyViewModel PowerSupply { get; }

        // ======================================
        // SON GÜNCELLEME
        // ======================================

        private string _lastUpdateText = "";
        public string LastUpdateText
        {
            get => _lastUpdateText;
            set => SetProperty(ref _lastUpdateText, value);
        }

        // ======================================
        // SEÇİLİ CİHAZ (DİNAMİK DETAY PANELİ)
        // ======================================

        private object? _selectedDevice;
        public object? SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                if (_selectedDevice is ISelectableDevice oldDev)
                    oldDev.IsSelected = false;

                if (SetProperty(ref _selectedDevice, value))
                {
                    if (_selectedDevice is ISelectableDevice newDev)
                        newDev.IsSelected = true;
                }
            }
        }

        public ICommand SelectDeviceCommand { get; }

        // ======================================
        // SİSTEM DURUMU (SOL ÜST)
        // ======================================

        private string _systemStateText = "HAZIR";
        public string SystemStateText
        {
            get => _systemStateText;
            set => SetProperty(ref _systemStateText, value);
        }

        private string _systemStateSubtitle = "Manuel kontrol aktif";
        public string SystemStateSubtitle
        {
            get => _systemStateSubtitle;
            set => SetProperty(ref _systemStateSubtitle, value);
        }

        private Brush _systemStateColor = new SolidColorBrush(Color.FromRgb(148, 163, 184));
        public Brush SystemStateColor
        {
            get => _systemStateColor;
            set => SetProperty(ref _systemStateColor, value);
        }

        // ======================================
        // CİHAZ SAĞLIĞI ÖZETİ (ÜST ORTA)
        // ======================================

        private string _deviceHealthSummaryText = "4/4 Cihaz Bağlı";
        public string DeviceHealthSummaryText
        {
            get => _deviceHealthSummaryText;
            set => SetProperty(ref _deviceHealthSummaryText, value);
        }

        private Brush _deviceHealthSummaryColor = new SolidColorBrush(Color.FromRgb(34, 197, 94));
        public Brush DeviceHealthSummaryColor
        {
            get => _deviceHealthSummaryColor;
            set => SetProperty(ref _deviceHealthSummaryColor, value);
        }

        private bool _isDeviceHealthFlyoutOpen;
        public bool IsDeviceHealthFlyoutOpen
        {
            get => _isDeviceHealthFlyoutOpen;
            set => SetProperty(ref _isDeviceHealthFlyoutOpen, value);
        }

        public ICommand ToggleDeviceHealthFlyoutCommand { get; }

        // ======================================
        // HESAPLANAN METRİKLER (SAĞ ÜST)
        // ======================================

        private float _totalFlowSccm;
        public float TotalFlowSccm
        {
            get => _totalFlowSccm;
            set
            {
                if (SetProperty(ref _totalFlowSccm, value))
                    OnPropertyChanged(nameof(TotalFlowText));
            }
        }

        public string TotalFlowText => $"{TotalFlowSccm:F1} sccm";

        private float _cylinderPpm = 10000f;
        public float CylinderPpm
        {
            get => _cylinderPpm;
            set => SetProperty(ref _cylinderPpm, value);
        }

        private float _calculatedConcentrationPpm;
        public float CalculatedConcentrationPpm
        {
            get => _calculatedConcentrationPpm;
            set
            {
                if (SetProperty(ref _calculatedConcentrationPpm, value))
                    OnPropertyChanged(nameof(CalculatedConcentrationText));
            }
        }

        public string CalculatedConcentrationText => $"{CalculatedConcentrationPpm:F1} ppm";

        private string _dilutionRatioText = "—";
        public string DilutionRatioText
        {
            get => _dilutionRatioText;
            set => SetProperty(ref _dilutionRatioText, value);
        }

        private string _stabilityStatusText = "HAZIR";
        public string StabilityStatusText
        {
            get => _stabilityStatusText;
            set => SetProperty(ref _stabilityStatusText, value);
        }

        private string _stabilityDurationText = "00:00";
        public string StabilityDurationText
        {
            get => _stabilityDurationText;
            set => SetProperty(ref _stabilityDurationText, value);
        }

        private Brush _stabilityStatusColor = new SolidColorBrush(Color.FromRgb(148, 163, 184));
        public Brush StabilityStatusColor
        {
            get => _stabilityStatusColor;
            set => SetProperty(ref _stabilityStatusColor, value);
        }

        private int _stabilitySeconds = 0;

        // ======================================
        // REÇETE (OTOMATİK SEKANS) MOTORU
        // ======================================

        public RecipeEngine RecipeEngine { get; }
        public bool IsRecipeRunning => RecipeEngine.IsRunning;
        public bool IsManualLocked => RecipeEngine.IsManualLocked;
        public string RecipeProgressText => RecipeEngine.StepProgressText;

        private bool _isRecipeModalOpen;
        public bool IsRecipeModalOpen
        {
            get => _isRecipeModalOpen;
            set => SetProperty(ref _isRecipeModalOpen, value);
        }

        public ICommand ToggleRecipeModalCommand { get; }
        public ICommand StartRecipeCommand { get; }
        public ICommand PauseRecipeCommand { get; }
        public ICommand ResumeRecipeCommand { get; }
        public ICommand CancelRecipeCommand { get; }

        // ======================================
        // OLAY VE HAREKET GÜNLÜĞÜ
        // ======================================

        public ObservableCollection<LogEntry> EventLogs => LoggingService.Instance.Entries;
        public ICollectionView FilteredLogsView { get; }

        private string _logFilterText = string.Empty;
        public string LogFilterText
        {
            get => _logFilterText;
            set
            {
                if (SetProperty(ref _logFilterText, value))
                    FilteredLogsView.Refresh();
            }
        }

        private string _selectedLogSeverity = "Tümü";
        public string SelectedLogSeverity
        {
            get => _selectedLogSeverity;
            set
            {
                if (SetProperty(ref _selectedLogSeverity, value))
                    FilteredLogsView.Refresh();
            }
        }

        public string[] LogSeverityOptions { get; } = new[] { "Tümü", "İşlemler (Hareket)", "Bilgiler", "Uyarılar", "Alarmlar" };

        public ICommand ExportLogsCommand { get; }
        public ICommand ExportFullSessionLogsCommand { get; }
        public ICommand OpenLogsFolderCommand { get; }
        public ICommand ClearUiLogsCommand { get; }
        public ICommand AcknowledgeAlarmsCommand { get; }

        // ======================================
        // TREND VERİ TAMPONLARI (12 SAAT KESİNTİSİZ GÜVENLİ TAMPON - 43.200 NOKTA)
        // ======================================

        public DateTime SessionStartTime { get; } = DateTime.Now;
        public List<TrendDataPoint> TrendHistory { get; } = new();
        public List<MfcTrendPoint> MfcHistory { get; } = new();
        public List<VovgTrendPoint> VovgHistory { get; } = new();
        private readonly object _trendLock = new();
        public event Action? OnTrendUpdated;

        public List<MfcTrendPoint> GetMfcHistorySnapshot()
        {
            lock (_trendLock)
            {
                return MfcHistory.ToList();
            }
        }

        public List<VovgTrendPoint> GetVovgHistorySnapshot()
        {
            lock (_trendLock)
            {
                return VovgHistory.ToList();
            }
        }

        // ======================================
        // VERİ TAZELİĞİ GÖSTERGESİ
        // ======================================

        private string _dataFreshnessText = "🟢 Canlı Veri · 1.0s";
        public string DataFreshnessText
        {
            get => _dataFreshnessText;
            set => SetProperty(ref _dataFreshnessText, value);
        }

        // ======================================
        // ACİL DURDUR & DİĞER KOMUTLAR
        // ======================================

        public ICommand EmergencyStopCommand { get; }
        public ICommand ConnectPlcCommand { get; }
        public ICommand ConnectVovgCommand { get; }
        public ICommand ConnectKeithleyCommand { get; }
        public ICommand ConnectDp831Command { get; }
        public ICommand ConnectGpdCommand => ConnectDp831Command;
        public ICommand CloseAllValvesCommand { get; }
        public ICommand TurnOffAllPowerOutputsCommand { get; }
        public ICommand StopAllMfcsCommand { get; }

        // ======================================
        // CONSTRUCTOR
        // ======================================

        public MainViewModel()
        {
            _plcService = new PlcService();
            _mfcManager = new MfcManager(_plcService);
            _vovgManager = new VovgManager(_plcService);
            _keysightService = new KeysightB2902bService();
            _dp831Service = new RigolDp831Service();

            // V-OVG ViewModels (3 Adet Cihaz)
            for (int i = 1; i <= PlcConfig.VovgCount; i++)
            {
                byte tempSlave = EurothermConfig.TempSlaveIds[i];
                byte flowSlave = EurothermConfig.FlowSlaveIds[i];
                VovgDevices.Add(new VovgViewModel(i, tempSlave, flowSlave, VovgSetFlowSetpoint, VovgSetTempSetpoint));
            }

            // Electrometer ViewModel (Keysight B2902B Precision SMU)
            Electrometer = new ElectrometerViewModel(
                KeysightConnect,
                KeysightSetMode,
                KeysightSetSourceVoltage,
                KeysightSetSourceCurrent,
                KeysightToggleOutput);

            // Power Supply ViewModel (RIGOL DP831)
            PowerSupply = new PowerSupplyViewModel(
                Dp831Connect,
                Dp831SetVoltage,
                Dp831SetCurrent,
                Dp831ChannelOutputToggle,
                Dp831AllOutputsToggle);

            // 8 valf oluştur (Valf 1-8: Y0.0-Y0.7)
            for (int i = 1; i <= PlcConfig.ValveCount; i++)
            {
                Valves.Add(new ValveViewModel(i, ValveOn, ValveOff));
            }

            // 4 Fırın Egzoz Tahliye Vanası oluştur (VALF 13, 14, 15, 16 - PLC Uyumlu)
            int[] auxValveNumbers = { 13, 14, 15, 16 };
            foreach (var vNum in auxValveNumbers)
            {
                AuxiliaryValves.Add(new ValveViewModel(vNum, ValveOn, ValveOff));
            }

            // 4 Güç Çıkışı oluştur (Çıkış 8-11: Y0.8-Y0.11)
            for (int i = 0; i < PlcConfig.PowerOutputCount; i++)
            {
                int outputNum = PlcConfig.PowerOutputStartNumber + i;
                PowerOutputs.Add(new PowerOutputViewModel(outputNum, PowerOutputOn, PowerOutputOff));
            }

            // 4 MFC oluştur (slave ID'ler sabit: MFC1=1, MFC2=2, MFC3=3, MFC4=6)
            int[] slaveIds = { 0, MfcConfig.Mfc1DefaultSlaveId, MfcConfig.Mfc2DefaultSlaveId,
                                  MfcConfig.Mfc3DefaultSlaveId, MfcConfig.Mfc4DefaultSlaveId };
            for (int i = 1; i <= PlcConfig.MfcCount; i++)
            {
                Mfcs.Add(new MfcViewModel(i, slaveIds[i], MfcSetSetpoint, MfcSetFlow));
            }

            // Seçili Cihaz varsayılan olarak MFC-1
            SelectedDevice = Mfcs.Count > 0 ? Mfcs[0] : null;

            // Reçete Motoru
            RecipeEngine = new RecipeEngine
            {
                OnSetMfcSetpoint = (mfcNum, sccm) => MfcSetSetpoint(mfcNum, sccm),
                OnSetMfcFlow = (mfcNum, on) => MfcSetFlow(mfcNum, on),
                OnSetVovgTemp = (vovgNum, temp) => VovgSetTempSetpoint(vovgNum, temp),
                OnSetVovgFlow = (vovgNum, sccm) => VovgSetFlowSetpoint(vovgNum, sccm),
                OnSetValve = (vNum, on) => { if (on) ValveOn(vNum); else ValveOff(vNum); },
                OnSetPowerOutput = (pNum, on) => { if (on) PowerOutputOn(pNum); else PowerOutputOff(pNum); },
                OnSetPowerSupplyChannel = (ch, v, i, on) =>
                {
                    Dp831SetVoltage(ch, v);
                    Dp831SetCurrent(ch, i);
                    Dp831ChannelOutputToggle(ch, on);
                },
                OnSetElectrometerChannel = (ch, isVoltSource, measureMode, val, comp, on) =>
                {
                    KeysightSetMode(ch, measureMode);
                    if (isVoltSource)
                        KeysightSetSourceVoltage(ch, val, comp);
                    else
                        KeysightSetSourceCurrent(ch, val, comp);
                    KeysightToggleOutput(ch, on);
                },
                OnResetToDefault = () =>
                {
                    // Reçete iptal edildiğinde tüm donanımı güvenli varsayılan değerlere çek
                    for (int i = 1; i <= PlcConfig.MfcCount; i++)
                    {
                        MfcSetSetpoint(i, 0f);
                        MfcSetFlow(i, false);
                    }
                    for (int i = 1; i <= PlcConfig.ValveCount; i++)
                    {
                        ValveOff(i);
                    }
                    foreach (var vNum in PlcConfig.AuxValveNumbers)
                    {
                        ValveOff(vNum);
                    }
                    for (int i = 0; i < PlcConfig.PowerOutputCount; i++)
                    {
                        PowerOutputOff(PlcConfig.PowerOutputStartNumber + i);
                    }
                    for (int i = 1; i <= PlcConfig.VovgCount; i++)
                    {
                        VovgSetFlowSetpoint(i, 0f);
                        VovgSetTempSetpoint(i, 20f);
                    }
                    Dp831AllOutputsToggle(false);
                    KeysightToggleOutput(1, false);
                    KeysightToggleOutput(2, false);
                },
                OnStateChanged = () =>
                {
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        OnPropertyChanged(nameof(IsRecipeRunning));
                        OnPropertyChanged(nameof(IsManualLocked));
                        OnPropertyChanged(nameof(RecipeProgressText));
                        OnPropertyChanged(nameof(SystemStateSubtitle));
                    });
                }
            };

            // Varsayılan Örnek Reçete
            var defaultRecipe = new Recipe
            {
                Name = "Standart Test Sekansı",
                Description = "V-OVG fırın ön ısıtma, N2 taşıyıcı gaz ve hedef gaz akış sekansı",
                ExecutionMode = RecipeExecutionMode.PerDeviceQueue,
                Steps = new ObservableCollection<RecipeStep>
                {
                    new RecipeStep
                    {
                        StepNumber = 1,
                        Name = "V-OVG 1 Ön Isıtma",
                        DeviceType = RecipeDeviceType.Vovg,
                        DeviceIndex = 1,
                        VovgTempC = 25f,
                        VovgFlowSccm = 100f,
                        DurationSeconds = 300
                    },
                    new RecipeStep
                    {
                        StepNumber = 2,
                        Name = "N2 Taşıyıcı Süpürme",
                        DeviceType = RecipeDeviceType.Mfc,
                        DeviceIndex = 2,
                        MfcFlowSccm = 400f,
                        MfcFlowEnabled = true,
                        DurationSeconds = 180
                    },
                    new RecipeStep
                    {
                        StepNumber = 3,
                        Name = "Fırın Giriş Valfi Açılışı",
                        DeviceType = RecipeDeviceType.Valve,
                        DeviceIndex = 5,
                        ValveIsOpen = true,
                        DurationSeconds = 180
                    },
                    new RecipeStep
                    {
                        StepNumber = 4,
                        Name = "V-OVG 1 Buharlaşma Sıcaklığı",
                        DeviceType = RecipeDeviceType.Vovg,
                        DeviceIndex = 1,
                        VovgTempC = 45f,
                        VovgFlowSccm = 100f,
                        DurationSeconds = 400
                    },
                    new RecipeStep
                    {
                        StepNumber = 5,
                        Name = "MFC-1 Hedef Gaz Karışımı",
                        DeviceType = RecipeDeviceType.Mfc,
                        DeviceIndex = 1,
                        MfcFlowSccm = 100f,
                        MfcFlowEnabled = true,
                        DurationSeconds = 300
                    }
                }
            };
            RecipeEngine.LoadRecipe(defaultRecipe);

            // Komutlar
            EmergencyStopCommand = new RelayCommand(ExecuteEmergencyStop);
            SelectDeviceCommand = new RelayCommand(p => { if (p != null) SelectedDevice = p; });
            ToggleDeviceHealthFlyoutCommand = new RelayCommand(() => IsDeviceHealthFlyoutOpen = !IsDeviceHealthFlyoutOpen);
            ToggleRecipeModalCommand = new RelayCommand(() => IsRecipeModalOpen = !IsRecipeModalOpen);
            StartRecipeCommand = new RelayCommand(() => RecipeEngine.Start());
            PauseRecipeCommand = new RelayCommand(() => RecipeEngine.Pause());
            ResumeRecipeCommand = new RelayCommand(() => RecipeEngine.Resume());
            CancelRecipeCommand = new RelayCommand(() => RecipeEngine.Cancel());
            FilteredLogsView = CollectionViewSource.GetDefaultView(EventLogs);
            FilteredLogsView.Filter = FilterLogEntry;

            ExportLogsCommand = new RelayCommand(ExecuteExportLogs);
            ExportFullSessionLogsCommand = new RelayCommand(ExecuteExportFullSessionLogs);
            OpenLogsFolderCommand = new RelayCommand(() => LoggingService.Instance.OpenLogsFolder());
            ClearUiLogsCommand = new RelayCommand(() => LoggingService.Instance.ClearUiLogs());
            AcknowledgeAlarmsCommand = new RelayCommand(() => LoggingService.Instance.AcknowledgeAllAlarms());

            ConnectPlcCommand = new RelayCommand(ExecuteConnectPlc);
            ConnectVovgCommand = new RelayCommand(ExecuteConnectVovg);
            ConnectKeithleyCommand = new RelayCommand(() => KeysightConnect(Electrometer.SelectedComPort));
            ConnectDp831Command = new RelayCommand(() => Dp831Connect(PowerSupply.SelectedComPort));
            CloseAllValvesCommand = new RelayCommand(CloseAllValves);
            TurnOffAllPowerOutputsCommand = new RelayCommand(TurnOffAllPowerOutputs);
            StopAllMfcsCommand = new RelayCommand(StopAllMfcs);

            // Başlangıç Logu
            LoggingService.Instance.Info("SİSTEM", "Gas Flow & Vapor Control System başlatıldı.");

            // PLC bağlantısı — arka planda yap, UI thread'i bloke etme
            Task.Run(() => ConnectPlc());

            // V-OVG bağlantısı — arka planda yap
            Task.Run(() => ConnectVovg());

            // RIGOL DP831 bağlantısı — arka planda otomatik dene
            Task.Run(() => Dp831Connect(PowerSupply.SelectedComPort));

            // Keysight B2902B SMU bağlantısı — arka planda otomatik bağlan
            Task.Run(() => KeysightConnect(Electrometer.SelectedComPort));

            // Timer (1 saniye) - asenkron güncelleme tetikleyici
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (_, _) => TriggerUpdateStatus();
            _timer.Start();
        }

        // ======================================
        // PLC BAĞLANTISI
        // ======================================

        private void ConnectPlc()
        {
            try
            {
                bool connected = _plcService.Connect();
                _plcInitialConnectDone = true;

                if (connected)
                {
                    LoggingService.Instance.Info("PLC", "PLC Modbus TCP bağlantısı başarıyla kuruldu.", category: "PLC");

                    // PLC bağlandı — MFC'leri de başlat (dijital mod ayarı)
                    _mfcManager.Connect();

                    // Genel gücü aç (D300=1) — röle yok, MFC'ler direkt güçlü
                    _mfcManager.SetGeneralPower(true);
                }
                else
                {
                    LoggingService.Instance.Warning("PLC", "PLC Modbus TCP bağlantısı kurulamadı.", category: "PLC");
                }

                Application.Current?.Dispatcher.Invoke(() =>
                {
                    if (connected)
                    {
                        PlcStatusText = "PLC: BAĞLI";
                        IsPlcConnected = true;
                        MfcStatusText = "MFC: PLC ÜZERİNDEN BAĞLI";
                        IsMfcConnected = true;
                    }
                    else
                    {
                        PlcStatusText = "PLC: BAĞLANTI YOK";
                        IsPlcConnected = false;
                        MfcStatusText = "MFC: PLC BAĞLANTISI YOK";
                        IsMfcConnected = false;
                    }
                });
            }
            catch (Exception e)
            {
                _plcInitialConnectDone = true;
                LoggingService.Instance.Alarm("PLC", $"PLC bağlantı hatası: {e.Message}", category: "PLC");

                Application.Current?.Dispatcher.Invoke(() =>
                {
                    PlcStatusText = "PLC: HATA";
                    IsPlcConnected = false;
                    MfcStatusText = "MFC: PLC HATASI";
                    IsMfcConnected = false;
                });
                System.Diagnostics.Debug.WriteLine($"PLC bağlantı hatası: {e.Message}");
            }
        }

        /// <summary>
        /// Manuel PLC yeniden bağlantı (buton).
        /// Arka planda çalışır. MFC bağlantısını da yeniler.
        /// </summary>
        private void ExecuteConnectPlc()
        {
            PlcStatusText = "PLC: BAĞLANIYOR...";
            MfcStatusText = "MFC: BAĞLANIYOR...";
            Task.Run(() => ConnectPlc());
        }

        // ======================================
        // V-OVG BAĞLANTISI
        // ======================================

        private void ConnectVovg()
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                if (_plcService.IsConnected)
                {
                    VovgStatusText = "V-OVG: PLC ÜZERİNDEN BAĞLI";
                    foreach (var dev in VovgDevices)
                        dev.IsConnected = true;
                }
                else
                {
                    VovgStatusText = "V-OVG: PLC BAĞLANTISI BEKLENİYOR";
                    foreach (var dev in VovgDevices)
                        dev.IsConnected = false;
                }
            });
        }

        private void ExecuteConnectVovg()
        {
            VovgStatusText = "V-OVG: BAĞLANIYOR...";
            Task.Run(() => ConnectVovg());
        }

        // ======================================
        // VALF AÇ (Async)
        // ======================================

        private void ValveOn(int valveNumber)
        {
            Task.Run(() =>
            {
                try
                {
                    _plcService.ValveOn(valveNumber);
                    var valveVm = AllValves.FirstOrDefault(v => v.ValveNumber == valveNumber);
                    string vName = valveVm != null ? $"{valveVm.FunctionalName} ({valveVm.ValveName})" : $"Valf {valveNumber}";
                    string tag = valveVm?.TechnicalTag ?? $"VALF_{valveNumber}";
                    LoggingService.Instance.Action("VALF", $"{vName} ({tag}) AÇILDI.", category: "VALF");

                    // Beklenen durumu hemen UI'da göster ve detay panelinde seç.
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        valveVm?.UpdateStatus(true);
                        SelectedDevice = valveVm;
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("VALF", $"Valf {valveNumber} açılırken hata: {e.Message}", category: "VALF");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        ShowError($"VALF_{valveNumber} açılırken hata:", e);
                    });
                }
            });
        }

        // ======================================
        // VALF KAPAT (Async)
        // ======================================

        private void ValveOff(int valveNumber)
        {
            Task.Run(() =>
            {
                try
                {
                    _plcService.ValveOff(valveNumber);
                    var valveVm = AllValves.FirstOrDefault(v => v.ValveNumber == valveNumber);
                    string vName = valveVm != null ? $"{valveVm.FunctionalName} ({valveVm.ValveName})" : $"Valf {valveNumber}";
                    string tag = valveVm?.TechnicalTag ?? $"VALF_{valveNumber}";
                    LoggingService.Instance.Action("VALF", $"{vName} ({tag}) KAPATILDI.", category: "VALF");

                    // Beklenen durumu hemen UI'da göster ve detay panelinde seç.
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        valveVm?.UpdateStatus(false);
                        SelectedDevice = valveVm;
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("VALF", $"Valf {valveNumber} kapatılırken hata: {e.Message}", category: "VALF");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        ShowError($"VALF_{valveNumber} kapatılırken hata:", e);
                    });
                }
            });
        }

        // ======================================
        // GÜÇ ÇIKIŞI AÇ (Async)
        // ======================================

        private void PowerOutputOn(int outputNumber)
        {
            Task.Run(() =>
            {
                try
                {
                    _plcService.PowerOutputOn(outputNumber);
                    LoggingService.Instance.Action("GÜÇ ÇIKIŞI", $"Güç Çıkışı #{outputNumber} (Y0.{outputNumber}) AÇILDI.", category: "GÜÇ");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var pwrVm = PowerOutputs.FirstOrDefault(p => p.OutputNumber == outputNumber);
                        pwrVm?.UpdateStatus(true);
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("GÜÇ ÇIKIŞI", $"Güç Çıkışı #{outputNumber} açılırken hata: {e.Message}", category: "GÜÇ");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        ShowError($"GÜÇ ÇIKIŞI {outputNumber} (Y0.{outputNumber}) açılırken hata:", e);
                    });
                }
            });
        }

        // ======================================
        // GÜÇ ÇIKIŞI KAPAT (Async)
        // ======================================

        private void PowerOutputOff(int outputNumber)
        {
            Task.Run(() =>
            {
                try
                {
                    _plcService.PowerOutputOff(outputNumber);
                    LoggingService.Instance.Action("GÜÇ ÇIKIŞI", $"Güç Çıkışı #{outputNumber} (Y0.{outputNumber}) KAPATILDI.", category: "GÜÇ");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var pwrVm = PowerOutputs.FirstOrDefault(p => p.OutputNumber == outputNumber);
                        pwrVm?.UpdateStatus(false);
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("GÜÇ ÇIKIŞI", $"Güç Çıkışı #{outputNumber} kapatılırken hata: {e.Message}", category: "GÜÇ");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        ShowError($"GÜÇ ÇIKIŞI {outputNumber} (Y0.{outputNumber}) kapatılırken hata:", e);
                    });
                }
            });
        }

        // ======================================
        // TÜM GÜÇ ÇIKIŞLARINI KAPAT (Async)
        // ======================================

        private void TurnOffAllPowerOutputs()
        {
            var result = MessageBox.Show(
                "Tüm güç çıkışları (8, 9, 10, 11) kapatılacak. Devam etmek istiyor musunuz?",
                "Onay",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < PlcConfig.PowerOutputCount; i++)
                    {
                        int outputNum = PlcConfig.PowerOutputStartNumber + i;
                        _plcService.PowerOutputOff(outputNum);
                    }
                    LoggingService.Instance.Action("GÜÇ ÇIKIŞI", "Tüm PLC güç çıkışları (8-11) kullanıcı tarafından kapatıldı.", category: "GÜÇ");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        foreach (var pwr in PowerOutputs)
                            pwr.UpdateStatus(false);
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("GÜÇ ÇIKIŞI", $"Güç çıkışları kapatılırken hata: {e.Message}", category: "GÜÇ");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        ShowError("Güç çıkışları kapatılırken hata:", e);
                    });
                }
            });
        }

        // ======================================
        // MFC SETPOINT (Async)
        // ======================================

        private void MfcSetSetpoint(int mfcNumber, float sccm)
        {
            Task.Run(() =>
            {
                try
                {
                    _mfcManager.SetSetpoint(mfcNumber, sccm);
                    var mfcVm = Mfcs.FirstOrDefault(m => m.MfcNumber == mfcNumber);
                    string gasName = mfcVm?.GasName ?? $"MFC-{mfcNumber}";
                    LoggingService.Instance.Action("MFC", $"MFC-{mfcNumber} ({gasName}) akış setpointi: {sccm:F1} sccm olarak ayarlandı.", category: "MFC");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        if (mfcVm != null)
                        {
                            mfcVm.Setpoint = sccm;
                            mfcVm.ErrorMessage = null;
                        }
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("MFC", $"MFC-{mfcNumber} setpoint hatası: {e.Message}", category: "MFC");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var mfcVm = Mfcs.FirstOrDefault(m => m.MfcNumber == mfcNumber);
                        if (mfcVm != null)
                            mfcVm.ErrorMessage = e.Message;
                        ShowError($"MFC-{mfcNumber} setpoint hatası:", e);
                    });
                }
            });
        }

        // ======================================
        // MFC FLOW KONTROL (Async)
        // ======================================

        private void MfcSetFlow(int mfcNumber, bool enabled)
        {
            Task.Run(() =>
            {
                try
                {
                    _mfcManager.SetFlow(mfcNumber, enabled);
                    var mfcVm = Mfcs.FirstOrDefault(m => m.MfcNumber == mfcNumber);
                    string gasName = mfcVm?.GasName ?? $"MFC-{mfcNumber}";
                    LoggingService.Instance.Action("MFC", $"MFC-{mfcNumber} ({gasName}) gaz akışı {(enabled ? "BAŞLATILDI" : "DURDURULDU")}.", category: "MFC");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        if (mfcVm != null)
                        {
                            mfcVm.FlowEnabled = enabled;
                            mfcVm.ErrorMessage = null;
                        }
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("MFC", $"MFC-{mfcNumber} flow hatası: {e.Message}", category: "MFC");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var mfcVm = Mfcs.FirstOrDefault(m => m.MfcNumber == mfcNumber);
                        if (mfcVm != null)
                            mfcVm.ErrorMessage = e.Message;
                        ShowError($"MFC-{mfcNumber} flow hatası:", e);
                    });
                }
            });
        }

        // ======================================
        // V-OVG SETPOINT (Async - Çoklu Cihaz)
        // ======================================

        private void VovgSetFlowSetpoint(int vovgNumber, float sccm)
        {
            Task.Run(() =>
            {
                try
                {
                    _vovgManager.WriteFlowSetpoint(vovgNumber, sccm);
                    LoggingService.Instance.Action("V-OVG", $"V-OVG {vovgNumber} debi setpointi: {sccm:F1} sccm olarak ayarlandı.", category: "V-OVG");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var dev = VovgDevices.FirstOrDefault(d => d.DeviceNumber == vovgNumber);
                        if (dev != null)
                        {
                            dev.FlowSetpoint = sccm;
                            dev.ErrorMessage = null;
                        }
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("V-OVG", $"V-OVG {vovgNumber} Flow setpoint hatası: {e.Message}", category: "V-OVG");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var dev = VovgDevices.FirstOrDefault(d => d.DeviceNumber == vovgNumber);
                        if (dev != null)
                            dev.ErrorMessage = e.Message;
                        ShowError($"V-OVG {vovgNumber} Flow setpoint hatası:", e);
                    });
                }
            });
        }

        private void VovgSetFlowSetpoint(float sccm) => VovgSetFlowSetpoint(1, sccm);

        private void VovgSetTempSetpoint(int vovgNumber, float celsius)
        {
            Task.Run(() =>
            {
                try
                {
                    _vovgManager.WriteTemperatureSetpoint(vovgNumber, celsius);
                    LoggingService.Instance.Action("V-OVG", $"V-OVG {vovgNumber} sıcaklık setpointi: {celsius:F1} °C olarak ayarlandı.", category: "V-OVG");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var dev = VovgDevices.FirstOrDefault(d => d.DeviceNumber == vovgNumber);
                        if (dev != null)
                        {
                            dev.TempSetpoint = celsius;
                            dev.ErrorMessage = null;
                        }
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("V-OVG", $"V-OVG {vovgNumber} Temperature setpoint hatası: {e.Message}", category: "V-OVG");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var dev = VovgDevices.FirstOrDefault(d => d.DeviceNumber == vovgNumber);
                        if (dev != null)
                            dev.ErrorMessage = e.Message;
                        ShowError($"V-OVG {vovgNumber} Temperature setpoint hatası:", e);
                    });
                }
            });
        }

        private void VovgSetTempSetpoint(float celsius) => VovgSetTempSetpoint(1, celsius);

        // ======================================
        // TÜM DURUMLARI OKU (Timer → Async)
        // ======================================

        /// <summary>
        /// Timer tarafından çağrılır. I/O işlemlerini arka plana atar.
        /// Re-entrancy koruması ile çift güncelleme engellenir.
        /// </summary>
        private void TriggerUpdateStatus()
        {
            // İlk bağlantı tamamlanmamışsa veya zaten güncelleme yapılıyorsa atla
            if (_isUpdating)
                return;

            _isUpdating = true;
            Task.Run(() =>
            {
                try
                {
                    UpdateStatusBackground();
                }
                finally
                {
                    _isUpdating = false;
                }
            });
        }

        /// <summary>
        /// Arka plan thread'inde çalışır.
        /// Tüm Modbus okumalarını yapar, sonuçları Dispatcher ile UI'a yazar.
        /// Heartbeat da burada gönderilir.
        /// </summary>
        private void UpdateStatusBackground()
        {
            // ==========================================
            // 1. PLC, VALFLER, GÜÇ ÇIKIŞLARI, MFC'LER
            // ==========================================
            if (_plcInitialConnectDone && _plcService.IsConnected)
            {
                // ---- HEARTBEAT GÖNDER ----
                _mfcManager.SendHeartbeat();

                // ---- PLC DURUM ----
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    PlcStatusText = "PLC: BAĞLI";
                    IsPlcConnected = true;
                });

            // ---- VALF DURUMLARI (Valf 1-8: D100..D107) ----
            for (int i = 1; i <= PlcConfig.ValveCount; i++)
            {
                int valveNum = i;
                try
                {
                    // D100 (komut register) oku — D200 (status) yerine.
                    // Röleler doğru çalıştığına göre komut değeri = gerçek durum.
                    // PLC'deki status network terslemesinden bağımsız çalışır.
                    ushort status = _plcService.ValveCommand(valveNum);
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var valveVm = Valves.FirstOrDefault(v => v.ValveNumber == valveNum);
                        valveVm?.UpdateStatus(status != 0);
                    });
                }
                catch (Exception e)
                {
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var valveVm = Valves.FirstOrDefault(v => v.ValveNumber == valveNum);
                        valveVm?.SetError();
                    });
                    System.Diagnostics.Debug.WriteLine($"VALF_{valveNum} durum hatası: {e.Message}");
                }
            }

            // ---- FIRIN EGZOZ TAHLİYE VALF DURUMLARI (Valf 13-16: D112..D115) ----
            foreach (var auxValve in AuxiliaryValves)
            {
                int valveNum = auxValve.ValveNumber;
                try
                {
                    ushort status = _plcService.ValveCommand(valveNum);
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        auxValve.UpdateStatus(status != 0);
                    });
                }
                catch (Exception e)
                {
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        auxValve.SetError();
                    });
                    System.Diagnostics.Debug.WriteLine($"VALF_{valveNum} durum hatası: {e.Message}");
                }
            }

            // ---- GÜÇ ÇIKIŞLARI DURUMLARI (POWER ON/OFF: Y0.8 - Y0.11) ----
            for (int i = 0; i < PlcConfig.PowerOutputCount; i++)
            {
                int outputNum = PlcConfig.PowerOutputStartNumber + i;
                try
                {
                    ushort status = _plcService.PowerOutputCommand(outputNum);
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var pwrVm = PowerOutputs.FirstOrDefault(p => p.OutputNumber == outputNum);
                        pwrVm?.UpdateStatus(status != 0);
                    });
                }
                catch (Exception e)
                {
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var pwrVm = PowerOutputs.FirstOrDefault(p => p.OutputNumber == outputNum);
                        pwrVm?.SetError();
                    });
                    System.Diagnostics.Debug.WriteLine($"GÜÇ ÇIKIŞI {outputNum} durum hatası: {e.Message}");
                }
            }

            // ---- INTERLOCK ----
            try
            {
                ushort interlock = _plcService.GetInterlock();
                if (interlock != _prevInterlock)
                {
                    if (interlock != 0)
                        LoggingService.Instance.Alarm("INTERLOCK", "Interlock güvenlik ihlali algılandı! Sistem korumaya geçti.", category: "GÜVENLİK");
                    else
                        LoggingService.Instance.Info("INTERLOCK", "Interlock güvenlik durumu normale döndü.", category: "GÜVENLİK");

                    _prevInterlock = interlock;
                }

                Application.Current?.Dispatcher.Invoke(() =>
                {
                    if (interlock != 0)
                    {
                        InterlockText = "INTERLOCK: İHLAL";
                        IsInterlockActive = true;
                    }
                    else
                    {
                        InterlockText = "INTERLOCK: NORMAL";
                        IsInterlockActive = false;
                    }
                });
            }
            catch (Exception e)
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    InterlockText = "INTERLOCK: HATA";
                });
                System.Diagnostics.Debug.WriteLine($"Interlock okuma hatası: {e.Message}");
            }

            // ---- PLC ALARM ----
            try
            {
                ushort alarm = _plcService.ReadAlarmStatus();
                if (alarm != _prevAlarm)
                {
                    if (alarm != 0)
                        LoggingService.Instance.Alarm("PLC ALARM", "PLC donanım alarmı tetiklendi!", category: "ALARM");
                    else
                        LoggingService.Instance.Info("PLC ALARM", "PLC donanım alarmı temizlendi.", category: "ALARM");

                    _prevAlarm = alarm;
                }

                Application.Current?.Dispatcher.Invoke(() =>
                {
                    if (alarm != 0)
                    {
                        AlarmText = "ALARM: AKTİF";
                        HasAlarm = true;
                    }
                    else
                    {
                        AlarmText = "";
                        HasAlarm = false;
                    }
                });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Alarm okuma hatası: {e.Message}");
            }

            // ---- WATCHDOG ----
            try
            {
                bool watchdog = _plcService.ReadWatchdogStatus();
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    IsWatchdogOk = watchdog;
                });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Watchdog okuma hatası: {e.Message}");
            }

            // PLC state machine (D455) kendi timer'larıyla döner.
            // PC buraya YAZMAZ — sadece izler.

            // ---- MFC DURUMLARI (PLC üzerinden) ----
            try
            {
                // === DİAGNOSTİK: PLC State Machine İzleme ===
                try
                {
                    ushort d455 = _plcService.ReadCommState();
                    System.Diagnostics.Debug.WriteLine($"[MFC-DIAG] D455(state)={d455}");
                }
                catch { /* diagnostic hata verirse sessizce geç */ }

                var allStatus = _mfcManager.ReadAllStatus();

                Application.Current?.Dispatcher.Invoke(() =>
                {
                    // En az bir MFC iletişimde mi?
                    bool anyOnline = false;

                    foreach (var (number, status) in allStatus)
                    {
                        var mfcVm = Mfcs.FirstOrDefault(m => m.MfcNumber == number);
                        if (mfcVm == null) continue;

                        // Her zaman veri güncelle — online olmasa bile
                        // D414/D415 register'larında veri olabilir.
                        mfcVm.UpdateFromStatus(status);

                        if (status.TryGetValue("online", out var online) && online is bool isOnline && isOnline)
                        {
                            anyOnline = true;
                        }
                        else
                        {
                            // Hata kodu varsa göster
                            if (status.TryGetValue("error_code", out var ec) && ec is int errorCode && errorCode != 0)
                            {
                                mfcVm.ErrorMessage = $"Hata kodu: {errorCode}";
                            }
                        }
                    }

                    MfcStatusText = anyOnline ? "MFC: PLC ÜZERİNDEN BAĞLI" : "MFC: İLETİŞİM BEKLENİYOR";
                    IsMfcConnected = anyOnline;
                });
            }
            catch (Exception e)
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    MfcStatusText = "MFC: OKUMA HATASI";
                });
                System.Diagnostics.Debug.WriteLine($"MFC durum okuma hatası: {e.Message}");
            }
            }
            else
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    PlcStatusText = _plcInitialConnectDone ? "PLC: BAĞLANTI KESİLDİ" : "PLC: BAĞLI DEĞİL";
                    IsPlcConnected = false;
                    MfcStatusText = "MFC: PLC BAĞLANTISI BEKLENİYOR";
                    IsMfcConnected = false;
                });
            }

            // ---- V-OVG DURUMLARI (PLC Üzerinden 3 Cihaz) ----
            if (_vovgManager.IsConnected)
            {
                try
                {
                    var allVovgStatus = _vovgManager.ReadAllDevicesStatus();

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        int totalDevices = PlcConfig.VovgCount;
                        int fullOnlineCount = 0;
                        int partialOnlineCount = 0;

                        foreach (var (devNum, vovgStatus) in allVovgStatus)
                        {
                            var devVm = VovgDevices.FirstOrDefault(d => d.DeviceNumber == devNum);
                            if (devVm == null) continue;

                            devVm.UpdateFromStatus(vovgStatus);
                            devVm.IsConnected = true;

                            bool flowOk = vovgStatus.TryGetValue("flow_online", out var fo) && fo is true;
                            bool tempOk = vovgStatus.TryGetValue("temp_online", out var to) && to is true;

                            if (flowOk && tempOk)
                                fullOnlineCount++;
                            else if (flowOk || tempOk)
                                partialOnlineCount++;
                        }

                        if (fullOnlineCount == totalDevices)
                            VovgStatusText = $"V-OVG: TÜMÜ BAĞLI ({totalDevices}/{totalDevices})";
                        else if (fullOnlineCount > 0 || partialOnlineCount > 0)
                            VovgStatusText = $"V-OVG: {fullOnlineCount}/{totalDevices} TAM, {partialOnlineCount} KISMİ";
                        else
                            VovgStatusText = "V-OVG: İLETİŞİM BEKLENİYOR";
                    });
                }
                catch (Exception e)
                {
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        VovgStatusText = "V-OVG: OKUMA HATASI";
                        foreach (var dev in VovgDevices)
                            dev.IsConnected = false;
                    });
                    System.Diagnostics.Debug.WriteLine($"V-OVG durum okuma hatası: {e.Message}");
                }
            }

            // ---- KEYSIGHT B2902B ÖLÇÜM & DURUM ----
            if (_keysightService.IsConnected)
            {
                try
                {
                    int selCh = Electrometer.SelectedChannel;
                    var activeCh = selCh == 2 ? Electrometer.Channel2 : Electrometer.Channel1;

                    // 1. Donanımın anlık çıkış durumunu sorgula (Ön panel tuşlarıyla senkron)
                    bool hwOutputOn = _keysightService.GetChannelOutput(selCh);

                    if (!_keysightService.IsConnected)
                    {
                        // Sorgulama sırasında bağlantı koptuysa (kablo çekildi)
                        Application.Current?.Dispatcher.Invoke(() =>
                        {
                            Electrometer.IsConnected = false;
                            Electrometer.StatusText = "BAĞLANTI KOPTU";
                            KeithleyStatusText = "KEYSIGHT: BAĞLANTI KOPTU";
                            Electrometer.Channel1.ResetMeasurements();
                            Electrometer.Channel2.ResetMeasurements();
                            Electrometer.NotifyTelemetryOnly();
                        });
                    }
                    else
                    {
                        double measV = double.NaN;
                        double measI = double.NaN;
                        double measR = double.NaN;

                        // YALNIZCA Çıkış AÇIK ise ölçüm sorgusu gönder!
                        // Çıkış kapalıyken ölçüm gönderilmez; bu sayede cihazın Automatic Output-ON tetiklemesi kesin olarak önlenir.
                        if (hwOutputOn)
                        {
                            var snap = _keysightService.MeasureSnapshot(
                                selCh,
                                activeCh.MeasureMode,
                                activeCh.IsVoltageSourceMode ? activeCh.VoltageSourceValue : 0f,
                                !activeCh.IsVoltageSourceMode ? activeCh.CurrentSourceValue : 0f);
                            measV = snap.Voltage;
                            measI = snap.Current;
                            measR = snap.Resistance;
                        }

                        if (!_keysightService.IsConnected)
                        {
                            Application.Current?.Dispatcher.Invoke(() =>
                            {
                                Electrometer.IsConnected = false;
                                Electrometer.StatusText = "BAĞLANTI KOPTU";
                                KeithleyStatusText = "KEYSIGHT: BAĞLANTI KOPTU";
                                Electrometer.Channel1.ResetMeasurements();
                                Electrometer.Channel2.ResetMeasurements();
                                Electrometer.NotifyTelemetryOnly();
                            });
                        }
                        else
                        {
                            Application.Current?.Dispatcher.Invoke(() =>
                            {
                                Electrometer.IsConnected = true;
                                Electrometer.StatusText = "BAĞLI";
                                KeithleyStatusText = "KEYSIGHT: BAĞLI";
                                activeCh.IsOutputOn = hwOutputOn;
                                if (hwOutputOn)
                                {
                                    activeCh.UpdateMeasurements(measV, measI, measR);
                                    Electrometer.AddSmuPoint(DateTime.Now, selCh, measV, measI, measR);
                                }
                                else
                                {
                                    activeCh.ResetMeasurements();
                                }
                                Electrometer.NotifyTelemetryOnly();
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        Electrometer.IsConnected = false;
                        Electrometer.StatusText = "BAĞLANTI KOPTU";
                        KeithleyStatusText = "KEYSIGHT: BAĞLANTI KOPTU";
                        Electrometer.Channel1.ResetMeasurements();
                        Electrometer.Channel2.ResetMeasurements();
                        Electrometer.NotifyTelemetryOnly();
                    });
                    System.Diagnostics.Debug.WriteLine($"Keysight okuma hatası: {e.Message}");
                }
            }
            else
            {
                // Cihaz bağlı değilse UI'ı güncelle
                if (Electrometer.IsConnected)
                {
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        Electrometer.IsConnected = false;
                        Electrometer.StatusText = "BAĞLANTI YOK";
                        KeithleyStatusText = "KEYSIGHT: BAĞLANTI YOK";
                        Electrometer.Channel1.ResetMeasurements();
                        Electrometer.Channel2.ResetMeasurements();
                        Electrometer.NotifyTelemetryOnly();
                    });
                }

                // Otomatik Yeniden Bağlanma Watchdog'u (Her 3 saniyede bir arka planda USB'yi tara ve bağlan)
                _keysightReconnectTicks++;
                if (_keysightReconnectTicks >= 3 && !_isKeysightConnecting)
                {
                    _keysightReconnectTicks = 0;
                    TryKeysightAutoReconnectAsync();
                }
            }

            // ---- RIGOL DP831 GÜÇ KAYNAĞI ----
            if (_dp831Service.IsConnected)
            {
                try
                {
                    var channels = _dp831Service.ReadAllChannels();

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        if (channels != null)
                        {
                            PowerSupply.UpdateFromStatus(channels);
                            PowerSupply.IsConnected = true;
                            PowerSupply.StatusText = "BAĞLI";
                            Dp831StatusText = "DP831: BAĞLI";
                        }
                        else
                        {
                            PowerSupply.IsConnected = false;
                            PowerSupply.StatusText = "BAĞLANTI KOPTU";
                            Dp831StatusText = "DP831: BAĞLANTI KOPTU";
                            if (!string.IsNullOrEmpty(_dp831Service.LastError))
                            {
                                PowerSupply.ErrorMessage = _dp831Service.LastError;
                            }
                        }
                    });
                }
                catch (Exception e)
                {
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        Dp831StatusText = "DP831: OKUMA HATASI";
                    });
                    System.Diagnostics.Debug.WriteLine($"DP831 durum okuma hatası: {e.Message}");
                }
            }
            else
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    if (PowerSupply.IsConnected)
                    {
                        PowerSupply.IsConnected = false;
                        PowerSupply.StatusText = "BAĞLI DEĞİL";
                        Dp831StatusText = "DP831: BAĞLI DEĞİL";
                    }
                });
            }

            // ---- HESAPLANAN METRİKLER, SİSTEM DURUMU VE TREND KAYDI ----
            Application.Current?.Dispatcher.Invoke(() =>
            {
                // 1. Toplam Debi (sccm)
                float totalFlow = 0f;
                foreach (var mfc in Mfcs)
                {
                    totalFlow += mfc.ActualFlow;
                }
                TotalFlowSccm = totalFlow;

                // 2. Konsantrasyon (ppm) - MFC-1 hedef gaz (veya reçetedeki hedef gaz)
                float targetFlow = Mfcs.Count > 0 ? Mfcs[0].ActualFlow : 0f;
                if (totalFlow > 0.05f)
                {
                    CalculatedConcentrationPpm = (targetFlow / totalFlow) * CylinderPpm;
                    DilutionRatioText = $"1 : {(totalFlow / Math.Max(0.01f, targetFlow)):F0}";
                }
                else
                {
                    CalculatedConcentrationPpm = 0f;
                    DilutionRatioText = "—";
                }

                // 3. Denge / Stabilite Hesabı
                bool hasFlow = Mfcs.Any(m => m.FlowEnabled && m.Setpoint > 0.5f);
                bool hasDev = Mfcs.Any(m => m.HasDeviationWarning);

                if (!hasFlow)
                {
                    _stabilitySeconds = 0;
                    StabilityStatusText = "HAZIR (BOŞTA)";
                    StabilityDurationText = "00:00";
                    StabilityStatusColor = new SolidColorBrush(Color.FromRgb(148, 163, 184)); // Nötr
                }
                else if (hasDev)
                {
                    _stabilitySeconds = 0;
                    StabilityStatusText = "SAPMA VAR";
                    StabilityDurationText = "00:00";
                    StabilityStatusColor = new SolidColorBrush(Color.FromRgb(245, 158, 11)); // Sarı
                }
                else
                {
                    _stabilitySeconds++;
                    int mins = _stabilitySeconds / 60;
                    int secs = _stabilitySeconds % 60;
                    StabilityDurationText = $"{mins:D2}:{secs:D2}";

                    if (_stabilitySeconds >= 10)
                    {
                        StabilityStatusText = "DENGEDE";
                        StabilityStatusColor = new SolidColorBrush(Color.FromRgb(34, 197, 94)); // Yeşil
                    }
                    else
                    {
                        StabilityStatusText = "DENGE KURULUYOR";
                        StabilityStatusColor = new SolidColorBrush(Color.FromRgb(245, 158, 11)); // Sarı
                    }
                }

                // 4. Cihaz Sağlığı Özeti (Konsolide Rozet)
                int connectedCount = 0;
                if (IsPlcConnected) connectedCount++;
                if (Electrometer.IsConnected) connectedCount++;
                if (PowerSupply.IsConnected) connectedCount++;
                if (VovgDevices.Any(d => d.IsConnected)) connectedCount++;

                DeviceHealthSummaryText = $"{connectedCount}/4 Cihaz Bağlı";
                DeviceHealthSummaryColor = connectedCount == 4
                    ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
                    : (connectedCount > 0 ? new SolidColorBrush(Color.FromRgb(245, 158, 11)) : new SolidColorBrush(Color.FromRgb(148, 163, 184)));

                // 5. Genel Sistem Durumu
                if (HasAlarm || IsInterlockActive)
                {
                    SystemStateText = "ALARM";
                    SystemStateColor = new SolidColorBrush(Color.FromRgb(239, 68, 68)); // Kırmızı
                    SystemStateSubtitle = HasAlarm ? "PLC Alarmı Aktif!" : "İnterlock İhlali!";
                }
                else if (RecipeEngine.IsRunning)
                {
                    SystemStateText = "REÇETE AKTİF";
                    SystemStateColor = new SolidColorBrush(Color.FromRgb(34, 197, 94)); // Yeşil
                    SystemStateSubtitle = RecipeEngine.StepProgressText;
                }
                else if (hasDev)
                {
                    SystemStateText = "UYARI";
                    SystemStateColor = new SolidColorBrush(Color.FromRgb(245, 158, 11)); // Sarı
                    SystemStateSubtitle = "Akış sapması > %2";
                }
                else if (hasFlow || Valves.Any(v => v.IsOpen) || AuxiliaryValves.Any(v => v.IsOpen))
                {
                    SystemStateText = "ÇALIŞIYOR";
                    SystemStateColor = new SolidColorBrush(Color.FromRgb(34, 197, 94)); // Yeşil
                    SystemStateSubtitle = "Gaz akışı aktif (Manuel mod)";
                }
                else
                {
                    SystemStateText = "HAZIR";
                    SystemStateColor = new SolidColorBrush(Color.FromRgb(148, 163, 184)); // Nötr
                    SystemStateSubtitle = "Sistem beklemede";
                }

                // 6. Trend Verisi Ekle (MFC, V-OVG, Sistem)
                DateTime now = DateTime.Now;
                lock (_trendLock)
                {
                    float m1Act = Mfcs.Count > 0 ? Mfcs[0].ActualFlow : 0f;
                    float m1Sp = Mfcs.Count > 0 ? Mfcs[0].Setpoint : 0f;
                    float m2Act = Mfcs.Count > 1 ? Mfcs[1].ActualFlow : 0f;
                    float m2Sp = Mfcs.Count > 1 ? Mfcs[1].Setpoint : 0f;
                    float m3Act = Mfcs.Count > 2 ? Mfcs[2].ActualFlow : 0f;
                    float m3Sp = Mfcs.Count > 2 ? Mfcs[2].Setpoint : 0f;
                    float m4Act = Mfcs.Count > 3 ? Mfcs[3].ActualFlow : 0f;
                    float m4Sp = Mfcs.Count > 3 ? Mfcs[3].Setpoint : 0f;

                    MfcHistory.Add(new MfcTrendPoint(now, m1Act, m1Sp, m2Act, m2Sp, m3Act, m3Sp, m4Act, m4Sp));
                    if (MfcHistory.Count > 43200) MfcHistory.RemoveRange(0, MfcHistory.Count - 40000);

                    float vTemp = Vovg?.TempPV ?? 0f;
                    float vTempSp = Vovg?.TempSetpoint ?? 0f;
                    float vFlow = Vovg?.FlowPV ?? 0f;
                    float vFlowSp = Vovg?.FlowSetpoint ?? 0f;

                    VovgHistory.Add(new VovgTrendPoint(now, vTemp, vTempSp, vFlow, vFlowSp));
                    if (VovgHistory.Count > 43200) VovgHistory.RemoveRange(0, VovgHistory.Count - 40000);

                    TrendHistory.Add(new TrendDataPoint(
                        now,
                        CalculatedConcentrationPpm,
                        Electrometer.MeasurementValue,
                        vTemp,
                        TotalFlowSccm));

                    if (TrendHistory.Count > 43200) // 12 saat güvenli tamponu (1 Hz)
                    {
                        TrendHistory.RemoveRange(0, TrendHistory.Count - 40000);
                    }
                }
                OnTrendUpdated?.Invoke();

                // 7. Veri Tazeliği
                DataFreshnessText = $"🟢 Canlı Veri · {DateTime.Now:HH:mm:ss}";
                LastUpdateText = $"Son güncelleme: {DateTime.Now:HH:mm:ss}";
            });
        }

        // ======================================
        // ACİL DURDUR (Async — Tüm Sistemi Kes)
        // ======================================

        public void ExecuteEmergencyStop()
        {
            // Reçeteyi durdur
            try { RecipeEngine.Cancel(); } catch { }

            LoggingService.Instance.Alarm("ACİL DURDUR", "Acil Durdur devreye alındı! Tüm valfler, debiler ve kaynaklar anında kapatılıyor.");

            Task.Run(() =>
            {
                try
                {
                    // 1. Tüm Valfleri Kapat (Valf 1-8 ve 13-16)
                    for (int i = 1; i <= PlcConfig.ValveCount; i++)
                    {
                        try { _plcService.ValveOff(i); } catch { }
                    }
                    foreach (var auxValve in AuxiliaryValves)
                    {
                        try { _plcService.ValveOff(auxValve.ValveNumber); } catch { }
                    }

                    // 2. Tüm MFC'leri durdur ve setpoint'leri 0 yap
                    try { _mfcManager.StopAll(); } catch { }
                    for (int i = 1; i <= PlcConfig.MfcCount; i++)
                    {
                        try { _mfcManager.SetSetpoint(i, 0f); } catch { }
                    }

                    // 3. Keysight Kaynağını Kapat
                    if (_keysightService.IsConnected)
                    {
                        try { _keysightService.TurnAllOutputsOff(); } catch { }
                    }

                    // 4. Rigol DP831 Tüm Çıkışları Kapat
                    if (_dp831Service.IsConnected)
                    {
                        try { _dp831Service.SetAllOutputs(false); } catch { }
                    }

                    // 5. PLC Güç Çıkışlarını Kapat
                    for (int i = 0; i < PlcConfig.PowerOutputCount; i++)
                    {
                        int outputNum = PlcConfig.PowerOutputStartNumber + i;
                        try { _plcService.PowerOutputOff(outputNum); } catch { }
                    }

                    // 6. V-OVG Fırınları Güvenli Sıcaklık/Akışa Çek
                    for (int i = 1; i <= PlcConfig.VovgCount; i++)
                    {
                        try { _vovgManager.WriteTemperatureSetpoint(i, 20f); } catch { }
                        try { _vovgManager.WriteFlowSetpoint(i, 0f); } catch { }
                    }

                    // UI Güncellemesi
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        foreach (var v in Valves) v.UpdateStatus(false);
                        foreach (var aux in AuxiliaryValves) aux.UpdateStatus(false);
                        foreach (var m in Mfcs) { m.FlowEnabled = false; m.Setpoint = 0f; }
                        foreach (var p in PowerOutputs) p.UpdateStatus(false);
                        Electrometer.IsVoltageSourceOn = false;
                        PowerSupply.IsOutputOn = false;
                        PowerSupply.Ch1IsOutputOn = false;
                        PowerSupply.Ch2IsOutputOn = false;
                        PowerSupply.Ch3IsOutputOn = false;

                        SystemStateText = "DURDURULDU";
                        SystemStateSubtitle = "Acil Durdur uygulandı. Sistem güvenli durumda.";
                        SystemStateColor = new SolidColorBrush(Color.FromRgb(239, 68, 68));
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Acil Durdur hatası: {ex.Message}");
                }
            });
        }

        private bool FilterLogEntry(object obj)
        {
            if (obj is not LogEntry entry) return false;

            // Seviye Filtresi
            if (SelectedLogSeverity == "İşlemler (Hareket)" && entry.Severity != LogSeverity.Action)
                return false;
            if (SelectedLogSeverity == "Bilgiler" && entry.Severity != LogSeverity.Info)
                return false;
            if (SelectedLogSeverity == "Uyarılar" && entry.Severity != LogSeverity.Warning)
                return false;
            if (SelectedLogSeverity == "Alarmlar" && entry.Severity != LogSeverity.Alarm)
                return false;

            // Arama Metni Filtresi
            if (!string.IsNullOrWhiteSpace(LogFilterText))
            {
                string q = LogFilterText.Trim();
                bool match = (entry.Source != null && entry.Source.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                             (entry.Category != null && entry.Category.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                             (entry.Message != null && entry.Message.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                             (entry.Details != null && entry.Details.Contains(q, StringComparison.OrdinalIgnoreCase));
                if (!match) return false;
            }

            return true;
        }

        private void ExecuteExportLogs()
        {
            var sfd = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV Dosyası (*.csv)|*.csv|Metin Dosyası (*.txt)|*.txt",
                FileName = $"GasFlow_Logs_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() == true)
            {
                bool isFiltered = !string.IsNullOrWhiteSpace(LogFilterText) || SelectedLogSeverity != "Tümü";
                bool ok;

                if (isFiltered)
                {
                    var filteredList = FilteredLogsView.Cast<LogEntry>().ToList();
                    ok = LoggingService.Instance.ExportEntriesToCsv(filteredList, sfd.FileName);
                }
                else
                {
                    ok = LoggingService.Instance.ExportFullSessionTo(sfd.FileName);
                    if (!ok)
                        ok = LoggingService.Instance.ExportToCsv(sfd.FileName);
                }

                if (ok)
                {
                    LoggingService.Instance.Action("KULLANICI", $"Olay ve hareket günlüğü kaydedildi: {Path.GetFileName(sfd.FileName)}");
                    MessageBox.Show("Olay ve hareket günlüğü başarıyla kaydedildi.", "Dışa Aktarma Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Dosya kaydedilirken bir hata oluştu.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteExportFullSessionLogs()
        {
            var sfd = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV Dosyası (*.csv)|*.csv",
                FileName = $"GasFlow_TamOturum_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() == true)
            {
                bool ok = LoggingService.Instance.ExportFullSessionTo(sfd.FileName);
                if (ok)
                {
                    LoggingService.Instance.Action("KULLANICI", $"Tam oturum günlüğü (disk) kaydedildi: {Path.GetFileName(sfd.FileName)}");
                    MessageBox.Show("Tüm oturum log geçmişi başarıyla kaydedildi.", "Tam Oturum Kaydedildi", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Oturum dosyası kaydedilirken bir hata oluştu.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ======================================
        // TÜM VALFLERİ KAPAT (Async)
        // ======================================

        private void CloseAllValves()
        {
            var result = MessageBox.Show(
                "Tüm valfler kapatılacak. Devam etmek istiyor musunuz?",
                "Onay",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            Task.Run(() =>
            {
                try
                {
                    for (int i = 1; i <= PlcConfig.ValveCount; i++)
                    {
                        _plcService.ValveOff(i);
                    }
                    foreach (var auxValve in AuxiliaryValves)
                    {
                        _plcService.ValveOff(auxValve.ValveNumber);
                    }
                    LoggingService.Instance.Action("VALF", "Tüm valfler (1-8 ve 13-16) kullanıcı komutuyla KAPATILDI.", category: "VALF");

                    // Tüm valfler kapatıldı — UI'ı hemen güncelle
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        foreach (var valve in Valves)
                            valve.UpdateStatus(false);
                        foreach (var auxValve in AuxiliaryValves)
                            auxValve.UpdateStatus(false);
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("VALF", $"Tüm valfler kapatılırken hata: {e.Message}", category: "VALF");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        ShowError("Valfler kapatılırken hata:", e);
                    });
                }
            });
        }

        // ======================================
        // TÜM MFC'LERİ DURDUR (Async)
        // ======================================

        private void StopAllMfcs()
        {
            var result = MessageBox.Show(
                "Tüm MFC'ler durdurulacak. Devam etmek istiyor musunuz?",
                "Onay",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            Task.Run(() =>
            {
                try
                {
                    _mfcManager.StopAll();
                    LoggingService.Instance.Action("MFC", "Tüm MFC akışları kullanıcı komutuyla DURDURULDU.", category: "MFC");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        foreach (var mfc in Mfcs)
                            mfc.FlowEnabled = false;
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("MFC", $"MFC'ler durdurulurken hata: {e.Message}", category: "MFC");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        ShowError("MFC'ler durdurulurken hata:", e);
                    });
                }
            });
        }

        // ======================================
        // HATA PENCERESİ
        // ======================================

        private void ShowError(string message, Exception error)
        {
            MessageBox.Show(
                $"{message}\n\n{error.Message}",
                "Sistem Hatası",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        // ======================================
        // DISPOSE
        // ======================================

        public void Dispose()
        {
            if (!_disposed)
            {
                _timer.Stop();
                RecipeEngine.Dispose();
                _mfcManager.Dispose();
                _plcService.Dispose();
                _vovgManager.Dispose();
                _keysightService.Dispose();
                _dp831Service.Dispose();
                CameraService.Instance.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        // ======================================
        // KEYSIGHT B2902B KONTROL (Async)
        // ======================================

        private void KeysightConnect(string endpoint)
        {
            if (_isKeysightConnecting) return;

            // Eğer zaten bağlıysa ve butona tekrar tıklandıysa bağlantıyı kes
            if (_keysightService.IsConnected)
            {
                _keysightService.Disconnect();
                LoggingService.Instance.Info("KEYSIGHT", "Keysight B2902B bağlantısı kesildi.", category: "SMU");
                Electrometer.IsConnected = false;
                Electrometer.StatusText = "BAĞLANTI KESİLDİ";
                KeithleyStatusText = "KEYSIGHT: BAĞLANTI YOK";
                Electrometer.MeasurementValue = double.NaN;
                return;
            }

            _isKeysightConnecting = true;
            KeithleyStatusText = "KEYSIGHT: BAĞLANIYOR...";
            Electrometer.StatusText = "BAĞLANIYOR...";

            Task.Run(() =>
            {
                try
                {
                    bool connected = _keysightService.Connect(endpoint);
                    if (connected)
                        LoggingService.Instance.Info("KEYSIGHT", $"Keysight B2902B bağlantısı kuruldu: {endpoint}", category: "SMU");
                    else
                        LoggingService.Instance.Warning("KEYSIGHT", $"Keysight B2902B bağlantısı kurulamadı: {endpoint}", category: "SMU");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        if (connected)
                        {
                            KeithleyStatusText = "KEYSIGHT: BAĞLI";
                            Electrometer.IsConnected = true;
                            Electrometer.StatusText = "BAĞLI";
                            Electrometer.ErrorMessage = null;
                        }
                        else
                        {
                            KeithleyStatusText = "KEYSIGHT: BAĞLANTI YOK";
                            Electrometer.IsConnected = false;
                            Electrometer.StatusText = "BAĞLANTI YOK";
                            Electrometer.ErrorMessage = _keysightService.LastError;
                        }
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("KEYSIGHT", $"Keysight bağlantı hatası: {e.Message}", category: "SMU");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        KeithleyStatusText = "KEYSIGHT: HATA";
                        Electrometer.IsConnected = false;
                        Electrometer.ErrorMessage = e.Message;
                    });
                }
                finally
                {
                    _isKeysightConnecting = false;
                }
            });
        }

        private void TryKeysightAutoReconnectAsync()
        {
            if (_isKeysightConnecting || _keysightService.IsConnected) return;
            _isKeysightConnecting = true;

            Task.Run(() =>
            {
                try
                {
                    string? keysightRes = KeysightB2902bService.FindKeysightUsbResource();
                    if (!string.IsNullOrEmpty(keysightRes))
                    {
                        bool ok = _keysightService.Connect(keysightRes);
                        if (ok)
                        {
                            LoggingService.Instance.Info("KEYSIGHT", $"Keysight B2902B otomatik bağlandı: {keysightRes}", category: "SMU");
                            Application.Current?.Dispatcher.Invoke(() =>
                            {
                                Electrometer.IsConnected = true;
                                Electrometer.StatusText = "BAĞLI";
                                KeithleyStatusText = "KEYSIGHT: BAĞLI";
                                Electrometer.ErrorMessage = null;
                            });
                        }
                    }
                }
                catch
                {
                    // Sessizce geç
                }
                finally
                {
                    _isKeysightConnecting = false;
                }
            });
        }

        private void KeysightSetMode(int channel, ElectrometerMeasureMode mode)
        {
            Task.Run(() =>
            {
                try
                {
                    LoggingService.Instance.Action("KEYSIGHT", $"CH{channel} çalışma modu seçildi: {mode}", category: "SMU");
                    if (_keysightService.IsConnected && _keysightService.GetChannelOutput(channel))
                    {
                        double val = _keysightService.Measure(channel, mode);
                        if (!double.IsNaN(val))
                        {
                            Application.Current?.Dispatcher.Invoke(() =>
                            {
                                Electrometer.MeasurementValue = val;
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("KEYSIGHT", $"CH{channel} mod ayar hatası: {e.Message}", category: "SMU");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        Electrometer.ErrorMessage = e.Message;
                    });
                }
            });
        }

        private void KeysightSetSourceVoltage(int channel, float voltage, float complianceCurrent)
        {
            Task.Run(() =>
            {
                try
                {
                    _keysightService.SetSourceVoltage(channel, voltage, complianceCurrent);
                    LoggingService.Instance.Action("KEYSIGHT", $"CH{channel} gerilim kaynağı: {voltage:F4} V (Akım sınırı: {complianceCurrent:G4} A) ayarlandı.", category: "SMU");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var chState = channel == 2 ? Electrometer.Channel2 : Electrometer.Channel1;
                        chState.VoltageSourceValue = voltage;
                        chState.CurrentComplianceValue = complianceCurrent;
                        Electrometer.NotifyChannelProperties();
                        Electrometer.ErrorMessage = null;
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("KEYSIGHT", $"CH{channel} gerilim kaynak ayar hatası: {e.Message}", category: "SMU");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        Electrometer.ErrorMessage = e.Message;
                    });
                }
            });
        }

        private void KeysightSetSourceCurrent(int channel, float current, float complianceVoltage)
        {
            Task.Run(() =>
            {
                try
                {
                    _keysightService.SetSourceCurrent(channel, current, complianceVoltage);
                    LoggingService.Instance.Action("KEYSIGHT", $"CH{channel} akım kaynağı: {current:G4} A (Gerilim sınırı: {complianceVoltage:F2} V) ayarlandı.", category: "SMU");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var chState = channel == 2 ? Electrometer.Channel2 : Electrometer.Channel1;
                        chState.CurrentSourceValue = current;
                        chState.VoltageComplianceValue = complianceVoltage;
                        Electrometer.NotifyChannelProperties();
                        Electrometer.ErrorMessage = null;
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("KEYSIGHT", $"CH{channel} akım kaynak ayar hatası: {e.Message}", category: "SMU");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        Electrometer.ErrorMessage = e.Message;
                    });
                }
            });
        }

        private void KeysightToggleOutput(int channel, bool on)
        {
            Task.Run(() =>
            {
                try
                {
                    _keysightService.SetChannelOutput(channel, on);
                    LoggingService.Instance.Action("KEYSIGHT", $"CH{channel} çıkışı {(on ? "AÇILDI (ON)" : "KAPATILDI (OFF)")}.", category: "SMU");

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        var chState = channel == 2 ? Electrometer.Channel2 : Electrometer.Channel1;
                        chState.IsOutputOn = on;
                        if (!on)
                        {
                            chState.ResetMeasurements();
                        }
                        Electrometer.NotifyTelemetryOnly();
                        Electrometer.ErrorMessage = null;
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("KEYSIGHT", $"CH{channel} çıkış açma/kapama hatası: {e.Message}", category: "SMU");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        Electrometer.ErrorMessage = e.Message;
                    });
                }
            });
        }

        // Geriye dönük uyumluluk forwarders
        private void KeithleyConnect(string comPort) => KeysightConnect(comPort);
        private void KeithleySetFunction(MeasureFunction func) => KeysightSetMode(Electrometer.SelectedChannel, Electrometer.CurrentMode);
        private void KeithleySetVoltageSource(float voltage) => KeysightSetSourceVoltage(Electrometer.SelectedChannel, voltage, Electrometer.CurrentComplianceValue);
        private void KeithleyToggleVoltageSource(bool on) => KeysightToggleOutput(Electrometer.SelectedChannel, on);

        // ======================================
        // RIGOL DP831 KONTROL (Async)
        // ======================================

        private void Dp831Connect(string comPort)
        {
            if (_dp831Service.IsConnected)
            {
                _dp831Service.Disconnect();
                LoggingService.Instance.Info("DP831", "RIGOL DP831 bağlantısı kesildi.", category: "GÜÇ KAYNAĞI");
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    Dp831StatusText = "DP831: BAĞLI DEĞİL";
                    PowerSupply.IsConnected = false;
                    PowerSupply.StatusText = "BAĞLI DEĞİL";
                });
                return;
            }

            Dp831StatusText = "DP831: BAĞLANIYOR...";
            Task.Run(() =>
            {
                try
                {
                    bool connected = _dp831Service.Connect(comPort);
                    if (connected)
                        LoggingService.Instance.Info("DP831", $"RIGOL DP831 bağlantısı kuruldu: {comPort}", category: "GÜÇ KAYNAĞI");
                    else
                        LoggingService.Instance.Warning("DP831", $"RIGOL DP831 bağlantısı kurulamadı: {comPort}", category: "GÜÇ KAYNAĞI");

                    (float v, float i)? ch1 = connected ? _dp831Service.GetChannelSettings(1) : null;
                    (float v, float i)? ch2 = connected ? _dp831Service.GetChannelSettings(2) : null;
                    (float v, float i)? ch3 = connected ? _dp831Service.GetChannelSettings(3) : null;
                    var initialChannels = connected ? _dp831Service.ReadAllChannels() : null;

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        if (connected)
                        {
                            Dp831StatusText = "DP831: BAĞLI";
                            PowerSupply.IsConnected = true;
                            PowerSupply.StatusText = "BAĞLI";
                            PowerSupply.ErrorMessage = null;

                            if (ch1.HasValue) PowerSupply.InitializeChannelSettings(1, ch1.Value.v, ch1.Value.i);
                            if (ch2.HasValue) PowerSupply.InitializeChannelSettings(2, ch2.Value.v, ch2.Value.i);
                            if (ch3.HasValue) PowerSupply.InitializeChannelSettings(3, ch3.Value.v, ch3.Value.i);

                            if (initialChannels != null)
                            {
                                PowerSupply.UpdateFromStatus(initialChannels);
                            }
                        }
                        else
                        {
                            Dp831StatusText = "DP831: BAĞLANTI YOK";
                            PowerSupply.IsConnected = false;
                            PowerSupply.StatusText = "BAĞLANTI YOK";
                            PowerSupply.ErrorMessage = string.IsNullOrEmpty(_dp831Service.LastError)
                                ? $"{comPort} bağlantısı kurulamadı."
                                : $"{comPort}: {_dp831Service.LastError}";
                        }
                    });
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("DP831", $"DP831 bağlantı hatası: {e.Message}", category: "GÜÇ KAYNAĞI");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        Dp831StatusText = "DP831: HATA";
                        PowerSupply.IsConnected = false;
                        PowerSupply.ErrorMessage = e.Message;
                    });
                }
            });
        }

        private void Dp831SetVoltage(int channel, float voltage)
        {
            Task.Run(() =>
            {
                try
                {
                    bool ok = _dp831Service.SetVoltage(channel, voltage);
                    if (ok)
                    {
                        LoggingService.Instance.Action("DP831", $"CH{channel} gerilim setpointi: {voltage:F3} V ayarlandı.", category: "GÜÇ KAYNAĞI");
                    }

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        if (ok)
                        {
                            PowerSupply.ErrorMessage = null;
                        }
                        else
                        {
                            PowerSupply.ErrorMessage = $"CH{channel} gerilimi ayarlanamadı ({voltage:F3}V): Cihaz komutu kabul etmedi veya bağlantı yok.";
                        }
                    });

                    if (ok)
                    {
                        var channels = _dp831Service.ReadAllChannels();
                        if (channels != null)
                        {
                            Application.Current?.Dispatcher.Invoke(() =>
                            {
                                PowerSupply.UpdateFromStatus(channels);
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("DP831", $"CH{channel} gerilim ayar hatası: {e.Message}", category: "GÜÇ KAYNAĞI");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        PowerSupply.ErrorMessage = e.Message;
                    });
                }
            });
        }

        private void Dp831SetCurrent(int channel, float current)
        {
            Task.Run(() =>
            {
                try
                {
                    bool ok = _dp831Service.SetCurrent(channel, current);
                    if (ok)
                    {
                        LoggingService.Instance.Action("DP831", $"CH{channel} akım limiti: {current:F3} A ayarlandı.", category: "GÜÇ KAYNAĞI");
                    }

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        if (ok)
                        {
                            PowerSupply.ErrorMessage = null;
                        }
                        else
                        {
                            PowerSupply.ErrorMessage = $"CH{channel} akımı ayarlanamadı ({current:F3}A): Cihaz komutu kabul etmedi veya bağlantı yok.";
                        }
                    });

                    if (ok)
                    {
                        var channels = _dp831Service.ReadAllChannels();
                        if (channels != null)
                        {
                            Application.Current?.Dispatcher.Invoke(() =>
                            {
                                PowerSupply.UpdateFromStatus(channels);
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("DP831", $"CH{channel} akım limit ayar hatası: {e.Message}", category: "GÜÇ KAYNAĞI");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        PowerSupply.ErrorMessage = e.Message;
                    });
                }
            });
        }

        private void Dp831ChannelOutputToggle(int channel, bool on)
        {
            Task.Run(() =>
            {
                try
                {
                    bool ok = _dp831Service.SetChannelOutput(channel, on);
                    if (ok)
                    {
                        LoggingService.Instance.Action("DP831", $"CH{channel} çıkışı {(on ? "AÇILDI (ON)" : "KAPATILDI (OFF)")}.", category: "GÜÇ KAYNAĞI");
                    }

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        if (ok)
                        {
                            if (channel == 1) PowerSupply.Ch1IsOutputOn = on;
                            else if (channel == 2) PowerSupply.Ch2IsOutputOn = on;
                            else if (channel == 3) PowerSupply.Ch3IsOutputOn = on;
                            PowerSupply.ErrorMessage = null;
                        }
                        else
                        {
                            PowerSupply.ErrorMessage = $"CH{channel} çıkışı {(on ? "açılamadı" : "kapatılamadı")}: Cihaz yanıt vermedi.";
                        }
                    });

                    if (ok)
                    {
                        var channels = _dp831Service.ReadAllChannels();
                        if (channels != null)
                        {
                            Application.Current?.Dispatcher.Invoke(() =>
                            {
                                PowerSupply.UpdateFromStatus(channels);
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingService.Instance.Alarm("DP831", $"CH{channel} çıkış açma/kapama hatası: {e.Message}", category: "GÜÇ KAYNAĞI");
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        PowerSupply.ErrorMessage = e.Message;
                    });
                }
            });
        }

        private void Dp831AllOutputsToggle(bool on)
        {
            Task.Run(() =>
            {
                try
                {
                    bool ok = _dp831Service.SetAllOutputs(on);
                    if (ok)
                    {
                        LoggingService.Instance.Action("DP831", $"Tüm kanal çıkışları {(on ? "AÇILDI (ON)" : "KAPATILDI (OFF)")}.", category: "GÜÇ KAYNAĞI");
                    }

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        if (ok)
                        {
                            PowerSupply.IsOutputOn = on;
                            PowerSupply.Ch1IsOutputOn = on;
                            PowerSupply.Ch2IsOutputOn = on;
                            PowerSupply.Ch3IsOutputOn = on;
                            PowerSupply.ErrorMessage = null;
                        }
                        else
                        {
                            PowerSupply.ErrorMessage = $"Tüm çıkışlar {(on ? "açılamadı" : "kapatılamadı")}: Cihaz yanıt vermedi.";
                        }
                    });

                    if (ok)
                    {
                        var channels = _dp831Service.ReadAllChannels();
                        if (channels != null)
                        {
                            Application.Current?.Dispatcher.Invoke(() =>
                            {
                                PowerSupply.UpdateFromStatus(channels);
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        PowerSupply.ErrorMessage = e.Message;
                    });
                }
            });
        }
    }
}
