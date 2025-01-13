#nullable disable
using System;
using System.Drawing;
#if COMPATMATH
using WinPoint=iSukces.Mathematics.Compatibility.Point;
#else
using WinPoint=System.Windows.Point;
#endif

namespace iSukces.DrawingPanel.Interfaces;

public static class DrawableTools
{
    public static RectangleF FromCorners(WinPoint p1, WinPoint p2)
    {
        var x      = Math.Min(p1.X, p2.X);
        var y      = Math.Min(p1.Y, p2.Y);
        var width  = Math.Abs(p1.X - p2.X);
        var height = Math.Abs(p1.Y - p2.Y);
        return new RectangleF((float)x, (float)y, (float)width, (float)height);
    }
}
