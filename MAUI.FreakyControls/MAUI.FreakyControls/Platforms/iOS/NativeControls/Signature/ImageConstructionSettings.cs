using NativeColor = UIKit.UIColor;
using NativeNullableColor = UIKit.UIColor;

namespace Maui.FreakyControls.Platforms.iOS;

public struct ImageConstructionSettings
{
    public static readonly bool DefaultShouldCrop = true;
    public static readonly SizeOrScale DefaultSizeOrScale = 1f;
    public static readonly NativeColor DefaultStrokeColor = NativeColor.Black;
    public static readonly NativeColor DefaultBackgroundColor = NativeColor.White;
    public static readonly float DefaultStrokeWidth = 2f;
    public static readonly float DefaultPadding = 5f;

    public bool? ShouldCrop { get; set; }

    public SizeOrScale? DesiredSizeOrScale { get; set; }

    public NativeNullableColor StrokeColor { get; set; }

    public NativeNullableColor BackgroundColor { get; set; }

    public float? StrokeWidth { get; set; }

    public float? Padding { get; set; }

    internal void ApplyDefaults()
    {
        ApplyDefaults(DefaultStrokeWidth, DefaultStrokeColor);
    }

    internal void ApplyDefaults(float strokeWidth, NativeColor strokeColor)
    {
        ShouldCrop = ShouldCrop ?? DefaultShouldCrop;
        DesiredSizeOrScale = DesiredSizeOrScale ?? DefaultSizeOrScale;
        StrokeColor = StrokeColor ?? strokeColor;
        BackgroundColor = BackgroundColor ?? DefaultBackgroundColor;
        StrokeWidth = StrokeWidth ?? strokeWidth;
        Padding = Padding ?? DefaultPadding;
    }
}