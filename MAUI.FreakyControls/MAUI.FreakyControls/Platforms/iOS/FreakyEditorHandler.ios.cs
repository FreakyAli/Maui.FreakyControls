using System;
using CoreGraphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;
using CoreAnimation;
using static MAUI.FreakyControls.Platforms.iOS.NativeExtensions;

namespace MAUI.FreakyControls
{
    public partial class FreakyEditorHandler 
    {
        CALayer bottomline;

        protected override UITextView CreatePlatformView()
        {
            var _nativeView = new UITextView();
            return _nativeView;
        }

        private void HandleNativeHasUnderline(bool hasUnderline, Color color)
        {
            if (hasUnderline)
            {
                var uiColor = color.ToNativeColor();
                bottomline = BottomLineDrawer(uiColor);
                bottomline.Frame = new CGRect(x: 0, y: PlatformView.Frame.Size.Height - 5,
                    width: PlatformView.Frame.Size.Width, height: 1);
                PlatformView.Layer.AddSublayer(bottomline);
                PlatformView.Layer.MasksToBounds = true;
            }
            else
            {
                bottomline?.RemoveFromSuperLayer();
            }
        }
    }
}

