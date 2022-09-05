using System.Globalization;

namespace MAUI.FreakyControls.Shared.Converters
{
    public class ToLowerConverter : BaseOneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString()?.ToLower(culture);
        }
    }
}

