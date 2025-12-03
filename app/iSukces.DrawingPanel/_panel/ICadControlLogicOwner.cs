using System;
using System.Drawing;
using System.Windows.Forms;

namespace iSukces.DrawingPanel;

public interface ICadControlLogicOwner
{
    EventControls GetEventSourceControl();
    void TransfromChanged();
    Size ClientSize { get; }
}

public sealed class EventControls
{
    public EventControls(
        Control mouseEventSource,
        Control keyboardEventSource,
        Control drawingControl)
    {
        _drawingControl     = drawingControl;
        MouseEventSource    = mouseEventSource ?? throw new ArgumentNullException(nameof(mouseEventSource));
        KeyboardEventSource = keyboardEventSource ?? throw new ArgumentNullException(nameof(keyboardEventSource));
    }


    public Point GetMousePositionOnDrawingControl()
    {
        var pos = Control.MousePosition;
        return _drawingControl.PointToClient(pos);
    }

    public Control MouseEventSource { get; }

    public Control KeyboardEventSource { get; }

    private readonly Control _drawingControl;
}
