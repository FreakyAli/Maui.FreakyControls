using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Maui.FreakyControls.Enums;
using FrameLayout = Android.Widget.FrameLayout;
using NativeColor = Android.Graphics.Color;
using NativeImage = Android.Graphics.Bitmap;
using NativePoint = System.Drawing.PointF;
using NativeRect = System.Drawing.RectangleF;
using NativeSize = System.Drawing.SizeF;
using Paint = Android.Graphics.Paint;

namespace Maui.FreakyControls.Platforms.Android;

public partial class SignaturePadCanvasView : FrameLayout
{
    private InkPresenter inkPresenter;

    public SignaturePadCanvasView(Context context)
        : base(context)
    {
        Initialize();
    }

    public SignaturePadCanvasView(Context context, IAttributeSet attrs)
        : base(context, attrs)
    {
        Initialize();
    }

    public SignaturePadCanvasView(Context context, IAttributeSet attrs, int defStyle)
        : base(context, attrs, defStyle)
    {
        Initialize();
    }

    private void Initialize()
    {
        inkPresenter = new InkPresenter(Context)
        {
            LayoutParameters = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.MatchParent)
        };
        inkPresenter.StrokeCompleted += OnStrokeCompleted;
        AddView(inkPresenter);

        StrokeWidth = ImageConstructionSettings.DefaultStrokeWidth;
        StrokeColor = ImageConstructionSettings.DefaultStrokeColor;
    }

    /// <summary>
    /// Gets or sets the color of the strokes for the signature.
    /// </summary>
    /// <value>The color of the stroke.</value>
    public NativeColor StrokeColor
    {
        get { return inkPresenter.StrokeColor; }
        set
        {
            inkPresenter.StrokeColor = value;
            foreach (var stroke in inkPresenter.GetStrokes())
            {
                stroke.NativeColor = value;
            }
            inkPresenter.Invalidate();
        }
    }

    /// <summary>
    /// Gets or sets the width in pixels of the strokes for the signature.
    /// </summary>
    /// <value>The width of the line.</value>
    public float StrokeWidth
    {
        get { return inkPresenter.StrokeWidth; }
        set
        {
            inkPresenter.StrokeWidth = value;
            foreach (var stroke in inkPresenter.GetStrokes())
            {
                stroke.Width = value;
            }
            inkPresenter.Invalidate();
        }
    }

    public void Clear()
    {
        inkPresenter.Clear();

        OnCleared();
    }

    private Bitmap GetImageInternal(System.Drawing.SizeF scale, System.Drawing.RectangleF signatureBounds, System.Drawing.SizeF imageSize, float strokeWidth, NativeColor strokeColor, NativeColor backgroundColor)
    {
        // create bitmap and set the desired options
        var image = Bitmap.CreateBitmap((int)imageSize.Width, (int)imageSize.Height, Bitmap.Config.Argb8888);
        using (var canvas = new Canvas(image))
        {
            // background
            canvas.DrawColor(backgroundColor);

            // cropping / scaling
            canvas.Scale(scale.Width, scale.Height);
            canvas.Translate(-signatureBounds.Left, -signatureBounds.Top);

            // strokes
            using (var paint = new Paint())
            {
                paint.Color = strokeColor;
                paint.StrokeWidth = strokeWidth * InkPresenter.ScreenDensity;
                paint.StrokeJoin = Paint.Join.Round;
                paint.StrokeCap = Paint.Cap.Round;
                paint.AntiAlias = true;
                paint.SetStyle(Paint.Style.Stroke);

                foreach (var path in inkPresenter.GetStrokes())
                {
                    canvas.DrawPath(path.Path, paint);
                }
            }
        }

        // get the image
        return image;
    }

    private async Task<Stream> GetImageStreamInternal(SignatureImageFormat format, System.Drawing.SizeF scale, System.Drawing.RectangleF signatureBounds, System.Drawing.SizeF imageSize, float strokeWidth, NativeColor strokeColor, NativeColor backgroundColor)
    {
        Bitmap.CompressFormat bcf;
        if (format == SignatureImageFormat.Jpeg)
        {
            bcf = Bitmap.CompressFormat.Jpeg;
        }
        else if (format == SignatureImageFormat.Png)
        {
            bcf = Bitmap.CompressFormat.Png;
        }
        else
        {
            return null;
        }

        var image = GetImageInternal(scale, signatureBounds, imageSize, strokeWidth, strokeColor, backgroundColor);
        if (image is not null)
        {
            using (image)
            {
                var stream = new MemoryStream();
                var result = await image.CompressAsync(bcf, 100, stream);

                image.Recycle();

                if (result)
                {
                    stream.Position = 0;
                    return stream;
                }
            }
        }

        return null;
    }

    public override bool OnInterceptTouchEvent(MotionEvent ev)
    {
        // don't accept touch when the view is disabled
        if (!Enabled)
            return true;

        return base.OnInterceptTouchEvent(ev);
    }
}

