using System;
using CoreGraphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace Maui.FreakyControls.Platforms.iOS.NativeControls;

public class FreakyCircularUIImageView : MauiImageView
{
    public override CGRect Frame
    {
        get => base.Frame;
        set
        {
            base.Frame = value;
            this.MakeCircular();
        }
    }

    public override void LayoutSubviews()
    {
        base.LayoutSubviews();
        this.MakeCircular();
    }
}