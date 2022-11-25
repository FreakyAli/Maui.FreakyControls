using System.Globalization;

namespace Maui.FreakyControls.Shared.Converters;

/// <summary>
/// String format converter.
/// This converter is safer than using MAUI' built-in Binding.StringFormat
/// because it will not throw an exception if the format is not a valid input string.
/// Instead, this will return the formatted string (if it is valid), the format string (if it is not valid),
/// or the value (if the parameter is not a string).
/// </summary>
public class StringFormatConverter : BaseOneWayValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // default the return value to the input value
        object result = value;
        try
        {
            if (targetType != typeof(string))
                throw new ArgumentException($"Converter can only convert to string, {targetType} is not a supported target type", nameof(targetType));

            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter), "Converter parameter must not be null");

            if (!(parameter is string format))
                throw new ArgumentException($"Converter parameter must be a string, {parameter.GetType()} is not supported", nameof(parameter));

            // if the parameter is a string, update the return value to the string
            result = parameter;

            // attempt to format the value using the input string and update the return value
            result = string.Format(culture, format, value);
        }
        catch
        {
            // suppress any exceptions as this will be used in XAML definitions
        }

        return result;
    }
}