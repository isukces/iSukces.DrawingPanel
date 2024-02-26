namespace iSukces.DrawingPanel.Interfaces;

public interface IDrawingPanelZoomStorage 
{
    bool TryRead(out DrawingPanelZoomStorageData data);
    void Write(DrawingPanelZoomStorageData data);
    void Flush();
}