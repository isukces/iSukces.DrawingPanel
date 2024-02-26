using System;
#if COMPATMATH
using WinPoint=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using WinPoint=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif

namespace iSukces.DrawingPanel;

public sealed class TwoCorners
{
    public TwoCorners(WinPoint a, WinPoint b)
    {
        A = a;
        B = b;
    }

    public bool IsInside(WinPoint cp, double tolerance)
    {
        return cp.X + tolerance >= XMinimum
               && cp.X - tolerance <= XMaximum
               && cp.Y + tolerance >= YMinimum
               && cp.Y - tolerance <= YMaximum;
    }

    private double XMinimum => Math.Min(A.X, B.X);
    private double XMaximum => Math.Max(A.X, B.X);

    private double YMinimum => Math.Min(A.Y, B.Y);
    private double YMaximum => Math.Max(A.Y, B.Y);

    public WinPoint A { get; }
    public WinPoint B { get; }
}