using System.Drawing;
using System.Drawing.Drawing2D;
using iSukces.Mathematics;
using Size = iSukces.Mathematics.Size;
using Point = iSukces.Mathematics.Point;


namespace iSukces.DrawingPanel.Interfaces;

public class FlippedYDrawingToPixelsTransformation : IDrawingToPixelsTransformation
{
    public FlippedYDrawingToPixelsTransformation(double scale, double deltaX, double deltaY)
    {
        DeltaX = deltaX;
        DeltaY = deltaY;
        Scale  = scale;
    }

    public static FlippedYDrawingToPixelsTransformation Make(System.Drawing.Size viewPortSize, Point center, double scale)
    {
        var screenCenter = new Point(viewPortSize.Width * 0.5, viewPortSize.Height * 0.5);

        var dx = screenCenter.X - center.X * scale;
        var dy = screenCenter.Y + center.Y * scale;
        return new FlippedYDrawingToPixelsTransformation(scale, dx, dy);
    }


    public Point FromCanvas(Point point)
    {
        var x = (point.X - DeltaX) / Scale;
        var y = (DeltaY - point.Y) / Scale;
        return new Point(x, y);
    }

    public Point FromCanvas(System.Drawing.Point point)
    {
        var x = (point.X - DeltaX) / Scale;
        var y = (DeltaY - point.Y) / Scale;
        return new Point(x, y);
    }

    public Matrix GetTransform()
    {
        var fScale = (float)Scale;
        var m = new Matrix(fScale, 0,
            0, -fScale,
            (float)DeltaX, (float)DeltaY);
        return m;
    }

    public Point ToCanvas(Point point)
    {
        var x = DeltaX + point.X * Scale;
        var y = DeltaY - point.Y * Scale;
        return new Point(x, y);
    }

    public Point ToCanvas(double pointX, double pointY)
    {
        var x = DeltaX + pointX * Scale;
        var y = DeltaY - pointY * Scale;
        return new Point(x, y);
    }

    public PointF ToCanvasF(double pointX, double pointY)
    {
        var x = DeltaX + pointX * Scale;
        var y = DeltaY - pointY * Scale;
        return new PointF((float)x, (float)y);
    }


    public PointF ToCanvasF(Point point)
    {
        var x = DeltaX + point.X * Scale;
        var y = DeltaY - point.Y * Scale;
        return new PointF((float)x, (float)y);
    }

    public override string ToString() { return $"DeltaX={DeltaX}, DeltaY={DeltaY}, Scale={Scale}"; }

    public double Scale { get; }

    public static readonly FlippedYDrawingToPixelsTransformation Empty =
        new FlippedYDrawingToPixelsTransformation(1, 0, 0);

    public readonly double DeltaX;
    public readonly double DeltaY;
}
