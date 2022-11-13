using UIKit;
using Maui.FreakyControls.Effects;
using CoreGraphics;
using Foundation;

namespace Maui.FreakyControls.Platforms.iOS.Effects;

public class TouchEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
{
    private TouchHandler _touchHandler;
    private UIView _view;
    private FreakyControls.Effects.TouchEffect _touchEffect;

    protected override void OnAttached()
    {
        _view = Control == null ? Container : Control;

        // Get access to the TouchEffect class in the PCL
        _touchEffect =
            (FreakyControls.Effects.TouchEffect)Element.Effects.FirstOrDefault(e => e is FreakyControls.Effects.TouchEffect);

        if (_touchEffect == null)
        {
            return;
        }

        _touchHandler = new TouchHandler();
        _touchHandler.TouchAction += TouchHandlerOnTouch;
        _touchHandler.Capture = _touchEffect.Capture;
        _touchHandler.RegisterEvents(_view);

    }

    private void TouchHandlerOnTouch(object sender, TouchActionEventArgs args)
    {
        _touchEffect.OnTouchAction(sender, args);
    }

    protected override void OnDetached()
    {
        if (_touchHandler == null)
        {
            return;
        }
        _touchHandler.TouchAction -= TouchHandlerOnTouch;
        _touchHandler.UnregisterEvents(_view);
    }
}

public class TouchHandler : TouchHandlerBase<UIView>
{
    TouchRecognizer _touchRecognizer;

    public override void RegisterEvents(UIView view)
    {
        if (view != null)
        {
            _touchRecognizer = new TouchRecognizer(view, this);
            view.AddGestureRecognizer(_touchRecognizer);
        }
    }

    public override void UnregisterEvents(UIView view)
    {
        if (_touchRecognizer != null)
        {
            // Clean up the TouchRecognizer object
            _touchRecognizer.Detach();

            // Remove the TouchRecognizer from the UIView
            view.RemoveGestureRecognizer(_touchRecognizer);
        }
    }
}

class TouchRecognizer : UIGestureRecognizer
{
    private readonly UIView _view;            // iOS UIView 
    private readonly TouchHandler _touchHandler;
    bool _capture;

    static Dictionary<UIView, TouchRecognizer> viewDictionary =
        new Dictionary<UIView, TouchRecognizer>();

    static Dictionary<long, TouchRecognizer> idToTouchDictionary =
        new Dictionary<long, TouchRecognizer>();

    public TouchRecognizer(UIView view, TouchHandler touchHandler)
    {
        _view = view;
        this._touchHandler = touchHandler;
        viewDictionary.Add(view, this);
    }

    public void Detach()
    {
        viewDictionary.Remove(_view);
    }


    // touches = touches of interest; evt = all touches of type UITouch
    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
        base.TouchesBegan(touches, evt);

        foreach (UITouch touch in touches.Cast<UITouch>())
        {
            long id = touch.Handle.Handle.ToInt64();
            FireEvent(this, id, TouchActionType.Pressed, touch, true);

            if (!idToTouchDictionary.ContainsKey(id))
                idToTouchDictionary.Add(id, this);
        }

        // Save the setting of the Capture property
        _capture = _touchHandler.Capture;
    }

    public override void TouchesMoved(NSSet touches, UIEvent evt)
    {
        base.TouchesMoved(touches, evt);

        foreach (UITouch touch in touches.Cast<UITouch>())
        {
            long id = touch.Handle.Handle.ToInt64();

            if (_capture)
            {
                FireEvent(this, id, TouchActionType.Moved, touch, true);
            }
            else
            {
                CheckForBoundaryHop(touch);

                if (idToTouchDictionary[id] != null)
                {
                    FireEvent(idToTouchDictionary[id], id, TouchActionType.Moved, touch, true);
                }
            }
        }
    }

    public override void TouchesEnded(NSSet touches, UIEvent evt)
    {
        base.TouchesEnded(touches, evt);

        foreach (UITouch touch in touches.Cast<UITouch>())
        {
            long id = touch.Handle.Handle.ToInt64();

            if (_capture)
            {
                FireEvent(this, id, TouchActionType.Released, touch, false);
            }
            else
            {
                CheckForBoundaryHop(touch);

                if (idToTouchDictionary[id] != null)
                {
                    FireEvent(idToTouchDictionary[id], id, TouchActionType.Released, touch, false);
                }
            }
            idToTouchDictionary.Remove(id);
        }
    }

    public override void TouchesCancelled(NSSet touches, UIEvent evt)
    {
        base.TouchesCancelled(touches, evt);

        foreach (UITouch touch in touches.Cast<UITouch>())
        {
            long id = touch.Handle.Handle.ToInt64();

            if (_capture)
            {
                FireEvent(this, id, TouchActionType.Cancelled, touch, false);
            }
            else if (idToTouchDictionary[id] != null)
            {
                FireEvent(idToTouchDictionary[id], id, TouchActionType.Cancelled, touch, false);
            }
            idToTouchDictionary.Remove(id);
        }
    }

    void CheckForBoundaryHop(UITouch touch)
    {
        long id = touch.Handle.Handle.ToInt64();

        // TODO: Might require converting to a List for multiple hits
        TouchRecognizer recognizerHit = null;

        foreach (UIView view in viewDictionary.Keys)
        {
            CGPoint location = touch.LocationInView(view);

            if (new CGRect(new CGPoint(), view.Frame.Size).Contains(location))
            {
                recognizerHit = viewDictionary[view];
            }
        }
        if (recognizerHit != idToTouchDictionary[id])
        {
            if (idToTouchDictionary[id] != null)
            {
                FireEvent(idToTouchDictionary[id], id, TouchActionType.Exited, touch, true);
            }
            if (recognizerHit != null)
            {
                FireEvent(recognizerHit, id, TouchActionType.Entered, touch, true);
            }
            idToTouchDictionary[id] = recognizerHit;
        }
    }

    private void FireEvent(TouchRecognizer recognizer, long id, TouchActionType actionType, UITouch touch, bool isInContact)
    {
        // Convert touch location to Xamarin.Forms Point value
        CGPoint cgPoint = touch.LocationInView(recognizer.View);
        TouchTrackingPoint xfPoint = new TouchTrackingPoint((float)cgPoint.X, (float)cgPoint.Y);

        // Get the method to call for firing events
        Action<UIView, TouchActionEventArgs> onTouchAction = recognizer._touchHandler.OnTouchAction;

        // Call that method
        onTouchAction(recognizer._view,
            new TouchActionEventArgs(id, actionType, xfPoint, isInContact));
    }
}

