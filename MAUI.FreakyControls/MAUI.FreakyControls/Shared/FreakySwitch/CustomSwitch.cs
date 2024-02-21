using Maui.FreakyControls.Extensions;
using System.Windows.Input;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp;

namespace Maui.FreakyControls;

public class CustomSwitch : ContentView, IDisposable
{
    #region Fields
    private readonly SKCanvasView skiaView;
    private readonly TapGestureRecognizer tapped = new();
    #endregion Fields

    #region ctor

    public CustomSwitch()
    {
        skiaView = new SKCanvasView();
        WidthRequest = skiaView.WidthRequest = width;
        HeightRequest = skiaView.HeightRequest = height;
        HorizontalOptions = VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false);
        Content = skiaView;

        skiaView.PaintSurface += Handle_PaintSurface;
        tapped.Tapped += Switch_Tapped;
        GestureRecognizers.Add(tapped);
    }

    private void Switch_Tapped(object sender, EventArgs e)
    {
        if (IsEnabled)
        {
            IsToggled = !IsToggled;
        }
    }

    #endregion ctor

    #region Defaults

    private static readonly float outlineWidth = 6.0f;
    private static readonly double width = 48.0;
    private static readonly double height = 24.0;

    #endregion Defaults


    #region Skia

    private void Handle_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        e?.Surface?.Canvas?.Clear();
        if (IsToggled)
            DrawOnState(e);
        else
            DrawOffState(e);
    }

    private void DrawOnState(SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear();

        var bounds = e.Info.Rect;

        // Draw background
        var backgroundPaint = new SKPaint
        {
            Color = ThumbColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(bounds, bounds.Height / 2, bounds.Height / 2, backgroundPaint);

        // Draw check mark
        var checkPaint = new SKPaint
        {
            Color = OnColor.ToSKColor(),
            IsAntialias = true,
        };

        var checkPath = new SKPath();
        checkPath.MoveTo(bounds.Left + bounds.Width * 0.25f, bounds.MidY);
        checkPath.LineTo(bounds.Left + bounds.Width * 0.45f, bounds.MidY + bounds.Height * 0.2f);
        checkPath.LineTo(bounds.Right - bounds.Width * 0.2f, bounds.Top + bounds.Height * 0.35f);
        canvas.DrawPath(checkPath, checkPaint);
    }

    private void DrawOffState(SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear();

        var bounds = e.Info.Rect;

        // Draw background
        var backgroundPaint = new SKPaint
        {
            Color = OutlineColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(bounds, bounds.Height / 2, bounds.Height / 2, backgroundPaint);
    }


    #endregion Skia

    #region Events

    /// <summary>
    /// Raised when <see cref="CustomSwitch.IsToggled"/> changes.
    /// </summary>
    public event EventHandler<ToggledEventArgs> Toggled;

    #endregion Events

    #region Bindable Properties

    public static readonly BindableProperty OutlineColorProperty =
    BindableProperty.Create(
        nameof(OutlineColor),
        typeof(Color),
        typeof(CustomSwitch),
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

    public static readonly BindableProperty ThumbColorProperty =
    BindableProperty.Create(
        nameof(ThumbColor),
        typeof(Color),
        typeof(CustomSwitch),
        Colors.White);

    /// <summary>
    /// Gets or sets the color of the fill.
    /// </summary>
    /// <value>Color value of the fill.</value>
    public Color ThumbColor
    {
        get { return (Color)GetValue(ThumbColorProperty); }
        set { SetValue(ThumbColorProperty, value); }
    }

    public static readonly BindableProperty OnColorProperty =
    BindableProperty.Create(
        nameof(OnColor),
        typeof(Color),
        typeof(CustomSwitch),
        Colors.Black);

    /// <summary>
    /// Gets or sets the color of the check.
    /// </summary>
    /// <value>Color of the check.</value>
    public Color OnColor
    {
        get { return (Color)GetValue(OnColorProperty); }
        set { SetValue(OnColorProperty, value); }
    }

    public static readonly BindableProperty OutlineWidthProperty =
    BindableProperty.Create(
        nameof(OutlineWidth),
        typeof(float),
        typeof(CustomSwitch),
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

    public static readonly BindableProperty ToggledCommandProperty =
    BindableProperty.Create(
        nameof(ToggledCommand),
        typeof(ICommand),
        typeof(CustomSwitch));

    /// <summary>
    /// Triggered when <see cref="CustomSwitch.IsToggled"/> changes.
    /// </summary>
    public ICommand ToggledCommand
    {
        get { return (ICommand)GetValue(ToggledCommandProperty); }
        set { SetValue(ToggledCommandProperty, value); }
    }

    public static readonly BindableProperty IsToggledProperty =
    BindableProperty.Create(
        nameof(IsToggled),
        typeof(bool),
        typeof(CustomSwitch),
        false,
        BindingMode.TwoWay,
        propertyChanged: IsToggledChanged);

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="CustomSwitch"/> is checked.
    /// </summary>
    /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>
    public bool IsToggled
    {
        get { return (bool)GetValue(IsToggledProperty); }
        set { SetValue(IsToggledProperty, value); }
    }

    private static void IsToggledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CustomSwitch checkbox) return;
        checkbox.Toggled?.Invoke(checkbox, new ToggledEventArgs((bool)newValue));
        checkbox.ToggledCommand?.ExecuteCommandIfAvailable(newValue);
        checkbox.ChangeVisualState();
    }

    protected override void ChangeVisualState()
    {
        base.ChangeVisualState();
        if (IsEnabled && IsToggled)
            VisualStateManager.GoToState(this, Switch.SwitchOnVisualState);
        else if (IsEnabled && !IsToggled)
            VisualStateManager.GoToState(this, Switch.SwitchOffVisualState);
    }

    #endregion Bindable Properties

    #region IDisposable

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~CustomSwitch()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        tapped.Tapped -= Switch_Tapped;
        GestureRecognizers.Clear();
        skiaView.PaintSurface -= Handle_PaintSurface;
    }

    #endregion IDisposable
}