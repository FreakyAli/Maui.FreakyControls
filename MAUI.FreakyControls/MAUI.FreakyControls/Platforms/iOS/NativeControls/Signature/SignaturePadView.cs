using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NativeRect = CoreGraphics.CGRect;
using NativePoint = CoreGraphics.CGPoint;
using NativeSize = CoreGraphics.CGSize;
using NativeColor = UIKit.UIColor;
using NativeImage = UIKit.UIImage;
using System.Runtime.InteropServices;

namespace Maui.FreakyControls.Platforms.iOS;

[Register("SignaturePadView")]
[DesignTimeVisible(true)]
public partial class SignaturePadView : UIView
{
    private UIEdgeInsets padding;

    public SignaturePadView()
    {
        Initialize();
    }

    public SignaturePadView(NSCoder coder)
        : base(coder)
    {
        Initialize(/* ? baseProperties: false ? */);
    }

    public SignaturePadView(IntPtr ptr)
        : base(ptr)
    {
        Initialize(false);
    }

    public SignaturePadView(CGRect frame)
        : base(frame)
    {
        Initialize();
    }

    private void Initialize(bool baseProperties = true)
    {
        // add the background view
        {
            BackgroundImageView = new UIImageView();
            AddSubview(BackgroundImageView);
        }

        // add the main signature view
        {
            SignaturePadCanvas = new SignaturePadCanvasView();
            SignaturePadCanvas.StrokeCompleted += delegate
            {
                OnSignatureStrokeCompleted();
            };
            SignaturePadCanvas.Cleared += delegate
            {
                OnSignatureCleared();
            };
            AddSubview(SignaturePadCanvas);
        }

        // add the caption
        {
            Caption = new UILabel()
            {
                BackgroundColor = UIColor.Clear,
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.SystemFontOfSize(DefaultFontSize),
                Text = DefaultCaptionText,
                TextColor = SignaturePadDarkColor,
            };
            AddSubview(Caption);
        }

        // add the signature line
        {
            SignatureLine = new UIView()
            {
                BackgroundColor = SignaturePadDarkColor,
            };
            SignatureLineWidth = DefaultLineThickness;
            SignatureLineSpacing = DefaultNarrowSpacing;
            AddSubview(SignatureLine);
        }

        // add the prompt
        {
            SignaturePrompt = new UILabel()
            {
                BackgroundColor = UIColor.Clear,
                Font = UIFont.BoldSystemFontOfSize(DefaultFontSize),
                Text = DefaultPromptText,
                TextColor = SignaturePadDarkColor,
            };
            AddSubview(SignaturePrompt);
        }

        // add the clear label
        {
            ClearLabel = UIButton.FromType(UIButtonType.Custom);
            ClearLabel.BackgroundColor = UIColor.Clear;
            ClearLabel.TitleLabel.Font = UIFont.BoldSystemFontOfSize(DefaultFontSize);
            ClearLabel.SetTitle(DefaultClearLabelText, UIControlState.Normal);
            ClearLabel.SetTitleColor(SignaturePadDarkColor, UIControlState.Normal);
            AddSubview(ClearLabel);

            // attach the "clear" command
            ClearLabel.TouchUpInside += delegate
            {
                OnClearTapped();
            };
        }

        Padding = new UIEdgeInsets(DefaultWideSpacing, DefaultWideSpacing, DefaultNarrowSpacing, DefaultWideSpacing);

        // clear / initialize the view
        UpdateUi();
    }

    public SignaturePadCanvasView SignaturePadCanvas { get; private set; }

    /// <summary>
    /// Gets the horizontal line that goes in the lower part of the pad.
    /// </summary>
    public UIView SignatureLine { get; private set; }

    /// <summary>
    /// The caption displayed under the signature line.
    /// </summary>
    public UILabel Caption { get; private set; }

    /// <summary>
    /// The prompt displayed at the beginning of the signature line.
    /// </summary>
    public UILabel SignaturePrompt { get; private set; }