public partial class SignaturePadCanvasView
{
    public event EventHandler StrokeCompleted;

    public event EventHandler Cleared;

    public bool IsBlank => inkPresenter is null ? true : inkPresenter.GetStrokes().Count == 0;

    public NativePoint[] Points
    {
        get
        {
            if (IsBlank)
            {
                return Array.Empty<NativePoint>();
            }

            // make a deep copy, with { 0, 0 } line starter
            return inkPresenter.GetStrokes()
                .SelectMany(s => new[] { new NativePoint(0, 0) }.Concat(s.GetPoints()))
                .Skip(1) // skip the first empty
                .ToArray();
        }
    }

    public NativePoint[][] Strokes
    {
        get
        {
            if (IsBlank)
            {
                return Array.Empty<NativePoint[]>();
            }

            // make a deep copy
            return inkPresenter.GetStrokes().Select(s => s.GetPoints().ToArray()).ToArray();
        }
    }

    public NativeRect GetSignatureBounds(float padding = 5f)
    {
        if (IsBlank)
        {
            return NativeRect.Empty;
        }

        var size = this.GetSize();
        double xMin = size.Width, xMax = 0, yMin = size.Height, yMax = 0;
        foreach (var point in inkPresenter.GetStrokes().SelectMany(stroke => stroke.GetPoints()))
        {
            xMin = point.X <= 0 ? 0 : Math.Min(xMin, point.X);
            yMin = point.Y <= 0 ? 0 : Math.Min(yMin, point.Y);
            xMax = point.X >= size.Width ? size.Width : Math.Max(xMax, point.X);
            yMax = point.Y >= size.Height ? size.Height : Math.Max(yMax, point.Y);
        }

        var spacing = (StrokeWidth / 2f) + padding;
        xMin = Math.Max(0, xMin - spacing);
        yMin = Math.Max(0, yMin - spacing);
        xMax = Math.Min(size.Width, xMax + spacing);
        yMax = Math.Min(size.Height, yMax + spacing);

        return new NativeRect(
            (float)xMin,
            (float)yMin,
            (float)xMax - (float)xMin,
            (float)yMax - (float)yMin);
    }

