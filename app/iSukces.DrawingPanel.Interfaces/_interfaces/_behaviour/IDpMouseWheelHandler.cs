namespace iSukces.DrawingPanel.Interfaces;

public interface IDpMouseWheelHandler : IDpHandler
{
    DrawingHandleResult HandleMouseWheel(MouseEventArgs2 args);
}