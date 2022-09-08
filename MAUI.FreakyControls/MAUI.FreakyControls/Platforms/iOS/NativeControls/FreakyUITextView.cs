using System;
using Foundation;
using Microsoft.Maui.Platform;
using ObjCRuntime;

namespace MAUI.FreakyControls.Platforms.iOS.NativeControls
{
    public class FreakyUITextView : MauiTextView
    {
        public bool AllowCopyPaste { get; set; } = true;

        public override bool CanPerform(Selector action, NSObject withSender)
        {
            return AllowCopyPaste;
        }
    }
}

