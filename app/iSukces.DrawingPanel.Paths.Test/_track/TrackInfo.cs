#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=iSukces.Mathematics.Point;
using Vector=iSukces.Mathematics.Vector;
#endif

namespace iSukces.DrawingPanel.Paths.Test;

public readonly struct TrackInfo
{
    public TrackInfo(Point location, Vector direction)
    {
        Location  = location;
        Direction = direction;
    }

    public override string ToString()
    {
        return $"TrackInfo {Location} -> {Direction}";
    }

    public Point  Location  { get; }
    public Vector Direction { get; }
}
