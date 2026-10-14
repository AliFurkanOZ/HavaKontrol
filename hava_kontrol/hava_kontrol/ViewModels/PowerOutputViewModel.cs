using System;
using System.Windows.Input;
using System.Windows.Media;

namespace hava_kontrol.ViewModels
{
    /// <summary>
    /// PLC Güç Çıkışı (Power ON/OFF - Y0.8 - Y0.11) ViewModel'i.
    /// Endüstriyel HMI renk semantiği ve tekil toggle destekler.
    /// </summary>
    public class PowerOutputViewModel : BaseViewModel
    {
        private readonly Action<int> _onPowerOn;
        private readonly Action<int> _onPowerOff;

        public int OutputNumber { get; }
        public string OutputName { get; }
        public string FunctionalName { get; set; }
        public string PlcAddress { get; }
        public string RegisterInfo { get; }

        public string ShortName => OutputNumber switch
        {
            8 => "Fırın",
            9 => "Hücre",
            10 => "Pompa",
            11 => "Cihaz",
            _ => $"Çıkış {OutputNumber}"
        };

        public string ButtonLabel => $"{PlcAddress} {ShortName}";

        private bool _isOn;
        public bool IsOn
        {
            get => _isOn;
            set
            {
                if (SetProperty(ref _isOn, value))
                {
                    OnPropertyChanged(nameof(StatusText));
                    OnPropertyChanged(nameof(StatusColor));
                    OnPropertyChanged(nameof(ToggleText));
                }
            }
        }

        private string _statusText = "KAPALI (OFF)";
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
                    OnPropertyChanged(nameof(StatusColor));
            }
        }

        // Renk Semantiği: Açık = Yeşil (#22C55E), Kapalı = Nötr (#94A3B8), Hata = Sarı (#F59E0B)
        public Brush StatusColor
        {
            get
            {
                if (HasError) return new SolidColorBrush(Color.FromRgb(245, 158, 11)); // Amber
                if (IsOn) return new SolidColorBrush(Color.FromRgb(34, 197, 94));     // Green
                return new SolidColorBrush(Color.FromRgb(148, 163, 184));            // Neutral Slate
            }
        }

        public string ToggleText => IsOn ? "KAPAT" : "AÇ";

        public ICommand PowerOnCommand { get; }
        public ICommand PowerOffCommand { get; }
        public ICommand ToggleCommand { get; }

        public PowerOutputViewModel(int outputNumber, Action<int> onPowerOn, Action<int> onPowerOff, string? functionalName = null)
        {
            OutputNumber = outputNumber;
            PlcAddress = $"Y0.{outputNumber}";
            OutputName = $"GÜÇ ÇIKIŞI {outputNumber} ({PlcAddress})";
            FunctionalName = functionalName ?? GetDefaultFunctionalName(outputNumber);
            RegisterInfo = $"D{100 + outputNumber}";
            _onPowerOn = onPowerOn;
            _onPowerOff = onPowerOff;

            PowerOnCommand = new RelayCommand(() => _onPowerOn(OutputNumber));
            PowerOffCommand = new RelayCommand(() => _onPowerOff(OutputNumber));
            ToggleCommand = new RelayCommand(Toggle);
        }

        public void Toggle()
        {
            if (IsOn)
                _onPowerOff(OutputNumber);
            else
                _onPowerOn(OutputNumber);
        }

        public void UpdateStatus(bool isOn)
        {
            HasError = false;
            IsOn = isOn;
            StatusText = isOn ? "AÇIK (ON)" : "KAPALI (OFF)";
        }

        public void SetError()
        {
            HasError = true;
            StatusText = "HATA";
        }

        private static string GetDefaultFunctionalName(int number) => number switch
        {
            8 => "Fırın Yardımcı Güç",
            9 => "Hücre Isıtıcı / Güç",
            10 => "Egzoz Tahliye Pompası",
            11 => "Genel Enstrüman Gücü",
            _ => $"Güç Çıkışı Y0.{number}"
        };
    }
}
