using System.Globalization;

namespace MAUI.FreakyControls.Shared.Converters
{
    public class InverseBoolConverter : BaseOneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue))
            {
                throw new ArgumentException("Value must be a boolean", nameof(value));
            }

            return !boolValue;
        }
    }
}

