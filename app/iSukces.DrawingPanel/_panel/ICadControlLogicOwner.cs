using System;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel
{
    public interface ICadControlLogicOwner
    {
        EventControls GetEventSourceControl();
        void TransfromChanged();
        Size ClientSize { get; }
    }

    public sealed class EventControls
    {
        public EventControls(
            [NotNull] Control mouseEventSource,
            [NotNull] Control keyboardEventSource,
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

        [NotNull]
        public Control MouseEventSource { get; }

        [NotNull]
        public Control KeyboardEventSource { get; }

        private readonly Control _drawingControl;
    }
}
