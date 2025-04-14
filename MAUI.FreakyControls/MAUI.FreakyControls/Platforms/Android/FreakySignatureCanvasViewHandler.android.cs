﻿using Maui.FreakyControls.Enums;
using Microsoft.Maui.Platform;

namespace Maui.FreakyControls
{
    public partial class FreakySignatureCanvasViewHandler
    {
        protected override Platforms.Android.SignaturePadCanvasView
            CreatePlatformView() => new(this.Context);

        private void OnImageStreamRequested(object sender, ImageStreamRequestedEventArgs e)
        {
            var ctrl = PlatformView;
            if (ctrl is not null)
            {
                var format = e.ImageFormat;

                var settings = new Platforms.Android.ImageConstructionSettings();
                if (e.Settings.BackgroundColor is not null)
                {
                    settings.BackgroundColor = e.Settings.BackgroundColor.ToPlatform();
                }
                if (e.Settings.DesiredSizeOrScale.HasValue)
                {
                    var val = e.Settings.DesiredSizeOrScale.Value;
                    settings.DesiredSizeOrScale = new Platforms.Android.SizeOrScale(val.X, val.Y,
                        (SizeOrScaleType)(int)val.Type, val.KeepAspectRatio);
                }
                settings.ShouldCrop = e.Settings.ShouldCrop;
                if (e.Settings.StrokeColor is not null)
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