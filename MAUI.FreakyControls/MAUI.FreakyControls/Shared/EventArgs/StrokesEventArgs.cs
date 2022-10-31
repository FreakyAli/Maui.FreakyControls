//Make sure .EventArgs is never created as a namespace.
namespace Maui.FreakyControls;

public class StrokesEventArgs : FreakyEventArgs
{
    public IEnumerable<IEnumerable<Point>> Strokes { get; set; } = Array.Empty<Point[]>();
}

