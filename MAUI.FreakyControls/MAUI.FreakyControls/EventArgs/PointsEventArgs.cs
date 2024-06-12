//Make sure .EventArgs is never created as a namespace.
namespace Maui.FreakyControls;

public class PointsEventArgs : FreakyEventArgs
{
    public IEnumerable<Point> Points { get; set; } = new Point[0];
}