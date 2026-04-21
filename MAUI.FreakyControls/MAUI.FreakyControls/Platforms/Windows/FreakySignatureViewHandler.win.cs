using Microsoft.Maui.Handlers;
#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
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
            e.Points = _strokes.SelectMany(s => s.Points.Select(p => new Point(p.X, p.Y))).ToList();
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
            e.Strokes = _strokes.Select(s => s.Points.Select(p => new Point(p.X, p.Y)).ToList()).ToList();
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
            int canvasW = Math.Max(1, (int)PlatformView.ActualWidth);
            int canvasH = Math.Max(1, (int)PlatformView.ActualHeight);
            int outW = canvasW, outH = canvasH;

            if (settings.DesiredSizeOrScale is SizeOrScale sos && sos.IsValid)
            {
                var sz = sos.GetSize(canvasW, canvasH);
                outW = Math.Max(1, (int)sz.Width);
                outH = Math.Max(1, (int)sz.Height);
            }

            float scaleX = (float)outW / canvasW;
            float scaleY = (float)outH / canvasH;
            float scaleAvg = (scaleX + scaleY) / 2f;

            var bg = settings.BackgroundColor ?? ImageConstructionSettings.DefaultBackgroundColor;
            byte bgB = (byte)(bg.Blue  * 255);
            byte bgG = (byte)(bg.Green * 255);
            byte bgR = (byte)(bg.Red   * 255);
            byte bgA = (byte)(bg.Alpha * 255);

            int stride = outW * 4;
            var pixels = new byte[outH * stride];

            // Pre-fill with background color.
            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i]     = bgB;
                pixels[i + 1] = bgG;
                pixels[i + 2] = bgR;
                pixels[i + 3] = bgA;
            }

            // Re-draw each captured stroke from its geometry — no PlatformView mutation.
            foreach (var polyline in _strokes)
            {
                var pts = polyline.Points;
                if (pts.Count == 0) continue;

                byte sA, sR, sG, sB;
                if (settings.StrokeColor is Color sc)
                {
                    sA = (byte)(sc.Alpha * 255);
                    sR = (byte)(sc.Red   * 255);
                    sG = (byte)(sc.Green * 255);
                    sB = (byte)(sc.Blue  * 255);
                }
                else if (polyline.Stroke is WinSolidColorBrush brush)
                {
                    sA = brush.Color.A;
                    sR = brush.Color.R;
                    sG = brush.Color.G;
                    sB = brush.Color.B;
                }
                else
                {
                    sA = 255; sR = 0; sG = 0; sB = 0;
                }

                float radius = (settings.StrokeWidth ?? (float)polyline.StrokeThickness) * scaleAvg / 2f;

                float px = (float)(pts[0].X * scaleX);
                float py = (float)(pts[0].Y * scaleY);

                // First point: paint a filled circle (round cap).
                PaintSegment(pixels, outW, outH, stride, px, py, px, py, sB, sG, sR, sA, radius);

                for (int i = 1; i < pts.Count; i++)
                {
                    float cx = (float)(pts[i].X * scaleX);
                    float cy = (float)(pts[i].Y * scaleY);
                    PaintSegment(pixels, outW, outH, stride, px, py, cx, cy, sB, sG, sR, sA, radius);
                    px = cx;
                    py = cy;
                }
            }

            var memStream = new InMemoryRandomAccessStream();
            var encoderId = format == SignatureImageFormat.Jpeg
                ? Windows.Graphics.Imaging.BitmapEncoder.JpegEncoderId
                : Windows.Graphics.Imaging.BitmapEncoder.PngEncoderId;

            var encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateAsync(encoderId, memStream);
            encoder.SetPixelData(
                Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
                Windows.Graphics.Imaging.BitmapAlphaMode.Straight,
                (uint)outW, (uint)outH, 96, 96,
                pixels);

            await encoder.FlushAsync();
            memStream.Seek(0);
            return memStream.AsStream();
        }

        // Rasterizes a thick anti-aliased capsule (widened line segment) into a BGRA8
        // pixel buffer. When x0==x1 and y0==y1 the capsule degenerates to a filled circle.
        private static void PaintSegment(
            byte[] pixels, int w, int h, int stride,
            float x0, float y0, float x1, float y1,
            byte sB, byte sG, byte sR, byte sA, float radius)
        {
            int px0 = Math.Max(0, (int)(MathF.Min(x0, x1) - radius - 1));
            int px1 = Math.Min(w - 1, (int)(MathF.Max(x0, x1) + radius + 1));
            int py0 = Math.Max(0, (int)(MathF.Min(y0, y1) - radius - 1));
            int py1 = Math.Min(h - 1, (int)(MathF.Max(y0, y1) + radius + 1));

            float dx = x1 - x0, dy = y1 - y0;
            float lenSq = dx * dx + dy * dy;
            float strokeAlpha = sA / 255f;

            for (int py = py0; py <= py1; py++)
            {
                for (int px = px0; px <= px1; px++)
                {
                    float ex = px - x0, ey = py - y0;

                    if (lenSq > 0.0001f)
                    {
                        // Project onto segment, clamp to [0,1], find perpendicular offset.
                        float t = Math.Clamp((ex * dx + ey * dy) / lenSq, 0f, 1f);
                        ex -= t * dx;
                        ey -= t * dy;
                    }

                    float dist = MathF.Sqrt(ex * ex + ey * ey);
                    float ink = Math.Clamp(radius - dist + 0.5f, 0f, 1f) * strokeAlpha;
                    if (ink <= 0f) continue;

                    int offset = py * stride + px * 4;
                    float inv = 1f - ink;
                    pixels[offset]     = (byte)(sB * ink + pixels[offset]     * inv);
                    pixels[offset + 1] = (byte)(sG * ink + pixels[offset + 1] * inv);
                    pixels[offset + 2] = (byte)(sR * ink + pixels[offset + 2] * inv);
                    pixels[offset + 3] = (byte)(255  * ink + pixels[offset + 3] * inv);
                }
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
