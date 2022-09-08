using System;
using System.Drawing;
using CoreAnimation;
using CoreGraphics;
using MAUI.FreakyControls.Extensions;
using MAUI.FreakyControls.Platforms.iOS.NativeControls;
using MAUI.FreakyControls.Shared.Enums;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace MAUI.FreakyControls
{
    public partial class FreakyEntryHandler
    {
        UIView uiView;

        protected override MauiTextField CreatePlatformView()
        {
            var mauiTextField = new FreakyUITextfield
            {
                BorderStyle = UITextBorderStyle.None,
                ClipsToBounds = true,
            };
            mauiTextField.Layer.BorderWidth = 0;
            mauiTextField.Layer.BorderColor = UIColor.Clear.CGColor;
            return mauiTextField;
        }

        internal void HandleAllowCopyPaste(FreakyEntry entry)
        {
            (PlatformView as FreakyUITextfield).AllowCopyPaste = entry.AllowCopyPaste;
        }

        internal async Task HandleAndAlignImageSourceAsync(FreakyEntry entry)
        {
            var uiImage = await entry.ImageSource?.ToNativeImageSourceAsync();
            if (uiImage != null)
            {
                uiView = UIImageToUIView(uiImage, entry.ImageHeight, entry.ImageWidth, entry.ImagePadding);
                uiView.UserInteractionEnabled = true;
                var tapGesture = new UITapGestureRecognizer(OnViewTouchBegan);
                uiView.AddGestureRecognizer(tapGesture);
                switch (entry.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        PlatformView.LeftViewMode = UITextFieldViewMode.Always;
                        PlatformView.LeftView = uiView;
                        break;
                    case ImageAlignment.Right:
                        PlatformView.RightViewMode = UITextFieldViewMode.Always;
                        PlatformView.RightView = uiView;
                        break;
                }
            }

            PlatformView.BorderStyle = UITextBorderStyle.None;
        }

        private void OnViewTouchBegan()
        {
            if (VirtualView is FreakyEntry entry)
            {
                if (entry.ImageCommand?.CanExecute(entry.ImageCommandParameter) == true)
                {
                    entry.ImageCommand.Execute(entry.ImageCommandParameter);
                }
            }
        }

        private UIView UIImageToUIView(UIImage image, int height, int width, int padding)
        {
            var uiImageView = new UIImageView(image)
            {
                Frame = new RectangleF(0, 0, height, width)
            };
            UIView uiView = new UIView(new System.Drawing.Rectangle(0, 0, width + padding, height));
            uiView.AddSubview(uiImageView);
            return uiView;
        }
    }
}

