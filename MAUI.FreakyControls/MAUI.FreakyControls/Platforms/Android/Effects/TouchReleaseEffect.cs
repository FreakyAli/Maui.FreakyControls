using Android.Views;
using Microsoft.Maui.Controls.Platform;
using View = Android.Views.View;

namespace Maui.FreakyControls.Platforms.Android;

internal class TouchReleaseEffect : PlatformEffect
{
    private Action _onRelease;
    private View _view;

    protected override void OnAttached()
    {
        _view = Control ?? Container;

        if (_view is not null)
        {
            var touchReleaseEffect = (Maui.FreakyControls.Shared.TouchPress.ToouchReleaseRoutingEffect)Element.Effects.FirstOrDefault(x => x is TouchReleaseEffect);
            if (touchReleaseEffect is not null && touchReleaseEffect.OnRelease is not null)
            {
                _onRelease = touchReleaseEffect.OnRelease;
                _view.Touch += OnViewOnTouch;
            }
        }
    }

    protected override void OnDetached()
    {
        if (_view is not null)
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