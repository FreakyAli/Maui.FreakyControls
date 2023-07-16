using Android.Views;
using Maui.FreakyControls.Shared.Enums;
using Maui.FreakyControls.Shared.TouchPress;
using Microsoft.Maui.Controls.Platform;
using View = Android.Views.View;

namespace Maui.FreakyControls.Platforms.Android;

internal class TouchAndPressEffect : PlatformEffect
{
    private ITouchPressEffect _touchAndPressEffectConsumer;
    private View _view;
    private float? firstX;
    private float? firstY;
    private bool ignored;

    public TouchAndPressEffect()
    {
    }

    protected override void OnAttached()
    {
        _view = Control ?? Container;

        if (_view != null && Element is ITouchPressEffect touchAndPressEffectConsumer)
        {
            _view.Touch += OnViewOnTouch;
            _touchAndPressEffectConsumer = touchAndPressEffectConsumer;
        }
    }

    protected override void OnDetached()
    {
        if (_view != null)
        {
            _view.Touch -= OnViewOnTouch;
        }
    }

    private void OnViewOnTouch(object sender, View.TouchEventArgs e)
    {
        switch (e.Event.ActionMasked)
        {
            case MotionEventActions.Cancel:
                _touchAndPressEffectConsumer?.ConsumeEvent(EventType.Cancelled);
                break;

            case MotionEventActions.Pointer1Down:
            case MotionEventActions.ButtonPress:
            case MotionEventActions.Down:
                _touchAndPressEffectConsumer?.ConsumeEvent(EventType.Pressing);
                break;

            case MotionEventActions.Move:
                OnPointerMoved(e.Event);
                break;

            case MotionEventActions.ButtonRelease:
            case MotionEventActions.Up:
            case MotionEventActions.Pointer1Up:
                _touchAndPressEffectConsumer?.ConsumeEvent(EventType.Released);
                break;

            default:
            case MotionEventActions.Outside:
            case MotionEventActions.HoverEnter:
            case MotionEventActions.HoverExit:
            case MotionEventActions.HoverMove:
            case MotionEventActions.Mask:
            case MotionEventActions.Pointer2Down:
            case MotionEventActions.Pointer2Up:
            case MotionEventActions.Pointer3Down:
            case MotionEventActions.Pointer3Up:
            case MotionEventActions.PointerIdMask:
            case MotionEventActions.PointerIdShift:
                break;
        }

        if (e.Event.ActionMasked != MotionEventActions.Move)
        {
            this.ignored = false;
            this.firstX = null;
            this.firstY = null;
        }
    }

    private void OnPointerMoved(MotionEvent motionEvent)
    {
        if (motionEvent != null)
        {
            var x = motionEvent.GetX();
            var y = motionEvent.GetY();

            if (!this.firstX.HasValue || !this.firstY.HasValue)
            {
                this.firstX = x;
                this.firstY = y;
            }

            var maxDelta = 10;
            var deltaX = Math.Abs(x - this.firstX.Value);
            var deltaY = Math.Abs(y - this.firstY.Value);
            if (!this.ignored && (deltaX > maxDelta || deltaY > maxDelta))
            {
                this.ignored = true;
                _touchAndPressEffectConsumer?.ConsumeEvent(EventType.Ignored);
            }
        }
    }
}