namespace iSukces.DrawingPanel.Interfaces;

public interface IDpMouseButtonHandler : IDpHandler
{
    DrawingHandleResult HandleOnMouseDown(MouseEventArgs2 e);
    DrawingHandleResult HandleOnMouseMove(MouseEventArgs2 args);
    DrawingHandleResult HandleOnMouseUp(MouseEventArgs2 e);
}
