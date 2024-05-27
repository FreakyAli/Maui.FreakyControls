//using Android.Content;
//using Android.Graphics;
//using Android.Util;
//using Android.Views;
//using AndroidX.AppCompat.Widget;
//using static Android.Views.GestureDetector;
//using static Android.Views.View;
//using View = Android.Views.View;
//using global::Android.Widget;
//using Resource = Android.Resource;
//using Uri = Android.Net.Uri;
//using Orientation = Android.Content.Res.Orientation;
//using Android.Graphics.Drawables;
//using RectF = Android.Graphics.RectF;
//using PointF = Android.Graphics.PointF;
//using Android.OS;
//using Android.Views.Animations;
//using static Android.Resource;
//using Java.Lang;

//namespace Maui.FreakyControls.Platforms.Android.NativeControls;

//public class TouchImageView : AppCompatImageView
//{
//    public float CurrentZoom { get; private set; } = 0f;
//    private Matrix touchMatrix;
//    private Matrix prevMatrix;
//    public bool isZoomEnabled { get; set; } = false;
//    public bool isSuperZoomEnabled { get; set; } = true;
//    private bool isRotateImageToFitScreen = false;

//    public FixedPixel? orientationChangeFixedPixel { get; set; } = FixedPixel.Center;
//    public FixedPixel? viewSizeChangeFixedPixel { get; set; } = FixedPixel.Center;
//    private bool orientationJustChanged = false;

//    private ImageActionState? imageActionState = null;
//    private float userSpecifiedMinScale = 0f;
//    private float minScale = 0f;
//    private bool maxScaleIsSetByMultiplier = false;
//    private float maxScaleMultiplier = 0f;
//    private float maxScale = 0f;
//    private float superMinScale = 0f;
//    private float superMaxScale = 0f;
//    private float[] floatMatrix;

//    public float doubleTapScale { get; set; } = 0f;
//    private Fling fling = null;
//    private Orientation orientation = Orientation.Undefined;
//    private ScaleType? touchScaleType = null;
//    private bool imageRenderedAtLeastOnce = false;
//    private bool onDrawReady = false;
//    private ZoomVariables delayedZoomVariables = null;

//    private int viewWidth = 0;
//    private int viewHeight = 0;
//    private int prevViewWidth = 0;
//    private int prevViewHeight = 0;

//    private float matchViewWidth = 0f;
//    private float matchViewHeight = 0f;
//    private float prevMatchViewWidth = 0f;
//    private float prevMatchViewHeight = 0f;
//    private ScaleGestureDetector scaleDetector;
//    private GestureDetector gestureDetector;
//    private IOnTouchCoordinatesListener touchCoordinatesListener = null;
//    private IOnDoubleTapListener doubleTapListener = null;
//    private View.IOnTouchListener userTouchListener = null;
//    private IOnTouchImageViewListener touchImageViewListener = null;

//    public TouchImageView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
//    {
//        Init();
//    }

//    public TouchImageView(Context context, IAttributeSet attrs) : base(context, attrs)
//    {
//        Init();
//    }

//    public TouchImageView(Context context) : base(context)
//    {
//        Init();
//    }

//    private void Init()
//    {
//        base.Clickable = (true);
//        orientation = Resources.Configuration.Orientation;
//        scaleDetector = new ScaleGestureDetector(Context, new ScaleListener(this));
//        gestureDetector = new GestureDetector(Context, new GestureListener(this));
//        touchMatrix = new Matrix();
//        prevMatrix = new Matrix();
//        floatMatrix = new float[9];
//        CurrentZoom = 1f;
//        if (touchScaleType == null)
//        {
//            touchScaleType = ScaleType.FitCenter;
//        }
//        minScale = 1f;
//        maxScale = 3f;
//        superMinScale = SUPER_MIN_MULTIPLIER * minScale;
//        superMaxScale = SUPER_MAX_MULTIPLIER * maxScale;
//        ImageMatrix = touchMatrix;
//        this.ScaleType = ImageView.ScaleType.Matrix;
//        //SetImageState(imageActionState.NONE);
//        onDrawReady = false;
//        base.SetOnTouchListener(new PrivateOnTouchListener(this));
//        using (var attributes = Context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.TouchImageView, 0, defStyle))
//        {
//            if (!IsInEditMode)
//            {
//                isZoomEnabled = attributes.GetBoolean(Resource.Style.TouchImageView_zoom_enabled, true);
//            }
//        }
//    }

//    public void SetRotateImageToFitScreen(bool rotateImageToFitScreen)
//    {
//        isRotateImageToFitScreen = rotateImageToFitScreen;
//    }

//    public override void SetOnTouchListener(View.IOnTouchListener onTouchListener)
//    {
//        userTouchListener = onTouchListener;
//    }

//    public void SetOnTouchImageViewListener(IOnTouchImageViewListener onTouchImageViewListener)
//    {
//        touchImageViewListener = onTouchImageViewListener;
//    }

//    public void SetOnDoubleTapListener(IOnDoubleTapListener onDoubleTapListener)
//    {
//        doubleTapListener = onDoubleTapListener;
//    }

//    public void SetOnTouchCoordinatesListener(IOnTouchCoordinatesListener onTouchCoordinatesListener)
//    {
//        touchCoordinatesListener = onTouchCoordinatesListener;
//    }

//    public override void SetImageResource(int resId)
//    {
//        imageRenderedAtLeastOnce = false;
//        base.SetImageResource(resId);
//        SavePreviousImageValues();
//        FitImageToView();
//    }

//    public override void SetImageBitmap(Bitmap bm)
//    {
//        imageRenderedAtLeastOnce = false;
//        base.SetImageBitmap(bm);
//        SavePreviousImageValues();
//        FitImageToView();
//    }

//    public override void SetImageDrawable(Drawable drawable)
//    {
//        imageRenderedAtLeastOnce = false;
//        base.SetImageDrawable(drawable);
//        SavePreviousImageValues();
//        FitImageToView();
//    }

