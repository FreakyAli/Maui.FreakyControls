using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using AndroidX.Core.View;
using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Platforms.Android.NativeControls;
using Maui.FreakyControls.Shared.Enums;
using Microsoft.Maui.Platform;
using static Microsoft.Maui.ApplicationModel.Platform;

namespace Maui.FreakyControls;

public partial class FreakyPickerHandler
{
    protected override MauiPicker CreatePlatformView()
    {
        var picker = new FreakyMauiPicker(Context);
        var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        ViewCompat.SetBackgroundTintList(picker, colorStateList);
        return picker;
    }

    internal async Task HandleAndAlignImageSourceAsync(FreakyPicker entry)
    {
        var imageBitmap = await entry.ImageSource?.ToNativeImageSourceAsync();
        if (imageBitmap != null)
        {
            var bitmapDrawable = new BitmapDrawable(CurrentActivity?.Resources,
                Bitmap.CreateScaledBitmap(imageBitmap, entry.ImageWidth * 2, entry.ImageHeight * 2, true));
            var freakyEditText = PlatformView as FreakyMauiPicker;
            freakyEditText.SetDrawableClickListener(new DrawableHandlerCallback(entry));
            switch (entry.ImageAlignment)
            {
                case ImageAlignment.Left:
                    freakyEditText.SetCompoundDrawablesWithIntrinsicBounds(bitmapDrawable, null, null, null);
                    break;
                case ImageAlignment.Right:
                    freakyEditText.SetCompoundDrawablesWithIntrinsicBounds(null, null, bitmapDrawable, null);
                    break;
            }
        }
        PlatformView.CompoundDrawablePadding = entry.ImagePadding;
    }
}