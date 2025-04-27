using System.Drawing;

namespace iSukces.DrawingPanel;

internal sealed class RullerTextManager
{
    public Brush TextBrush
    {
        get => _textBrush;
        set => _textBrush = value ??  Brushes.Black;
    }

    public double TextHeight
    {
        get
        {
            if (_textHeight >= 0) return _textHeight;
            return _textHeight = 12;
        }
    }

    private Brush _textBrush = Brushes.Black;

    private double _textHeight = -1;
}
