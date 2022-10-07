namespace Maui.FreakyControls;

public struct ImageConstructionSettings
{
    public static readonly bool DefaultShouldCrop = true;
    public static readonly SizeOrScale DefaultSizeOrScale = 1f;
    public static readonly Color DefaultStrokeColor = Colors.Black;
    public static readonly Color DefaultBackgroundColor = Colors.Transparent;
    public static readonly float DefaultStrokeWidth = 2f;
    public static readonly float DefaultPadding = 5f;

    public bool? ShouldCrop { get; set; }

    public SizeOrScale? DesiredSizeOrScale { get; set; }

    public Color? StrokeColor { get; set; }

    public Color? BackgroundColor { get; set; }

    public float? StrokeWidth { get; set; }

    public float? Padding { get; set; }
}