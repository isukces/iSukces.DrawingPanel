using System.Windows.Forms;

namespace iSukces.DrawingPanel.Interfaces
{
    public interface IDpKeyboardHandler : IDpHandler
    {
        DrawingHandleResult HandleKeyDown(KeyEventArgs keyEventArgs);
        DrawingHandleResult HandleKeyUp(KeyEventArgs keyEventArgs);
    }
}
