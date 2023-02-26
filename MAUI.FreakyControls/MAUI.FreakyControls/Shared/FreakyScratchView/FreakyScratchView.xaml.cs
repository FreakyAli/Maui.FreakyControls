using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Reflection;
using Maui.FreakyControls.Effects;
using Maui.FreakyControls.Extensions;
using SkiaSharp.Views.Maui.Controls;

namespace Maui.FreakyControls;

public partial class FreakyScratchView : ContentView
{
    public FreakyScratchView()
    {
        InitializeComponent();
        paint.StrokeWidth = StrokeWidth;
    }

    public static BindableProperty FrontImageSourceProperty = BindableProperty.Create(nameof(FrontImageSource), typeof(string), typeof(FreakyScratchView), propertyChanged: OnFrontImageChanged);
    public string FrontImageSource
    {
        get => (string)GetValue(FrontImageSourceProperty);
        set => SetValue(FrontImageSourceProperty, value);
    }

    static void OnFrontImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FreakyScratchView)bindable;
        control.rect = control.GetBitMap(newValue.ToString());
    }

    public static BindableProperty BackImageSourceProperty = BindableProperty.Create(nameof(BackImageSource), typeof(string), typeof(FreakyScratchView), propertyChanged: OnBackImageChanged);
    public string BackImageSource
    {
        get => (string)GetValue(BackImageSourceProperty);
        set => SetValue(BackImageSourceProperty, value);
    }

    static void OnBackImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FreakyScratchView)bindable;

        Assembly assembly = control.GetType().GetTypeInfo().Assembly;
        string resourceId = $"{assembly.GetName().Name}.{newValue}";

        control.BackImage.Source = ImageSource.FromResource(resourceId, assembly);
    }

    public static BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(FreakyScratchView), propertyChanged: OnStrokeWidthChanged, defaultValue: (float)100);
    public float StrokeWidth
    {
        get => (float)GetValue(StrokeWidthProperty);
        set => SetValue(StrokeWidthProperty, value);
    }

    static void OnStrokeWidthChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FreakyScratchView)bindable;
        control.paint.StrokeWidth = (float)newValue;
    }

    SKBitmap GetBitMap(string resourceName)
    {
        SKBitmap resourceBitmap;
        Assembly assembly = GetType().GetTypeInfo().Assembly;
        string resourceId = $"{assembly.GetName().Name}.{resourceName}";

        using (Stream stream = assembly.GetManifestResourceStream(resourceId))
        {
            resourceBitmap = SKBitmap.Decode(stream);
        }

        return resourceBitmap;
    }

    Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
    SKBitmap rect = new SKBitmap();

    SKPaint paint = new SKPaint
    {
        Style = SKPaintStyle.Stroke,
        BlendMode = SKBlendMode.Clear,
        StrokeCap = SKStrokeCap.Round,
        StrokeJoin = SKStrokeJoin.Round
    };

    private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        SKCanvas canvas = e.Surface.Canvas;
        canvas.DrawBitmap(rect, e.Info.Rect);

        foreach (SKPath path in inProgressPaths.Values)
        {
            canvas.DrawPath(path, paint);
        }
    }

    private void TouchEffect_TouchAction(object sender, TouchActionEventArgs args)
    {
        switch (args.Type)
        {
            case TouchActionType.Pressed:
                if (!inProgressPaths.ContainsKey(args.Id))
                {
                    SKPath path = new SKPath();
                    path.MoveTo(ConvertToPixel(args.Location));
                    inProgressPaths.Add(args.Id, path);
                    canvasView.InvalidateSurface();
                }
                break;

            case TouchActionType.Moved:
                if (inProgressPaths.ContainsKey(args.Id))
                {
                    SKPath path = inProgressPaths[args.Id];
                    path.LineTo(ConvertToPixel(args.Location));
                    canvasView.InvalidateSurface();
                }
                break;

            case TouchActionType.Released:
                if (inProgressPaths.ContainsKey(args.Id))
                {
                    canvasView.InvalidateSurface();
                }
                break;

            case TouchActionType.Cancelled:
                if (inProgressPaths.ContainsKey(args.Id))
                {
                    inProgressPaths.Remove(args.Id);
                    canvasView.InvalidateSurface();
                }
                break;
        }
    }

    SKPoint ConvertToPixel(TouchTrackingPoint pt)
    {
        return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                           (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
    }
}