    /// <summary>
    /// Create an image of the currently drawn signature.
    /// </summary>
    public NativeImage GetImage(bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImage(new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(1f, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified size.
    /// </summary>
    public NativeImage GetImage(NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImage(new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(size, SizeOrScaleType.Size, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified scale.
    /// </summary>
    public NativeImage GetImage(float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImage(new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(scale, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an image of the currently drawn signature with the specified stroke color.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImage(new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(1f, SizeOrScaleType.Scale, keepAspectRatio),
            StrokeColor = strokeColor,
        });
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified size with the specified stroke color.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImage(new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            DesiredSizeOrScale = new SizeOrScale(size, SizeOrScaleType.Size, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified scale with the specified stroke color.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImage(new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            DesiredSizeOrScale = new SizeOrScale(scale, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an image of the currently drawn signature with the specified stroke and background colors.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, NativeColor fillColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImage(new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(1f, SizeOrScaleType.Scale, keepAspectRatio),
            StrokeColor = strokeColor,
            BackgroundColor = fillColor,
        });
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified size with the specified stroke and background colors.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, NativeColor fillColor, NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImage(new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            BackgroundColor = fillColor,
            DesiredSizeOrScale = new SizeOrScale(size, SizeOrScaleType.Size, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified scale with the specified stroke and background colors.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, NativeColor fillColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImage(new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            BackgroundColor = fillColor,
            DesiredSizeOrScale = new SizeOrScale(scale, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an image of the currently drawn signature using the specified settings.
    /// </summary>
    public NativeImage GetImage(ImageConstructionSettings settings)
    {
        NativeSize scale;
        NativeRect signatureBounds;
        NativeSize imageSize;
        float strokeWidth;
        NativeColor strokeColor;
        NativeColor backgroundColor;

        if (GetImageConstructionArguments(settings, out scale, out signatureBounds, out imageSize, out strokeWidth, out strokeColor, out backgroundColor))
        {
            return GetImageInternal(scale, signatureBounds, imageSize, strokeWidth, strokeColor, backgroundColor);
        }

        return null;
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(1f, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified size.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(size, SizeOrScaleType.Size, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified scale.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(scale, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(1f, SizeOrScaleType.Scale, keepAspectRatio),
            StrokeColor = strokeColor
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified size with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            DesiredSizeOrScale = new SizeOrScale(size, SizeOrScaleType.Size, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified scale with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            DesiredSizeOrScale = new SizeOrScale(scale, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, NativeColor fillColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            DesiredSizeOrScale = new SizeOrScale(1f, SizeOrScaleType.Scale, keepAspectRatio),
            StrokeColor = strokeColor,
            BackgroundColor = fillColor
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified size with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, NativeColor fillColor, NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            BackgroundColor = fillColor,
            DesiredSizeOrScale = new SizeOrScale(size, SizeOrScaleType.Size, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature at the specified scale with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, NativeColor fillColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return GetImageStreamAsync(format, new ImageConstructionSettings
        {
            ShouldCrop = shouldCrop,
            StrokeColor = strokeColor,
            BackgroundColor = fillColor,
            DesiredSizeOrScale = new SizeOrScale(scale, SizeOrScaleType.Scale, keepAspectRatio)
        });
    }

    /// <summary>
    /// Create an encoded image stream of the currently drawn signature using the specified settings.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, ImageConstructionSettings settings)
    {
        NativeSize scale;
        NativeRect signatureBounds;
        NativeSize imageSize;
        float strokeWidth;
        NativeColor strokeColor;
        NativeColor backgroundColor;

        if (GetImageConstructionArguments(settings, out scale, out signatureBounds, out imageSize, out strokeWidth, out strokeColor, out backgroundColor))
        {
            return GetImageStreamInternal(format, scale, signatureBounds, imageSize, strokeWidth, strokeColor, backgroundColor);
        }

        return Task.FromResult<Stream>(null);
    }

    private bool GetImageConstructionArguments(ImageConstructionSettings settings, out NativeSize scale, out NativeRect signatureBounds, out NativeSize imageSize, out float strokeWidth, out NativeColor strokeColor, out NativeColor backgroundColor)
    {
        settings.ApplyDefaults((float)StrokeWidth, StrokeColor);

        if (IsBlank || settings.DesiredSizeOrScale?.IsValid != true)
        {
            scale = default(NativeSize);
            signatureBounds = default(NativeRect);
            imageSize = default(NativeSize);
            strokeWidth = default(float);
            strokeColor = default(NativeColor);
            backgroundColor = default(NativeColor);

            return false;
        }

        var sizeOrScale = settings.DesiredSizeOrScale.Value;
        var viewSize = this.GetSize();

        imageSize = sizeOrScale.GetSize((float)viewSize.Width, (float)viewSize.Height);
        scale = sizeOrScale.GetScale((float)imageSize.Width, (float)imageSize.Height);

        if (settings.ShouldCrop == true)
        {
            signatureBounds = GetSignatureBounds(settings.Padding.Value);

            if (sizeOrScale.Type == SizeOrScaleType.Size)
            {
                // if a specific size was set, scale to that
                var scaleX = imageSize.Width / (float)signatureBounds.Width;
                var scaleY = imageSize.Height / (float)signatureBounds.Height;
                if (sizeOrScale.KeepAspectRatio)
                {
                    scaleX = scaleY = Math.Min((float)scaleX, (float)scaleY);
                }
                scale = new NativeSize((float)scaleX, (float)scaleY);
            }
            else if (sizeOrScale.Type == SizeOrScaleType.Scale)
            {
                imageSize.Width = signatureBounds.Width * scale.Width;
                imageSize.Height = signatureBounds.Height * scale.Height;
            }
        }
        else
        {
            signatureBounds = new NativeRect(0, 0, viewSize.Width, viewSize.Height);
        }

        strokeWidth = settings.StrokeWidth.Value;
        strokeColor = (NativeColor)settings.StrokeColor;
        backgroundColor = (NativeColor)settings.BackgroundColor;

        return true;
    }

    public void LoadStrokes(NativePoint[][] loadedStrokes)
    {
        // clear any existing paths or points.
        Clear();

        // there is nothing
        if (loadedStrokes is null || loadedStrokes.Length == 0)
        {
            return;
        }

        inkPresenter.AddStrokes(loadedStrokes, StrokeColor, (float)StrokeWidth);

        if (!IsBlank)
        {
            OnStrokeCompleted();
        }
    }

    /// <summary>
    /// Allow the user to import an array of points to be used to draw a signature in the view, with new
    /// lines indicated by a { 0, 0 } point in the array.
    /// <param name="loadedPoints"></param>
    public void LoadPoints(NativePoint[] loadedPoints)
    {
        // clear any existing paths or points.
        Clear();

        // there is nothing
        if (loadedPoints is null || loadedPoints.Length == 0)
        {
            return;
        }

        var startIndex = 0;

        var emptyIndex = Array.IndexOf(loadedPoints, new NativePoint(0, 0));
        if (emptyIndex == -1)
        {
            emptyIndex = loadedPoints.Length;
        }

        var strokes = new List<NativePoint[]>();

        do
        {
            // add a stroke to the ink presenter
            var currentStroke = new NativePoint[emptyIndex - startIndex];
            strokes.Add(currentStroke);
            Array.Copy(loadedPoints, startIndex, currentStroke, 0, currentStroke.Length);

            // obtain the indices for the next line to be drawn.
            startIndex = emptyIndex + 1;
            if (startIndex < loadedPoints.Length - 1)
            {
                emptyIndex = Array.IndexOf(loadedPoints, new NativePoint(0, 0), startIndex);
                if (emptyIndex == -1)
                {
                    emptyIndex = loadedPoints.Length;
                }
            }
            else
            {
                emptyIndex = startIndex;
            }
        }
        while (startIndex < emptyIndex);

        inkPresenter.AddStrokes(strokes, StrokeColor, (float)StrokeWidth);

        if (!IsBlank)
        {
            OnStrokeCompleted();
        }
    }

    private void OnCleared()
    {
        Cleared?.Invoke(this, EventArgs.Empty);
    }

    private void OnStrokeCompleted()
    {
        OnStrokeCompleted(this, EventArgs.Empty);
    }

    private void OnStrokeCompleted(object sender, EventArgs e)
    {
        StrokeCompleted?.Invoke(this, e);
    }
}