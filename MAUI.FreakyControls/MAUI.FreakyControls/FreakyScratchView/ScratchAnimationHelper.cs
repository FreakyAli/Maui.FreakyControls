#nullable disable

using SkiaSharp.Views.Maui.Controls;

namespace Maui.FreakyControls;

public static class ScratchAnimationHelper
{
    public static async void PlayRevealAnimation(FreakyScratchView view, ScratchRevealAnimationType animationType)
    {
        if (view?.Content is not Grid grid)
            return;

        var canvas = grid.Children.OfType<SKCanvasView>().FirstOrDefault();
        if (canvas is null)
            return;

        try
        {
            switch (animationType)
            {
                case ScratchRevealAnimationType.FadeOut:
                    await canvas.FadeToAsync(0, 500, Easing.CubicOut);
                    canvas.IsVisible = false;
                    break;

                case ScratchRevealAnimationType.Shimmer:
                    await PlayShimmerAnimation(view, grid, canvas);
                    break;

                case ScratchRevealAnimationType.None:
                default:
                    canvas.Opacity = 0;
                    canvas.IsVisible = false;
                    break;
            }
        }
        catch
        {
            canvas.Opacity = 0;
            canvas.IsVisible = false;
        }
    }

    private static async Task PlayShimmerAnimation(FreakyScratchView view, Grid grid, SKCanvasView canvas)
    {
        var shimmerBox = new BoxView
        {
            BackgroundColor = new Color(1, 1, 1, 0.4f),
            WidthRequest = 150,
            HeightRequest = view.Height,
            InputTransparent = true,
            Opacity = 0,
            TranslationX = -300
        };

        grid.Children.Add(shimmerBox);

        await shimmerBox.FadeToAsync(0.5, 100);
        await shimmerBox.TranslateToAsync(view.Width + 150, 0, 600, Easing.SinInOut);
        await shimmerBox.FadeToAsync(0, 200);

        grid.Children.Remove(shimmerBox);

        canvas.Opacity = 0;
        canvas.IsVisible = false;
    }
}
