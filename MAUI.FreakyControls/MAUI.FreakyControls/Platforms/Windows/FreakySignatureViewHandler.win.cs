using Microsoft.Maui.Handlers;
#if WINDOWS
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using Windows.Storage.Streams;
using WinColor = Windows.UI.Color;
using WinPoint = Windows.Foundation.Point;
using WinSolidColorBrush = Microsoft.UI.Xaml.Media.SolidColorBrush;
#endif

namespace Maui.FreakyControls
{
    public partial class FreakySignatureCanvasViewHandler : ViewHandler<FreakySignatureCanvasView, Canvas>
    {
        public static IPropertyMapper<FreakySignatureCanvasView, FreakySignatureCanvasViewHandler> Mapper =
            new PropertyMapper<FreakySignatureCanvasView, FreakySignatureCanvasViewHandler>(ViewHandler.ViewMapper)
            {
#if WINDOWS
                [nameof(FreakySignatureCanvasView.StrokeColor)] = MapStrokeColor,
                [nameof(FreakySignatureCanvasView.StrokeWidth)] = MapStrokeWidth,
#endif
            };

        public FreakySignatureCanvasViewHandler() : base(Mapper)
        {
        }

#if WINDOWS
        private bool _isDrawing;
        private Polyline? _currentStroke;
        private readonly List<Polyline> _strokes = new();
        private WinColor _strokeColor = Microsoft.UI.Colors.Black;
        private double _strokeThickness = 2;

        protected override Canvas CreatePlatformView()
        {
            return new Canvas
            {
                Background = new WinSolidColorBrush(Microsoft.UI.Colors.White)
            };
        }

        protected override void ConnectHandler(Canvas platformView)
        {
            base.ConnectHandler(platformView);

            platformView.PointerPressed += OnPointerPressed;
            platformView.PointerMoved += OnPointerMoved;
            platformView.PointerReleased += OnPointerReleased;
            platformView.PointerCanceled += OnPointerCanceled;
            platformView.PointerCaptureLost += OnPointerCaptureLost;

            VirtualView.ImageStreamRequested += OnImageStreamRequested;
            VirtualView.IsBlankRequested += OnIsBlankRequested;
            VirtualView.PointsRequested += OnPointsRequested;
            VirtualView.PointsSpecified += OnPointsSpecified;
            VirtualView.StrokesRequested += OnStrokesRequested;
            VirtualView.StrokesSpecified += OnStrokesSpecified;
            VirtualView.ClearRequested += OnClearRequested;
        }

        protected override void DisconnectHandler(Canvas platformView)
        {
            platformView.PointerPressed -= OnPointerPressed;
            platformView.PointerMoved -= OnPointerMoved;
            platformView.PointerReleased -= OnPointerReleased;
            platformView.PointerCanceled -= OnPointerCanceled;
            platformView.PointerCaptureLost -= OnPointerCaptureLost;

            VirtualView.ImageStreamRequested -= OnImageStreamRequested;
            VirtualView.IsBlankRequested -= OnIsBlankRequested;
            VirtualView.PointsRequested -= OnPointsRequested;
            VirtualView.PointsSpecified -= OnPointsSpecified;
            VirtualView.StrokesRequested -= OnStrokesRequested;
            VirtualView.StrokesSpecified -= OnStrokesSpecified;
            VirtualView.ClearRequested -= OnClearRequested;

            base.DisconnectHandler(platformView);
        }

        // ── Property mappers ───────────────────────────────────────────────

        private static void MapStrokeColor(FreakySignatureCanvasViewHandler handler, FreakySignatureCanvasView view)
        {
            var c = view.StrokeColor;
            handler._strokeColor = WinColor.FromArgb(
                (byte)(c.Alpha * 255),
                (byte)(c.Red * 255),
                (byte)(c.Green * 255),
                (byte)(c.Blue * 255));
        }

        private static void MapStrokeWidth(FreakySignatureCanvasViewHandler handler, FreakySignatureCanvasView view)
        {
            handler._strokeThickness = view.StrokeWidth;
        }

        // ── Drawing ────────────────────────────────────────────────────────

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is not Canvas canvas) return;

            _isDrawing = true;
            var pt = e.GetCurrentPoint(canvas).Position;

            _currentStroke = new Polyline
            {
                Stroke = new WinSolidColorBrush(_strokeColor),
                StrokeThickness = _strokeThickness,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };
            _currentStroke.Points.Add(pt);
            canvas.Children.Add(_currentStroke);
            _strokes.Add(_currentStroke);

