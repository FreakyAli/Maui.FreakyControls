using System;
using System.Globalization;
using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls.Shared.Converters
{
    public class TilPaddingConverter : BaseOneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var borderType = (BorderType)value;
#if ANDROID
            var emptyThickness = new Thickness(10, 0, 10, 0);
#endif
#if IOS
            var emptyThickness = new Thickness(10);

#endif
            var fullThickness = new Thickness(10);
            return borderType == BorderType.None ? emptyThickness : borderType == BorderType.Underline ? emptyThickness : fullThickness;
        }
    }
}

