using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace hava_kontrol.Views
{
    public partial class PowerSupplyPanel : UserControl
    {
        public PowerSupplyPanel()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }

        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();
            }
        }
    }
}
