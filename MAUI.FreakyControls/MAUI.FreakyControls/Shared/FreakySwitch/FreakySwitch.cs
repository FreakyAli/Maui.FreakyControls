using Maui.FreakyControls.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Windows.Input;
using static Maui.FreakyControls.Extensions.Extensions;

namespace Maui.FreakyControls;

public class FreakySwitch : ContentView, IDisposable
{
    private readonly SKCanvasView skiaView;
    private readonly TapGestureRecognizer tapped = new();

    private static readonly Color outlineColor = IsiOS ? Colors.LightGray : Colors.Black;
    private static readonly float outlineWidth = IsAndroid ? 6.0f : 3.0f;
    private static readonly double width = 54.0d;
    private static readonly double height = 32.0d;

    // Animation related fields
    private float animationProgress;
    private bool isAnimating;
    private TaskCompletionSource<bool> animatonTaskCompletionSource;
   // private const int AnimationDuration = 250;
    


    public FreakySwitch()
    {
        skiaView = new SKCanvasView();
        WidthRequest = skiaView.WidthRequest = width;
        HeightRequest = skiaView.HeightRequest = height;
        HorizontalOptions = VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false);
        Content = skiaView;

        skiaView.PaintSurface += HandlePaintSurface;
        tapped.Tapped += SwitchTapped;
        GestureRecognizers.Add(tapped);

        // Initalize animation field
        isAnimating = false;
    }

    private async Task AnimateThumbAsync()
    {
        if (isAnimating)
            return;

        isAnimating = true;
        animationProgress = 0.0f;

        const int frameRate = 60;
        const double totalDuration = 0.25; // 250 ms
        double frameDuration = 1.0 / frameRate;

        while (animationProgress < 1.0f)
        {
            animationProgress += (float)(frameDuration / totalDuration);
            skiaView.InvalidateSurface();
            await Task.Delay(TimeSpan.FromSeconds(frameDuration));
        }

        animationProgress = 1.0f;
        skiaView.InvalidateSurface();
        isAnimating = false;
    }

    private async void SwitchTapped(object sender, EventArgs e)
    {
        if (IsEnabled)
        {
            await AnimateThumbAsync();
            IsToggled = !IsToggled;
        }
    }

    private void DrawAnimatingState(SKCanvas canvas, SKRect bounds)
    {
        // Draw background with OnColor
        var backgroundPaint = new SKPaint
        {
            Color = OnColor.ToSKColor(),
            IsAntialias = true
        };

        canvas.DrawRoundRect(bounds, bounds.Height / 2, bounds.Height / 2, backgroundPaint);

        // Calculate thumb position based on animation progress
        var thumbWidth = bounds.Height * 0.8f;
        var thumbLeftOff = bounds.Left + bounds.Height * 0.1f; // 10% padding
        var thumbLeftOn = bounds.Right - thumbWidth - bounds.Height * 0.1f;
        var thumbLeft = thumbLeftOff + (thumbLeftOn - thumbLeftOff) * animationProgress;
        var thumbTop = bounds.Top + (bounds.Height - thumbWidth) / 2;
        var thumbRect = SKRect.Create(thumbLeft, thumbTop, thumbWidth, thumbWidth);

          // Draw the switch thumb
        var thumbPaint = new SKPaint
        {
            Color = ThumbOnColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(thumbRect, thumbWidth / 2, thumbWidth / 2, thumbPaint); // Maintain circular shape
    }


    private void HandlePaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(); 
        
        if (isAnimating)
        {
            DrawAnimatingState(canvas, e.Info.Rect);
        }
        else{
            
            if (IsToggled)
                DrawOnState(canvas, e.Info.Rect);
            else
                DrawOffState(canvas, e.Info.Rect);
        }
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

        // Draw outline
        var outlineBounds = SKRect.Create(bounds.Left + outlineWidth / 2, bounds.Top + outlineWidth / 2, bounds.Width - outlineWidth, bounds.Height - outlineWidth);
        var outlinePaint = new SKPaint
        {
            Color = OnColor.ToSKColor(), // Use OnColor for outline in On state
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = outlineWidth
        };
        canvas.DrawRoundRect(outlineBounds, outlineBounds.Height / 2, outlineBounds.Height / 2, outlinePaint);

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
            Color = ThumbOnColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(thumbRect, thumbWidth / 2, thumbWidth / 2, thumbPaint); // Maintain circular shape
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

        if (IsAndroid)
        {
            // Reduce thumb size on Android in the off state to 2/3 of its original size
            var reducedThumbWidth = thumbWidth * (2.0f / 3.0f);
            thumbLeft += (thumbWidth - reducedThumbWidth) / 2;
            thumbTop += (thumbWidth - reducedThumbWidth) / 2;
            thumbRect = SKRect.Create(thumbLeft, thumbTop, reducedThumbWidth, reducedThumbWidth);
        }

        // Draw the switch thumb
        var thumbPaint = new SKPaint
        {
            Color = ThumbOffColor.ToSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(thumbRect, thumbRect.Width / 2, thumbRect.Height / 2, thumbPaint); // Maintain circular shape

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

    public Color ThumbOnColor
    {
        get => (Color)GetValue(ThumbOnColorProperty);
        set => SetValue(ThumbOnColorProperty, value);
    }

    public Color ThumbOffColor
    {
        get => (Color)GetValue(ThumbOffColorProperty);
        set => SetValue(ThumbOffColorProperty, value);
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
        if (bindable is not FreakySwitch freakySwitch) return;
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
            typeof(FreakySwitch),
            outlineColor);

    public static readonly BindableProperty ThumbOnColorProperty =
        BindableProperty.Create(
            nameof(ThumbOnColor),
            typeof(Color),
            typeof(FreakySwitch),
            Colors.White);

    public static readonly BindableProperty ThumbOffColorProperty =
        BindableProperty.Create(
            nameof(ThumbOffColor),
            typeof(Color),
            typeof(FreakySwitch),
            Colors.White);

    public static readonly BindableProperty OnColorProperty =
        BindableProperty.Create(
            nameof(OnColor),
            typeof(Color),
            typeof(FreakySwitch),
            Colors.SeaGreen);

    public static readonly BindableProperty OffColorProperty =
        BindableProperty.Create(
            nameof(OffColor),
            typeof(Color),
            typeof(FreakySwitch),
            Colors.LightGray);

    public static readonly BindableProperty ToggledCommandProperty =
        BindableProperty.Create(
            nameof(ToggledCommand),
            typeof(ICommand),
            typeof(FreakySwitch));

    public static readonly BindableProperty IsToggledProperty =
        BindableProperty.Create(
            nameof(IsToggled),
            typeof(bool),
            typeof(FreakySwitch),
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

    ~FreakySwitch()
    {
        Dispose(false);
    }
}