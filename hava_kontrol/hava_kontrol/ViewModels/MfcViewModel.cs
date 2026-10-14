using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace hava_kontrol.ViewModels
{
    /// <summary>
    /// Tek MFC durumu ve kontrol ViewModel'i.
    /// P&ID şeması ve detay paneli için sapma takibi, gaz adı, satır içi hata ve tekil toggle destekler.
    /// Endüstriyel HMI renk semantiği: Kapalı MFC kırmızı değildir (nötr).
    /// </summary>
    public class MfcViewModel : BaseViewModel, ISelectableDevice
    {
        private readonly Action<int, float> _onSetSetpoint;
        private readonly Action<int, bool> _onSetFlow;

        public int MfcNumber { get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private string _gasName = string.Empty;
        public string GasName
        {
            get => string.IsNullOrEmpty(_gasName) ? GetDefaultGasName(MfcNumber) : _gasName;
            set => SetProperty(ref _gasName, value);
        }

        private int? _slaveId;
        public int? SlaveId
        {
            get => _slaveId;
            set
            {
                if (SetProperty(ref _slaveId, value))
                    OnPropertyChanged(nameof(IsConfigured));
            }
        }

        public bool IsConfigured => _slaveId != null;

        private bool _isOnline;
        public bool IsOnline
        {
            get => _isOnline;
            set
            {
                if (SetProperty(ref _isOnline, value))
                {
                    OnPropertyChanged(nameof(OnlineStatusColor));
                    OnPropertyChanged(nameof(OnlineStatusText));
                }
            }
        }

        public string OnlineStatusText => IsOnline ? "Çevrimiçi" : "Bağlantı Yok";

        private float _actualFlow;
        public float ActualFlow
        {
            get => _actualFlow;
            set
            {
                if (SetProperty(ref _actualFlow, value))
                {
                    OnPropertyChanged(nameof(ActualFlowText));
                    OnPropertyChanged(nameof(DeviationPercent));
                    OnPropertyChanged(nameof(DeviationText));
                    OnPropertyChanged(nameof(HasDeviationWarning));
                    OnPropertyChanged(nameof(FlowStatusColor));
                }
            }
        }

        public string ActualFlowText => $"{ActualFlow:F1} sccm";

        private float _setpoint;
        public float Setpoint
        {
            get => _setpoint;
            set
            {
                if (SetProperty(ref _setpoint, value))
                {
                    OnPropertyChanged(nameof(SetpointText));
                    OnPropertyChanged(nameof(DeviationPercent));
                    OnPropertyChanged(nameof(DeviationText));
                    OnPropertyChanged(nameof(HasDeviationWarning));
                    OnPropertyChanged(nameof(FlowStatusColor));
                }
            }
        }

        public string SetpointText => $"{Setpoint:F1} sccm";

        // Sapma Hesaplamaları (%2 kuralı)
        public float DeviationPercent
        {
            get
            {
                if (Setpoint <= 0.05f) return 0f;
                return (Math.Abs(ActualFlow - Setpoint) / Setpoint) * 100f;
            }
        }

        public string DeviationText => Setpoint > 0.05f
            ? $"{ActualFlow - Setpoint:+0.0;-0.0;0.0} sccm (%{DeviationPercent:F1})"
            : "—";

        public bool HasDeviationWarning => FlowEnabled && Setpoint > 0.5f && DeviationPercent > 2.0f;

        private float _maxFlow = 2000f;
        public float MaxFlow
        {
            get => _maxFlow;
            set => SetProperty(ref _maxFlow, value);
        }

        private bool _flowEnabled;
        public bool FlowEnabled
        {
            get => _flowEnabled;
            set
            {
                if (SetProperty(ref _flowEnabled, value))
                {
                    OnPropertyChanged(nameof(FlowStatusText));
                    OnPropertyChanged(nameof(FlowStatusColor));
                    OnPropertyChanged(nameof(ToggleFlowText));
                    OnPropertyChanged(nameof(HasDeviationWarning));
                }
            }
        }

        public string FlowStatusText => FlowEnabled ? "AKIŞ AÇIK" : "KAPALI";
        public string ToggleFlowText => FlowEnabled ? "Akışı Kes" : "Akış Başlat";

        // Renk Semantiği: Sapma >%2 ise Sarı (#F59E0B), Akış Açık ise Yeşil (#22C55E), Kapalı ise Nötr (#94A3B8)
        public Brush FlowStatusColor
        {
            get
            {
                if (HasDeviationWarning) return new SolidColorBrush(Color.FromRgb(245, 158, 11)); // Amber
                if (FlowEnabled) return new SolidColorBrush(Color.FromRgb(34, 197, 94));         // Green
                return new SolidColorBrush(Color.FromRgb(148, 163, 184));                      // Neutral Slate
            }
        }

        public Brush OnlineStatusColor
        {
            get
            {
                if (!IsConfigured || !IsOnline) return new SolidColorBrush(Color.FromRgb(148, 163, 184)); // Gri
                return new SolidColorBrush(Color.FromRgb(34, 197, 94));                                 // Yeşil
            }
        }

        // Satır içi Setpoint girişi
        private string _setpointInput = "0";
        public string SetpointInput
        {
            get => _setpointInput;
            set
            {
                if (SetProperty(ref _setpointInput, value))
                {
                    ValidateSetpointInput();
                }
            }
        }

        private string? _setpointValidationError;
        public string? SetpointValidationError
        {
            get => _setpointValidationError;
            set
            {
                if (SetProperty(ref _setpointValidationError, value))
                    OnPropertyChanged(nameof(HasValidationError));
            }
        }

        public bool HasValidationError => !string.IsNullOrEmpty(_setpointValidationError);

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

        public string DisplayName => $"MFC-{MfcNumber}";
        public string SlaveIdText => SlaveId.HasValue ? $"Slave ID: #{SlaveId}" : "ID: -";

        public ICommand ApplySetpointCommand { get; }
        public ICommand StartFlowCommand { get; }
        public ICommand StopFlowCommand { get; }
        public ICommand ToggleFlowCommand { get; }

        public MfcViewModel(
            int mfcNumber,
            int? slaveId,
            Action<int, float> onSetSetpoint,
            Action<int, bool> onSetFlow,
            string? gasName = null)
        {
            MfcNumber = mfcNumber;
            _slaveId = slaveId;
            _gasName = gasName ?? GetDefaultGasName(mfcNumber);
            _onSetSetpoint = onSetSetpoint;
            _onSetFlow = onSetFlow;

            ApplySetpointCommand = new RelayCommand(ApplySetpoint, () => IsConfigured && !HasValidationError);
            StartFlowCommand = new RelayCommand(() => _onSetFlow(MfcNumber, true), () => IsConfigured);
            StopFlowCommand = new RelayCommand(() => _onSetFlow(MfcNumber, false), () => IsConfigured);
            ToggleFlowCommand = new RelayCommand(ToggleFlow, () => IsConfigured);
        }

        public void ToggleFlow()
        {
            _onSetFlow(MfcNumber, !FlowEnabled);
        }

        private void ValidateSetpointInput()
        {
            if (!float.TryParse(SetpointInput, out float sccm))
            {
                SetpointValidationError = "Geçerli bir sayı girin.";
                return;
            }

            if (sccm < 0 || sccm > MaxFlow)
            {
                SetpointValidationError = $"Aralık dışı! (0 - {MaxFlow:F0} sccm)";
                return;
            }

            SetpointValidationError = null;
        }

        private void ApplySetpoint()
        {
            ValidateSetpointInput();
            if (HasValidationError) return;

            ErrorMessage = null;
            if (float.TryParse(SetpointInput, out float sccm))
            {
                try
                {
                    _onSetSetpoint(MfcNumber, sccm);
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
        }

        public void UpdateFromStatus(Dictionary<string, object> status)
        {
            ErrorMessage = null;

            if (status.TryGetValue("online", out var online) && online is bool isOnline)
                IsOnline = isOnline;

            if (status.TryGetValue("actual_flow_sccm", out var flow) && flow is float actualFlow)
                ActualFlow = actualFlow;

            if (status.TryGetValue("setpoint_sccm", out var sp) && sp is float setpoint)
                Setpoint = setpoint;

            if (status.TryGetValue("max_flow_sccm", out var mf) && mf is float maxFlow)
                MaxFlow = maxFlow;

            if (status.TryGetValue("flow_on", out var fo) && fo is bool flowOn)
                FlowEnabled = flowOn;
        }

        public void SetOffline(string? error = null)
        {
            IsOnline = false;
            ErrorMessage = error;
        }

        private static string GetDefaultGasName(int number) => number switch
        {
            1 => "CH₄ (Metan)",
            2 => "N₂ (Taşıyıcı)",
            3 => "Hedef Gaz 2",
            4 => "Kalibrasyon / N₂",
            _ => $"Hat {number}"
        };
    }
}
