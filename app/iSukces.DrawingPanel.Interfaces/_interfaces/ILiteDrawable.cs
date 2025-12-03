using System.Drawing;

namespace iSukces.DrawingPanel.Interfaces;

public interface ILiteDrawable
{
    void Draw(Graphics graphics, DrawingCanvasInfo canvasInfo);
}
