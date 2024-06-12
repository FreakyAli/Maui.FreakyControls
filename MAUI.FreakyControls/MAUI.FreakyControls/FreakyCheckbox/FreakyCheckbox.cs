using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Enums;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Windows.Input;

namespace Maui.FreakyControls;

public class FreakyCheckbox : ContentView, IDisposable
{
    #region Fields

    private bool isAnimating;
    private readonly SKCanvasView skiaView;
    private readonly TapGestureRecognizer tapped = new();

    #endregion Fields

    #region ctor

    public FreakyCheckbox()
    {
        skiaView = new SKCanvasView();
        WidthRequest = HeightRequest = skiaView.WidthRequest = skiaView.HeightRequest = size;
        HorizontalOptions = VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false);
        Content = skiaView;

        skiaView.PaintSurface += Handle_PaintSurface;
        tapped.Tapped += CheckBox_Tapped;
        GestureRecognizers.Add(tapped);
    }

    private void CheckBox_Tapped(object sender, EventArgs e)
    {
        if (IsEnabled)
        {
            if (isAnimating)
                return;
            IsChecked = !IsChecked;
        }
    }

    #endregion ctor

    #region Defaults

    private static readonly Design design = Enums.Design.Unified;

    private static readonly Shape shape =
        DeviceInfo.Platform == DevicePlatform.iOS ?
        Enums.Shape.Circle :
        Enums.Shape.Sqaure;

    private static readonly float outlineWidth = 6.0f;

    private static readonly double size = 24.0;

    #endregion Defaults

    #region Canvas

    private async Task ToggleAnimationAsync()
    {
        isAnimating = true;
        await PreAnimateAsync();
        skiaView.InvalidateSurface();
        await PostAnimateAsync();
        isAnimating = false;
    }

    private async Task PreAnimateAsync()
    {
        if (HasCheckAnimation)
        {
            if (Design == Design.Native)
            {
                await skiaView.ScaleTo(0.80, 100);
                return;
            }

            switch (AnimationType)
            {
                case AnimationType.Default:
                    await skiaView.ScaleTo(0.80, 100);
                    break;

                case AnimationType.Bounce:
                    if (IsChecked)
                    {
                        // https://github.com/dotnet/maui/issues/11852
                        // To avoid this weird issue on android,
                        // where the defualt 0.5 anchors seem to rotate the whole view
                        // into a circular motion instead of rotating on the provided anchor
                        if (DevicePlatform.Android == DeviceInfo.Platform)
                            skiaView.AnchorY = skiaView.AnchorX = 0.501;
                        await skiaView.ScaleYTo(0.60, 500, Easing.Linear);
                    }
                    break;

                case AnimationType.Flip:
                    await skiaView.RotateYTo(90, 200);
                    break;

                case AnimationType.Rotate:
                    // https://github.com/dotnet/maui/issues/11852
                    // To avoid this weird issue on android,
                    // where the defualt 0.5 anchors seem to rotate the whole view
                    // into a circular motion instead of rotating on the provided anchor
                    if (DevicePlatform.Android == DeviceInfo.Platform)
                        skiaView.AnchorY = skiaView.AnchorX = 0.501;
                    await skiaView.RotateTo(IsChecked ? 90 : -90, 200);
                    break;

                case AnimationType.Slam:
                    // https://github.com/dotnet/maui/issues/11852
                    // To avoid this weird issue on android,
                    // where the defualt 0.5 anchors seem to rotate the whole view
                    // into a circular motion instead of rotating on the provided anchor
                    if (DevicePlatform.Android == DeviceInfo.Platform)
                        skiaView.AnchorY = skiaView.AnchorX = 0.501;
                    if (IsChecked)
                    {
                        skiaView.InvalidateSurface();
                        skiaView.Opacity = 0;
                        await TaskExt.WhenAll(
                            skiaView.ScaleTo(3.5, 100, Easing.Linear),
                            skiaView.FadeTo(0.5, 100, Easing.Linear)
                            );
                        await TaskExt.WhenAll(
                            skiaView.ScaleTo(3, 100, Easing.Linear),
                            skiaView.FadeTo(0.6, 100, Easing.Linear)
                            );
                        await TaskExt.WhenAll(
                            skiaView.ScaleTo(2.5, 100, Easing.Linear),
                            skiaView.FadeTo(0.7, 100, Easing.Linear)
                            );
                        await TaskExt.WhenAll(
                            skiaView.ScaleTo(2, 100, Easing.Linear),
                            skiaView.FadeTo(0.8, 100, Easing.Linear)
                            );
                    }
                    break;
            }
        }
    }

    private async Task PostAnimateAsync()
    {
        if (HasCheckAnimation)
        {
            if (Design == Design.Native)
            {
                await skiaView.ScaleTo(1, 100, Easing.BounceOut);
                return;
            }

            switch (AnimationType)
            {
                case AnimationType.Default:
                    await skiaView.ScaleTo(1, 100, Easing.BounceOut);
                    break;

                case AnimationType.Bounce:
                    if (IsChecked)
                    {
                        await skiaView.ScaleYTo(1, 100, Easing.BounceOut);
                        await skiaView.ScaleTo(1.2, 400, Easing.BounceOut);
                        skiaView.Scale = 1;
                    }
                    break;

                case AnimationType.Flip:
                    skiaView.RotationY = 0;
                    break;

                case AnimationType.Rotate:
                    skiaView.Rotation = 0;
                    break;

                case AnimationType.Slam:
                    skiaView.Scale = 1;
                    await skiaView.ScaleTo(0.8, 200, Easing.Linear);
                    skiaView.Scale = 1;
                    skiaView.Opacity = 1;
                    break;
            }
        }
    }

    #endregion Canvas

    #region Skia

    private void Handle_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        e?.Surface?.Canvas?.Clear();
        if (IsChecked)
            DrawCheckFilled(e);
        else
            DrawOutline(e);
    }

    private void DrawCheckFilled(SKPaintSurfaceEventArgs e)
    {
        var imageInfo = e.Info;
        var canvas = e?.Surface?.Canvas;

        var shape = Design == Design.Unified ? Shape : FreakyCheckbox.shape;
        using var checkfill = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = FillColor.ToSKColor(),
            StrokeJoin = SKStrokeJoin.Round,
            IsAntialias = true
        };

        using var checkBoxStroke = new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            Color = OutlineColor.ToSKColor(),
            StrokeWidth = OutlineWidth,
            StrokeJoin = SKStrokeJoin.Round,
            IsAntialias = true
        };

        if (shape == Enums.Shape.Circle)
        {
            canvas.DrawCircle(
                imageInfo.Width / 2,
                imageInfo.Height / 2,
                (imageInfo.Width / 2) - (OutlineWidth / 2),
                checkfill);

            canvas.DrawCircle(
                imageInfo.Width / 2,
                imageInfo.Height / 2,
                (imageInfo.Width / 2) - (OutlineWidth / 2),
                checkBoxStroke);
        }
        else
        {
            canvas.DrawRoundRect(
                OutlineWidth,
                OutlineWidth,
                imageInfo.Width - (OutlineWidth * 2),
                imageInfo.Height - (OutlineWidth * 2),
                OutlineWidth,
                OutlineWidth,
                checkfill);

            canvas.DrawRoundRect(
                OutlineWidth,
                OutlineWidth,
                imageInfo.Width - (OutlineWidth * 2),
                imageInfo.Height - (OutlineWidth * 2),
                OutlineWidth,
                OutlineWidth,
                checkBoxStroke);
        }

        using var checkPath = new SKPath();
        if (Design == Design.Unified)
        {
            switch (CheckType)
            {
                case CheckType.Check:
                    checkPath.DrawUnifiedCheck(imageInfo);
                    break;

                case CheckType.Line:
                    checkPath.DrawCenteredLine(imageInfo);
                    break;

                case CheckType.Cross:
                    checkPath.DrawCross(imageInfo);
                    break;

                case CheckType.Star:
                    checkPath.DrawStar(imageInfo);
                    break;

                case CheckType.Box:
                    checkPath.DrawSquare(imageInfo);
                    break;

                case CheckType.Fill:
                default:
                    // In case of fill no checkpaths are needed.
                    break;
            }
        }
        else
        {
#if IOS
            checkPath.DrawNativeiOSCheck(imageInfo);
#else
            checkPath.DrawNativeAndroidCheck(imageInfo);
#endif
        }

        using var checkStroke = new SKPaint
        {
            Style = ((Design == Design.Unified) &&
            (CheckType == CheckType.Fill)) ||
            ((CheckType == CheckType.Star) ||
            (CheckType == CheckType.Box)) ?
            SKPaintStyle.Fill : SKPaintStyle.Stroke,
            Color = CheckColor.ToSKColor(),
            StrokeWidth = CheckWidth,
            IsAntialias = true,
            StrokeCap = Design == Design.Unified ? SKStrokeCap.Round : SKStrokeCap.Butt
        };

        canvas.DrawPath(checkPath, checkStroke);
    }

    private void DrawOutline(SKPaintSurfaceEventArgs e)
    {
        var imageInfo = e.Info;
        var canvas = e?.Surface?.Canvas;

        using (var outline = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = OutlineColor.ToSKColor(),
            StrokeWidth = OutlineWidth,
            StrokeJoin = SKStrokeJoin.Round,
            IsAntialias = true
        })
        {
            var shape = Design == Design.Unified ? Shape : FreakyCheckbox.shape;
            if (shape == Enums.Shape.Circle)
            {
                canvas.DrawCircle(
                    imageInfo.Width / 2,
                    imageInfo.Height / 2,
                    (imageInfo.Width / 2) - (OutlineWidth / 2),
                    outline);
            }
            else
            {
                var cornerRadius = OutlineWidth;
                canvas.DrawRoundRect(
                    OutlineWidth,
                    OutlineWidth,
                    imageInfo.Width - (OutlineWidth * 2),
                    imageInfo.Height - (OutlineWidth * 2),
                    cornerRadius,
                    cornerRadius,
                    outline);
            }
        }
    }

    #endregion Skia

    #region Events

    /// <summary>
    /// Raised when <see cref="FreakyCheckbox.IsChecked"/> changes.
    /// </summary>
    public event EventHandler<CheckedChangedEventArgs> CheckedChanged;

    #endregion Events

    #region Bindable Properties

    public static readonly BindableProperty HasCheckAnimationProperty =
    BindableProperty.Create(
        nameof(HasCheckAnimation),
        typeof(bool),
        typeof(FreakyCheckbox),
        true);

    /// <summary>
    /// Gets or sets the color of the outline.
    /// </summary>
    /// <value>Color value of the outline</value>
    public bool HasCheckAnimation
    {
        get => (bool)GetValue(HasCheckAnimationProperty);
        set => SetValue(HasCheckAnimationProperty, value);
    }

    public static readonly BindableProperty OutlineColorProperty =
    BindableProperty.Create(
        nameof(OutlineColor),
        typeof(Color),
        typeof(FreakyCheckbox),
        Colors.Black);

    /// <summary>
    /// Gets or sets the color of the outline.
    /// </summary>
    /// <value>Color value of the outline</value>
    public Color OutlineColor
    {
        get { return (Color)GetValue(OutlineColorProperty); }
        set { SetValue(OutlineColorProperty, value); }
    }

    public static readonly BindableProperty FillColorProperty =
    BindableProperty.Create(
        nameof(FillColor),
        typeof(Color),
        typeof(FreakyCheckbox),
        Colors.White);

    /// <summary>
    /// Gets or sets the color of the fill.
    /// </summary>
    /// <value>Color value of the fill.</value>
    public Color FillColor
    {
        get { return (Color)GetValue(FillColorProperty); }
        set { SetValue(FillColorProperty, value); }
    }

    public static readonly BindableProperty CheckColorProperty =
    BindableProperty.Create(
        nameof(CheckColor),
        typeof(Color),
        typeof(FreakyCheckbox),
        Colors.Black);

    /// <summary>
    /// Gets or sets the color of the check.
    /// </summary>
    /// <value>Color of the check.</value>
    public Color CheckColor
    {
        get { return (Color)GetValue(CheckColorProperty); }
        set { SetValue(CheckColorProperty, value); }
    }

    public static readonly BindableProperty OutlineWidthProperty =
    BindableProperty.Create(
        nameof(OutlineWidth),
        typeof(float),
        typeof(FreakyCheckbox),
        outlineWidth);

    /// <summary>
    /// Gets or sets the width of the outline.
    /// </summary>
    /// <value>The width of the outline</value>
    public float OutlineWidth
    {
        get { return (float)GetValue(OutlineWidthProperty); }
        set { SetValue(OutlineWidthProperty, value); }
    }

    public static readonly BindableProperty CheckWidthProperty =
    BindableProperty.Create(
      nameof(CheckWidth),
      typeof(float),
      typeof(FreakyCheckbox),
      outlineWidth);

    /// <summary>
    /// Gets or sets the width of the check.
    /// </summary>
    /// <value>The width of the check.</value>
    public float CheckWidth
    {
        get { return (float)GetValue(CheckWidthProperty); }
        set { SetValue(CheckWidthProperty, value); }
    }

    public static readonly BindableProperty ShapeProperty =
    BindableProperty.Create(
        nameof(Shape),
        typeof(Shape),
        typeof(FreakyCheckbox),
        shape);

    /// <summary>
    /// Gets or sets the shape of the <see cref="FreakyCheckbox"/>.
    /// </summary>
    public Shape Shape
    {
        get { return (Shape)GetValue(ShapeProperty); }
        set { SetValue(ShapeProperty, value); }
    }

    public static readonly BindableProperty CheckTypeProperty =
    BindableProperty.Create(
       nameof(CheckType),
       typeof(CheckType),
       typeof(FreakyCheckbox),
       CheckType.Check);

    /// <summary>
    /// Gets or sets the type of the check on <see cref="FreakyCheckbox"/>.
    /// </summary>
    public CheckType CheckType
    {
        get { return (CheckType)GetValue(CheckTypeProperty); }
        set { SetValue(CheckTypeProperty, value); }
    }

    public static readonly BindableProperty DesignProperty =
    BindableProperty.Create(
        nameof(Design),
        typeof(Design),
        typeof(FreakyCheckbox),
        design);

    /// <summary>
    /// Gets or sets the design of the <see cref="FreakyCheckbox"/>.
    /// </summary>
    public Design Design
    {
        get { return (Design)GetValue(DesignProperty); }
        set { SetValue(DesignProperty, value); }
    }

    public static readonly BindableProperty AnimationTypeProperty =
    BindableProperty.Create(
        nameof(AnimationType),
        typeof(AnimationType),
        typeof(FreakyCheckbox),
        AnimationType.Default);

    /// <summary>
    /// Gets or sets the design of the <see cref="FreakyCheckbox"/>.
    /// </summary>
    public AnimationType AnimationType
    {
        get { return (AnimationType)GetValue(AnimationTypeProperty); }
        set { SetValue(AnimationTypeProperty, value); }
    }

    public static readonly BindableProperty CheckedChangedCommandProperty =
    BindableProperty.Create(
        nameof(CheckedChangedCommand),
        typeof(ICommand),
        typeof(FreakyCheckbox));

    /// <summary>
    /// Triggered when <see cref="FreakyCheckbox.IsChecked"/> changes.
    /// </summary>
    public ICommand CheckedChangedCommand
    {
        get { return (ICommand)GetValue(CheckedChangedCommandProperty); }
        set { SetValue(CheckedChangedCommandProperty, value); }
    }

    public static readonly BindableProperty IsCheckedProperty =
    BindableProperty.Create(
        nameof(IsChecked),
        typeof(bool),
        typeof(FreakyCheckbox),
        false,
        BindingMode.TwoWay,
        propertyChanged: OnCheckedChanged);

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="FreakyCheckbox"/> is checked.
    /// </summary>
    /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>
    public bool IsChecked
    {
        get { return (bool)GetValue(IsCheckedProperty); }
        set { SetValue(IsCheckedProperty, value); }
    }

    private static async void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is FreakyCheckbox checkbox)) return;
        checkbox.CheckedChanged?.Invoke(checkbox, new CheckedChangedEventArgs((bool)newValue));
        checkbox.CheckedChangedCommand?.ExecuteCommandIfAvailable(newValue);
        checkbox.ChangeVisualState();
        await checkbox.ToggleAnimationAsync();
    }

    protected override void ChangeVisualState()
    {
        if (IsEnabled && IsChecked)
            VisualStateManager.GoToState(this, CheckBox.IsCheckedVisualState);
        else
            base.ChangeVisualState();
    }

    public static readonly BindableProperty SizeRequestProperty =
    BindableProperty.Create(
       nameof(SizeRequest),
       typeof(double),
       typeof(FreakyCheckbox),
       size,
       propertyChanged: SizeRequestChanged);

    /// <summary>
    /// Gets or sets a value indicating the size of this <see cref="FreakyCheckbox"/>
    /// </summary>
    /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>
    public double SizeRequest
    {
        get { return (double)GetValue(SizeRequestProperty); }
        set { SetValue(SizeRequestProperty, value); }
    }

    private static void SizeRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is FreakyCheckbox checkbox)) return;
        checkbox.WidthRequest = checkbox.HeightRequest = (double)(newValue);
        checkbox.skiaView.WidthRequest = checkbox.skiaView.HeightRequest = (double)(newValue);
    }

    #endregion Bindable Properties

    #region IDisposable

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~FreakyCheckbox()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        tapped.Tapped -= CheckBox_Tapped;
        GestureRecognizers.Clear();
        skiaView.PaintSurface -= Handle_PaintSurface;
    }

    #endregion IDisposable
}