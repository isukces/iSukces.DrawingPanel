namespace iSukces.DrawingPanel.Interfaces;

public interface IDpKeyboardHandler : IDpHandler
{
    DrawingHandleResult HandleKeyDown(KeyEventArgs2 keyEventArgs);
    DrawingHandleResult HandleKeyUp(KeyEventArgs2 keyEventArgs);
}
