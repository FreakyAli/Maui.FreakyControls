using Foundation;
using Microsoft.Maui.Platform;
using ObjCRuntime;

namespace Maui.FreakyControls.Platforms.iOS.NativeControls
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