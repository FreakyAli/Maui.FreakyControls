using Maui.FreakyControls.Extensions;
using System.Windows.Input;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp;
using static Maui.FreakyControls.Extensions.Extensions;
namespace Maui.FreakyControls;

public class CustomSwitch : ContentView, IDisposable
{
    private readonly SKCanvasView skiaView;
    private readonly TapGestureRecognizer tapped = new();

    private static readonly float outlineWidth = IsAndroid ? 6.0f : 3.0f;
    private static readonly double width = 54.0d;
    private static readonly double height = 32.0d;

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
        var thumbWidth = bounds.Height * 0.8f; // Adjust thumb width
        var spacingPercentage = 0.05; // 5% spacing
        var spacing = (float)(bounds.Width * spacingPercentage);
        var thumbLeft = bounds.Left + bounds.Width - thumbWidth - spacing;
        var thumbTop = bounds.Top + (bounds.Height - thumbWidth) / 2; // Center the thumb vertically
        var thumbRect = SKRect.Create(thumbLeft, thumbTop, thumbWidth, thumbWidth); // Make the thumb circular

        // Draw the switch thumb
        var thumbPaint = new SKPaint
        {
            Color = ThumbColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(thumbRect, thumbWidth / 2, thumbWidth / 2, thumbPaint); // Maintain circular shape

        // Draw outline
        var outlineBounds = SKRect.Create(bounds.Left + outlineWidth / 2, bounds.Top + outlineWidth / 2, bounds.Width - outlineWidth, bounds.Height - outlineWidth);
        var outlinePaint = new SKPaint
        {
            Color = OutlineColor.ToSKColor(),
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = outlineWidth
        };
        canvas.DrawRoundRect(outlineBounds, outlineBounds.Height / 2, outlineBounds.Height / 2, outlinePaint);
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
        var thumbWidth = bounds.Height * 0.8f; // Adjust thumb width
        var spacingPercentage = 0.05; // 5% spacing
        var spacing = (float)(bounds.Width * spacingPercentage);
        var thumbLeft = bounds.Left + spacing;
        var thumbTop = bounds.Top + (bounds.Height - thumbWidth) / 2; // Center the thumb vertically
        var thumbRect = SKRect.Create(thumbLeft, thumbTop, thumbWidth, thumbWidth); // Make the thumb circular

        // Draw the switch thumb
        var thumbPaint = new SKPaint
        {
            Color = ThumbColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(thumbRect, thumbWidth / 2, thumbWidth / 2, thumbPaint); // Maintain circular shape

        // Draw outline
        var outlineBounds = SKRect.Create(bounds.Left + outlineWidth / 2, bounds.Top + outlineWidth / 2, bounds.Width - outlineWidth, bounds.Height - outlineWidth);
        var outlinePaint = new SKPaint
        {
            Color = OutlineColor.ToSKColor(),
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = outlineWidth
        };
        canvas.DrawRoundRect(outlineBounds, outlineBounds.Height / 2, outlineBounds.Height / 2, outlinePaint);
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