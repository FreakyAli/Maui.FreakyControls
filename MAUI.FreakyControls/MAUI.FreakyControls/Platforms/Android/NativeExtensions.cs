using System.Collections;
using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using ArrayList = Java.Util.ArrayList;
using View = Android.Views.View;

namespace Maui.FreakyControls.Platforms.Android;

public static class NativeExtensions
{
    public static ArrayList ToArrayList(this ICollection input)
    {
        return input.Count == 0 ? new ArrayList() : new ArrayList(input);
    }

    public static System.Drawing.SizeF GetSize(this Bitmap image)
    {
        return new System.Drawing.SizeF(image.Width, image.Height);
    }

    public static System.Drawing.SizeF GetSize(this View view)
    {
        return new System.Drawing.SizeF(view.Width, view.Height);
    }

    public static float GetDensity(this Activity activity)
    {
#if ANDROID30_0_OR_GREATER
        var displayMetrics = activity.Resources.DisplayMetrics;
        var density = displayMetrics.Density;
        return density;
#else       
        DisplayMetrics displayMetrics = new DisplayMetrics();
        activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
        return displayMetrics.Density;
#endif
    }
}