namespace Maui.FreakyControls;

public class TouchGestureRecognizer : Element, IGestureRecognizer
{
    public Action TouchDown;

    public Action TouchUp;

    /// <summary>
    /// Canceled the touch event by mobing the pointer outside the bounds
    /// of the button
    /// </summary>
    public Action TouchCanceled;
}

