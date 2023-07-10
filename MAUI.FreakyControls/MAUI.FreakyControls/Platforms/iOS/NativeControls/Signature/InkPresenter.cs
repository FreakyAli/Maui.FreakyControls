using CoreGraphics;
using Foundation;
using UIKit;
using NativeColor = UIKit.UIColor;
using NativeImage = UIKit.UIImage;
using NativePath = UIKit.UIBezierPath;
using NativePoint = CoreGraphics.CGPoint;
using NativeRect = CoreGraphics.CGRect;

namespace Maui.FreakyControls.Platforms.iOS;

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

    // If you put SignaturePad inside a ScrollView, this line of code prevent that the gesture inside
    // an InkPresenter are dispatched to the ScrollView below
    public override bool GestureRecognizerShouldBegin(UIGestureRecognizer gestureRecognizer) => false;

    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
        // create a new path and set the options
        currentPath = new InkStroke(UIBezierPath.Create(), new List<CGPoint>(), StrokeColor, StrokeWidth);

        // obtain the location of the touch
        var touch = touches.AnyObject as UITouch;
        var touchLocation = touch.LocationInView(this);

        // move the path to that position
        currentPath.Path.MoveTo(touchLocation);
        currentPath.GetPoints().Add(touchLocation);

        // update the dirty rectangle
        ResetBounds(touchLocation);
    }

    public override void TouchesMoved(NSSet touches, UIEvent evt)
    {
        // something may have happened (clear) so start the stroke again
        if (currentPath == null)
        {
            TouchesBegan(touches, evt);
        }

        // obtain the location of the touch
        var touch = touches.AnyObject as UITouch;
        var touchLocation = touch.LocationInView(this);

        if (HasMovedFarEnough(currentPath, touchLocation.X, touchLocation.Y))
        {
            // add it to the current path
            currentPath.Path.AddLineTo(touchLocation);
            currentPath.GetPoints().Add(touchLocation);

            // update the dirty rectangle
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
        // obtain the location of the touch
        var touch = touches.AnyObject as UITouch;
        var touchLocation = touch.LocationInView(this);

        // something may have happened (clear) during the stroke
        if (currentPath != null)
        {
            if (HasMovedFarEnough(currentPath, touchLocation.X, touchLocation.Y))
            {
                // add it to the current path
                currentPath.Path.AddLineTo(touchLocation);
                currentPath.GetPoints().Add(touchLocation);
            }

            // obtain the smoothed path, and add it to the old paths
            var smoothed = PathSmoothing.SmoothedPathWithGranularity(currentPath, 4);
            paths.Add(smoothed);
        }

        // clear the current path
        currentPath = null;

        // update the dirty rectangle
        UpdateBounds(touchLocation);
        SetNeedsDisplay();

        // we are done with drawing
        OnStrokeCompleted();
    }

    public override void Draw(CGRect rect)
    {
        base.Draw(rect);

        // destroy an old bitmap
        if (bitmapBuffer != null && ShouldRedrawBufferImage)
        {
            var temp = bitmapBuffer;
            bitmapBuffer = null;

            temp.Dispose();
            temp = null;
        }

        // re-create
        if (bitmapBuffer == null)
        {
            bitmapBuffer = CreateBufferImage();
        }

        // if there are no lines, the the bitmap will be null
        if (bitmapBuffer != null)
        {
            bitmapBuffer.Draw(CGPoint.Empty);
        }

        // draw the current path over the old paths
        if (currentPath != null)
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

    private UIImage CreateBufferImage()
    {
        if (paths == null || paths.Count == 0)
        {
            return null;
        }

        var size = Bounds.Size;
        UIGraphics.BeginImageContextWithOptions(size, false, ScreenDensity);
        var context = UIGraphics.GetCurrentContext();

        context.SetLineCap(CGLineCap.Round);
        context.SetLineJoin(CGLineJoin.Round);

        foreach (var path in paths)
        {
            context.SetStrokeColor(path.Color.CGColor);
            context.SetLineWidth(path.Width);

            context.AddPath(path.Path.CGPath);
            context.StrokePath();

            path.IsDirty = false;
        }

        var image = UIGraphics.GetImageFromCurrentImageContext();

        UIGraphics.EndImageContext();

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

    // used to determine rectangle that needs to be redrawn
    private float dirtyRectLeft;
    private float dirtyRectTop;
    private float dirtyRectRight;
    private float dirtyRectBottom;

    private NativeImage bitmapBuffer;

    // public properties

    public NativeColor StrokeColor { get; set; } = NativeColor.Black;

    public float StrokeWidth { get; set; } = 1f;

    // private properties

#if __IOS__
    private float Width => (float)Bounds.Width;

    private float Height => (float)Bounds.Height;
#endif

    private bool ShouldRedrawBufferImage
    {
        get
        {
            var sizeChanged = false;
            if (bitmapBuffer != null)
            {
                var s = bitmapBuffer.GetSize();
                sizeChanged = s.Width != Width || s.Height != Height;
            }

            return sizeChanged ||
                (bitmapBuffer != null && paths.Count == 0) ||
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

    // public events

    public event EventHandler StrokeCompleted;

    // public methods

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

        if (strokePoints == null || strokePoints.Count == 0)
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

    // private methods

    private bool HasMovedFarEnough(InkStroke stroke, double touchX, double touchY)
    {
        var lastPoint = stroke.GetPoints().LastOrDefault();
        var deltaX = touchX - lastPoint.X;
        var deltaY = touchY - lastPoint.Y;

        var distance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
        return distance >= MinimumPointDistance;
    }

    /// <summary>
    /// Update the bounds for the rectangle to be redrawn if necessary for the given point.
    /// </summary>
    private void UpdateBounds(NativePoint touch)
    {
        UpdateBounds((float)touch.X, (float)touch.Y);
    }

    /// <summary>
    /// Update the bounds for the rectangle to be redrawn if necessary for the given point.
    /// </summary>
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

    /// <summary>
    /// Set the bounds for the rectangle that will need to be redrawn to show the drawn path.
    /// </summary>
    private void ResetBounds(NativePoint touch)
    {
        ResetBounds((float)touch.X, (float)touch.Y);
    }

    /// <summary>
    /// Set the bounds for the rectangle that will need to be redrawn to show the drawn path.
    /// </summary>
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