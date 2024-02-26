using System.Drawing;
using System.Drawing.Drawing2D;
#if COMPATMATH
using WinPoint=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using WinPoint=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif


namespace iSukces.DrawingPanel.Interfaces;

public interface IDrawingToPixelsTransformation
{
    WinPoint FromCanvas(WinPoint point);

    WinPoint FromCanvas(Point point);

    Matrix GetTransform();
        
    WinPoint ToCanvas(WinPoint point);

    WinPoint ToCanvas(double pointX, double pointY);

    PointF ToCanvasF(WinPoint point);

    PointF ToCanvasF(double pointX, double pointY);

    double Scale { get; }
}