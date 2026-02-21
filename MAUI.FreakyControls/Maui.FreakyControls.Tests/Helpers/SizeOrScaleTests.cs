using Maui.FreakyControls.Enums;

namespace Maui.FreakyControls.Tests.Helpers;

public class SizeOrScaleTests
{
    [Theory]
    [InlineData(1f, 1f, true)]
    [InlineData(0.5f, 0.5f, true)]
    [InlineData(0f, 1f, false)]
    [InlineData(1f, 0f, false)]
    [InlineData(-1f, 1f, false)]
    [InlineData(0f, 0f, false)]
    public void IsValid_ReflectsPositiveXAndY(float x, float y, bool expected)
    {
        var sizeOrScale = new SizeOrScale(x, y, SizeOrScaleType.Scale);
        Assert.Equal(expected, sizeOrScale.IsValid);
    }

    [Fact]
    public void Constructor_XY_SetsXAndYEqually()
    {
        var s = new SizeOrScale(2f, SizeOrScaleType.Scale);
        Assert.Equal(2f, s.X);
        Assert.Equal(2f, s.Y);
    }

    [Fact]
    public void Constructor_XY_DefaultsKeepAspectRatioTrue()
    {
        var s = new SizeOrScale(2f, SizeOrScaleType.Scale);
        Assert.True(s.KeepAspectRatio);
    }

    [Fact]
    public void Constructor_XYKeepAspect_SetsKeepAspectRatio()
    {
        var s = new SizeOrScale(2f, SizeOrScaleType.Scale, keepAspectRatio: false);
        Assert.False(s.KeepAspectRatio);
    }

    [Fact]
    public void Constructor_SeparateXY_SetsXAndYIndependently()
    {
        var s = new SizeOrScale(3f, 4f, SizeOrScaleType.Scale);
        Assert.Equal(3f, s.X);
        Assert.Equal(4f, s.Y);
    }

    [Fact]
    public void GetScale_WhenTypeIsScale_ReturnsXAndYDirectly()
    {
        var s = new SizeOrScale(2f, SizeOrScaleType.Scale);
        var scale = s.GetScale(100f, 200f);
        Assert.Equal(2.0, scale.Width);
        Assert.Equal(2.0, scale.Height);
    }

    [Fact]
    public void GetScale_WhenTypeIsSize_DividesByWidthAndHeight()
    {
        var s = new SizeOrScale(50f, 100f, SizeOrScaleType.Size);
        var scale = s.GetScale(200f, 400f);
        Assert.Equal(0.25, scale.Width, precision: 5);
        Assert.Equal(0.25, scale.Height, precision: 5);
    }

    [Fact]
    public void GetSize_WhenTypeIsScale_MultipliesWidthAndHeight()
    {
        var s = new SizeOrScale(2f, SizeOrScaleType.Scale);
        var size = s.GetSize(100f, 200f);
        Assert.Equal(200.0, size.Width);
        Assert.Equal(400.0, size.Height);
    }

    [Fact]
    public void GetSize_WhenTypeIsSize_ReturnsXAndYDirectly()
    {
        var s = new SizeOrScale(50f, 75f, SizeOrScaleType.Size);
        var size = s.GetSize(100f, 200f);
        Assert.Equal(50.0, size.Width);
        Assert.Equal(75.0, size.Height);
    }

    [Fact]
    public void ImplicitConversion_FromFloat_CreatesScaleType()
    {
        SizeOrScale s = 3f;
        Assert.Equal(3f, s.X);
        Assert.Equal(3f, s.Y);
        Assert.Equal(SizeOrScaleType.Scale, s.Type);
    }
}
