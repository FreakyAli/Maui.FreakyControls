namespace Maui.FreakyControls;

public static class ScratchAnimationHelper
{
    public static async void PlayRevealAnimation(FreakyScratchView view, ScratchRevealAnimationType animationType)
    {
        if (view?.FrontContent == null)
            return;

        try
        {
            switch (animationType)
            {
                case ScratchRevealAnimationType.FadeOut:
                    await PlayFadeOutAnimation(view);
                    break;

                case ScratchRevealAnimationType.Shimmer:
                    await PlayShimmerAnimation(view);
                    break;

                case ScratchRevealAnimationType.None:
                default:
                    view.FrontContent.Opacity = 0;
                    view.FrontContent.IsVisible = false;
                    break;
            }

        }
        catch
        {
            view.FrontContent.Opacity = 0;
            view.FrontContent.IsVisible = false;
        }
    }

    private static async Task PlayShimmerAnimation(FreakyScratchView view)
    {
        if (view?.FrontContent == null)
            return;

        var shimmerBox = new BoxView
        {
            BackgroundColor = new Color(1, 1, 1, 0.4f),
            WidthRequest = 150,
            HeightRequest = view.Width,
            InputTransparent = true,
            Opacity = 0
        };

        if (view.Content is Grid grid)
        {
            shimmerBox.TranslationX = -300;
            grid.Children.Add(shimmerBox);

            // Fade in shimmer
            await shimmerBox.FadeTo(0.5, 100);
            await shimmerBox.TranslateTo(view.Width + 150, 0, 600, Easing.SinInOut);
            await shimmerBox.FadeTo(0, 200);

            grid.Children.Remove(shimmerBox);
        }

        view.FrontContent.Opacity = 0;
        view.FrontContent.IsVisible = false;
    }

    private static async Task PlayFadeOutAnimation(FreakyScratchView view)
    {
        if (view?.FrontContent == null)
            return;

        try
        {
            await view.FrontContent.FadeTo(0, 500, Easing.CubicOut);
            view.FrontContent.IsVisible = false;
        }
        catch
        {
            // In case animation fails due to disposal/timing
            view.FrontContent.Opacity = 0;
            view.FrontContent.IsVisible = false;
        }
    }
}
