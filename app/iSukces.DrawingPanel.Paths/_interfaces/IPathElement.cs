using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public interface IPathElement
    {
        Point GetEndPoint();
        Point GetStartPoint();
        
        
        
        Vector GetEndVector();
        Vector GetStartVector();

    }
}
