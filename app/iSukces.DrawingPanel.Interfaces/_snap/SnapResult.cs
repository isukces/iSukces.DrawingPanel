using Point=iSukces.Mathematics.Point;

namespace iSukces.DrawingPanel.Interfaces;

public struct SnapResult
{
    public SnapResult(Point point)
    {
        Point = point;    
    }

    public Point Point { get; }
}
