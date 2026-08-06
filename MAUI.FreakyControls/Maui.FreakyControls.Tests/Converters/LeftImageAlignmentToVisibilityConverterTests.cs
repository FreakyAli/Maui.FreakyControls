using System.Globalization;
using Maui.FreakyControls.Converters;
using Maui.FreakyControls.Enums;

namespace Maui.FreakyControls.Tests.Converters;

public class LeftImageAlignmentToVisibilityConverterTests
{
    private readonly LeftImageAlignmentToVisibilityConverter _converter = new();

    [Fact]
    public void Convert_Left_ReturnsTrue()
    {
        var result = _converter.Convert(ImageAlignment.Left, typeof(bool), null, CultureInfo.InvariantCulture);
        Assert.True((bool)result);
    }

    [Theory]
    [InlineData(ImageAlignment.Right)]
    public void Convert_NonLeftAlignment_ReturnsFalse(ImageAlignment alignment)
    {
        var result = _converter.Convert(alignment, typeof(bool), null, CultureInfo.InvariantCulture);
        Assert.False((bool)result);
    }

    [Theory]
    [InlineData("Left")]
    [InlineData(null)]
    [InlineData(0)]
    public void Convert_NonImageAlignmentValue_ReturnsFalse(object? value)
    {
        var result = _converter.Convert(value, typeof(bool), null, CultureInfo.InvariantCulture);
        Assert.False((bool)result);
    }

    [Fact]
    public void ConvertBack_ThrowsNotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() =>
            _converter.ConvertBack(true, typeof(bool), null, CultureInfo.InvariantCulture));
    }
}
