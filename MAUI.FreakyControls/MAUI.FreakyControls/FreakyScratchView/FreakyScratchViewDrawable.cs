using SkiaSharp.Views.Maui.Controls;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Maui.FreakyControls;

// public class FreakyScratchViewDrawable
// {
//     private readonly FreakyScratchView _parent;
//     private SKBitmap _maskBitmap;
//     private SKCanvas _maskCanvas;
//     private bool _scratchCompleted;
//     private bool _isAutoRevealed;

//     public FreakyScratchViewDrawable(FreakyScratchView parent)
//     {
//         _parent = parent;
//     }

//     public void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
//     {
//         var canvas = e.Surface.Canvas;
//         canvas.Clear();

//         int width = e.Info.Width;
//         int height = e.Info.Height;

//         if (_maskBitmap == null || _maskBitmap.Width != width || _maskBitmap.Height != height)
//         {
//             _maskBitmap = new SKBitmap(width, height);
//             _maskCanvas = new SKCanvas(_maskBitmap);
//             _maskCanvas.Clear(SKColors.Gray);
//             _scratchCompleted = false;
//             _isAutoRevealed = false;
//         }

//         // Draw the scratchable layer (mask)
//         var paint = new SKPaint
//         {
//             BlendMode = SKBlendMode.SrcOver
//         };
//         canvas.DrawBitmap(_maskBitmap, SKPoint.Empty, paint);

//         // Debug mode: show % cleared
//         if (_parent.IsDebugModeEnabled)
//         {
//             var percent = ScratchUtils.CalculateClearedPercent(_maskBitmap);

//             using var debugFont = new SKFont(SKTypeface.Default, 50);
//             using var debugPaint = new SKPaint
//             {
//                 Color = SKColors.Red,
//                 IsAntialias = true,
//             };
//             ScratchUtils.DrawDebugPercentage(canvas, percent, new SKPoint(20, 60));
//         }

//     }

//     private SKPoint? _lastTouchPoint = null;

//     private bool _hasMoved = false;

//     public void OnTouch(object sender, SKTouchEventArgs e)
//     {
//         if (_scratchCompleted && !_parent.IsTapToRevealEnabled)
//             return;

//         var canvasView = sender as SKCanvasView;

//         switch (e.ActionType)
//         {
//             case SKTouchAction.Pressed:
//                 _lastTouchPoint = e.Location;
//                 _hasMoved = false;
//                 break;

//             case SKTouchAction.Moved when _lastTouchPoint.HasValue:
//                 _hasMoved = true;

//                 using (var paint = new SKPaint
//                 {
//                     Style = SKPaintStyle.Stroke,
//                     StrokeCap = SKStrokeCap.Round,
//                     StrokeWidth = _parent.ScratchBrushSize,
//                     Color = SKColors.Transparent,
//                     BlendMode = SKBlendMode.Clear,
//                     IsAntialias = true
//                 })
//                 using (var path = new SKPath())
//                 {
//                     path.MoveTo(_lastTouchPoint.Value);
//                     path.LineTo(e.Location);
//                     _maskCanvas.DrawPath(path, paint);
//                 }

//                 _lastTouchPoint = e.Location;

//                 if (!_scratchCompleted)
//                 {
//                     var percent = ScratchUtils.CalculateClearedPercent(_maskBitmap);
//                     if (percent >= _parent.RevealThreshold)
//                     {
//                         _scratchCompleted = true;
//                         _parent.OnScratchCompleted();

//                         if (_parent.AutoRevealEnabled && !_isAutoRevealed)
//                         {
//                             _isAutoRevealed = true;
//                             _maskCanvas.Clear(SKColors.Transparent);
//                             canvasView?.InvalidateSurface();
//                             AnimateReveal();
//                         }
//                     }
//                 }

//                 canvasView?.InvalidateSurface();
//                 break;

//             case SKTouchAction.Released:
//                 _lastTouchPoint = null;

