using System.Globalization;

namespace Maui.FreakyControls.Shared.Converters;

public class StringIsNullOrWhitespaceConverter : BaseOneWayValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.IsNullOrWhiteSpace(value?.ToString());
    }
}