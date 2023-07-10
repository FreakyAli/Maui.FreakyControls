using Android.App;
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

public partial class FreakyDatePickerHandler
{
    private DatePickerDialog? _dialog;

    protected override MauiDatePicker CreatePlatformView()
    {
        var mauiDatePicker = new FreakyMauiDatePicker(Context)
        {
            ShowPicker = ShowPickerDialog,
            HidePicker = HidePickerDialog
        };

        var date = VirtualView?.Date;

        if (date != null)
            _dialog = CreateDatePickerDialog(date.Value.Year, date.Value.Month, date.Value.Day);
        var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        ViewCompat.SetBackgroundTintList(mauiDatePicker, colorStateList);
        return mauiDatePicker;
    }

    protected override void DisconnectHandler(MauiDatePicker platformView)
    {
        base.DisconnectHandler(platformView);
        if (_dialog != null)
        {
            _dialog.Hide();
            _dialog.Dispose();
            _dialog = null;
        }
    }

    internal DatePickerDialog? DatePickerDialog
    { get { return _dialog; } }

    internal async Task HandleAndAlignImageSourceAsync(FreakyDatePicker entry)
    {
        var imageBitmap = await entry.ImageSource?.ToNativeImageSourceAsync();
        if (imageBitmap != null)
        {
            var bitmapDrawable = new BitmapDrawable(CurrentActivity?.Resources,
                Bitmap.CreateScaledBitmap(imageBitmap, entry.ImageWidth * 2, entry.ImageHeight * 2, true));
            var freakyEditText = PlatformView as FreakyMauiDatePicker;
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

        if (_dialog != null && _dialog.IsShowing)
            return;

        var date = VirtualView.Date;
        ShowPickerDialog(date.Year, date.Month - 1, date.Day);
    }

    private void ShowPickerDialog(int year, int month, int day)
    {
        if (_dialog == null)
            _dialog = CreateDatePickerDialog(year, month, day);
        else
        {
            EventHandler? setDateLater = null;
            setDateLater = (sender, e) => { _dialog!.UpdateDate(year, month, day); _dialog.ShowEvent -= setDateLater; };
            _dialog.ShowEvent += setDateLater;
        }

        _dialog.Show();
    }

    private void HidePickerDialog()
    {
        _dialog?.Hide();
    }
}