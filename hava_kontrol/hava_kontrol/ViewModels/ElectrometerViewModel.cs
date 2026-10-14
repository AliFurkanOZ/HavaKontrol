using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using hava_kontrol.Models;
using hava_kontrol.Services;

namespace hava_kontrol.ViewModels
{
    /// <summary>
    /// Keysight B2902B Precision Source/Measure Unit (SMU) Kontrol Paneli ViewModel.
    /// 
    /// Yetenekler:
    ///   - USB-TMC / VISA ve Seri Port bağlantı yönetimi
    ///   - Çift Kanal (Ch1, Ch2) bağımsız durum yönetimi (ElectrometerChannelState)
    ///   - Kaynak Modları: Voltaj Kaynağı (V-Source) ve Akım Kaynağı (I-Source)
    ///   - Ölçüm modları: Akım (pA..A), Voltaj (µV..V), Direnç (mΩ..GΩ/TΩ)
    ///   - Eşzamanlı 3 metrik (V, I, R) canlı telemetri okuması
    ///   - Gerçek zamanlı V-I-R zamana bağlı grafik veri arabelleği (SmuHistory)
    ///   - Gecikmesiz çıkış kesme (:ABOR) ve Yüksek Gerilim (>42V) Güvenlik Doğrulaması
    /// </summary>
    public class ElectrometerViewModel : BaseViewModel, ISelectableDevice
    {
        private readonly Action<string> _onConnect;
        private readonly Action<int, ElectrometerMeasureMode> _onSetMode;
        private readonly Action<int, float, float> _onSetSourceVoltage;
        private readonly Action<int, float, float> _onSetSourceCurrent;
        private readonly Action<int, bool> _onToggleOutput;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private static readonly Brush ActiveGreenBrush = new SolidColorBrush(Color.FromRgb(34, 197, 94));
        private static readonly Brush InactiveSlateBrush = new SolidColorBrush(Color.FromRgb(148, 163, 184));
        private static readonly Brush WarningAmberBrush = new SolidColorBrush(Color.FromRgb(245, 158, 11));
        private static readonly Brush DangerRedBrush = new SolidColorBrush(Color.FromRgb(239, 68, 68));

        static ElectrometerViewModel()
        {
            ActiveGreenBrush.Freeze();
            InactiveSlateBrush.Freeze();
            WarningAmberBrush.Freeze();
            DangerRedBrush.Freeze();
        }

        // ======================================
        // ÇİFT KANAL BAĞIMSIZ DURUM MODELLERİ
        // ======================================

        public ElectrometerChannelState Channel1 { get; } = new(1, "KANAL 1 (Test Hücresi)");
        public ElectrometerChannelState Channel2 { get; } = new(2, "KANAL 2 (İkincil / Bias)");

        public ElectrometerChannelState ActiveChannel => SelectedChannel == 1 ? Channel1 : Channel2;

        private int _selectedChannel = 1;
        public int SelectedChannel
        {
            get => _selectedChannel;
            set
            {
                if (SetProperty(ref _selectedChannel, value))
                {
                    OnPropertyChanged(nameof(IsChannel1Selected));
                    OnPropertyChanged(nameof(IsChannel2Selected));
                    OnPropertyChanged(nameof(ChannelTitleText));
                    OnPropertyChanged(nameof(ActiveChannel));
                    NotifyChannelProperties();
                }
            }
        }

        public bool IsChannel1Selected => SelectedChannel == 1;
        public bool IsChannel2Selected => SelectedChannel == 2;
        public string ChannelTitleText => $"KANAL {SelectedChannel}";

