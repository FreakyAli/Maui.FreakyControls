using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Platforms.iOS;
using Maui.FreakyControls.Platforms.iOS.NativeControls;
using Maui.FreakyControls.Enums;
using Microsoft.Maui.Platform;
using UIKit;

namespace Maui.FreakyControls;

public partial class FreakyEntryHandler
{
    protected override MauiTextField CreatePlatformView()
    {
        var mauiTextField = new FreakyUITextfield
        {
            BorderStyle = UITextBorderStyle.None,
            ClipsToBounds = true,
        };
        mauiTextField.Layer.BorderWidth = 0;
        mauiTextField.Layer.BorderColor = UIColor.Clear.CGColor;
        return mauiTextField;
    }

    internal void HandleAllowCopyPaste(FreakyEntry entry)
    {
        (PlatformView as FreakyUITextfield).AllowCopyPaste = entry.AllowCopyPaste;
    }

    internal async Task HandleAndAlignImageSourceAsync(FreakyEntry entry)
    {
        var uiImage = await entry.ImageSource?.ToNativeImageSourceAsync();
        if (uiImage is not null)
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
        if (VirtualView is FreakyEntry entry)
        {
            entry.ImageCommand?.ExecuteCommandIfAvailable(entry.ImageCommandParameter);
        }
    }
}