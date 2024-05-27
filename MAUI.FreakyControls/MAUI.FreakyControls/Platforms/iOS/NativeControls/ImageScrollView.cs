using CoreFoundation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Maui.FreakyControls.Platforms.iOS.NativeControls;

public interface IImageScrollViewDelegate : IUIScrollViewDelegate
{
    void ImageScrollViewDidChangeOrientation(ImageScrollView imageScrollView);
}

public enum ScaleMode
{
    AspectFill,
    AspectFit,
    WidthFill,
    HeightFill
}

public enum Offset
{
    Begining,
    Center
}

public class ImageScrollView : UIScrollView, IImageScrollViewDelegate
{
    public static nfloat KZoomInFactorFromMinWhenDoubleTap { get; } = 2;

    public ScaleMode ImageContentMode { get; set; } = ScaleMode.WidthFill;
    public Offset InitialOffset { get; set; } = Offset.Begining;
    public UIImageView ZoomView { get; private set; } = null;

    public IImageScrollViewDelegate ImageScrollViewDelegate { get; set; }

    public CGSize ImageSize { get; private set; } = CGSize.Empty;
    private CGPoint pointToCenterAfterResize = CGPoint.Empty;
    private nfloat scaleToRestoreAfterResize = 1.0f;
    public nfloat MaxScaleFromMinScale { get; set; } = 3.0f;


    public override CGRect Frame
    {
        get { return base.Frame; }
        set
        {
            if (!base.Frame.Equals(value) && !value.Equals(CGRect.Empty) && !ImageSize.Equals(CGSize.Empty))
                PrepareToResize();

            base.Frame = value;

            if (!base.Frame.Equals(value) && !base.Frame.Equals(CGRect.Empty) && !ImageSize.Equals(CGSize.Empty))
                RecoverFromResizing();
        }
    }

    public ImageScrollView(CGRect frame) : base(frame)
    {
        Initialize();
    }

    public ImageScrollView(NSCoder aDecoder) : base(aDecoder)
    {
        Initialize();
    }

    ~ImageScrollView()
    {
        NSNotificationCenter.DefaultCenter.RemoveObserver(this);
    }

    private void Initialize()
    {
        ShowsVerticalScrollIndicator = false;
        ShowsHorizontalScrollIndicator = false;
        BouncesZoom = true;
        DecelerationRate = UIScrollView.DecelerationRateFast;
        Delegate = this;

        NSNotificationCenter.DefaultCenter.AddObserver(
            UIDevice.OrientationDidChangeNotification, ChangeOrientationNotification, null);
    }

    public void AdjustFrameToCenter()
    {
        if (ZoomView == null)
            return;

        var unwrappedZoomView = ZoomView;
        var frameToCenter = unwrappedZoomView.Frame;

        // Center horizontally
        if (frameToCenter.Width < Bounds.Width)
        {
            frameToCenter.X = (Bounds.Width - frameToCenter.Width) / 2;
        }
        else
        {
            frameToCenter.X = 0;
        }

        // Center vertically
        if (frameToCenter.Height < Bounds.Height)
        {
            frameToCenter.Y = (Bounds.Height - frameToCenter.Height) / 2;
        }
        else
        {
            frameToCenter.Y = 0;
        }

        unwrappedZoomView.Frame = frameToCenter;
    }

    private void PrepareToResize()
    {
        CGPoint boundsCenter = new CGPoint(Bounds.GetMidX(), Bounds.GetMidY());
        pointToCenterAfterResize = ConvertPointToView(boundsCenter, ZoomView);

        scaleToRestoreAfterResize = ZoomScale;

        // If we're at the minimum zoom scale, preserve that by returning 0, which will be converted to the minimum
        // allowable scale when the scale is restored.
        if (scaleToRestoreAfterResize <= MinimumZoomScale + (nfloat)float.Epsilon)
        {
            scaleToRestoreAfterResize = 0;
        }
    }

    private void RecoverFromResizing()
    {
        SetMaxMinZoomScalesForCurrentBounds();

        // Restore zoom scale, first making sure it is within the allowable range.
        var maxZoomScale = Math.Max(MinimumZoomScale, scaleToRestoreAfterResize);
        ZoomScale = (nfloat)Math.Min(MaximumZoomScale, maxZoomScale);

        // Restore center point, first making sure it is within the allowable range.

        // Convert our desired center point back to our own coordinate space
        CGPoint boundsCenter = ConvertPointToView(pointToCenterAfterResize, ZoomView);

        // Calculate the content offset that would yield that center point
        CGPoint offset = new CGPoint(boundsCenter.X - Bounds.Width / 2.0, boundsCenter.Y - Bounds.Height / 2.0);

        // Restore offset, adjusted to be within the allowable range
        CGPoint maxOffset = MaximumContentOffset();
        CGPoint minOffset = MinimumContentOffset();

        var realMaxOffsetX = Math.Min(maxOffset.X, offset.X);
        offset.X = (nfloat)Math.Max(minOffset.X, realMaxOffsetX);

        var realMaxOffsetY = Math.Min(maxOffset.Y, offset.Y);
        offset.Y = (nfloat)Math.Max(minOffset.Y, realMaxOffsetY);

        ContentOffset = offset;
    }

    private CGPoint MaximumContentOffset()
    {
        return new CGPoint(ContentSize.Width - Bounds.Width, ContentSize.Height - Bounds.Height);
    }

