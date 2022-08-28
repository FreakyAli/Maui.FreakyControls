using System;
using CoreGraphics;
using MAUI.FreakyControls;
using UIKit;
using static MAUI.FreakyControls.Platforms.iOS.NativeExtensions;


namespace Samples
{
    public partial class ExtendedLabelHandler
    {
        CoreAnimation.CALayer bottomLine;

        protected override UILabel CreatePlatformView()
        {
            return new UILabel();
        }

        private void HandleNativeHasUnderline(bool hasUnderline, Color underlineColor)
        {
            if (hasUnderline)
            {
                var uiColor = underlineColor.ToNativeColor();
                bottomLine = BottomLineDrawer(uiColor);
                bottomLine.Frame = new CGRect(x: 0, y: PlatformView.Frame.Size.Height - 5,
                    width: PlatformView.Frame.Size.Width, height: 1);
                PlatformView.Layer.AddSublayer(bottomLine);
                PlatformView.Layer.MasksToBounds = true;
            }
            else
            {
                bottomLine?.RemoveFromSuperLayer();
            }
        }
    }
}

