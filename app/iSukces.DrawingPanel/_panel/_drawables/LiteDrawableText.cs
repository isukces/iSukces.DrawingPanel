using System;
using System.Drawing;
using System.Windows;
using iSukces.DrawingPanel.Interfaces;
#if COMPATMATH
using WinPoint=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using WinPoint=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif

namespace iSukces.DrawingPanel;

public class LiteDrawableText : ILiteDrawable
{
    public void Draw(Graphics graphics, DrawingCanvasInfo canvasInfo)
    {
        if (_text is null)
            return;

        var          fontSizeLogic = FontSize;
        var          scale         = canvasInfo.Transformation.Scale;
        var          fontSize      = fontSizeLogic * scale;
        const string familyName    = "Arial";

        var fontSizeForMeasure = GetFontSizeForTextMeasure(fontSize, _lastFontSizeUsedForTextMeasure);
        var fontMeasureScale2  = 1.0d / (fontSizeForMeasure * 2);
        var nonScaledMeasure   = fontMeasureScale2 * fontSizeLogic;
        var scaledMeasure      = scale * nonScaledMeasure;

        using var format = new StringFormat();
        if (!_isMeasured || !_lastFontSizeUsedForTextMeasure.Equals(fontSizeForMeasure))
        {
            _lastFontSizeUsedForTextMeasure = fontSizeForMeasure;
            //System.Drawing.Drawing2D.Matrix
            // https://www.codeproject.com/Articles/2118/Bypass-Graphics-MeasureString-limitations
            _lines = _text.Split('\n');
            var measureFont = new Font(familyName, fontSizeForMeasure);
            _measure = graphics.MeasureString(Text, measureFont, new PointF(), format);
            var dy1 = (int)_verticalAlignment * _measure.Height * nonScaledMeasure;
            if (_lines.Length == 1)
            {
                _measures = [_measure];
                {
                    var dx = (float)((int)_horizontalAlignment * _measure.Width * nonScaledMeasure);
                    var p  = _point;
                    p = new WinPoint(p.X - dx, p.Y + dy1);
                    var p2 = p + new Vector(_measure.Width, -_measure.Height) * (FontSize / fontSizeForMeasure);
                    _areas = [new TwoCorners(p, p2)];
                }
            }
            else
            {
                _measures = new SizeF[_lines.Length];
                _h        = new float[_lines.Length];
                var t2 = "";
                for (var index = 0; index < _lines.Length; index++)
                {
                    var t             = _lines[index];
                    var measureString = graphics.MeasureString(t, measureFont, new PointF(), format);
                    _measures[index] = measureString;
                    if (index == 0)
                    {
                        _h[0] = measureString.Height;
                        t2    = t;
                    }
                    else
                    {
                        t2            += "\n" + t;
                        measureString =  graphics.MeasureString(t2, measureFont, new PointF(), format);
                        _h[index]     =  measureString.Height;
                    }
                }

                {
                    // na razie jest jeden obszar, ale możnaby zrobić per-linijka tekstu
                    var dx = (float)((int)_horizontalAlignment * _measure.Width * nonScaledMeasure);
                    var p  = _point;
                    p = new WinPoint(p.X - dx, p.Y + dy1);
                    var p2 = p + new Vector(_measure.Width, -_measure.Height) * (FontSize / fontSizeForMeasure);
                    _areas =
                    [
                        new TwoCorners(p, p2)
                    ];
                }
            }

            _isMeasured = true;
        }

        var rotated        = !Angle.Equals(0d);
        var savedTransform = graphics.Transform;

        if (rotated)
        {
            var p = canvasInfo.Transformation.ToCanvasF(_point);
            var x = p.X;
            var y = p.Y;
            graphics.TranslateTransform(x, y); // Set rotation point
            graphics.RotateTransform((float)Angle); // Rotate text
            graphics.TranslateTransform(-x, -y); // Rese
        }

        try
        {
            using var font = new Font(familyName, (float)fontSize);
            var       dy   = (int)_verticalAlignment * _measure.Height * scaledMeasure;
            if (_lines.Length == 1)
            {
                var dx = (float)((int)_horizontalAlignment * _measure.Width * scaledMeasure);
                var p  = canvasInfo.Transformation.ToCanvas(_point);
                p = new WinPoint(p.X - dx, p.Y - dy);
                var tmp = scaledMeasure * 2;
                if (canvasInfo.IsOutside(p.X, p.Y, _measure.Width * tmp, _measure.Height * tmp))
                    return;
                graphics.DrawString(Text, font, FontBrush, p.ToPointF(), format);
            }
            else
            {
                var p0 = canvasInfo.Transformation.ToCanvasF(_point);

                for (var i = 0; i < _lines.Length; i++)
                {
                    var m    = _measures[i];
                    var dx   = (int)_horizontalAlignment * m.Width * scaledMeasure;
                    var line = _lines[i];
                    var p    = new PointF(p0.X - (float)dx, p0.Y - (float)dy);

                    graphics.DrawString(line, font, FontBrush, p, format);
                    dy -= _h[i] * scaledMeasure * 2;
                }
            }

#if DEBUGx
            {
                foreach (var i in _areas)
                {
                    var pen = new Pen(Color.Fuchsia);
                    var p1 = canvasInfo.Transformation.ToCanvasF(i.A);
                    var p2 = canvasInfo.Transformation.ToCanvasF(i.B);
                    graphics.DrawLine(pen, p1, p2);
                }
            }
#endif
        }
        finally
        {
            if (rotated)
                graphics.Transform = savedTransform;
            savedTransform.Dispose();
        }
    }

