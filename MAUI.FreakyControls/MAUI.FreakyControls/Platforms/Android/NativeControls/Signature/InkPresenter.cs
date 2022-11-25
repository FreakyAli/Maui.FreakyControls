using Android.Content;
using Android.Views;
using System.Drawing;
using Android.Graphics;

using NativeRect = System.Drawing.RectangleF;
using NativePoint = System.Drawing.PointF;
using NativeColor = Android.Graphics.Color;
using NativeImage = Android.Graphics.Bitmap;
using NativePath = Android.Graphics.Path;
using Application = Android.App.Application;
using View = Android.Views.View;
using Path = Android.Graphics.Path;
using Paint = Android.Graphics.Paint;
using Rect = Android.Graphics.Rect;
using SizeF = System.Drawing.SizeF;

namespace Maui.FreakyControls.Platforms.Android;

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

partial class InkPresenter : View
{
    static InkPresenter()
    {
        // we may be in a designer
        if (Application.Context == null)
        {
            ScreenDensity = 1f;
            return;
        }

        var density = Platform.CurrentActivity?.GetDensity();
        ScreenDensity = density.HasValue ? density.Value : 0f;
    }

    public InkPresenter(Context context)
        : base(context)
    {
        Initialize();
    }

    private void Initialize()
    {
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
        switch (e.Action)
        {
            case MotionEventActions.Down:
                TouchesBegan(e);
                return true;
            case MotionEventActions.Move:
                TouchesMoved(e);
                return true;
            case MotionEventActions.Up:
                TouchesEnded(e);
                return true;
        }
        return false;
    }

    private void TouchesBegan(MotionEvent e)
    {
        // don't allow the event to propagate because we're handling it here
        Parent?.RequestDisallowInterceptTouchEvent(true);

        // create a new path and set the options
        currentPath = new InkStroke(new Path(), new List<System.Drawing.PointF>(), StrokeColor, StrokeWidth);

        // obtain the location of the touch
        float touchX = e.GetX();
        float touchY = e.GetY();

        // move to the touched point
        currentPath.Path.MoveTo(touchX, touchY);
        currentPath.GetPoints().Add(new System.Drawing.PointF(touchX, touchY));

        // update the dirty rectangle
        ResetBounds(touchX, touchY);
        Invalidate(DirtyRect);
    }

    private void TouchesMoved(MotionEvent e, bool update = true)
    {
        // something may have happened (clear) so start the stroke again
        if (currentPath == null)
        {
            TouchesBegan(e);
        }

        var hasMoved = false;

        for (var i = 0; i < e.HistorySize; i++)
        {
            float historicalX = e.GetHistoricalX(i);
            float historicalY = e.GetHistoricalY(i);

            if (HasMovedFarEnough(currentPath, historicalX, historicalY))
            {
                // update the dirty rectangle
                UpdateBounds(historicalX, historicalY);
                hasMoved = true;

                // add it to the current path
                currentPath.Path.LineTo(historicalX, historicalY);
                currentPath.GetPoints().Add(new System.Drawing.PointF(historicalX, historicalY));
            }
        }

        float touchX = e.GetX();
        float touchY = e.GetY();

        if (HasMovedFarEnough(currentPath, touchX, touchY))
        {
            // add it to the current path
            currentPath.Path.LineTo(touchX, touchY);
            currentPath.GetPoints().Add(new System.Drawing.PointF(touchX, touchY));

            // update the dirty rectangle
            UpdateBounds(touchX, touchY);
            hasMoved = true;
        }

        if (update && hasMoved)
        {
            Invalidate(DirtyRect);
        }
    }

    private void TouchesEnded(MotionEvent e)
    {
        // something may have happened (clear) during the stroke
        if (currentPath != null)
        {
            TouchesMoved(e, false);

            // add the current path and points to their respective lists.
            var smoothed = PathSmoothing.SmoothedPathWithGranularity(currentPath, 2);
            paths.Add(smoothed);
        }

        // reset the drawing
        currentPath = null;

        // update the dirty rectangle
        Invalidate(DirtyRect);

        // we are done with drawing
        OnStrokeCompleted();

        // allow the event to propagate
        Parent?.RequestDisallowInterceptTouchEvent(false);
    }

    private void Invalidate(RectangleF dirtyRect)
    {
#if ANDROID28_0_OR_GREATER
        Invalidate();
#else
        using (var rect = new Rect(
            (int)(dirtyRect.Left - 0.5f),
            (int)(dirtyRect.Top - 0.5f),
            (int)(dirtyRect.Right + 0.5f),
            (int)(dirtyRect.Bottom + 0.5f)))
        {
            Invalidate(rect);
        }
#endif
    }

    protected override void OnDraw(Canvas canvas)
    {
        base.OnDraw(canvas);

        // destroy an old bitmap
        if (bitmapBuffer != null && ShouldRedrawBufferImage)
        {
            var temp = bitmapBuffer;
            bitmapBuffer = null;

            temp.Recycle();
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
            canvas.DrawBitmap(bitmapBuffer, 0, 0, null);
        }

        // draw the current path over the old paths
        if (currentPath != null)
        {
            using (var paint = new Paint())
            {
                paint.StrokeJoin = Paint.Join.Round;
                paint.StrokeCap = Paint.Cap.Round;
                paint.AntiAlias = true;
                paint.SetStyle(Paint.Style.Stroke);

                paint.Color = currentPath.NativeColor;
                paint.StrokeWidth = currentPath.Width * ScreenDensity;

                canvas.DrawPath(currentPath.Path, paint);
            }
        }
    }

    private Bitmap CreateBufferImage()
    {
        if (paths == null || paths.Count == 0)
        {
            return null;
        }

        var size = new SizeF(Width, Height);
        var image = Bitmap.CreateBitmap((int)size.Width, (int)size.Height, Bitmap.Config.Argb8888);

        using (var canvas = new Canvas(image))
        using (var paint = new Paint())
        {
            paint.StrokeJoin = Paint.Join.Round;
            paint.StrokeCap = Paint.Cap.Round;
            paint.AntiAlias = true;
            paint.SetStyle(Paint.Style.Stroke);

            foreach (var path in paths)
            {
                paint.Color = path.NativeColor;
                paint.StrokeWidth = path.Width * ScreenDensity;

                canvas.DrawPath(path.Path, paint);

                path.IsDirty = false;
            }
        }

        return image;
    }
}