//    public override void SetImageURI(Uri uri)
//    {
//        imageRenderedAtLeastOnce = false;
//        base.SetImageURI(uri);
//        SavePreviousImageValues();
//        FitImageToView();
//    }

//    public override void SetScaleType(ScaleType type)
//    {
//        if (type == ScaleType.Matrix)
//        {
//            base.SetScaleType(ScaleType.Matrix);
//        }
//        else
//        {
//            touchScaleType = type;
//            if (onDrawReady)
//            {
//                // If the image is already rendered, scaleType has been called programmatically
//                // and the TouchImageView should be updated with the new scaleType.
//                SetZoom(this);
//            }
//        }
//    }

//    public override ScaleType GetScaleType() => touchScaleType ?? throw new InvalidOperationException("touchScaleType is null");

//    public bool IsZoomed => CurrentZoom != 1f;

//    public RectF ZoomedRect
//    {
//        get
//        {
//            if (touchScaleType == ScaleType.FitXy)
//            {
//                throw new NotSupportedException("getZoomedRect() not supported with FIT_XY");
//            }
//            PointF topLeft = TransformCoordTouchToBitmap(0f, 0f, true);
//            PointF bottomRight = TransformCoordTouchToBitmap(viewWidth, viewHeight, true);
//            float w = GetDrawableWidth(Drawable);
//            float h = GetDrawableHeight(Drawable);
//            return new RectF(topLeft.X / w, topLeft.Y / h, bottomRight.X / w, bottomRight.Y / h);
//        }
//    }


//    public void SavePreviousImageValues()
//    {
//        if (viewHeight != 0 && viewWidth != 0)
//        {
//            touchMatrix.GetValues(floatMatrix);
//            prevMatrix.SetValues(floatMatrix);
//            prevMatchViewHeight = matchViewHeight;
//            prevMatchViewWidth = matchViewWidth;
//            prevViewHeight = viewHeight;
//            prevViewWidth = viewWidth;
//        }
//    }

//    protected override IParcelable OnSaveInstanceState()
//    {
//        Bundle bundle = new Bundle();
//        bundle.PutParcelable(STATE, base.OnSaveInstanceState());
//        bundle.PutInt("orientation", (int)orientation);
//        bundle.PutFloat("saveScale", CurrentZoom);
//        bundle.PutFloat("matchViewHeight", matchViewHeight);
//        bundle.PutFloat("matchViewWidth", matchViewWidth);
//        bundle.PutInt("viewWidth", viewWidth);
//        bundle.PutInt("viewHeight", viewHeight);
//        touchMatrix.GetValues(floatMatrix);
//        bundle.PutFloatArray("matrix", floatMatrix);
//        bundle.PutBoolean("imageRendered", imageRenderedAtLeastOnce);
//        bundle.PutSerializable("viewSizeChangeFixedPixel", viewSizeChangeFixedPixel);
//        bundle.PutSerializable("orientationChangeFixedPixel", orientationChangeFixedPixel);
//        return bundle;
//    }

//    protected override void OnRestoreInstanceState(IParcelable state)
//    {
//        if (state is Bundle)
//        {
//            CurrentZoom = state.GetFloat("saveScale");  
//            floatMatrix = state.GetFloatArray("matrix");
//            prevMatrix.SetValues(floatMatrix);
//            prevMatchViewHeight = state.GetFloat("matchViewHeight");
//            prevMatchViewWidth = state.GetFloat("matchViewWidth");
//            prevViewHeight = state.GetInt("viewHeight");
//            prevViewWidth = state.GetInt("viewWidth");
//            imageRenderedAtLeastOnce = state.GetBoolean("imageRendered");
//            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
//            {
//                viewSizeChangeFixedPixel = (FixedPixel)state.GetSerializable("viewSizeChangeFixedPixel");
//                orientationChangeFixedPixel = (FixedPixel)state.GetSerializable("orientationChangeFixedPixel");
//            }
//            else
//            {
//                viewSizeChangeFixedPixel = (FixedPixel?)state.GetSerializable("viewSizeChangeFixedPixel");
//                orientationChangeFixedPixel = (FixedPixel?)state.GetSerializable("orientationChangeFixedPixel");
//            }
//            int oldOrientation = state.GetInt("orientation");
//            if (orientation != oldOrientation)
//            {
//                orientationJustChanged = true;
//            }
//            base.OnRestoreInstanceState(state.GetParcelable(STATE));
//            return;
//        }
//        base.OnRestoreInstanceState(state);
//    }

//    protected override void OnDraw(Canvas canvas)
//    {
//        onDrawReady = true;
//        imageRenderedAtLeastOnce = true;
//        if (delayedZoomVariables != null)
//        {
//            SetZoom(delayedZoomVariables.scale, delayedZoomVariables.focusX, delayedZoomVariables.focusY, delayedZoomVariables.scaleType);
//            delayedZoomVariables = null;
//        }
//        base.OnDraw(canvas);
//    }

//    public override void OnConfigurationChanged(Configuration newConfig)
//    {
//        base.OnConfigurationChanged(newConfig);
//        int newOrientation = Resources.Configuration.Orientation;
//        if (newOrientation != orientation)
//        {
//            orientationJustChanged = true;
//            orientation = newOrientation;
//        }
//        SavePreviousImageValues();
//    }

//    public float MaxZoom
//    {
//        get => maxScale;
//        set
//        {
//            maxScale = value;
//            superMaxScale = SUPER_MAX_MULTIPLIER * maxScale;
//            maxScaleIsSetByMultiplier = false;
//        }
//    }

//    public void SetMaxZoomRatio(float max)
//    {
//        maxScaleMultiplier = max;
//        maxScale = minScale * maxScaleMultiplier;
//        superMaxScale = SUPER_MAX_MULTIPLIER * maxScale;
//        maxScaleIsSetByMultiplier = true;
//    }

