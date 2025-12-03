using System.Drawing;

namespace iSukces.DrawingPanel.Sample;

public class CrossDrawable : DrawableBase
{
    public override void Draw(Graphics graphics)
    {
        var       tr  = CanvasInfo.Transformation;
        var       c   = tr.ToCanvasF(0, 0);
        using var pen = new Pen(Color.Gold, 3);
        graphics.DrawLine(pen, c.X - _size, c.Y, c.X + _size, c.Y);
        graphics.DrawLine(pen, c.X, c.Y - _size, c.X, c.Y + _size);
    }

    public int Size
    {
        get => _size;
        set => SetAndNotify(ref _size, value < 1 ? 1 : value);
    }

    private int _size = 50;
}
