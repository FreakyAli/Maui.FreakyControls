using Foundation;
using Maui.FreakyControls.Shared.Enums;
using Maui.FreakyControls.Shared.TouchPress;
using Microsoft.Maui.Controls.Platform;
using UIKit;

namespace Maui.FreakyControls.Platforms.iOS
{
    internal class TouchAndPressEffect : PlatformEffect
    {
        private UIView _view;
        private TouchAndPressGestureRecognizer _touchAndPressGestureRecognizer;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            if (Element is ITouchPressEffect touchAndPressEffectConsumer)
            {
                _view.UserInteractionEnabled = true;

                _touchAndPressGestureRecognizer = new TouchAndPressGestureRecognizer(touchAndPressEffectConsumer);
                _view.AddGestureRecognizer(_touchAndPressGestureRecognizer);
            }
        }

        protected override void OnDetached()
        {
            if (_view != null && _touchAndPressGestureRecognizer != null)
            {
                _view.RemoveGestureRecognizer(_touchAndPressGestureRecognizer);
            }
        }

        private sealed class TouchAndPressGestureRecognizer : UIGestureRecognizer
        {
            private readonly ITouchPressEffect _touchAndPressEffectConsumer;

            public TouchAndPressGestureRecognizer(ITouchPressEffect touchAndPressEffectConsumer)
            {
                _touchAndPressEffectConsumer = touchAndPressEffectConsumer;
            }

            public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
            {
                base.PressesBegan(presses, evt);
                _touchAndPressEffectConsumer.ConsumeEvent(EventType.Pressing);
            }

            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                _touchAndPressEffectConsumer.ConsumeEvent(EventType.Pressing);
            }

            public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
            {
                base.PressesEnded(presses, evt);
                _touchAndPressEffectConsumer.ConsumeEvent(EventType.Released);
            }

            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                base.TouchesEnded(touches, evt);
                _touchAndPressEffectConsumer.ConsumeEvent(EventType.Released);
            }

            public override void PressesCancelled(NSSet<UIPress> presses, UIPressesEvent evt)
            {
                base.PressesCancelled(presses, evt);
                _touchAndPressEffectConsumer.ConsumeEvent(EventType.Cancelled);
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);
                _touchAndPressEffectConsumer.ConsumeEvent(EventType.Cancelled);
            }

            public override void IgnoreTouch(UITouch touch, UIEvent forEvent)
            {
                base.IgnoreTouch(touch, forEvent);
                _touchAndPressEffectConsumer.ConsumeEvent(EventType.Ignored);
            }
        }
    }
}