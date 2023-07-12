using Android.Views;
using Microsoft.Maui.Controls.Platform;
using View = Android.Views.View;

namespace Maui.FreakyControls.Platforms.Android;

public class TouchReleaseEffect : PlatformEffect
{
    private View _view;

    private Action _onRelease;

    protected override void OnAttached()
    {
        _view = Control ?? Container;

        if (_view != null)
        {
            var touchReleaseEffect = (Maui.FreakyControls.TouchPress.TouchReleaseEffect)Element.Effects.FirstOrDefault(x => x is TouchReleaseEffect);
            if (touchReleaseEffect != null && touchReleaseEffect.OnRelease != null)
            {
                _onRelease = touchReleaseEffect.OnRelease;
                _view.Touch += OnViewOnTouch;
            }
        }
    }

    protected override void OnDetached()
    {
        if (_view != null)
            _view.Touch -= OnViewOnTouch;
    }

    private void OnViewOnTouch(object sender, View.TouchEventArgs e)
    {
        e.Handled = false;

        if (e.Event.ActionMasked == MotionEventActions.Up)
        {
            System.Diagnostics.Debug.WriteLine(e.Event.ActionMasked);
            _onRelease.Invoke();
        }
    }
}