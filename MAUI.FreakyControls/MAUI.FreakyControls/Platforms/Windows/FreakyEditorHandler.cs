#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
#endif

namespace Maui.FreakyControls
{
    public partial class FreakyEditorHandler
    {
#if WINDOWS
        private Microsoft.UI.Xaml.Controls.Primitives.FlyoutBase _originalContextFlyout;

        internal void HandleAllowCopyPaste(FreakyEditor entry)
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
#endif
    }
}
