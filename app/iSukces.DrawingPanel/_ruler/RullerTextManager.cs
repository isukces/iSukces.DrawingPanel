using System.Drawing;

namespace iSukces.DrawingPanel;

internal sealed class RullerTextManager
{
    /*
    public FormattedText CreateFormattedText(string text)
    {
        var formattedText = new FormattedText(
            text,
            CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            TypeFace,
            DeviceIndependentPixelsHelper.PtToDip(8),
            TextBrush);
        formattedText.SetFontWeight(FontWeights.Regular);
        formattedText.TextAlignment = TextAlignment.Center;
        return formattedText;
    }
    */


    /*public string FontName
    {
        get { return _fontName; }
        set
        {
            value = value ?? DefaultFontFamily;
            if (_fontName == value)
                return;
            _fontName   = value;
            _typeFace   = null;
            _textHeight = -1;
        }
    }*/

    /*
    public Typeface TypeFace
    {
        get
        {
            if (_typeFace is null)
                _typeFace = new Typeface(_fontName ?? DefaultFontFamily);
            return _typeFace;
        }
    }
    */

    public Brush TextBrush
    {
        get => _textBrush;
        set
        {
            if (value is null)
                value = Brushes.Black;
            if (_textBrush == value)
                return;
            _textBrush = value;
        }
    }

    public double TextHeight
    {
        get
        {
            if (_textHeight >= 0) return _textHeight;
            // throw new NotImplementedException();
            //var formattedText = CreateFormattedText("0123456789,");
            return _textHeight = 12;
        }
    }

    // private string _fontName;
    private Brush _textBrush = Brushes.Black;

    private double _textHeight = -1;
    //private Typeface _typeFace;

    // private const string DefaultFontFamily = "Segoe UI";
}