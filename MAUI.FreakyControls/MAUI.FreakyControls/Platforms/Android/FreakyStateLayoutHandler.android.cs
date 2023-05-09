using System;
using Android.Content;
using Android.Text.Method;
using Android.Views;
using Java.Lang;
using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Platforms.Android;
using Microsoft.Maui.Platform;

namespace Maui.FreakyControls;

public partial class FreakyStateLayoutHandler
{
    protected override ContentViewGroup CreatePlatformView()
    {
        return new NativeStateLayout(Context,VirtualView);
    }
}