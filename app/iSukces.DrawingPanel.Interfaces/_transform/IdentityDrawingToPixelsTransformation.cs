using System.Drawing;
using System.Drawing.Drawing2D;
using SPoint=iSukces.Mathematics.Point;

namespace iSukces.DrawingPanel.Interfaces;

public sealed class IdentityDrawingToPixelsTransformation : IDrawingToPixelsTransformation
{
    private IdentityDrawingToPixelsTransformation() { }

    public SPoint FromCanvas(SPoint point) { return point; }

    public SPoint FromCanvas(Point point) { return new SPoint(point.X, point.Y); }

    public Matrix GetTransform()
    {
        const int fScale = 1;
        var m = new Matrix(fScale, 0,
            0, fScale,
            0, 0);
        return m;
    }

    public SPoint ToCanvas(SPoint point) { return point; }

    public SPoint ToCanvas(double pointX, double pointY) { return new(pointX, pointY); }

    public PointF ToCanvasF(SPoint point) { return new((float)point.X, (float)point.Y); }

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
