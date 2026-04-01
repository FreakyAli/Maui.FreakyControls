#if WINDOWS
using Microsoft.UI.Xaml.Controls;
#endif

namespace Maui.FreakyControls
{
    public partial class FreakyEditorHandler
    {
#if WINDOWS
        internal void HandleAllowCopyPaste(FreakyEditor entry)
        {
            if (!entry.AllowCopyPaste)
            {
                PlatformView.ContextFlyout = null;
            }
        }
#endif
    }
}
