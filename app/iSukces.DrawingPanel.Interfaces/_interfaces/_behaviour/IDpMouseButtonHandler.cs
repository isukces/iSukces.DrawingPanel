namespace iSukces.DrawingPanel.Interfaces;

public interface IDpMouseButtonHandler : IDpHandler
{
    DrawingHandleResult HandleOnMouseDown(DpMouseEventArgs e);
    DrawingHandleResult HandleOnMouseMove(DpMouseEventArgs args);
    DrawingHandleResult HandleOnMouseUp(DpMouseEventArgs e);
}
