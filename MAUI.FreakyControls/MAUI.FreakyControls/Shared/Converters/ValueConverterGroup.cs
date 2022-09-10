using System.Globalization;

namespace Maui.FreakyControls.Shared.Converters
{
    /// <summary>
    /// Value converter group that will chain converters together.
    /// </summary>
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{GetType().Name} is a one-way converter");
        }
    }
}

