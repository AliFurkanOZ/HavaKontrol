using System;
using System.Globalization;
using hava_kontrol.ViewModels;

namespace hava_kontrol.Models
{
    /// <summary>
    /// Keysight B2902B SMU'nun bir kanalına (CH1 veya CH2) ait bağımsız durum modeli.
    /// Kanal ayarları, kaynak modları (V-Source / I-Source) ve ölçümleri birbirinden izole tutar.
    /// </summary>
    public class ElectrometerChannelState : BaseViewModel
    {
        public int ChannelNumber { get; }
        public string Name { get; }

        public ElectrometerChannelState(int channelNumber, string name)
        {
            ChannelNumber = channelNumber;
            Name = name;
        }

        // ==========================================
        // ÇIKIŞ DURUMU (ON / OFF)
        // ==========================================

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
                    UpdateMeasurementTexts();
                }
            }
        }

        public string OutputStatusText => IsOutputOn ? "AÇIK (ON)" : "KAPALI (OFF)";
        public string OutputStatusColor => IsOutputOn ? "#10B981" : "#94A3B8";

        // ==========================================
        // KAYNAK (SOURCE) MODU VE AYARLARI
        // ==========================================

        private bool _isVoltageSourceMode = true;
        /// <summary>True ise Voltaj Kaynağı (V-Source), False ise Akım Kaynağı (I-Source).</summary>
        public bool IsVoltageSourceMode
        {
            get => _isVoltageSourceMode;
            set
            {
                if (SetProperty(ref _isVoltageSourceMode, value))
                {
                    OnPropertyChanged(nameof(IsCurrentSourceMode));
                    OnPropertyChanged(nameof(SourceModeText));
                }
            }
        }

        public bool IsCurrentSourceMode => !IsVoltageSourceMode;
        public string SourceModeText => IsVoltageSourceMode ? "VOLTAJ KAYNAĞI" : "AKIM KAYNAĞI";

        // Voltaj Kaynağı Ayarları
        private float _voltageSourceValue = 0.0f;
        public float VoltageSourceValue
        {
            get => _voltageSourceValue;
            set
            {
                if (SetProperty(ref _voltageSourceValue, value))
                {
                    OnPropertyChanged(nameof(VoltageSourceText));
                    OnPropertyChanged(nameof(HasHvWarning));
                }
            }
        }

        private string _voltageSourceInput = "0.0";
        public string VoltageSourceInput
        {
            get => _voltageSourceInput;
            set => SetProperty(ref _voltageSourceInput, value);
        }

        private float _currentComplianceValue = 0.05f; // 50 mA varsayılan
        public float CurrentComplianceValue
        {
            get => _currentComplianceValue;
            set
            {
                if (SetProperty(ref _currentComplianceValue, value))
                {
                    OnPropertyChanged(nameof(CurrentComplianceText));
                }
            }
        }

        private string _currentComplianceInput = "50"; // mA cinsinden
        public string CurrentComplianceInput
        {
            get => _currentComplianceInput;
            set => SetProperty(ref _currentComplianceInput, value);
        }

        // Akım Kaynağı Ayarları
        private float _currentSourceValue = 0.001f; // 1 mA varsayılan
        public float CurrentSourceValue
        {
            get => _currentSourceValue;
            set
            {
                if (SetProperty(ref _currentSourceValue, value))
                {
                    OnPropertyChanged(nameof(CurrentSourceText));
                }
            }
        }

        private string _currentSourceInput = "1.0"; // mA cinsinden
        public string CurrentSourceInput
        {
            get => _currentSourceInput;
            set => SetProperty(ref _currentSourceInput, value);
        }

        private float _voltageComplianceValue = 21.0f; // 21 V varsayılan
        public float VoltageComplianceValue
        {
            get => _voltageComplianceValue;
            set
            {
                if (SetProperty(ref _voltageComplianceValue, value))
                {
                    OnPropertyChanged(nameof(VoltageComplianceText));
                    OnPropertyChanged(nameof(HasHvWarning));
                }
            }
        }

        private string _voltageComplianceInput = "21.0"; // V cinsinden
        public string VoltageComplianceInput
        {
            get => _voltageComplianceInput;
            set => SetProperty(ref _voltageComplianceInput, value);
        }

        public string VoltageSourceText => $"{VoltageSourceValue:F2} V (I_lim: {CurrentComplianceValue * 1000:F1} mA)";
        public string CurrentComplianceText => $"{CurrentComplianceValue * 1000:F1} mA";
        public string CurrentSourceText => $"{CurrentSourceValue * 1000:F3} mA (V_lim: {VoltageComplianceValue:F1} V)";
        public string VoltageComplianceText => $"{VoltageComplianceValue:F1} V";

        public bool HasHvWarning => Math.Abs(VoltageSourceValue) >= KeysightB2902bConfig.HighVoltageWarningThreshold ||
                                   (IsCurrentSourceMode && Math.Abs(VoltageComplianceValue) >= KeysightB2902bConfig.HighVoltageWarningThreshold);

        // ==========================================
        // ÖLÇÜM MODU VE DEĞERLERİ
        // ==========================================

        private ElectrometerMeasureMode _measureMode = ElectrometerMeasureMode.Current;
        public ElectrometerMeasureMode MeasureMode
        {
            get => _measureMode;
            set
            {
                if (SetProperty(ref _measureMode, value))
                {
                    OnPropertyChanged(nameof(IsCurrentMode));
                    OnPropertyChanged(nameof(IsVoltageMode));
                    OnPropertyChanged(nameof(IsResistanceMode));
                    OnPropertyChanged(nameof(FunctionText));
                    UpdateMeasurementTexts();
                }
            }
        }

        public bool IsCurrentMode => MeasureMode == ElectrometerMeasureMode.Current;
        public bool IsVoltageMode => MeasureMode == ElectrometerMeasureMode.Voltage;
        public bool IsResistanceMode => MeasureMode == ElectrometerMeasureMode.Resistance;

        public string FunctionText => MeasureMode switch
        {
            ElectrometerMeasureMode.Current => "AKIM (I)",
            ElectrometerMeasureMode.Voltage => "VOLTAJ (V)",
            ElectrometerMeasureMode.Resistance => "DİRENÇ (R)",
            _ => "?"
        };

        private double _measuredCurrent = double.NaN;
        public double MeasuredCurrent
        {
            get => _measuredCurrent;
            private set => SetProperty(ref _measuredCurrent, value);
        }

        private double _measuredVoltage = double.NaN;
        public double MeasuredVoltage
        {
            get => _measuredVoltage;
            private set => SetProperty(ref _measuredVoltage, value);
        }

        private double _measuredResistance = double.NaN;
        public double MeasuredResistance
        {
            get => _measuredResistance;
            private set => SetProperty(ref _measuredResistance, value);
        }

        private string _currentText = "---";
        public string CurrentText
        {
            get => _currentText;
            private set => SetProperty(ref _currentText, value);
        }

        private string _voltageText = "---";
        public string VoltageText
        {
            get => _voltageText;
            private set => SetProperty(ref _voltageText, value);
        }

        private string _resistanceText = "---";
        public string ResistanceText
        {
            get => _resistanceText;
            private set => SetProperty(ref _resistanceText, value);
        }

        private string _primaryMeasurementText = "---";
        public string PrimaryMeasurementText
        {
            get => _primaryMeasurementText;
            private set => SetProperty(ref _primaryMeasurementText, value);
        }

        /// <summary>
        /// Tüm telemetri değerlerini (V, I, R) tek adımda günceller.
        /// </summary>
        public void UpdateMeasurements(double voltage, double current, double resistance)
        {
            MeasuredVoltage = voltage;
            MeasuredCurrent = current;
            MeasuredResistance = resistance;
            UpdateMeasurementTexts();
        }

        public void ResetMeasurements()
        {
            MeasuredVoltage = double.NaN;
            MeasuredCurrent = double.NaN;
            MeasuredResistance = double.NaN;
            UpdateMeasurementTexts();
        }

        private void UpdateMeasurementTexts()
        {
            if (!IsOutputOn)
            {
                PrimaryMeasurementText = "KAPALI (OFF)";
                CurrentText = "KAPALI";
                VoltageText = "KAPALI";
                ResistanceText = "KAPALI";
                return;
            }

            CurrentText = FormatCurrent(MeasuredCurrent);
            VoltageText = FormatVoltage(MeasuredVoltage);
            ResistanceText = FormatResistance(MeasuredResistance);

            PrimaryMeasurementText = MeasureMode switch
            {
                ElectrometerMeasureMode.Current => CurrentText,
                ElectrometerMeasureMode.Voltage => VoltageText,
                ElectrometerMeasureMode.Resistance => ResistanceText,
                _ => "---"
            };
        }

        public static string FormatCurrent(double amps)
        {
            if (double.IsNaN(amps)) return "---";
            if (double.IsInfinity(amps)) return "O.L (Aşırı Akım)";

            double abs = Math.Abs(amps);
            if (abs < 1e-15) return "0.000 pA";

            if (abs < 1e-9)
                return $"{amps * 1e12:F3} pA";
            if (abs < 1e-6)
                return $"{amps * 1e9:F3} nA";
            if (abs < 1e-3)
                return $"{amps * 1e6:F3} µA";
            if (abs < 1.0)
                return $"{amps * 1e3:F4} mA";

            return $"{amps:F4} A";
        }

        public static string FormatVoltage(double volts)
        {
            if (double.IsNaN(volts)) return "---";
            if (double.IsInfinity(volts)) return "O.L (Aşırı Voltaj)";

            double abs = Math.Abs(volts);
            if (abs < 1e-6) return $"{volts * 1e6:F2} µV";
            if (abs < 1.0) return $"{volts * 1e3:F3} mV";

            return $"{volts:F4} V";
        }

        public static string FormatResistance(double ohms)
        {
            if (double.IsNaN(ohms)) return "---";
            if (double.IsInfinity(ohms) || ohms > 1e14 || ohms < 0) return "O.L (Açık Devre)";

            double abs = Math.Abs(ohms);
            if (abs < 1e-3) return $"{ohms * 1e3:F2} mΩ";
            if (abs < 1e3) return $"{ohms:F3} Ω";
            if (abs < 1e6) return $"{ohms / 1e3:F3} kΩ";
            if (abs < 1e9) return $"{ohms / 1e6:F3} MΩ";
            if (abs < 1e12) return $"{ohms / 1e9:F3} GΩ";

            return $"{ohms / 1e12:F3} TΩ";
        }
    }
}
