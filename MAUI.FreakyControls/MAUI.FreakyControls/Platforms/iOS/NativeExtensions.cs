using System;
using UIKit;
using Foundation;
using CoreGraphics;
using CoreAnimation;
using System.Runtime.InteropServices;
using System.Drawing;
using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Shared.Enums;
using Maui.FreakyControls.Shared;

namespace Maui.FreakyControls.Platforms.iOS;

public static class NativeExtensions
{
    internal static UIView UIImageToUIView(this UIImage image, int height, int width, int padding)
    {
        var uiImageView = new UIImageView(image)
        {
            Frame = new RectangleF(0, 0, height, width)
        };
        UIView uiView = new UIView(new System.Drawing.Rectangle(0, 0, width + padding, height));
        uiView.AddSubview(uiImageView);
        return uiView;
    }

    public static void MakeCircular(this UIView uIView)
    {
        uIView.ClipsToBounds = true;
        uIView.Layer.CornerRadius = (uIView.Frame.Width + uIView.Frame.Height) / 4;
    }

    public static CGSize GetSize(this UIImage image)
    {
        return image.Size;
    }

    public static void Invalidate(this UIView view)
    {
        view.SetNeedsDisplay();
    }

    public static void MoveTo(this UIBezierPath path, NFloat x, NFloat y)
    {
        path.MoveTo(new CGPoint(x, y));
    }

    public static void LineTo(this UIBezierPath path, NFloat x, NFloat y)
    {
        path.AddLineTo(new CGPoint(x, y));
    }

    public static CGSize GetSize(this UIView view)
    {
        return view.Bounds.Size;
    }
}