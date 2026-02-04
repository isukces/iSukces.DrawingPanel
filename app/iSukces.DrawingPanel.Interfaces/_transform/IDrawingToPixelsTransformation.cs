using System.Drawing;
using System.Drawing.Drawing2D;
using SPoint=iSukces.Mathematics.Point;


namespace iSukces.DrawingPanel.Interfaces;

public interface IDrawingToPixelsTransformation
{
    SPoint FromCanvas(SPoint point);

    SPoint FromCanvas(Point point);

    Matrix GetTransform();
        
    SPoint ToCanvas(SPoint point);

    SPoint ToCanvas(double pointX, double pointY);

    PointF ToCanvasF(SPoint point);

    PointF ToCanvasF(double pointX, double pointY);

    double Scale { get; }
}
