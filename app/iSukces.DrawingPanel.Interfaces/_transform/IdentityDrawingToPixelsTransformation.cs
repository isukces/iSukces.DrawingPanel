using System.Drawing;
using System.Drawing.Drawing2D;
#if COMPATMATH
using WinPoint=iSukces.Mathematics.Compatibility.Point;
#else
using WinPoint=System.Windows.Point;
#endif
namespace iSukces.DrawingPanel.Interfaces;

public sealed class IdentityDrawingToPixelsTransformation : IDrawingToPixelsTransformation
{
    private IdentityDrawingToPixelsTransformation() { }

    public WinPoint FromCanvas(WinPoint point) { return point; }

    public WinPoint FromCanvas(Point point) { return new WinPoint(point.X, point.Y); }

    public Matrix GetTransform()
    {
        const int fScale = 1;
        var m = new Matrix(fScale, 0,
            0, fScale,
            0, 0);
        return m;
    }

    public WinPoint ToCanvas(WinPoint point) { return point; }

    public WinPoint ToCanvas(double pointX, double pointY) { return new(pointX, pointY); }

    public PointF ToCanvasF(WinPoint point) { return new((float)point.X, (float)point.Y); }

    public PointF ToCanvasF(double pointX, double pointY) { return new((float)pointX, (float)pointY); }

    public double Scale => 1;

    public static IdentityDrawingToPixelsTransformation Instance
        => IdentityDrawingToPixelsTransformationHolder.SingleIstance;

    private static class IdentityDrawingToPixelsTransformationHolder
    {
        public static readonly IdentityDrawingToPixelsTransformation SingleIstance =
            new IdentityDrawingToPixelsTransformation();
    }
}
