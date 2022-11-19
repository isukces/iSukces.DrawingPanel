using System;
#if COREFX
using Point=iSukces.Mathematics.Compatibility.Point;
#else
using Point=System.Windows.Point;
using System.Drawing;
#endif





namespace iSukces.DrawingPanel.Interfaces
{
    public static class DrawableTools
    {
        public static RectangleF FromCorners(Point p1, Point p2)
        {
            var x      = Math.Min(p1.X, p2.X);
            var y      = Math.Min(p1.Y, p2.Y);
            var width  = Math.Abs(p1.X - p2.X);
            var height = Math.Abs(p1.Y - p2.Y);
            return new RectangleF((float)x, (float)y, (float)width, (float)height);
        }
    }
}
