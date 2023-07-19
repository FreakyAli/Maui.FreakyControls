using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using static Maui.FreakyControls.Platforms.Android.NativeExtensions;
using Microsoft.Maui.Platform;

namespace Maui.FreakyControls;

public partial class FreakySliderHandler
{
    private void HandleSliderChanges(FreakySliderHandler sliderHandler)
    {
        if (sliderHandler.PlatformView != null)
        {
            var customSlider = (FreakySlider)sliderHandler.VirtualView;

            if (customSlider.ThumbColor != Colors.Transparent)
                sliderHandler.PlatformView.Thumb.SetColorFilter(customSlider.ThumbColor.ToPlatform(), PorterDuff.Mode.SrcIn);

            BuildVersionCodes androidVersion = Build.VERSION.SdkInt;

            if (androidVersion >= BuildVersionCodes.M)
            {
                var trackCornerRadius = sliderHandler.Context.DpToPixels(customSlider.TrackCornerRadius);
                var trackHeight = sliderHandler.Context.DpToPixels(customSlider.TrackHeight);

                var progressGradientDrawable = new GradientDrawable(GradientDrawable.Orientation.LeftRight, new int[] { customSlider.ActiveTrackColor.ToAndroid(), customSlider.ActiveTrackColor.ToAndroid() });
                progressGradientDrawable.SetCornerRadius(trackCornerRadius);
                var progress = new ClipDrawable(progressGradientDrawable, GravityFlags.Left, ClipDrawableOrientation.Horizontal);

                var background = new GradientDrawable();
                background.SetColor(customSlider.InactiveTrackColor.ToPlatform());
                background.SetCornerRadius(trackCornerRadius);

                var progressDrawable = new LayerDrawable(new Drawable[] { background, progress });

                progressDrawable.SetLayerHeight(0, (int)trackHeight);
                progressDrawable.SetLayerHeight(1, (int)trackHeight);
                progressDrawable.SetLayerGravity(0, GravityFlags.CenterVertical);
                progressDrawable.SetLayerGravity(1, GravityFlags.CenterVertical);
                sliderHandler.PlatformView.ProgressDrawable = progressDrawable;
            }
            else
            {
                if (sliderHandler.VirtualView is FreakySlider freakySlider)
                    freakySlider.MinimumTrackColor = customSlider.ActiveTrackColor;

                sliderHandler.PlatformView.SecondaryProgressTintList = ColorStateList.ValueOf(customSlider.InactiveTrackColor.ToAndroid());
                sliderHandler.PlatformView.SecondaryProgressTintMode = PorterDuff.Mode.SrcIn;
                sliderHandler.PlatformView.SecondaryProgress = int.MaxValue;
            }
            sliderHandler.PlatformView.SetPadding(52, sliderHandler.PlatformView.PaddingTop, 52, sliderHandler.PlatformView.PaddingBottom);
        }
    }

    private void ApplyBounds(FreakySliderHandler handler)
    {
        SeekBar seekbar = handler.PlatformView;

        Drawable thumb = seekbar.Thumb;
        int thumbTop = (seekbar.Height / 2 - thumb.IntrinsicHeight / 2);

        thumb.SetBounds(thumb.Bounds.Left, thumbTop, thumb.Bounds.Left + thumb.IntrinsicWidth, thumbTop + thumb.IntrinsicHeight);

    }
}

