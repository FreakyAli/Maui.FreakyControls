using Foundation;
using Microsoft.Maui.Platform;
using ObjCRuntime;

namespace Maui.FreakyControls.Platforms.iOS.NativeControls;

public class FreakyUITextView : MauiTextView
{
    public bool AllowCopyPaste { get; set; } = true;

    public override bool CanPerform(Selector action, NSObject withSender)
    {
        if (action.Name == "paste:" || action.Name == "copy:" || action.Name == "cut:")
            return AllowCopyPaste;
        return base.CanPerform(action, withSender);
    }
}