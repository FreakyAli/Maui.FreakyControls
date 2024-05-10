using System.Globalization;
using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls.Shared.Converters;

public class LeftImageAlignmentToVisibilityConverter : BaseOneWayValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is ImageAlignment alignment && (alignment == ImageAlignment.Left);
    }
}