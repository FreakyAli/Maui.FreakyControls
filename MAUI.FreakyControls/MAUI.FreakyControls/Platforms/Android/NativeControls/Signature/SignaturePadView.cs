using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Graphics;
using Android.Hardware.Lights;
using Microsoft.Maui;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Runtime;

using NativeRect = System.Drawing.RectangleF;
using NativePoint = System.Drawing.PointF;
using NativeSize = System.Drawing.SizeF;
using NativeColor = Android.Graphics.Color;
using SignaturePadCanvasView = Maui.FreakyControls.Platforms.Android.SignaturePadCanvasView;
using NativeImage = Android.Graphics.Bitmap;
using View = Android.Views.View;

namespace Maui.FreakyControls.Platforms.Android;

public partial class SignaturePadView : RelativeLayout
{
    private static Random rnd = new Random();

    public SignaturePadView(Context context)
        : base(context)
    {
        Initialize(null);
    }

    public SignaturePadView(Context context, IAttributeSet attrs)
        : base(context, attrs)
    {
        Initialize(attrs);
    }

    public SignaturePadView(Context context, IAttributeSet attrs, int defStyle)
        : base(context, attrs, defStyle)
    {
        Initialize(attrs);
    }

    private void Initialize(IAttributeSet attrs)
    {
        Inflate(Context, Resource.Layout.signature_pad_layout, this);

        // find the views
        SignaturePadCanvas = FindViewById<SignaturePadCanvasView>(Resource.Id.signature_canvas);
        Caption = FindViewById<TextView>(Resource.Id.caption);
        SignatureLine = FindViewById<View>(Resource.Id.signature_line);
        SignaturePrompt = FindViewById<TextView>(Resource.Id.signature_prompt);
        ClearLabel = FindViewById<TextView>(Resource.Id.clear_label);

        // set the properties from the attributes
        if (attrs != null)
        {
            using (var a = Context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.SignaturePadView, 0, 0))
            {
                if (a.HasValue(Resource.Styleable.SignaturePadView_strokeColor))
                    StrokeColor = a.GetColor(Resource.Styleable.SignaturePadView_strokeColor, ImageConstructionSettings.DefaultStrokeColor);
                if (a.HasValue(Resource.Styleable.SignaturePadView_strokeWidth))
                    StrokeWidth = a.GetDimension(Resource.Styleable.SignaturePadView_strokeWidth, ImageConstructionSettings.DefaultStrokeWidth);

                if (a.HasValue(Resource.Styleable.SignaturePadView_captionText))
                    CaptionText = a.GetString(Resource.Styleable.SignaturePadView_captionText);
                if (a.HasValue(Resource.Styleable.SignaturePadView_captionTextColor))
                    CaptionTextColor = a.GetColor(Resource.Styleable.SignaturePadView_captionTextColor, SignaturePadDarkColor);
                if (a.HasValue(Resource.Styleable.SignaturePadView_captionTextSize))
                    CaptionTextSize = a.GetDimension(Resource.Styleable.SignaturePadView_captionTextSize, DefaultFontSize);

                if (a.HasValue(Resource.Styleable.SignaturePadView_clearLabelText))
                    ClearLabelText = a.GetString(Resource.Styleable.SignaturePadView_clearLabelText);
                if (a.HasValue(Resource.Styleable.SignaturePadView_clearLabelTextColor))
                    ClearLabelTextColor = a.GetColor(Resource.Styleable.SignaturePadView_clearLabelTextColor, SignaturePadDarkColor);
                if (a.HasValue(Resource.Styleable.SignaturePadView_clearLabelTextSize))
                    ClearLabelTextSize = a.GetDimension(Resource.Styleable.SignaturePadView_clearLabelTextSize, DefaultFontSize);

                if (a.HasValue(Resource.Styleable.SignaturePadView_signaturePromptText))
                    SignaturePromptText = a.GetString(Resource.Styleable.SignaturePadView_signaturePromptText);
                if (a.HasValue(Resource.Styleable.SignaturePadView_signaturePromptTextColor))
                    SignaturePromptTextColor = a.GetColor(Resource.Styleable.SignaturePadView_signaturePromptTextColor, SignaturePadDarkColor);
                if (a.HasValue(Resource.Styleable.SignaturePadView_signaturePromptTextSize))
                    SignaturePromptTextSize = a.GetDimension(Resource.Styleable.SignaturePadView_signaturePromptTextSize, DefaultFontSize);

                if (a.HasValue(Resource.Styleable.SignaturePadView_signatureLineColor))
                    SignatureLineColor = a.GetColor(Resource.Styleable.SignaturePadView_signatureLineColor, SignaturePadDarkColor);
                if (a.HasValue(Resource.Styleable.SignaturePadView_signatureLineSpacing))
                    SignatureLineSpacing = a.GetInt(Resource.Styleable.SignaturePadView_signatureLineSpacing, (int)DefaultNarrowSpacing);
                if (a.HasValue(Resource.Styleable.SignaturePadView_signatureLineWidth))
                    SignatureLineWidth = a.GetInt(Resource.Styleable.SignaturePadView_signatureLineWidth, (int)DefaultLineThickness);

                a.Recycle();
            }
        }