    /// <summary>
    /// Gets the label that clears the pad when clicked.
    /// </summary>
    public UIButton ClearLabel { get; private set; }

    /// <summary>
    ///  Gets the image view that may be used as a watermark or as a texture
    ///  for the signature pad.
    /// </summary>
    public UIImageView BackgroundImageView { get; private set; }

    [Export("StrokeColor"), Browsable(true)]
    public UIColor StrokeColor
    {
        get => SignaturePadCanvas.StrokeColor;
        set => SignaturePadCanvas.StrokeColor = value;
    }

    [Export("StrokeWidth"), Browsable(true)]
    public float StrokeWidth
    {
        get => SignaturePadCanvas.StrokeWidth;
        set => SignaturePadCanvas.StrokeWidth = value;
    }

    /// <summary>
    /// Gets or sets the color of the signature line.
    /// </summary>
    [Export("SignatureLineColor"), Browsable(true)]
    public UIColor SignatureLineColor
    {
        get => SignatureLine.BackgroundColor;
        set => SignatureLine.BackgroundColor = value;
    }

    /// <summary>
    /// Gets or sets the width of the signature line.
    /// </summary>
    [Export("SignatureLineWidth"), Browsable(true)]
    public NFloat SignatureLineWidth
    {
        get => SignatureLine.Bounds.Height;
        set
        {
            var bounds = SignatureLine.Bounds;
            bounds.Height = value;
            SignatureLine.Bounds = bounds;
            SetNeedsLayout();
        }
    }

    /// <summary>
    /// Gets or sets the spacing between the signature line and the caption and prompt.
    /// </summary>
    [Export("SignatureLineSpacing"), Browsable(true)]
    public NFloat SignatureLineSpacing
    {
        get => SignatureLine.LayoutMargins.Bottom;
        set
        {
            var margins = SignatureLine.LayoutMargins;
            margins.Top = value;
            margins.Bottom = value;
            SignatureLine.LayoutMargins = margins;
            SetNeedsLayout();
        }
    }

    /// <summary>
    /// Gets or sets the text for the caption displayed under the signature line.
    /// </summary>
    [Export("CaptionText"), Browsable(true)]
    public string CaptionText
    {
        get => Caption.Text;
        set
        {
            Caption.Text = value;
            SetNeedsLayout();
        }
    }

    /// <summary>
    /// Gets or sets the font size text for the caption displayed under the signature line.
    /// </summary>
    [Export("CaptionFontSize"), Browsable(true)]
    public NFloat CaptionFontSize
    {
        get => Caption.Font.PointSize;
        set
        {
            Caption.Font = Caption.Font.WithSize(value);
            SetNeedsLayout();
        }
    }

    /// <summary>
    /// Gets or sets the text color for the caption displayed under the signature line.
    /// </summary>
    [Export("CaptionTextColor"), Browsable(true)]
    public UIColor CaptionTextColor
    {
        get => Caption.TextColor;
        set => Caption.TextColor = value;
    }

    /// <summary>
    /// Gets or sets the text for the prompt displayed at the beginning of the signature line.
    /// </summary>
    [Export("SignaturePromptText"), Browsable(true)]
    public string SignaturePromptText
    {
        get => SignaturePrompt.Text;
        set
        {
            SignaturePrompt.Text = value;
            SetNeedsLayout();
        }
    }

    /// <summary>
    /// Gets or sets the font size the prompt displayed at the beginning of the signature line.
    /// </summary>
    [Export("SignaturePromptFontSize"), Browsable(true)]
    public NFloat SignaturePromptFontSize
    {
        get => SignaturePrompt.Font.PointSize;
        set
        {
            SignaturePrompt.Font = SignaturePrompt.Font.WithSize(value);
            SetNeedsLayout();
        }
    }

