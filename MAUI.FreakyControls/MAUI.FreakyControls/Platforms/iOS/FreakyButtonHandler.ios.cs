using System;
using UIKit;

namespace Maui.FreakyControls
{
    public partial class FreakyButtonHandler
    {
        private void HandleLongPressed()
        {
            PlatformView.UserInteractionEnabled = true;
            PlatformView.AddGestureRecognizer(new UILongPressGestureRecognizer(HandleLongClick));
        }

        private void HandleLongClick(UILongPressGestureRecognizer sender)
        {
            ExecuteLongPressedHandlers();
        }
    }
}

