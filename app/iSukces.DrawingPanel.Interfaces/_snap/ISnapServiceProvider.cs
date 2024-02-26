namespace iSukces.DrawingPanel.Interfaces;

public interface ISnapServiceProvider
{
    ISnapService GetService(string documentUid);
}