    /// <summary>
    /// Gets or sets the text color for the prompt displayed at the beginning of the signature line.
    /// </summary>
    [Export("SignaturePromptTextColor"), Browsable(true)]
    public UIColor SignaturePromptTextColor
    {
        get => SignaturePrompt.TextColor;
        set => SignaturePrompt.TextColor = value;
    }

    /// <summary>
    /// Gets or sets the text for the label that clears the pad when clicked.
    /// </summary>
    [Export("ClearLabelText"), Browsable(true)]
    public string ClearLabelText
    {
        get => ClearLabel.Title(UIControlState.Normal);
        set
        {
            ClearLabel.SetTitle(value, UIControlState.Normal);
            SetNeedsLayout();
        }
    }

    /// <summary>
    /// Gets or sets the font size the label that clears the pad when clicked.
    /// </summary>
    [Export("ClearLabelFontSize"), Browsable(true)]
    public NFloat ClearLabelFontSize
    {
        get => ClearLabel.TitleLabel.Font.PointSize;
        set
        {
            ClearLabel.TitleLabel.Font = ClearLabel.TitleLabel.Font.WithSize(value);
            SetNeedsLayout();
        }
    }

    /// <summary>
    /// Gets or sets the text color for the label that clears the pad when clicked.
    /// </summary>
    [Export("ClearLabelTextColor"), Browsable(true)]
    public UIColor ClearLabelTextColor
    {
        get => ClearLabel.TitleColor(UIControlState.Normal);
        set => ClearLabel.SetTitleColor(value, UIControlState.Normal);
    }

    [Export("BackgroundImage"), Browsable(true)]
    public UIImage BackgroundImage
    {
        get => BackgroundImageView.Image;
        set => BackgroundImageView.Image = value;
    }

    [Export("BackgroundImageContentMode"), Browsable(true)]
    public UIViewContentMode BackgroundImageContentMode
    {
        get => BackgroundImageView.ContentMode;
        set => BackgroundImageView.ContentMode = value;
    }

    [Export("BackgroundImageAlpha"), Browsable(true)]
    public NFloat BackgroundImageAlpha
    {
        get => BackgroundImageView.Alpha;
        set => BackgroundImageView.Alpha = value;
    }

    [Export("Padding"), Browsable(true)]
    public UIEdgeInsets Padding
    {
        get => padding;
        set
        {
            padding = value;
            SetNeedsLayout();
        }
    }

    private void UpdateUi()
    {
        ClearLabel.Hidden = IsBlank;
    }

    public override void LayoutSubviews()
    {
        var w = Frame.Width;
        var h = Frame.Height;
        var currentY = h;

        SignaturePrompt.SizeToFit();
        ClearLabel.SizeToFit();

        var captionHeight = Caption.SizeThatFits(Caption.Frame.Size).Height;
        var clearButtonHeight = (int)ClearLabel.TitleLabel.Font.LineHeight + 1;

        var rect = new CGRect(0, 0, w, h);
        SignaturePadCanvas.Frame = rect;
        BackgroundImageView.Frame = rect;

        currentY = currentY - Padding.Bottom - captionHeight;
        Caption.Frame = new CGRect(Padding.Left, currentY, w - Padding.Left - Padding.Right, captionHeight);

        currentY = currentY - SignatureLine.LayoutMargins.Bottom - SignatureLine.Frame.Height;
        SignatureLine.Frame = new CGRect(Padding.Left, currentY, w - Padding.Left - Padding.Right, SignatureLine.Frame.Height);

        currentY = currentY - SignatureLine.LayoutMargins.Top - SignaturePrompt.Frame.Height;
        SignaturePrompt.Frame = new CGRect(Padding.Left, currentY, SignaturePrompt.Frame.Width, SignaturePrompt.Frame.Height);

        ClearLabel.Frame = new CGRect(w - Padding.Right - ClearLabel.Frame.Width, Padding.Top, ClearLabel.Frame.Width, clearButtonHeight);
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



