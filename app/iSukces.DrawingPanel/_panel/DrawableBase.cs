using System.Drawing;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel;

public abstract class DrawableBase : IDrawable
{
    public abstract void Draw(Graphics graphics);

    protected void OnChanged() { Changed?.Invoke(this, EventArgs.Empty); }

    protected void SetAndNotify(ref int field, int value)
    {
        if (field == value)
            return;
        field = value;
        OnChanged();
    }

    protected void SetAndNotify<T>(ref T field, T value)
    {
        if (Equals(field, value))
            return;
        field = value;
        OnChanged();
    }

    protected void SetAndNotify<T>(ref T field, T value, Action a)
    {
        if (Equals(field, value))
            return;
        field = value;
        a();
        OnChanged();
    }

    public virtual void SetCanvasInfo(DrawingCanvasInfo canvasInfo) { CanvasInfo = canvasInfo; }

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

    public DrawingCanvasInfo CanvasInfo { get; private set; }

    private bool _visible = true;
}
