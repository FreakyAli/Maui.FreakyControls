using Maui.FreakyControls.Extensions;
using System.Windows.Input;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp;

namespace Maui.FreakyControls;

public class CustomSwitch : ContentView, IDisposable
{
    private readonly SKCanvasView skiaView;
    private readonly TapGestureRecognizer tapped = new();

    private static readonly float outlineWidth = 6.0f;
    private static readonly double width = 50.0;
    private static readonly double height = 30.0;

    public CustomSwitch()
    {
        skiaView = new SKCanvasView();
        WidthRequest = skiaView.WidthRequest = width;
        HeightRequest = skiaView.HeightRequest = height;
        HorizontalOptions = VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false);
        Content = skiaView;

        skiaView.PaintSurface += HandlePaintSurface;
        tapped.Tapped += SwitchTapped;
        GestureRecognizers.Add(tapped);
    }

    private void SwitchTapped(object sender, EventArgs e)
    {
        if (IsEnabled)
        {
            IsToggled = !IsToggled;
        }
    }

    private void HandlePaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        if (IsToggled)
            DrawOnState(canvas, e.Info.Rect);
        else
            DrawOffState(canvas, e.Info.Rect);
    }

    private void DrawOnState(SKCanvas canvas, SKRect bounds)
    {
        // Draw background with OnColor
        var backgroundPaint = new SKPaint
        {
            Color = OnColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(bounds, bounds.Height / 2, bounds.Height / 2, backgroundPaint);

        // Draw check mark
        var checkPaint = new SKPaint
        {
            Color = ThumbColor.ToSKColor(),
            IsAntialias = true,
        };

        var checkPath = new SKPath();
        checkPath.MoveTo(bounds.Left + bounds.Width * 0.25f, bounds.MidY);
        checkPath.LineTo(bounds.Left + bounds.Width * 0.45f, bounds.MidY + bounds.Height * 0.2f);
        checkPath.LineTo(bounds.Right - bounds.Width * 0.2f, bounds.Top + bounds.Height * 0.35f);
        canvas.DrawPath(checkPath, checkPaint);
    }

    private void DrawOffState(SKCanvas canvas, SKRect bounds)
    {
        // Draw background
        var backgroundPaint = new SKPaint
        {
            Color = OutlineColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(bounds, bounds.Height / 2, bounds.Height / 2, backgroundPaint);
    }

    public Color OutlineColor
    {
        get => (Color)GetValue(OutlineColorProperty);
        set => SetValue(OutlineColorProperty, value);
    }

    public Color ThumbColor
    {
        get => (Color)GetValue(ThumbColorProperty);
        set => SetValue(ThumbColorProperty, value);
    }

    public Color OnColor
    {
        get => (Color)GetValue(OnColorProperty);
        set => SetValue(OnColorProperty, value);
    }

    public float OutlineWidth
    {
        get => (float)GetValue(OutlineWidthProperty);
        set => SetValue(OutlineWidthProperty, value);
    }

    public ICommand ToggledCommand
    {
        get => (ICommand)GetValue(ToggledCommandProperty);
        set => SetValue(ToggledCommandProperty, value);
    }

    public bool IsToggled
    {
        get => (bool)GetValue(IsToggledProperty);
        set => SetValue(IsToggledProperty, value);
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

    public event EventHandler<ToggledEventArgs> Toggled;

    public static readonly BindableProperty OutlineColorProperty =
        BindableProperty.Create(
            nameof(OutlineColor),
            typeof(Color),
            typeof(CustomSwitch),
            Colors.Black);

    public static readonly BindableProperty ThumbColorProperty =
        BindableProperty.Create(
            nameof(ThumbColor),
            typeof(Color),
            typeof(CustomSwitch),
            Colors.White);

    public static readonly BindableProperty OnColorProperty =
        BindableProperty.Create(
            nameof(OnColor),
            typeof(Color),
            typeof(CustomSwitch),
            Colors.Green);

    public static readonly BindableProperty OutlineWidthProperty =
        BindableProperty.Create(
            nameof(OutlineWidth),
            typeof(float),
            typeof(CustomSwitch),
            outlineWidth);

    public static readonly BindableProperty ToggledCommandProperty =
        BindableProperty.Create(
            nameof(ToggledCommand),
            typeof(ICommand),
            typeof(CustomSwitch));

    public static readonly BindableProperty IsToggledProperty =
        BindableProperty.Create(
            nameof(IsToggled),
            typeof(bool),
            typeof(CustomSwitch),
            false,
            BindingMode.TwoWay,
            propertyChanged: IsToggledChanged);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            tapped.Tapped -= SwitchTapped;
            GestureRecognizers.Clear();
            skiaView.PaintSurface -= HandlePaintSurface;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~CustomSwitch()
    {
        Dispose(false);
    }
}