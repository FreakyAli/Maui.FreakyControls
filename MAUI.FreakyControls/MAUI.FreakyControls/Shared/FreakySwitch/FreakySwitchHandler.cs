using Microsoft.Maui.Handlers;
#if ANDROID
using AndroidSwitch = Google.Android.Material.MaterialSwitch.MaterialSwitch;
#endif
namespace Maui.FreakyControls;
#if ANDROID
public partial class FreakySwitchHandler : SwitchHandler
{
    protected override AndroidSwitch CreatePlatformView()
    {
        return new AndroidSwitch(Context);
    }
}
#else
public partial class FreakySwitchHandler : SwitchHandler
{
}
#endif