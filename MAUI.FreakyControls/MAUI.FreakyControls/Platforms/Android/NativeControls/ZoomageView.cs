using Android.Animation;
using Android.Content;
using Android.Graphics;
using RectF = Android.Graphics.RectF;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using static Android.Views.ScaleGestureDetector;
using PointF = Android.Graphics.PointF;
using AndroidX.Core.View;
using Java.Lang;
using static Android.Animation.ValueAnimator;
using static Android.Animation.Animator;
using Drawable = Android.Graphics.Drawables.Drawable;
using static Android.Views.GestureDetector;

namespace Maui.FreakyControls.Platforms.Android.NativeControls;

public class AutoResetMode
{
    internal const int UNDER = 0;
    internal const int OVER = 1;
    internal const int ALWAYS = 2;
    internal const int NEVER = 3;

    internal static int FromInt(int value)
    {
        return value switch
        {
            OVER => OVER,
            ALWAYS => ALWAYS,
            NEVER => NEVER,
            _ => UNDER,
        };
    }
}

public class GestureListener : SimpleOnGestureListener
{
    private bool singleTapDetected;
    private bool doubleTapDetected;

    public GestureListener(bool doubleTapDetected, bool singleTapDetected)
    {
        this.doubleTapDetected = doubleTapDetected;
        this.singleTapDetected = singleTapDetected;
    }

    public override bool OnDoubleTapEvent(MotionEvent e)
    {
        if (e.Action == MotionEventActions.Up)
        {
            doubleTapDetected = true;
        }

        singleTapDetected = false;

        return false;
    }

    public override bool OnSingleTapUp(MotionEvent e)
    {
        singleTapDetected = false;
        return false;
    }

    public override bool OnSingleTapConfirmed(MotionEvent e)
    {
        return base.OnSingleTapConfirmed(e);
    }

    public override bool OnDown(MotionEvent e)
    {
        return true;
    }
}

public class ZoomageView : AppCompatImageView, IOnScaleGestureListener
{
    private static float MIN_SCALE = 0.6f;
    private static float MAX_SCALE = 8f;
    private int RESET_DURATION = 200;

    private ScaleType startScaleType;

    // These matrices will be used to move and zoom image
    private Matrix matrix = new Matrix();
    private Matrix startMatrix = new Matrix();

    private float[] matrixValues = new float[9];
    private float[] startValues = null;

    private float minScale = MIN_SCALE;
    private float maxScale = MAX_SCALE;

    //the adjusted scale bounds that account for an image's starting scale values
    private float calculatedMinScale = MIN_SCALE;
    private float calculatedMaxScale = MAX_SCALE;

    private RectF bounds = new RectF();

    private bool translatable;
    private bool zoomable;
    private bool doubleTapToZoom;
    private bool restrictBounds;
    private bool animateOnReset;
    private bool autoCenter;
    private float doubleTapToZoomScaleFactor;
    private int autoResetMode;

    private PointF last = new PointF(0, 0);
    private float startScale = 1f;
    private float scaleBy = 1f;
    private float currentScaleFactor = 1f;
    private int previousPointerCount = 1;
    private int currentPointerCount = 0;

    private ScaleGestureDetector scaleDetector;
    private ValueAnimator resetAnimator;

    private GestureDetector gestureDetector;
    private bool doubleTapDetected = false;
    private bool singleTapDetected = false;

    private GestureListener gestureListener;

    public ZoomageView(Context context) : base(context)
    {
        gestureListener = new GestureListener(doubleTapDetected, singleTapDetected);
        Init(context, null);
    }

    public ZoomageView(Context context, IAttributeSet attrs) : base(context, attrs)
    {
        gestureListener = new GestureListener(doubleTapDetected, singleTapDetected);
        Init(context, attrs);
    }

    public ZoomageView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
    {
        gestureListener = new GestureListener(doubleTapDetected, singleTapDetected);
        Init(context, attrs);
    }

