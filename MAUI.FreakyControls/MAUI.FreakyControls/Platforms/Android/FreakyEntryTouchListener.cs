using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using MAUI.FreakyControls.Shared.Enums;
using static Android.Views.View;

namespace MAUI.FreakyControls.Platforms.Android
{
    internal class FreakyEntryTouchListener : Java.Lang.Object, IOnTouchListener
    {
        private readonly AppCompatEditText mEditText;
        private readonly FreakyEntry frentry;

        public FreakyEntryTouchListener(AppCompatEditText mEditText, FreakyEntry frentry)
        {
            this.mEditText = mEditText;
            this.frentry = frentry;
        }

        public bool OnTouch(global::Android.Views.View v, MotionEvent e)
        {
            int[] textLocation = new int[2];
            mEditText.GetLocationOnScreen(textLocation);

            if (e.Action == MotionEventActions.Up)
            {
                if (e.RawX >= textLocation[0] + mEditText.Width - mEditText.TotalPaddingRight)
                {
                    if (frentry.ImageCommand?.CanExecute(frentry.ImageCommandParameter) == true)
                    {
                        frentry.ImageCommand.Execute(frentry.ImageCommandParameter);
                    }
                    return true;
                }
                else if (e.RawX <= textLocation[0] + mEditText.TotalPaddingLeft)
                {
                    if (frentry.ImageCommand?.CanExecute(frentry.ImageCommandParameter) == true)
                    {
                        frentry.ImageCommand.Execute(frentry.ImageCommandParameter);
                    }
                    return true;
                }
                else
                {
                    frentry.Focus();
                }
            }
            return true;
        }
    }
}

