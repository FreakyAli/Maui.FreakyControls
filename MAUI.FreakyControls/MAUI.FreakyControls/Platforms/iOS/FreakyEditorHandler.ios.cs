using Maui.FreakyControls.Platforms.iOS.NativeControls;
using Microsoft.Maui.Platform;

namespace Maui.FreakyControls;

public partial class FreakyEditorHandler
{
    protected override MauiTextView CreatePlatformView()
    {
        var mauiTextview = new FreakyUITextView();
        return mauiTextview;
    }

    internal void HandleAllowCopyPaste(FreakyEditor entry)
    {
        (PlatformView as FreakyUITextView).AllowCopyPaste = entry.AllowCopyPaste;
    }
}