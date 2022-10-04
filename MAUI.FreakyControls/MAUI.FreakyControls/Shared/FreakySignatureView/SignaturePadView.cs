using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Maui.FreakyControls.Extensions;

namespace Maui.FreakyControls;

public class SignaturePadView : Grid
{
    private const double DefaultWideSpacing = 12.0;
    private const double DefaultNarrowSpacing = 3.0;
    private const double DefaultLineWidth = 1.0;
    private const double DefaultFontSize = 15.0;

    private const string DefaultClearLabelText = "clear";
    private const string DefaultPromptText = "▶";
    private const string DefaultCaptionText = "sign above the line";

    private static readonly Color SignaturePadDarkColor = Colors.Black;
    private static readonly Color SignaturePadLightColor = Colors.White;

    public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(
            nameof(StrokeColor),
            typeof(Color),
            typeof(SignaturePadView),
            ImageConstructionSettings.DefaultStrokeColor,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).SignaturePadCanvas.StrokeColor = (Color)newValue);

    public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
            nameof(StrokeWidth),
            typeof(float),
            typeof(SignaturePadView),
            ImageConstructionSettings.DefaultStrokeWidth,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).SignaturePadCanvas.StrokeWidth = (float)newValue);

    public static readonly BindableProperty SignatureLineColorProperty = BindableProperty.Create(
            nameof(SignatureLineColor),
            typeof(Color),
            typeof(SignaturePadView),
            SignaturePadDarkColor,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).SignatureLine.Color = (Color)newValue);

    public static readonly BindableProperty SignatureLineWidthProperty = BindableProperty.Create(
            nameof(SignatureLineWidth),
            typeof(double),
            typeof(SignaturePadView),
            DefaultLineWidth,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).SignatureLine.HeightRequest = (double)newValue);

    public static readonly BindableProperty SignatureLineSpacingProperty = BindableProperty.Create(
            nameof(SignatureLineSpacing),
            typeof(double),
            typeof(SignaturePadView),
            DefaultNarrowSpacing,
            propertyChanged: OnPaddingChanged);

    public static readonly BindableProperty CaptionTextProperty = BindableProperty.Create(
            nameof(CaptionText),
            typeof(string),
            typeof(SignaturePadView),
            DefaultCaptionText,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).CaptionLabel.Text = (string)newValue);

    public static readonly BindableProperty CaptionFontSizeProperty = BindableProperty.Create(
            nameof(CaptionFontSize),
            typeof(double),
            typeof(SignaturePadView),
            DefaultFontSize,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).CaptionLabel.FontSize = (double)newValue);

    public static readonly BindableProperty CaptionTextColorProperty = BindableProperty.Create(
            nameof(CaptionTextColor),
            typeof(Color),
            typeof(SignaturePadView),
            SignaturePadDarkColor,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).CaptionLabel.TextColor = (Color)newValue);

    public static readonly BindableProperty PromptTextProperty = BindableProperty.Create(
            nameof(PromptText),
            typeof(string),
            typeof(SignaturePadView),
            DefaultPromptText,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).SignaturePrompt.Text = (string)newValue);

    public static readonly BindableProperty PromptFontSizeProperty = BindableProperty.Create(
            nameof(PromptFontSize),
            typeof(double),
            typeof(SignaturePadView),
            DefaultFontSize,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).SignaturePrompt.FontSize = (double)newValue);

    public static readonly BindableProperty PromptTextColorProperty = BindableProperty.Create(
            nameof(PromptTextColor),
            typeof(Color),
            typeof(SignaturePadView),
            SignaturePadDarkColor,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).SignaturePrompt.TextColor = (Color)newValue);

    public static readonly BindableProperty ClearTextProperty = BindableProperty.Create(
            nameof(ClearText),
            typeof(string),
            typeof(SignaturePadView),
            DefaultClearLabelText,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).ClearLabel.Text = (string)newValue);

    public static readonly BindableProperty ClearFontSizeProperty = BindableProperty.Create(
            nameof(ClearFontSize),
            typeof(double),
            typeof(SignaturePadView),
            DefaultFontSize,
            propertyChanged: (bindable, oldValue, newValue) => ((SignaturePadView)bindable).ClearLabel.FontSize = (double)newValue);

    public static readonly BindableProperty ClearTextColorProperty = BindableProperty.Create(
            nameof(ClearTextColor),
            typeof(Color),
            typeof(SignaturePadView),
            SignaturePadDarkColor,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).ClearLabel.TextColor = (Color)newValue);

    public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create(
            nameof(BackgroundImage),
            typeof(ImageSource),
            typeof(SignaturePadView),
            default(ImageSource),
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).BackgroundImageView.Source = (ImageSource)newValue);

    public static readonly BindableProperty BackgroundImageAspectProperty = BindableProperty.Create(
                nameof(BackgroundImageAspect),
                typeof(Aspect),
                typeof(SignaturePadView),
                Aspect.AspectFit,
                propertyChanged: (bindable, oldValue, newValue) =>
                ((SignaturePadView)bindable).BackgroundImageView.Aspect = (Aspect)newValue);

    public static readonly BindableProperty BackgroundImageOpacityProperty = BindableProperty.Create(
            nameof(BackgroundImageOpacity),
            typeof(double),
            typeof(SignaturePadView),
            1.0,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((SignaturePadView)bindable).BackgroundImageView.Opacity = (double)newValue);

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


    private readonly TapGestureRecognizer clearLabelTap;

    public SignaturePadView()
    {
        // add the background view
        BackgroundImageView = new Image
        {
            Source = BackgroundImage,
            Aspect = BackgroundImageAspect,
            Opacity = BackgroundImageOpacity,
            InputTransparent = true,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
        };
        Children.Add(BackgroundImageView);

        // add the main signature view
        SignaturePadCanvas = new SignaturePadCanvasView
        {
            StrokeColor = StrokeColor,
            StrokeWidth = StrokeWidth,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };
        Children.Add(SignaturePadCanvas);

        // add the clear label
        ClearLabel = new Label
        {
            Text = ClearText,
            FontSize = ClearFontSize,
            TextColor = ClearTextColor,
            FontAttributes = FontAttributes.Bold,
            IsVisible = false,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start
        };
        Children.Add(ClearLabel);

        // add the footer bit
        var footer = new StackLayout
        {
            Spacing = 0,
            Padding = 0,
            Margin = 0,
            InputTransparent = true,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End,
        };
        Children.Add(footer);

        // add the prompt
        SignaturePrompt = new Label
        {
            Text = PromptText,
            FontSize = PromptFontSize,
            TextColor = PromptTextColor,
            FontAttributes = FontAttributes.Bold,
            InputTransparent = true,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End
        };
        footer.Children.Add(SignaturePrompt);

        // add the signature line
        SignatureLine = new BoxView
        {
            Color = SignatureLineColor,
            HeightRequest = SignatureLineWidth,
            InputTransparent = true,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End
        };
        footer.Children.Add(SignatureLine);

        // add the caption
        CaptionLabel = new Label
        {
            Text = CaptionText,
            FontSize = CaptionFontSize,
            TextColor = CaptionTextColor,
            HorizontalTextAlignment = TextAlignment.Center,
            InputTransparent = true,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End
        };
        footer.Children.Add(CaptionLabel);

        // set up the main control
        RowSpacing = 0;
        ColumnSpacing = 0;
        Padding = new Thickness(DefaultWideSpacing, DefaultWideSpacing, DefaultWideSpacing, DefaultNarrowSpacing);
        BackgroundColor = SignaturePadLightColor;

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

        OnPaddingChanged();
        UpdateUi();
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
        else if (propertyName == PaddingProperty.PropertyName)
        {
            OnPaddingChanged();
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

    public bool IsBlank => (bool)GetValue(IsBlankProperty);

    /// <summary>
    /// Gets the underlying control that handles the signatures.
    /// </summary>
    public SignaturePadCanvasView SignaturePadCanvas { get; private set; }

    /// <summary>
    /// Gets the horizontal line that goes in the lower part of the pad.
    /// </summary>
    public BoxView SignatureLine { get; private set; }

    /// <summary>
    /// The caption displayed under the signature line.
    /// </summary>
    public Label CaptionLabel { get; private set; }

    /// <summary>
    /// The prompt displayed at the beginning of the signature line.
    /// </summary>
    public Label SignaturePrompt { get; private set; }

    /// <summary>
    /// Gets the label that clears the pad when clicked.
    /// </summary>
    public Label ClearLabel { get; private set; }

    /// <summary>
    /// Gets the image view that handles the background image.
    /// </summary>
    public Image BackgroundImageView { get; private set; }

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
    /// Gets or sets the size of the spacing between the signature line and the caption.
    /// </summary>
    public double SignatureLineSpacing
    {
        get => (double)GetValue(SignatureLineSpacingProperty);
        set => SetValue(SignatureLineSpacingProperty, value);
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
    /// Gets or sets the text for the prompt displayed at the beginning of the signature line.
    /// </summary>
    public string PromptText
    {
        get => (string)GetValue(PromptTextProperty);
        set => SetValue(PromptTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size of the prompt displayed at the beginning of the signature line.
    /// </summary>
    public double PromptFontSize
    {
        get => (double)GetValue(PromptFontSizeProperty);
        set => SetValue(PromptFontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the text color of the prompt displayed at the beginning of the signature line.
    /// </summary>
    public Color PromptTextColor
    {
        get => (Color)GetValue(PromptTextColorProperty);
        set => SetValue(PromptTextColorProperty, value);
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

    public ICommand ClearedCommand
    {
        get => (ICommand)GetValue(ClearedCommandProperty);
        set => SetValue(ClearedCommandProperty, value);
    }

    public ICommand StrokeCompletedCommand
    {
        get => (ICommand)GetValue(StrokeCompletedCommandProperty);
        set => SetValue(StrokeCompletedCommandProperty, value);
    }

    public event EventHandler StrokeCompleted;

    public event EventHandler Cleared;

    public new void Clear()
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

    private static void OnPaddingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((SignaturePadView)bindable).OnPaddingChanged();
    }

    private void OnPaddingChanged()
    {
        var padding = Padding;
        var spacing = SignatureLineSpacing;

        var ignorePadding = new Thickness(-padding.Left, -padding.Top, -padding.Right, -padding.Bottom);

        SignatureLine.Margin = new Thickness(0, spacing, 0, spacing);
        BackgroundImageView.Margin = ignorePadding;
        SignaturePadCanvas.Margin = ignorePadding;
    }
}

