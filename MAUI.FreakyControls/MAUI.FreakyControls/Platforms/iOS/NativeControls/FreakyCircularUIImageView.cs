using CoreGraphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Maui.FreakyControls.Platforms.iOS.NativeControls;

public class FreakyCircularUIImageView : MauiImageView
{
    public FreakyCircularUIImageView(IImageHandler handler) : base(handler)
    {
    }

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