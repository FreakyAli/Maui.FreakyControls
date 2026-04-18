using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;
using System.Windows.Input;

namespace Maui.FreakyControls;

public class FreakyScratchView : ContentView
{
    private Grid _mainLayout;
    private SKCanvasView _skiaCanvas;
    private FreakyScratchViewDrawable _drawable;

    private static readonly BindableProperty FrontContentProperty =
        BindableProperty.Create(nameof(FrontContent), typeof(View), typeof(FreakyScratchView),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var view = (FreakyScratchView)bindable;
                if (oldValue is View oldView)
                    view._mainLayout.Children.Remove(oldView);
                if (newValue is View newView)
                {
                    // Insert below the SKCanvasView so the canvas stays on top and owns all touches
                    var canvasIndex = view._mainLayout.Children.IndexOf(view._skiaCanvas);
                    view._mainLayout.Children.Insert(canvasIndex, newView);
                }
            });

    public static readonly BindableProperty BackContentProperty =
        BindableProperty.Create(nameof(BackContent), typeof(View), typeof(FreakyScratchView),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var view = (FreakyScratchView)bindable;
                if (oldValue is View oldView)
                    view._mainLayout.Children.Remove(oldView);
                if (newValue is View newView)
                    view._mainLayout.Children.Insert(0, newView);
            });

    public static readonly BindableProperty FrontImageSourceProperty =
        BindableProperty.Create(nameof(FrontImageSource), typeof(ImageSource), typeof(FreakyScratchView),
            propertyChanged: (bindable, _, _) =>
            {
                var view = (FreakyScratchView)bindable;
                view._drawable?.ResetFrontBitmap();
                view._skiaCanvas?.InvalidateSurface();
            });

    public static readonly BindableProperty FrontColorProperty =
        BindableProperty.Create(nameof(FrontColor), typeof(Color), typeof(FreakyScratchView), Colors.LightGray,
            propertyChanged: (bindable, _, _) =>
            {
                var view = (FreakyScratchView)bindable;
                view._drawable?.ResetMask();
                view._skiaCanvas?.InvalidateSurface();
            });

    public static readonly BindableProperty BrushSizeProperty =
        BindableProperty.Create(nameof(BrushSize), typeof(float), typeof(FreakyScratchView), 40f);

    public static readonly BindableProperty RevealThresholdProperty =
        BindableProperty.Create(nameof(RevealThreshold), typeof(float), typeof(FreakyScratchView), 0.7f);

    public static readonly BindableProperty AutoRevealEnabledProperty =
        BindableProperty.Create(nameof(AutoRevealEnabled), typeof(bool), typeof(FreakyScratchView), true);

    public static readonly BindableProperty IsTapToRevealEnabledProperty =
        BindableProperty.Create(nameof(IsTapToRevealEnabled), typeof(bool), typeof(FreakyScratchView), false);

    public static readonly BindableProperty ScratchCompletedCommandProperty =
        BindableProperty.Create(nameof(ScratchCompletedCommand), typeof(ICommand), typeof(FreakyScratchView));

    public static readonly BindableProperty RevealAnimationTypeProperty =
        BindableProperty.Create(nameof(RevealAnimationType), typeof(ScratchRevealAnimationType), typeof(FreakyScratchView), ScratchRevealAnimationType.FadeOut);

    public ScratchRevealAnimationType RevealAnimationType
    {
        get => (ScratchRevealAnimationType)GetValue(RevealAnimationTypeProperty);
        set => SetValue(RevealAnimationTypeProperty, value);
    }
    private View FrontContent
    {
        get => (View)GetValue(FrontContentProperty);
        set => SetValue(FrontContentProperty, value);
    }

    public View BackContent
    {
        get => (View)GetValue(BackContentProperty);
        set => SetValue(BackContentProperty, value);
    }

    public ImageSource FrontImageSource
    {
        get => (ImageSource)GetValue(FrontImageSourceProperty);
        set => SetValue(FrontImageSourceProperty, value);
    }

    public Color FrontColor
    {
        get => (Color)GetValue(FrontColorProperty);
        set => SetValue(FrontColorProperty, value);
    }

    public float BrushSize
    {
        get => (float)GetValue(BrushSizeProperty);
        set => SetValue(BrushSizeProperty, value);
    }

    public float RevealThreshold
    {
        get => (float)GetValue(RevealThresholdProperty);
        set => SetValue(RevealThresholdProperty, value);
    }

    public bool AutoRevealEnabled
    {
        get => (bool)GetValue(AutoRevealEnabledProperty);
        set => SetValue(AutoRevealEnabledProperty, value);
    }

    public bool IsTapToRevealEnabled
    {
        get => (bool)GetValue(IsTapToRevealEnabledProperty);
        set => SetValue(IsTapToRevealEnabledProperty, value);
    }
    public ICommand ScratchCompletedCommand
    {
        get => (ICommand)GetValue(ScratchCompletedCommandProperty);
        set => SetValue(ScratchCompletedCommandProperty, value);
    }

    public event EventHandler ScratchCompleted;

    public FreakyScratchView()
    {
        BuildLayout();
    }

    private void BuildLayout()
    {
        _mainLayout = new Grid();

        _skiaCanvas = new SKCanvasView();
        _drawable = new FreakyScratchViewDrawable(this);
        _skiaCanvas.PaintSurface += _drawable.OnPaintSurface;
        _skiaCanvas.EnableTouchEvents = true;
        _skiaCanvas.Touch += _drawable.OnTouch;

        // SKCanvasView is always the topmost child — it owns all touches
        _mainLayout.Children.Add(_skiaCanvas);

        Content = _mainLayout;
    }

    public void OnScratchCompleted()
    {
        ScratchCompleted?.Invoke(this, EventArgs.Empty);
        ScratchCompletedCommand?.Execute(null);
    }

    public void Reset()
    {
        _skiaCanvas.Opacity = 1;
        _skiaCanvas.IsVisible = true;
        _drawable?.Reset();
        _skiaCanvas?.InvalidateSurface();
    }
}
