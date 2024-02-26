using System.Windows.Forms;

namespace iSukces.DrawingPanel.Interfaces;

public interface IDpMouseWheelHandler : IDpHandler
{
    DrawingHandleResult HandleMouseWheel(MouseEventArgs args);
}