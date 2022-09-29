using System;
using System.Globalization;
using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls.Shared.Converters
{
    public class TitleBackgroundColorConverter : BaseOneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var borderType = (BorderType)value;
            var titleColor = (parameter as FreakyTextInputLayout)?.BackgroundColor;
            return borderType == BorderType.Outlined ? titleColor : Colors.Transparent;
        }
    }
}

