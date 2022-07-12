using System.Drawing;
using System.Drawing.Drawing2D;
using WinPoint = System.Windows.Point;

namespace iSukces.DrawingPanel.Interfaces
{
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
}
