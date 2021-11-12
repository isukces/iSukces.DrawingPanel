using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public interface IPathElement:ILineCollider
    {
        Point GetEndPoint();
        Vector GetEndVector();
        Point GetStartPoint();
        Vector GetStartVector();
        double GetLength();
    }
}
