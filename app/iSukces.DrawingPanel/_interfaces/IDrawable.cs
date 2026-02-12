using System.Drawing;

namespace iSukces.DrawingPanel.Interfaces;

public interface IDrawable: IDrawableBase
{
    void Draw(Graphics graphics);
    
}