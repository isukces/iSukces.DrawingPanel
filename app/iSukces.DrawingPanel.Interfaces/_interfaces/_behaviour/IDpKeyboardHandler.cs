namespace iSukces.DrawingPanel.Interfaces;

public interface IDpKeyboardHandler : IDpHandler
{
    DrawingHandleResult HandleKeyDown(DpKeyEventArgs keyEventArgs);
    DrawingHandleResult HandleKeyUp(DpKeyEventArgs keyEventArgs);
}
