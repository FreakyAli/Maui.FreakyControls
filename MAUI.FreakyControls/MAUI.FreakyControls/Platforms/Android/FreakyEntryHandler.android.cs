using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using MAUI.FreakyControls.Extensions;
using MAUI.FreakyControls.Platforms.Android;
using MAUI.FreakyControls.Platforms.Android.NativeControls;
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
            if (imageBitmap != null)
            {
                var bitmapDrawable = new BitmapDrawable(CurrentActivity.Resources,
                    Bitmap.CreateScaledBitmap(imageBitmap, entry.ImageWidth * 2, entry.ImageHeight * 2, true));
                var freakyEditText = (PlatformView as FreakyEditText);
                freakyEditText.SetDrawableClickListener(new DrawableHandlerCallback(entry));
                //PlatformView.SetOnTouchListener(new FreakyEntryTouchListener(PlatformView, entry));
                //PlatformView.Touch += PlatformView_Touch;
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

        private void PlatformView_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {

        }
    }

    public class DrawableHandlerCallback : IDrawableClickListener
    {
        private readonly FreakyEntry frentry;

        public DrawableHandlerCallback(FreakyEntry frentry)
        {
            this.frentry = frentry;
        }
        public void OnClick(DrawablePosition target)
        {
            switch (target)
            {
                case DrawablePosition.LEFT:
                    if (frentry.ImageCommand?.CanExecute(frentry.ImageCommandParameter) == true)
                    {
                        frentry.ImageCommand.Execute(frentry.ImageCommandParameter);
                    }
                    break;
                case DrawablePosition.RIGHT:
                    if (frentry.ImageCommand?.CanExecute(frentry.ImageCommandParameter) == true)
                    {
                        frentry.ImageCommand.Execute(frentry.ImageCommandParameter);
                    }
                    break;
            }
        }
    }
}

