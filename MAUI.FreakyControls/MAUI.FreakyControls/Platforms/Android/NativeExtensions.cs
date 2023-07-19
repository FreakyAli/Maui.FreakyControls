using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Color = Android.Graphics.Color;
using System.Collections;
using ArrayList = Java.Util.ArrayList;
using View = Android.Views.View;
using AndroidX.Core.Graphics;

namespace Maui.FreakyControls.Platforms.Android;

public static class NativeExtensions
{
    public static void SetColorFilter(this Drawable drawable, Color color, ColorFilterMode mode)
    {
#if ANDROID29_0_OR_GREATER
            var blendModeCompat = GetBlendFilterMode(mode);
            drawable.SetColorFilter(
                        BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(
                        color,
                        blendModeCompat
                        ));
#else
            var porterMode= GetPorterFilterMode(mode);
            drawable.SetColorFilter(color, PorterDuff.Mode.SrcIn, porterMode);
#endif
    }

    private static BlendModeCompat? GetBlendFilterMode(ColorFilterMode mode)
    {
        var modeCompat = mode switch
        {
            ColorFilterMode.Src => BlendModeCompat.Src,
            ColorFilterMode.Dst => BlendModeCompat.Dst,
            ColorFilterMode.SrcOver => BlendModeCompat.SrcOver,
            ColorFilterMode.DstOver => BlendModeCompat.DstOver,
            ColorFilterMode.SrcIn => BlendModeCompat.SrcIn,
            ColorFilterMode.DstIn => BlendModeCompat.DstIn,
            ColorFilterMode.SrcOut => BlendModeCompat.SrcOut,
            ColorFilterMode.DstOut => BlendModeCompat.DstOut,
            ColorFilterMode.SrcAtop => BlendModeCompat.SrcAtop,
            ColorFilterMode.DstAtop => BlendModeCompat.DstAtop,
            ColorFilterMode.Xor => BlendModeCompat.Xor,
            ColorFilterMode.Darken => BlendModeCompat.Darken,
            ColorFilterMode.Lighten => BlendModeCompat.Lighten,
            ColorFilterMode.Multiply => BlendModeCompat.Multiply,
            ColorFilterMode.Screen => BlendModeCompat.Screen,
            ColorFilterMode.Add => BlendModeCompat.Plus,
            ColorFilterMode.Overlay => BlendModeCompat.Overlay,
            _ => BlendModeCompat.Clear,
        };
        return modeCompat;
    }

    private static PorterDuff.Mode GetPorterFilterMode(ColorFilterMode mode)
    {
        var modeCompat = mode switch
        {
            ColorFilterMode.Src => PorterDuff.Mode.Src,
            ColorFilterMode.Dst => PorterDuff.Mode.Dst,
            ColorFilterMode.SrcOver => PorterDuff.Mode.SrcOver,
            ColorFilterMode.DstOver => PorterDuff.Mode.DstOver,
            ColorFilterMode.SrcIn => PorterDuff.Mode.SrcIn,
            ColorFilterMode.DstIn => PorterDuff.Mode.DstIn,
            ColorFilterMode.SrcOut => PorterDuff.Mode.SrcOut,
            ColorFilterMode.DstOut => PorterDuff.Mode.DstOut,
            ColorFilterMode.SrcAtop => PorterDuff.Mode.SrcAtop,
            ColorFilterMode.DstAtop => PorterDuff.Mode.DstAtop,
            ColorFilterMode.Xor => PorterDuff.Mode.Xor,
            ColorFilterMode.Darken => PorterDuff.Mode.Darken,
            ColorFilterMode.Lighten => PorterDuff.Mode.Lighten,
            ColorFilterMode.Multiply => PorterDuff.Mode.Multiply,
            ColorFilterMode.Screen => PorterDuff.Mode.Screen,
            ColorFilterMode.Add => PorterDuff.Mode.Add,
            ColorFilterMode.Overlay => PorterDuff.Mode.Overlay,
            _ => PorterDuff.Mode.Clear,
        };
        return modeCompat;
    }

    public static float DpToPixels(this Context context, float valueInDp)
    {
        DisplayMetrics metrics = context.Resources.DisplayMetrics;
        return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
    }

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