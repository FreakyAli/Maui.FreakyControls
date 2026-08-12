using Maui.FreakyControls.Platforms.Apple.NativeControls;
using Microsoft.Maui.Platform;

namespace Maui.FreakyControls
{
    public partial class FreakyEditorHandler
    {
        protected override MauiTextView CreatePlatformView()
        {
            var mauiTextview = new FreakyUITextView();
            return mauiTextview;
        }

        internal void HandleAllowCopyPaste(FreakyEditor entry)
        {
            if (PlatformView is FreakyUITextView textView)
            {
                textView.AllowCopyPaste = entry.AllowCopyPaste;
            }
        }
    }
}
