using System;
namespace Maui.FreakyControls.GestureRecognizers;

public sealed class FreakyTouchGestureRecognizer : Element, IGestureRecognizer
{
    public EventHandler<FreakyEventArgs> Pressed;
    public EventHandler<FreakyEventArgs> Released;
    public EventHandler<FreakyEventArgs> Clicked;
}

