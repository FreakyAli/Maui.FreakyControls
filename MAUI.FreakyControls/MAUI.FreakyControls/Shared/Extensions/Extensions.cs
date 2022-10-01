using System;
using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;
using System.Reflection;
using System.Xml;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Maui.FreakyControls.Shared;
using System.Windows.Input;
using Microsoft.Maui.Controls.Compatibility.Hosting;
#if ANDROID
using SupportAutoCompleteRenderer = Maui.FreakyControls.Platforms.Android.SupportAutoCompleteRenderer;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using static Microsoft.Maui.ApplicationModel.Platform;
using NativeColor = Android.Graphics.Color;
using NativeImage = Android.Graphics.Bitmap;
#endif
#if IOS
using SupportAutoCompleteRenderer = Maui.FreakyControls.Platforms.iOS.SupportAutoCompleteRenderer;
using Maui.FreakyControls.Platforms.iOS;
using NativeImage = UIKit.UIImage;
using NativeColor = UIKit.UIColor;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#endif

namespace Maui.FreakyControls.Extensions
{
    public static class Extensions
    {
        public static void ExecuteIfAvailable(this ICommand command, object parameter = null)
        {
            if (command?.CanExecute(parameter) == true)
            {
                command.Execute(parameter);
            }
        }

        public static void AddFreakyHandlers(this IMauiHandlersCollection handlers)
        {   
            handlers.AddHandler(typeof(FreakyEditor), typeof(FreakyEditorHandler));
            handlers.AddHandler(typeof(FreakyEntry), typeof(FreakyEntryHandler));
            handlers.AddHandler(typeof(FreakySvgImageView), typeof(FreakySvgImageViewHandler));
            handlers.AddHandler(typeof(FreakyTextInputLayout), typeof(FreakyTextInputLayoutHandler));
            //handlers.AddHandler(typeof(FreakyAutoCompleteView),typeof(FreakyAutoCompleteViewHandler));
            handlers.AddHandler(typeof(FreakyCircularImage),typeof(FreakyCircularImageHandler));
            handlers.AddHandler(typeof(SupportAutoComplete), typeof(SupportAutoCompleteRenderer));

        }

        public static void InitSkiaSharp(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.UseSkiaSharp();
            mauiAppBuilder.UseMauiCompatibility();
        }

        /// <summary>
        /// Get native color from Maui graphics Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns>native Color</returns>
        public static NativeColor ToNativeColor(this Color color)
        {
            var hexCode = color.ToHex();
            NativeColor nativeColor;
#if ANDROID
            nativeColor = Android.Graphics.Color.ParseColor(hexCode);
#endif
#if IOS
            nativeColor = UIKit.UIColor.Clear.FromHex(hexCode);
#endif
            return nativeColor;
        }

        /// <summary>
        /// Get native imagesource from Maui imagesource 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static async Task<NativeImage> ToNativeImageSourceAsync(this ImageSource source)
        {
            var handler = GetHandler(source);
            var returnValue = (NativeImage)null;
#if IOS
            returnValue = await handler.LoadImageAsync(source);
#endif
#if ANDROID
            returnValue = await handler.LoadImageAsync(source, CurrentActivity);
#endif
            return returnValue;
        }

        private static IImageSourceHandler GetHandler(this ImageSource source)
        {
            //Image source handler to return 
            IImageSourceHandler returnValue = null;
            //check the specific source type and return the correct image source handler 
            switch (source)
            {
                case UriImageSource:
                    returnValue = new ImageLoaderSourceHandler();
                    break;
                case FileImageSource:
                    returnValue = new FileImageSourceHandler();
                    break;
                case StreamImageSource:
                    returnValue = new StreamImagesourceHandler();
                    break;
                case FontImageSource:
                    returnValue = new FontImageSourceHandler();
                    break;
            }
            return returnValue;
        }
    }
}

