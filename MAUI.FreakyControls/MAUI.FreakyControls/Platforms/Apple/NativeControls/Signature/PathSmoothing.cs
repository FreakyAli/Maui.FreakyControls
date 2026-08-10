#nullable disable

using NativePath = UIKit.UIBezierPath;
using NativePoint = CoreGraphics.CGPoint;

namespace Maui.FreakyControls.Platforms.Apple;

internal static class PathSmoothing
{
    /// <summary>
    /// Obtain a smoothed path with the specified granularity from the current path using Catmull-Rom spline.
    /// Also outputs a List of the points corresponding to the smoothed path.
    /// </summary>
    public static InkStroke SmoothedPathWithGranularity(InkStroke currentPath, int granularity)
    {
        var currentPoints = currentPath.GetPoints().ToList();

        SmoothedPathWithGranularity(currentPoints, granularity, out NativePath smoothedPath, out List<NativePoint> smoothedPoints);
        if (smoothedPath is null)
        {
            return currentPath;
        }
        return new InkStroke(smoothedPath, smoothedPoints.ToList(), currentPath.Color, currentPath.Width);
    }

    public static void SmoothedPathWithGranularity(List<NativePoint> currentPoints, int granularity, out NativePath smoothedPath, out List<NativePoint> smoothedPoints)
    {
        if (currentPoints.Count < 4)
        {
            smoothedPath = null;
            smoothedPoints = null;
            return;
        }

        smoothedPath = new NativePath();
        smoothedPoints = new List<NativePoint>();

        currentPoints.Insert(0, currentPoints[0]);
        currentPoints.Add(currentPoints[currentPoints.Count - 1]);

        smoothedPath.MoveTo(currentPoints[0].X, currentPoints[0].Y);
        smoothedPoints.Add(currentPoints[0]);

        for (var index = 1; index < currentPoints.Count - 2; index++)
        {
            var p0 = currentPoints[index - 1];
            var p1 = currentPoints[index];
            var p2 = currentPoints[index + 1];
            var p3 = currentPoints[index + 2];

            for (var i = 1; i < granularity; i++)
            {
                var t = (float)i * (1f / (float)granularity);
                var tt = t * t;
                var ttt = tt * t;

                var mid = new NativePoint
                {
                    X = 0.5f * ((2f * p1.X) + ((p2.X - p0.X) * t) +
                        (((2f * p0.X) - (5f * p1.X) + (4f * p2.X) - p3.X) * tt) +
                        (((3f * p1.X) - p0.X - (3f * p2.X) + p3.X) * ttt)),

                    Y = 0.5f * ((2 * p1.Y) + ((p2.Y - p0.Y) * t) +
                        (((2 * p0.Y) - (5 * p1.Y) + (4 * p2.Y) - p3.Y) * tt) +
                        (((3 * p1.Y) - p0.Y - (3 * p2.Y) + p3.Y) * ttt))
                };
                smoothedPath.LineTo(mid.X, mid.Y);
                smoothedPoints.Add(mid);
            }

            smoothedPath.LineTo(p2.X, p2.Y);
            smoothedPoints.Add(p2);
        }

        var last = currentPoints[currentPoints.Count - 1];
        smoothedPath.LineTo(last.X, last.Y);
        smoothedPoints.Add(last);
    }
}
