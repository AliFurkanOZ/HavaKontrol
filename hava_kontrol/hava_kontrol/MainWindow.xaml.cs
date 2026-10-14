using System.Windows;
using hava_kontrol.ViewModels;

namespace hava_kontrol
{
    /// <summary>
    /// MainWindow code-behind.
    /// MVVM: DataContext olarak MainViewModel bağlanır.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                _viewModel = new MainViewModel();
                DataContext = _viewModel;
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("crash.log", ex.ToString());
                throw;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _viewModel.Dispose();
            base.OnClosed(e);
        }

        private void MfcControlPanel_Loaded()
        {

        }
    }
}