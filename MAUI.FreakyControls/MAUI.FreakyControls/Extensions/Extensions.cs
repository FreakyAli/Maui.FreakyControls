using SkiaSharp.Views.Maui.Controls.Hosting;
using Maui.FreakyEffects;
#if WINDOWS
using Maui.FreakyControls.Platforms.Windows;
#endif
#if ANDROID
using NativeImage = Android.Graphics.Bitmap;
#endif
#if IOS || MACCATALYST
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
        handlers.AddHandler<FreakyEditor, FreakyEditorHandler>();
        handlers.AddHandler<FreakyEntry, FreakyEntryHandler>();
        handlers.AddHandler<FreakyCircularImage, FreakyCircularImageHandler>();
        handlers.AddHandler<FreakyDatePicker, FreakyDatePickerHandler>();
        handlers.AddHandler<FreakyTimePicker, FreakyTimePickerHandler>();
        handlers.AddHandler<FreakyPicker, FreakyPickerHandler>();
        handlers.AddHandler<FreakyImage, FreakyImageHandler>();
#if ANDROID || IOS || MACCATALYST || WINDOWS
        handlers.AddHandler<FreakySignatureCanvasView, FreakySignatureCanvasViewHandler>();
        handlers.AddHandler<FreakyAutoCompleteView, FreakyAutoCompleteViewHandler>();
#endif
    }

#if ANDROID || IOS || MACCATALYST

    /// <summary>
    /// Get native <see cref="NativeImage"/> from Maui <see cref="ImageSource"/>
    /// </summary>
    public static async Task<NativeImage?> ToNativeImageSourceAsync(this ImageSource source)
    {
        var provider = IPlatformApplication.Current.Services.GetRequiredService<IImageSourceServiceProvider>();
        var service = provider.GetImageSourceService(source);
#if IOS || MACCATALYST
        var result = await service.GetImageAsync(source);
        return result?.Value;
#elif ANDROID
        var result = await service.GetDrawableAsync(source, Android.App.Application.Context);
        var drawable = result?.Value;
        if (drawable is null) return null;
        var w = drawable.IntrinsicWidth > 0 ? drawable.IntrinsicWidth : 1;
        var h = drawable.IntrinsicHeight > 0 ? drawable.IntrinsicHeight : 1;
        var bitmap = Android.Graphics.Bitmap.CreateBitmap(w, h, Android.Graphics.Bitmap.Config.Argb8888);
        using var canvas = new Android.Graphics.Canvas(bitmap);
        drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
        drawable.Draw(canvas);
        return bitmap;
#endif
    }

#endif
}