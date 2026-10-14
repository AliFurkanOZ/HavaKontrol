using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using hava_kontrol.Models;
using hava_kontrol.Services;
using hava_kontrol.ViewModels;

namespace hava_kontrol.Views
{
    public enum TrendChartMode
    {
        Mfc,
        Keysight,
        Vovg
    }

    /// <summary>
    /// Interaction logic for TrendGraphView.xaml
    /// Endüstriyel Çok Eksenli Canlı Trend Grafiği:
    ///   - Dinamik Dikey & Yatay Otomatik Ölçekleme (Auto-Fit)
    ///   - Etkileşimli Fare ile Yakınlaştırma (Mouse Wheel Zoom) ve Sürükleyerek Kaydırma (Pan)
    ///   - Değer Okuma İmleci (Crosshair HUD Tooltip)
    ///   - Tıklanabilir Kanal Göster/Gizle Lejantı
    ///   - 8-9 Saatlik Kesintisiz Test Güvencesi (43.200 Nokta Tamponu + Min-Max Ekran Örneklemesi)
    ///   - PNG Yüksek Çözünürlüklü Görsel ve Excel Uyumlu CSV Dışa Aktarımı
    /// </summary>
    public partial class TrendGraphView : UserControl
    {
        private MainViewModel? _viewModel;
        private double _windowSeconds = 600; // Varsayılan 10 dakika (600 sn)
        private TrendChartMode _activeMode = TrendChartMode.Mfc;

        // Yakınlaştırma, Kaydırma ve Dinamik Ölçekleme Durumları
        private bool _isAutoYScale = true;
        private bool _isLiveFollowing = true;
        private DateTime? _viewEndTime = null; // null ise canlı akışta (DateTime.Now)
        private bool _isPanning = false;
        private Point _panStartPoint;
        private DateTime _panStartEndTime;

        // Kanal Görünürlük Bayrakları (Lejant üzerinden kontrol edilir)
        private bool _mfc1Visible = true;
        private bool _mfc2Visible = true;
        private bool _mfc3Visible = true;
        private bool _mfc4Visible = true;

        private bool _smuVVisible = true;
        private bool _smuIVisible = true;
        private bool _smuRVisible = true;

        private bool _vovgTempVisible = true;
        private bool _vovgFlowVisible = true;

        public TrendGraphView()
        {
            InitializeComponent();
            Loaded += TrendGraphView_Loaded;
            Unloaded += TrendGraphView_Unloaded;
            DataContextChanged += TrendGraphView_DataContextChanged;
        }

        private void TrendGraphView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UnsubscribeViewModel();
            if (DataContext is MainViewModel vm)
            {
                _viewModel = vm;
                SubscribeViewModel();
            }
        }