//    public float MinZoom
//    {
//        get => minScale;
//        set
//        {
//            userSpecifiedMinScale = value;
//            if (value == AUTOMATIC_MIN_ZOOM)
//            {
//                if (touchScaleType == ScaleType.Center || touchScaleType == ScaleType.CenterCrop)
//                {
//                    Drawable drawable = Drawable;
//                    int drawableWidth = GetDrawableWidth(drawable);
//                    int drawableHeight = GetDrawableHeight(drawable);
//                    if (drawable != null && drawableWidth > 0 && drawableHeight > 0)
//                    {
//                        float widthRatio = (float)viewWidth / drawableWidth;
//                        float heightRatio = (float)viewHeight / drawableHeight;
//                        minScale = touchScaleType == ScaleType.Center ?
//                            Math.Min(widthRatio, heightRatio) :
//                            Math.Min(widthRatio, heightRatio) / Math.Max(widthRatio, heightRatio);
//                    }
//                }
//                else
//                {
//                    minScale = 1.0f;
//                }
//            }
//            else
//            {
//                minScale = userSpecifiedMinScale;
//            }
//            if (maxScaleIsSetByMultiplier)
//            {
//                SetMaxZoomRatio(maxScaleMultiplier);
//            }
//            superMinScale = SUPER_MIN_MULTIPLIER * minScale;
//        }
//    }

//    public void ResetZoom()
//    {
//        CurrentZoom = 1f;
//        FitImageToView();
//    }

//    public void ResetZoomAnimated()
//    {
//        SetZoomAnimated(1f, 0.5f, 0.5f);
//    }

//    public void SetZoom(float scale)
//    {
//        SetZoom(scale, 0.5f, 0.5f);
//    }

//    public void SetZoom(float scale, float focusX, float focusY)
//    {
//        SetZoom(scale, focusX, focusY, touchScaleType);
//    }

//    public void SetZoom(float scale, float focusX, float focusY, ScaleType scaleType)
//    {
//        if (!onDrawReady)
//        {
//            delayedZoomVariables = new ZoomVariables(scale, focusX, focusY, scaleType);
//            return;
//        }
//        if (userSpecifiedMinScale == AUTOMATIC_MIN_ZOOM)
//        {
//            MinZoom = AUTOMATIC_MIN_ZOOM;
//            if (CurrentZoom < minScale)
//            {
//                CurrentZoom = minScale;
//            }
//        }
//        if (scaleType != touchScaleType)
//        {
//            SetScaleType(scaleType);
//        }
//        ResetZoom();
//        ScaleImage(scale, viewWidth / 2f, viewHeight / 2f, isSuperZoomEnabled);
//        touchMatrix.GetValues(floatMatrix);
//        floatMatrix[Matrix.TransX] = -(focusX * imageWidth - viewWidth * 0.5f);
//        floatMatrix[Matrix.TransY] = -(focusY * imageHeight - viewHeight * 0.5f);
//        touchMatrix.SetValues(floatMatrix);
//        FixTrans();
//        SavePreviousImageValues();
//        ImageMatrix = touchMatrix;
//    }

//    public void SetZoom(TouchImageView imageSource)
//    {
//        PointF center = imageSource.ScrollPosition;
//        SetZoom(imageSource.CurrentZoom, center.X, center.Y, imageSource.ScaleType);
//    }

//    public PointF ScrollPosition
//    {
//        get
//        {
//            if (Drawable == null)
//                return new PointF(0.5f, 0.5f);

//            Drawable drawable = Drawable;
//            int drawableWidth = GetDrawableWidth(drawable);
//            int drawableHeight = GetDrawableHeight(drawable);
//            PointF point = TransformCoordTouchToBitmap(viewWidth / 2f, viewHeight / 2f, true);
//            point.X /= drawableWidth;
//            point.Y /= drawableHeight;
//            return point;
//        }
//    }

//    private bool OrientationMismatch(Drawable drawable)
//    {
//        return viewWidth > viewHeight != drawable.IntrinsicWidth > drawable.IntrinsicHeight;
//    }

//    private int GetDrawableWidth(Drawable drawable)
//    {
//        return OrientationMismatch(drawable) && isRotateImageToFitScreen ?
//            drawable.IntrinsicHeight : drawable.IntrinsicWidth;
//    }

//    private int GetDrawableHeight(Drawable drawable)
//    {
//        return OrientationMismatch(drawable) && isRotateImageToFitScreen ?
//            drawable.IntrinsicWidth : drawable.IntrinsicHeight;
//    }

//    public void SetScrollPosition(float focusX, float focusY)
//    {
//        SetZoom(CurrentZoom, focusX, focusY);
//    }

//    private void FixTrans()
//    {
//        touchMatrix.GetValues(floatMatrix);
//        float transX = floatMatrix[Matrix.TransX];
//        float transY = floatMatrix[Matrix.TransY];
//        float offset = 0f;
//        if (isRotateImageToFitScreen && OrientationMismatch(Drawable))
//        {
//            offset = imageWidth;
//        }
//        float fixTransX = GetFixTrans(transX, viewWidth, imageWidth, offset);
//        float fixTransY = GetFixTrans(transY, viewHeight, imageHeight, 0f);
//        touchMatrix.PostTranslate(fixTransX, fixTransY);
//    }


//    private void FixScaleTrans()
//    {
//        FixTrans();
//        touchMatrix.GetValues(floatMatrix);
//        if (imageWidth < viewWidth)
//        {
//            float xOffset = (viewWidth - imageWidth) / 2;
//            if (isRotateImageToFitScreen && OrientationMismatch(Drawable))
//            {
//                xOffset += imageWidth;
//            }
//            floatMatrix[Matrix.TransX] = xOffset;
//        }
//        if (imageHeight < viewHeight)
//        {
//            floatMatrix[Matrix.TransY] = (viewHeight - imageHeight) / 2;
//        }
//        touchMatrix.SetValues(floatMatrix);
//    }

