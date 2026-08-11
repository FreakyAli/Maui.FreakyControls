using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Microsoft.Maui.Platform;
using Rect = Android.Graphics.Rect;

namespace Maui.FreakyControls.Platforms.Android.NativeControls
{
    public class FreakyMauiPicker : MauiPicker
    {
        private Drawable? drawableRight;
        private Drawable? drawableLeft;
        private Drawable? drawableTop;
        private Drawable? drawableBottom;

        private int actionX, actionY;

        private IDrawableClickListener? clickListener;

        public FreakyMauiPicker(Context context) : base(context)
        {
        }

        public override void SetCompoundDrawablesWithIntrinsicBounds(Drawable? left, Drawable? top,
               Drawable? right, Drawable? bottom)
        {
            drawableLeft = left;
            drawableRight = right;
            drawableTop = top;
            drawableBottom = bottom;
            base.SetCompoundDrawablesWithIntrinsicBounds(left, top, right, bottom);
        }

        public override bool OnTouchEvent(MotionEvent? e)
        {
            if (e is null) return base.OnTouchEvent(e);

            if (e.Action != MotionEventActions.Down)
                return base.OnTouchEvent(e);

            actionX = (int)e.GetX();
            actionY = (int)e.GetY();

            if (TryHandleSimpleDrawableClick())
                return base.OnTouchEvent(e);

            if (TryHandleLeftDrawableClick())
                return false;

            if (TryHandleRightDrawableClick())
                return false;

            return base.OnTouchEvent(e);
        }

        private bool TryHandleSimpleDrawableClick()
        {
            if (drawableBottom is not null && drawableBottom.Bounds.Contains(actionX, actionY))
            {
                clickListener?.OnClick(DrawablePosition.Bottom);
                return true;
            }

            if (drawableTop is not null && drawableTop.Bounds.Contains(actionX, actionY))
            {
                clickListener?.OnClick(DrawablePosition.Top);
                return true;
            }

            return false;
        }

        private bool TryHandleLeftDrawableClick()
        {
            if (drawableLeft is null)
                return false;

            var bounds = drawableLeft.Bounds;
            int extraTapArea = (int)((13 * (Resources?.DisplayMetrics?.Density ?? 1f)) + 0.5);
            var (x, y) = AdjustCoordinatesForLeftDrawable(actionX, actionY, bounds, extraTapArea);

            if (bounds.Contains(x, y) && clickListener is not null)
            {
                clickListener.OnClick(DrawablePosition.Left);
                return true;
            }

            return false;
        }

        private bool TryHandleRightDrawableClick()
        {
            if (drawableRight is null)
                return false;

            var bounds = drawableRight.Bounds;
            const int extraTapArea = 13;
            var (x, y) = AdjustCoordinatesForRightDrawable(actionX, actionY, bounds, extraTapArea);

            if (bounds.Contains(x, y) && clickListener is not null)
            {
                clickListener.OnClick(DrawablePosition.Right);
                return true;
            }

            return false;
        }

        private (int, int) AdjustCoordinatesForLeftDrawable(int actionX, int actionY, Rect bounds, int extraTapArea)
        {
            int x = actionX;
            int y = actionY;

            if (!bounds.Contains(actionX, actionY))
            {
                x = (int)(actionX - extraTapArea);
                y = (int)(actionY - extraTapArea);

                if (x <= 0) x = actionX;
                if (y <= 0) y = actionY;

                if (x < y)
                    y = x;
            }

            return (x, y);
        }

        private (int, int) AdjustCoordinatesForRightDrawable(int actionX, int actionY, Rect bounds, int extraTapArea)
        {
            int x = (int)(actionX + extraTapArea);
            int y = (int)(actionY - extraTapArea);

            x = Width - x;

            if (x <= 0)
                x += extraTapArea;

            if (y <= 0)
                y = actionY;

            return (x, y);
        }

        protected override void JavaFinalize()
        {
            drawableRight = null;
            drawableBottom = null;
            drawableLeft = null;
            drawableTop = null;
            base.JavaFinalize();
        }

        public void SetDrawableClickListener(IDrawableClickListener listener)
        {
            this.clickListener = listener;
        }
    }
}