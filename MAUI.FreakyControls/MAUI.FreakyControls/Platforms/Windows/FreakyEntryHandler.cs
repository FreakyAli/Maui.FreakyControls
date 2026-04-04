#if WINDOWS
using Maui.FreakyControls.Platforms.Windows.NativeControls;
#endif

namespace Maui.FreakyControls
{
    public sealed partial class FreakyEntryHandler
    {
#if WINDOWS
        private Microsoft.UI.Xaml.Controls.Primitives.FlyoutBase? _originalContextFlyout;

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

        internal async Task HandleAndAlignImageSourceAsync(FreakyEntry entry)
        {
            if (entry.ImageSource is null)
                return;

            await WindowsIconInjector.InjectAsync(
                PlatformView,
                entry.ImageSource,
                entry.ImageAlignment,
                entry.ImageWidth,
                entry.ImageHeight,
                entry.ImagePadding,
                () => entry.ImageCommand?.ExecuteWhenAvailable(entry.ImageCommandParameter));
        }
#endif
    }
}
