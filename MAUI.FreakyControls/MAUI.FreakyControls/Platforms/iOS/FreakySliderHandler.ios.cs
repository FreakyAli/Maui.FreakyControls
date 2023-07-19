using Microsoft.Maui.Platform;

namespace Maui.FreakyControls;
public partial class FreakySliderHandler
{
    private void HandleSliderChanges(FreakySliderHandler handler)
    {
        if (handler.PlatformView != null)
        {
            var customSlider = (FreakySlider)handler.VirtualView;

            if (handler.VirtualView is FreakySlider slider)
            {
                slider.MinimumTrackColor = customSlider.ActiveTrackColor;
                slider.MaximumTrackColor = customSlider.InactiveTrackColor;
            }

            if (customSlider.ThumbColor != Colors.Transparent)
                handler.PlatformView.ThumbTintColor = customSlider.ThumbColor.ToPlatform();
        }
    }
}