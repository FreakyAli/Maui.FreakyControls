using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Maui.FreakyControls.Extensions;

namespace Maui.FreakyControls;

public partial class SignaturePadView : ContentView
{
    public event EventHandler StrokeCompleted;
    public event EventHandler Cleared;
    private readonly TapGestureRecognizer clearLabelTap;

    private const string DefaultClearLabelText = "clear";
    private const string DefaultCaptionText = "sign above the line";

    private static readonly Color SignaturePadDarkColor = Colors.Black;
    private static readonly Color SignaturePadLightColor = Colors.White;

    public SignaturePadView()
    {
        InitializeComponent();
        // set up events from the controls
        SignaturePadCanvas.StrokeCompleted += delegate
        {
            OnSignatureStrokeCompleted();
        };
        SignaturePadCanvas.Cleared += delegate
        {
            OnSignatureCleared();
        };
        clearLabelTap = new TapGestureRecognizer
        {
            Command = new Command(() => OnClearTapped())
        };

        ClearLabel.GestureRecognizers.Add(clearLabelTap);
        UpdateUi();
    }

    public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(
           nameof(StrokeColor),
           typeof(Color),
           typeof(SignaturePadView),
           ImageConstructionSettings.DefaultStrokeColor);

    public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
            nameof(StrokeWidth),
            typeof(float),
            typeof(SignaturePadView),
            ImageConstructionSettings.DefaultStrokeWidth);

    public static readonly BindableProperty CaptionTextProperty = BindableProperty.Create(
            nameof(CaptionText),
            typeof(string),
            typeof(SignaturePadView),
            DefaultCaptionText);

    public static readonly BindableProperty CaptionFontSizeProperty = BindableProperty.Create(
            nameof(CaptionFontSize),
            typeof(double),
            typeof(SignaturePadView));

    public static readonly BindableProperty CaptionTextColorProperty = BindableProperty.Create(
            nameof(CaptionTextColor),
            typeof(Color),
            typeof(SignaturePadView),
            SignaturePadDarkColor);

    public static readonly BindableProperty SignatureLineColorProperty = BindableProperty.Create(
            nameof(SignatureLineColor),
            typeof(Color),
            typeof(SignaturePadView),
            SignaturePadDarkColor);

    public static readonly BindableProperty SignatureLineWidthProperty = BindableProperty.Create(
            nameof(SignatureLineWidth),
            typeof(double),
            typeof(SignaturePadView),
            1.0);

    public static readonly BindableProperty ClearTextProperty = BindableProperty.Create(
            nameof(ClearText),
            typeof(string),
            typeof(SignaturePadView),
            DefaultClearLabelText);

    public static readonly BindableProperty ClearFontSizeProperty = BindableProperty.Create(
            nameof(ClearFontSize),
            typeof(double),
            typeof(SignaturePadView));

    public static readonly BindableProperty ClearTextColorProperty = BindableProperty.Create(
            nameof(ClearTextColor),
            typeof(Color),
            typeof(SignaturePadView),
            SignaturePadDarkColor);

    public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create(
            nameof(BackgroundImage),
            typeof(ImageSource),
            typeof(SignaturePadView),
            default(ImageSource));

    public static readonly BindableProperty BackgroundImageAspectProperty = BindableProperty.Create(
            nameof(BackgroundImageAspect),
            typeof(Aspect),
            typeof(SignaturePadView),
            Aspect.AspectFit);

    public static readonly BindableProperty BackgroundImageOpacityProperty = BindableProperty.Create(
            nameof(BackgroundImageOpacity),
            typeof(double),
            typeof(SignaturePadView),
            1.0);

    public static readonly BindableProperty ClearedCommandProperty = BindableProperty.Create(
            nameof(ClearedCommand),
            typeof(ICommand),
            typeof(SignaturePadView),
            default(ICommand));

    public static readonly BindableProperty StrokeCompletedCommandProperty = BindableProperty.Create(
            nameof(StrokeCompletedCommand),
            typeof(ICommand),
            typeof(SignaturePadView),
            default(ICommand));

    internal static readonly BindablePropertyKey IsBlankPropertyKey = BindableProperty.CreateReadOnly(
            nameof(IsBlank),
            typeof(bool),
            typeof(SignaturePadView),
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
    /// Gets or sets the text for the caption displayed under the signature line.
    /// </summary>
    public string CaptionText
    {
        get => (string)GetValue(CaptionTextProperty);
        set => SetValue(CaptionTextProperty, value);
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
    public Color SignatureLineColor
    {
        get => (Color)GetValue(SignatureLineColorProperty);
        set => SetValue(SignatureLineColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of the signature line.
    /// </summary>
    public double SignatureLineWidth
    {
        get => (double)GetValue(SignatureLineWidthProperty);
        set => SetValue(SignatureLineWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the text for the label that clears the pad when clicked.
    /// </summary>
    public string ClearText
    {
        get => (string)GetValue(ClearTextProperty);
        set => SetValue(ClearTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size of the label that clears the pad when clicked.
    /// </summary>
    [TypeConverter(typeof(FontSizeConverter))]
    public double ClearFontSize
    {
        get => (double)GetValue(ClearFontSizeProperty);
        set => SetValue(ClearFontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the label that clears the pad when clicked.
    /// </summary>
    public Color ClearTextColor
    {
        get => (Color)GetValue(ClearTextColorProperty);
        set => SetValue(ClearTextColorProperty, value);
    }

    /// <summary>
    ///  Gets or sets the watermark image.
    /// </summary>
    public ImageSource BackgroundImage
    {
        get => (ImageSource)GetValue(BackgroundImageProperty);
        set => SetValue(BackgroundImageProperty, value);
    }

    /// <summary>
    ///  Gets or sets the aspect for the watermark image.
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

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == IsEnabledProperty.PropertyName)
        {
            SignaturePadCanvas.IsEnabled = IsEnabled;
            if (IsEnabled)
                ClearLabel.GestureRecognizers.Add(clearLabelTap);
            else
                ClearLabel.GestureRecognizers.Remove(clearLabelTap);
        }
    }

    public IEnumerable<IEnumerable<Point>> Strokes
    {
        get { return SignaturePadCanvas.Strokes; }
        set
        {
            SignaturePadCanvas.Strokes = value;
            UpdateUi();
        }
    }

    public IEnumerable<Point> Points
    {
        get { return SignaturePadCanvas.Points; }
        set
        {
            SignaturePadCanvas.Points = value;
            UpdateUi();
        }
    }

    public void Clear()
    {
        SignaturePadCanvas.Clear();
        UpdateUi();
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

    private void OnClearTapped()
    {
        Clear();
    }

    private void OnSignatureCleared()
    {
        UpdateBindableProperties();

        UpdateUi();

        Cleared?.Invoke(this, EventArgs.Empty);

        if (ClearedCommand != null && ClearedCommand.CanExecute(null))
        {
            ClearedCommand.Execute(null);
        }
    }

    private void OnSignatureStrokeCompleted()
    {
        UpdateBindableProperties();

        UpdateUi();

        StrokeCompleted?.Invoke(this, EventArgs.Empty);
        StrokeCompletedCommand.ExecuteCommandIfAvailable();
    }

    private void UpdateBindableProperties()
    {
        SetValue(IsBlankPropertyKey, SignaturePadCanvas.IsBlank);
    }

    private void UpdateUi()
    {
        ClearLabel.IsVisible = !IsBlank;
    }
}
