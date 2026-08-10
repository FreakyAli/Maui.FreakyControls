#nullable disable

using CoreGraphics;
using Foundation;
using UIKit;
using NativeColor = UIKit.UIColor;
using NativeImage = UIKit.UIImage;
using NativePath = UIKit.UIBezierPath;
using NativePoint = CoreGraphics.CGPoint;
using NativeRect = CoreGraphics.CGRect;

namespace Maui.FreakyControls.Platforms.Apple;

internal partial class InkPresenter : UIView
{
    static InkPresenter()
    {
        ScreenDensity = (float)UIScreen.MainScreen.Scale;
    }

    public InkPresenter()
        : base()
    {
        Initialize();
    }

    public InkPresenter(CGRect frame)
        : base(frame)
    {
        Initialize();
    }

    private void Initialize()
    {
        Opaque = false;
    }

    public override bool GestureRecognizerShouldBegin(UIGestureRecognizer gestureRecognizer) => false;

    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
        currentPath = new InkStroke(UIBezierPath.Create(), new List<CGPoint>(), StrokeColor, StrokeWidth);

        var touch = touches.AnyObject as UITouch;
        var touchLocation = touch.LocationInView(this);

        currentPath.Path.MoveTo(touchLocation);
        currentPath.GetPoints().Add(touchLocation);

        ResetBounds(touchLocation);
    }

    public override void TouchesMoved(NSSet touches, UIEvent evt)
    {
        if (currentPath is null)
        {
            TouchesBegan(touches, evt);
        }

        var touch = touches.AnyObject as UITouch;
        var touchLocation = touch.LocationInView(this);

        if (HasMovedFarEnough(currentPath, touchLocation.X, touchLocation.Y))
        {
            currentPath.Path.AddLineTo(touchLocation);
            currentPath.GetPoints().Add(touchLocation);

            UpdateBounds(touchLocation);
            SetNeedsDisplayInRect(DirtyRect);
        }
    }

    public override void TouchesCancelled(NSSet touches, UIEvent evt)
    {
        TouchesEnded(touches, evt);
    }

    public override void TouchesEnded(NSSet touches, UIEvent evt)
    {
        var touch = touches.AnyObject as UITouch;
        var touchLocation = touch.LocationInView(this);

        if (currentPath is not null)
        {
            if (HasMovedFarEnough(currentPath, touchLocation.X, touchLocation.Y))
            {
                currentPath.Path.AddLineTo(touchLocation);
                currentPath.GetPoints().Add(touchLocation);
            }

            var smoothed = PathSmoothing.SmoothedPathWithGranularity(currentPath, 4);
            paths.Add(smoothed);
        }

        currentPath = null;

        UpdateBounds(touchLocation);
        SetNeedsDisplay();

        OnStrokeCompleted();
    }

    public override void Draw(CGRect rect)
    {
        base.Draw(rect);

        if (bitmapBuffer is not null && ShouldRedrawBufferImage)
        {
            var temp = bitmapBuffer;
            bitmapBuffer = null;

            temp.Dispose();
            temp = null;
        }

        if (bitmapBuffer is null)
        {
            bitmapBuffer = CreateBufferImage();
        }

        if (bitmapBuffer is not null)
        {
            bitmapBuffer.Draw(CGPoint.Empty);
        }

        if (currentPath is not null)
        {
            var context = UIGraphics.GetCurrentContext();
            context.SetLineCap(CGLineCap.Round);
            context.SetLineJoin(CGLineJoin.Round);
            context.SetStrokeColor(currentPath.Color.CGColor);
            context.SetLineWidth(currentPath.Width);

            context.AddPath(currentPath.Path.CGPath);
            context.StrokePath();
        }
    }

    private NativeImage CreateBufferImage()
    {
        if (paths is null || paths.Count == 0)
        {
            return null;
        }

        var size = Bounds.Size;

        var renderer = new UIGraphicsImageRenderer(size, new UIGraphicsImageRendererFormat { Opaque = false, Scale = ScreenDensity });

        var image = renderer.CreateImage((context) =>
        {
            var cgcontext = context.CGContext;
            cgcontext.SetLineCap(CGLineCap.Round);
            cgcontext.SetLineJoin(CGLineJoin.Round);

            foreach (var path in paths)
            {
                cgcontext.SetStrokeColor(path.Color.CGColor);
                cgcontext.SetLineWidth(path.Width);

                cgcontext.AddPath(path.Path.CGPath);
                cgcontext.StrokePath();

                path.IsDirty = false;
            }
        });

        return image;
    }

    public override void LayoutSubviews()
    {
        base.LayoutSubviews();

        SetNeedsDisplay();
    }
}

