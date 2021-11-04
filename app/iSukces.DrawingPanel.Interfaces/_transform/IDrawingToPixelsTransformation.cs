using System.Drawing;
using System.Drawing.Drawing2D;
using Point = System.Windows.Point;

namespace iSukces.DrawingPanel.Interfaces
{
    public interface IDrawingToPixelsTransformation
    {
        Point FromCanvas(Point point);

        Point FromCanvas(System.Drawing.Point point);

        Matrix GetTransform();
        Point ToCanvas(Point point);

        Point ToCanvas(double pointX, double pointY);

        PointF ToCanvasF(Point point);

        PointF ToCanvasF(double pointX, double pointY);
        double Scale { get; }
    }
}
