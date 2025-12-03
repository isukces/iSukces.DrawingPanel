#if COMPATMATH
using Vector=iSukces.Mathematics.Compatibility.Vector;
using Point=iSukces.Mathematics.Compatibility.Point;
#else
using Point=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif


namespace iSukces.DrawingPanel.Interfaces;

public struct SnapServiceItem
{
    public SnapServiceItem(Point vertex, SnapServiceSpecialPointKind kind, Point v = new Point())
        : this()
    {
        Vertex      = vertex;
        Kind        = kind;
        SecondPoint = v;
        Custom      = SnapServiceTarget.None;
    }

    public Point                       SecondPoint { get; set; }
    public Point                       Vertex      { get; set; }
    public SnapServiceSpecialPointKind Kind        { get; }
    public SnapServiceTarget           Custom      { get; set; }
    public Vector                      LineVector  { get; set; }
    public SnapServiceItemState        ItemState   { get; set; }
}