//    private float GetFixTrans(float trans, float viewSize, float contentSize, float offset)
//    {
//        float minTrans;
//        float maxTrans;
//        if (contentSize <= viewSize)
//        {
//            minTrans = offset;
//            maxTrans = offset + viewSize - contentSize;
//        }
//        else
//        {
//            minTrans = offset + viewSize - contentSize;
//            maxTrans = offset;
//        }
//        if (trans < minTrans) return -trans + minTrans;
//        return trans > maxTrans ? -trans + maxTrans : 0f;
//    }

//    private float GetFixDragTrans(float delta, float viewSize, float contentSize)
//    {
//        return contentSize <= viewSize ? 0f : delta;
//    }

//    private float ImageWidth => matchViewWidth * CurrentZoom;

//    private float ImageHeight => matchViewHeight * CurrentZoom;


//    protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
//    {
//        Drawable drawable = Drawable;
//        if (drawable == null || drawable.IntrinsicWidth == 0 || drawable.IntrinsicHeight == 0)
//        {
//            SetMeasuredDimension(0, 0);
//            return;
//        }
//        int drawableWidth = GetDrawableWidth(drawable);
//        int drawableHeight = GetDrawableHeight(drawable);
//        int widthSize = MeasureSpec.GetSize(widthMeasureSpec);
//        MeasureSpecMode widthMode = MeasureSpec.GetMode(widthMeasureSpec);
//        int heightSize = MeasureSpec.GetSize(heightMeasureSpec);
//        MeasureSpecMode heightMode = MeasureSpec.GetMode(heightMeasureSpec);
//        int totalViewWidth = SetViewSize(widthMode, widthSize, drawableWidth);
//        int totalViewHeight = SetViewSize(heightMode, heightSize, drawableHeight);
//        if (!orientationJustChanged)
//        {
//            SavePreviousImageValues();
//        }

//        // Image view width, height must consider padding
//        int width = totalViewWidth - PaddingLeft - PaddingRight;
//        int height = totalViewHeight - PaddingTop - PaddingBottom;

//        // Set view dimensions
//        SetMeasuredDimension(width, height);
//    }

//    protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
//    {
//        base.OnSizeChanged(w, h, oldw, oldh);

//        // Fit content within view.
//        //
//        // onMeasure may be called multiple times for each layout change, including orientation
//        // changes. For example, if the TouchImageView is inside a ConstraintLayout, onMeasure may
//        // be called with:
//        // widthMeasureSpec == "AT_MOST 2556" and then immediately with
//        // widthMeasureSpec == "EXACTLY 1404", then back and forth multiple times in quick
//        // succession, as the ConstraintLayout tries to solve its constraints.
//        //
//        // onSizeChanged is called once after the final onMeasure is called. So we make all changes
//        // to class members, such as fitting the image into the new shape of the TouchImageView,
//        // here, after the final size has been determined. This helps us avoid both
//        // repeated computations, and making irreversible changes (e.g. making the View temporarily too
//        // big or too small, thus making the current zoom fall outside of an automatically-changing
//        // minZoom and maxZoom).
//        viewWidth = w;
//        viewHeight = h;
//        FitImageToView();
//    }

//    private void FitImageToView()
//    {
//        var fixedPixel = orientationJustChanged ? orientationChangeFixedPixel : viewSizeChangeFixedPixel;
//        orientationJustChanged = false;
//        var drawable = Drawable;
//        if (drawable == null || drawable.IntrinsicWidth == 0 || drawable.IntrinsicHeight == 0)
//        {
//            return;
//        }
//        if (touchMatrix == null || prevMatrix == null)
//        {
//            return;
//        }
//        if (userSpecifiedMinScale == AUTOMATIC_MIN_ZOOM)
//        {
//            MinZoom = AUTOMATIC_MIN_ZOOM;
//            if (CurrentZoom < minScale)
//            {
//                CurrentZoom = minScale;
//            }
//        }
//        var drawableWidth = GetDrawableWidth(drawable);
//        var drawableHeight = GetDrawableHeight(drawable);

//        // Scale image for view
//        var scaleX = viewWidth / (float)drawableWidth;
//        var scaleY = viewHeight / (float)drawableHeight;
//        switch (touchScaleType)
//        {
//            case ScaleType.Center:
//                {
//                    scaleY = 1f;
//                    scaleX = scaleY;
//                    break;
//                }
//            case ScaleType.CenterCrop:
//                {
//                    scaleY = Math.Max(scaleX, scaleY);
//                    scaleX = scaleY;
//                    break;
//                }
//            case ScaleType.CenterInside:
//                {
//                    scaleY = Math.Min(1f, Math.Min(scaleX, scaleY));
//                    scaleX = scaleY;
//                    break;
//                }
//            case ScaleType.FitCenter:
//            case ScaleType.FitStart:
//            case ScaleType.FitEnd:
//                {
//                    scaleY = Math.Min(scaleX, scaleY);
//                    scaleX = scaleY;
//                    break;
//                }
//            case ScaleType.FitXY:
//                break;
//        }

//        // Put the image's center in the right place.
//        var redundantXSpace = viewWidth - scaleX * drawableWidth;
//        var redundantYSpace = viewHeight - scaleY * drawableHeight;
//        matchViewWidth = viewWidth - redundantXSpace;
//        matchViewHeight = viewHeight - redundantYSpace;
//        if (!isZoomed && !imageRenderedAtLeastOnce)
//        {
//            // Stretch and center image to fit view
//            if (isRotateImageToFitScreen && OrientationMismatch(drawable))
//            {
//                touchMatrix.SetRotate(90f);
//                touchMatrix.PostTranslate(drawableWidth, 0f);
//                touchMatrix.PostScale(scaleX, scaleY);
//            }
//            else
//            {
//                touchMatrix.SetScale(scaleX, scaleY);
//            }
//            switch (touchScaleType)
//            {
//                case ScaleType.FitStart:
//                    touchMatrix.PostTranslate(0f, 0f);
//                    break;
//                case ScaleType.FitEnd:
//                    touchMatrix.PostTranslate(redundantXSpace, redundantYSpace);
//                    break;
//                default:
//                    touchMatrix.PostTranslate(redundantXSpace / 2, redundantYSpace / 2);
//                    break;
//            }
//            CurrentZoom = 1f;
//        }
//        else
//        {
//            // These values should never be 0 or we will set viewWidth and viewHeight
//            // to NaN in newTranslationAfterChange. To avoid this, call savePreviousImageValues
//            // to set them equal to the current values.
//            if (prevMatchViewWidth == 0f || prevMatchViewHeight == 0f)
//            {
//                SavePreviousImageValues();
//            }

