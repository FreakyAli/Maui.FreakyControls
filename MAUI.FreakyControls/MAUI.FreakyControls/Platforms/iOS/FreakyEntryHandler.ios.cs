using System;
using CoreAnimation;
using CoreGraphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace MAUI.FreakyControls
{
    public partial class FreakyEntryHandler
    {
        protected override MauiTextField CreatePlatformView()
        {
            var mauiTextField = new MauiTextField
            {
                BorderStyle = UITextBorderStyle.None,
                ClipsToBounds = true,
            };
            mauiTextField.Layer.BorderWidth = 0;
            mauiTextField.Layer.BorderColor = UIColor.Clear.CGColor;
            return mauiTextField;
        }
    }
}