    private void Init(Context context, IAttributeSet attrs)
    {
        scaleDetector = new ScaleGestureDetector(context, this);
        gestureDetector = new GestureDetector(context, gestureListener);
        ScaleGestureDetectorCompat.SetQuickScaleEnabled(scaleDetector, false);
        startScaleType = GetScaleType();


        zoomable = true;
        translatable = true;
        animateOnReset = true;
        autoCenter = true;
        restrictBounds = false;
        doubleTapToZoom = true;
        minScale = MIN_SCALE;
        maxScale = MAX_SCALE;
        doubleTapToZoomScaleFactor = 3;
        autoResetMode = AutoResetMode.UNDER;

        VerifyScaleRange();
    }

    private void VerifyScaleRange()
    {
        if (minScale >= maxScale)
        {
            throw new IllegalStateException("minScale must be less than maxScale");
        }

        if (minScale < 0)
        {
            throw new IllegalStateException("minScale must be greater than 0");
        }

        if (maxScale < 0)
        {
            throw new IllegalStateException("maxScale must be greater than 0");
        }

        if (doubleTapToZoomScaleFactor > maxScale)
        {
            doubleTapToZoomScaleFactor = maxScale;
        }

        if (doubleTapToZoomScaleFactor < minScale)
        {
            doubleTapToZoomScaleFactor = minScale;
        }
    }

    public void setScaleRange(float minScale, float maxScale)
    {
        this.minScale = minScale;
        this.maxScale = maxScale;

        startValues = null;

        VerifyScaleRange();
    }

    public bool IsTranslatable()
    {
        return translatable;
    }

    public void SetTranslatable(bool translatable)
    {
        this.translatable = translatable;
    }

    public void SetZoomable(bool zoomable)
    {
        this.zoomable = zoomable;
    }

    public bool GetRestrictBounds()
    {
        return restrictBounds;
    }

    public void SetRestrictBounds(bool restrictBounds)
    {
        this.restrictBounds = restrictBounds;
    }

    public bool GetAnimateOnReset()
    {
        return animateOnReset;
    }

    public void SetAnimateOnReset(bool animateOnReset)
    {
        this.animateOnReset = animateOnReset;
    }

    public int GetAutoResetMode()
    {
        return autoResetMode;
    }

    public void SetAutoResetMode(int autoReset)
    {
        this.autoResetMode = autoReset;
    }

    public bool GetAutoCenter()
    {
        return autoCenter;
    }

    public void setAutoCenter(bool autoCenter)
    {
        this.autoCenter = autoCenter;
    }

    public bool getDoubleTapToZoom()
    {
        return doubleTapToZoom;
    }

    public void setDoubleTapToZoom(bool doubleTapToZoom)
    {
        this.doubleTapToZoom = doubleTapToZoom;
    }

    public float getDoubleTapToZoomScaleFactor()
    {
        return doubleTapToZoomScaleFactor;
    }

    public void setDoubleTapToZoomScaleFactor(float doubleTapToZoomScaleFactor)
    {
        this.doubleTapToZoomScaleFactor = doubleTapToZoomScaleFactor;
        VerifyScaleRange();
    }

    public float getCurrentScaleFactor()
    {
        return currentScaleFactor;
    }

    public override bool Enabled
    {
        get => base.Enabled;
        set
        {
            base.Enabled = value;
            if (!Enabled)
            {
                SetScaleType(startScaleType);
            }
        }
    }

    public override void SetImageResource(int resId)
    {
        base.SetImageResource(resId);
        SetScaleType(startScaleType);
    }

    public override void SetImageDrawable(Drawable drawable)
    {
        base.SetImageDrawable(drawable);
        SetScaleType(startScaleType);
    }

    public override void SetImageBitmap(Bitmap bm)
    {
        base.SetImageBitmap(bm);
        SetScaleType(startScaleType);
    }

    public override void SetImageURI(global::Android.Net.Uri uri)
    {
        base.SetImageURI(uri);
        SetScaleType(startScaleType);
    }

