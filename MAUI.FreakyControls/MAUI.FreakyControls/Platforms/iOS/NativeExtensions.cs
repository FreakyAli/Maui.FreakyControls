using CoreGraphics;
using System.Drawing;
using System.Runtime.InteropServices;
using UIKit;

namespace Maui.FreakyControls.Platforms.iOS;

public static class NativeExtensions
{
    internal static UIView UiImageToUiView(this UIImage image, int height, int width, int padding)
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

    public static UIColor FromHex(this UIColor color, string hex)
    {
        int r = 0, g = 0, b = 0, a = 0;

        if (hex.Contains("#"))
            hex = hex.Replace("#", "");

        switch (hex.Length)
        {
            case 2:
                r = int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
                g = int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
                b = int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
                a = 255;
                break;

            case 3:
                r = int.Parse(hex.Substring(0, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                g = int.Parse(hex.Substring(1, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                b = int.Parse(hex.Substring(2, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                a = 255;
                break;

            case 4:
                r = int.Parse(hex.Substring(0, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                g = int.Parse(hex.Substring(1, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                b = int.Parse(hex.Substring(2, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                a = int.Parse(hex.Substring(3, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
                break;

            case 6:
                r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                a = 255;
                break;

            case 8:
                r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                a = int.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                break;
        }
        return UIColor.FromRGBA(r, g, b, a);
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
