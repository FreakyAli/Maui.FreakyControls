using System.Globalization;
using Maui.FreakyControls.Converters;

namespace Maui.FreakyControls.Tests.Converters;

public class InverseBoolConverterTests
{
    private readonly InverseBoolConverter _converter = new();

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void Convert_BoolValue_ReturnsInverse(bool input, bool expected)
    {
        var result = _converter.Convert(input, typeof(bool), null, CultureInfo.InvariantCulture);
        Assert.Equal(expected, (bool)result);
    }

    [Theory]
    [InlineData("not a bool")]
    [InlineData(42)]
    [InlineData(3.14)]
    public void Convert_NonBoolValue_ThrowsArgumentException(object value)
    {
        Assert.Throws<ArgumentException>(() =>
            _converter.Convert(value, typeof(bool), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Convert_Null_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _converter.Convert(null, typeof(bool), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ConvertBack_ThrowsNotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() =>
            _converter.ConvertBack(true, typeof(bool), null, CultureInfo.InvariantCulture));
    }
}
