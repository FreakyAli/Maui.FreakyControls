using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Windows.Input;
using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui;
#if ANDROID
using AndroidExtensions = SkiaSharp.Views.Android.AndroidExtensions;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using static Microsoft.Maui.ApplicationModel.Platform;
using NativeImage = Android.Graphics.Bitmap;
using PlatformTouchEffects = Maui.FreakyControls.Platforms.Android.Effects.TouchEffect;
#endif
#if IOS
using SkiaSharp.Views.iOS;
using PlatformTouchEffects = Maui.FreakyControls.Platforms.iOS.Effects.TouchEffect;
using Maui.FreakyControls.Platforms.iOS;
#endif
#if MACCATALYST
using PlatformTouchEffects = Maui.FreakyControls.Platforms.MacCatalyst.TouchEffect;
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
            handlers.AddHandler(typeof(FreakySignatureCanvasView), typeof(FreakySignatureCanvasViewHandler));
        }

        public static void InitFreakyEffects(this IEffectsBuilder effects)
        {
            effects.Add<Effects.TouchEffect, PlatformTouchEffects>();
        }

        /// <summary>
        /// Forced to do this for this issue : https://github.com/mono/SkiaSharp/issues/1979
        /// </summary>
        /// <param name="mauiAppBuilder"></param>
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

//        public static async Task<SKBitmap> GetSKBitmapAsync(this ImageSource imageSource)
//        {
//            var nativeImage = await imageSource.ToNativeImageSourceAsync();
//#if ANDROID
//            var skImage = AndroidExtensions.ToSKBitmap(nativeImage);
//            SKBitmapImageSource sKBitmapImage = ;
//            return skImage;
//#endif
//#if IOS
//            var skImage = iOSExtensions.ToSKBitmap(nativeImage);
//            return skImage;
//#endif
//#if MACCATALYST
//            return new SKBitmap();
//#endif

//        }

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

