using System.Globalization;
using Maui.FreakyControls.Converters;
using Maui.FreakyControls.Enums;

namespace Maui.FreakyControls.Tests.Converters;

public class RightImageAlignmentToVisibilityConverterTests
{
    private readonly RightImageAlignmentToVisibilityConverter _converter = new();

    [Fact]
    public void Convert_Right_ReturnsTrue()
    {
        var result = _converter.Convert(ImageAlignment.Right, typeof(bool), null, CultureInfo.InvariantCulture);
        Assert.True(result is true);
    }

    [Theory]
    [InlineData(ImageAlignment.Left)]
    public void Convert_NonRightAlignment_ReturnsFalse(ImageAlignment alignment)
    {
        var result = _converter.Convert(alignment, typeof(bool), null, CultureInfo.InvariantCulture);
        Assert.False(Assert.IsType<bool>(result));
    }

    [Theory]
    [InlineData("Right")]
    [InlineData(null)]
    [InlineData(0)]
    public void Convert_NonImageAlignmentValue_ReturnsFalse(object? value)
    {
        var result = _converter.Convert(value, typeof(bool), null, CultureInfo.InvariantCulture);
        Assert.False(Assert.IsType<bool>(result));
    }

    [Fact]
    public void ConvertBack_ThrowsNotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() =>
            _converter.ConvertBack(true, typeof(bool), null, CultureInfo.InvariantCulture));
    }
}
