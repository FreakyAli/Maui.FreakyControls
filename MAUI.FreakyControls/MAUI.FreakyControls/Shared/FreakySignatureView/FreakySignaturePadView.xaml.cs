using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Maui.FreakyControls.Extensions;

namespace Maui.FreakyControls;

public partial class FreakySignaturePadView : ContentView
{
    private const string defaultCaptionText = "sign above this line";
    private const string defaultClearIcon = "Maui.FreakyControls.clear_icon.svg";

    private static readonly Color signaturePadDarkColor = Colors.Black;
    private static readonly Color signaturePadLightColor = Colors.White;

    public event EventHandler StrokeCompleted;
    public event EventHandler Cleared;

    public FreakySignaturePadView()
    {
        InitializeComponent();
    }

    #region Properties& BindableProperties

    public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(
           nameof(StrokeColor),
           typeof(Color),
           typeof(FreakySignaturePadView),
           ImageConstructionSettings.DefaultStrokeColor);

    public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
            nameof(StrokeWidth),
            typeof(float),
            typeof(FreakySignaturePadView),
            ImageConstructionSettings.DefaultStrokeWidth);

    public static readonly BindableProperty CaptionTextProperty = BindableProperty.Create(
            nameof(CaptionText),
            typeof(string),
            typeof(FreakySignaturePadView),
            defaultCaptionText);

    public static readonly BindableProperty CaptionFontSizeProperty = BindableProperty.Create(
            nameof(CaptionFontSize),
            typeof(double),
            typeof(FreakySignaturePadView));

    public static readonly BindableProperty CaptionTextColorProperty = BindableProperty.Create(
            nameof(CaptionTextColor),
            typeof(Color),
            typeof(FreakySignaturePadView),
            signaturePadDarkColor);

    public static readonly BindableProperty CaptionFontFamilyProperty = BindableProperty.Create(
            nameof(CaptionFontFamily),
            typeof(string),
            typeof(FreakySignaturePadView),
            default(string));

    public static readonly BindableProperty SignatureUnderlineColorProperty = BindableProperty.Create(
            nameof(SignatureUnderlineColor),
            typeof(Color),
            typeof(FreakySignaturePadView),
            signaturePadDarkColor);

    public static readonly BindableProperty SignatureUnderlineWidthProperty = BindableProperty.Create(
            nameof(SignatureUnderlineWidth),
            typeof(double),
            typeof(FreakySignaturePadView),
            1.0);

    public static readonly BindableProperty ClearImageColorProperty = BindableProperty.Create(
            nameof(ClearImageColor),
            typeof(Color),
            typeof(FreakySignaturePadView),
            Colors.Transparent);

    public static readonly BindableProperty ClearImageAssemblyProperty = BindableProperty.Create(
            nameof(ClearImageAssembly),
            typeof(Assembly),
            typeof(FreakySvgImageView),
            typeof(FreakySignaturePadView).Assembly);

    public static readonly BindableProperty ClearResourceIdProperty = BindableProperty.Create(
            nameof(ClearResourceId),
            typeof(string),
            typeof(FreakySvgImageView),
            defaultClearIcon);

    public static readonly BindableProperty ClearImageBase64Property = BindableProperty.Create(
            nameof(ClearImageBase64),
            typeof(string),
            typeof(FreakySvgImageView),
            default(string));

    public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create(
            nameof(BackgroundImage),
            typeof(ImageSource),
            typeof(FreakySignaturePadView),
            default(ImageSource));

    public static readonly BindableProperty BackgroundImageAspectProperty = BindableProperty.Create(
            nameof(BackgroundImageAspect),
            typeof(Aspect),
            typeof(FreakySignaturePadView),
            Aspect.AspectFit);

    public static readonly BindableProperty BackgroundImageOpacityProperty = BindableProperty.Create(
            nameof(BackgroundImageOpacity),
            typeof(double),
            typeof(FreakySignaturePadView),
            1.0);

    public static readonly BindableProperty ClearedCommandProperty = BindableProperty.Create(
            nameof(ClearedCommand),
            typeof(ICommand),
            typeof(FreakySignaturePadView),
            default(ICommand));

    public static readonly BindableProperty StrokeCompletedCommandProperty = BindableProperty.Create(
            nameof(StrokeCompletedCommand),
            typeof(ICommand),
            typeof(FreakySignaturePadView),
            default(ICommand));

    internal static readonly BindablePropertyKey IsBlankPropertyKey = BindableProperty.CreateReadOnly(
            nameof(IsBlank),
            typeof(bool),
            typeof(FreakySignaturePadView),
            true);

    public static readonly BindableProperty IsBlankProperty = IsBlankPropertyKey.BindableProperty;

    public bool IsBlank => (bool)GetValue(IsBlankProperty);

    /// <summary>
    /// Gets or sets the color of the signature strokes.
    /// </summary>
    public Color StrokeColor
    {
        get => (Color)GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of the signature strokes.
    /// </summary>
    public float StrokeWidth
    {
        get => (float)GetValue(StrokeWidthProperty);
        set => SetValue(StrokeWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the text for the caption displayed under the signature Underline.
    /// </summary>
    public string CaptionText
    {
        get => (string)GetValue(CaptionTextProperty);
        set => SetValue(CaptionTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the font family for the caption text.
    /// </summary>
    public string CaptionFontFamily
    {
        get => (string)GetValue(CaptionFontFamilyProperty);
        set => SetValue(CaptionFontFamilyProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size of the caption.
    /// </summary>
    [TypeConverter(typeof(FontSizeConverter))]
    public double CaptionFontSize
    {
        get => (double)GetValue(CaptionFontSizeProperty);
        set => SetValue(CaptionFontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the caption text.
    /// </summary>
    public Color CaptionTextColor
    {
        get => (Color)GetValue(CaptionTextColorProperty);
        set => SetValue(CaptionTextColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the signature line.
    /// </summary>
    public Color SignatureUnderlineColor
    {
        get => (Color)GetValue(SignatureUnderlineColorProperty);
        set => SetValue(SignatureUnderlineColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of the signature line.
    /// </summary>
    public double SignatureUnderlineWidth
    {
        get => (double)GetValue(SignatureUnderlineWidthProperty);
        set => SetValue(SignatureUnderlineWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the image that clears the pad when clicked.
    /// </summary>
    public Color ClearImageColor
    {
        get => (Color)GetValue(ClearImageColorProperty);
        set => SetValue(ClearImageColorProperty, value);
    }

    /// <summary>
    /// of type <see cref="Assembly"/>, specifies the Assembly for your ResourceId.
    /// </summary>
    public Assembly ClearImageAssembly
    {
        get { return (Assembly)GetValue(ClearImageAssemblyProperty); }
        set { SetValue(ClearImageAssemblyProperty, value); }
    }

    /// <summary>
    /// of type <see cref="string"/>, specifies the source of the image.
    /// A default clear button exists if you keep this field empty
    /// </summary>
    public string ClearResourceId
    {
        get => (string)GetValue(ClearResourceIdProperty);
        set => SetValue(ClearResourceIdProperty, value);
    }

    /// <summary>
    /// of type <see cref="string"/>, specifies the Base64 source of the image.
    /// A default clear button exists if you keep this field empty
    /// </summary>
    public string ClearImageBase64
    {
        get => (string)GetValue(ClearImageBase64Property);
        set => SetValue(ClearImageBase64Property, value);
    }

    /// <summary>
    ///  Gets or sets the background image for your signature pad.
    /// </summary>
    public ImageSource BackgroundImage
    {
        get => (ImageSource)GetValue(BackgroundImageProperty);
        set => SetValue(BackgroundImageProperty, value);
    }

    /// <summary>
    ///  Gets or sets the aspect for the background image.
    /// </summary>
    public Aspect BackgroundImageAspect
    {
        get => (Aspect)GetValue(BackgroundImageAspectProperty);
        set => SetValue(BackgroundImageAspectProperty, value);
    }

    /// <summary>
    ///  Gets or sets the transparency of the watermark.
    /// </summary>
    public double BackgroundImageOpacity
    {
        get => (double)GetValue(BackgroundImageOpacityProperty);
        set => SetValue(BackgroundImageOpacityProperty, value);
    }

    /// <summary>
    /// Gets or sets the command to be fired on clear button click 
    /// </summary>
    public ICommand ClearedCommand
    {
        get => (ICommand)GetValue(ClearedCommandProperty);
        set => SetValue(ClearedCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command to be fired on stroke completed
    /// </summary>
    public ICommand StrokeCompletedCommand
    {
        get => (ICommand)GetValue(StrokeCompletedCommandProperty);
        set => SetValue(StrokeCompletedCommandProperty, value);
    }
    #endregion

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == IsEnabledProperty.PropertyName)
        {
            SignaturePadCanvas.IsEnabled = IsEnabled;
            ClearLabel.IsEnabled = IsEnabled;
        }
    }

    public IEnumerable<IEnumerable<Point>> Strokes
    {
        get => SignaturePadCanvas.Strokes;
        set => SignaturePadCanvas.Strokes = value;
    }

    public IEnumerable<Point> Points
    {
        get => SignaturePadCanvas.Points;
        set => SignaturePadCanvas.Points = value;
    }

    public void Clear()
    {
        SignaturePadCanvas.Clear();
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified size.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Size size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, size, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified scale.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, scale, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified size with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, Size size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, size, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified scale with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, scale, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, Color fillColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, fillColor, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified size with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, Color fillColor, Size size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, fillColor, size, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified scale with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, Color strokeColor, Color fillColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, fillColor, scale, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature using the specified settings.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, ImageConstructionSettings settings)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, settings);
    }

    private void OnClearTapped(object sender, EventArgs e)
    {
        Clear();
    }

    private void OnSignatureCleared(object sender, EventArgs e)
    {
        UpdateBindableProperties();
        Cleared?.Invoke(this, EventArgs.Empty);
        ClearedCommand.ExecuteCommandIfAvailable();
    }

    private void OnSignatureStrokeCompleted(object sender, EventArgs e)
    {
        UpdateBindableProperties();
        StrokeCompleted?.Invoke(this, EventArgs.Empty);
        StrokeCompletedCommand.ExecuteCommandIfAvailable();
    }

    private void UpdateBindableProperties()
    {
        SetValue(IsBlankPropertyKey, SignaturePadCanvas.IsBlank);
    }
}