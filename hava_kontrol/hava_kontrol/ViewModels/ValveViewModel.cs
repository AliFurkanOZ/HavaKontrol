using System.Windows.Input;
using System.Windows.Media;

namespace hava_kontrol.ViewModels
{
    /// <summary>
    /// Tek valf durumu ve kontrol ViewModel'i.
    /// P&ID şeması ve detay paneli için fonksiyonel isim, teknik etiket ve tekil toggle destekler.
    /// Endüstriyel HMI renk semantiği: Kapalı valf asla kırmızı değildir (nötr/dolgusuz).
    /// </summary>
    public class ValveViewModel : BaseViewModel, ISelectableDevice
    {
        private readonly Action<int> _onValveOn;
        private readonly Action<int> _onValveOff;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public int ValveNumber { get; }
        public string ValveName { get; }
        public string TechnicalTag { get; }
        public string PlcAddress => ValveNumber switch
        {
            <= 8 => $"Y0.{ValveNumber - 1}",
            13 => "Y1.0",
            14 => "Y1.1",
            15 => "Y1.2",
            16 => "Y1.3",
            _ => $"Y{ValveNumber}"
        };
        public string RegisterAddress => $"D{100 + ValveNumber - 1}";

        private string _functionalName = string.Empty;
        public string FunctionalName
        {
            get => string.IsNullOrEmpty(_functionalName) ? ValveName : _functionalName;
            set => SetProperty(ref _functionalName, value);
        }

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (SetProperty(ref _isOpen, value))
                {
                    StatusText = _isOpen ? "AÇIK" : "KAPALI";
                    OnPropertyChanged(nameof(StatusColor));
                    OnPropertyChanged(nameof(StatusFillBrush));
                    OnPropertyChanged(nameof(StatusBorderBrush));
                    OnPropertyChanged(nameof(ToggleText));
                    OnPropertyChanged(nameof(ToolTipText));
                }
            }
        }

        private string _statusText = "KAPALI";
        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set
            {
                if (SetProperty(ref _hasError, value))
                {
                    OnPropertyChanged(nameof(StatusColor));
                    OnPropertyChanged(nameof(StatusFillBrush));
                    OnPropertyChanged(nameof(StatusBorderBrush));
                    OnPropertyChanged(nameof(ToolTipText));
                }
            }
        }

        // Durum Renk Semantiği: Kapalı = Nötr Slate (#94A3B8), Açık = Yeşil (#22C55E), Hata = Sarı/Amber (#F59E0B)
        public Brush StatusColor
        {
            get
            {
                if (HasError) return new SolidColorBrush(Color.FromRgb(245, 158, 11)); // Amber
                if (IsOpen) return new SolidColorBrush(Color.FromRgb(34, 197, 94));     // Green
                return new SolidColorBrush(Color.FromRgb(148, 163, 184));              // Neutral Slate
            }
        }

        // P&ID Şema Vana Dolgusu (Açıkken yeşil dolgu, kapalıyken şeffaf/dolgusuz)
        public Brush StatusFillBrush
        {
            get
            {
                if (HasError) return new SolidColorBrush(Color.FromArgb(120, 245, 158, 11));
                if (IsOpen) return new SolidColorBrush(Color.FromRgb(34, 197, 94));
                return Brushes.Transparent;
            }
        }

        public Brush StatusBorderBrush
        {
            get
            {
                if (HasError) return new SolidColorBrush(Color.FromRgb(245, 158, 11));
                if (IsOpen) return new SolidColorBrush(Color.FromRgb(34, 197, 94));
                return new SolidColorBrush(Color.FromRgb(148, 163, 184));
            }
        }

        public string ToggleText => IsOpen ? "KAPAT" : "AÇ";

        public string DiagramLabel => ValveNumber switch
        {
            1 => "VALF 1",
            2 => "VALF 2",
            3 => "VALF 3",
            4 => "VALF 4",
            5 => "VALF 5",
            6 => "VALF 6",
            7 => "VALF 7",
            8 => "VALF 8",
            13 => "VALF 13",
            14 => "VALF 14",
            15 => "VALF 15",
            16 => "VALF 16",
            _ => $"VALF {ValveNumber}"
        };

        public string ToolTipText => $"{TechnicalTag}: {FunctionalName} · [{StatusText}] — Tıklayarak Aç/Kapat";

        public ICommand OpenCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand ToggleCommand { get; }

        public ValveViewModel(int valveNumber, Action<int> onValveOn, Action<int> onValveOff, string? functionalName = null)
        {
            ValveNumber = valveNumber;
            ValveName = $"VALF_{valveNumber}";
            TechnicalTag = $"VALF_{valveNumber} · {PlcAddress} ({RegisterAddress})";
            _functionalName = functionalName ?? GetDefaultFunctionalName(valveNumber);
            _onValveOn = onValveOn;
            _onValveOff = onValveOff;

            OpenCommand = new RelayCommand(() => _onValveOn(ValveNumber));
            CloseCommand = new RelayCommand(() => _onValveOff(ValveNumber));
            ToggleCommand = new RelayCommand(Toggle);
        }

        public void Toggle()
        {
            if (IsOpen)
                _onValveOff(ValveNumber);
            else
                _onValveOn(ValveNumber);
        }

        public void UpdateStatus(bool isOpen)
        {
            HasError = false;
            IsOpen = isOpen;
            StatusText = isOpen ? "AÇIK" : "KAPALI";
        }

        public void SetError()
        {
            HasError = true;
            StatusText = "HATA";
        }

        private static string GetDefaultFunctionalName(int number) => number switch
        {
            1 => "Gaz 1 Giriş Hattı",
            2 => "Gaz 2 Hattı (V-OVG2 → MFC-2)",
            3 => "Gaz 3 Hattı (V-OVG3 → MFC-3)",
            4 => "Gaz 4 Hattı (V-OVG4 → MFC-4)",
            5 => "Kuru Hava Hattı (V-OVG1 → Manifold)",
            6 => "Manifold → Sensör Hücresi Giriş",
            7 => "Sensör Temizleme / Bypass Hattı",
            8 => "Karışım Manifoldu Tahliye / Egzoz",
            13 => "V-OVG 2 Egzoz Tahliye",
            14 => "V-OVG 3 Egzoz Tahliye",
            15 => "V-OVG 4 Egzoz Tahliye",
            16 => "V-OVG 1 Egzoz Tahliye",
            _ => $"Hat Valfi {number}"
        };
    }
}