internal partial class InkPresenter
{
    private const float MinimumPointDistance = 2.0f;

    public static float ScreenDensity;

    private readonly List<InkStroke> paths = new List<InkStroke>();
    private InkStroke currentPath;

    private float dirtyRectLeft;
    private float dirtyRectTop;
    private float dirtyRectRight;
    private float dirtyRectBottom;

    private NativeImage bitmapBuffer;

    public NativeColor StrokeColor { get; set; } = NativeColor.Black;

    public float StrokeWidth { get; set; } = 1f;

#if __IOS__
    private float Width => (float)Bounds.Width;

    private float Height => (float)Bounds.Height;
#endif

    private bool ShouldRedrawBufferImage
    {
        get
        {
            var sizeChanged = false;
            if (bitmapBuffer is not null)
            {
                var s = bitmapBuffer.GetSize();
                sizeChanged = s.Width != Width || s.Height != Height;
            }

            return sizeChanged ||
                (bitmapBuffer is not null && paths.Count == 0) ||
                paths.Any(p => p.IsDirty);
        }
    }

    private NativeRect DirtyRect
    {
        get
        {
            var x = Math.Min(dirtyRectLeft, dirtyRectRight);
            var y = Math.Min(dirtyRectTop, dirtyRectBottom);
            var w = Math.Abs(dirtyRectRight - dirtyRectLeft);
            var h = Math.Abs(dirtyRectBottom - dirtyRectTop);
            var half = StrokeWidth / 2f;
            return new NativeRect(x - half, y - half, w + StrokeWidth, h + StrokeWidth);
        }
    }

    public event EventHandler StrokeCompleted;

    public IReadOnlyList<InkStroke> GetStrokes()
    {
        return paths;
    }

    public void Clear()
    {
        paths.Clear();
        currentPath = null;

        this.Invalidate();
    }

    public void AddStroke(NativePoint[] strokePoints, NativeColor color, float width)
    {
        if (AddStrokeInternal(strokePoints, color, width))
        {
            this.Invalidate();
        }
    }

    public void AddStrokes(IEnumerable<NativePoint[]> strokes, NativeColor color, float width)
    {
        var changed = false;

        foreach (var stroke in strokes)
        {
            if (AddStrokeInternal(stroke, color, width))
            {
                changed = true;
            }
        }

        if (changed)
        {
            this.Invalidate();
        }
    }

    private bool AddStrokeInternal(IEnumerable<NativePoint> points, NativeColor color, float width)
    {
        var strokePoints = points?.ToList();

        if (strokePoints is null || strokePoints.Count == 0)
        {
            return false;
        }

        var newpath = new NativePath();
        newpath.MoveTo(strokePoints[0].X, strokePoints[0].Y);
        foreach (var point in strokePoints.Skip(1))
        {
            newpath.LineTo(point.X, point.Y);
        }

        paths.Add(new InkStroke(newpath, strokePoints, color, width));

        return true;
    }

    private bool HasMovedFarEnough(InkStroke stroke, double touchX, double touchY)
    {
        var lastPoint = stroke.GetPoints().LastOrDefault();
        var deltaX = touchX - lastPoint.X;
        var deltaY = touchY - lastPoint.Y;

        var distance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
        return distance >= MinimumPointDistance;
    }

    private void UpdateBounds(NativePoint touch)
    {
        UpdateBounds((float)touch.X, (float)touch.Y);
    }

    private void UpdateBounds(float touchX, float touchY)
    {
        if (touchX < dirtyRectLeft)
            dirtyRectLeft = touchX;
        else if (touchX > dirtyRectRight)
            dirtyRectRight = touchX;

        if (touchY < dirtyRectTop)
            dirtyRectTop = touchY;
        else if (touchY > dirtyRectBottom)
            dirtyRectBottom = touchY;
    }

    private void ResetBounds(NativePoint touch)
    {
        ResetBounds((float)touch.X, (float)touch.Y);
    }

    private void ResetBounds(float touchX, float touchY)
    {
        dirtyRectLeft = touchX;
        dirtyRectRight = touchX;
        dirtyRectTop = touchY;
        dirtyRectBottom = touchY;
    }

    private void OnStrokeCompleted()
    {
        StrokeCompleted?.Invoke(this, EventArgs.Empty);
    }
}