            canvas.CapturePointer(e.Pointer);
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!_isDrawing || _currentStroke is null || sender is not Canvas canvas) return;
            _currentStroke.Points.Add(e.GetCurrentPoint(canvas).Position);
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (_isDrawing && sender is Canvas canvas)
                canvas.ReleasePointerCapture(e.Pointer);
            EndCurrentStroke();
        }

        private void OnPointerCanceled(object sender, PointerRoutedEventArgs e)
            => EndCurrentStroke();

        private void OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
            => EndCurrentStroke();

        private void EndCurrentStroke()
        {
            if (!_isDrawing) return;
            _isDrawing = false;
            _currentStroke = null;
            VirtualView?.OnStrokeCompleted();
        }

        // ── VirtualView events ─────────────────────────────────────────────

        private void OnClearRequested(object? sender, EventArgs e) => ClearCanvas(raiseEvent: true);

        private void ClearCanvas(bool raiseEvent)
        {
            PlatformView.Children.Clear();
            _strokes.Clear();
            _currentStroke = null;
            _isDrawing = false;
            if (raiseEvent)
                VirtualView?.OnCleared();
        }

        private void OnIsBlankRequested(object? sender, IsBlankRequestedEventArgs e)
        {
            e.IsBlank = _strokes.Count == 0 || _strokes.All(s => s.Points.Count == 0);
        }

        private void OnPointsRequested(object? sender, PointsEventArgs e)
        {
            e.Points = _strokes.SelectMany(s => s.Points.Select(p => new Point(p.X, p.Y)));
        }

        private void OnPointsSpecified(object? sender, PointsEventArgs e)
        {
            ClearCanvas(raiseEvent: false);
            if (e.Points?.Any() != true) return;

            var polyline = CreatePolyline();
            foreach (var p in e.Points)
                polyline.Points.Add(new WinPoint(p.X, p.Y));
            PlatformView.Children.Add(polyline);
            _strokes.Add(polyline);
        }

        private void OnStrokesRequested(object? sender, StrokesEventArgs e)
        {
            e.Strokes = _strokes.Select(s => s.Points.Select(p => new Point(p.X, p.Y)));
        }

        private void OnStrokesSpecified(object? sender, StrokesEventArgs e)
        {
            ClearCanvas(raiseEvent: false);
            if (e.Strokes is null) return;

            foreach (var stroke in e.Strokes)
            {
                var polyline = CreatePolyline();
                foreach (var p in stroke)
                    polyline.Points.Add(new WinPoint(p.X, p.Y));
                PlatformView.Children.Add(polyline);
                _strokes.Add(polyline);
            }
        }

        private void OnImageStreamRequested(object? sender, ImageStreamRequestedEventArgs e)
        {
            e.ImageStreamTask = RenderToStreamAsync(e.ImageFormat, e.Settings);
        }

        // ── Image export ───────────────────────────────────────────────────

        private async Task<Stream> RenderToStreamAsync(SignatureImageFormat format, ImageConstructionSettings settings)
        {
            var originalBackground = PlatformView.Background;
            if (settings.BackgroundColor is Color bgColor)
            {
                PlatformView.Background = new WinSolidColorBrush(WinColor.FromArgb(
                    (byte)(bgColor.Alpha * 255),
                    (byte)(bgColor.Red * 255),
                    (byte)(bgColor.Green * 255),
                    (byte)(bgColor.Blue * 255)));
            }

            try
            {
                var rtb = new RenderTargetBitmap();

                if (settings.DesiredSizeOrScale is SizeOrScale sos && sos.IsValid)
                {
                    var sz = sos.GetSize((float)PlatformView.ActualWidth, (float)PlatformView.ActualHeight);
                    await rtb.RenderAsync(PlatformView, (int)sz.Width, (int)sz.Height);
                }
                else
                {
                    await rtb.RenderAsync(PlatformView);
                }

                var pixelBuffer = await rtb.GetPixelsAsync();
                var pixels = pixelBuffer.ToArray();

                var memStream = new InMemoryRandomAccessStream();
                var encoderId = format == SignatureImageFormat.Jpeg
                    ? Windows.Graphics.Imaging.BitmapEncoder.JpegEncoderId
                    : Windows.Graphics.Imaging.BitmapEncoder.PngEncoderId;

                var encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateAsync(encoderId, memStream);
                encoder.SetPixelData(
                    Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
                    Windows.Graphics.Imaging.BitmapAlphaMode.Premultiplied,
                    (uint)rtb.PixelWidth,
                    (uint)rtb.PixelHeight,
                    96, 96,
                    pixels);

                await encoder.FlushAsync();
                memStream.Seek(0);
                return memStream.AsStream();
            }
            finally
            {
                PlatformView.Background = originalBackground;
            }
        }

        // ── Helpers ────────────────────────────────────────────────────────

        private Polyline CreatePolyline() => new()
        {
            Stroke = new WinSolidColorBrush(_strokeColor),
            StrokeThickness = _strokeThickness,
            StrokeLineJoin = PenLineJoin.Round,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round
        };
#endif
    }
}
