using Microsoft.Maui.Handlers;
#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI; 
#endif

namespace Maui.FreakyControls;

public partial class FreakySignatureCanvasViewHandler : ViewHandler<FreakySignatureCanvasView, Canvas>
{
#if WINDOWS
    protected override Canvas CreatePlatformView()
    {
        var canvas = new Canvas
        {
            Background = new SolidColorBrush(Colors.White)
        };

        canvas.PointerPressed += OnPointerPressed;
        canvas.PointerMoved += OnPointerMoved;
        canvas.PointerReleased += OnPointerReleased;
        return canvas;
    }

    protected override void ConnectHandler(Canvas platformView)
    {
        base.ConnectHandler(platformView);
        // Additional setup if needed
    }

    protected override void DisconnectHandler(Canvas platformView)
    {
        base.DisconnectHandler(platformView);
        platformView.PointerPressed -= OnPointerPressed;
        platformView.PointerMoved -= OnPointerMoved;
        platformView.PointerReleased -= OnPointerReleased;
    }

    private bool _isDrawing;
    private Windows.Foundation.Point _previousPoint;

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is not Canvas canvas)
            return;

        _isDrawing = true;
        _previousPoint = e.GetCurrentPoint(canvas).Position;
    }

    private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDrawing || sender is not Canvas canvas)
            return;

        var currentPoint = e.GetCurrentPoint(canvas).Position;

        var line = new Line
        {
            X1 = _previousPoint.X,
            Y1 = _previousPoint.Y,
            X2 = currentPoint.X,
            Y2 = currentPoint.Y,
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 2
        };

        canvas.Children.Add(line);
        _previousPoint = currentPoint;
    }

    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        _isDrawing = false;
    }
#endif
}
