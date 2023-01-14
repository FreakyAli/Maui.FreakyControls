using System;
using Maui.FreakyControls.Extensions;
using System.Drawing;
using Maui.FreakyControls.Shared.Enums;
using Microsoft.Maui.Platform;
using UIKit;
using Maui.FreakyControls.Platforms.iOS;

namespace Maui.FreakyControls;

public partial class FreakyDatePickerHandler
{
    protected override void ConnectHandler(MauiDatePicker platformView)
    {
        base.ConnectHandler(platformView);
        platformView.BorderStyle = UITextBorderStyle.None;
        platformView.ClipsToBounds = true;
        platformView.Layer.BorderWidth = 0;
        platformView.Layer.BorderColor = UIColor.Clear.CGColor;
    }

    internal async Task HandleAndAlignImageSourceAsync(FreakyDatePicker entry)
    {
        var uiImage = await entry.ImageSource?.ToNativeImageSourceAsync();
        if (uiImage != null)
        {
            var uiView = uiImage.UiImageToUiView(entry.ImageHeight, entry.ImageWidth, entry.ImagePadding);
            uiView.UserInteractionEnabled = true;
            var tapGesture = new UITapGestureRecognizer(OnViewTouchBegan);
            uiView.AddGestureRecognizer(tapGesture);
            switch (entry.ImageAlignment)
            {
                case ImageAlignment.Left:
                    PlatformView.LeftViewMode = UITextFieldViewMode.Always;
                    PlatformView.LeftView = uiView;
                    break;
                case ImageAlignment.Right:
                    PlatformView.RightViewMode = UITextFieldViewMode.Always;
                    PlatformView.RightView = uiView;
                    break;
            }
        }
    }

    private void OnViewTouchBegan()
    {
        if (VirtualView is FreakyDatePicker entry)
        {
            entry.ImageCommand?.ExecuteCommandIfAvailable(entry.ImageCommandParameter);
        }
    }
}