using Android.Content;
using Android.Views;
using Rect = Android.Graphics.Rect;
using View = Android.Views.View;

namespace Maui.FreakyControls.Platforms.Android;

public class NativeStateLayout : Microsoft.Maui.Platform.ContentViewGroup
{
    Rect _rect;
    FreakyStateLayout stateLayout;

    public NativeStateLayout(Context context, IBorderView virtualView) : base(context)
    {
        stateLayout = (FreakyStateLayout)virtualView;
        Clickable = true;

        Click += (sender, e) => stateLayout.InvokeClicked();

        Touch += (sender, te) =>
        {
            if (sender is not View view)
            {
                return;
            }

            switch (te?.Event?.Action)
            {
                case MotionEventActions.Down:
                    _rect = new Rect(view.Left, view.Top, view.Right, view.Bottom);

                    stateLayout.InvokePressed();
                    break;

                case MotionEventActions.Up:
                    if (_rect.Contains(view.Left + (int)te.Event.GetX(), view.Top + (int)te.Event.GetY()))
                    {
                        stateLayout.InvokeReleased();
                        stateLayout.InvokeClicked();
                    }
                    else
                    {
                        stateLayout.InvokeReleased();
                    }
                    break;

                case MotionEventActions.Cancel:
                    stateLayout.InvokeReleased();

                    break;

                case MotionEventActions.Move:
                    if (!_rect.Contains(view.Left + (int)te.Event.GetX(), view.Top + (int)te.Event.GetY()))
                    {
                        stateLayout.InvokeReleased();
                    }

                    break;
            }
        };
    }
}