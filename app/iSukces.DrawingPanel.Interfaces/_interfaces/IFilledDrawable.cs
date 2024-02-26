using System.Drawing;

namespace iSukces.DrawingPanel.Interfaces;

public interface IFilledDrawable
{
    Brush Fill { get; set; }
}