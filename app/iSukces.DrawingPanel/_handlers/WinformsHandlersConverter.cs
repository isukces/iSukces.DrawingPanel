using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;
using iSukces.Mathematics;
using Matrix = System.Drawing.Drawing2D.Matrix;

namespace iSukces.DrawingPanel;

public static class WinformsHandlersConverterX
{
    public static DpMouseEventArgs ToModel(this MouseEventArgs  src)
    {
        var button = (DpMouseButtons)src.Button; 
        return new DpMouseEventArgs(src.Delta, src.Location.ToModel(), button);
    }

    private static Point ToModel(this System.Drawing.Point point)
    {
        return new Point(point.X, point.Y);
    }

    public static DpKeyEventArgs ToModel(this KeyEventArgs src)
    {
        var ee = new DpKeyEventArgs(src.Alt, src.Control, src.KeyCode.ToString());
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
