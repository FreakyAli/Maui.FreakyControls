using System;
using Foundation;
using Maui.FreakyControls.Platforms.iOS;
using UIKit;

namespace Maui.FreakyControls;

public partial class FreakyStateLayoutHandler
{
    protected override Microsoft.Maui.Platform.ContentView CreatePlatformView()
    {

        return new NativeStateLayout(this.VirtualView);
    }
}