        // attach the events
        SignaturePadCanvas.StrokeCompleted += (sender, e) => OnSignatureStrokeCompleted();
        SignaturePadCanvas.Cleared += (sender, e) => OnSignatureCleared();
        ClearLabel.Click += (sender, e) => OnClearTapped();

        // initialize the view
        UpdateUi();
    }



    public SignaturePadCanvasView SignaturePadCanvas { get; private set; }

    public View SignatureLine { get; private set; }

    public TextView Caption { get; private set; }

    public TextView SignaturePrompt { get; private set; }

    public TextView ClearLabel { get; private set; }

    public NativeColor StrokeColor
    {
        get => SignaturePadCanvas.StrokeColor;
        set => SignaturePadCanvas.StrokeColor = value;
    }

    public float StrokeWidth
    {
        get => SignaturePadCanvas.StrokeWidth;
        set => SignaturePadCanvas.StrokeWidth = value;
    }

    public NativeColor SignatureLineColor
    {
        get => (SignatureLine.Background as ColorDrawable)?.Color ?? NativeColor.Transparent;
        set => SignatureLine.SetBackgroundColor(value);
    }

    public int SignatureLineWidth
    {
        get => SignatureLine.Height;
        set
        {
            var param = SignatureLine.LayoutParameters;
            param.Height = value;
            SignatureLine.LayoutParameters = param;
        }
    }

    public int SignatureLineSpacing
    {
        get => SignatureLine.PaddingBottom;
        set => SignatureLine.SetPadding(PaddingLeft, value, PaddingRight, value);
    }

    public string CaptionText
    {
        get => Caption.Text;
        set => Caption.Text = value;
    }

    public float CaptionTextSize
    {
        get => Caption.TextSize;
        set => Caption.TextSize = value;
    }

    public NativeColor CaptionTextColor
    {
        get => new NativeColor(Caption.CurrentTextColor);
        set => Caption.SetTextColor(value);
    }

    public string SignaturePromptText
    {
        get => SignaturePrompt.Text;
        set => SignaturePrompt.Text = value;
    }

    public float SignaturePromptTextSize
    {
        get => SignaturePrompt.TextSize;
        set => SignaturePrompt.TextSize = value;
    }

    public NativeColor SignaturePromptTextColor
    {
        get => new NativeColor(SignaturePrompt.CurrentTextColor);
        set => SignaturePrompt.SetTextColor(value);
    }

    public string ClearLabelText
    {
        get => ClearLabel.Text;
        set => ClearLabel.Text = value;
    }

    public float ClearLabelTextSize
    {
        get => ClearLabel.TextSize;
        set => ClearLabel.TextSize = value;
    }

    public NativeColor ClearLabelTextColor
    {
        get => new NativeColor(ClearLabel.CurrentTextColor);
        set => ClearLabel.SetTextColor(value);
    }

    [Obsolete("Set the background instead.")]
    public NativeColor BackgroundColor
    {
        get => (Background as ColorDrawable)?.Color ?? NativeColor.Transparent;
        set => SetBackgroundColor(value);
    }

    private void UpdateUi()
    {
        ClearLabel.Visibility = IsBlank ? ViewStates.Invisible : ViewStates.Visible;
    }

    public override bool OnInterceptTouchEvent(MotionEvent ev)
    {
        // don't accept touch when the view is disabled
        if (!Enabled)
            return true;

        return base.OnInterceptTouchEvent(ev);
    }
}

