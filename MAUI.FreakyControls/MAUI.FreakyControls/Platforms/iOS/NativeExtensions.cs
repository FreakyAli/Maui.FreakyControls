using System;
using UIKit;
using Foundation;
using CoreGraphics;
using CoreAnimation;
using System.Runtime.InteropServices;

namespace MAUI.FreakyControls.Platforms.iOS
{
    public static class NativeExtensions
    {
        public static CALayer BottomLineDrawer(UIColor color)
        {
            var border = new CALayer();
            var width = NFloat.Parse("1.0");
            border.BorderColor = color.CGColor;
            border.BorderWidth = width;
            return border;
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
    }
}