//            // Use the previous matrix as our starting point for the new matrix.
//            prevMatrix.GetValues(floatMatrix);

//            // Rescale Matrix if appropriate
//            floatMatrix[Matrix.MscaleX] = matchViewWidth / drawableWidth * CurrentZoom;
//            floatMatrix[Matrix.MscaleY] = matchViewHeight / drawableHeight * CurrentZoom;

//            // TransX and TransY from previous matrix
//            var transX = floatMatrix[Matrix.MtransX];
//            var transY = floatMatrix[Matrix.MtransY];

//            // X position
//            var prevActualWidth = prevMatchViewWidth * CurrentZoom;
//            var actualWidth = imageWidth;
//            floatMatrix[Matrix.MtransX] = NewTranslationAfterChange(transX, prevActualWidth, actualWidth, prevViewWidth, viewWidth, drawableWidth, fixedPixel);

//            // Y position
//            var prevActualHeight = prevMatchViewHeight * CurrentZoom;
//            var actualHeight = imageHeight;
//            floatMatrix[Matrix.MtransY] = NewTranslationAfterChange(transY, prevActualHeight, actualHeight, prevViewHeight, viewHeight, drawableHeight, fixedPixel);

//            // Set the matrix to the adjusted scale and translation values.
//            touchMatrix.SetValues(floatMatrix);
//        }
//        FixTrans();
//        ImageMatrix = touchMatrix;
//    }

//    private int SetViewSize(int mode, int size, int drawableWidth)
//    {
//        switch (mode)
//        {
//            case MeasureSpecMode.Exactly:
//                return size;
//            case MeasureSpecMode.AtMost:
//                return Math.Min(drawableWidth, size);
//            case MeasureSpecMode.Unspecified:
//                return drawableWidth;
//            default:
//                return size;
//        }
//    }

//    private float NewTranslationAfterChange(float trans, float prevImageSize, float imageSize, int prevViewSize, int viewSize, int drawableSize, FixedPixel? sizeChangeFixedPixel)
//    {
//        if (imageSize < viewSize)
//        {
//            // The width/height of image is less than the view's width/height. Center it.
//            return (viewSize - drawableSize * floatMatrix[Matrix.MscaleX]) * 0.5f;
//        }
//        else if (trans > 0)
//        {
//            // The image is larger than the view, but was not before the view changed. Center it.
//            return -((imageSize - viewSize) * 0.5f);
//        }
//        else
//        {
//            // Where is the pixel in the View that we are keeping stable, as a fraction of the width/height of the View?
//            float fixedPixelPositionInView = 0.5f; // CENTER
//            if (sizeChangeFixedPixel == FixedPixel.BottomRight)
//            {
//                fixedPixelPositionInView = 1.0f;
//            }
//            else if (sizeChangeFixedPixel == FixedPixel.TopLeft)
//            {
//                fixedPixelPositionInView = 0.0f;
//            }
//            // Where is the pixel in the Image that we are keeping stable, as a fraction of the
//            // width/height of the Image?
//            float fixedPixelPositionInImage = (-trans + fixedPixelPositionInView * prevViewSize) / prevImageSize;

//            // Here's what the new translation should be so that, after whatever change triggered
//            // this function to be called, the pixel at fixedPixelPositionInView of the View is
//            // still the pixel at fixedPixelPositionInImage of the image.
//            return -(fixedPixelPositionInImage * imageSize - viewSize * fixedPixelPositionInView);
//        }
//    }

//    private void SetState(ImageActionState imageActionState)
//    {
//        this.imageActionState = imageActionState;
//    }

//    public override bool CanScrollHorizontally(Direction direction)
//    {
//        touchMatrix.GetValues(floatMatrix);
//        float x = floatMatrix[Matrix.MTransX];
//        if (imageWidth < viewWidth)
//        {
//            return false;
//        }
//        else if (x >= -1 && direction < 0)
//        {
//            return false;
//        }
//        else
//        {
//            return Math.Abs(x) + viewWidth + 1 < imageWidth || direction <= 0;
//        }
//    }

//    public override bool CanScrollVertically(Direction direction)
//    {
//        touchMatrix.GetValues(floatMatrix);
//        float y = floatMatrix[Matrix.MTransY];
//        if (imageHeight < viewHeight)
//        {
//            return false;
//        }
//        else if (y >= -1 && direction < 0)
//        {
//            return false;
//        }
//        else
//        {
//            return Math.Abs(y) + viewHeight + 1 < imageHeight || direction <= 0;
//        }
//    }


//    public void scaleImage(double deltaScale, float focusX, float focusY, bool stretchImageToSuper)
//    {
//        float deltaScaleLocal = (float)deltaScale;
//        float lowerScale;
//        float upperScale;
//        if (stretchImageToSuper)
//        {
//            lowerScale = superMinScale;
//            upperScale = superMaxScale;
//        }
//        else
//        {
//            lowerScale = minScale;
//            upperScale = maxScale;
//        }
//        float origScale = CurrentZoom;
//        CurrentZoom *= deltaScaleLocal;
//        if (CurrentZoom > upperScale)
//        {
//            CurrentZoom = upperScale;
//            deltaScaleLocal = upperScale / origScale;
//        }
//        else if (CurrentZoom < lowerScale)
//        {
//            CurrentZoom = lowerScale;
//            deltaScaleLocal = lowerScale / origScale;
//        }
//        touchMatrix.PostScale(deltaScaleLocal, deltaScaleLocal, focusX, focusY);
//        FixScaleTrans();
//    }

