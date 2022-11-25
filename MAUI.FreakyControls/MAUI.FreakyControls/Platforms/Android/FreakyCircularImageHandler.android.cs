using System;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Maui.FreakyControls.Platforms.Android.NativeControls;

namespace Maui.FreakyControls;

public partial class FreakyCircularImageHandler
{
    protected override ImageView CreatePlatformView()
    {
        var imageView = new CircularImageView(Context);

        // Enable view bounds adjustment on measure.
        // This allows the ImageView's OnMeasure method to account for the image's intrinsic
        // aspect ratio during measurement, which gives us more useful values during constrained
        // measurement passes.
        imageView.SetAdjustViewBounds(true);

        return imageView;
    }
}