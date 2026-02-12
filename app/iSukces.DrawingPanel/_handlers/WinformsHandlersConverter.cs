using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;
using iSukces.Mathematics;
using Matrix = System.Drawing.Drawing2D.Matrix;

namespace iSukces.DrawingPanel;

public static class WinformsHandlersConverterX
{
    public static MouseEventArgs2 ToModel(this MouseEventArgs  src)
    {
        var button = (MouseButtons2)src.Button; 
        return new MouseEventArgs2(src.Delta, src.Location.ToModel(), button);
    }

    private static Point ToModel(this System.Drawing.Point point)
    {
        return new Point(point.X, point.Y);
    }

    public static KeyEventArgs2 ToModel(this KeyEventArgs src)
    {
        var ee = new KeyEventArgs2(src.Alt, src.Control);
        return ee;
    }

    public static Matrix ToGdi(this Mathematics.Matrix matrix)
    {
        return new Matrix(
            (float)matrix.M11, (float)matrix.M12, 
            (float)matrix.M21, (float)matrix.M22, 
            (float)matrix.OffsetX, (float)matrix.OffsetY);
    }
}
