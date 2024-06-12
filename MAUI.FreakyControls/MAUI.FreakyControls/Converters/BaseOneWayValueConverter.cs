using System.Globalization;

namespace Maui.FreakyControls.Converters;

public abstract class BaseOneWayValueConverter : IValueConverter
{
    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException($"{GetType().Name} is a one-way converter");
    }
}