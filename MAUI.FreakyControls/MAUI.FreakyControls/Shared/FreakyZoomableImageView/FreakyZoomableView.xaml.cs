using Maui.FreakyEffects.TouchTracking;
namespace Maui.FreakyControls;

public partial class FreakyZoomableView : ContentView
{
    private double _currentScale = 1;
    private double _startScale = 1;
    private double _xOffset = 0;
    private double _yOffset = 0;
    private TouchTrackingPoint _point = new(0.5f, 0.5f); // default to center
    private double _previousX, _previousY;

    public FreakyZoomableView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty MinScaleProperty =
            BindableProperty.Create(nameof(MinScale), typeof(double), typeof(FreakyZoomableView), 1.0);

    public double MinScale
    {
        get => (double)GetValue(MinScaleProperty);
        set => SetValue(MinScaleProperty, value);
    }

    public static readonly BindableProperty MaxScaleProperty =
        BindableProperty.Create(nameof(MaxScale), typeof(double), typeof(FreakyZoomableView), 4.0);

    public double MaxScale
    {
        get => (double)GetValue(MaxScaleProperty);
        set => SetValue(MaxScaleProperty, value);
    }

    public static readonly BindableProperty DoubleTapScaleFactorProperty =
       BindableProperty.Create(nameof(DoubleTapScaleFactor), typeof(double), typeof(FreakyZoomableView), 4.0);

    public double DoubleTapScaleFactor
    {
        get => (double)GetValue(DoubleTapScaleFactorProperty);
        set => SetValue(DoubleTapScaleFactorProperty, value);
    }

    public static readonly BindableProperty DoubleTapToZoomProperty =
        BindableProperty.Create(nameof(DoubleTapToZoom), typeof(bool), typeof(FreakyZoomableView), true);

    public bool DoubleTapToZoom
    {
        get => (bool)GetValue(DoubleTapToZoomProperty);
        set => SetValue(DoubleTapToZoomProperty, value);
    }

    public static readonly BindableProperty ZoomableProperty =
        BindableProperty.Create(nameof(Zoomable), typeof(bool), typeof(FreakyZoomableView), true);

    public static readonly BindableProperty IsDoubleTapZoomAnimationEnabledProperty =
      BindableProperty.Create(nameof(IsDoubleTapZoomAnimationEnabled), typeof(bool), typeof(FreakyZoomableView), true);

    public bool IsDoubleTapZoomAnimationEnabled
    {
        get => (bool)GetValue(IsDoubleTapZoomAnimationEnabledProperty);
        set => SetValue(IsDoubleTapZoomAnimationEnabledProperty, value);
    }

    public bool Zoomable
    {
        get => (bool)GetValue(ZoomableProperty);
        set => SetValue(ZoomableProperty, value);
    }

    public static readonly BindableProperty TranslateableProperty =
        BindableProperty.Create(nameof(Translateable), typeof(bool), typeof(FreakyZoomableView), true);

    public bool Translateable
    {
        get => (bool)GetValue(TranslateableProperty);
        set => SetValue(TranslateableProperty, value);
    }

    private void PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (!Zoomable) return;

