using System;
namespace Maui.FreakyControls;

public partial class SignaturePadViewHandler
{
    protected override Platforms.Android.SignaturePadView CreatePlatformView()
    {
        return new(this.Context);
    }
}

