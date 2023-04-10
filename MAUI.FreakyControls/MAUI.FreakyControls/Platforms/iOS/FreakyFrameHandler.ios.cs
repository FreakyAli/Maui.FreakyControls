using System.ComponentModel;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using UIKit;

namespace Maui.FreakyControls;

public partial class FreakyFrameHandler
{
    UILongPressGestureRecognizer pressGestureRecognizer;

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        //// Fix Xamarin.Forms Frame BackgroundColor Bug (https://github.com/xamarin/Xamarin.Forms/issues/2218)
        if (e.PropertyName == nameof(this.Element.BackgroundColor))
            this.Layer.BackgroundColor = Element.BackgroundColor.ToPlatform().CGColor;
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
    {
        base.OnElementChanged(e);

        AccessibilityTraits = UIAccessibilityTrait.Button;

        if (e.OldElement == null)
        {
            // Fix Xamarin.Forms Frame BackgroundColor Bug (https://github.com/xamarin/Xamarin.Forms/issues/2218)
            this.Layer.BackgroundColor = e.NewElement.BackgroundColor.ToPlatform().CGColor;

            // FIX: This fixes another Xamarin.Forms bug introduced in Xamarin.Forms 4.7, that messes with corner radius
            // in iOS. To find out, of the bug has been resolved, just take out the following lines and check, if the border
            // radiusses render correctly.
            // Bug: https://github.com/xamarin/Xamarin.Forms/issues/2405 and https://github.com/xamarin/Xamarin.Forms/issues/7823
            if (e.NewElement != null)
            {
                this.Layer.CornerRadius = e.NewElement.CornerRadius;
                this.ClipsToBounds = false;
            }
            // END FIX

            if (!e.NewElement.GestureRecognizers.Any())
                return;

            if (!e.NewElement.GestureRecognizers.Any(x => x.GetType() == typeof(TouchGestureRecognizer)))
                return;

            var hasLeftButtonBounds = false;

            pressGestureRecognizer = new UILongPressGestureRecognizer(() =>
            {
                var touchedPoint = pressGestureRecognizer.LocationInView(this);
                var isInsideButtonBounds = e.NewElement.Bounds.Contains(touchedPoint.ToPoint());
                if (!isInsideButtonBounds)
                {
                    // Pointer left the bounds of the button.
                    hasLeftButtonBounds = true;
                    FireTouchCanceled();
                }

                if (pressGestureRecognizer.State == UIGestureRecognizerState.Began)
                {
                    // Reset
                    hasLeftButtonBounds = false;
                    FireTouchDown();
                }
                else if (pressGestureRecognizer.State == UIGestureRecognizerState.Ended)
                {
                    // Only fire, when pointer has never left the button bounds
                    if (!hasLeftButtonBounds)
                    {
                        FireTouchUp();
                    }
                }
            });

            pressGestureRecognizer.MinimumPressDuration = 0.0;
            AddGestureRecognizer(pressGestureRecognizer);
        }
    }

    private void FireTouchDown()
    {
        foreach (var recognizer in Element.GestureRecognizers.Where(x => x.GetType() == typeof(TouchGestureRecognizer)))
        {
            if (recognizer is TouchGestureRecognizer touchGestureRecognizer)
            {
                touchGestureRecognizer?.TouchDown();
            }
        }
    }

    private void FireTouchUp()
    {
        foreach (var recognizer in Element.GestureRecognizers.Where(x => x.GetType() == typeof(TouchGestureRecognizer)))
        {
            if (recognizer is TouchGestureRecognizer touchGestureRecognizer)
            {
                touchGestureRecognizer?.TouchUp();
            }
        }
    }

    private void FireTouchCanceled()
    {
        foreach (var recognizer in Element.GestureRecognizers.Where(x => x.GetType() == typeof(TouchGestureRecognizer)))
        {
            if (recognizer is TouchGestureRecognizer touchGestureRecognizer)
            {
                touchGestureRecognizer?.TouchCanceled();
            }
        }
    }
}

public class ColorOverlayEffectiOS : PlatformEffect
{
    private UIImageRenderingMode originalRenderingMode = UIImageRenderingMode.Automatic;
    private UIColor originalTintColor = UIColor.Black;

    protected override void OnAttached()
    {
        var effect = (ColorOverlayEffect)Element.Effects.FirstOrDefault(e => e is ColorOverlayEffect);
        if (effect == null)
            return;

        SetOverlay(effect.Color);
    }

    protected override void OnDetached()
    {
        if (Control is UIImageView imageView && imageView.Image != null)
        {
            imageView.TintColor = originalTintColor;
            imageView.Image = imageView.Image.ImageWithRenderingMode(originalRenderingMode);
        }
    }

    void SetOverlay(Color color)
    {
        var formsImage = (Image)Element;
        if (formsImage?.Source == null || formsImage?.IsLoading == true)
            return;

        if (Control is UIImageView imageView && imageView.Image != null)
        {
            originalRenderingMode = imageView.Image.RenderingMode;
            originalTintColor = imageView.TintColor;
            imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            imageView.TintColor = color.ToPlatform();
        }
    }
}