    private void UpdateBounds(float[] values)
    {
        if (Drawable != null)
        {
            bounds.Set(values[Matrix.MtransX],
                    values[Matrix.MtransY],
                    Drawable.IntrinsicWidth * values[Matrix.MscaleX] + values[Matrix.MtransX],
                    Drawable.IntrinsicHeight * values[Matrix.MscaleY] + values[Matrix.MtransY]);
        }
    }

    private float GetCurrentDisplayedWidth()
    {
        if (Drawable != null)
            return Drawable.IntrinsicWidth * matrixValues[Matrix.MscaleX];
        else
            return 0;
    }

    private float GetCurrentDisplayedHeight()
    {
        if (Drawable != null)
            return Drawable.IntrinsicHeight * matrixValues[Matrix.MscaleY];
        else
            return 0;
    }

    public override void SetScaleType(ScaleType scaleType)
    {
        if (scaleType != null)
        {
            base.SetScaleType(scaleType);
            startScaleType = scaleType;
            startValues = null;
        }
    }

    private void SetStartValues()
    {
        startValues = new float[9];
        startMatrix = new Matrix(ImageMatrix);
        startMatrix.GetValues(startValues);
        calculatedMinScale = minScale * startValues[Matrix.MscaleX];
        calculatedMaxScale = maxScale * startValues[Matrix.MscaleX];
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
        if (!Clickable && Enabled && (zoomable || translatable))
        {
            if (GetScaleType() != ScaleType.Matrix)
            {
                base.SetScaleType(ScaleType.Matrix);
            }

            if (startValues == null)
            {
                SetStartValues();
            }

            currentPointerCount = e.PointerCount;

            //get the current state of the image matrix, its values, and the bounds of the drawn bitmap
            matrix.Set(ImageMatrix);
            matrix.GetValues(matrixValues);
            UpdateBounds(matrixValues);

            scaleDetector.OnTouchEvent(e);
            gestureDetector.OnTouchEvent(e);

            if (doubleTapToZoom && doubleTapDetected)
            {
                doubleTapDetected = false;
                singleTapDetected = false;
                if (matrixValues[Matrix.MscaleX] != startValues[Matrix.MscaleX])
                {
                    reset();
                }
                else
                {
                    Matrix zoomMatrix = new Matrix(matrix);
                    zoomMatrix.PostScale(doubleTapToZoomScaleFactor, doubleTapToZoomScaleFactor, scaleDetector.FocusX, scaleDetector.FocusY);
                    animateScaleAndTranslationToMatrix(zoomMatrix, RESET_DURATION);
                }
                return true;
            }
            else if (!singleTapDetected)
            {
                /* if the e is a down touch, or if the number of touch points changed,
                 * we should reset our start point, as e origins have likely shifted to a
                 * different part of the screen*/
                if (e.ActionMasked == MotionEventActions.Down ||
                        currentPointerCount != previousPointerCount)
                {
                    last.Set(scaleDetector.FocusX, scaleDetector.FocusY);
                }
                else if (e.ActionMasked == MotionEventActions.Move)
                {

                    float focusx = scaleDetector.FocusX;
                    float focusy = scaleDetector.FocusY;

                    if (allowTranslate(e))
                    {
                        //calculate the distance for translation
                        float xdistance = getXDistance(focusx, last.X);
                        float ydistance = getYDistance(focusy, last.Y);
                        matrix.PostTranslate(xdistance, ydistance);
                    }

                    if (allowZoom(e))
                    {
                        matrix.PostScale(scaleBy, scaleBy, focusx, focusy);
                        currentScaleFactor = matrixValues[Matrix.MscaleX] / startValues[Matrix.MscaleX];
                    }

                    ImageMatrix = matrix;

                    last.Set(focusx, focusy);
                }

                if (e.ActionMasked == MotionEventActions.Up ||
                                e.ActionMasked == MotionEventActions.Cancel)
                {
                    scaleBy = 1f;
                    resetImage();
                }
            }

            Parent.RequestDisallowInterceptTouchEvent(disallowParentTouch(e));

            //this tracks whether they have changed the number of fingers down
            previousPointerCount = currentPointerCount;


            return true;
        }
        return base.OnTouchEvent(e);
    }

