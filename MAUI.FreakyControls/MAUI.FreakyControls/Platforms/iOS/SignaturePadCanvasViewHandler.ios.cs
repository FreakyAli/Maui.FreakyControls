using System;
using Microsoft.Maui.Platform;

namespace Maui.FreakyControls
{
    public partial class SignaturePadCanvasViewHandler
    {
        protected override Platforms.iOS.SignaturePadCanvasView
            CreatePlatformView() => new();

        private void OnImageStreamRequested(object sender, SignaturePadCanvasView.ImageStreamRequestedEventArgs e)
        {
            var ctrl = this.PlatformView;
            if (ctrl != null)
            {
                var format = e.ImageFormat;

                var settings = new Platforms.iOS.ImageConstructionSettings();
                if (e.Settings.BackgroundColor != null)
                {
                    settings.BackgroundColor = e.Settings.BackgroundColor.ToPlatform();
                }
                if (e.Settings.DesiredSizeOrScale.HasValue)
                {
                    var val = e.Settings.DesiredSizeOrScale.Value;
                    settings.DesiredSizeOrScale = new Platforms.iOS.SizeOrScale(val.X, val.Y,
                        (Platforms.iOS.SizeOrScaleType)(int)val.Type, val.KeepAspectRatio);
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

