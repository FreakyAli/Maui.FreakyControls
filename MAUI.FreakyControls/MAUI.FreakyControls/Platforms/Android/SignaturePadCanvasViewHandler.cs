using System;
using Microsoft.Maui.Platform;

namespace Maui.FreakyControls
{
    public partial class SignaturePadCanvasViewHandler
    {
        protected override Platforms.Android.SignaturePadCanvasView
            CreatePlatformView() => new Platforms.Android.SignaturePadCanvasView(this.Context);

        private void OnImageStreamRequested(object sender, SignaturePadCanvasView.ImageStreamRequestedEventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                var format = e.ImageFormat;

                var settings = new Platforms.Android.ImageConstructionSettings();
                if (e.Settings.BackgroundColor != null)
                {
                    settings.BackgroundColor = e.Settings.BackgroundColor.ToPlatform();
                }
                if (e.Settings.DesiredSizeOrScale.HasValue)
                {
                    var val = e.Settings.DesiredSizeOrScale.Value;
                    settings.DesiredSizeOrScale = new Platforms.Android.SizeOrScale(val.X, val.Y,
                        (Platforms.Android.SizeOrScaleType)(int)val.Type, val.KeepAspectRatio);
                }
                settings.ShouldCrop = e.Settings.ShouldCrop;
                if (e.Settings.StrokeColor != null)
                {
                    settings.StrokeColor = e.Settings.StrokeColor.ToPlatform();
                }
                settings.StrokeWidth = e.Settings.StrokeWidth;
                settings.Padding = e.Settings.Padding;

                e.ImageStreamTask = ctrl.GetImageStreamAsync(format, settings);
            }
        }
    }
}

