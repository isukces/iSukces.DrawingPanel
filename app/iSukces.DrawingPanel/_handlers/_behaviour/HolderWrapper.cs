#nullable disable
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel;

internal struct HolderWrapper
{
    public HolderWrapper(IDpHandler handler, int order)
    {
        Handler = handler;
        Order   = order;
    }

    public override string ToString() { return Handler?.ToString() ?? base.ToString(); }

    public IDpHandler Handler { get; }
    public int        Order   { get; }
}
