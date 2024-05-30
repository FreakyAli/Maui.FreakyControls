using Maui.FreakyEffects.TouchTracking;
namespace Maui.FreakyControls;

public partial class FreakyZoomableView : ContentView
{
    private double _currentScale = 1;
    private double _startScale = 1;
    private double _xOffset = 0;
    private double _yOffset = 0;
    private TouchTrackingPoint point = new(0.5f, 0.5f); // default to center
    private bool _secondDoubleTapp = false;
    private double _previousX, _previousY;

    public FreakyZoomableView()
    {
        InitializeComponent();
    }

    private void PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        switch (e.Status)
        {
            case GestureStatus.Started:
                _startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
                break;
            case GestureStatus.Running:
                {
                    _currentScale += (e.Scale - 1) * _startScale;
                    _currentScale = Math.Max(1, _currentScale);

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

                    Content.TranslationX = Math.Min(0, Math.Max(targetX, -Content.Width * (_currentScale - 1)));
                    Content.TranslationY = Math.Min(0, Math.Max(targetY, -Content.Height * (_currentScale - 1)));

                    Content.Scale = _currentScale;
                    break;
                }
            case GestureStatus.Completed:
                _xOffset = Content.TranslationX;
                _yOffset = Content.TranslationY;
                break;
        }
    }

    public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (Content.Scale == 1)
        {
            return;
        }

        switch (e.StatusType)
        {
            case GestureStatus.Running:
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
                break;
            case GestureStatus.Started:
                // Save the current translation when the pan starts
                _previousX = e.TotalX;
                _previousY = e.TotalY;
                _xOffset = Content.TranslationX;
                _yOffset = Content.TranslationY;
                break;
            case GestureStatus.Canceled:
            case GestureStatus.Completed:
                // Update the offsets when the pan ends or is canceled
                _xOffset = Content.TranslationX;
                _yOffset = Content.TranslationY;
                break;
        }
    }

    public async void DoubleTapped(object sender, TappedEventArgs e)
    {
        var multiplicator = Math.Pow(2, 1.0 / 10.0);
        _startScale = Content.Scale;
        Content.AnchorX = 0;
        Content.AnchorY = 0;

        for (var i = 0; i < 10; i++)
        {
            if (!_secondDoubleTapp)
            {
                _currentScale *= multiplicator;
            }
            else
            {
                _currentScale /= multiplicator;
            }

            var renderedX = Content.X + _xOffset;
            var renderedY = Content.Y + _yOffset;

            // Calculate origin based on double tap point
            var originX = (point.X - renderedX) / (Content.Width * _startScale);
            var originY = (point.Y - renderedY) / (Content.Height * _startScale);

            var targetX = _xOffset - (originX * Content.Width) * (_currentScale - _startScale);
            var targetY = _yOffset - (originY * Content.Height) * (_currentScale - _startScale);

            Content.TranslationX = Math.Min(0, Math.Max(targetX, -Content.Width * (_currentScale - 1)));
            Content.TranslationY = Math.Min(0, Math.Max(targetY, -Content.Height * (_currentScale - 1)));

            Content.Scale = _currentScale;
            await Task.Delay(10);
        }

        _secondDoubleTapp = !_secondDoubleTapp;
        _xOffset = Content.TranslationX;
        _yOffset = Content.TranslationY;
    }

    void OnTouch(object sender, TouchActionEventArgs e)
    {
        point = e.Location;
    }
}