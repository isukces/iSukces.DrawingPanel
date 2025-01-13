#nullable disable
using System.Windows.Forms;

namespace iSukces.DrawingPanel.Interfaces;

public interface IDpMouseButtonHandler : IDpHandler
{
    DrawingHandleResult HandleOnMouseDown(MouseEventArgs e);
    DrawingHandleResult HandleOnMouseMove(MouseEventArgs args);
    DrawingHandleResult HandleOnMouseUp(MouseEventArgs e);
}
