using System;
using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;
#if ANDROID
using NativeColor = Android.Graphics.Color;
#endif
#if IOS
using MAUI.FreakyControls.Platforms.iOS;
using NativeColor = UIKit.UIColor;
#endif

namespace MAUI.FreakyControls
{
    public static class Extensions
    {
        public static void AddFreakyHandlers(this IMauiHandlersCollection handlers)
        {
            handlers.AddHandler(typeof(IFreakyEditor), typeof(FreakyEditorHandler));
        }

        /// <summary>
        /// Get native color from 
        /// </summary>
        /// <param name="color"></param>
        /// <returns>native Color</returns>
        public static NativeColor ToNativeColor(this Color color)
        {
            var hexCode = color.ToHex();
            NativeColor nativeColor;
#if ANDROID
            nativeColor=  Android.Graphics.Color.ParseColor(hexCode);
#endif
#if IOS
            nativeColor = UIKit.UIColor.Clear.FromHex(hexCode);
#endif
            return nativeColor;
        }
    }
}

