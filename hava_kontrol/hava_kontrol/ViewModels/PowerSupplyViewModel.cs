using System.Globalization;
using System.IO.Ports;
using System.Windows.Input;
using System.Windows.Media;
using hava_kontrol.Models;
using hava_kontrol.Services;

namespace hava_kontrol.ViewModels
{
    /// <summary>
    /// RIGOL DP831 3-Kanallı Programlanabilir DC Güç Kaynağı ViewModel'i.
    /// 
    /// Kanallar:
    ///   CH1: 0V - 8V / 0A - 5A (Pozitif)
    ///   CH2: 0V - +30V / 0A - 2A (Pozitif)
    ///   CH3: 0V - -30V / 0A - 2A (Negatif Polarite)
    /// </summary>
    public class PowerSupplyViewModel : BaseViewModel, ISelectableDevice
    {
        private readonly Action<string> _onConnect;
        private readonly Action<int, float> _onSetVoltage;
        private readonly Action<int, float> _onSetCurrent;
        private readonly Action<int, bool> _onChannelOutputToggle;
        private readonly Action<bool> _onAllOutputsToggle;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private static readonly Brush ActiveGreenBrush = new SolidColorBrush(Color.FromRgb(34, 197, 94));
        private static readonly Brush InactiveGrayBrush = new SolidColorBrush(Color.FromRgb(156, 163, 175));
        private static readonly Brush ActiveBgBrush = new SolidColorBrush(Color.FromArgb(45, 34, 197, 94));
        private static readonly Brush InactiveBgBrush = new SolidColorBrush(Color.FromArgb(15, 255, 255, 255));
        private static readonly Brush ActiveBorderBrush = new SolidColorBrush(Color.FromRgb(34, 197, 94));
        private static readonly Brush InactiveBorderBrush = new SolidColorBrush(Color.FromRgb(71, 85, 105));

