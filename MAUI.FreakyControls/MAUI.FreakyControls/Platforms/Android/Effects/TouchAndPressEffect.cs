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
            case MotionEventActions.ButtonPress:
                _touchAndPressEffectConsumer?.ConsumeEvent(EventType.Pressing);
                break;

            case MotionEventActions.ButtonRelease:
                _touchAndPressEffectConsumer?.ConsumeEvent(EventType.Released);
                break;

            case MotionEventActions.Cancel:
                _touchAndPressEffectConsumer?.ConsumeEvent(EventType.Cancelled);
                break;

            case MotionEventActions.Down:
                _touchAndPressEffectConsumer?.ConsumeEvent(EventType.Pressing);
                break;

            case MotionEventActions.HoverEnter:
                break;

            case MotionEventActions.HoverExit:
                break;

            case MotionEventActions.HoverMove:
                break;

            case MotionEventActions.Mask:
                break;

            case MotionEventActions.Move:
                var motionEvent = e.Event;

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
                break;

            case MotionEventActions.Outside:
                break;

            case MotionEventActions.Pointer1Down:
                _touchAndPressEffectConsumer?.ConsumeEvent(EventType.Pressing);
                break;

            case MotionEventActions.Pointer1Up:
                _touchAndPressEffectConsumer?.ConsumeEvent(EventType.Released);
                break;

            case MotionEventActions.Pointer2Down:
                break;

            case MotionEventActions.Pointer2Up:
                break;

            case MotionEventActions.Pointer3Down:
                break;

            case MotionEventActions.Pointer3Up:
                break;

            case MotionEventActions.PointerIdMask:
                break;

            case MotionEventActions.PointerIdShift:
                break;

            case MotionEventActions.Up:
                _touchAndPressEffectConsumer?.ConsumeEvent(EventType.Released);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        if (e.Event.ActionMasked != MotionEventActions.Move)
        {
            this.ignored = false;
            this.firstX = null;
            this.firstY = null;
        }
    }
}