        switch (e.Status)
        {
            case GestureStatus.Started:
                OnPinchStarted();
                break;
            case GestureStatus.Running:
                OnPinchRunning(e);
                break;
            case GestureStatus.Completed:
                OnPinchCompleted();
                break;
        }
    }

    private void OnPinchStarted()
    {
        _startScale = Content.Scale;
        Content.AnchorX = 0;
        Content.AnchorY = 0;
    }

    private void OnPinchRunning(PinchGestureUpdatedEventArgs e)
    {
        _currentScale += (e.Scale - 1) * _startScale;
        _currentScale = Math.Max(MinScale, _currentScale);
        _currentScale = Math.Min(MaxScale, _currentScale);

        var renderedX = Content.X + _xOffset;
        var deltaX = renderedX / Width;
        var deltaWidth = Width / (Content.Width * _startScale);
        var originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

        var renderedY = Content.Y + _yOffset;
        var deltaY = renderedY / Height;
        var deltaHeight = Height / (Content.Height * _startScale);
        var originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

        var targetX = _xOffset - (originX * Content.Width) * (_currentScale - _startScale);
        var targetY = _yOffset - (originY * Content.Height) * (_currentScale - _startScale);

        // Center the content when scale is less than 1
        if (_currentScale < 1)
        {
            targetX = (Width - Content.Width * _currentScale) / 2;
            targetY = (Height - Content.Height * _currentScale) / 2;
        }
        else
        {
            targetX = Math.Min(0, Math.Max(targetX, -Content.Width * (_currentScale - 1)));
            targetY = Math.Min(0, Math.Max(targetY, -Content.Height * (_currentScale - 1)));
        }

        Content.TranslationX = targetX;
        Content.TranslationY = targetY;

        Content.Scale = _currentScale;
    }


    private void OnPinchCompleted()
    {
        _xOffset = Content.TranslationX;
        _yOffset = Content.TranslationY;
    }

    public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (!Translateable) return;

        switch (e.StatusType)
        {
            case GestureStatus.Running:
                OnPanRunning(e);
                break;
            case GestureStatus.Started:
                OnPanStarted(e);
                break;
            case GestureStatus.Canceled:
            case GestureStatus.Completed:
                OnPanCompleted();
                break;
        }
    }

    private void OnPanStarted(PanUpdatedEventArgs e)
    {
        _previousX = e.TotalX;
        _previousY = e.TotalY;
        _xOffset = Content.TranslationX;
        _yOffset = Content.TranslationY;
    }

    private void OnPanRunning(PanUpdatedEventArgs e)
    {
        if (Content.Scale == 1)
            return;

        // Update the translation based on the delta of the pan
        double deltaX = e.TotalX - _previousX;
        double deltaY = e.TotalY - _previousY;

        Content.TranslationX += deltaX;
        Content.TranslationY += deltaY;

        // Calculate the maximum translation values
        double maxX = Math.Max(0, Content.Width * Content.Scale - Width);
        double maxY = Math.Max(0, Content.Height * Content.Scale - Height);

        // Ensure the translation stays within bounds
        Content.TranslationX = Math.Clamp(Content.TranslationX, -maxX, 0);
        Content.TranslationY = Math.Clamp(Content.TranslationY, -maxY, 0);

        _previousX = e.TotalX;
        _previousY = e.TotalY;
    }

    private void OnPanCompleted()
    {
        _xOffset = Content.TranslationX;
        _yOffset = Content.TranslationY;
    }

    public async void DoubleTapped(object sender, TappedEventArgs e)
    {
        if (!Zoomable || !DoubleTapToZoom) return;

        _startScale = Content.Scale;
        _currentScale = _startScale > MinScale ? MinScale : DoubleTapScaleFactor;

        _currentScale = Math.Max(MinScale, Math.Min(_currentScale, MaxScale));

        Content.AnchorX = 0;
        Content.AnchorY = 0;

        var renderedX = Content.X + _xOffset;
        var renderedY = Content.Y + _yOffset;

        // Calculate origin based on double tap point
        var originX = (_point.X - renderedX) / (Content.Width * _startScale);
        var originY = (_point.Y - renderedY) / (Content.Height * _startScale);

        var targetX = _xOffset - (originX * Content.Width) * (_currentScale - _startScale);
        var targetY = _yOffset - (originY * Content.Height) * (_currentScale - _startScale);

        // Clamp the translation values to ensure they are within bounds
        targetX = Math.Min(0, Math.Max(targetX, -Content.Width * (_currentScale - 1)));
        targetY = Math.Min(0, Math.Max(targetY, -Content.Height * (_currentScale - 1)));

        if (IsDoubleTapZoomAnimationEnabled)
        {
            // Animate the scaling and translation
            var animationDuration = 250u; // duration in milliseconds

            await Task.WhenAll(
                Content.ScaleTo(_currentScale, animationDuration, Easing.CubicInOut),
                Content.TranslateTo(targetX, targetY, animationDuration, Easing.CubicInOut)
            );
        }
        else
        {
            // Set scale and translation directly without animation
            Content.Scale = _currentScale;
            Content.TranslationX = targetX;
            Content.TranslationY = targetY;
        }

        _xOffset = Content.TranslationX;
        _yOffset = Content.TranslationY;
    }


    void OnTouch(object sender, TouchActionEventArgs e)
    {
        _point = e.Location;
    }
}