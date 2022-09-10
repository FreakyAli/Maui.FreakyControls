using System;
using System.Globalization;

namespace Maui.FreakyControls.Shared.Converters
{
    public class StrokeThicknessConverter : BaseOneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)(value) ? 0 : (int)parameter;
        }
    }
}

