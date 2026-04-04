#if WINDOWS
using Maui.FreakyControls.Platforms.Windows.NativeControls;
#endif

namespace Maui.FreakyControls
{
    public partial class FreakyTimePickerHandler
    {
        internal async Task HandleAndAlignImageSourceAsync(FreakyTimePicker entry)
        {
#if WINDOWS
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
#else
            await Task.CompletedTask;
#endif
        }
    }
}
