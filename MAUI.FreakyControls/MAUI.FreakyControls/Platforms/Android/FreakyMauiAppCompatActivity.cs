using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.Activity;
using View = Android.Views.View;

namespace Maui.FreakyControls.Platforms.Android
{
    public abstract class FreakyMauiAppCompatActivity: MauiAppCompatActivity
    {
        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            try
            {
                View view = CurrentFocus;
                if (view != null && (ev.Action == MotionEventActions.Up || ev.Action == MotionEventActions.Move) &&
                    view is EditText &&
                    !view.Class.Name.StartsWith("android.webkit."))
                {
                    int[] Touch = new int[2];
                    view.GetLocationOnScreen(Touch);
                    float x = ev.RawX + view.Left - Touch[0];
                    float y = ev.RawY + view.Top - Touch[1];
                    if (x < view.Left || x > view.Right || y < view.Top || y > view.Bottom)
                        ((InputMethodManager)GetSystemService(InputMethodService)).
                            HideSoftInputFromWindow((Window.DecorView.ApplicationWindowToken), 0);
                }
            }
            catch
            {

            }

            return base.DispatchTouchEvent(ev);
        }
    }
}

