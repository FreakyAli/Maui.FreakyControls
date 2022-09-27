using System;
using Maui.FreakyControls.Extensions;

namespace Maui.FreakyControls;

public sealed partial class FreakyButtonHandler
{
    private void HandleLongPressed()
    {
        PlatformView.LongClick += PlatformView_LongClick;
    }

    private void PlatformView_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
    {
        ExecuteLongPressedHandlers();
    }
}

