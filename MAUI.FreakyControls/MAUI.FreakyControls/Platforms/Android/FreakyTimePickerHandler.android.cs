using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text.Format;
using AndroidX.Core.View;
using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Platforms.Android.NativeControls;
using Maui.FreakyControls.Shared.Enums;
using Microsoft.Maui.Platform;
using static Microsoft.Maui.ApplicationModel.Platform;

namespace Maui.FreakyControls;

public partial class FreakyTimePickerHandler
{
    private MauiTimePicker? _timePicker;
    private AlertDialog? _dialog;

    protected override MauiTimePicker CreatePlatformView()
    {
        _timePicker = new FreakyMauiTimePicker(Context)
        {
            ShowPicker = ShowPickerDialog,
            HidePicker = HidePickerDialog
        };

        var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        ViewCompat.SetBackgroundTintList(_timePicker, colorStateList);
        return _timePicker;
    }

    protected override void DisconnectHandler(MauiTimePicker platformView)
    {
        if (_dialog != null)
        {
            _dialog.Hide();
            _dialog = null;
        }
    }

    internal async Task HandleAndAlignImageSourceAsync(FreakyTimePicker entry)
    {
        var imageBitmap = await entry.ImageSource?.ToNativeImageSourceAsync();
        if (imageBitmap != null)
        {
            var bitmapDrawable = new BitmapDrawable(CurrentActivity?.Resources,
                Bitmap.CreateScaledBitmap(imageBitmap, entry.ImageWidth * 2, entry.ImageHeight * 2, true));
            var freakyEditText = PlatformView as FreakyMauiTimePicker;
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

    private void ShowPickerDialog()
    {
        if (VirtualView == null)
            return;

        var time = VirtualView.Time;
        ShowPickerDialog(time.Hours, time.Minutes);
    }

    // This overload is here so we can pass in the current values from the dialog
    // on an orientation change (so that orientation changes don't cause the user's date selection progress
    // to be lost). Not useful until we have orientation changed events.
    private void ShowPickerDialog(int hour, int minute)
    {
        _dialog = CreateTimePickerDialog(hour, minute);
        _dialog.Show();
    }

    private void HidePickerDialog()
    {
        if (_dialog != null)
        {
            _dialog.Hide();
        }

        _dialog = null;
    }

    private bool Use24HourView => (VirtualView != null) && (DateFormat.Is24HourFormat(PlatformView?.Context)
            && (VirtualView.Format == "t") || (VirtualView.Format == "HH:mm"));
}