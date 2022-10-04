using System;
namespace Maui.FreakyControls
{
    public partial class SignaturePadViewHandler
    {
        protected override Platforms.iOS.SignaturePadView CreatePlatformView()
        {
            return new();
        }
    }
}

