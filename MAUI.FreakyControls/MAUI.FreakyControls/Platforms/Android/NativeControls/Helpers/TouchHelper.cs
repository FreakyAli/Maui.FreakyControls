using Android.Graphics.Drawables;
using Android.Views;
using View = Android.Views.View;

namespace Maui.FreakyControls.Platforms.Android.NativeControls;

internal class TouchHelper : Java.Lang.Object, View.IOnTouchListener
{
    ClearableEditext Editext;
    public ClearableEditext objClearable { get; set; }
    Drawable imgX;
    public TouchHelper(ClearableEditext editext, Drawable imgx)
    {
        Editext = editext;
        objClearable = objClearable;
        imgX = imgx;
    }
    public bool OnTouch(View v, MotionEvent e)
    {
        ClearableEditext et = Editext;

        if (et.GetCompoundDrawables()[2] == null)
            return false;
        // Only do this for up touches
        if (e.Action != MotionEventActions.Up)
            return false;
        // Is touch on our clear button?
        if (e.GetX() > et.Width - et.PaddingRight - imgX.IntrinsicWidth)
        {
            Editext.Text = string.Empty;
            if (objClearable != null)
                objClearable.removeClearButton();

        }
        return false;
    }
}