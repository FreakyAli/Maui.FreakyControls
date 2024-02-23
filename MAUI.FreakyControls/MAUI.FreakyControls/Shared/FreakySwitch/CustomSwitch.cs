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

    private static readonly float outlineWidth = 2.0f;
    private static readonly double switchWidth = 54.0d;
    private static readonly double switchHeight = 32.0d;
    private static readonly float thumbRadius = (float)(switchHeight / 2.5);
    private static readonly float thumbWidth = thumbRadius * 2;

    public CustomSwitch()
    {
        skiaView = new SKCanvasView();
        WidthRequest = skiaView.WidthRequest = switchWidth;
        HeightRequest = skiaView.HeightRequest = switchHeight;
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
        canvas.Clear();
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

        // Calculate thumb position with a percentage-based offset from the edge
        var spacingPercentage = 0.05; // 5% spacing
        var spacing = (float)(bounds.Width * spacingPercentage);
        var thumbLeft = bounds.Left + bounds.Width - thumbWidth - spacing - OutlineWidth;
        var thumbRect = SKRect.Create(thumbLeft, bounds.MidY - thumbRadius, thumbWidth, thumbRadius * 2);

        // Draw outline
        var outlineBounds = SKRect.Create(bounds.Left + OutlineWidth / 2, bounds.Top + OutlineWidth / 2, bounds.Width - OutlineWidth, bounds.Height - OutlineWidth);
        var outlinePaint = new SKPaint
        {
            Color = OutlineColor.ToSKColor(),
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = OutlineWidth
        };
        canvas.DrawRoundRect(outlineBounds, bounds.Height / 2, bounds.Height / 2, outlinePaint);

        // Draw the switch thumb
        var thumbPaint = new SKPaint
        {
            Color = ThumbColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(thumbRect, thumbRadius, thumbRadius, thumbPaint);
    }

    private void DrawOffState(SKCanvas canvas, SKRect bounds)
    {
        // Draw background with OffColor
        var backgroundPaint = new SKPaint
        {
            Color = OffColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(bounds, bounds.Height / 2, bounds.Height / 2, backgroundPaint);

        // Calculate thumb position with a percentage-based offset from the edge
        var spacingPercentage = 0.05; // 5% spacing
        var spacing = (float)(bounds.Width * spacingPercentage);
        var thumbLeft = bounds.Left + spacing;
        var thumbRect = SKRect.Create(thumbLeft, bounds.MidY - thumbRadius, thumbWidth, thumbRadius * 2);

        // Draw outline
        var outlineBounds = SKRect.Create(bounds.Left + OutlineWidth / 2, bounds.Top + OutlineWidth / 2, bounds.Width - OutlineWidth, bounds.Height - OutlineWidth);
        var outlinePaint = new SKPaint
        {
            Color = OutlineColor.ToSKColor(),
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = OutlineWidth
        };
        canvas.DrawRoundRect(outlineBounds, bounds.Height / 2, bounds.Height / 2, outlinePaint);

        // Draw the switch thumb
        var thumbPaint = new SKPaint
        {
            Color = ThumbColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(thumbRect, thumbRadius, thumbRadius, thumbPaint);
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

    public Color OffColor
    {
        get => (Color)GetValue(OffColorProperty);
        set => SetValue(OffColorProperty, value);
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
        if (bindable is not CustomSwitch freakySwitch) return;
        freakySwitch.Toggled?.Invoke(freakySwitch, new ToggledEventArgs((bool)newValue));
        freakySwitch.ToggledCommand?.ExecuteCommandIfAvailable(newValue);
        freakySwitch.ChangeVisualState();
        freakySwitch.skiaView.InvalidateSurface();
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
            Colors.LightGreen);

    public static readonly BindableProperty OffColorProperty =
        BindableProperty.Create(
            nameof(OffColor),
            typeof(Color),
            typeof(CustomSwitch),
            Colors.LightGray);

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