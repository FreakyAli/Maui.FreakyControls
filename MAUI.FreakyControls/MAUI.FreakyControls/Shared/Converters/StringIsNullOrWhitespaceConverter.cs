using System.Globalization;

namespace MAUI.FreakyControls.Shared.Converters
{
    public class StringIsNullOrWhitespaceConverter : BaseOneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrWhiteSpace(value?.ToString());
        }
    }
}