//    protected PointF TransformCoordTouchToBitmap(float x, float y, bool clipToBitmap)
//    {
//        touchMatrix.GetValues(floatMatrix);
//        float origW = drawable.IntrinsicWidth;
//        float origH = drawable.IntrinsicHeight;
//        float transX = floatMatrix[Matrix.MtransX];
//        float transY = floatMatrix[Matrix.MtransY];
//        float finalX = (x - transX) * origW / imageWidth;
//        float finalY = (y - transY) * origH / imageHeight;
//        if (clipToBitmap)
//        {
//            finalX = Math.Min(Math.Max(finalX, 0), origW);
//            finalY = Math.Min(Math.Max(finalY, 0), origH);
//        }
//        return new PointF(finalX, finalY);
//    }

//    protected PointF TransformCoordBitmapToTouch(float bx, float by)
//    {
//        touchMatrix.GetValues(floatMatrix);
//        float origW = drawable.IntrinsicWidth;
//        float origH = drawable.IntrinsicHeight;
//        float px = bx / origW;
//        float py = by / origH;
//        float finalX = floatMatrix[Matrix.MtransX] + imageWidth * px;
//        float finalY = floatMatrix[Matrix.MtransY] + imageHeight * py;
//        return new PointF(finalX, finalY);
//    }

//    public void CompatPostOnAnimation(IRunnable runnable)
//    {
//        PostOnAnimation(runnable);
//    }

//    public void SetZoomAnimated(float scale, float focusX, float focusY)
//    {
//        SetZoomAnimated(scale, focusX, focusY, DefaultZoomTime);
//    }

//    public void SetZoomAnimated(float scale, float focusX, float focusY, int zoomTimeMs)
//    {
//        AnimatedZoom animation = new AnimatedZoom(scale, new PointF(focusX, focusY), zoomTimeMs);
//        CompatPostOnAnimation(animation);
//    }

//    public void SetZoomAnimated(float scale, float focusX, float focusY, int zoomTimeMs, OnZoomFinishedListener listener)
//    {
//        AnimatedZoom animation = new AnimatedZoom(scale, new PointF(focusX, focusY), zoomTimeMs);
//        animation.SetListener(listener);
//        CompatPostOnAnimation(animation);
//    }

//    public void SetZoomAnimated(float scale, float focusX, float focusY, OnZoomFinishedListener listener)
//    {
//        AnimatedZoom animation = new AnimatedZoom(scale, new PointF(focusX, focusY), DefaultZoomTime);
//        animation.SetListener(listener);
//        CompatPostOnAnimation(animation);
//    }

//}

//public class AnimatedZoom : Java.Lang.Object, IRunnable
//{
//    private readonly int _zoomTimeMillis;
//    private readonly long _startTime;
//    private readonly float _startZoom;
//    private readonly float _targetZoom;
//    private readonly PointF _startFocus;
//    private readonly PointF _targetFocus;
//    private readonly IInterpolator _interpolator = new LinearInterpolator();
//    private OnZoomFinishedListener _zoomFinishedListener;

//    public AnimatedZoom(float targetZoom, PointF focus, int zoomTimeMillis)
//    {
//        setState(ImageActionState.ANIMATE_ZOOM);
//        _startTime = JavaSystem.CurrentTimeMillis();
//        _startZoom = currentZoom;
//        _targetZoom = targetZoom;
//        _zoomTimeMillis = zoomTimeMillis;
//        _startFocus = scrollPosition;
//        _targetFocus = focus;
//    }

//    public void Run()
//    {
//        float t = interpolate();

//        // Calculate the next focus and zoom based on the progress of the interpolation
//        float nextZoom = _startZoom + (_targetZoom - _startZoom) * t;
//        float nextX = _startFocus.X + (_targetFocus.X - _startFocus.X) * t;
//        float nextY = _startFocus.Y + (_targetFocus.Y - _startFocus.Y) * t;
//        SetZoom(nextZoom, nextX, nextY);
//        if (t < 1f)
//        {
//            // We haven't finished zooming
//            CompatPostOnAnimation(this);
//        }
//        else
//        {
//            // Finished zooming
//            setState(ImageActionState.NONE);
//            _zoomFinishedListener?.OnZoomFinished();
//        }
//    }

//    /**
//     * Use interpolator to get t
//     *
//     * @return progress of the interpolation
//     */
//    private float interpolate()
//    {
//        float elapsed = (JavaSystem.CurrentTimeMillis() - _startTime) / (float)_zoomTimeMillis;
//        elapsed = Math.Min(1f, elapsed);
//        return _interpolator.GetInterpolation(elapsed);
//    }

//    public void SetListener(OnZoomFinishedListener listener)
//    {
//        _zoomFinishedListener = listener;
//    }
//}

//public static class Constants
//{
//    public const string STATE = "instanceState";
//    // SuperMin and SuperMax multipliers. Determine how much the image can be zoomed below or above the zoom boundaries,
//    // before animating back to the min/max zoom boundary.
//    public const float SUPER_MIN_MULTIPLIER = 0.75f;
//    public const float SUPER_MAX_MULTIPLIER = 1.25f;
//    public const int DEFAULT_ZOOM_TIME = 500;

//    // If setMinZoom(AUTOMATIC_MIN_ZOOM), then we'll set the min scale to include the whole image.
//    public const float AUTOMATIC_MIN_ZOOM = -1.0f;
//}

//internal class ZoomVariables
//{
//    public float Scale { get; set; }
//    public float FocusX { get; set; }
//    public float FocusY { get; set; }
//    public ImageView.ScaleType? ScaleType { get; set; }

//    public ZoomVariables(float scale, float focusX, float focusY, ImageView.ScaleType? scaleType)
//    {
//        Scale = scale;
//        FocusX = focusX;
//        FocusY = focusY;
//        ScaleType = scaleType;
//    }
//}

