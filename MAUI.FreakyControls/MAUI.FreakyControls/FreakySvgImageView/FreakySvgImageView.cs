using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Helpers;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using Color = Microsoft.Maui.Graphics.Color;
using SKPaintSurfaceEventArgs = SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs;
using SKSvg = Svg.Skia.SKSvg;

namespace Maui.FreakyControls;

public class FreakySvgImageView : SKCanvasView, IDisposable
{
    private SKImageInfo info;
    private SKSurface surface;
    private SKCanvas canvas;
    private readonly TapGestureRecognizer tapGestureRecognizer;

    private SKSvg Svg { get; }

    private SKCanvas Canvas
    {
        get => canvas;
        set
        {
            canvas = value;
            OnPropertyChanged(nameof(this.SvgMode));
        }
    }

    /// <summary>
    /// Tapped event for our control
    /// </summary>
    public event EventHandler<TappedEventArgs> Tapped;

    #region Constructor, Destructor, Disposal and Assignments

    public FreakySvgImageView()
    {
        tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, new Binding(nameof(this.Command), source: this));
        tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, new Binding(nameof(this.CommandParameter), source: this));
        GestureRecognizers.Add(tapGestureRecognizer);
        tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
        SizeChanged += FreakySvgImageView_SizeChanged;
        this.Svg = new SKSvg();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
        SizeChanged -= FreakySvgImageView_SizeChanged;
        tapGestureRecognizer.Tapped -= TapGestureRecognizer_Tapped;
        GestureRecognizers.Clear();
    }

    ~FreakySvgImageView()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        info = e.Info;
        surface = e.Surface;
        Canvas = surface.Canvas;
        Canvas.Clear();
        if (Svg is null || Svg?.Picture is null)
        {
            return;
        }
        if (ImageColor != Colors.Transparent)
        {
            using var paint = new SKPaint
            {
                ColorFilter = SKColorFilter.CreateBlendMode(ImageColor.ToSKColor(), SKBlendMode.SrcIn),
                Style = SKPaintStyle.StrokeAndFill
            };
            canvas.DrawPicture(Svg.Picture, paint);
            return;
        }
        canvas.DrawPicture(Svg.Picture);
    }

    #endregion Constructor, Destructor, Disposal and Assignments

    public static readonly BindableProperty ImageColorProperty = BindableProperty.Create(
         nameof(ImageColor),
         typeof(Color),
         typeof(FreakySvgImageView),
         Colors.Transparent
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
        default(string)
        );

    public static readonly BindableProperty Base64StringProperty = BindableProperty.Create(
        nameof(Base64String),
        typeof(string),
        typeof(FreakySvgImageView),
        default(string)
        );

    public static readonly BindableProperty UriProperty = BindableProperty.Create(
       nameof(Uri),
       typeof(Uri),
       typeof(FreakySvgImageView),
       default(Uri)
       );

    /// <summary>
    /// of type <see cref="Uri"/>, specifies the Assembly for your Uri
    /// </summary>
    public Uri Uri
    {
        get => (Uri)GetValue(UriProperty);
        set => SetValue(UriProperty, value);
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

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Tapped?.Invoke(this, e);
    }

    private void FreakySvgImageView_SizeChanged(object sender, EventArgs e)
    {
        InvalidateSurface();
    }

    private void SetResourceId()
    {
        try
        {
            Stream svgStream;
            svgStream = SvgAssembly?.GetManifestResourceStream(ResourceId);
            if (svgStream is null)
            {
                Trace.TraceError($"{nameof(FreakySvgImageView)}: Embedded Resource not found for Svg: {ResourceId}");
                return;
            }
            Svg?.Load(svgStream);
            InvalidateSurface();
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

    private void SetBase64String()
    {
        try
        {
            string base64 = Base64String.Substring(Base64String.IndexOf(',') + 1);
            var byteArray = Convert.FromBase64String(base64);
            using var stream = new MemoryStream(byteArray);
            Svg.Load(stream);
            InvalidateSurface();
        }
        catch (Exception ex)
        {
            ex.TraceException();
        }
    }

    private void SetSvgMode()
    {
        Canvas.Translate(info.Width / 2f, info.Height / 2f);
        var bounds = Svg.Picture.CullRect;
        var xRatio = info.Width / bounds.Width;
        var yRatio = info.Height / bounds.Height;
        xRatio *= .95f;
        yRatio *= .95f;
        float ratio;
        switch (SvgMode)
        {
            case Aspect.AspectFill:
                ratio = Math.Max(xRatio, yRatio);
                Canvas.Scale(ratio);
                break;

            case Aspect.Fill:
                Canvas.Scale(xRatio, yRatio);
                break;

            case Aspect.Center:
            case Aspect.AspectFit:
            default:
                ratio = Math.Min(xRatio, yRatio);
                Canvas.Scale(ratio);
                break;
        }
        Canvas.Translate(-bounds.MidX, -bounds.MidY);
    }

    private async Task SetUriAsync()
    {
        try
        {
            var stream = await DownloadHelper.GetStreamAsync(Uri);
            Svg.Load(stream);
            InvalidateSurface();
        }
        catch (Exception ex)
        {
            ex.TraceException();
        }
    }

    protected override async void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(ResourceId) || propertyName == nameof(SvgAssembly))
        {
            if (string.IsNullOrWhiteSpace(ResourceId) || SvgAssembly is null)
                return;
            SetResourceId();
        }
        else if (propertyName == nameof(Base64String))
        {
            if (string.IsNullOrWhiteSpace(Base64String) && Svg is not null)
                return;
            SetBase64String();
        }
        else if (propertyName == nameof(Uri))
        {
            if (Uri == default || Svg is null)
            {
                return;
            }
            await SetUriAsync();
        }
        else if (propertyName == nameof(SvgMode))
        {
            if (Canvas is null || Svg is null || Svg.Picture is null)
                return;
            SetSvgMode();
        }
        else if (propertyName == nameof(ImageColor))
        {
            InvalidateSurface();
        }
    }
}