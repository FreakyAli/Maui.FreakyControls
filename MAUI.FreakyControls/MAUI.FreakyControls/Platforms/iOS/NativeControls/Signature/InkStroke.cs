using NativeColor = UIKit.UIColor;
using NativePath = UIKit.UIBezierPath;
using NativePoint = CoreGraphics.CGPoint;

namespace Maui.FreakyControls.Platforms.iOS;

internal class InkStroke
{
    private NativeColor color;
    private readonly IList<NativePoint> inkPoints;
    private float width;

    public InkStroke(NativePath path, IList<NativePoint> points, NativeColor color, float width)
    {
        Path = path;
        inkPoints = points;
        Color = color;
        Width = width;
    }

    public NativePath Path { get; private set; }

    public NativeColor Color
    {
        get => color;
        set
        {
            color = value;
            IsDirty = true;
        }
    }

    public float Width
    {
        get => width;
        set
        {
            width = value;
            IsDirty = true;
        }
    }

    internal bool IsDirty { get; set; }

    public IList<NativePoint> GetPoints()
    {
        return inkPoints;
    }
}