using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using MAUI.FreakyControls.Extensions;
using MAUI.FreakyControls.Platforms.Android;
using MAUI.FreakyControls.Shared.Enums;
using Microsoft.Maui.Handlers;
using static Android.Views.View;
using static Microsoft.Maui.ApplicationModel.Platform;


namespace MAUI.FreakyControls
{
    public partial class FreakyEntryHandler
    {
        protected override AppCompatEditText CreatePlatformView()
        {
            var _nativeView = new AppCompatEditText(Context);
            var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
            ViewCompat.SetBackgroundTintList(_nativeView, colorStateList);
            return _nativeView;
        }

        internal async Task HandleAndAlignImageSourceAsync(FreakyEntry entry)
        {
            var imageBitmap = await entry.ImageSource?.ToNativeImageSourceAsync();
            if (imageBitmap != null)
            {
                var bitmapDrawable = new BitmapDrawable(CurrentActivity.Resources,
                    Bitmap.CreateScaledBitmap(imageBitmap, entry.ImageWidth * 2, entry.ImageHeight * 2, true));
                PlatformView.SetOnTouchListener(new FreakyEntryTouchListener(PlatformView, entry));
                switch (entry.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        PlatformView.SetCompoundDrawablesWithIntrinsicBounds(bitmapDrawable, null, null, null);
                        break;
                    case ImageAlignment.Right:
                        PlatformView.SetCompoundDrawablesWithIntrinsicBounds(null, null, bitmapDrawable, null);
                        break;
                }
            }
            PlatformView.CompoundDrawablePadding = entry.ImagePadding;
        }
    }
}

