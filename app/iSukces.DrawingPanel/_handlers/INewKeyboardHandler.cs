using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel
{
    public interface INewKeyboardHandler : INewHandler
    {
        DrawingHandleResult HandleKeyDown(KeyEventArgs keyEventArgs);
        DrawingHandleResult HandleKeyUp(KeyEventArgs keyEventArgs);
    }
}
