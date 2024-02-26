using System;
using System.ComponentModel;
using System.Drawing;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel;

public abstract class PresenterDecorator : IDrawable, ISupportInitialize, IDisposable
{
    ~PresenterDecorator()
    {
        DisposeInternal(false);
    }

    public void BeginInit()
    {
        _suspendLevel++;
    }

    public void Dispose()
    {
        DisposeInternal(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void DisposeInternal(bool disposing)
    {
        if (disposing)
        {
            // Release managed resources here
        }
        // Release unmanaged resources here
    }

    public abstract void Draw(Graphics graphics);

    public void EndInit()
    {
        if (_suspendLevel < 1)
            throw new InvalidOperationException();
        _suspendLevel--;
        if (_suspendLevel == 0 && _needNotifyOnChanged)
        {
            _needNotifyOnChanged = false;
            OnChanged();
        }
    }

    protected void OnChanged()
    {
        if (_suspendLevel > 0)
            _needNotifyOnChanged = true;
        else
            Changed?.Invoke(this, EventArgs.Empty);
    }

    public virtual void SetCanvasInfo(DrawingCanvasInfo canvasInfo)
    {
        CanvasInfo = canvasInfo;
    }

    #region properties

    protected DrawingCanvasInfo CanvasInfo { get; private set; }

    #endregion

    public event EventHandler Changed;

    public bool Visible
    {
        get => _visible;
        set
        {
            if (_visible == value)
                return;
            _visible = value;
            OnChanged();
        }
    }

    public bool PresenterRenderingFlag { get; set; }

    #region Fields

    private bool _needNotifyOnChanged;
    private int _suspendLevel;
    private bool _visible = true;

    #endregion
}