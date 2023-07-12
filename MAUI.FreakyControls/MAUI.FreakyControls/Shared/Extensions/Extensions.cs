using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Windows.Input;
using TouchAndPressEffect = Maui.FreakyControls.TouchPress.TouchAndPressEffect;
using TouchReleaseEffect = Maui.FreakyControls.TouchPress.TouchReleaseEffect;

#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using static Microsoft.Maui.ApplicationModel.Platform;
using NativeImage = Android.Graphics.Bitmap;
using PlatformTouchAndPressEffect = Maui.FreakyControls.Platforms.Android.TouchAndPressEffect;
using PlatformTouchReleaseEffect = Maui.FreakyControls.Platforms.Android.TouchReleaseEffect;
#endif
#if IOS
using Maui.FreakyControls.Platforms.iOS;
using PlatformTouchAndPressEffect = Maui.FreakyControls.Platforms.iOS.TouchAndPressEffect;
using PlatformTouchReleaseEffect = Maui.FreakyControls.Platforms.iOS.TouchReleaseEffect;
#endif
#if IOS || MACCATALYST
using NativeImage = UIKit.UIImage;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#endif

namespace Maui.FreakyControls.Extensions;

public static class Extensions
{
    public static void ExecuteCommandIfAvailable(this ICommand command, object parameter = null)
    {
        if (command?.CanExecute(parameter) == true)
        {
            command?.Execute(parameter);
        }
    }

    public static void InitializeFreakyControls(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(builders => builders.AddFreakyHandlers());
        builder.ConfigureEffects(effects => {

#if Android || IOS
            effects.Add<TouchAndPressEffect, PlatformTouchAndPressEffect>();
            effects.Add<TouchReleaseEffect, PlatformTouchReleaseEffect>();
#endif
        });
    }

    [Obsolete("Please use InitializeFreakyControls instead.")]
    public static void AddFreakyHandlers(this IMauiHandlersCollection handlers)
    {
        handlers.AddHandler(typeof(FreakyEditor), typeof(FreakyEditorHandler));
        handlers.AddHandler(typeof(FreakyEntry), typeof(FreakyEntryHandler));
        handlers.AddHandler(typeof(FreakyCircularImage), typeof(FreakyCircularImageHandler));
        //handlers.AddHandler(typeof(MaterialButton), typeof(FreakyButtonHandler));
        handlers.AddHandler(typeof(FreakyDatePicker), typeof(FreakyDatePickerHandler));
        handlers.AddHandler(typeof(FreakyTimePicker), typeof(FreakyTimePickerHandler));
        handlers.AddHandler(typeof(FreakyPicker), typeof(FreakyPickerHandler));
        handlers.AddHandler(typeof(FreakyImage), typeof(FreakyImageHandler));
        handlers.AddHandler(typeof(FreakySignatureCanvasView), typeof(FreakySignatureCanvasViewHandler));
    }

    public static void InitSkiaSharp(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.UseSkiaSharp();
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