using Maui.FreakyControls.Shared.Enums;
using System.Globalization;

namespace Maui.FreakyControls.Shared.Converters;

public class TilPaddingConverter : BaseOneWayValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var borderType = (BorderType)value;
#if ANDROID
        var emptyThickness = new Thickness(10, 0, 10, 0);
#else
        var emptyThickness = new Thickness(10);
#endif
        var fullThickness = new Thickness(10);
        return borderType == BorderType.None ? emptyThickness :
            borderType == BorderType.Underline ? emptyThickness : fullThickness;
    }
}