#nullable disable
using System.Collections.Generic;
using System.ComponentModel;
#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths;

[ImmutableObject(true)]
public sealed class ZeroReferencePointPathCalculatorLineResult : IPathResult
{
    private ZeroReferencePointPathCalculatorLineResult(LinePathElement line)
    {
        _line = line;
    }

    public ZeroReferencePointPathCalculatorLineResult(Point start, Point end)
    {
        _line = new LinePathElement(start, end);
    }

    public static ZeroReferencePointPathCalculatorLineResult operator +(
        ZeroReferencePointPathCalculatorLineResult a, Vector v)
    {
        if (a is null)
            return null;
        var line = a._line;
        line += v;
        return new ZeroReferencePointPathCalculatorLineResult(line);
    }

    public double GetLength()
    {
        return _line.GetLength();
    }

    public override string ToString()
    {
        return $"Line {Start} .. {End}";
    }

    public Point Start => _line.GetStartPoint();
    public Point End   => _line.GetEndPoint();

    public IReadOnlyList<IPathElement> Elements
    {
        get
        {
            return new[]
            {
                _line
            };
        }
    }

    public Vector StartVector => _line.GetStartVector();

    public Vector EndVector => _line.GetEndVector();

    #region Fields

    private readonly LinePathElement _line;

    #endregion
}
