using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace hava_kontrol.Views
{
    /// <summary>
    /// Interaction logic for DeviceDetailView.xaml
    /// </summary>
    public partial class DeviceDetailView : UserControl
    {
        public DeviceDetailView()
        {
            InitializeComponent();

            AddHandler(TextBox.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(OnTextBoxGotKeyboardFocus), true);
            AddHandler(TextBox.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(OnTextBoxPreviewMouseLeftButtonDown), true);
        }

        private void OnTextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.OriginalSource is TextBox tb)
            {
                tb.SelectAll();
            }
        }

        private void OnTextBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject d)
            {
                var tb = FindParent<TextBox>(d);
                if (tb != null && !tb.IsKeyboardFocusWithin)
                {
                    tb.Focus();
                    e.Handled = true;
                }
            }
        }

        private static T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject? current = child;
            while (current != null && !(current is T))
                current = VisualTreeHelper.GetParent(current);
            return current as T;
        }
    }
}
