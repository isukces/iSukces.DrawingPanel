using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel;

public sealed class SimpleMouseCapturableContainer : IMouseCapturableContainer
{
    public SimpleMouseCapturableContainer(Control panel)
    {
        _panel = panel;
    }

    public bool CaptureMouse(bool startCapture)
    {
        if (startCapture)
        {
            _panel.Capture = true;
            return _panel.Capture;
        }

        _panel.Capture = false;
        return true;
    }

    #region Fields

    private readonly Control _panel;

    #endregion
}
