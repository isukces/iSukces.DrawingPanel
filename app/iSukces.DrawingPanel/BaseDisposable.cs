using System;

namespace iSukces.DrawingPanel;

public class BaseDisposable : IDisposable
{
    ~BaseDisposable() { DisposeInternal(); }

    public void Dispose()
    {
        DisposeInternal();
        GC.SuppressFinalize(this);
    }

    protected virtual void DisposeInternal()
    {
        // Release managed resources here
    }
}
