using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public interface IPathElement : ILineCollider
    {
        double DistanceFromElement(Point point, out double distanceFromStart);
        Point GetEndPoint();
        Vector GetEndVector();
        double GetLength();
        Point GetStartPoint();
        Vector GetStartVector();
    }
}
