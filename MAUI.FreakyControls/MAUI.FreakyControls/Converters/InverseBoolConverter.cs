using System.Globalization;

namespace Maui.FreakyControls.Converters;

public class InverseBoolConverter : BaseOneWayValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool boolValue)
        {
            throw new ArgumentException("Value must be a boolean", nameof(value));
        }

        return !boolValue;
    }
}