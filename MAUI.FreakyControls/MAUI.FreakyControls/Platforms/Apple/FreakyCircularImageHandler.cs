#nullable disable

using Maui.FreakyControls.Platforms.Apple.NativeControls;
using UIKit;

namespace Maui.FreakyControls
{
    public sealed partial class FreakyCircularImageHandler
    {
        protected override UIImageView CreatePlatformView() => new FreakyCircularUIImageView(this);
    }
}
