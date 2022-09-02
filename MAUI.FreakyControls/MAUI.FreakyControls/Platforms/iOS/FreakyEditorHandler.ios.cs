using System;
using CoreGraphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;
using CoreAnimation;
using static MAUI.FreakyControls.Platforms.iOS.NativeExtensions;
using Microsoft.Maui;

namespace MAUI.FreakyControls
{
    public partial class FreakyEditorHandler
    {
        CALayer bottomline;

        protected override MauiTextView CreatePlatformView()
        {
            var uiTextView = new FreakyUITextView();
            uiTextView.OnFrameSetCompelete += UiTextView_OnFrameSetCompelete;
            return uiTextView;
        }

        private void SetBottomLine(FreakyEditor thisSharedView, FreakyUITextView thisPlatformView)
        {
            var uiColor = thisSharedView.UnderlineColor.ToNativeColor();
            if (bottomline != null)
            {
                bottomline.RemoveFromSuperLayer();
            }
            bottomline = new CALayer();
            var width = 1;
            bottomline.BorderColor = uiColor.CGColor;
            bottomline.BorderWidth = width;
            bottomline.Frame = new CGRect(0, thisPlatformView.Frame.Height - 1, thisPlatformView.Frame.Width, 1.0);
            PlatformView.Layer.AddSublayer(bottomline);
            PlatformView.Layer.MasksToBounds = true;
        }

        private void UiTextView_OnFrameSetCompelete(object sender, EventArgs e)
        {
            if (VirtualView is FreakyEditor thisSharedView && thisSharedView.HasUnderline)
            {
                SetBottomLine(VirtualView as FreakyEditor, sender as FreakyUITextView);
            }
            else
            {
                bottomline?.RemoveFromSuperLayer();
            }
        }

        internal void HandleNativeHasUnderline(bool hasUnderline, Color color)
        {
            if (hasUnderline)
            {
                SetBottomLine(VirtualView as FreakyEditor, PlatformView as FreakyUITextView);
            }
            else
            {
                bottomline?.RemoveFromSuperLayer();
            }
        }
    }

}