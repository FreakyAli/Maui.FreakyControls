using SkiaSharp.Views.Maui.Controls;
using System.Windows.Input;

namespace Maui.FreakyControls;

public class FreakyScratchView : ContentView
{
    private Grid _mainLayout;
    private SKCanvasView _skiaCanvas;
    private FreakyScratchViewDrawable _drawable;

    public static readonly BindableProperty FrontContentProperty =
        BindableProperty.Create(nameof(FrontContent), typeof(View), typeof(FreakyScratchView));

    public static readonly BindableProperty BackContentProperty =
        BindableProperty.Create(nameof(BackContent), typeof(View), typeof(FreakyScratchView));

    public static readonly BindableProperty BrushSizeProperty =
        BindableProperty.Create(nameof(BrushSize), typeof(float), typeof(FreakyScratchView), 40f);

    public static readonly BindableProperty RevealThresholdProperty =
        BindableProperty.Create(nameof(RevealThreshold), typeof(float), typeof(FreakyScratchView), 0.7f);

    public static readonly BindableProperty AutoRevealEnabledProperty =
        BindableProperty.Create(nameof(AutoRevealEnabled), typeof(bool), typeof(FreakyScratchView), true);

    public static readonly BindableProperty IsTapToRevealEnabledProperty =
        BindableProperty.Create(nameof(IsTapToRevealEnabled), typeof(bool), typeof(FreakyScratchView), false);

    public static readonly BindableProperty IsDebugModeEnabledProperty =
        BindableProperty.Create(nameof(IsDebugModeEnabled), typeof(bool), typeof(FreakyScratchView), false);

    public static readonly BindableProperty ScratchCompletedCommandProperty =
        BindableProperty.Create(nameof(ScratchCompletedCommand), typeof(ICommand), typeof(FreakyScratchView));

    public static readonly BindableProperty RevealAnimationTypeProperty =
    BindableProperty.Create(nameof(RevealAnimationType), typeof(ScratchRevealAnimationType), typeof(FreakyScratchView), ScratchRevealAnimationType.FadeOut);

    public ScratchRevealAnimationType RevealAnimationType
    {
        get => (ScratchRevealAnimationType)GetValue(RevealAnimationTypeProperty);
        set => SetValue(RevealAnimationTypeProperty, value);
    }

    public View FrontContent
    {
        get => (View)GetValue(FrontContentProperty);
        set => SetValue(FrontContentProperty, value);
    }

    public View BackContent
    {
        get => (View)GetValue(BackContentProperty);
        set => SetValue(BackContentProperty, value);
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

    public bool IsDebugModeEnabled
    {
        get => (bool)GetValue(IsDebugModeEnabledProperty);
        set => SetValue(IsDebugModeEnabledProperty, value);
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

        if (BackContent != null)
            _mainLayout.Children.Add(BackContent);

        _skiaCanvas = new SKCanvasView();
        _drawable = new FreakyScratchViewDrawable(this);
        _skiaCanvas.PaintSurface += _drawable.OnPaintSurface;
        _skiaCanvas.EnableTouchEvents = true;
        _skiaCanvas.Touch += _drawable.OnTouch;

        _mainLayout.Children.Add(_skiaCanvas);

        if (FrontContent != null)
            _mainLayout.Children.Add(FrontContent);

        Content = _mainLayout;
    }

    public void OnScratchCompleted()
    {
        ScratchCompleted?.Invoke(this, EventArgs.Empty);
        ScratchCompletedCommand?.Execute(null);
    }

    public void Reset()
    {
        _drawable?.Reset();
        _skiaCanvas?.InvalidateSurface();
    }
}