        static PowerSupplyViewModel()
        {
            ActiveGreenBrush.Freeze();
            InactiveGrayBrush.Freeze();
            ActiveBgBrush.Freeze();
            InactiveBgBrush.Freeze();
            ActiveBorderBrush.Freeze();
            InactiveBorderBrush.Freeze();
        }

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
                    try
                    {
                        System.Windows.Application.Current?.Dispatcher?.InvokeAsync(CommandManager.InvalidateRequerySuggested);
                    }
                    catch { }
                }
            }
        }

        public string ConnectButtonText => IsConnected ? "BAĞLANTIYI KES" : "BAĞLAN";

        public Brush ConnectionStatusColor => IsConnected
            ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
            : new SolidColorBrush(Color.FromRgb(148, 163, 184));

        private string _statusText = "BAĞLI DEĞİL";
        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        private string _selectedComPort = RigolDp831Config.DefaultComPort;
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
        // GENEL (MASTER) ÇIKIŞ DURUMU
        // ======================================

        private bool _isOutputOn;
        public bool IsOutputOn
        {
            get => _isOutputOn;
            set
            {
                if (SetProperty(ref _isOutputOn, value))
                {
                    OnPropertyChanged(nameof(OutputStatusText));
                    OnPropertyChanged(nameof(OutputStatusColor));
                }
            }
        }

        public string OutputStatusText => IsOutputOn ? "AÇIK (ON)" : "KAPALI (OFF)";

        public Brush OutputStatusColor => IsOutputOn
            ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
            : new SolidColorBrush(Color.FromRgb(148, 163, 184));

        // ======================================
        // KANAL 1 (8V / 5A)
        // ======================================

        private bool _ch1IsOutputOn;
        public bool Ch1IsOutputOn
        {
            get => _ch1IsOutputOn;
            set
            {
                if (SetProperty(ref _ch1IsOutputOn, value))
                {
                    OnPropertyChanged(nameof(Ch1StatusColor));
                    OnPropertyChanged(nameof(Ch1OutputStatusText));
                    OnPropertyChanged(nameof(Ch1OutputBgBrush));
                    OnPropertyChanged(nameof(Ch1OutputBorderBrush));
                    IsOutputOn = _ch1IsOutputOn || _ch2IsOutputOn || _ch3IsOutputOn;
                }
            }
        }

        public string Ch1OutputStatusText => Ch1IsOutputOn ? "AÇIK" : "KAPALI";

        public Brush Ch1StatusColor => Ch1IsOutputOn ? ActiveGreenBrush : InactiveGrayBrush;
        public Brush Ch1OutputBgBrush => Ch1IsOutputOn ? ActiveBgBrush : InactiveBgBrush;
        public Brush Ch1OutputBorderBrush => Ch1IsOutputOn ? ActiveBorderBrush : InactiveBorderBrush;

        private float _ch1VoltageActual;
        public float Ch1VoltageActual
        {
            get => _ch1VoltageActual;
            set
            {
                if (SetProperty(ref _ch1VoltageActual, value))
                {
                    OnPropertyChanged(nameof(Ch1VoltageActualText));
                    OnPropertyChanged(nameof(Ch1MeasuredVoltageText));
                }
            }
        }

        public string Ch1VoltageActualText => $"{Ch1VoltageActual:F3} V";

        private float _ch1CurrentActual;
        public float Ch1CurrentActual
        {
            get => _ch1CurrentActual;
            set
            {
                if (SetProperty(ref _ch1CurrentActual, value))
                {
                    OnPropertyChanged(nameof(Ch1CurrentActualText));
                    OnPropertyChanged(nameof(Ch1MeasuredVoltageText));
                }
            }
        }

        public string Ch1CurrentActualText => $"{Ch1CurrentActual:F3} A";

        private float _ch1PowerActual;
        public float Ch1PowerActual
        {
            get => _ch1PowerActual;
            set
            {
                if (SetProperty(ref _ch1PowerActual, value))
                    OnPropertyChanged(nameof(Ch1PowerActualText));
            }
        }

        public string Ch1PowerActualText => $"{Ch1PowerActual:F2} W";

        private string _ch1VoltageInput = "0.00";
        public string Ch1VoltageInput
        {
            get => _ch1VoltageInput;
            set => SetProperty(ref _ch1VoltageInput, value);
        }

        private string _ch1CurrentInput = "1.00";
        public string Ch1CurrentInput
        {
            get => _ch1CurrentInput;
            set => SetProperty(ref _ch1CurrentInput, value);
        }

        private float _ch1VoltageSet;
        public float Ch1VoltageSet
        {
            get => _ch1VoltageSet;
            set
            {
                if (SetProperty(ref _ch1VoltageSet, value))
                {
                    OnPropertyChanged(nameof(Ch1SetSummary));
                    OnPropertyChanged(nameof(Ch1SettingVoltageText));
                }
            }
        }

        private float _ch1CurrentSet = 1.0f;
        public float Ch1CurrentSet
        {
            get => _ch1CurrentSet;
            set
            {
                if (SetProperty(ref _ch1CurrentSet, value))
                {
                    OnPropertyChanged(nameof(Ch1SetSummary));
                    OnPropertyChanged(nameof(Ch1SettingVoltageText));
                }
            }
        }

        public string Ch1SetSummary => $"Ayar: {Ch1VoltageSet:F3}V / {Ch1CurrentSet:F3}A";

        // Geriye dönük ve görünüm alias'ları
        public string Ch1MeasuredVoltageText => $"{Ch1VoltageActual:F2} V ({Ch1CurrentActual:F3} A)";
        public string Ch1SettingVoltageText => $"{Ch1VoltageSet:F2} V / {Ch1CurrentSet:F3} A";
        public ICommand Ch1ToggleOutputCommand => ToggleCh1OutputCommand;

        // ======================================
        // KANAL 2 (+30V / 2A)
        // ======================================

        private bool _ch2IsOutputOn;
        public bool Ch2IsOutputOn
        {
            get => _ch2IsOutputOn;
            set
            {
                if (SetProperty(ref _ch2IsOutputOn, value))
                {
                    OnPropertyChanged(nameof(Ch2StatusColor));
                    OnPropertyChanged(nameof(Ch2OutputStatusText));
                    OnPropertyChanged(nameof(Ch2OutputBgBrush));
                    OnPropertyChanged(nameof(Ch2OutputBorderBrush));
                    IsOutputOn = _ch1IsOutputOn || _ch2IsOutputOn || _ch3IsOutputOn;
                }
            }
        }

        public string Ch2OutputStatusText => Ch2IsOutputOn ? "AÇIK" : "KAPALI";

        public Brush Ch2StatusColor => Ch2IsOutputOn ? ActiveGreenBrush : InactiveGrayBrush;
        public Brush Ch2OutputBgBrush => Ch2IsOutputOn ? ActiveBgBrush : InactiveBgBrush;
        public Brush Ch2OutputBorderBrush => Ch2IsOutputOn ? ActiveBorderBrush : InactiveBorderBrush;

        private float _ch2VoltageActual;
        public float Ch2VoltageActual
        {
            get => _ch2VoltageActual;
            set
            {
                if (SetProperty(ref _ch2VoltageActual, value))
                {
                    OnPropertyChanged(nameof(Ch2VoltageActualText));
                    OnPropertyChanged(nameof(Ch2MeasuredVoltageText));
                }
            }
        }

        public string Ch2VoltageActualText => $"{Ch2VoltageActual:F2} V";

        private float _ch2CurrentActual;
        public float Ch2CurrentActual
        {
            get => _ch2CurrentActual;
            set
            {
                if (SetProperty(ref _ch2CurrentActual, value))
                {
                    OnPropertyChanged(nameof(Ch2CurrentActualText));
                    OnPropertyChanged(nameof(Ch2MeasuredVoltageText));
                }
            }
        }

        public string Ch2CurrentActualText => $"{Ch2CurrentActual:F3} A";

        private float _ch2PowerActual;
        public float Ch2PowerActual
        {
            get => _ch2PowerActual;
            set
            {
                if (SetProperty(ref _ch2PowerActual, value))
                    OnPropertyChanged(nameof(Ch2PowerActualText));
            }
        }

        public string Ch2PowerActualText => $"{Ch2PowerActual:F2} W";

        private string _ch2VoltageInput = "0.00";
        public string Ch2VoltageInput
        {
            get => _ch2VoltageInput;
            set => SetProperty(ref _ch2VoltageInput, value);
        }

        private string _ch2CurrentInput = "1.00";
        public string Ch2CurrentInput
        {
            get => _ch2CurrentInput;
            set => SetProperty(ref _ch2CurrentInput, value);
        }

        private float _ch2VoltageSet;
        public float Ch2VoltageSet
        {
            get => _ch2VoltageSet;
            set
            {
                if (SetProperty(ref _ch2VoltageSet, value))
                {
                    OnPropertyChanged(nameof(Ch2SetSummary));
                    OnPropertyChanged(nameof(Ch2SettingVoltageText));
                }
            }
        }

        private float _ch2CurrentSet = 1.0f;
        public float Ch2CurrentSet
        {
            get => _ch2CurrentSet;
            set
            {
                if (SetProperty(ref _ch2CurrentSet, value))
                {
                    OnPropertyChanged(nameof(Ch2SetSummary));
                    OnPropertyChanged(nameof(Ch2SettingVoltageText));
                }
            }
        }

        public string Ch2SetSummary => $"Ayar: {Ch2VoltageSet:F2}V / {Ch2CurrentSet:F3}A";

        // Geriye dönük ve görünüm alias'ları
        public string Ch2MeasuredVoltageText => $"{Ch2VoltageActual:F2} V ({Ch2CurrentActual:F3} A)";
        public string Ch2SettingVoltageText => $"{Ch2VoltageSet:F2} V / {Ch2CurrentSet:F3} A";
        public ICommand Ch2ToggleOutputCommand => ToggleCh2OutputCommand;

        // ======================================
        // KANAL 3 (-30V / 2A - NEGATİF)
        // ======================================

        private bool _ch3IsOutputOn;
        public bool Ch3IsOutputOn
        {
            get => _ch3IsOutputOn;
            set
            {
                if (SetProperty(ref _ch3IsOutputOn, value))
                {
                    OnPropertyChanged(nameof(Ch3StatusColor));
                    OnPropertyChanged(nameof(Ch3OutputStatusText));
                    OnPropertyChanged(nameof(Ch3OutputBgBrush));
                    OnPropertyChanged(nameof(Ch3OutputBorderBrush));
                    IsOutputOn = _ch1IsOutputOn || _ch2IsOutputOn || _ch3IsOutputOn;
                }
            }
        }

        public string Ch3OutputStatusText => Ch3IsOutputOn ? "AÇIK" : "KAPALI";

        public Brush Ch3StatusColor => Ch3IsOutputOn ? ActiveGreenBrush : InactiveGrayBrush;
        public Brush Ch3OutputBgBrush => Ch3IsOutputOn ? ActiveBgBrush : InactiveBgBrush;
        public Brush Ch3OutputBorderBrush => Ch3IsOutputOn ? ActiveBorderBrush : InactiveBorderBrush;

        private float _ch3VoltageActual;
        public float Ch3VoltageActual
        {
            get => _ch3VoltageActual;
            set
            {
                if (SetProperty(ref _ch3VoltageActual, value))
                {
                    OnPropertyChanged(nameof(Ch3VoltageActualText));
                    OnPropertyChanged(nameof(Ch3MeasuredVoltageText));
                }
            }
        }

        public string Ch3VoltageActualText => $"{Ch3VoltageActual:F2} V";

        private float _ch3CurrentActual;
        public float Ch3CurrentActual
        {
            get => _ch3CurrentActual;
            set
            {
                if (SetProperty(ref _ch3CurrentActual, value))
                {
                    OnPropertyChanged(nameof(Ch3CurrentActualText));
                    OnPropertyChanged(nameof(Ch3MeasuredVoltageText));
                }
            }
        }

        public string Ch3CurrentActualText => $"{Ch3CurrentActual:F3} A";

        private float _ch3PowerActual;
        public float Ch3PowerActual
        {
            get => _ch3PowerActual;
            set
            {
                if (SetProperty(ref _ch3PowerActual, value))
                    OnPropertyChanged(nameof(Ch3PowerActualText));
            }
        }

        public string Ch3PowerActualText => $"{Ch3PowerActual:F2} W";

        private string _ch3VoltageInput = "0.00";
        public string Ch3VoltageInput
        {
            get => _ch3VoltageInput;
            set => SetProperty(ref _ch3VoltageInput, value);
        }

        private string _ch3CurrentInput = "1.00";
        public string Ch3CurrentInput
        {
            get => _ch3CurrentInput;
            set => SetProperty(ref _ch3CurrentInput, value);
        }

        private float _ch3VoltageSet;
        public float Ch3VoltageSet
        {
            get => _ch3VoltageSet;
            set
            {
                if (SetProperty(ref _ch3VoltageSet, value))
                {
                    OnPropertyChanged(nameof(Ch3SetSummary));
                    OnPropertyChanged(nameof(Ch3SettingVoltageText));
                }
            }
        }

        private float _ch3CurrentSet = 1.0f;
        public float Ch3CurrentSet
        {
            get => _ch3CurrentSet;
            set
            {
                if (SetProperty(ref _ch3CurrentSet, value))
                {
                    OnPropertyChanged(nameof(Ch3SetSummary));
                    OnPropertyChanged(nameof(Ch3SettingVoltageText));
                }
            }
        }

        public string Ch3SetSummary => $"Ayar: {Ch3VoltageSet:F2}V / {Ch3CurrentSet:F3}A";

        // Geriye dönük ve görünüm alias'ları
        public string Ch3MeasuredVoltageText => $"{Ch3VoltageActual:F2} V ({Ch3CurrentActual:F3} A)";
        public string Ch3SettingVoltageText => $"{Ch3VoltageSet:F2} V / {Ch3CurrentSet:F3} A";
        public ICommand Ch3ToggleOutputCommand => ToggleCh3OutputCommand;

        public void InitializeChannelSettings(int channel, float volt, float curr)
        {
            if (channel == 1)
            {
                Ch1VoltageSet = volt;
                Ch1CurrentSet = curr;
                Ch1VoltageInput = volt.ToString("F3", CultureInfo.InvariantCulture);
                Ch1CurrentInput = curr.ToString("F3", CultureInfo.InvariantCulture);
            }
            else if (channel == 2)
            {
                Ch2VoltageSet = volt;
                Ch2CurrentSet = curr;
                Ch2VoltageInput = volt.ToString("F2", CultureInfo.InvariantCulture);
                Ch2CurrentInput = curr.ToString("F3", CultureInfo.InvariantCulture);
            }
            else if (channel == 3)
            {
                Ch3VoltageSet = volt;
                Ch3CurrentSet = curr;
                Ch3VoltageInput = volt.ToString("F2", CultureInfo.InvariantCulture);
                Ch3CurrentInput = curr.ToString("F3", CultureInfo.InvariantCulture);
            }
        }

        // ======================================
        // HATA MESAJI
        // ======================================

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (SetProperty(ref _errorMessage, value))
                    OnPropertyChanged(nameof(HasError));
            }
        }

        public bool HasError => !string.IsNullOrEmpty(_errorMessage);

        // ======================================
        // KOMUTLAR
        // ======================================

        public ICommand ConnectCommand { get; }
        public ICommand RefreshComPortsCommand { get; }
        public ICommand ToggleAllOutputsCommand { get; }
        public ICommand AllOutputsOnCommand { get; }
        public ICommand AllOutputsOffCommand { get; }

        public ICommand ToggleCh1OutputCommand { get; }
        public ICommand ApplyCh1VoltageCommand { get; }
        public ICommand ApplyCh1CurrentCommand { get; }
        public ICommand QuickSetCh1VoltageCommand { get; }
        public ICommand QuickSetCh1CurrentCommand { get; }

        public ICommand ToggleCh2OutputCommand { get; }
        public ICommand ApplyCh2VoltageCommand { get; }
        public ICommand ApplyCh2CurrentCommand { get; }
        public ICommand QuickSetCh2VoltageCommand { get; }
        public ICommand QuickSetCh2CurrentCommand { get; }

        public ICommand ToggleCh3OutputCommand { get; }
        public ICommand ApplyCh3VoltageCommand { get; }
        public ICommand ApplyCh3CurrentCommand { get; }
        public ICommand QuickSetCh3VoltageCommand { get; }
        public ICommand QuickSetCh3CurrentCommand { get; }

        public PowerSupplyViewModel(
            Action<string> onConnect,
            Action<int, float> onSetVoltage,
            Action<int, float> onSetCurrent,
            Action<int, bool> onChannelOutputToggle,
            Action<bool> onAllOutputsToggle)
        {
            _onConnect = onConnect;
            _onSetVoltage = onSetVoltage;
            _onSetCurrent = onSetCurrent;
            _onChannelOutputToggle = onChannelOutputToggle;
            _onAllOutputsToggle = onAllOutputsToggle;

            RefreshComPorts();

            RefreshComPortsCommand = new RelayCommand(RefreshComPorts);
            ConnectCommand = new RelayCommand(() => _onConnect(SelectedComPort));
            ToggleAllOutputsCommand = new RelayCommand(ToggleAllOutputs);
            AllOutputsOnCommand = new RelayCommand(() => { if (!EnsureConnected()) return; _onAllOutputsToggle(true); });
            AllOutputsOffCommand = new RelayCommand(() => { if (!EnsureConnected()) return; _onAllOutputsToggle(false); });

            // CH1
            ToggleCh1OutputCommand = new RelayCommand(() => { if (!EnsureConnected()) return; _onChannelOutputToggle(1, !Ch1IsOutputOn); });
            ApplyCh1VoltageCommand = new RelayCommand(ApplyCh1Voltage);
            ApplyCh1CurrentCommand = new RelayCommand(ApplyCh1Current);
            QuickSetCh1VoltageCommand = new RelayCommand(p => QuickSetVoltage(1, p));
            QuickSetCh1CurrentCommand = new RelayCommand(p => QuickSetCurrent(1, p));

            // CH2
            ToggleCh2OutputCommand = new RelayCommand(() => { if (!EnsureConnected()) return; _onChannelOutputToggle(2, !Ch2IsOutputOn); });
            ApplyCh2VoltageCommand = new RelayCommand(ApplyCh2Voltage);
            ApplyCh2CurrentCommand = new RelayCommand(ApplyCh2Current);
            QuickSetCh2VoltageCommand = new RelayCommand(p => QuickSetVoltage(2, p));
            QuickSetCh2CurrentCommand = new RelayCommand(p => QuickSetCurrent(2, p));

            // CH3
            ToggleCh3OutputCommand = new RelayCommand(() => { if (!EnsureConnected()) return; _onChannelOutputToggle(3, !Ch3IsOutputOn); });
            ApplyCh3VoltageCommand = new RelayCommand(ApplyCh3Voltage);
            ApplyCh3CurrentCommand = new RelayCommand(ApplyCh3Current);
            QuickSetCh3VoltageCommand = new RelayCommand(p => QuickSetVoltage(3, p));
            QuickSetCh3CurrentCommand = new RelayCommand(p => QuickSetCurrent(3, p));
        }

        private void ToggleAllOutputs()
        {
            if (!EnsureConnected()) return;
            _onAllOutputsToggle(!IsOutputOn);
        }

        private bool EnsureConnected()
        {
            ErrorMessage = null;
            if (!IsConnected)
            {
                ErrorMessage = "Rigol DP831 bağlı değil. Lütfen önce 'BAĞLAN' butonuna tıklayın.";
                return false;
            }
            return true;
        }

        public void RefreshComPorts()
        {
            try
            {
                var list = new List<string>();

                // 1. USB-TMC (VISA) cihazlarını tara
                var usbDevices = VisaSession.FindUsbInstruments();
                foreach (var dev in usbDevices)
                {
                    list.Add(dev);
                }

                // USB Otomatik seçeneği (kullanıcı kabloyu takar takmaz direkt bağlanabilir)
                list.Add("USB (Otomatik)");

                // 2. Seri Portları (COM) ekle
                var ports = SerialPort.GetPortNames();
                foreach (var p in ports)
                {
                    list.Add(p);
                }

                AvailableComPorts = list.ToArray();

                if (usbDevices.Count > 0)
                {
                    SelectedComPort = usbDevices[0];
                }
                else if (!AvailableComPorts.Contains(SelectedComPort) && AvailableComPorts.Length > 0)
                {
                    SelectedComPort = AvailableComPorts[0];
                }
            }
            catch
            {
                AvailableComPorts = new[] { "USB (Otomatik)", RigolDp831Config.DefaultComPort };
                SelectedComPort = AvailableComPorts[0];
            }
        }

        private void QuickSetVoltage(int channel, object? param)
        {
            if (param == null) return;
            if (float.TryParse(param.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
            {
                if (channel == 1)
                {
                    Ch1VoltageInput = val.ToString("F2", CultureInfo.InvariantCulture);
                    ApplyCh1Voltage();
                }
                else if (channel == 2)
                {
                    Ch2VoltageInput = val.ToString("F2", CultureInfo.InvariantCulture);
                    ApplyCh2Voltage();
                }
                else if (channel == 3)
                {
                    Ch3VoltageInput = val.ToString("F2", CultureInfo.InvariantCulture);
                    ApplyCh3Voltage();
                }
            }
        }

        private void QuickSetCurrent(int channel, object? param)
        {
            if (param == null) return;
            if (float.TryParse(param.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
            {
                if (channel == 1)
                {
                    Ch1CurrentInput = val.ToString("F2", CultureInfo.InvariantCulture);
                    ApplyCh1Current();
                }
                else if (channel == 2)
                {
                    Ch2CurrentInput = val.ToString("F2", CultureInfo.InvariantCulture);
                    ApplyCh2Current();
                }
                else if (channel == 3)
                {
                    Ch3CurrentInput = val.ToString("F2", CultureInfo.InvariantCulture);
                    ApplyCh3Current();
                }
            }
        }

        private void ApplyCh1Voltage()
        {
            ErrorMessage = null;
            string input = (Ch1VoltageInput ?? string.Empty).Trim().Replace(',', '.');
            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out float v))
            {
                ErrorMessage = "CH1 geçersiz gerilim değeri.";
                return;
            }

            if (v < RigolDp831Config.Ch1MinVoltage || v > RigolDp831Config.Ch1MaxVoltage)
            {
                ErrorMessage = $"CH1 gerilim {RigolDp831Config.Ch1MinVoltage}V - {RigolDp831Config.Ch1MaxVoltage}V arasında olmalıdır.";
                return;
            }

            Ch1VoltageSet = v;
            Ch1VoltageInput = v.ToString("F2", CultureInfo.InvariantCulture);

            if (IsConnected)
            {
                _onSetVoltage(1, v);
            }
        }

        private void ApplyCh1Current()
        {
            ErrorMessage = null;
            string input = (Ch1CurrentInput ?? string.Empty).Trim().Replace(',', '.');
            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out float i))
            {
                ErrorMessage = "CH1 geçersiz akım değeri.";
                return;
            }

            if (i < RigolDp831Config.Ch1MinCurrent || i > RigolDp831Config.Ch1MaxCurrent)
            {
                ErrorMessage = $"CH1 akım {RigolDp831Config.Ch1MinCurrent}A - {RigolDp831Config.Ch1MaxCurrent}A arasında olmalıdır.";
                return;
            }

            Ch1CurrentSet = i;
            Ch1CurrentInput = i.ToString("F2", CultureInfo.InvariantCulture);

            if (IsConnected)
            {
                _onSetCurrent(1, i);
            }
        }

        private void ApplyCh2Voltage()
        {
            ErrorMessage = null;
            string input = (Ch2VoltageInput ?? string.Empty).Trim().Replace(',', '.');
            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out float v))
            {
                ErrorMessage = "CH2 geçersiz gerilim değeri.";
                return;
            }

            if (v < RigolDp831Config.Ch2MinVoltage || v > RigolDp831Config.Ch2MaxVoltage)
            {
                ErrorMessage = $"CH2 gerilim {RigolDp831Config.Ch2MinVoltage}V - {RigolDp831Config.Ch2MaxVoltage}V arasında olmalıdır.";
                return;
            }

            Ch2VoltageSet = v;
            Ch2VoltageInput = v.ToString("F2", CultureInfo.InvariantCulture);

            if (IsConnected)
            {
                _onSetVoltage(2, v);
            }
        }

        private void ApplyCh2Current()
        {
            ErrorMessage = null;
            string input = (Ch2CurrentInput ?? string.Empty).Trim().Replace(',', '.');
            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out float i))
            {
                ErrorMessage = "CH2 geçersiz akım değeri.";
                return;
            }

            if (i < RigolDp831Config.Ch2MinCurrent || i > RigolDp831Config.Ch2MaxCurrent)
            {
                ErrorMessage = $"CH2 akım {RigolDp831Config.Ch2MinCurrent}A - {RigolDp831Config.Ch2MaxCurrent}A arasında olmalıdır.";
                return;
            }

            Ch2CurrentSet = i;
            Ch2CurrentInput = i.ToString("F2", CultureInfo.InvariantCulture);

            if (IsConnected)
            {
                _onSetCurrent(2, i);
            }
        }

        private void ApplyCh3Voltage()
        {
            ErrorMessage = null;
            string input = (Ch3VoltageInput ?? string.Empty).Trim().Replace(',', '.');
            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out float v))
            {
                ErrorMessage = "CH3 geçersiz gerilim değeri.";
                return;
            }

            // CH3 negatif voltaj kanalıdır (-30V .. 0V).
            // Kullanıcı pozitif girdiyse (örn: 15), otomatik olarak -15V yapılır.
            if (v > 0)
            {
                v = -v;
            }

            if (v < RigolDp831Config.Ch3MinVoltage || v > RigolDp831Config.Ch3MaxVoltage)
            {
                ErrorMessage = $"CH3 negatif gerilim {RigolDp831Config.Ch3MinVoltage}V - 0V arasında olmalıdır.";
                return;
            }

            Ch3VoltageSet = v;
            Ch3VoltageInput = v.ToString("F2", CultureInfo.InvariantCulture);

            if (IsConnected)
            {
                _onSetVoltage(3, v);
            }
        }

        private void ApplyCh3Current()
        {
            ErrorMessage = null;
            string input = (Ch3CurrentInput ?? string.Empty).Trim().Replace(',', '.');
            if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out float i))
            {
                ErrorMessage = "CH3 geçersiz akım değeri.";
                return;
            }

            if (i < RigolDp831Config.Ch3MinCurrent || i > RigolDp831Config.Ch3MaxCurrent)
            {
                ErrorMessage = $"CH3 akım {RigolDp831Config.Ch3MinCurrent}A - {RigolDp831Config.Ch3MaxCurrent}A arasında olmalıdır.";
                return;
            }

            Ch3CurrentSet = i;
            Ch3CurrentInput = i.ToString("F2", CultureInfo.InvariantCulture);

            if (IsConnected)
            {
                _onSetCurrent(3, i);
            }
        }

        /// <summary>
        /// Servisten okunan 3 kanalın verileri ile ViewModel'i günceller.
        /// </summary>
        public void UpdateFromStatus(Dp831ChannelStatus[] channels)
        {
            if (channels == null || channels.Length < 3) return;

            // CH1
            var ch1 = channels[0];
            Ch1IsOutputOn = ch1.IsOutputOn;
            Ch1VoltageActual = SanitizeVoltage(1, ch1.VoltageActual);
            Ch1CurrentActual = SanitizeCurrent(1, ch1.CurrentActual);
            Ch1PowerActual = SanitizePower(ch1.PowerActual, Ch1VoltageActual, Ch1CurrentActual);

            // CH2
            var ch2 = channels[1];
            Ch2IsOutputOn = ch2.IsOutputOn;
            Ch2VoltageActual = SanitizeVoltage(2, ch2.VoltageActual);
            Ch2CurrentActual = SanitizeCurrent(2, ch2.CurrentActual);
            Ch2PowerActual = SanitizePower(ch2.PowerActual, Ch2VoltageActual, Ch2CurrentActual);

            // CH3
            var ch3 = channels[2];
            Ch3IsOutputOn = ch3.IsOutputOn;
            Ch3VoltageActual = SanitizeVoltage(3, ch3.VoltageActual);
            Ch3CurrentActual = SanitizeCurrent(3, ch3.CurrentActual);
            Ch3PowerActual = SanitizePower(ch3.PowerActual, Ch3VoltageActual, Ch3CurrentActual);

            // Master durum: Herhangi bir kanal açık ise Master Açık kabul edilir
            IsOutputOn = Ch1IsOutputOn || Ch2IsOutputOn || Ch3IsOutputOn;
        }

        private static float SanitizeCurrent(int channel, float current)
        {
            float max = channel == 1 ? RigolDp831Config.Ch1MaxCurrent + 0.5f : RigolDp831Config.Ch2MaxCurrent + 0.5f;
            if (float.IsNaN(current) || float.IsInfinity(current) || current < 0f || current > max)
                return 0f;
            return current;
        }

        private static float SanitizeVoltage(int channel, float voltage)
        {
            float max = channel == 1 ? RigolDp831Config.Ch1MaxVoltage + 1f : 35f;
            if (float.IsNaN(voltage) || float.IsInfinity(voltage) || Math.Abs(voltage) > max)
                return 0f;
            return voltage;
        }

        private static float SanitizePower(float power, float voltage, float current)
        {
            if (float.IsNaN(power) || float.IsInfinity(power) || power < 0f || power > 80f)
                return Math.Abs(voltage * current);
            return power;
        }
    }
}
