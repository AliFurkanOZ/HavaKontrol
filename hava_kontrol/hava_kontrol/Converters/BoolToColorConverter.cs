using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace hava_kontrol.Converters
{
    /// <summary>
    /// Boolean değeri renk fırçasına çevirir.
    /// true → Yeşil (#22C55E), false → Kırmızı (#EF4444)
    /// ConverterParameter = "invert" ile tersine çevirir (Interlock için).
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        private static readonly SolidColorBrush GreenBrush =
            new(Color.FromRgb(34, 197, 94));

        private static readonly SolidColorBrush RedBrush =
            new(Color.FromRgb(239, 68, 68));

        private static readonly SolidColorBrush GrayBrush =
            new(Color.FromRgb(156, 163, 175));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool boolValue)
                return GrayBrush;

            bool invert = parameter is string s && s.Equals("invert", StringComparison.OrdinalIgnoreCase);

            if (invert)
                boolValue = !boolValue;

            return boolValue ? GreenBrush : RedBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Boolean → Visibility converter.
    /// true → Visible, false → Collapsed.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = value is bool boolVal && boolVal;
            if (parameter is string p && p.Equals("Inverse", StringComparison.OrdinalIgnoreCase))
                b = !b;

            return b ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Negatif bool converter (tersine çevirme).
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;
            return false;
        }
    }
}