    protected virtual float GetFontSizeForTextMeasure(double drawFontSize, float lastFontSizeUsedForTextMeasure)
    {
        return 10;
    }

    public SizeF GetLastMeasure()
    {
        return _measure;
    }

    public bool IsInside(WinPoint point, double tolerance)
    {
        if (_text is null || _areas is null)
            return false;
        foreach (var i in _areas)
        {
            if (i.IsInside(point, tolerance))
                return true;
        }

        return false;
    }

    private void SetAndNotify<T>(ref T horizontalAlignment, T value)
    {
        if (Equals(horizontalAlignment, value))
            return;
        horizontalAlignment = value;
        _isMeasured         = false;
        Changed?.Invoke(this, EventArgs.Empty);
    }

    #region properties

    public double FontSize
    {
        get => _fontSize;
        set => SetAndNotify(ref _fontSize, value);
    }


    public string Text
    {
        get => _text;
        set => SetAndNotify(ref _text, value.TrimToNull()?.Replace("\r\n", "\n"));
    }

    public WinPoint Point
    {
        get => _point;
        set => SetAndNotify(ref _point, value);
    }

    public HorizontalDrawableTextAlignment HorizontalAlignment
    {
        get => _horizontalAlignment;
        set => SetAndNotify(ref _horizontalAlignment, value);
    }

    public VerticalDrawableTextAlignment VerticalAlignment
    {
        get => _verticalAlignment;
        set => SetAndNotify(ref _verticalAlignment, value);
    }

    public Brush FontBrush
    {
        get => _fontBrush;
        set => SetAndNotify(ref _fontBrush, value);
    }

    public double Angle { get; set; }

    #endregion

    public event EventHandler Changed;

    #region Fields

    private TwoCorners[] _areas;

    private Brush _fontBrush = Brushes.White;
    private double _fontSize = 1;
    private float[] _h;

    private HorizontalDrawableTextAlignment _horizontalAlignment;
    private bool _isMeasured;
    private string[] _lines;
    private SizeF _measure;

    private SizeF[] _measures;
    private WinPoint _point;
    private string _text;
    private VerticalDrawableTextAlignment _verticalAlignment;
    private float _lastFontSizeUsedForTextMeasure = -1;

    #endregion
  
}

public enum HorizontalDrawableTextAlignment
{
    Left = 0, Center = 1, Right = 2
}

public enum VerticalDrawableTextAlignment
{
    Top, Middle, Bottom
}
