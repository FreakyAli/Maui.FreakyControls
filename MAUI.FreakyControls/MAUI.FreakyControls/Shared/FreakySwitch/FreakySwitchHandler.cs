using Microsoft.Maui.Handlers;
#if ANDROID
using AndroidSwitch = Google.Android.Material.MaterialSwitch.MaterialSwitch;
using AndroidX.Core.Graphics;
using Maui.FreakyControls.Extensions;
using Microsoft.Maui.Platform;
#endif
namespace Maui.FreakyControls;
#if ANDROID
public partial class FreakySwitchHandler : SwitchHandler
{
    protected override AndroidSwitch CreatePlatformView()
    {
        return new AndroidSwitch(Context);
    }

    public FreakySwitchHandler()
    {
        //Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping(nameof(FreakySwitch.OutlineColor), MapOutlineColor);
    }

    private void MapOutlineColor(ISwitchHandler handler, ISwitch switchh)
    {
        try
        {
            if (handler is FreakySwitchHandler freakyHandler && switchh is FreakySwitch freakySwitch)
            {
                freakyHandler.UpdateOutlineColor(freakySwitch, freakyHandler);
            }
        }
        catch (Exception ex)
        {
            ex.TraceException();
        }
    }

    private void UpdateOutlineColor(FreakySwitch freakySwitch, FreakySwitchHandler freakySwitchHandler)
    {
        if (freakySwitchHandler.PlatformView is AndroidSwitch natSwitch)
        {
            var colorFilter=BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(
                freakySwitch.OutlineColor.ToPlatform(), BlendModeCompat.SrcIn);
            natSwitch.TrackDrawable.SetColorFilter(colorFilter);
        }
    }
}
#else
public partial class FreakySwitchHandler : SwitchHandler
{
}
#endif