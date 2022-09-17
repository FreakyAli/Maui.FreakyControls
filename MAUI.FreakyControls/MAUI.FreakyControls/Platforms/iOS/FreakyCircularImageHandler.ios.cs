using System;
using Maui.FreakyControls.Platforms.iOS.NativeControls;
using UIKit;

namespace Maui.FreakyControls
{
	public partial class FreakyCircularImageHandler
	{
        protected override UIImageView CreatePlatformView() => new FreakyCircularUIImageView();
    }
}

