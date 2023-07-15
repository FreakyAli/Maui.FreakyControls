using NativeColor = Android.Graphics.Color;
using NativePath = Android.Graphics.Path;
using NativePoint = System.Drawing.PointF;

namespace Maui.FreakyControls.Platforms.Android;

internal class InkStroke
{
    private NativeColor color;
    private IList<NativePoint> inkPoints;
    private float width;

    public InkStroke(NativePath path, IList<NativePoint> points, NativeColor color, float width)
    {
        Path = path;
        inkPoints = points;
        NativeColor = color;
        Width = width;
    }

    public NativeColor NativeColor
    {
        get { return color; }
        set
        {
            color = value;
            IsDirty = true;
        }
    }

    public NativePath Path { get; private set; }

    public float Width
    {
        get { return width; }
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