using Maui.FreakyControls.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace Maui.FreakyControls;

public partial class FreakySvgImageView : BaseSKCanvas
{
    private DateTime firstTap;
    public const int TAP_TIME_TRESHOLD = 200;

    public event EventHandler Tapped;

    private SKCanvas canvas;
    private SKImageInfo info;
    private SKSurface surface;

    #region bindable properties

    public static readonly BindableProperty ImageColorProperty = BindableProperty.Create(
       nameof(ImageColor),
       typeof(Color),
       typeof(FreakySvgImageView),
       Colors.Transparent,
       propertyChanged: OnColorChangedPropertyChanged
       );

    public static readonly BindableProperty SvgAssemblyProperty = BindableProperty.Create(
       nameof(SvgAssembly),
       typeof(Assembly),
       typeof(FreakySvgImageView),
       default(Assembly)
       );

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(FreakySvgImageView)
        );

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(FreakySvgImageView)
        );

    public static readonly BindableProperty SvgModeProperty = BindableProperty.Create(
       nameof(SvgMode),
       typeof(Aspect),
       typeof(FreakySvgImageView),
       Aspect.AspectFit
       );

    public static readonly BindableProperty ResourceIdProperty = BindableProperty.Create(
        nameof(ResourceId),
        typeof(string),
        typeof(FreakySvgImageView),
        default(string),
        propertyChanged: RedrawCanvas
        );

    public static readonly BindableProperty Base64StringProperty = BindableProperty.Create(
        nameof(Base64String),
        typeof(string),
        typeof(FreakySvgImageView),
        default(string),
        propertyChanged: RedrawCanvas
        );

    private static void OnColorChangedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as FreakySvgImageView;
        view.InvalidateSurface();
    }

    /// <summary>
    /// of type <see cref="Assembly"/>, specifies the Assembly for your ResourceId.
    /// </summary>
    public Assembly SvgAssembly
    {
        get { return (Assembly)GetValue(SvgAssemblyProperty); }
        set { SetValue(SvgAssemblyProperty, value); }
    }

    /// <summary>
    /// of type <see cref="Color"/>, specifies the color you want of your SVG image
    /// </summary>
    public Color ImageColor
    {
        get => (Color)GetValue(ImageColorProperty);
        set => SetValue(ImageColorProperty, value);
    }

    /// <summary>
    /// of type <see cref="string"/>, specifies the source of the image.
    /// </summary>
    public string ResourceId
    {
        get => (string)GetValue(ResourceIdProperty);
        set => SetValue(ResourceIdProperty, value);
    }

    /// <summary>
    /// of type <see cref="string"/>, specifies the Base64 source of the image.
    /// </summary>
    public string Base64String
    {
        get => (string)GetValue(Base64StringProperty);
        set => SetValue(Base64StringProperty, value);
    }

    /// <summary>
    /// of type <see cref="ICommand"/>, defines the command that's executed when the image is tapped.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// of type <see cref="object"/>, is the parameter that's passed to Command.
    /// </summary>
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    /// of type <see cref="Aspect"/>, defines the scaling mode of the image.
    /// </summary>
    public Aspect SvgMode
    {
        get { return (Aspect)GetValue(SvgModeProperty); }
        set { SetValue(SvgModeProperty, value); }
    }

    #endregion bindable properties

    public FreakySvgImageView()
    {
        InitializeComponent();
    }

    private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
    {
        var time = Math.Round((DateTime.Now - firstTap).TotalMilliseconds, MidpointRounding.AwayFromZero);

        //Check to avoid handling multiple tap events at same time using threshold
        if (time > TAP_TIME_TRESHOLD)
        {
            firstTap = DateTime.Now;
            Tapped?.Invoke(this, e);
        }
    }

    protected override void DoPaintSurface(SKPaintSurfaceEventArgs skPaintSurfaceEventArgs)
    {
        try
        {
            info = skPaintSurfaceEventArgs.Info;
            surface = skPaintSurfaceEventArgs.Surface;
            canvas = surface.Canvas;
            canvas.Clear();
            //TODO: Figure out why drawing on coachmark is leading to surface being null
            if (skPaintSurfaceEventArgs.Surface == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(ResourceId) && this.SvgAssembly != null)
            {
                UpdateResourceId();
            }
            else if (!string.IsNullOrWhiteSpace(Base64String))
            {
                UpdateBase64();
            }
            else
            {
                return;
            }
        }
        catch (KeyNotFoundException ex)
        {
            Trace.TraceError("KeyNotFoundException is usually thrown because one or more elements in your SVG file do not have the offset property set to a value i.e. not in the correct format");
            ex.TraceException();
        }
        catch (Exception ex)
        {
            ex.TraceException();
        }
    }

    private void SKCanvasView_OnSizeChanged(object sender, EventArgs e)
    {
        InvalidateSurface();
    }

    private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
    {
        FreakySvgImageView svgIcon = bindable as FreakySvgImageView;
        svgIcon?.InvalidateSurface();
    }

    private void UpdateBase64()
    {
        var svg = new SKSvg();
        string base64 = Base64String.Substring(Base64String.IndexOf(',') + 1);
        var byteArray = Convert.FromBase64String(base64);
        using (var stream = new MemoryStream(byteArray))
        {
            svg.Load(stream);
        }

        canvas.Translate(info.Width / 2f, info.Height / 2f);
        var bounds = svg.Picture.CullRect;
        var xRatio = info.Width / bounds.Width;
        var yRatio = info.Height / bounds.Height;
        xRatio *= .95f;
        yRatio *= .95f;
        float ratio;
        switch (SvgMode)
        {
            case Aspect.Center:
            case Aspect.AspectFit:
                ratio = Math.Min(xRatio, yRatio);
                canvas.Scale(ratio);
                break;

            case Aspect.AspectFill:
                ratio = Math.Max(xRatio, yRatio);
                canvas.Scale(ratio);
                break;

            case Aspect.Fill:
                canvas.Scale(xRatio, yRatio);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        canvas.Translate(-bounds.MidX, -bounds.MidY);

        if (ImageColor != Colors.Transparent)
        {
            using (var paint = new SKPaint
            {
                ColorFilter = SKColorFilter.CreateBlendMode(ImageColor.ToSKColor(), SKBlendMode.SrcIn),
                Style = SKPaintStyle.StrokeAndFill
            })
            {
                canvas.DrawPicture(svg.Picture, paint);
                return;
            }
        }

        canvas.DrawPicture(svg.Picture);
    }

    private void UpdateResourceId()
    {
        Stream svgStream;
        var svg = new SKSvg();
        svgStream = this.SvgAssembly.GetManifestResourceStream(ResourceId);
        if (svgStream == null)
        {
            // TODO: write log entry notifying that this Svg does not have a matching EmbeddedResource
            Trace.TraceError($"SKSvgImage: Embedded Resource not found for Svg: {ResourceId}");
            return;
        }
        svg.Load(svgStream);

        canvas.Translate(info.Width / 2f, info.Height / 2f);
        var bounds = svg.Picture.CullRect;
        var xRatio = info.Width / bounds.Width;
        var yRatio = info.Height / bounds.Height;
        xRatio *= .95f;
        yRatio *= .95f;
        float ratio;
        switch (SvgMode)
        {
            case Aspect.Center:
            case Aspect.AspectFit:
                ratio = Math.Min(xRatio, yRatio);
                canvas.Scale(ratio);
                break;

            case Aspect.AspectFill:
                ratio = Math.Max(xRatio, yRatio);
                canvas.Scale(ratio);
                break;

            case Aspect.Fill:
                canvas.Scale(xRatio, yRatio);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        canvas.Translate(-bounds.MidX, -bounds.MidY);

        if (ImageColor != Colors.Transparent)
        {
            using (var paint = new SKPaint
            {
                ColorFilter = SKColorFilter.CreateBlendMode(ImageColor.ToSKColor(), SKBlendMode.SrcIn),
                Style = SKPaintStyle.StrokeAndFill
            })
            {
                canvas.DrawPicture(svg.Picture, paint);
                return;
            }
        }
        canvas.DrawPicture(svg.Picture);
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName != nameof(ResourceId) &&
            propertyName != nameof(SvgMode))
        {
            return;
        }
        InvalidateSurface();
    }
}