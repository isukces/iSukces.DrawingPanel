using System.Drawing;
using iSukces.Mathematics;
using Point = System.Drawing.Point;
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
