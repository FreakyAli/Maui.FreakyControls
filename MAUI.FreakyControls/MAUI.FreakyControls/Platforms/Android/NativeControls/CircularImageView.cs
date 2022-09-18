using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using AndroidX.AppCompat.Widget;
using Paint = Android.Graphics.Paint;
using Rect = Android.Graphics.Rect;
using Color = Android.Graphics.Color;
using Android.Util;
using Microsoft.Maui.Graphics;

namespace Maui.FreakyControls.Platforms.Android.NativeControls
{
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
            Drawable drawable = this.Drawable;

            if (drawable == null)
            {
                return;
            }

            if (Width == 0 || Height == 0)
            {
                return;
            }
            Bitmap b = ((BitmapDrawable)drawable).Bitmap;
            Bitmap bitmap = b.Copy(Bitmap.Config.Argb8888, true);

            int w = Width, h = Height;

            Bitmap roundBitmap = GetRoundedCroppedBitmap(bitmap, w);
            canvas.DrawBitmap(roundBitmap, 0, 0, null);
        }

        public static Bitmap GetRoundedCroppedBitmap(Bitmap bmp, int radius)
        {
            Bitmap sbmp;

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

            Bitmap output = Bitmap.CreateBitmap(radius, radius, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(output);

            Paint paint = new Paint();
            Rect rect = new Rect(0, 0, radius, radius);

            paint.AntiAlias=(true);
            paint.FilterBitmap=(true);
            paint.Dither=(true);
            canvas.DrawARGB(0, 0, 0, 0);
            //paint.SetColor(Color.ParseColor("#BAB399"));
            canvas.DrawCircle(radius / 2 + 0.7f,
                    radius / 2 + 0.7f, radius / 2 + 0.1f, paint);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(sbmp, rect, rect, paint);

            return output;
        }

    }
}


