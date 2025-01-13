#nullable disable
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
#else
using Point=System.Windows.Point;
#endif

namespace iSukces.DrawingPanel.Interfaces;

public struct SnapResult
{
    public SnapResult(Point point)
    {
        Point = point;    
    }

    public Point Point { get; }
}
