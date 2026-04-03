#if WINDOWS
using Microsoft.UI.Xaml.Controls;
#endif

namespace Maui.FreakyControls
{
    public sealed partial class FreakyEntryHandler
    {
#if WINDOWS
        private Microsoft.UI.Xaml.Controls.Primitives.FlyoutBase _originalContextFlyout;

        internal void HandleAllowCopyPaste(FreakyEntry entry)
        {
            if (!entry.AllowCopyPaste)
            {
                _originalContextFlyout ??= PlatformView.ContextFlyout;
                PlatformView.ContextFlyout = null;
            }
            else if (_originalContextFlyout is not null)
            {
                PlatformView.ContextFlyout = _originalContextFlyout;
                _originalContextFlyout = null;
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
