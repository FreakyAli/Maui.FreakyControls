using System;
using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;
using System.Reflection;
using System.Xml;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Maui.FreakyControls.Shared;
using System.Windows.Input;
#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using static Microsoft.Maui.ApplicationModel.Platform;
using NativeImage = Android.Graphics.Bitmap;
#endif
#if IOS
using Maui.FreakyControls.Platforms.iOS;
#endif
#if IOS || MACCATALYST
using NativeImage = UIKit.UIImage;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#endif

namespace Maui.FreakyControls.Extensions
{
    public static class Extensions
    {

        public static void ExecuteCommandIfAvailable(this ICommand command, object parameter = null)
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
            handlers.AddHandler(typeof(FreakyCircularImage), typeof(FreakyCircularImageHandler));
            handlers.AddHandler(typeof(FreakyButton), typeof(FreakyButtonHandler));
            handlers.AddHandler(typeof(FreakyDatePicker), typeof(FreakyDatePickerHandler));
            handlers.AddHandler(typeof(FreakyTimePicker), typeof(FreakyTimePickerHandler));
            handlers.AddHandler(typeof(FreakyPicker), typeof(FreakyPickerHandler));
            handlers.AddHandler(typeof(FreakyImage), typeof(FreakyImageHandler));
            handlers.AddHandler(typeof(SignaturePadCanvasView), typeof(SignaturePadCanvasViewHandler));
        }

        public static void InitSkiaSharp(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.UseSkiaSharp();
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
#if IOS || MACCATALYST
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

