#if WINDOWS
using Microsoft.UI.Xaml.Controls;
#endif

namespace Maui.FreakyControls
{
    public sealed partial class FreakyEntryHandler
    {
#if WINDOWS
        internal void HandleAllowCopyPaste(FreakyEntry entry)
        {
            if (!entry.AllowCopyPaste)
            {
                PlatformView.ContextFlyout = null;
            }
        }

        internal Task HandleAndAlignImageSourceAsync(FreakyEntry entry)
        {
            // TODO: Image alignment inside TextBox is not natively supported on Windows.
            // Implement by wrapping in a custom control when needed.
            return Task.CompletedTask;
        }
#endif
    }
}
