using SkiaSharp;

namespace Maui.FreakyControls;

public static class ScratchUtils
{
    public static float CalculateClearedPercent(SKBitmap bitmap)
    {
        if (bitmap == null)
            return 0;

        int total = bitmap.Width * bitmap.Height;
        int cleared = 0;

        var pixels = bitmap.Pixels;
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].Alpha == 0)
                cleared++;
        }

        return (float)cleared / total;
    }

    public static void DrawDebugPercentage(SKCanvas canvas, float percent, SKPoint position)
    {
        using var font = new SKFont(SKTypeface.Default, 50);
        using var paint = new SKPaint
        {
            Color = SKColors.Red,
            IsAntialias = true
        };

        canvas.DrawText($"{percent:P0}", position.X, position.Y, SKTextAlign.Left, font, paint);
    }
}
