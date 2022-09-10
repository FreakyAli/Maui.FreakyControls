using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using Drawable2 = Android.Resource.Drawable;

namespace Maui.FreakyControls.Platforms.Android.NativeControls
{
    public class ClearableEditext : AppCompatEditText
    {
        Context mContext;
        Drawable imgX;

        public ClearableEditext(Context context, Drawable clearButtonDrawable = null) : base(context)
        {
            Init(context, null, clearButtonDrawable);
        }

        public ClearableEditext(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public ClearableEditext(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        public void Init(Context ctx, IAttributeSet attrs, Drawable clearButtonDrawable = null)
        {
            mContext = ctx;
            imgX = clearButtonDrawable ?? ContextCompat.GetDrawable(ctx, Drawable2.PresenceOffline);
            imgX.SetBounds(0, 0, imgX.IntrinsicWidth, imgX.IntrinsicHeight);
            manageClearButton();
            this.SetOnTouchListener(new TouchHelper(this, imgX));
            this.AddTextChangedListener(new TextListener(this));
        }

        public void manageClearButton()
        {
            if (string.IsNullOrWhiteSpace(Text))
                removeClearButton();
            else
                addClearButton();
        }
        public void addClearButton()
        {
            this.SetCompoundDrawables(this.GetCompoundDrawables()[0],
                    this.GetCompoundDrawables()[1],
                    imgX,
                    this.GetCompoundDrawables()[3]);
        }
        public void removeClearButton()
        {
            this.SetCompoundDrawables(this.GetCompoundDrawables()[0],
                    this.GetCompoundDrawables()[1],
                    null,
                    this.GetCompoundDrawables()[3]);
        }
    }
}

