using Color = Microsoft.Maui.Graphics.Color;
#if ANDROID
#endif
#if IOS
using Maui.FreakyControls.Platforms.iOS;
#endif
#if IOS || MACCATALYST
using NativeImage = UIKit.UIImage;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#endif

namespace Maui.FreakyControls.Extensions;

public static class AnimationExtensions
{
    public static Task<bool> ChangeBackgroundColorTo(this View self, Color newColor, uint length = 250, Easing easing = null)
    {
        Task<bool> ret = new Task<bool>(() => false);

        if (!self.AnimationIsRunning(nameof(ChangeBackgroundColorTo)))
        {
            Color fromColor = self.BackgroundColor;

            try
            {
                Func<double, Color> transform = (t) =>
                  Color.FromRgba(fromColor.Red + t * (newColor.Red - fromColor.Red),
                                 fromColor.Green + t * (newColor.Green - fromColor.Green),
                                 fromColor.Blue + t * (newColor.Blue - fromColor.Blue),
                                 fromColor.Alpha + t * (newColor.Alpha - fromColor.Alpha));

                ret = TransmuteColorAnimation(self, nameof(ChangeBackgroundColorTo), transform, length, easing);
            }
            catch (Exception ex)
            {
                // to supress animation overlapping errors 
                self.BackgroundColor = fromColor;
            }
        }

        return ret;
    }

    private static Task<bool> TransmuteColorAnimation(View button, string name, Func<double, Color> transform, uint length, Easing easing)
    {
        easing = easing ?? Easing.Linear;
        var taskCompletionSource = new TaskCompletionSource<bool>();

        button.Animate(name, transform, (color) => { button.BackgroundColor = color; }, 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));
        return taskCompletionSource.Task;
    }
}

