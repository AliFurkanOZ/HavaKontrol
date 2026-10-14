using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using hava_kontrol.Models;
using hava_kontrol.Services;
using hava_kontrol.Views;

namespace hava_kontrol.ViewModels
{
    /// <summary>
    /// V-OVG kontrol paneli ViewModel.
    /// Tek bir V-OVG cihazının (Flow Controller RCXX ve Temperature Controller RRXX) durumlarını yönetir.
    /// 4 Adet V-OVG cihazını bağımsız yönetebilecek şekilde parametriktir.
    /// MfcViewModel pattern'i ile tutarlıdır.
    /// Kamera izleme, fotoğraf çekme ve bağımsız tam ekran pencere desteği içerir.
    /// </summary>
    public class VovgViewModel : BaseViewModel, ISelectableDevice
    {
        private readonly Action<int, float>? _onSetFlowSetpointWithIndex;
        private readonly Action<int, float>? _onSetTempSetpointWithIndex;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (SetProperty(ref _isSelected, value))
                {
                    if (value && !IsCameraActive)
                    {
                        StartCameraStream();
                    }
                }
            }
        }

        // ======================================
        // CİHAZ KİMLİK BİLGİLERİ
        // ======================================

        public int DeviceNumber { get; }
        public string DisplayName => $"V-OVG {DeviceNumber}";
        public byte TempSlaveId { get; }
        public byte FlowSlaveId { get; }
        public string SlaveBadgeText => $"T: #{TempSlaveId} | F: #{FlowSlaveId}";

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
                    OnPropertyChanged(nameof(DeviceStatusSummary));
                }
            }
        }

        public Brush ConnectionStatusColor => IsConnected
            ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
            : new SolidColorBrush(Color.FromRgb(148, 163, 184));

        public string DeviceStatusSummary
        {
            get
            {
                if (!IsConnected) return "BAĞLANTI YOK";
                if (IsFlowOnline && IsTempOnline) return "AKTİF (T+F OK)";
                if (IsFlowOnline) return "SADECE AKIŞ OK";
                if (IsTempOnline) return "SADECE SICAKLIK OK";
                return "İLETİŞİM BEKLENİYOR";
            }
        }

        // ======================================
        // FLOW CONTROLLER (Slave 4, 6, 8)
        // ======================================

        private bool _isFlowOnline;
        public bool IsFlowOnline
        {
            get => _isFlowOnline;
            set
            {
                if (SetProperty(ref _isFlowOnline, value))
                {
                    OnPropertyChanged(nameof(FlowStatusColor));
                    OnPropertyChanged(nameof(DeviceStatusSummary));
                }
            }
        }

        private float _flowPV;
        public float FlowPV
        {
            get => _flowPV;
            set
            {
                if (SetProperty(ref _flowPV, value))
                    OnPropertyChanged(nameof(FlowPVText));
            }
        }

        public string FlowPVText => $"{FlowPV:F0} sccm";

        private float _flowSetpoint;
        public float FlowSetpoint
        {
            get => _flowSetpoint;
            set
            {
                if (SetProperty(ref _flowSetpoint, value))
                    OnPropertyChanged(nameof(FlowSetpointText));
            }
        }

        public string FlowSetpointText => $"{FlowSetpoint:F0} sccm";

        private float _flowOutput;
        public float FlowOutput
        {
            get => _flowOutput;
            set
            {
                if (SetProperty(ref _flowOutput, value))
                    OnPropertyChanged(nameof(FlowOutputText));
            }
        }

        public string FlowOutputText => $"{FlowOutput:F0}%";

        private bool _flowAlarm;
        public bool FlowAlarm
        {
            get => _flowAlarm;
            set
            {
                if (SetProperty(ref _flowAlarm, value))
                    OnPropertyChanged(nameof(FlowAlarmColor));
            }
        }

        public Brush FlowStatusColor => IsFlowOnline
            ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
            : new SolidColorBrush(Color.FromRgb(148, 163, 184));

        public Brush FlowAlarmColor => FlowAlarm
            ? new SolidColorBrush(Color.FromRgb(239, 68, 68))
            : new SolidColorBrush(Color.FromRgb(148, 163, 184));

        // Flow setpoint girişi
        private string _flowSetpointInput = "0";
        public string FlowSetpointInput
        {
            get => _flowSetpointInput;
            set => SetProperty(ref _flowSetpointInput, value);
        }

        // ======================================
        // TEMPERATURE CONTROLLER (Slave 3, 5, 7)
        // ======================================

        private bool _isTempOnline;
        public bool IsTempOnline
        {
            get => _isTempOnline;
            set
            {
                if (SetProperty(ref _isTempOnline, value))
                {
                    OnPropertyChanged(nameof(TempStatusColor));
                    OnPropertyChanged(nameof(DeviceStatusSummary));
                }
            }
        }

        private float _tempPV;
        public float TempPV
        {
            get => _tempPV;
            set
            {
                if (SetProperty(ref _tempPV, value))
                    OnPropertyChanged(nameof(TempPVText));
            }
        }

        public string TempPVText => $"{TempPV:F1} °C";

        private float _tempSetpoint;
        public float TempSetpoint
        {
            get => _tempSetpoint;
            set
            {
                if (SetProperty(ref _tempSetpoint, value))
                    OnPropertyChanged(nameof(TempSetpointText));
            }
        }

        public string TempSetpointText => $"{TempSetpoint:F1} °C";

        private float _tempOutput;
        public float TempOutput
        {
            get => _tempOutput;
            set
            {
                if (SetProperty(ref _tempOutput, value))
                    OnPropertyChanged(nameof(TempOutputText));
            }
        }

        public string TempOutputText => $"{TempOutput:F1}%";

        private bool _tempAlarm;
        public bool TempAlarm
        {
            get => _tempAlarm;
            set
            {
                if (SetProperty(ref _tempAlarm, value))
                    OnPropertyChanged(nameof(TempAlarmColor));
            }
        }

        public Brush TempStatusColor => IsTempOnline
            ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
            : new SolidColorBrush(Color.FromRgb(148, 163, 184));

        public Brush TempAlarmColor => TempAlarm
            ? new SolidColorBrush(Color.FromRgb(239, 68, 68))
            : new SolidColorBrush(Color.FromRgb(148, 163, 184));

        // Temperature setpoint girişi
        private string _tempSetpointInput = "25";
        public string TempSetpointInput
        {
            get => _tempSetpointInput;
            set => SetProperty(ref _tempSetpointInput, value);
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
        // KAMERA İZLEME SİSTEMİ
        // ======================================

        private BitmapSource? _cameraFrame;
        public BitmapSource? CameraFrame
        {
            get => _cameraFrame;
            set => SetProperty(ref _cameraFrame, value);
        }

        private bool _isCameraActive;
        public bool IsCameraActive
        {
            get => _isCameraActive;
            set
            {
                if (SetProperty(ref _isCameraActive, value))
                {
                    OnPropertyChanged(nameof(CameraStatusColor));
                    OnPropertyChanged(nameof(CameraToggleText));
                }
            }
        }

        public string CameraToggleText => IsCameraActive ? "⏹ DURDUR" : "▶ BAŞLAT";

        private string _cameraStatusText = "KAPALI";
        public string CameraStatusText
        {
            get => _cameraStatusText;
            set
            {
                if (SetProperty(ref _cameraStatusText, value))
                    OnPropertyChanged(nameof(CameraStatusColor));
            }
        }

        public Brush CameraStatusColor
        {
            get
            {
                if (!string.IsNullOrEmpty(CameraError))
                    return new SolidColorBrush(Color.FromRgb(239, 68, 68)); // Kırmızı (Hata)
                if (IsCameraActive)
                    return new SolidColorBrush(Color.FromRgb(34, 197, 94)); // Yeşil (Canlı)
                return new SolidColorBrush(Color.FromRgb(148, 163, 184));   // Gri (Kapalı)
            }
        }

        private string? _cameraError;
        public string? CameraError
        {
            get => _cameraError;
            set
            {
                if (SetProperty(ref _cameraError, value))
                {
                    OnPropertyChanged(nameof(HasCameraError));
                    OnPropertyChanged(nameof(CameraStatusColor));
                }
            }
        }

        public bool HasCameraError => !string.IsNullOrEmpty(_cameraError);

        public ObservableCollection<CameraDeviceInfo> AvailableCameras { get; } = new();

        private CameraDeviceInfo? _selectedCamera;
        public CameraDeviceInfo? SelectedCamera
        {
            get => _selectedCamera;
            set
            {
                if (SetProperty(ref _selectedCamera, value))
                {
                    OnCameraSelectionChanged();
                }
            }
        }

        private string? _snapshotNotification;
        public string? SnapshotNotification
        {
            get => _snapshotNotification;
            set => SetProperty(ref _snapshotNotification, value);
        }

        private string? _lastSnapshotPath;
        public string? LastSnapshotPath
        {
            get => _lastSnapshotPath;
            set => SetProperty(ref _lastSnapshotPath, value);
        }

        private bool _showOsdTelemetry = true;
        public bool ShowOsdTelemetry
        {
            get => _showOsdTelemetry;
            set
            {
                if (SetProperty(ref _showOsdTelemetry, value))
                {
                    CameraService.Instance.Config.ShowTelemetryOverlay = value;
                    CameraService.Instance.Config.Save();
                }
            }
        }

        // ======================================
        // KOMUTLAR
        // ======================================

        public ICommand ApplyFlowSetpointCommand { get; }
        public ICommand ApplyTempSetpointCommand { get; }
        public ICommand QuickSetFlowCommand { get; }
        public ICommand QuickSetTempCommand { get; }

        public ICommand ToggleCameraCommand { get; }
        public ICommand RefreshCamerasCommand { get; }
        public ICommand TakeSnapshotCommand { get; }
        public ICommand OpenFullViewCommand { get; }
        public ICommand OpenSnapshotFolderCommand { get; }

        // ======================================
        // CONSTRUCTORS
        // ======================================

        /// <summary>
        /// Çoklu V-OVG cihazı için ana constructor.
        /// </summary>
        public VovgViewModel(
            int deviceNumber,
            byte tempSlaveId,
            byte flowSlaveId,
            Action<int, float> onSetFlowSetpoint,
            Action<int, float> onSetTempSetpoint)
        {
            DeviceNumber = deviceNumber;
            TempSlaveId = tempSlaveId;
            FlowSlaveId = flowSlaveId;
            _onSetFlowSetpointWithIndex = onSetFlowSetpoint;
            _onSetTempSetpointWithIndex = onSetTempSetpoint;

            ApplyFlowSetpointCommand = new RelayCommand(ApplyFlowSetpoint);
            ApplyTempSetpointCommand = new RelayCommand(ApplyTempSetpoint);
            QuickSetFlowCommand = new RelayCommand(QuickSetFlow);
            QuickSetTempCommand = new RelayCommand(QuickSetTemp);

            ToggleCameraCommand = new RelayCommand(_ => ToggleCameraStream());
            RefreshCamerasCommand = new RelayCommand(_ => RefreshCameras());
            TakeSnapshotCommand = new RelayCommand(_ => TakeSnapshot());
            OpenFullViewCommand = new RelayCommand(_ => OpenFullView());
            OpenSnapshotFolderCommand = new RelayCommand(_ => OpenSnapshotFolder());

            RefreshCameras();
        }

        /// <summary>
        /// Geriye dönük uyumluluk constructor'ı (V-OVG 1).
        /// </summary>
        public VovgViewModel(
            Action<float> onSetFlowSetpoint,
            Action<float> onSetTempSetpoint)
            : this(1, EurothermConfig.Vovg1TempSlaveId, EurothermConfig.Vovg1FlowSlaveId,
                   (idx, val) => onSetFlowSetpoint(val),
                   (idx, val) => onSetTempSetpoint(val))
        {
        }

        // ======================================
        // SETPOINT UYGULA
        // ======================================

        private void QuickSetFlow(object? parameter)
        {
            if (parameter == null) return;
            if (float.TryParse(parameter.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float val) ||
                float.TryParse(parameter.ToString(), out val))
            {
                FlowSetpointInput = val.ToString("F0", System.Globalization.CultureInfo.InvariantCulture);
                ApplyFlowSetpoint();
            }
        }

        private void QuickSetTemp(object? parameter)
        {
            if (parameter == null) return;
            if (float.TryParse(parameter.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float val) ||
                float.TryParse(parameter.ToString(), out val))
            {
                TempSetpointInput = val.ToString("F0", System.Globalization.CultureInfo.InvariantCulture);
                ApplyTempSetpoint();
            }
        }

        private void ApplyFlowSetpoint()
        {
            ErrorMessage = null;

            if (!float.TryParse(FlowSetpointInput, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float sccm) &&
                !float.TryParse(FlowSetpointInput, out sccm))
            {
                ErrorMessage = "Geçersiz sayı değeri.";
                return;
            }

            if (sccm < EurothermConfig.FlowMinSetpoint || sccm > EurothermConfig.FlowMaxSetpoint)
            {
                ErrorMessage = $"Flow setpoint {EurothermConfig.FlowMinSetpoint}-{EurothermConfig.FlowMaxSetpoint} sccm arasında olmalıdır.";
                return;
            }

            // Alarma düşmemesi için: setpoint, mevcut PV'nin en fazla 38 sccm üzerinde olabilir
            float maxAllowed = FlowPV + EurothermConfig.FlowMaxOverPV;
            if (maxAllowed > EurothermConfig.FlowMaxSetpoint)
                maxAllowed = EurothermConfig.FlowMaxSetpoint;

            if (sccm > maxAllowed)
            {
                ErrorMessage = $"⚠ Flow setpoint çok yüksek! Mevcut PV: {FlowPV:F0} sccm → Maksimum izin verilen SP: {maxAllowed:F0} sccm (PV + {EurothermConfig.FlowMaxOverPV:F0}). Alarma düşmemek için daha düşük bir değer girin.";
                return;
            }

            try
            {
                _onSetFlowSetpointWithIndex?.Invoke(DeviceNumber, sccm);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void ApplyTempSetpoint()
        {
            ErrorMessage = null;

            if (!float.TryParse(TempSetpointInput, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float celsius) &&
                !float.TryParse(TempSetpointInput, out celsius))
            {
                ErrorMessage = "Geçersiz sayı değeri.";
                return;
            }

            if (celsius < EurothermConfig.TempMinSetpoint || celsius > EurothermConfig.TempMaxSetpoint)
            {
                ErrorMessage = $"Sıcaklık setpoint {EurothermConfig.TempMinSetpoint}-{EurothermConfig.TempMaxSetpoint} °C arasında olmalıdır.";
                return;
            }

            try
            {
                _onSetTempSetpointWithIndex?.Invoke(DeviceNumber, celsius);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        // ======================================
        // DURUM GÜNCELLEME
        // ======================================

        /// <summary>
        /// VovgManager.ReadStatus() veya ReadAllDevicesStatus() sonucuyla UI'ı günceller.
        /// </summary>
        public void UpdateFromStatus(Dictionary<string, object> status)
        {
            // --- Flow ---
            if (status.TryGetValue("flow_online", out var fo) && fo is bool flowOnline)
                IsFlowOnline = flowOnline;

            if (status.TryGetValue("flow_pv", out var fpv) && fpv is float flowPv)
                FlowPV = flowPv;

            if (status.TryGetValue("flow_sp", out var fsp) && fsp is float flowSp)
                FlowSetpoint = flowSp;

            if (status.TryGetValue("flow_output", out var fop) && fop is float flowOp)
                FlowOutput = flowOp;

            if (status.TryGetValue("flow_alarm", out var fa) && fa is bool flowAlarm)
                FlowAlarm = flowAlarm;

            // --- Temperature ---
            if (status.TryGetValue("temp_online", out var to) && to is bool tempOnline)
                IsTempOnline = tempOnline;

            if (status.TryGetValue("temp_pv", out var tpv) && tpv is float tempPv)
                TempPV = tempPv;

            if (status.TryGetValue("temp_sp", out var tsp) && tsp is float tempSp)
                TempSetpoint = tempSp;

            if (status.TryGetValue("temp_output", out var top) && top is float tempOp)
                TempOutput = tempOp;

            if (status.TryGetValue("temp_alarm", out var ta) && ta is bool tempAlarm)
                TempAlarm = tempAlarm;

            // --- Hata ---
            if (status.TryGetValue("error", out var err) && err is string errMsg && !string.IsNullOrEmpty(errMsg))
                ErrorMessage = errMsg;
        }

        // ======================================
        // KAMERA YÖNETİM METOTLARI
        // ======================================

        public void RefreshCameras()
        {
            try
            {
                int currentDeviceId = SelectedCamera?.DeviceId ?? -999;
                string? currentDeviceName = SelectedCamera?.Name;
                bool wasActive = IsCameraActive;

                var list = CameraService.Instance.GetAvailableCameras();

                // Koleksiyonu akıllı güncelle
                bool listChanged = AvailableCameras.Count != list.Count ||
                                   !AvailableCameras.Select(c => c.DeviceId).SequenceEqual(list.Select(c => c.DeviceId));

                if (listChanged)
                {
                    AvailableCameras.Clear();
                    foreach (var cam in list)
                    {
                        AvailableCameras.Add(cam);
                    }
                }

                // Önceden bu V-OVG için kaydedilmiş son kamera ayarını bul
                var cfg = CameraService.Instance.Config;
                string? savedName = cfg.VovgCameraNameAssignments.TryGetValue(DeviceNumber, out var sName) ? sName : null;
                int savedDeviceId = cfg.VovgCameraAssignments.TryGetValue(DeviceNumber, out int devId)
                    ? devId
                    : (DeviceNumber - 1); // Varsayılan: V-OVG 1 -> 0, V-OVG 2 -> 1 ...

                // Eşleştirme önceliği:
                // 1) Şu an seçili olan kamera (eğer hala listedeyse)
                // 2) İsimle kaydedilmiş son kamera
                // 3) DeviceId ile kaydedilmiş son kamera
                // 4) Simülasyon modu
                CameraDeviceInfo? matched = null;
                if (currentDeviceId != -999)
                {
                    matched = AvailableCameras.FirstOrDefault(c => c.DeviceId == currentDeviceId);
                }

                if (matched == null && !string.IsNullOrEmpty(savedName))
                {
                    matched = AvailableCameras.FirstOrDefault(c => c.Name == savedName);
                }

                if (matched == null)
                {
                    matched = AvailableCameras.FirstOrDefault(c => c.DeviceId == savedDeviceId)
                              ?? AvailableCameras.FirstOrDefault(c => c.IsSimulation)
                              ?? AvailableCameras.FirstOrDefault();
                }

                if (matched != null)
                {
                    _selectedCamera = matched;
                    OnPropertyChanged(nameof(SelectedCamera));
                }

                // Eğer kamera yayındaydıysa ve akış kesildiyse otomatik yeniden canlandır
                if (wasActive && !CameraService.Instance.IsStreaming(DeviceNumber))
                {
                    StartCameraStream();
                }
            }
            catch (Exception ex)
            {
                CameraError = ex.Message;
            }
        }

        private void OnCameraSelectionChanged()
        {
            if (SelectedCamera == null) return;

            // Ayarı yapılandırmaya kaydet (hem DeviceId hem de Name olarak saklanır)
            var cfg = CameraService.Instance.Config;
            cfg.VovgCameraAssignments[DeviceNumber] = SelectedCamera.DeviceId;
            cfg.VovgCameraNameAssignments[DeviceNumber] = SelectedCamera.Name;
            cfg.Save();

            // Yayın açıksa yeni seçilen kamerayla hemen yeniden başlat
            if (IsCameraActive)
            {
                StartCameraStream();
            }
        }

        public void StartCameraStream()
        {
            CameraError = null;
            int devId = SelectedCamera?.DeviceId ?? -1;

            CameraService.Instance.StartStream(
                DeviceNumber,
                devId,
                telemetryProvider: () => (TempPV, FlowPV, DeviceStatusSummary),
                onFrame: frame =>
                {
                    CameraFrame = frame;
                },
                onStatusChanged: status =>
                {
                    Application.Current?.Dispatcher.InvokeAsync(() =>
                    {
                        CameraStatusText = status;
                        IsCameraActive = status.StartsWith("CANLI") || status.StartsWith("SİMÜLASYON");
                    });
                },
                onError: err =>
                {
                    Application.Current?.Dispatcher.InvokeAsync(() =>
                    {
                        CameraError = err;
                    });
                });
        }

        public void StopCameraStream()
        {
            CameraService.Instance.StopStream(DeviceNumber);
            IsCameraActive = false;
            CameraStatusText = "KAPALI";
            CameraFrame = null;
        }

        public void ToggleCameraStream()
        {
            if (IsCameraActive)
            {
                StopCameraStream();
            }
            else
            {
                StartCameraStream();
            }
        }

        public void TakeSnapshot()
        {
            try
            {
                string? saved = CameraService.Instance.CaptureSnapshot(DeviceNumber);
                if (!string.IsNullOrEmpty(saved))
                {
                    string filename = Path.GetFileName(saved);
                    SnapshotNotification = $"✓ Fotoğraf Kaydedildi: {filename}";
                    LastSnapshotPath = saved;

                    Task.Delay(3500).ContinueWith(_ =>
                    {
                        Application.Current?.Dispatcher.InvokeAsync(() =>
                        {
                            SnapshotNotification = null;
                        });
                    });
                }
                else
                {
                    SnapshotNotification = "⚠ Görüntü alınamadı (Kamera yayını açık olmalıdır)";
                    Task.Delay(3000).ContinueWith(_ =>
                    {
                        Application.Current?.Dispatcher.InvokeAsync(() =>
                        {
                            SnapshotNotification = null;
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                CameraError = ex.Message;
            }
        }

        private VovgCameraWindow? _cameraWindow;

        public void OpenFullView()
        {
            try
            {
                if (_cameraWindow == null || !_cameraWindow.IsLoaded)
                {
                    _cameraWindow = new VovgCameraWindow(this);
                    _cameraWindow.Closed += (s, e) => _cameraWindow = null;
                    _cameraWindow.WindowState = WindowState.Normal;
                    _cameraWindow.Show();
                }
                else
                {
                    _cameraWindow.Activate();
                    if (_cameraWindow.WindowState == WindowState.Minimized)
                    {
                        _cameraWindow.WindowState = WindowState.Normal;
                    }
                }

                if (!IsCameraActive)
                {
                    StartCameraStream();
                }
            }
            catch (Exception ex)
            {
                CameraError = ex.Message;
            }
        }

        public void OpenSnapshotFolder()
        {
            try
            {
                string dir = CameraService.Instance.Config.SnapshotDirectory;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                Process.Start(new ProcessStartInfo
                {
                    FileName = dir,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                CameraError = ex.Message;
            }
        }
    }
}