//                 // ✅ Only trigger tap-to-reveal if the user *did not move*
//                 if (_parent.IsTapToRevealEnabled && !_hasMoved && !_scratchCompleted)
//                 {
//                     RevealAll();
//                 }

//                 break;
//         }

//         e.Handled = true;
//     }

//     public void ResetMask()
//     {
//         if (_maskBitmap != null)
//         {
//             _maskCanvas.Clear(SKColors.Gray);
//             _scratchCompleted = false;
//             _isAutoRevealed = false;
//         }
//     }

//     private void RevealAll()
//     {
//         _maskCanvas.Clear(SKColors.Transparent);
//         _scratchCompleted = true;
//         _parent.OnScratchCompleted();

//         if (_parent.AutoRevealEnabled && !_isAutoRevealed)
//         {
//             _isAutoRevealed = true;
//             AnimateReveal();
//         }

//         (_parent.Content as Grid)?.Children.OfType<SKCanvasView>().FirstOrDefault()?.InvalidateSurface();
//     }

//     private void AnimateReveal()
//     {
//         _maskCanvas.Clear(SKColors.Transparent); // Clear full mask
//         (_parent.Content as Grid)?.Children.OfType<SKCanvasView>().FirstOrDefault()?.InvalidateSurface();

//         ScratchAnimationHelper.PlayRevealAnimation(_parent, _parent.RevealAnimationType);
//     }
// }

public class FreakyScratchViewDrawable
{
    private SKBitmap _maskBitmap;
    private SKCanvas _maskCanvas;
    private SKBitmap _frontBitmap;
    private bool _isLoadingFrontImage;
    private SKPoint? _lastTouchPoint = null;
    private bool _scratchCompleted = false;
    private bool _isAutoRevealed = false;
    private bool _hasMoved = false;

    private readonly FreakyScratchView _parent;

    public FreakyScratchViewDrawable(FreakyScratchView parent)
    {
        _parent = parent;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // not used — handled by SKCanvasView
    }

    public void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        var info = e.Info;

        canvas.Clear(SKColors.Transparent);

        if (_maskBitmap == null || _maskBitmap.Width != info.Width || _maskBitmap.Height != info.Height)
        {
            _maskBitmap = new SKBitmap(info.Width, info.Height);
            _maskCanvas = new SKCanvas(_maskBitmap);
            _scratchCompleted = false;
            _isAutoRevealed = false;

            FillMask(info.Width, info.Height, sender as SkiaSharp.Views.Maui.Controls.SKCanvasView);
        }

