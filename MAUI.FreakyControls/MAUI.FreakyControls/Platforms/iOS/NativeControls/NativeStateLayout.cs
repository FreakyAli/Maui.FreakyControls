using Foundation;
using UIKit;

namespace Maui.FreakyControls.Platforms.iOS;

public class NativeStateLayout : Microsoft.Maui.Platform.ContentView
{
    FreakyStateLayout stateLayout;

    public NativeStateLayout(IBorderView virtualView)
    {
        AccessibilityTraits = UIAccessibilityTrait.Button;
        stateLayout = (FreakyStateLayout)virtualView;
        AddGestureRecognizer(new UITapGestureRecognizer(stateLayout.InvokeClicked));
    }

    public override bool CanBecomeFocused => true;

    public override void TouchesMoved(NSSet touches, UIEvent? evt)
    {
        stateLayout.InvokeReleased();
        base.TouchesMoved(touches, evt);
    }

    public override void TouchesBegan(NSSet touches, UIEvent? evt)
    {
        stateLayout.InvokePressed();
        base.TouchesBegan(touches, evt);
    }

    public override void TouchesCancelled(NSSet touches, UIEvent? evt)
    {
        stateLayout.InvokeReleased();
        base.TouchesCancelled(touches, evt);
    }
}