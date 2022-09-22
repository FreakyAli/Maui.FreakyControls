using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using static Android.Views.WindowInsetsAnimation;
using Rect = Android.Graphics.Rect;

namespace Maui.FreakyControls.Platforms.Android.NativeControls
{
    public class FreakyAutoCompeleView : AppCompatAutoCompleteTextView
    {
        private Drawable drawableRight;
        private Drawable drawableLeft;
        private Drawable drawableTop;
        private Drawable drawableBottom;

        int actionX, actionY;

        private IDrawableClickListener clickListener;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public event EventHandler? SelectionChanged;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.


        public FreakyAutoCompeleView(Context context) : base(context)
        {

        }

        public FreakyAutoCompeleView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public FreakyAutoCompeleView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }


        public override void SetCompoundDrawablesWithIntrinsicBounds(Drawable left, Drawable top,
                Drawable right, Drawable bottom)
        {
            if (left != null)
            {
                drawableLeft = left;
            }
            if (right != null)
            {
                drawableRight = right;
            }
            if (top != null)
            {
                drawableTop = top;
            }
            if (bottom != null)
            {
                drawableBottom = bottom;
            }
            base.SetCompoundDrawablesWithIntrinsicBounds(left, top, right, bottom);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            Rect bounds;
            if (e.Action == MotionEventActions.Down)
            {
                actionX = (int)e.GetX();
                actionY = (int)e.GetY();
                if (drawableBottom != null
                    && drawableBottom.Bounds.Contains(actionX, actionY))
                {
                    clickListener.OnClick(DrawablePosition.Bottom);
                    return base.OnTouchEvent(e);
                }

                if (drawableTop != null
                        && drawableTop.Bounds.Contains(actionX, actionY))
                {
                    clickListener.OnClick(DrawablePosition.Top);
                    return base.OnTouchEvent(e);
                }

                // this works for left since container shares 0,0 origin with bounds
                if (drawableLeft != null)
                {
                    bounds = null;
                    bounds = drawableLeft.Bounds;

                    int x, y;
                    int extraTapArea = (int)(13 * Resources.DisplayMetrics.Density + 0.5);

                    x = actionX;
                    y = actionY;

                    if (!bounds.Contains(actionX, actionY))
                    {
                        // Gives the +20 area for tapping. /
                        x = (int)(actionX - extraTapArea);
                        y = (int)(actionY - extraTapArea);

                        if (x <= 0)
                            x = actionX;
                        if (y <= 0)
                            y = actionY;

                        // Creates square from the smallest value /
                        if (x < y)
                        {
                            y = x;
                        }
                    }

                    if (bounds.Contains(x, y) && clickListener != null)
                    {
                        clickListener.OnClick(DrawablePosition.Left);
                        e.Action = (MotionEventActions.Cancel);
                        return false;

                    }
                }

                if (drawableRight != null)
                {

                    bounds = null;
                    bounds = drawableRight.Bounds;

                    int x, y;
                    int extraTapArea = 13;

                    //
                    //  IF USER CLICKS JUST OUT SIDE THE RECTANGLE OF THE DRAWABLE
                    //  THAN ADD X AND SUBTRACT THE Y WITH SOME VALUE SO THAT AFTER
                    //  CALCULATING X AND Y CO-ORDINATE LIES INTO THE DRAWBABLE
                    //  BOUND. - this process help to increase the tappable area of
                    //  the rectangle.
                    // 
                    x = (int)(actionX + extraTapArea);
                    y = (int)(actionY - extraTapArea);

                    //Since this is right drawable subtract the value of x from the width 
                    // of view. so that width - tappedarea will result in x co-ordinate in drawable bound. 
                    //
                    x = Width - x;

                    //x can be negative if user taps at x co-ordinate just near the width.
                    // e.g views width = 300 and user taps 290. Then as per previous calculation
                    // 290 + 13 = 303. So subtract X from getWidth() will result in negative value.
                    // So to avoid this add the value previous added when x goes negative.
                    //

                    if (x <= 0)
                    {
                        x += extraTapArea;
                    }

                    // If result after calculating for extra tappable area is negative.
                    // assign the original value so that after subtracting
                    // extratapping area value doesn't go into negative value.
                    //

                    if (y <= 0)
                        y = actionY;

                    //If drawble bounds contains the x and y points then move ahead./
                    if (bounds.Contains(x, y) && clickListener != null)
                    {
                        clickListener
                                .OnClick(DrawablePosition.Right);
                        e.Action = (MotionEventActions.Cancel);
                        return false;
                    }
                    return base.OnTouchEvent(e);
                }

            }
            return base.OnTouchEvent(e);
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

        protected override void OnSelectionChanged(int selStart, int selEnd)
        {
            base.OnSelectionChanged(selStart, selEnd);

            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}


