using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using AndroidX.AppCompat.Widget;
using Paint = Android.Graphics.Paint;
using Rect = Android.Graphics.Rect;

namespace Maui.FreakyControls.Platforms.Android.NativeControls;

public class CircularImageView : AppCompatImageView
{
    public CircularImageView(Context context) : base(context)
    {
    }

    public CircularImageView(Context context, IAttributeSet attrs) : base(context, attrs)
    {
    }

    public CircularImageView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
    {
    }

    protected override void OnDraw(Canvas canvas)
    {
        Drawable? drawable = this.Drawable;

        if (drawable is null)
        {
            return;
        }

        if (Width == 0 || Height == 0)
        {
            return;
        }
        Bitmap? b = ((BitmapDrawable)drawable).Bitmap;
        if (b is null)
            return;
        var argb8888 = Bitmap.Config.Argb8888;
        if (argb8888 is null)
            return;
        Bitmap? bitmap = b.Copy(argb8888, true);
        if (bitmap is null)
            return;

        int w = Width, h = Height;

        Bitmap? roundBitmap = GetRoundedCroppedBitmap(bitmap, w);
        if (roundBitmap is not null)
            canvas.DrawBitmap(roundBitmap, 0, 0, null);
    }

    public static Bitmap? GetRoundedCroppedBitmap(Bitmap bmp, int radius)
    {
        Bitmap? sbmp;

        if (bmp.Width != radius || bmp.Height != radius)
        {
            float smallest = Math.Min(bmp.Width, bmp.Height);
            float factor = smallest / radius;
            sbmp = Bitmap.CreateScaledBitmap(bmp, (int)(bmp.Width / factor), (int)(bmp.Height / factor), false);
        }
        else
        {
            sbmp = bmp;
        }

        if (sbmp is null)
            return null;

        var config = Bitmap.Config.Argb8888;
        if (config is null)
            return null;
        Bitmap? output = Bitmap.CreateBitmap(radius, radius, config);
        if (output is null)
            return null;
        Canvas canvas = new Canvas(output);

        Paint paint = new Paint();
        Rect rect = new Rect(0, 0, radius, radius);

        paint.AntiAlias = (true);
        paint.FilterBitmap = (true);
        paint.Dither = (true);
        canvas.DrawARGB(0, 0, 0, 0);
        canvas.DrawCircle(((radius / 2) + 0.7f),
                ((radius / 2) + 0.7f), ((radius / 2) + 0.1f), paint);
        paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
        canvas.DrawBitmap(sbmp, rect, rect, paint);
        return output;
    }
}