    private CGPoint MinimumContentOffset()
    {
        return CGPoint.Empty;
    }

    public void Setup()
    {
        UIView topSuperview = Superview;

        while (topSuperview.Superview != null)
        {
            topSuperview = topSuperview.Superview;
        }

        // Make sure views have already layout with precise frame
        topSuperview.LayoutIfNeeded();

        DispatchQueue.MainQueue.DispatchAsync(() =>
        {
            Refresh();
        });
    }

    public void DisplayImage(UIImage image)
    {
        if (ZoomView != null)
        {
            ZoomView.RemoveFromSuperview();
        }

        ZoomView = new UIImageView(image);
        ZoomView.UserInteractionEnabled = true;
        AddSubview(ZoomView);

        UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(DoubleTapGestureRecognizer);
        tapGesture.NumberOfTapsRequired = 2;
        ZoomView.AddGestureRecognizer(tapGesture);

        ConfigureImageForSize(image.Size);
    }

    private void ConfigureImageForSize(CGSize size)
    {
        ImageSize = size;
        ContentSize = ImageSize;
        SetMaxMinZoomScalesForCurrentBounds();
        ZoomScale = MinimumZoomScale;

        switch (InitialOffset)
        {
            case Offset.Begining:
                ContentOffset = CGPoint.Empty;
                break;
            case Offset.Center:
                nfloat xOffset = ContentSize.Width < Bounds.Width ? 0 : (ContentSize.Width - Bounds.Width) / 2;
                nfloat yOffset = ContentSize.Height < Bounds.Height ? 0 : (ContentSize.Height - Bounds.Height) / 2;

                switch (ImageContentMode)
                {
                    case ScaleMode.AspectFit:
                        ContentOffset = CGPoint.Empty;
                        break;
                    case ScaleMode.AspectFill:
                        ContentOffset = new CGPoint(xOffset, yOffset);
                        break;
                    case ScaleMode.HeightFill:
                        ContentOffset = new CGPoint(xOffset, 0);
                        break;
                    case ScaleMode.WidthFill:
                        ContentOffset = new CGPoint(0, yOffset);
                        break;
                }
                break;
        }
    }

    private void SetMaxMinZoomScalesForCurrentBounds()
    {
        // Calculate min/max zoomscale
        nfloat xScale = Bounds.Width / ImageSize.Width;    // the scale needed to perfectly fit the image width-wise
        nfloat yScale = Bounds.Height / ImageSize.Height;   // the scale needed to perfectly fit the image height-wise

        double minScale = 1;

        switch (ImageContentMode)
        {
            case ScaleMode.AspectFill:
                minScale = Math.Max(xScale, yScale);
                break;
            case ScaleMode.AspectFit:
                minScale = Math.Min(xScale, yScale);
                break;
            case ScaleMode.WidthFill:
                minScale = xScale;
                break;
            case ScaleMode.HeightFill:
                minScale = yScale;
                break;
        }

        nfloat maxScale = (nfloat)(MaxScaleFromMinScale * minScale);

        // Don't let minScale exceed maxScale. (If the image is smaller than the screen, we don't want to force it to be zoomed.)
        if (minScale > maxScale)
        {
            minScale = maxScale;
        }

        MaximumZoomScale = maxScale;
        MinimumZoomScale = (nfloat)(minScale * 0.999f); // the multiply factor to prevent user cannot scroll page while they use this control in UIPageViewController
    }

    public void DoubleTapGestureRecognizer(UIGestureRecognizer gestureRecognizer)
    {
        // Zoom out if it's bigger than the scale factor after double-tap scaling. Else, zoom in
        if (ZoomScale >= MinimumZoomScale * KZoomInFactorFromMinWhenDoubleTap - 0.01f)
        {
            SetZoomScale(MinimumZoomScale, true);
        }
        else
        {
            var center = gestureRecognizer.LocationInView(gestureRecognizer.View);
            var zoomRect = ZoomRectForScale(KZoomInFactorFromMinWhenDoubleTap * MinimumZoomScale, center);
            ZoomToRect(zoomRect, true);
        }
    }

    private CGRect ZoomRectForScale(nfloat scale, CGPoint center)
    {
        CGRect zoomRect = CGRect.Empty;

        // The zoom rect is in the content view's coordinates.
        // At a zoom scale of 1.0, it would be the size of the imageScrollView's bounds.
        // As the zoom scale decreases, so more content is visible, the size of the rect grows.
        zoomRect.Size = new CGSize(Frame.Size.Width / scale, Frame.Size.Height / scale);

        // Choose an origin so as to get the right center.
        zoomRect.X = center.X - (zoomRect.Size.Width / 2.0f);
        zoomRect.Y = center.Y - (zoomRect.Size.Height / 2.0f);

        return zoomRect;
    }

    public void Refresh()
    {
        if (ZoomView?.Image != null)
        {
            DisplayImage(ZoomView.Image);
        }
    }

    public void ChangeOrientationNotification(NSNotification notification)
    {
        // A weird bug that frames are not update right after orientation changed. Need delay a little bit with async.
        DispatchQueue.MainQueue.DispatchAsync(() =>
        {
            ConfigureImageForSize(ImageSize);
            ImageScrollViewDelegate?.ImageScrollViewDidChangeOrientation(this);
        });
    }

    public void ImageScrollViewDidChangeOrientation(ImageScrollView imageScrollView)
    {
    }
}