//public class GestureListener : GestureDetector.SimpleOnGestureListener
//{
//    public override bool OnSingleTapConfirmed(MotionEvent e)
//    {
//        // Pass on to the OnDoubleTapListener if it is present, otherwise let the View handle the click.
//        return doubleTapListener?.OnSingleTapConfirmed(e) ?? PerformClick();
//    }

//    public override void OnLongPress(MotionEvent e)
//    {
//        PerformLongClick();
//    }

//    public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
//    {
//        // If a previous fling is still active, it should be cancelled so that two flings
//        // are not run simultaneously.
//        fling?.CancelFling();
//        fling = new Fling((int)velocityX, (int)velocityY);
//        compatPostOnAnimation(fling);
//        return base.OnFling(e1, e2, velocityX, velocityY);
//    }

//    public override bool OnDoubleTap(MotionEvent e)
//    {
//        bool consumed = false;
//        if (isZoomEnabled)
//        {
//            consumed = doubleTapListener?.OnDoubleTap(e) ?? false;
//            if (imageActionState == ImageActionState.NONE)
//            {
//                float maxZoomScale = doubleTapScale == 0f ? maxScale : doubleTapScale;
//                float targetZoom = currentZoom == minScale ? maxZoomScale : minScale;
//                DoubleTapZoom doubleTap = new DoubleTapZoom(targetZoom, e.GetX(), e.GetY(), false);
//                compatPostOnAnimation(doubleTap);
//                consumed = true;
//            }
//        }
//        return consumed;
//    }

//    public override bool OnDoubleTapEvent(MotionEvent e)
//    {
//        return doubleTapListener?.OnDoubleTapEvent(e) ?? false;
//    }
//}

//public class Fling : Java.Lang.Object, IRunnable
//{
//    private CompatScroller scroller;
//    private int currX;
//    private int currY;
//    private readonly Context context;

//    public Fling(Context context, int velocityX, int velocityY)
//    {
//        this.context = context;
//        setState(ImageActionState.FLING);
//        scroller = new CompatScroller(context);
//        touchMatrix.GetValues(floatMatrix);
//        int startX = (int)floatMatrix[Matrix.MtransX];
//        int startY = (int)floatMatrix[Matrix.MtransY];
//        int minX;
//        int maxX;
//        int minY;
//        int maxY;
//        if (isRotateImageToFitScreen && orientationMismatch(drawable))
//        {
//            startX -= (int)imageWidth;
//        }
//        if (imageWidth > viewWidth)
//        {
//            minX = viewWidth - (int)imageWidth;
//            maxX = 0;
//        }
//        else
//        {
//            maxX = startX;
//            minX = maxX;
//        }
//        if (imageHeight > viewHeight)
//        {
//            minY = viewHeight - (int)imageHeight;
//            maxY = 0;
//        }
//        else
//        {
//            maxY = startY;
//            minY = maxY;
//        }
//        scroller.Fling(startX, startY, velocityX, velocityY, minX, maxX, minY, maxY);
//        currX = startX;
//        currY = startY;
//    }

//    public void CancelFling()
//    {
//        setState(ImageActionState.NONE);
//        scroller.ForceFinished(true);
//    }

//    public void Run()
//    {
//        // OnTouchImageViewListener is set: TouchImageView listener has been flung by user.
//        // Listener runnable updated with each frame of fling animation.
//        touchImageViewListener?.OnMove();
//        if (scroller.IsFinished)
//        {
//            return;
//        }
//        if (scroller.ComputeScrollOffset())
//        {
//            int newX = scroller.CurrX;
//            int newY = scroller.CurrY;
//            int transX = newX - currX;
//            int transY = newY - currY;
//            currX = newX;
//            currY = newY;
//            touchMatrix.PostTranslate(transX, transY);
//            FixTrans();
//            imageMatrix = touchMatrix;
//            compatPostOnAnimation(this);
//        }
//    }
//}

//public class CompatScroller
//{
//    private readonly OverScroller overScroller;

//    public CompatScroller(Context context)
//    {
//        overScroller = new OverScroller(context);
//    }

//    public void Fling(int startX, int startY, int velocityX, int velocityY, int minX, int maxX, int minY, int maxY)
//    {
//        overScroller.Fling(startX, startY, velocityX, velocityY, minX, maxX, minY, maxY);
//    }

//    public void ForceFinished(bool finished)
//    {
//        overScroller.ForceFinished(finished);
//    }

//    public bool IsFinished => overScroller.IsFinished;

//    public bool ComputeScrollOffset()
//    {
//        return overScroller.ComputeScrollOffset();
//    }

//    public int CurrX => overScroller.CurrX;

//    public int CurrY => overScroller.CurrY;
//}

//public class PrivateOnTouchListener : Java.Lang.Object, View.IOnTouchListener
//{
//    // Remember last point position for dragging
//    private readonly PointF last = new PointF();

//    public bool OnTouch(View v, MotionEvent e)
//    {
//        if (Drawable == null)
//        {
//            setState(ImageActionState.NONE);
//            return false;
//        }

//        if (isZoomEnabled)
//        {
//            scaleDetector.OnTouchEvent(e);
//        }

//        gestureDetector.OnTouchEvent(e);

//        PointF curr = new PointF(e.GetX(), e.GetY());

//        if (imageActionState == ImageActionState.NONE || imageActionState == ImageActionState.DRAG || imageActionState == ImageActionState.FLING)
//        {
//            switch (e.Action)
//            {
//                case MotionEventActions.Down:
//                    last.Set(curr);
//                    fling?.CancelFling();
//                    setState(ImageActionState.DRAG);
//                    break;

