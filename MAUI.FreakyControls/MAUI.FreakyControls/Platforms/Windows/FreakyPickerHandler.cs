#if WINDOWS
using Maui.FreakyControls.Platforms.Windows.NativeControls;
#endif

namespace Maui.FreakyControls
{
    public partial class FreakyPickerHandler
    {
        internal async Task HandleAndAlignImageSourceAsync(FreakyPicker entry)
        {
#if WINDOWS
            await WindowsIconInjector.InjectAsync(
                PlatformView,
                entry.ImageSource,
                entry.ImageAlignment,
                entry.ImageWidth,
                entry.ImageHeight,
                entry.ImagePadding,
                () => entry.ImageCommand?.ExecuteWhenAvailable(entry.ImageCommandParameter));
#else
            await Task.CompletedTask;
#endif
        }
    }
}
