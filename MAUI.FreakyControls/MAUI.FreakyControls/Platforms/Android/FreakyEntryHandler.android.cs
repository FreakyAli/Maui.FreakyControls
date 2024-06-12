using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Platforms.Android.NativeControls;
using Maui.FreakyControls.Enums;
using static Microsoft.Maui.ApplicationModel.Platform;

namespace Maui.FreakyControls;

public partial class FreakyEntryHandler
{
    protected override AppCompatEditText CreatePlatformView()
    {
        var _nativeView = new FreakyEditText(Context);
        var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        ViewCompat.SetBackgroundTintList(_nativeView, colorStateList);
        return _nativeView;
    }

    internal void HandleAllowCopyPaste(FreakyEntry entry)
    {
        if (entry.AllowCopyPaste)
        {
            PlatformView.CustomInsertionActionModeCallback = null;
            PlatformView.CustomSelectionActionModeCallback = null;
        }
        else
        {
            PlatformView.CustomInsertionActionModeCallback = new CustomInsertionActionModeCallback();
            PlatformView.CustomSelectionActionModeCallback = new CustomSelectionActionModeCallback();
        }
    }

    internal async Task HandleAndAlignImageSourceAsync(FreakyEntry entry)
    {
        var imageBitmap = await entry.ImageSource?.ToNativeImageSourceAsync();
        if (imageBitmap is not null)
        {
            var bitmapDrawable = new BitmapDrawable(CurrentActivity.Resources,
                Bitmap.CreateScaledBitmap(imageBitmap, entry.ImageWidth * 2, entry.ImageHeight * 2, true));
            var freakyEditText = (PlatformView as FreakyEditText);
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