    protected bool disallowParentTouch(MotionEvent e)
    {
        if ((currentPointerCount > 1 || currentScaleFactor > 1.0f || isAnimating()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected bool allowTranslate(MotionEvent e)
    {
        return translatable && currentScaleFactor > 1.0f;
    }

    protected bool allowZoom(MotionEvent e)
    {
        return zoomable;
    }

    private bool isAnimating()
    {
        return resetAnimator != null && resetAnimator.IsRunning;
    }


    public bool OnScale(ScaleGestureDetector detector)
    {
        //calculate value we should scale by, ultimately the scale will be startScale*scaleFactor
        scaleBy = (startScale * detector.ScaleFactor) / matrixValues[Matrix.MscaleX];

        //what the scaling should end up at after the transformation
        float projectedScale = scaleBy * matrixValues[Matrix.MscaleX];

        //clamp to the min/max if it's going over
        if (projectedScale < calculatedMinScale)
        {
            scaleBy = calculatedMinScale / matrixValues[Matrix.MscaleX];
        }
        else if (projectedScale > calculatedMaxScale)
        {
            scaleBy = calculatedMaxScale / matrixValues[Matrix.MscaleX];
        }

        return false;
    }

    public bool OnScaleBegin(ScaleGestureDetector detector)
    {
        startScale = matrixValues[Matrix.MscaleX];
        return true;
    }

    public void OnScaleEnd(ScaleGestureDetector detector)
    {
        scaleBy = 1f;
    }

    private void resetImage()
    {
        switch (autoResetMode)
        {
            case AutoResetMode.UNDER:
                if (matrixValues[Matrix.MscaleX] <= startValues[Matrix.MscaleX])
                {
                    reset();
                }
                else
                {
                    center();
                }
                break;
            case AutoResetMode.OVER:
                if (matrixValues[Matrix.MscaleX] >= startValues[Matrix.MscaleX])
                {
                    reset();
                }
                else
                {
                    center();
                }
                break;
            case AutoResetMode.ALWAYS:
                reset();
                break;
            case AutoResetMode.NEVER:
                center();
                break;
        }
    }

    private void center()
    {
        if (autoCenter)
        {
            animateTranslationX();
            animateTranslationY();
        }
    }

    public void reset()
    {
        reset(animateOnReset);
    }

    public void reset(bool animate)
    {
        if (animate)
        {
            animateToStartMatrix();
        }
        else
        {

            ImageMatrix = (startMatrix);
        }
    }

    private void animateToStartMatrix()
    {
        animateScaleAndTranslationToMatrix(startMatrix, RESET_DURATION);
    }

    private void animateTranslationX()
    {
        if (GetCurrentDisplayedWidth() > Width)
        {
            //the left edge is too far to the interior
            if (bounds.Left > 0)
            {
                animateMatrixIndex(Matrix.MtransX, 0);
            }
            //the right edge is too far to the interior
            else if (bounds.Right < Width)
            {
                animateMatrixIndex(Matrix.MtransX, bounds.Left + Width - bounds.Right);
            }
        }
        else
        {
            //left edge needs to be pulled in, and should be considered before the right edge
            if (bounds.Left < 0)
            {
                animateMatrixIndex(Matrix.MtransX, 0);
            }
            //right edge needs to be pulled in
            else if (bounds.Right > Width)
            {
                animateMatrixIndex(Matrix.MtransX, bounds.Left + Width - bounds.Right);
            }
        }
    }

    private void animateTranslationY()
    {
        if (GetCurrentDisplayedHeight() > Height)
        {
            //the top edge is too far to the interior
            if (bounds.Top > 0)
            {
                animateMatrixIndex(Matrix.MtransY, 0);
            }
            //the bottom edge is too far to the interior
            else if (bounds.Bottom < Height)
            {
                animateMatrixIndex(Matrix.MtransY, bounds.Top + Height - bounds.Bottom);
            }
        }
        else
        {
            //top needs to be pulled in, and needs to be considered before the bottom edge
            if (bounds.Top < 0)
            {
                animateMatrixIndex(Matrix.MtransY, 0);
            }
            //bottom edge needs to be pulled in
            else if (bounds.Bottom > Height)
            {
                animateMatrixIndex(Matrix.MtransY, bounds.Top + Height - bounds.Bottom);
            }
        }
    }

    private void animateMatrixIndex(int index, float to)
    {
        ValueAnimator animator = ValueAnimator.OfFloat(matrixValues[index], to);
        animator.AddUpdateListener(new AnimationUpdateListener2(ImageMatrix, index));
        animator.SetDuration(RESET_DURATION);
        animator.Start();
    }

    private void animateScaleAndTranslationToMatrix(Matrix targetMatrix, int duration)
    {

        float[] targetValues = new float[9];
        targetMatrix.GetValues(targetValues);

        var beginMatrix = new Matrix(ImageMatrix);
        beginMatrix.GetValues(matrixValues);

        //difference in current and original values
        float xsdiff = targetValues[Matrix.MscaleX] - matrixValues[Matrix.MscaleX];
        float ysdiff = targetValues[Matrix.MscaleY] - matrixValues[Matrix.MscaleY];
        float xtdiff = targetValues[Matrix.MtransX] - matrixValues[Matrix.MtransX];
        float ytdiff = targetValues[Matrix.MtransY] - matrixValues[Matrix.MtransY];

        resetAnimator = ValueAnimator.OfFloat(0, 1f);
        resetAnimator.AddUpdateListener(new AnimationUpdateListener(ImageMatrix, beginMatrix, xsdiff, ysdiff, xtdiff, ytdiff));
        resetAnimator.AddListener(new SimpleAnimationListener(targetMatrix, this.ImageMatrix));
        resetAnimator.SetDuration(duration);
        resetAnimator.Start();
    }

    private float getXDistance(float toX, float fromX)
    {
        float xdistance = toX - fromX;

        if (restrictBounds)
        {
            xdistance = getRestrictedXDistance(xdistance);
        }

        //prevents image from translating an infinite distance offscreen
        if (bounds.Right + xdistance < 0)
        {
            xdistance = -bounds.Right;
        }
        else if (bounds.Left + xdistance > Width)
        {
            xdistance = Width - bounds.Left;
        }

        return xdistance;
    }

    private float getRestrictedXDistance(float xdistance)
    {
        float restrictedXDistance = xdistance;

        if (GetCurrentDisplayedWidth() >= Width)
        {
            if (bounds.Left <= 0 && bounds.Left + xdistance > 0 && !scaleDetector.IsInProgress)
            {
                restrictedXDistance = -bounds.Left;
            }
            else if (bounds.Right >= Width && bounds.Right + xdistance < Width && !scaleDetector.IsInProgress)
            {
                restrictedXDistance = Width - bounds.Right;
            }
        }
        else if (!scaleDetector.IsInProgress)
        {
            if (bounds.Left >= 0 && bounds.Left + xdistance < 0)
            {
                restrictedXDistance = -bounds.Left;
            }
            else if (bounds.Right <= Width && bounds.Right + xdistance > Width)
            {
                restrictedXDistance = Width - bounds.Right;
            }
        }

        return restrictedXDistance;
    }

    private float getYDistance(float toY, float fromY)
    {
        float ydistance = toY - fromY;

        if (restrictBounds)
        {
            ydistance = getRestrictedYDistance(ydistance);
        }

        //prevents image from translating an infinite distance offscreen
        if (bounds.Bottom + ydistance < 0)
        {
            ydistance = -bounds.Bottom;
        }
        else if (bounds.Top + ydistance > Height)
        {
            ydistance = Height - bounds.Top;
        }

        return ydistance;
    }

    private float getRestrictedYDistance(float ydistance)
    {
        float restrictedYDistance = ydistance;

        if (GetCurrentDisplayedHeight() >= Height)
        {
            if (bounds.Top <= 0 && bounds.Top + ydistance > 0 && !scaleDetector.IsInProgress)
            {
                restrictedYDistance = -bounds.Top;
            }
            else if (bounds.Bottom >= Height && bounds.Bottom + ydistance < Height && !scaleDetector.IsInProgress)
            {
                restrictedYDistance = Height - bounds.Bottom;
            }
        }
        else if (!scaleDetector.IsInProgress)
        {
            if (bounds.Top >= 0 && bounds.Top + ydistance < 0)
            {
                restrictedYDistance = -bounds.Top;
            }
            else if (bounds.Bottom <= Height && bounds.Bottom + ydistance > Height)
            {
                restrictedYDistance = Height - bounds.Bottom;
            }
        }

        return restrictedYDistance;
    }

}

public class AnimationUpdateListener2 : Java.Lang.Object, IAnimatorUpdateListener
{
    readonly float[] values = new float[9];
    readonly Matrix current = new Matrix();
    Matrix ImageMatrix;
    readonly int index;

    public AnimationUpdateListener2(Matrix matrix, int index)
    {
        ImageMatrix = matrix;
        this.index = index;
    }

    public void OnAnimationUpdate(ValueAnimator animation)
    {
        current.Set(ImageMatrix);
        current.GetValues(values);
        values[index] = (float)animation.AnimatedValue;
        current.SetValues(values);
        ImageMatrix = (current);
    }
}

public class AnimationUpdateListener : Java.Lang.Object, IAnimatorUpdateListener
{
    private Matrix imageMatrix;
    private readonly Matrix beginMatrix;
    private readonly float xsdiff;
    private readonly float ysdiff;
    private readonly float xtdiff;
    private readonly float ytdiff;
    private readonly Matrix activeMatrix;
    readonly float[] values = new float[9];

    public AnimationUpdateListener(Matrix imageMatrix, Matrix beginMatrix,
        float xsdiff, float ysdiff, float xtdiff, float ytdiff)
    {
        this.imageMatrix = imageMatrix;
        this.beginMatrix = beginMatrix;
        this.xsdiff = xsdiff;
        this.ysdiff = ysdiff;
        this.xtdiff = xtdiff;
        this.ytdiff = ytdiff;
        activeMatrix = new Matrix(imageMatrix);
    }

    public void OnAnimationUpdate(ValueAnimator animation)
    {
        float val = (float)animation.AnimatedValue;
        activeMatrix.Set(beginMatrix);
        activeMatrix.GetValues(values);
        values[Matrix.MtransX] = values[Matrix.MtransX] + xtdiff * val;
        values[Matrix.MtransY] = values[Matrix.MtransY] + ytdiff * val;
        values[Matrix.MscaleX] = values[Matrix.MscaleX] + xsdiff * val;
        values[Matrix.MscaleY] = values[Matrix.MscaleY] + ysdiff * val;
        activeMatrix.SetValues(values);
        imageMatrix = (activeMatrix);
    }
}

public class SimpleAnimationListener : Java.Lang.Object, IAnimatorListener
{
    private readonly Matrix targetMatrix;
    private Matrix ImageMatrix;

    public SimpleAnimationListener(Matrix targetMatrix, Matrix imageMatrix)
    {
        this.targetMatrix = targetMatrix;
        ImageMatrix = imageMatrix;
    }

    public void OnAnimationCancel(global::Android.Animation.Animator animation)
    {
    }

    public void OnAnimationEnd(global::Android.Animation.Animator animation)
    {
        ImageMatrix = (targetMatrix);
    }

    public void OnAnimationRepeat(global::Android.Animation.Animator animation)
    {
    }

    public void OnAnimationStart(global::Android.Animation.Animator animation)
    {
    }
}
