using NativeColor = Android.Graphics.Color;
using NativeNullableColor = System.Nullable<Android.Graphics.Color>;

namespace Maui.FreakyControls.Platforms.Android;

public struct ImageConstructionSettings
{
    public static readonly NativeColor DefaultBackgroundColor = NativeColor.White;
    public static readonly float DefaultPadding = 5f;
    public static readonly bool DefaultShouldCrop = true;
    public static readonly SizeOrScale DefaultSizeOrScale = 1f;
    public static readonly NativeColor DefaultStrokeColor = NativeColor.Black;
    public static readonly float DefaultStrokeWidth = 2f;
    public NativeNullableColor BackgroundColor { get; set; }
    public SizeOrScale? DesiredSizeOrScale { get; set; }
    public float? Padding { get; set; }
    public bool? ShouldCrop { get; set; }
    public NativeNullableColor StrokeColor { get; set; }
    public float? StrokeWidth { get; set; }

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