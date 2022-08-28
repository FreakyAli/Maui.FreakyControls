using System;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Graphics;
using MAUI.FreakyControls;

namespace Samples
{
	public partial class ExtendedLabelHandler
	{
        protected override AppCompatTextView CreatePlatformView()
        {
            var nativeView = new AppCompatTextView(this.Context)
            {

            };
            return nativeView;
        }

        private void HandleNativeHasUnderline(bool hasUnderline, Color underlineColor)
        {
            if (hasUnderline)
            {
                var AndroidColor = underlineColor.ToNativeColor();
                var colorFilter = BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(
                    AndroidColor, BlendModeCompat.SrcIn);
                PlatformView.Background?.SetColorFilter(colorFilter);
            }
            else
            {
                PlatformView.Background?.ClearColorFilter();
            }
        }
    }
}

