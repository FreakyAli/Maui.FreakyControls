using System.Globalization;

namespace MAUI.FreakyControls.Shared.Converters
{
    public class ToUpperConverter : BaseOneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString()?.ToUpper(culture);
        }
    }
}

