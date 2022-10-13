using NativePoint = System.Drawing.PointF;
using NativeColor = Android.Graphics.Color;
using NativePath = Android.Graphics.Path;

namespace Maui.FreakyControls.Platforms.Android;

internal class InkStroke
{
    private NativeColor color;
    private float width;
    private IList<NativePoint> inkPoints;

    public InkStroke(NativePath path, IList<NativePoint> points, NativeColor color, float width)
    {
        Path = path;
        inkPoints = points;
        NativeColor = color;
        Width = width;
    }

    public NativePath Path { get; private set; }

    public IList<NativePoint> GetPoints()
    {
        return inkPoints;
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
}





