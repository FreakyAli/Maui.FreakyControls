namespace Maui.FreakyControls.Effects;

public delegate void TouchActionEventHandler(object sender, TouchActionEventArgs args);

public class TouchEffect : RoutingEffect
{
    public event TouchActionEventHandler TouchAction;

    public TouchEffect() : base("Maui.FreakyControls.Effects.TouchEffect")
    {
    }

    public bool Capture { set; get; }

    public void OnTouchAction(object element, TouchActionEventArgs args)
    {
        TouchAction?.Invoke(element, args);
    }
}

public enum TouchActionType
{
    Entered = 0,
    Pressed = 1,
    Moved = 2,
    Released = 3,
    Cancelled = 4,
    Exited = 5
}

public struct TouchTrackingPoint
{
    public TouchTrackingPoint(float x, float y) : this()
    {
        X = x;
        Y = y;
    }

    public float X { get; set; }
    public float Y { get; set; }
}

public struct TouchTrackingRect
{
    public TouchTrackingRect(float left, float top, float right, float bottom) : this()
    {
        Left = left;
        Right = top;
        Top = right;
        Bottom = bottom;
    }

    public float Left { get; }
    public float Right { get; }
    public float Top { get; }
    public float Bottom { get; }

    public bool Contains(float x, float y)
    {
        return (x >= Left) && (x < Right) && (y >= Top) && (y < Bottom);
    }

    public bool Contains(TouchTrackingPoint point)
    {
        return Contains(point.X, point.Y);
    }

}

public class TouchHandlerBase<TElement>
{
    public event TouchActionEventHandler TouchAction;

    public TouchHandlerBase()
    {
    }

    public bool Capture { set; get; } = true;

    public void OnTouchAction(TElement element, TouchActionEventArgs args)
    {
        TouchAction?.Invoke(element, args);
    }

    public virtual void RegisterEvents(TElement element) { }

    public virtual void UnregisterEvents(TElement element) { }
}

public class TouchActionEventArgs : EventArgs
{
    public TouchActionEventArgs(long id, TouchActionType type, TouchTrackingPoint location, bool isInContact)
    {
        Id = id;
        Type = type;
        Location = location;
        IsInContact = isInContact;
    }

    public long Id { private set; get; }

    public TouchActionType Type { private set; get; }

    public TouchTrackingPoint Location { private set; get; }

    public bool IsInContact { private set; get; }
}