partial class SignaturePadView
{
    private const float DefaultWideSpacing = 12.0f;
    private const float DefaultNarrowSpacing = 3.0f;
    private const float DefaultLineThickness = 1.0f;

    private const float DefaultFontSize = 15.0f;

    private const string DefaultClearLabelText = "clear";
    private const string DefaultPromptText = "▶";
    private const string DefaultCaptionText = "sign above the line";

    private static readonly NativeColor SignaturePadDarkColor = NativeColor.Black;
    private static readonly NativeColor SignaturePadLightColor = NativeColor.White;

    public NativePoint[][] Strokes => SignaturePadCanvas.Strokes;

    public NativePoint[] Points => SignaturePadCanvas.Points;

    public bool IsBlank => SignaturePadCanvas?.IsBlank ?? true;

    public event EventHandler StrokeCompleted;

    public event EventHandler Cleared;

    public void Clear()
    {
        SignaturePadCanvas.Clear();

        UpdateUi();
    }

    public void LoadPoints(NativePoint[] points)
    {
        SignaturePadCanvas.LoadPoints(points);

        UpdateUi();
    }

    public void LoadStrokes(NativePoint[][] strokes)
    {
        SignaturePadCanvas.LoadStrokes(strokes);

        UpdateUi();
    }

    /// <summary>
    /// Create an image of the currently drawn signature.
    /// </summary>
    public NativeImage GetImage(bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImage(shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified size.
    /// </summary>
    public NativeImage GetImage(NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImage(size, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified scale.
    /// </summary>
    public NativeImage GetImage(float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImage(scale, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an image of the currently drawn signature with the specified stroke color.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImage(strokeColor, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified size with the specified stroke color.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImage(strokeColor, size, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified scale with the specified stroke color.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImage(strokeColor, scale, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an image of the currently drawn signature with the specified stroke and background colors.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, NativeColor fillColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImage(strokeColor, fillColor, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified size with the specified stroke and background colors.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, NativeColor fillColor, NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImage(strokeColor, fillColor, size, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an image of the currently drawn signature at the specified scale with the specified stroke and background colors.
    /// </summary>
    public NativeImage GetImage(NativeColor strokeColor, NativeColor fillColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImage(strokeColor, fillColor, scale, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an image of the currently drawn signature using the specified settings.
    /// </summary>
    public NativeImage GetImage(ImageConstructionSettings settings)
    {
        return SignaturePadCanvas.GetImage(settings);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified size.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, size, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified scale.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, scale, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified size with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, size, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified scale with the specified stroke color.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, scale, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, NativeColor fillColor, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, fillColor, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified size with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, NativeColor fillColor, NativeSize size, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, fillColor, size, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature at the specified scale with the specified stroke and background colors.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, NativeColor strokeColor, NativeColor fillColor, float scale, bool shouldCrop = true, bool keepAspectRatio = true)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, strokeColor, fillColor, scale, shouldCrop, keepAspectRatio);
    }

    /// <summary>
    /// Create an encoded image of the currently drawn signature using the specified settings.
    /// </summary>
    public Task<Stream> GetImageStreamAsync(SignatureImageFormat format, ImageConstructionSettings settings)
    {
        return SignaturePadCanvas.GetImageStreamAsync(format, settings);
    }

    private void OnClearTapped()
    {
        Clear();
    }

    private void OnSignatureCleared()
    {
        UpdateUi();
        Cleared?.Invoke(this, EventArgs.Empty);
    }

    private void OnSignatureStrokeCompleted()
    {
        UpdateUi();
        StrokeCompleted?.Invoke(this, EventArgs.Empty);
    }
}