        // ======================================
        // BAĞLANTI DURUMU
        // ======================================

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (SetProperty(ref _isConnected, value))
                {
                    OnPropertyChanged(nameof(ConnectionStatusColor));
                    OnPropertyChanged(nameof(ConnectButtonText));
                    OnPropertyChanged(nameof(MeasurementText));
                    try
                    {
                        Application.Current?.Dispatcher?.InvokeAsync(CommandManager.InvalidateRequerySuggested);
                    }
                    catch { }
                }
            }
        }

        public Brush ConnectionStatusColor => IsConnected ? ActiveGreenBrush : InactiveSlateBrush;

        private string _statusText = "BAĞLI DEĞİL";
        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        public string ConnectButtonText => IsConnected ? "BAĞLANTIYI KES" : "BAĞLAN";

        // ======================================
        // PORT / ENDPOINT SEÇİMİ
        // ======================================

        private string _selectedComPort = KeysightB2902bConfig.DefaultUsbEndpoint;
        public string SelectedComPort
        {
            get => _selectedComPort;
            set => SetProperty(ref _selectedComPort, value);
        }

        private string[] _availableComPorts = Array.Empty<string>();
        public string[] AvailableComPorts
        {
            get => _availableComPorts;
            set => SetProperty(ref _availableComPorts, value);
        }

        // ======================================
        // AKTİF KANAL İLETİLMİŞ (FORWARDED) ÖZELLİKLER
        // ======================================

        public bool IsOutputOn
        {
            get => ActiveChannel.IsOutputOn;
            set
            {
                if (ActiveChannel.IsOutputOn != value)
                {
                    ActiveChannel.IsOutputOn = value;
                    NotifyChannelProperties();
                }
            }
        }

        public bool IsVoltageSourceOn
        {
            get => IsOutputOn;
            set => IsOutputOn = value;
        }

        public string OutputStatusText => ActiveChannel.OutputStatusText;
        public Brush OutputStatusColor => ActiveChannel.IsOutputOn
            ? (HasHvWarning ? WarningAmberBrush : ActiveGreenBrush)
            : InactiveSlateBrush;

        public bool HasHvWarning => ActiveChannel.HasHvWarning;

        // Kaynak Modu
        public bool IsVoltageSourceMode
        {
            get => ActiveChannel.IsVoltageSourceMode;
            set
            {
                if (ActiveChannel.IsVoltageSourceMode != value)
                {
                    ActiveChannel.IsVoltageSourceMode = value;
                    OnPropertyChanged(nameof(IsVoltageSourceMode));
                    OnPropertyChanged(nameof(IsCurrentSourceMode));
                    OnPropertyChanged(nameof(SourceModeText));
                }
            }
        }

        public bool IsCurrentSourceMode => ActiveChannel.IsCurrentSourceMode;
        public string SourceModeText => ActiveChannel.SourceModeText;

        // Voltaj Kaynağı Değerleri
        public float VoltageSourceValue
        {
            get => ActiveChannel.VoltageSourceValue;
            set
            {
                ActiveChannel.VoltageSourceValue = value;
                OnPropertyChanged(nameof(VoltageSourceValue));
                OnPropertyChanged(nameof(VoltageSourceText));
                OnPropertyChanged(nameof(HasHvWarning));
                OnPropertyChanged(nameof(OutputStatusColor));
            }
        }

        public string VoltageSourceInput
        {
            get => ActiveChannel.VoltageSourceInput;
            set
            {
                ActiveChannel.VoltageSourceInput = value;
                OnPropertyChanged(nameof(VoltageSourceInput));
            }
        }

        public string VoltageSourceText => ActiveChannel.VoltageSourceText;

        public float CurrentComplianceValue
        {
            get => ActiveChannel.CurrentComplianceValue;
            set
            {
                ActiveChannel.CurrentComplianceValue = value;
                OnPropertyChanged(nameof(CurrentComplianceValue));
                OnPropertyChanged(nameof(CurrentComplianceText));
            }
        }

        public string CurrentComplianceInput
        {
            get => ActiveChannel.CurrentComplianceInput;
            set
            {
                ActiveChannel.CurrentComplianceInput = value;
                OnPropertyChanged(nameof(CurrentComplianceInput));
            }
        }

        public string CurrentComplianceText => ActiveChannel.CurrentComplianceText;

        // Akım Kaynağı Değerleri
        public float CurrentSourceValue
        {
            get => ActiveChannel.CurrentSourceValue;
            set
            {
                ActiveChannel.CurrentSourceValue = value;
                OnPropertyChanged(nameof(CurrentSourceValue));
                OnPropertyChanged(nameof(CurrentSourceText));
            }
        }

        public string CurrentSourceInput
        {
            get => ActiveChannel.CurrentSourceInput;
            set
            {
                ActiveChannel.CurrentSourceInput = value;
                OnPropertyChanged(nameof(CurrentSourceInput));
            }
        }

        public string CurrentSourceText => ActiveChannel.CurrentSourceText;

        public float VoltageComplianceValue
        {
            get => ActiveChannel.VoltageComplianceValue;
            set
            {
                ActiveChannel.VoltageComplianceValue = value;
                OnPropertyChanged(nameof(VoltageComplianceValue));
                OnPropertyChanged(nameof(VoltageComplianceText));
                OnPropertyChanged(nameof(HasHvWarning));
            }
        }

        public string VoltageComplianceInput
        {
            get => ActiveChannel.VoltageComplianceInput;
            set
            {
                ActiveChannel.VoltageComplianceInput = value;
                OnPropertyChanged(nameof(VoltageComplianceInput));
            }
        }

        public string VoltageComplianceText => ActiveChannel.VoltageComplianceText;

        // Ölçüm Modu ve Birimleri
        public ElectrometerMeasureMode CurrentMode
        {
            get => ActiveChannel.MeasureMode;
            set
            {
                if (ActiveChannel.MeasureMode != value)
                {
                    ActiveChannel.MeasureMode = value;
                    NotifyChannelProperties();
                }
            }
        }

        public MeasureFunction CurrentFunction
        {
            get => CurrentMode switch
            {
                ElectrometerMeasureMode.Voltage => MeasureFunction.Voltage,
                ElectrometerMeasureMode.Resistance => MeasureFunction.Charge,
                _ => MeasureFunction.Current
            };
            set
            {
                CurrentMode = value switch
                {
                    MeasureFunction.Voltage => ElectrometerMeasureMode.Voltage,
                    MeasureFunction.Charge => ElectrometerMeasureMode.Resistance,
                    _ => ElectrometerMeasureMode.Current
                };
            }
        }

        public string FunctionText => ActiveChannel.FunctionText;
        public string MeasurementUnit => CurrentMode switch
        {
            ElectrometerMeasureMode.Current => "A",
            ElectrometerMeasureMode.Voltage => "V",
            ElectrometerMeasureMode.Resistance => "Ω",
            _ => ""
        };

        public bool IsCurrentMode => ActiveChannel.IsCurrentMode;
        public bool IsVoltageMode => ActiveChannel.IsVoltageMode;
        public bool IsResistanceMode => ActiveChannel.IsResistanceMode;
        public bool IsChargeMode => false;

        // Ölçüm Değerleri
        public double MeasurementValue
        {
            get => CurrentMode switch
            {
                ElectrometerMeasureMode.Current => ActiveChannel.MeasuredCurrent,
                ElectrometerMeasureMode.Voltage => ActiveChannel.MeasuredVoltage,
                ElectrometerMeasureMode.Resistance => ActiveChannel.MeasuredResistance,
                _ => double.NaN
            };
            set
            {
                switch (CurrentMode)
                {
                    case ElectrometerMeasureMode.Current:
                        ActiveChannel.UpdateMeasurements(ActiveChannel.MeasuredVoltage, value, ActiveChannel.MeasuredResistance);
                        break;
                    case ElectrometerMeasureMode.Voltage:
                        ActiveChannel.UpdateMeasurements(value, ActiveChannel.MeasuredCurrent, ActiveChannel.MeasuredResistance);
                        break;
                    case ElectrometerMeasureMode.Resistance:
                        ActiveChannel.UpdateMeasurements(ActiveChannel.MeasuredVoltage, ActiveChannel.MeasuredCurrent, value);
                        break;
                }
                OnPropertyChanged(nameof(MeasurementValue));
                OnPropertyChanged(nameof(MeasurementText));
            }
        }

        public string MeasurementText => !IsConnected ? "---" : ActiveChannel.PrimaryMeasurementText;

        // Eşzamanlı V, I, R Canlı Metrikleri
        public double MeasuredCurrent => ActiveChannel.MeasuredCurrent;
        public double MeasuredVoltage => ActiveChannel.MeasuredVoltage;
        public double MeasuredResistance => ActiveChannel.MeasuredResistance;
        public string CurrentText => ActiveChannel.CurrentText;
        public string VoltageText => ActiveChannel.VoltageText;
        public string ResistanceText => ActiveChannel.ResistanceText;

        /// <summary>
        /// Kanal değiştiğinde veya mod değiştiğinde tüm özellikleri (kullanıcı giriş kutuları dahil) yeniler.
        /// </summary>
        public void NotifyChannelProperties()
        {
            OnPropertyChanged(nameof(IsOutputOn));
            OnPropertyChanged(nameof(IsVoltageSourceOn));
            OnPropertyChanged(nameof(OutputStatusText));
            OnPropertyChanged(nameof(OutputStatusColor));
            OnPropertyChanged(nameof(HasHvWarning));

            OnPropertyChanged(nameof(IsVoltageSourceMode));
            OnPropertyChanged(nameof(IsCurrentSourceMode));
            OnPropertyChanged(nameof(SourceModeText));

            OnPropertyChanged(nameof(VoltageSourceValue));
            OnPropertyChanged(nameof(VoltageSourceInput));
            OnPropertyChanged(nameof(VoltageSourceText));
            OnPropertyChanged(nameof(CurrentComplianceValue));
            OnPropertyChanged(nameof(CurrentComplianceInput));
            OnPropertyChanged(nameof(CurrentComplianceText));

            OnPropertyChanged(nameof(CurrentSourceValue));
            OnPropertyChanged(nameof(CurrentSourceInput));
            OnPropertyChanged(nameof(CurrentSourceText));
            OnPropertyChanged(nameof(VoltageComplianceValue));
            OnPropertyChanged(nameof(VoltageComplianceInput));
            OnPropertyChanged(nameof(VoltageComplianceText));

            OnPropertyChanged(nameof(CurrentMode));
            OnPropertyChanged(nameof(FunctionText));
            OnPropertyChanged(nameof(MeasurementUnit));
            OnPropertyChanged(nameof(IsCurrentMode));
            OnPropertyChanged(nameof(IsVoltageMode));
            OnPropertyChanged(nameof(IsResistanceMode));

            NotifyTelemetryOnly();
        }

        /// <summary>
        /// Arka plan periyodik ölçüm döngüsünde çağrılır.
        /// Kullanıcının klavyeden yazdığı giriş kutularını (TextBox) KESİNLİKLE EZMEZ, sadece canlı ölçümleri günceller.
        /// </summary>
        public void NotifyTelemetryOnly()
        {
            OnPropertyChanged(nameof(IsOutputOn));
            OnPropertyChanged(nameof(IsVoltageSourceOn));
            OnPropertyChanged(nameof(OutputStatusText));
            OnPropertyChanged(nameof(OutputStatusColor));
            OnPropertyChanged(nameof(HasHvWarning));

            OnPropertyChanged(nameof(VoltageSourceText));
            OnPropertyChanged(nameof(CurrentComplianceText));
            OnPropertyChanged(nameof(CurrentSourceText));
            OnPropertyChanged(nameof(VoltageComplianceText));

            OnPropertyChanged(nameof(MeasurementValue));
            OnPropertyChanged(nameof(MeasurementText));
            OnPropertyChanged(nameof(MeasuredCurrent));
            OnPropertyChanged(nameof(MeasuredVoltage));
            OnPropertyChanged(nameof(MeasuredResistance));
            OnPropertyChanged(nameof(CurrentText));
            OnPropertyChanged(nameof(VoltageText));
            OnPropertyChanged(nameof(ResistanceText));
        }

        public void ResetMeasurements()
        {
            Channel1.ResetMeasurements();
            Channel2.ResetMeasurements();
            NotifyTelemetryOnly();
        }

        // ======================================
        // ZAMANA BAĞLI V-I-R TREND GRAFİĞİ VERİLERİ
        // ======================================

        public List<SmuTrendPoint> SmuHistory { get; } = new();
        private readonly object _smuHistoryLock = new();
        public event Action? OnSmuTrendUpdated;

        public void AddSmuPoint(DateTime timestamp, int channel, double voltage, double current, double resistance)
        {
            lock (_smuHistoryLock)
            {
                SmuHistory.Add(new SmuTrendPoint(timestamp, channel, voltage, current, resistance, IsOutputOn));
                if (SmuHistory.Count > 43200) // 12 saat kesintisiz veri tamponu (1 Hz)
                {
                    SmuHistory.RemoveRange(0, SmuHistory.Count - 40000);
                }
            }
            OnSmuTrendUpdated?.Invoke();
        }

        public List<SmuTrendPoint> GetSmuHistorySnapshot()
        {
            lock (_smuHistoryLock)
            {
                return SmuHistory.ToList();
            }
        }

        public void ClearSmuHistory()
        {
            lock (_smuHistoryLock)
            {
                SmuHistory.Clear();
            }
            OnSmuTrendUpdated?.Invoke();
        }

        // ======================================
        // HATA YÖNETİMİ
        // ======================================

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (SetProperty(ref _errorMessage, value))
                {
                    OnPropertyChanged(nameof(HasError));
                }
            }
        }

        public bool HasError => !string.IsNullOrEmpty(_errorMessage);

        // ======================================
        // KOMUTLAR
        // ======================================

        public ICommand ConnectCommand { get; }
        public ICommand RefreshPortsCommand { get; }
        public ICommand SetCurrentFuncCommand { get; }
        public ICommand SetVoltageFuncCommand { get; }
        public ICommand SetResistanceFuncCommand { get; }
        public ICommand SetChargeFuncCommand { get; }

        public ICommand SelectChannel1Command { get; }
        public ICommand SelectChannel2Command { get; }

        public ICommand SetVoltageSourceModeCommand { get; }
        public ICommand SetCurrentSourceModeCommand { get; }

        public ICommand ApplyVoltageSourceCommand { get; }
        public ICommand ApplyCurrentSourceCommand { get; }
        public ICommand QuickSetVoltageCommand { get; }
        public ICommand QuickSetCurrentCommand { get; }
        public ICommand QuickSetComplianceCommand { get; }
        public ICommand ToggleOutputCommand { get; }
        public ICommand TurnOnOutputCommand { get; }
        public ICommand TurnOffOutputCommand { get; }
        public ICommand ToggleVoltageSourceCommand { get; }
        public ICommand ClearSmuHistoryCommand { get; }

        // ======================================
        // CONSTRUCTOR
        // ======================================

        public ElectrometerViewModel(
            Action<string> onConnect,
            Action<int, ElectrometerMeasureMode> onSetMode,
            Action<int, float, float> onSetSourceVoltage,
            Action<int, float, float> onSetSourceCurrent,
            Action<int, bool> onToggleOutput)
        {
            _onConnect = onConnect;
            _onSetMode = onSetMode;
            _onSetSourceVoltage = onSetSourceVoltage;
            _onSetSourceCurrent = onSetSourceCurrent;
            _onToggleOutput = onToggleOutput;

            ConnectCommand = new RelayCommand(DoConnect);
            RefreshPortsCommand = new RelayCommand(RefreshPorts);

            SetCurrentFuncCommand = new RelayCommand(() => SetMode(ElectrometerMeasureMode.Current));
            SetVoltageFuncCommand = new RelayCommand(() => SetMode(ElectrometerMeasureMode.Voltage));
            SetResistanceFuncCommand = new RelayCommand(() => SetMode(ElectrometerMeasureMode.Resistance));
            SetChargeFuncCommand = new RelayCommand(() => SetMode(ElectrometerMeasureMode.Resistance));

            SelectChannel1Command = new RelayCommand(() => SetChannel(1));
            SelectChannel2Command = new RelayCommand(() => SetChannel(2));

            SetVoltageSourceModeCommand = new RelayCommand(() => IsVoltageSourceMode = true);
            SetCurrentSourceModeCommand = new RelayCommand(() => IsVoltageSourceMode = false);

            ApplyVoltageSourceCommand = new RelayCommand(ApplyVoltageSource);
            ApplyCurrentSourceCommand = new RelayCommand(ApplyCurrentSource);
            QuickSetVoltageCommand = new RelayCommand<string>(QuickSetVoltage);
            QuickSetCurrentCommand = new RelayCommand<string>(QuickSetCurrent);
            QuickSetComplianceCommand = new RelayCommand<string>(QuickSetCompliance);

            ToggleOutputCommand = new RelayCommand(ToggleOutput);
            TurnOnOutputCommand = new RelayCommand(() => SetOutputState(true));
            TurnOffOutputCommand = new RelayCommand(() => SetOutputState(false));
            ToggleVoltageSourceCommand = new RelayCommand(ToggleOutput);
            ClearSmuHistoryCommand = new RelayCommand(ClearSmuHistory);

            RefreshPorts();
        }

        // ======================================
        // METOTLAR VE AKSIYONLAR
        // ======================================

        private void DoConnect()
        {
            ErrorMessage = null;
            _onConnect(SelectedComPort);
        }

        public void SetChannel(int channel)
        {
            if (channel < 1 || channel > 2) return;
            SelectedChannel = channel;
            NotifyChannelProperties();

            if (IsConnected)
            {
                try
                {
                    _onSetMode(SelectedChannel, CurrentMode);
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
        }

        public void SetMode(ElectrometerMeasureMode mode)
        {
            ErrorMessage = null;
            CurrentMode = mode;

            if (IsConnected)
            {
                try
                {
                    _onSetMode(SelectedChannel, mode);
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
        }

        public void ApplyVoltageSource()
        {
            ErrorMessage = null;

            string vInput = (VoltageSourceInput ?? string.Empty).Trim().Replace(',', '.');
            if (string.IsNullOrEmpty(vInput))
            {
                vInput = "0.0";
                VoltageSourceInput = "0.0";
            }

            if (!float.TryParse(vInput, NumberStyles.Float, CultureInfo.InvariantCulture, out float voltage))
            {
                ErrorMessage = "Geçersiz voltaj değeri. Lütfen sayısal bir değer giriniz (Örn: 5 veya 2.5).";
                return;
            }

            if (voltage < KeysightB2902bConfig.VoltageSourceMin || voltage > KeysightB2902bConfig.VoltageSourceMax)
            {
                ErrorMessage = $"Voltaj {KeysightB2902bConfig.VoltageSourceMin} ile {KeysightB2902bConfig.VoltageSourceMax} V arasında olmalıdır.";
                return;
            }

            string cInput = (CurrentComplianceInput ?? string.Empty).Trim().Replace(',', '.');
            if (string.IsNullOrEmpty(cInput))
            {
                cInput = "50";
                CurrentComplianceInput = "50";
            }

            if (!float.TryParse(cInput, NumberStyles.Float, CultureInfo.InvariantCulture, out float complianceMa) || complianceMa <= 0)
            {
                complianceMa = 50f;
                CurrentComplianceInput = "50";
            }

            float complianceAmps = complianceMa / 1000f;

            try
            {
                _onSetSourceVoltage(SelectedChannel, voltage, complianceAmps);
                VoltageSourceValue = voltage;
                CurrentComplianceValue = complianceAmps;
                IsVoltageSourceMode = true;
                NotifyTelemetryOnly();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void ApplyCurrentSource()
        {
            ErrorMessage = null;

            string iInput = (CurrentSourceInput ?? string.Empty).Trim().Replace(',', '.');
            if (string.IsNullOrEmpty(iInput))
            {
                iInput = "1.0";
                CurrentSourceInput = "1.0";
            }

            if (!float.TryParse(iInput, NumberStyles.Float, CultureInfo.InvariantCulture, out float currentMa))
            {
                ErrorMessage = "Geçersiz akım değeri. Lütfen sayısal bir değer giriniz (Örn: 1.0 veya 0.05).";
                return;
            }

            float currentAmps = currentMa / 1000f;

            if (currentAmps < KeysightB2902bConfig.CurrentSourceMin || currentAmps > KeysightB2902bConfig.CurrentSourceMax)
            {
                ErrorMessage = $"Akım {KeysightB2902bConfig.CurrentSourceMin * 1000f:F1} ile {KeysightB2902bConfig.CurrentSourceMax * 1000f:F1} mA arasında olmalıdır.";
                return;
            }

            string vCompInput = (VoltageComplianceInput ?? string.Empty).Trim().Replace(',', '.');
            if (string.IsNullOrEmpty(vCompInput))
            {
                vCompInput = "21.0";
                VoltageComplianceInput = "21.0";
            }

            if (!float.TryParse(vCompInput, NumberStyles.Float, CultureInfo.InvariantCulture, out float complianceVolts) || complianceVolts <= 0)
            {
                complianceVolts = 21.0f;
                VoltageComplianceInput = "21.0";
            }

            try
            {
                _onSetSourceCurrent(SelectedChannel, currentAmps, complianceVolts);
                CurrentSourceValue = currentAmps;
                VoltageComplianceValue = complianceVolts;
                IsVoltageSourceMode = false;
                NotifyTelemetryOnly();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void QuickSetVoltage(string? vStr)
        {
            if (string.IsNullOrWhiteSpace(vStr)) return;
            VoltageSourceInput = vStr;
            ApplyVoltageSource();
        }

        public void QuickSetCurrent(string? iStr)
        {
            if (string.IsNullOrWhiteSpace(iStr)) return;
            CurrentSourceInput = iStr;
            ApplyCurrentSource();
        }

        public void QuickSetCompliance(string? cStr)
        {
            if (string.IsNullOrWhiteSpace(cStr)) return;
            if (IsVoltageSourceMode)
            {
                CurrentComplianceInput = cStr;
                ApplyVoltageSource();
            }
            else
            {
                VoltageComplianceInput = cStr;
                ApplyCurrentSource();
            }
        }

        public void SetOutputState(bool targetState)
        {
            ErrorMessage = null;
            try
            {
                // Yüksek Gerilim Güvenlik Onayı (>42V)
                float checkVoltage = IsVoltageSourceMode ? VoltageSourceValue : VoltageComplianceValue;
                if (targetState && Math.Abs(checkVoltage) >= KeysightB2902bConfig.HighVoltageWarningThreshold)
                {
                    var res = MessageBox.Show(
                        $"DİKKAT: Keysight B2902B Kanal {SelectedChannel} kaynağı ({checkVoltage:F1} V) açılacak.\n\n" +
                        "42 V üzeri gerilimler elektrik çarpması ve hassas sensör numunelerinde hasar riski taşır.\n" +
                        "Bağlantıların ve hücre izolasyonunun güvenli olduğunu onaylıyor musunuz?",
                        "Keysight B2902B Yüksek Gerilim Uyarısı",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (res != MessageBoxResult.Yes)
                        return;
                }

                _onToggleOutput(SelectedChannel, targetState);
                IsOutputOn = targetState;
                if (!targetState)
                {
                    ActiveChannel.ResetMeasurements();
                    NotifyTelemetryOnly();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void ToggleOutput()
        {
            SetOutputState(!IsOutputOn);
        }

        public void RefreshPorts()
        {
            try
            {
                var list = new List<string>
                {
                    KeysightB2902bConfig.DefaultUsbEndpoint
                };

                var usbDevices = VisaSession.FindUsbInstruments();
                foreach (var d in usbDevices)
                {
                    list.Add(d);
                }

                var serialPorts = SerialPort.GetPortNames();
                foreach (var p in serialPorts)
                {
                    list.Add(p);
                }

                AvailableComPorts = list.Distinct().ToArray();
                if (!AvailableComPorts.Contains(SelectedComPort))
                {
                    SelectedComPort = AvailableComPorts.FirstOrDefault() ?? KeysightB2902bConfig.DefaultUsbEndpoint;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Port listesi alınamadı: {ex.Message}";
            }
        }
    }
}