        canvas.DrawBitmap(_maskBitmap, SKPoint.Empty);
    }

    private void FillMask(int width, int height, SkiaSharp.Views.Maui.Controls.SKCanvasView canvasView)
    {
        if (_parent.FrontImageSource != null)
        {
            if (_frontBitmap != null)
            {
                // Image already loaded — draw it scaled to fill the mask
                using var image = SKImage.FromBitmap(_frontBitmap);
                _maskCanvas.DrawImage(image, new SKRect(0, 0, width, height),
                    new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
            }
            else if (!_isLoadingFrontImage)
            {
                // Fill with FrontColor as placeholder while image loads
                FillMaskWithColor();
                _ = LoadFrontImageAsync(width, height, canvasView);
            }
        }
        else
        {
            FillMaskWithColor();
        }
    }

    private void FillMaskWithColor()
    {
        var c = _parent.FrontColor;
        _maskCanvas.Clear(new SKColor(
            (byte)(c.Red * 255), (byte)(c.Green * 255), (byte)(c.Blue * 255), (byte)(c.Alpha * 255)));
    }

    private async Task LoadFrontImageAsync(int width, int height, SkiaSharp.Views.Maui.Controls.SKCanvasView canvasView)
    {
        _isLoadingFrontImage = true;
        try
        {
            var bitmap = await ImageSourceToSKBitmapAsync(_parent.FrontImageSource);
            if (bitmap != null && _maskBitmap != null)
            {
                _frontBitmap = bitmap;
                _maskCanvas.Clear(SKColors.Transparent);
                using var image = SKImage.FromBitmap(_frontBitmap);
                _maskCanvas.DrawImage(image, new SKRect(0, 0, width, height),
                    new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
                MainThread.BeginInvokeOnMainThread(() => canvasView?.InvalidateSurface());
            }
        }
        finally
        {
            _isLoadingFrontImage = false;
        }
    }

    private static async Task<SKBitmap> ImageSourceToSKBitmapAsync(ImageSource imageSource)
    {
        try
        {
            Stream stream = imageSource switch
            {
                StreamImageSource s => await s.Stream(CancellationToken.None),
                FileImageSource f => await FileSystem.OpenAppPackageFileAsync(f.File),
                UriImageSource u => await new HttpClient().GetStreamAsync(u.Uri),
                _ => null
            };

            if (stream is null) return null;
            using (stream)
                return SKBitmap.Decode(stream);
        }
        catch
        {
            return null;
        }
    }

    public void OnTouch(object sender, SKTouchEventArgs e)
    {
        if (_maskCanvas == null)
            return;

        if (_scratchCompleted && !_parent.IsTapToRevealEnabled)
            return;

        var canvasView = sender as SKCanvasView;

        switch (e.ActionType)
        {
            case SKTouchAction.Pressed:
                _lastTouchPoint = e.Location;
                _hasMoved = false;
                break;

            case SKTouchAction.Moved when _lastTouchPoint.HasValue:
                _hasMoved = true;

                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeCap = SKStrokeCap.Round,
                    StrokeWidth = _parent.BrushSize,
                    Color = SKColors.Transparent,
                    BlendMode = SKBlendMode.Clear,
                    IsAntialias = true
                })
                using (var path = new SKPath())
                {
                    path.MoveTo(_lastTouchPoint.Value);
                    path.LineTo(e.Location);
                    _maskCanvas.DrawPath(path, paint);
                }

                _lastTouchPoint = e.Location;

                if (!_scratchCompleted)
                {
                    var percent = ScratchUtils.CalculateClearedPercent(_maskBitmap);
                    if (percent >= _parent.RevealThreshold)
                    {
                        _scratchCompleted = true;
                        _parent.OnScratchCompleted();

                        if (_parent.AutoRevealEnabled && !_isAutoRevealed)
                        {
                            _isAutoRevealed = true;
                            AnimateReveal();
                        }
                    }
                }

                canvasView?.InvalidateSurface();
                break;

            case SKTouchAction.Released:
                _lastTouchPoint = null;

                // Tap-to-reveal: only if finger didn’t move
                if (_parent.IsTapToRevealEnabled && !_hasMoved && !_scratchCompleted)
                {
                    RevealAll();
                }

                break;
        }

        e.Handled = true;
    }

    private void AnimateReveal()
    {
        _maskCanvas.Clear(SKColors.Transparent); // Clear full mask
        (_parent.Content as Grid)?.Children.OfType<SKCanvasView>().FirstOrDefault()?.InvalidateSurface();

        ScratchAnimationHelper.PlayRevealAnimation(_parent, _parent.RevealAnimationType);
    }

    public void RevealAll()
    {
        if (_scratchCompleted)
            return;

        _scratchCompleted = true;
        _isAutoRevealed = true;

        _parent.OnScratchCompleted();
        AnimateReveal();
    }

    public void ResetMask()
    {
        _maskBitmap = null;
    }

    public void ResetFrontBitmap()
    {
        _frontBitmap = null;
        _isLoadingFrontImage = false;
        _maskBitmap = null;
    }

    public void Reset()
    {
        _maskBitmap = null;
        _scratchCompleted = false;
        _isAutoRevealed = false;
        _lastTouchPoint = null;
    }
}
