﻿using Microsoft.Maui.Handlers;
#if ANDROID
using Android.Content;
using AndroidSwitch = Google.Android.Material.MaterialSwitch.MaterialSwitch;
#endif
namespace Maui.FreakyControls;

public class FreakySwitch : Switch
{

}

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