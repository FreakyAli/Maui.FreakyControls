namespace Maui.FreakyControls.Shared.Enums;

public enum Shape
{
    // <summary>
    // a Circular shape to your control
    // </summary>
    Circle,

    [Obsolete("Please use Square instead")]
    // <summary>
    // a Rectangular shape to your control
    // </summary>
    Rectangle,

    // <summary>
    // a Square shape to your control
    // </summary>
    Sqaure = Rectangle,
}