        private void TrendGraphView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && _viewModel == null)
            {
                _viewModel = vm;
                SubscribeViewModel();
            }
            UpdateModeVisibility();
            RedrawChart();
        }

        private void TrendGraphView_Unloaded(object sender, RoutedEventArgs e)
        {
            UnsubscribeViewModel();
        }

        private void SubscribeViewModel()
        {
            if (_viewModel != null)
            {
                _viewModel.OnTrendUpdated += ViewModel_OnTrendUpdated;
                if (_viewModel.Electrometer != null)
                {
                    _viewModel.Electrometer.OnSmuTrendUpdated += Electrometer_OnSmuTrendUpdated;
                }
            }
        }

        private void UnsubscribeViewModel()
        {
            if (_viewModel != null)
            {
                _viewModel.OnTrendUpdated -= ViewModel_OnTrendUpdated;
                if (_viewModel.Electrometer != null)
                {
                    _viewModel.Electrometer.OnSmuTrendUpdated -= Electrometer_OnSmuTrendUpdated;
                }
                _viewModel = null;
            }
        }

        private void ViewModel_OnTrendUpdated()
        {
            if (_activeMode != TrendChartMode.Keysight && _isLiveFollowing)
            {
                Dispatcher.InvokeAsync(RedrawChart);
            }
        }

        private void Electrometer_OnSmuTrendUpdated()
        {
            if (_activeMode == TrendChartMode.Keysight && _isLiveFollowing)
            {
                Dispatcher.InvokeAsync(RedrawChart);
            }
        }

        // =======================================================
        // 1. MOD SEÇİMİ VE GÖRÜNÜRLÜK YÖNETİMİ
        // =======================================================
        private void OnTrendModeChanged(object sender, RoutedEventArgs e)
        {
            if (RbMfcTrend == null || RbKeysightTrend == null || RbVovgTrend == null) return;

            if (RbMfcTrend.IsChecked == true)
                _activeMode = TrendChartMode.Mfc;
            else if (RbKeysightTrend.IsChecked == true)
                _activeMode = TrendChartMode.Keysight;
            else if (RbVovgTrend.IsChecked == true)
                _activeMode = TrendChartMode.Vovg;

            UpdateModeVisibility();
            RedrawChart();
        }

        private void UpdateModeVisibility()
        {
            if (MfcLegendPanel == null || KeysightLegendPanel == null || VovgLegendPanel == null) return;

            // 1. Lejant Görünürlükleri
            MfcLegendPanel.Visibility = _activeMode == TrendChartMode.Mfc ? Visibility.Visible : Visibility.Collapsed;
            KeysightLegendPanel.Visibility = _activeMode == TrendChartMode.Keysight ? Visibility.Visible : Visibility.Collapsed;
            VovgLegendPanel.Visibility = _activeMode == TrendChartMode.Vovg ? Visibility.Visible : Visibility.Collapsed;

            // 2. MFC Çizgileri Görünürlüğü
            Visibility mfcVis = _activeMode == TrendChartMode.Mfc ? Visibility.Visible : Visibility.Collapsed;
            Mfc1ActLine.Visibility = (mfcVis == Visibility.Visible && _mfc1Visible) ? Visibility.Visible : Visibility.Collapsed;
            Mfc1SpLine.Visibility = (mfcVis == Visibility.Visible && _mfc1Visible) ? Visibility.Visible : Visibility.Collapsed;
            Mfc2ActLine.Visibility = (mfcVis == Visibility.Visible && _mfc2Visible) ? Visibility.Visible : Visibility.Collapsed;
            Mfc2SpLine.Visibility = (mfcVis == Visibility.Visible && _mfc2Visible) ? Visibility.Visible : Visibility.Collapsed;
            Mfc3ActLine.Visibility = (mfcVis == Visibility.Visible && _mfc3Visible) ? Visibility.Visible : Visibility.Collapsed;
            Mfc3SpLine.Visibility = (mfcVis == Visibility.Visible && _mfc3Visible) ? Visibility.Visible : Visibility.Collapsed;
            Mfc4ActLine.Visibility = (mfcVis == Visibility.Visible && _mfc4Visible) ? Visibility.Visible : Visibility.Collapsed;
            Mfc4SpLine.Visibility = (mfcVis == Visibility.Visible && _mfc4Visible) ? Visibility.Visible : Visibility.Collapsed;

            // 3. Keysight SMU Çizgileri Görünürlüğü
            Visibility smuVis = _activeMode == TrendChartMode.Keysight ? Visibility.Visible : Visibility.Collapsed;
            SmuVoltageLine.Visibility = (smuVis == Visibility.Visible && _smuVVisible) ? Visibility.Visible : Visibility.Collapsed;
            SmuCurrentLine.Visibility = (smuVis == Visibility.Visible && _smuIVisible) ? Visibility.Visible : Visibility.Collapsed;
            SmuResistanceLine.Visibility = (smuVis == Visibility.Visible && _smuRVisible) ? Visibility.Visible : Visibility.Collapsed;

            // 4. V-OVG Çizgileri Görünürlüğü
            Visibility vovgVis = _activeMode == TrendChartMode.Vovg ? Visibility.Visible : Visibility.Collapsed;
            VovgTempPvLine.Visibility = (vovgVis == Visibility.Visible && _vovgTempVisible) ? Visibility.Visible : Visibility.Collapsed;
            VovgTempSpLine.Visibility = (vovgVis == Visibility.Visible && _vovgTempVisible) ? Visibility.Visible : Visibility.Collapsed;
            VovgFlowPvLine.Visibility = (vovgVis == Visibility.Visible && _vovgFlowVisible) ? Visibility.Visible : Visibility.Collapsed;
            VovgFlowSpLine.Visibility = (vovgVis == Visibility.Visible && _vovgFlowVisible) ? Visibility.Visible : Visibility.Collapsed;
        }

        // =======================================================
        // 2. KANAL GÖSTER / GİZLE (LEJANT TIKLAMA)
        // =======================================================
        private void OnToggleChannelClicked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton btn && btn.Tag is string tag)
            {
                bool isChecked = btn.IsChecked == true;
                switch (tag)
                {
                    case "MFC1": _mfc1Visible = isChecked; break;
                    case "MFC2": _mfc2Visible = isChecked; break;
                    case "MFC3": _mfc3Visible = isChecked; break;
                    case "MFC4": _mfc4Visible = isChecked; break;
                    case "SMU_V": _smuVVisible = isChecked; break;
                    case "SMU_I": _smuIVisible = isChecked; break;
                    case "SMU_R": _smuRVisible = isChecked; break;
                    case "VOVG_TEMP": _vovgTempVisible = isChecked; break;
                    case "VOVG_FLOW": _vovgFlowVisible = isChecked; break;
                }
                UpdateModeVisibility();
                RedrawChart();
            }
        }

        // =======================================================
        // 3. ZAMAN ARALIĞI VE ZOOM / PAN DENETİMLERİ
        // =======================================================
        private void OnTimeRangeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbTimeRange?.SelectedItem is ComboBoxItem item && item.Tag != null)
            {
                if (double.TryParse(item.Tag.ToString(), out double sec))
                {
                    if (sec <= 0) // TÜMÜ (Tüm Test Oturumu Süresi)
                    {
                        DateTime sessionStart = _viewModel?.SessionStartTime ?? DateTime.Now.AddHours(-1);
                        _windowSeconds = Math.Max(60, (DateTime.Now - sessionStart).TotalSeconds);
                    }
                    else
                    {
                        _windowSeconds = sec;
                    }
                    RedrawChart();
                }
            }
        }

        private void OnAutoYScaleClicked(object sender, RoutedEventArgs e)
        {
            _isAutoYScale = BtnAutoYScale.IsChecked == true;
            RedrawChart();
        }

        private void OnZoomInClicked(object sender, RoutedEventArgs e)
        {
            ApplyZoom(0.67); // %33 yakınlaş
        }

        private void OnZoomOutClicked(object sender, RoutedEventArgs e)
        {
            ApplyZoom(1.50); // %50 uzaklaş
        }

        private void ApplyZoom(double factor)
        {
            _windowSeconds = Math.Clamp(_windowSeconds * factor, 15.0, 86400.0);
            RedrawChart();
        }

        private void OnResumeLiveClicked(object sender, RoutedEventArgs e)
        {
            _isLiveFollowing = true;
            _viewEndTime = null;
            UpdateLiveStatusBadge();
            RedrawChart();
        }

        private void UpdateLiveStatusBadge()
        {
            if (LiveIndicatorDot == null || TxtLiveStatus == null || BtnResumeLive == null) return;

            if (_isLiveFollowing)
            {
                LiveIndicatorDot.Fill = new SolidColorBrush(Color.FromRgb(16, 185, 129)); // Yeşil
                TxtLiveStatus.Text = "CANLI TAKİP";
                TxtLiveStatus.Foreground = new SolidColorBrush(Color.FromRgb(30, 64, 175)); // Koyu mavi
                BtnResumeLive.Visibility = Visibility.Collapsed;
            }
            else
            {
                LiveIndicatorDot.Fill = new SolidColorBrush(Color.FromRgb(245, 158, 11)); // Turuncu
                TxtLiveStatus.Text = "GEÇMİŞ İNCELENİYOR";
                TxtLiveStatus.Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9));
                BtnResumeLive.Visibility = Visibility.Visible;
            }
        }

        // =======================================================
        // 4. FARE İLE ETKİLEŞİM: WHEEL ZOOM, PAN & CROSSHAIR
        // =======================================================
        private void ChartCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double factor = e.Delta > 0 ? 0.75 : 1.33;
            double oldWindow = _windowSeconds;
            _windowSeconds = Math.Clamp(_windowSeconds * factor, 15.0, 86400.0);

            // Fare imlecinin altındaki zamanı merkez tutarak yakınlaş
            if (ChartCanvas.ActualWidth > 0)
            {
                Point pos = e.GetPosition(ChartCanvas);
                double ratio = Math.Clamp(pos.X / ChartCanvas.ActualWidth, 0.0, 1.0);

                DateTime currentEnd = _viewEndTime ?? DateTime.Now;
                DateTime currentStart = currentEnd.AddSeconds(-oldWindow);
                DateTime centerTime = currentStart.AddSeconds(ratio * oldWindow);

                DateTime newEnd = centerTime.AddSeconds((1.0 - ratio) * _windowSeconds);
                if (newEnd > DateTime.Now)
                {
                    newEnd = DateTime.Now;
                    _isLiveFollowing = true;
                }
                else
                {
                    _isLiveFollowing = false;
                }
                _viewEndTime = newEnd;
            }

            UpdateLiveStatusBadge();
            RedrawChart();
            e.Handled = true;
        }

        private void ChartCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isPanning = true;
            _panStartPoint = e.GetPosition(ChartCanvas);
            _panStartEndTime = _viewEndTime ?? DateTime.Now;
            ChartCanvas.CaptureMouse();
            Cursor = Cursors.SizeWE;
        }

        private void ChartCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isPanning)
            {
                _isPanning = false;
                ChartCanvas.ReleaseMouseCapture();
                Cursor = Cursors.Arrow;
            }
        }

        private void ChartCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (ChartCanvas.ActualWidth <= 0 || ChartCanvas.ActualHeight <= 0) return;

            Point currentPos = e.GetPosition(ChartCanvas);

            // A. Sürükleyerek Kaydırma (Pan)
            if (_isPanning)
            {
                double dx = currentPos.X - _panStartPoint.X;
                double secondsMoved = -(dx / ChartCanvas.ActualWidth) * _windowSeconds;

                DateTime newEnd = _panStartEndTime.AddSeconds(secondsMoved);
                if (newEnd >= DateTime.Now)
                {
                    _viewEndTime = null;
                    _isLiveFollowing = true;
                }
                else
                {
                    _viewEndTime = newEnd;
                    _isLiveFollowing = false;
                }

                UpdateLiveStatusBadge();
                RedrawChart();
                return;
            }

            // B. Değer Okuma İmleci (Crosshair Tracking & HUD)
            UpdateCrosshair(currentPos);
        }

        private void ChartCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                _isPanning = false;
                ChartCanvas.ReleaseMouseCapture();
                Cursor = Cursors.Arrow;
            }

            if (CrosshairLine != null) CrosshairLine.Visibility = Visibility.Collapsed;
            if (CrosshairHud != null) CrosshairHud.Visibility = Visibility.Collapsed;
        }

        private void UpdateCrosshair(Point pos)
        {
            if (ChartCanvas.ActualWidth <= 0 || ChartCanvas.ActualHeight <= 0) return;

            double width = ChartCanvas.ActualWidth;
            double height = ChartCanvas.ActualHeight;

            double clampedX = Math.Clamp(pos.X, 0, width);
            CrosshairLine.X1 = clampedX;
            CrosshairLine.X2 = clampedX;
            CrosshairLine.Y1 = 0;
            CrosshairLine.Y2 = height;
            CrosshairLine.Visibility = Visibility.Visible;

            DateTime viewEnd = _viewEndTime ?? DateTime.Now;
            DateTime viewStart = viewEnd.AddSeconds(-_windowSeconds);
            DateTime cursorTime = viewStart.AddSeconds((clampedX / width) * _windowSeconds);

            var sb = new StringBuilder();
            sb.Append($"⏱ {cursorTime:HH:mm:ss.fff}\n");

            if (_activeMode == TrendChartMode.Mfc && _viewModel != null)
            {
                var snapshot = _viewModel.GetMfcHistorySnapshot();
                var nearest = snapshot.OrderBy(p => Math.Abs((p.Timestamp - cursorTime).TotalMilliseconds)).FirstOrDefault();
                if (nearest.Timestamp != default)
                {
                    if (_mfc1Visible) sb.AppendLine($"MFC-1: {nearest.Mfc1Actual:F1} (SP: {nearest.Mfc1Setpoint:F0}) sccm");
                    if (_mfc2Visible) sb.AppendLine($"MFC-2: {nearest.Mfc2Actual:F1} (SP: {nearest.Mfc2Setpoint:F0}) sccm");
                    if (_mfc3Visible) sb.AppendLine($"MFC-3: {nearest.Mfc3Actual:F1} (SP: {nearest.Mfc3Setpoint:F0}) sccm");
                    if (_mfc4Visible) sb.AppendLine($"MFC-4: {nearest.Mfc4Actual:F1} (SP: {nearest.Mfc4Setpoint:F0}) sccm");
                }
            }
            else if (_activeMode == TrendChartMode.Keysight && _viewModel?.Electrometer != null)
            {
                var snapshot = _viewModel.Electrometer.GetSmuHistorySnapshot();
                var nearest = snapshot.OrderBy(p => Math.Abs((p.Timestamp - cursorTime).TotalMilliseconds)).FirstOrDefault();
                if (nearest.Timestamp != default)
                {
                    if (_smuVVisible) sb.AppendLine($"V: {ElectrometerChannelState.FormatVoltage(nearest.Voltage)}");
                    if (_smuIVisible) sb.AppendLine($"I: {ElectrometerChannelState.FormatCurrent(nearest.Current)}");
                    if (_smuRVisible) sb.AppendLine($"R: {ElectrometerChannelState.FormatResistance(nearest.Resistance)}");
                }
            }
            else if (_activeMode == TrendChartMode.Vovg && _viewModel != null)
            {
                var snapshot = _viewModel.GetVovgHistorySnapshot();
                var nearest = snapshot.OrderBy(p => Math.Abs((p.Timestamp - cursorTime).TotalMilliseconds)).FirstOrDefault();
                if (nearest.Timestamp != default)
                {
                    if (_vovgTempVisible) sb.AppendLine($"Sıcaklık: {nearest.TempPV:F1} °C (SP: {nearest.TempSetpoint:F0})");
                    if (_vovgFlowVisible) sb.AppendLine($"Taşıyıcı: {nearest.FlowPV:F0} sccm (SP: {nearest.FlowSetpoint:F0})");
                }
            }

            TxtCrosshairInfo.Text = sb.ToString().TrimEnd();

            // HUD Kutusunu imlecin yanına yerleştir (ekrandan taşmayacak şekilde)
            double hudLeft = clampedX + 12;
            if (hudLeft + 140 > width) hudLeft = clampedX - 150;
            if (hudLeft < 4) hudLeft = 4;

            double hudTop = Math.Clamp(pos.Y - 20, 5, height - 70);

            Canvas.SetLeft(CrosshairHud, hudLeft);
            Canvas.SetTop(CrosshairHud, hudTop);
            CrosshairHud.Visibility = Visibility.Visible;
        }

        // =======================================================
        // 5. ANA GRAFİK ÇİZİM MOTORU (REDRAW)
        // =======================================================
        private void ChartCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RedrawChart();
        }

        private void RedrawChart()
        {
            if (_viewModel == null || ChartCanvas == null || ChartCanvas.ActualWidth <= 10 || ChartCanvas.ActualHeight <= 10)
                return;

            double width = ChartCanvas.ActualWidth;
            double height = ChartCanvas.ActualHeight;

            // Izgara çizgilerini ayarla (3 Yatay, 3 Dikey)
            GridLineTop.X1 = 0; GridLineTop.Y1 = height * 0.15;
            GridLineTop.X2 = width; GridLineTop.Y2 = height * 0.15;

            GridLineMid.X1 = 0; GridLineMid.Y1 = height * 0.50;
            GridLineMid.X2 = width; GridLineMid.Y2 = height * 0.50;

            GridLineBottom.X1 = 0; GridLineBottom.Y1 = height * 0.85;
            GridLineBottom.X2 = width; GridLineBottom.Y2 = height * 0.85;

            GridV1.X1 = width * 0.25; GridV1.Y1 = 0;
            GridV1.X2 = width * 0.25; GridV1.Y2 = height;

            GridV2.X1 = width * 0.50; GridV2.Y1 = 0;
            GridV2.X2 = width * 0.50; GridV2.Y2 = height;

            GridV3.X1 = width * 0.75; GridV3.Y1 = 0;
            GridV3.X2 = width * 0.75; GridV3.Y2 = height;

            // Zaman Penceresi Belirleme
            DateTime viewEnd = _viewEndTime ?? DateTime.Now;
            DateTime viewStart = viewEnd.AddSeconds(-_windowSeconds);

            // Alt Zaman Etiketlerini Güncelle
            TxtTimeStart.Text = viewStart.ToString("HH:mm:ss");
            TxtTimeMid.Text = viewStart.AddSeconds(_windowSeconds / 2).ToString("HH:mm:ss");
            TxtTimeEnd.Text = _isLiveFollowing ? $"Şimdi ({viewEnd:HH:mm:ss})" : viewEnd.ToString("HH:mm:ss");

            switch (_activeMode)
            {
                case TrendChartMode.Mfc:
                    RedrawMfcChart(width, height, viewStart, viewEnd);
                    break;
                case TrendChartMode.Keysight:
                    RedrawKeysightChart(width, height, viewStart, viewEnd);
                    break;
                case TrendChartMode.Vovg:
                    RedrawVovgChart(width, height, viewStart, viewEnd);
                    break;
            }
        }

        // =======================================================
        // 6. YÜKSEK PERFORMANSLI MİN-MAX PİKSEL ÖRNEKLEME (DECIMATION)
        // =======================================================
        /// <summary>
        /// 8-9 saatlik uzun testlerde (onbinlerce nokta) WPF Polyline'ın donmasını engeller.
        /// Ekran piksel genişliğine göre gruplama yaparak her pikseldeki en küçük ve en büyük
        /// noktayı alır. Böylece ani sıçramalar (peak/dip) kaybolmaz, 60 FPS akıcılık korunur.
        /// </summary>
        private static PointCollection BuildDecimatedPolyline(
            List<(DateTime time, double val)> rawData,
            DateTime start,
            DateTime end,
            double minVal,
            double maxVal,
            double width,
            double height)
        {
            var points = new PointCollection();
            if (rawData.Count == 0 || width <= 0 || height <= 0) return points;

            double totalSec = (end - start).TotalSeconds;
            if (totalSec <= 0) totalSec = 1.0;

            double valRange = maxVal - minVal;
            if (valRange <= 0) valRange = 1.0;
            double chartH = height - 10;

            // Nokta sayısı az ise doğrudan eşle
            if (rawData.Count <= width * 1.5)
            {
                foreach (var pt in rawData)
                {
                    double sec = (pt.time - start).TotalSeconds;
                    double x = Math.Clamp((sec / totalSec) * width, 0, width);
                    double norm = Math.Clamp((pt.val - minVal) / valRange, 0.0, 1.0);
                    double y = height - (norm * chartH) - 5;
                    points.Add(new Point(x, y));
                }
                return points;
            }

            // Min-Max Piksel Kova Örneklemesi (Decimation)
            int numBuckets = (int)Math.Max(50, width);
            double secPerBucket = totalSec / numBuckets;

            int dataIdx = 0;
            for (int b = 0; b < numBuckets && dataIdx < rawData.Count; b++)
            {
                DateTime bucketStart = start.AddSeconds(b * secPerBucket);
                DateTime bucketEnd = bucketStart.AddSeconds(secPerBucket);

                double bMin = double.MaxValue;
                double bMax = double.MinValue;
                DateTime tMin = bucketStart;
                DateTime tMax = bucketStart;
                bool hasData = false;

                while (dataIdx < rawData.Count && rawData[dataIdx].time < bucketEnd)
                {
                    hasData = true;
                    double v = rawData[dataIdx].val;
                    DateTime t = rawData[dataIdx].time;

                    if (v < bMin) { bMin = v; tMin = t; }
                    if (v > bMax) { bMax = v; tMax = t; }
                    dataIdx++;
                }

                if (hasData)
                {
                    // Kronolojik sırayı koruyarak ekle
                    if (tMin <= tMax)
                    {
                        AddPoint(points, tMin, bMin, start, totalSec, width, minVal, valRange, chartH, height);
                        if (Math.Abs(bMax - bMin) > 0.0001)
                        {
                            AddPoint(points, tMax, bMax, start, totalSec, width, minVal, valRange, chartH, height);
                        }
                    }
                    else
                    {
                        AddPoint(points, tMax, bMax, start, totalSec, width, minVal, valRange, chartH, height);
                        if (Math.Abs(bMax - bMin) > 0.0001)
                        {
                            AddPoint(points, tMin, bMin, start, totalSec, width, minVal, valRange, chartH, height);
                        }
                    }
                }
            }

            return points;
        }

        private static void AddPoint(PointCollection col, DateTime t, double v, DateTime start, double totalSec, double width, double minVal, double valRange, double chartH, double height)
        {
            double sec = (t - start).TotalSeconds;
            double x = Math.Clamp((sec / totalSec) * width, 0, width);
            double norm = Math.Clamp((v - minVal) / valRange, 0.0, 1.0);
            double y = height - (norm * chartH) - 5;
            col.Add(new Point(x, y));
        }

        // =======================================================
        // 7. MFC AKIŞLARI ÇİZİMİ
        // =======================================================
        private void RedrawMfcChart(double width, double height, DateTime viewStart, DateTime viewEnd)
        {
            if (_viewModel == null) return;

            var snapshot = _viewModel.GetMfcHistorySnapshot();
            var filtered = snapshot.Where(p => p.Timestamp >= viewStart && p.Timestamp <= viewEnd).OrderBy(p => p.Timestamp).ToList();

            if (filtered.Count == 0)
            {
                TxtNoData.Text = "MFC akış verisi bekleniyor...";
                TxtNoData.Visibility = Visibility.Visible;
                ClearMfcLines();
                return;
            }

            TxtNoData.Visibility = Visibility.Collapsed;

            // En son ölçüm ve hedef değerlerini üst lejanta aktar
            var latest = filtered.Last();
            TxtMfc1Act.Text = $"{latest.Mfc1Actual:F1}"; TxtMfc1Sp.Text = $"/{latest.Mfc1Setpoint:F0}";
            TxtMfc2Act.Text = $"{latest.Mfc2Actual:F1}"; TxtMfc2Sp.Text = $"/{latest.Mfc2Setpoint:F0}";
            TxtMfc3Act.Text = $"{latest.Mfc3Actual:F1}"; TxtMfc3Sp.Text = $"/{latest.Mfc3Setpoint:F0}";
            TxtMfc4Act.Text = $"{latest.Mfc4Actual:F1}"; TxtMfc4Sp.Text = $"/{latest.Mfc4Setpoint:F0}";

            // Dinamik Dikey Otomatik Ölçekleme (Y Min-Max)
            double minFlow = double.MaxValue;
            double maxFlow = double.MinValue;

            foreach (var p in filtered)
            {
                if (_mfc1Visible) { minFlow = Math.Min(minFlow, Math.Min(p.Mfc1Actual, p.Mfc1Setpoint)); maxFlow = Math.Max(maxFlow, Math.Max(p.Mfc1Actual, p.Mfc1Setpoint)); }
                if (_mfc2Visible) { minFlow = Math.Min(minFlow, Math.Min(p.Mfc2Actual, p.Mfc2Setpoint)); maxFlow = Math.Max(maxFlow, Math.Max(p.Mfc2Actual, p.Mfc2Setpoint)); }
                if (_mfc3Visible) { minFlow = Math.Min(minFlow, Math.Min(p.Mfc3Actual, p.Mfc3Setpoint)); maxFlow = Math.Max(maxFlow, Math.Max(p.Mfc3Actual, p.Mfc3Setpoint)); }
                if (_mfc4Visible) { minFlow = Math.Min(minFlow, Math.Min(p.Mfc4Actual, p.Mfc4Setpoint)); maxFlow = Math.Max(maxFlow, Math.Max(p.Mfc4Actual, p.Mfc4Setpoint)); }
            }

            if (minFlow == double.MaxValue) { minFlow = 0; maxFlow = 50; }

            if (_isAutoYScale)
            {
                double span = Math.Max(5.0, maxFlow - minFlow);
                minFlow = Math.Max(0, minFlow - span * 0.08);
                maxFlow = maxFlow + span * 0.08;
            }
            else
            {
                minFlow = 0;
                maxFlow = Math.Max(20.0, Math.Ceiling(maxFlow * 1.15 / 10.0) * 10.0);
            }

            // Sol Eksen Etiketleri
            TxtYLeftMax.Text = $"{maxFlow:F1}";
            TxtYLeftMid.Text = $"{(minFlow + maxFlow) / 2:F1}";
            TxtYLeftMin.Text = $"{minFlow:F1} sccm";
            TxtYLeftMax.Foreground = new SolidColorBrush(Color.FromRgb(37, 99, 235));
            TxtYLeftMid.Foreground = new SolidColorBrush(Color.FromRgb(37, 99, 235));
            TxtYLeftMin.Foreground = new SolidColorBrush(Color.FromRgb(37, 99, 235));

            TxtYRightMax.Text = ""; TxtYRightMid.Text = ""; TxtYRightMin.Text = "";

            // Çizgileri Min-Max örneklemesiyle üret
            if (_mfc1Visible)
            {
                Mfc1ActLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.Mfc1Actual)).ToList(), viewStart, viewEnd, minFlow, maxFlow, width, height);
                Mfc1SpLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.Mfc1Setpoint)).ToList(), viewStart, viewEnd, minFlow, maxFlow, width, height);
            }
            else { Mfc1ActLine.Points.Clear(); Mfc1SpLine.Points.Clear(); }

            if (_mfc2Visible)
            {
                Mfc2ActLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.Mfc2Actual)).ToList(), viewStart, viewEnd, minFlow, maxFlow, width, height);
                Mfc2SpLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.Mfc2Setpoint)).ToList(), viewStart, viewEnd, minFlow, maxFlow, width, height);
            }
            else { Mfc2ActLine.Points.Clear(); Mfc2SpLine.Points.Clear(); }

            if (_mfc3Visible)
            {
                Mfc3ActLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.Mfc3Actual)).ToList(), viewStart, viewEnd, minFlow, maxFlow, width, height);
                Mfc3SpLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.Mfc3Setpoint)).ToList(), viewStart, viewEnd, minFlow, maxFlow, width, height);
            }
            else { Mfc3ActLine.Points.Clear(); Mfc3SpLine.Points.Clear(); }

            if (_mfc4Visible)
            {
                Mfc4ActLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.Mfc4Actual)).ToList(), viewStart, viewEnd, minFlow, maxFlow, width, height);
                Mfc4SpLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.Mfc4Setpoint)).ToList(), viewStart, viewEnd, minFlow, maxFlow, width, height);
            }
            else { Mfc4ActLine.Points.Clear(); Mfc4SpLine.Points.Clear(); }
        }

        private void ClearMfcLines()
        {
            Mfc1ActLine.Points.Clear(); Mfc1SpLine.Points.Clear();
            Mfc2ActLine.Points.Clear(); Mfc2SpLine.Points.Clear();
            Mfc3ActLine.Points.Clear(); Mfc3SpLine.Points.Clear();
            Mfc4ActLine.Points.Clear(); Mfc4SpLine.Points.Clear();
        }

        // =======================================================
        // 8. KEYSIGHT SMU ÇİZİMİ
        // =======================================================
        private void RedrawKeysightChart(double width, double height, DateTime viewStart, DateTime viewEnd)
        {
            if (_viewModel?.Electrometer == null) return;

            var snapshot = _viewModel.Electrometer.GetSmuHistorySnapshot();
            var filtered = snapshot.Where(p => p.Timestamp >= viewStart && p.Timestamp <= viewEnd).OrderBy(p => p.Timestamp).ToList();

            if (filtered.Count == 0)
            {
                TxtNoData.Text = "Keysight SMU V-I-R verisi bekleniyor (Çıkış Açıkken Kaydedilir)...";
                TxtNoData.Visibility = Visibility.Visible;
                SmuVoltageLine.Points.Clear(); SmuCurrentLine.Points.Clear(); SmuResistanceLine.Points.Clear();
                return;
            }

            TxtNoData.Visibility = Visibility.Collapsed;

            var latest = filtered.Last();
            TxtSmuVoltageValue.Text = ElectrometerChannelState.FormatVoltage(latest.Voltage);
            TxtSmuCurrentValue.Text = ElectrometerChannelState.FormatCurrent(latest.Current);
            TxtSmuResistanceValue.Text = ElectrometerChannelState.FormatResistance(latest.Resistance);

            // Gerilim Dinamik Skalası (Sol Eksen)
            double minV = filtered.Min(p => p.Voltage);
            double maxV = filtered.Max(p => p.Voltage);

            if (_isAutoYScale)
            {
                double spanV = Math.Max(0.1, maxV - minV);
                minV = minV - spanV * 0.08;
                maxV = maxV + spanV * 0.08;
            }
            else
            {
                minV = Math.Min(0, minV);
                maxV = Math.Max(1.0, Math.Ceiling(maxV * 1.15 * 10) / 10.0);
            }

            TxtYLeftMax.Text = $"{maxV:F2} V";
            TxtYLeftMid.Text = $"{(minV + maxV) / 2:F2} V";
            TxtYLeftMin.Text = $"{minV:F2} V";
            TxtYLeftMax.Foreground = new SolidColorBrush(Color.FromRgb(245, 158, 11));
            TxtYLeftMid.Foreground = new SolidColorBrush(Color.FromRgb(245, 158, 11));
            TxtYLeftMin.Foreground = new SolidColorBrush(Color.FromRgb(245, 158, 11));

            // Akım Dinamik Skalası (Sağ Eksen)
            double maxI = Math.Max(1e-12, filtered.Max(p => Math.Abs(p.Current)));
            double minI = 0.0;
            if (_isAutoYScale)
            {
                minI = filtered.Min(p => Math.Abs(p.Current));
            }
            TxtYRightMax.Text = ElectrometerChannelState.FormatCurrent(maxI);
            TxtYRightMid.Text = ElectrometerChannelState.FormatCurrent((minI + maxI) / 2);
            TxtYRightMin.Text = ElectrometerChannelState.FormatCurrent(minI);
            TxtYRightMax.Foreground = new SolidColorBrush(Color.FromRgb(139, 92, 246));
            TxtYRightMid.Foreground = new SolidColorBrush(Color.FromRgb(139, 92, 246));
            TxtYRightMin.Foreground = new SolidColorBrush(Color.FromRgb(139, 92, 246));

            // Çizgileri Örnekle
            if (_smuVVisible)
                SmuVoltageLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, p.Voltage)).ToList(), viewStart, viewEnd, minV, maxV, width, height);
            else SmuVoltageLine.Points.Clear();

            if (_smuIVisible)
                SmuCurrentLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, Math.Abs(p.Current))).ToList(), viewStart, viewEnd, minI, maxI, width, height);
            else SmuCurrentLine.Points.Clear();

            if (_smuRVisible)
            {
                var validR = filtered.Where(p => !double.IsNaN(p.Resistance) && !double.IsInfinity(p.Resistance) && p.Resistance > 0).ToList();
                double maxR = validR.Count > 0 ? validR.Max(p => p.Resistance) : 1e6;
                double minR = validR.Count > 0 ? validR.Min(p => p.Resistance) : 1.0;
                double logMin = Math.Log10(Math.Max(1e-3, minR));
                double logMax = Math.Log10(Math.Max(1e-2, maxR));

                SmuResistanceLine.Points = BuildDecimatedPolyline(
                    validR.Select(p => (p.Timestamp, Math.Log10(p.Resistance))).ToList(),
                    viewStart, viewEnd, logMin, logMax, width, height);
            }
            else SmuResistanceLine.Points.Clear();
        }

        // =======================================================
        // 9. V-OVG FIRIN ÇİZİMİ
        // =======================================================
        private void RedrawVovgChart(double width, double height, DateTime viewStart, DateTime viewEnd)
        {
            if (_viewModel == null) return;

            var snapshot = _viewModel.GetVovgHistorySnapshot();
            var filtered = snapshot.Where(p => p.Timestamp >= viewStart && p.Timestamp <= viewEnd).OrderBy(p => p.Timestamp).ToList();

            if (filtered.Count == 0)
            {
                TxtNoData.Text = "V-OVG fırın ve taşıyıcı gaz verisi bekleniyor...";
                TxtNoData.Visibility = Visibility.Visible;
                VovgTempPvLine.Points.Clear(); VovgTempSpLine.Points.Clear();
                VovgFlowPvLine.Points.Clear(); VovgFlowSpLine.Points.Clear();
                return;
            }

            TxtNoData.Visibility = Visibility.Collapsed;

            var latest = filtered.Last();
            TxtVovgTempPV.Text = $"{latest.TempPV:F1} °C"; TxtVovgTempSP.Text = $" / {latest.TempSetpoint:F0} °C";
            TxtVovgFlowPV.Text = $"{latest.FlowPV:F0} sccm"; TxtVovgFlowSP.Text = $" / {latest.FlowSetpoint:F0} sccm";

            // Sıcaklık Skalası (Sol Eksen °C)
            double minTemp = filtered.Min(p => Math.Min(p.TempPV, p.TempSetpoint));
            double maxTemp = filtered.Max(p => Math.Max(p.TempPV, p.TempSetpoint));

            if (_isAutoYScale)
            {
                double spanT = Math.Max(5.0, maxTemp - minTemp);
                minTemp = Math.Max(0, minTemp - spanT * 0.08);
                maxTemp = maxTemp + spanT * 0.08;
            }
            else
            {
                minTemp = 0;
                maxTemp = Math.Max(40.0, Math.Ceiling((maxTemp + 5.0) / 10.0) * 10.0);
            }

            TxtYLeftMax.Text = $"{maxTemp:F1} °C";
            TxtYLeftMid.Text = $"{(minTemp + maxTemp) / 2:F1} °C";
            TxtYLeftMin.Text = $"{minTemp:F1} °C";
            TxtYLeftMax.Foreground = new SolidColorBrush(Color.FromRgb(234, 88, 12));
            TxtYLeftMid.Foreground = new SolidColorBrush(Color.FromRgb(234, 88, 12));
            TxtYLeftMin.Foreground = new SolidColorBrush(Color.FromRgb(234, 88, 12));

            // Taşıyıcı Gaz Akış Skalası (Sağ Eksen sccm)
            double minFlow = filtered.Min(p => Math.Min(p.FlowPV, p.FlowSetpoint));
            double maxFlow = filtered.Max(p => Math.Max(p.FlowPV, p.FlowSetpoint));

            if (_isAutoYScale)
            {
                double spanF = Math.Max(5.0, maxFlow - minFlow);
                minFlow = Math.Max(0, minFlow - spanF * 0.08);
                maxFlow = maxFlow + spanF * 0.08;
            }
            else
            {
                minFlow = 0;
                maxFlow = Math.Max(50.0, Math.Ceiling(maxFlow * 1.15 / 10.0) * 10.0);
            }

            TxtYRightMax.Text = $"{maxFlow:F0} sccm";
            TxtYRightMid.Text = $"{(minFlow + maxFlow) / 2:F0}";
            TxtYRightMin.Text = $"{minFlow:F0}";
            TxtYRightMax.Foreground = new SolidColorBrush(Color.FromRgb(2, 132, 199));
            TxtYRightMid.Foreground = new SolidColorBrush(Color.FromRgb(2, 132, 199));
            TxtYRightMin.Foreground = new SolidColorBrush(Color.FromRgb(2, 132, 199));

            // Çizgileri Örnekle
            if (_vovgTempVisible)
            {
                VovgTempPvLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.TempPV)).ToList(), viewStart, viewEnd, minTemp, maxTemp, width, height);
                VovgTempSpLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.TempSetpoint)).ToList(), viewStart, viewEnd, minTemp, maxTemp, width, height);
            }
            else { VovgTempPvLine.Points.Clear(); VovgTempSpLine.Points.Clear(); }

            if (_vovgFlowVisible)
            {
                VovgFlowPvLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.FlowPV)).ToList(), viewStart, viewEnd, minFlow, maxFlow, width, height);
                VovgFlowSpLine.Points = BuildDecimatedPolyline(filtered.Select(p => (p.Timestamp, (double)p.FlowSetpoint)).ToList(), viewStart, viewEnd, minFlow, maxFlow, width, height);
            }
            else { VovgFlowPvLine.Points.Clear(); VovgFlowSpLine.Points.Clear(); }
        }

        // =======================================================
        // 10. DIŞA AKTARMA: PNG GÖRSEL KAYDETME
        // =======================================================
        private void OnExportPngClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string modeName = _activeMode switch
                {
                    TrendChartMode.Mfc => "MFC_Akislari",
                    TrendChartMode.Keysight => "Keysight_SMU",
                    TrendChartMode.Vovg => "VOVG_Firin",
                    _ => "Trend"
                };

                var sfd = new SaveFileDialog
                {
                    Title = "Grafiği PNG Resmi Olarak Kaydet",
                    Filter = "PNG Resmi (*.png)|*.png",
                    FileName = $"{modeName}_{DateTime.Now:yyyyMMdd_HHmmss}.png"
                };

                if (sfd.ShowDialog() == true)
                {
                    int renderW = (int)Math.Max(100, ChartOuterBorder.ActualWidth);
                    int renderH = (int)Math.Max(100, ChartOuterBorder.ActualHeight);

                    var rtb = new RenderTargetBitmap(renderW, renderH, 96, 96, PixelFormats.Pbgra32);
                    rtb.Render(ChartOuterBorder);

                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));

                    using (var fs = File.OpenWrite(sfd.FileName))
                    {
                        encoder.Save(fs);
                    }

                    LoggingService.Instance.Info("GRAFİK", $"Trend grafiği PNG olarak kaydedildi: {Path.GetFileName(sfd.FileName)}");
                    MessageBox.Show($"Grafik başarıyla kaydedildi:\n{sfd.FileName}", "PNG Kaydedildi", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                LoggingService.Instance.Warning("GRAFİK", $"PNG kaydetme hatası: {ex.Message}");
                MessageBox.Show($"PNG kaydedilirken hata oluştu:\n{ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // =======================================================
        // 11. DIŞA AKTARMA: CSV VERİ TABLOSU KAYDETME
        // =======================================================
        private void OnExportCsvClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string modeName = _activeMode switch
                {
                    TrendChartMode.Mfc => "MFC_Akislari",
                    TrendChartMode.Keysight => "Keysight_SMU",
                    TrendChartMode.Vovg => "VOVG_Firin",
                    _ => "Trend"
                };

                // Kullanıcıya görüntülenen aralık mı yoksa tüm oturum mu istendiğini seçtir
                var result = MessageBox.Show(
                    "Trend verilerini nasıl kaydetmek istersiniz?\n\n" +
                    "• EVET: Yalnızca şu an ekranda görüntülenen zaman aralığını kaydet\n" +
                    "• HAYIR: Başlangıçtan bu yana kaydedilen TÜM TEST OTURUMUNU (8-9 sa) kaydet",
                    "CSV Veri Dışa Aktarma Kapsamı",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel) return;

                bool onlyVisible = (result == MessageBoxResult.Yes);

                var sfd = new SaveFileDialog
                {
                    Title = "Grafik Verilerini CSV Olarak Kaydet",
                    Filter = "CSV Dosyası (*.csv)|*.csv|Metin Dosyası (*.txt)|*.txt",
                    FileName = $"{modeName}_{(onlyVisible ? "Aralik" : "TumOturum")}_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (sfd.ShowDialog() == true)
                {
                    DateTime viewEnd = _viewEndTime ?? DateTime.Now;
                    DateTime viewStart = onlyVisible ? viewEnd.AddSeconds(-_windowSeconds) : DateTime.MinValue;

                    var sb = new StringBuilder();

                    if (_activeMode == TrendChartMode.Mfc)
                    {
                        sb.AppendLine("Zaman;MFC1_Olculen_sccm;MFC1_Hedef_sccm;MFC2_Olculen_sccm;MFC2_Hedef_sccm;MFC3_Olculen_sccm;MFC3_Hedef_sccm;MFC4_Olculen_sccm;MFC4_Hedef_sccm");
                        var data = _viewModel?.GetMfcHistorySnapshot() ?? new List<MfcTrendPoint>();
                        foreach (var row in data.Where(p => p.Timestamp >= viewStart && p.Timestamp <= viewEnd).OrderBy(p => p.Timestamp))
                        {
                            sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                                "{0:yyyy-MM-dd HH:mm:ss.fff};{1:F2};{2:F2};{3:F2};{4:F2};{5:F2};{6:F2};{7:F2};{8:F2}",
                                row.Timestamp,
                                row.Mfc1Actual, row.Mfc1Setpoint,
                                row.Mfc2Actual, row.Mfc2Setpoint,
                                row.Mfc3Actual, row.Mfc3Setpoint,
                                row.Mfc4Actual, row.Mfc4Setpoint));
                        }
                    }
                    else if (_activeMode == TrendChartMode.Keysight)
                    {
                        sb.AppendLine("Zaman;Kanal;Gerilim_V;Akim_A;Direnc_Ohm;Cikis_Durumu");
                        var data = _viewModel?.Electrometer?.GetSmuHistorySnapshot() ?? new List<SmuTrendPoint>();
                        foreach (var row in data.Where(p => p.Timestamp >= viewStart && p.Timestamp <= viewEnd).OrderBy(p => p.Timestamp))
                        {
                            sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                                "{0:yyyy-MM-dd HH:mm:ss.fff};{1};{2:G6};{3:E5};{4:E5};{5}",
                                row.Timestamp,
                                row.Channel,
                                row.Voltage,
                                row.Current,
                                double.IsNaN(row.Resistance) ? 0.0 : row.Resistance,
                                row.IsOutputOn ? "ACIK" : "KAPALI"));
                        }
                    }
                    else if (_activeMode == TrendChartMode.Vovg)
                    {
                        sb.AppendLine("Zaman;Sicaklik_PV_C;Sicaklik_SP_C;Akis_PV_sccm;Akis_SP_sccm");
                        var data = _viewModel?.GetVovgHistorySnapshot() ?? new List<VovgTrendPoint>();
                        foreach (var row in data.Where(p => p.Timestamp >= viewStart && p.Timestamp <= viewEnd).OrderBy(p => p.Timestamp))
                        {
                            sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                                "{0:yyyy-MM-dd HH:mm:ss.fff};{1:F2};{2:F2};{3:F2};{4:F2}",
                                row.Timestamp,
                                row.TempPV, row.TempSetpoint,
                                row.FlowPV, row.FlowSetpoint));
                        }
                    }

                    // Türkçe Excel için UTF-8 BOM ile kaydet
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    LoggingService.Instance.Info("GRAFİK", $"Trend verileri CSV olarak kaydedildi: {Path.GetFileName(sfd.FileName)}");
                    MessageBox.Show($"Veriler başarıyla CSV dosyasına aktarıldı:\n{sfd.FileName}", "CSV Aktarıldı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                LoggingService.Instance.Warning("GRAFİK", $"CSV kaydetme hatası: {ex.Message}");
                MessageBox.Show($"CSV kaydedilirken hata oluştu:\n{ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