//                case MotionEventActions.Move:
//                    if (imageActionState == ImageActionState.DRAG)
//                    {
//                        float deltaX = curr.X - last.X;
//                        float deltaY = curr.Y - last.Y;
//                        float fixTransX = getFixDragTrans(deltaX, viewWidth, imageWidth);
//                        float fixTransY = getFixDragTrans(deltaY, viewHeight, imageHeight);
//                        touchMatrix.PostTranslate(fixTransX, fixTransY);
//                        fixTrans();
//                        last.Set(curr.X, curr.Y);
//                    }
//                    break;

//                case MotionEventActions.Up:
//                case MotionEventActions.PointerUp:
//                    setState(ImageActionState.NONE);
//                    break;
//            }
//        }

//        touchCoordinatesListener?.Invoke(v, e, transformCoordTouchToBitmap(e.GetX(), e.GetY(), true));

//        imageMatrix = touchMatrix;

//        // User-defined OnTouchListener
//        userTouchListener?.OnTouch(v, e);

//        // OnTouchImageViewListener is set: TouchImageView dragged by user.
//        touchImageViewListener?.OnMove();

//        // indicate event was handled
//        return true;
//    }
//}

//public class ScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener
//{
//    private readonly TouchImageView imageView;

//    public ScaleListener(TouchImageView imageView)
//    {
//        this.imageView = imageView;
//    }
//    public override bool OnScaleBegin(ScaleGestureDetector detector)
//    {
//        //imageView.SetImageState(ImageActionState.ZOOM);
//        return true;
//    }

//    public override bool OnScale(ScaleGestureDetector detector)
//    {
//        scaleImage(detector.ScaleFactor, detector.FocusX, detector.FocusY, isSuperZoomEnabled);

//        // OnTouchImageViewListener is set: TouchImageView pinch zoomed by user.
//        touchImageViewListener?.OnMove();
//        return true;
//    }

//    public override void OnScaleEnd(ScaleGestureDetector detector)
//    {
//        base.OnScaleEnd(detector);
//        setState(ImageActionState.NONE);
//        bool animateToZoomBoundary = false;
//        float targetZoom = currentZoom;
//        if (currentZoom > maxScale)
//        {
//            targetZoom = maxScale;
//            animateToZoomBoundary = true;
//        }
//        else if (currentZoom < minScale)
//        {
//            targetZoom = minScale;
//            animateToZoomBoundary = true;
//        }
//        if (animateToZoomBoundary)
//        {
//            DoubleTapZoom doubleTap = new DoubleTapZoom(targetZoom, viewWidth / 2f, viewHeight / 2f, isSuperZoomEnabled);
//            compatPostOnAnimation(doubleTap);
//        }
//    }
//}

//public class DoubleTapZoom : Java.Lang.Object, Java.Lang.IRunnable
//{
//    private readonly long startTime;
//    private readonly float startZoom;
//    private readonly float targetZoom;
//    private readonly float bitmapX;
//    private readonly float bitmapY;
//    private readonly bool stretchImageToSuper;
//    private readonly AccelerateDecelerateInterpolator interpolator = new AccelerateDecelerateInterpolator();
//    private readonly PointF startTouch;
//    private readonly PointF endTouch;

//    public DoubleTapZoom(float targetZoom, float focusX, float focusY, bool stretchImageToSuper)
//    {
//        setState(ImageActionState.ANIMATE_ZOOM);
//        startTime = Java.Lang.JavaSystem.CurrentTimeMillis();
//        startZoom = currentZoom;
//        this.targetZoom = targetZoom;
//        this.stretchImageToSuper = stretchImageToSuper;
//        PointF bitmapPoint = transformCoordTouchToBitmap(focusX, focusY, false);
//        bitmapX = bitmapPoint.X;
//        bitmapY = bitmapPoint.Y;
//        startTouch = transformCoordBitmapToTouch(bitmapX, bitmapY);
//        endTouch = new PointF(viewWidth / 2f, viewHeight / 2f);
//    }

//    public void Run()
//    {
//        if (Drawable == null)
//        {
//            setState(ImageActionState.NONE);
//            return;
//        }

//        float t = interpolate();
//        double deltaScale = calculateDeltaScale(t);
//        scaleImage(deltaScale, bitmapX, bitmapY, stretchImageToSuper);
//        translateImageToCenterTouchPosition(t);
//        fixScaleTrans();
//        ImageMatrix = touchMatrix;

//        // double tap runnable updates listener with every frame.
//        touchImageViewListener?.OnMove();
//        if (t < 1f)
//        {
//            // We haven't finished zooming
//            compatPostOnAnimation(this);
//        }
//        else
//        {
//            // Finished zooming
//            setState(ImageActionState.NONE);
//        }
//    }

//    private float interpolate()
//    {
//        long currTime = Java.Lang.JavaSystem.CurrentTimeMillis();
//        float elapsed = (currTime - startTime) / DEFAULT_ZOOM_TIME;
//        elapsed = Math.Min(1f, elapsed);
//        return interpolator.GetInterpolation(elapsed);
//    }

//    private double calculateDeltaScale(float t)
//    {
//        double zoom = startZoom + t * (targetZoom - startZoom);
//        return zoom / currentZoom;
//    }

//    private void translateImageToCenterTouchPosition(float t)
//    {
//        float targetX = startTouch.X + t * (endTouch.X - startTouch.X);
//        float targetY = startTouch.Y + t * (endTouch.Y - startTouch.Y);
//        PointF curr = transformCoordBitmapToTouch(bitmapX, bitmapY);
//        touchMatrix.PostTranslate(targetX - curr.X, targetY - curr.Y);
//    }
//}

//public interface IOnZoomFinishedListener
//{
//    void OnZoomFinished();
//}

//public interface IOnTouchImageViewListener
//{
//    void OnMove();
//}

//public interface IOnTouchCoordinatesListener
//{
//    void OnTouchCoordinate(View view, MotionEvent e, PointF bitmapPoint);
//}

//public enum FixedPixel
//{
//    Center,
//    TopLeft,
//    BottomRight
//}

//internal enum ImageActionState
//{
//    None,
//    Drag,
//    Zoom,
//    Fling,
//    AnimateZoom
//}
