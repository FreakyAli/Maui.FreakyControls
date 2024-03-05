using Maui.FreakyControls.Platforms.iOS.NativeControls;
using Microsoft.Maui.Platform;
using UIKit;

namespace Maui.FreakyControls;

internal partial class FreakyInternalEntryHandler
{
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
}