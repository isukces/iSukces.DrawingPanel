using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public interface IPathElement
    {
        Point GetEndPoint();


        Vector GetEndVector();
        Point GetStartPoint();
        Vector GetStartVector();
    }
}
