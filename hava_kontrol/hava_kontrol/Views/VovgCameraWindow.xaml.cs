using System;
using System.Windows;
using hava_kontrol.ViewModels;

namespace hava_kontrol.Views
{
    /// <summary>
    /// VovgCameraWindow.xaml etkileşim mantığı.
    /// Belirli bir V-OVG ünitesinin kamera akışını bağımsız pencerede sunar.
    /// </summary>
    public partial class VovgCameraWindow : Window
    {
        public VovgCameraWindow(VovgViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Loaded += (s, e) => UpdateFullscreenButtonText();
            StateChanged += VovgCameraWindow_StateChanged;
        }

        public VovgCameraWindow()
        {
            InitializeComponent();
            Loaded += (s, e) => UpdateFullscreenButtonText();
            StateChanged += VovgCameraWindow_StateChanged;
        }

        private void VovgCameraWindow_StateChanged(object? sender, EventArgs e)
        {
            UpdateFullscreenButtonText();
        }

        private void UpdateFullscreenButtonText()
        {
            if (BtnFullscreen != null)
            {
                if (WindowState == WindowState.Maximized)
                {
                    BtnFullscreen.Content = "🗗 PENCERE MODU";
                    BtnFullscreen.ToolTip = "Normal Pencere Boyutuna Dön";
                }
                else
                {
                    BtnFullscreen.Content = "⛶ TAM EKRAN";
                    BtnFullscreen.ToolTip = "Ekranı Kapla / Tam Ekran Moduna Geç";
                }
            }
        }

        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }
}
