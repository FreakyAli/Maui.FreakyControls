//Make sure .EventArgs is never created as a namespace.
namespace Maui.FreakyControls;

/// <summary>
/// Color picker event args
/// </summary>
public class FreakyColorPickerEventArgs : FreakyEventArgs
{
    public Color Color { get; set; }

    public double PointerX { get; set; }

    public double PointerY { get; set; }
}