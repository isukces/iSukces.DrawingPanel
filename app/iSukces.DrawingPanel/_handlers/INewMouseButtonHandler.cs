using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel
{
    public interface INewMouseButtonHandler : INewHandler
    {
        DrawingHandleResult HandleOnMouseDown(MouseEventArgs e);
        DrawingHandleResult HandleOnMouseMove(MouseEventArgs args);
        DrawingHandleResult HandleOnMouseUp(MouseEventArgs e);
    }
}
