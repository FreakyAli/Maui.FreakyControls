using Maui.FreakyControls.Shared.Enums;
using System.Globalization;

namespace Maui.FreakyControls.Shared.Converters;

public class TitleBackgroundColorConverter : BaseOneWayValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var borderType = (BorderType)value;
        var titleColor = (parameter as FreakyTextInputLayout)?.BackgroundColor ?? Colors.White;
        return borderType == BorderType.Outlined ? titleColor : Colors.Transparent;
    }
}