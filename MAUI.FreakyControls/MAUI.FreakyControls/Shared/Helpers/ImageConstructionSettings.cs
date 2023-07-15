namespace Maui.FreakyControls;

public struct ImageConstructionSettings
{
    public static readonly Color DefaultBackgroundColor = Colors.Transparent;
    public static readonly float DefaultPadding = 5f;
    public static readonly bool DefaultShouldCrop = true;
    public static readonly SizeOrScale DefaultSizeOrScale = 1f;
    public static readonly Color DefaultStrokeColor = Colors.Black;
    public static readonly float DefaultStrokeWidth = 2f;
    public Color? BackgroundColor { get; set; }
    public SizeOrScale? DesiredSizeOrScale { get; set; }
    public float? Padding { get; set; }
    public bool? ShouldCrop { get; set; }
    public Color? StrokeColor { get; set; }
    public float? StrokeWidth { get; set; }
}