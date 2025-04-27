namespace iSukces.DrawingPanel.Interfaces;

public interface IDpHandlerContainer
{
    void RegisterHandler(IDpHandler handler, int order);
    void UnregisterHandler(IDpHandler handler);
}
