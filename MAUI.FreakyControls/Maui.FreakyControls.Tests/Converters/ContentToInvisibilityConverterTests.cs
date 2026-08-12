using System.Globalization;
using Maui.FreakyControls.Converters;

namespace Maui.FreakyControls.Tests.Converters;

public class ContentToInvisibilityConverterTests
{
    private readonly ContentToInvisibilityConverter _converter = new();

    [Fact]
    public void Convert_Null_ReturnsFalse()
    {
        var result = _converter.Convert(null, typeof(bool), null, CultureInfo.InvariantCulture);
        Assert.False(result is bool b && b);
    }

    [Theory]
    [InlineData("some content")]
    [InlineData(42)]
    [InlineData(true)]
    public void Convert_NonNullValue_ReturnsTrue(object value)
    {
        var result = _converter.Convert(value, typeof(bool), null, CultureInfo.InvariantCulture);
        Assert.True(result is true);
    }

    [Fact]
    public void ConvertBack_ThrowsNotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() =>
            _converter.ConvertBack(true, typeof(bool), null, CultureInfo.InvariantCulture));
    }
}
