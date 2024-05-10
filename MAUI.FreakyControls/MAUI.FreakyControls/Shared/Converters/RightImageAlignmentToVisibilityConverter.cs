using Maui.FreakyControls.Shared.Enums;
using System.Globalization;

namespace Maui.FreakyControls.Shared.Converters;

public class RightImageAlignmentToVisibilityConverter : BaseOneWayValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is ImageAlignment alignment && (alignment == ImageAlignment.Right);
    }
}