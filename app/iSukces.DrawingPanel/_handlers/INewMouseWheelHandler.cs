using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel
{
    public interface INewMouseWheelHandler : INewHandler
    {
        DrawingHandleResult HandleMouseWheel(MouseEventArgs args);
    }
}