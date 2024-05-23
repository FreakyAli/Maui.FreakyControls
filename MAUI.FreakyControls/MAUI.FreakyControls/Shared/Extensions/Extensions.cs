using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Windows.Input;
using Maui.FreakyEffects;
using Maui.FreakyControls.Dotnet;

#if MACCATALYST
using Maui.FreakyControls.Platforms.MacCatalyst;
#endif
#if WINDOWS
using Maui.FreakyControls.Platforms.Windows;
#endif
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

namespace Maui.FreakyControls.Extensions;

public static class Extensions
{
    public static bool IsAndroid => DeviceInfo.Current.Platform == DevicePlatform.Android;

    public static bool IsiOS => DeviceInfo.Current.Platform == DevicePlatform.iOS;

    public static void ExecuteCommandIfAvailable(this ICommand command, object parameter = null)
    {
        if (command?.CanExecute(parameter) == true)
        {
            command?.Execute(parameter);
        }
    }

    public static MauiAppBuilder InitializeFreakyControls(this MauiAppBuilder builder, bool useSkiaSharp = true, bool useFreakyEffects = true)
    {
        if (useSkiaSharp)
        {
            builder.UseSkiaSharp();
        }
        builder.ConfigureMauiHandlers(builders => builders.AddHandlers());
        builder.ConfigureEffects(effects =>
        {
            if (useFreakyEffects)
            {
                effects.InitFreakyEffects();
            }
            effects.AddEffects();
        });
        return builder;
    }

    private static void AddEffects(this IEffectsBuilder effects)
    {
    }

    private static void AddHandlers(this IMauiHandlersCollection handlers)
    {
        handlers.AddHandler(typeof(FreakyEditor), typeof(FreakyEditorHandler));
        handlers.AddHandler(typeof(FreakyEntry), typeof(FreakyEntryHandler));
        handlers.AddHandler(typeof(FreakyCircularImage), typeof(FreakyCircularImageHandler));
        handlers.AddHandler(typeof(FreakyDatePicker), typeof(FreakyDatePickerHandler));
        handlers.AddHandler(typeof(FreakyTimePicker), typeof(FreakyTimePickerHandler));
        handlers.AddHandler(typeof(FreakyPicker), typeof(FreakyPickerHandler));
        handlers.AddHandler(typeof(FreakyImage), typeof(FreakyImageHandler));
        handlers.AddHandler(typeof(FreakySignatureCanvasView), typeof(FreakySignatureCanvasViewHandler));
        handlers.AddHandler(typeof(FreakyAutoCompleteView), typeof(FreakyAutoCompleteViewHandler));
    }

#if ANDROID || IOS || MACCATALYST

    /// <summary>
    /// Get native <see cref="NativeImage"/> from Maui <see cref="ImageSource"/>
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
        //ImageSource handler to return
        IImageSourceHandler returnValue = null;
        //check the specific source type and return the correct ImageSource handler
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

#endif
}