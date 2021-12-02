#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    public interface IPathElement : ILineCollider
    {
        double DistanceFromElement(Point point, out double distanceFromStart, out Vector direction);
        Point GetEndPoint();
        Vector GetEndVector();
        double GetLength();
        Point GetStartPoint();
        Vector GetStartVector();
        object Tag { get; set; }
    }
}
