using System.Globalization;

namespace Maui.FreakyControls.Shared.Converters;

public class IsNullConverter : BaseOneWayValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null;
    }
}