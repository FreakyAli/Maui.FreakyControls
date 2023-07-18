using Maui.FreakyControls.Platforms.iOS.NativeControls;
using UIKit;

namespace Maui.FreakyControls;

public sealed partial class FreakyCircularImageHandler
{
    protected override UIImageView CreatePlatformView()
    {
        return new FreakyCircularUIImageView();
    }
}