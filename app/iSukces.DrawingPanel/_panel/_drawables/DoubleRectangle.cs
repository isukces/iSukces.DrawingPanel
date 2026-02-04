using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel;

public struct DoubleRectangle
{
    public void SetXFromCenterAndLength(double xCenter, double width)
    {
        width = Math.Abs(width * 0.5);
        X1    = xCenter - width;
        X2    = xCenter + width;
    }

    public double X1 { get; set; }
    public double X2 { get; set; }
    public double Y1 { get; set; }
    public double Y2 { get; set; }

    

    public void SetYFromCenterAndLength(double yCenter, double height)
    {
        height = Math.Abs(height * 0.5);
        Y1     = yCenter - height;
        Y2     = yCenter + height;
    }

    public System.Drawing.RectangleF Transform(IDrawingToPixelsTransformation transformation)
    {
        var a = transformation.ToCanvasF(X1, Y1);
        var b = transformation.ToCanvasF(X2, Y2);
        return new System.Drawing.RectangleF(
            Math.Min(a.X, b.X),
            Math.Min(a.Y, b.Y),
            Math.Abs(b.X - a.X),
            Math.Abs(b.Y - a.Y));
    }
}

