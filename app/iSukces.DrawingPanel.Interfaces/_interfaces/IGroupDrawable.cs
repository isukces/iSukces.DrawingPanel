using System.ComponentModel;

namespace iSukces.DrawingPanel.Interfaces
{
    public interface IGroupDrawable : ISupportInitialize
    {
        bool PendingDrawing { get; }
    }
}
