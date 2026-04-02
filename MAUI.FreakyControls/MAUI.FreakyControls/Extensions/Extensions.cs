using SkiaSharp.Views.Maui.Controls.Hosting;
using Maui.FreakyEffects;
using Maui.FreakyControls.Dotnet;

#if WINDOWS
using Maui.FreakyControls.Platforms.Windows;
#endif
#if ANDROID
using static Microsoft.Maui.ApplicationModel.Platform;
using NativeImage = Android.Graphics.Bitmap;
#endif
#if IOS || MACCATALYST
using Maui.FreakyControls.Platforms.Apple;
using NativeImage = UIKit.UIImage;
#endif

namespace Maui.FreakyControls.Extensions;

public static class Extensions
{
    public static bool IsAndroid => DeviceInfo.Current.Platform == DevicePlatform.Android;

    public static bool IsiOS => DeviceInfo.Current.Platform == DevicePlatform.iOS;

    public static double DipsToPixels(this double dip)
    {
        return dip * DeviceDisplay.MainDisplayInfo.Density;
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
    public static async Task<NativeImage> ToNativeImageSourceAsync(this ImageSource source)
    {
        var services = IPlatformApplication.Current!.Services;
        var provider = services.GetRequiredService<IImageSourceServiceProvider>();
        var service = provider.GetImageSourceService(source);

#if IOS || MACCATALYST
        var result = await service.GetImageAsync(source);
        return result?.Value;
#endif
#if ANDROID
        var result = await service.GetDrawableAsync(source, CurrentActivity ?? Android.App.Application.Context);
        if (result?.Value is Android.Graphics.Drawables.BitmapDrawable bitmapDrawable)
            return bitmapDrawable.Bitmap;
        var drawable = result?.Value;
        if (drawable == null)
            return null;
        var bitmap = NativeImage.CreateBitmap(
            Math.Max(drawable.IntrinsicWidth, 1),
            Math.Max(drawable.IntrinsicHeight, 1),
            NativeImage.Config.Argb8888);
        var canvas = new Android.Graphics.Canvas(bitmap);
        drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
        drawable.Draw(canvas);
        return bitmap;
#endif
    }

#endif
}
