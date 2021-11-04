using System;
using System.Windows;

namespace iSukces.DrawingPanel
{
    public sealed class TwoCorners
    {
        public TwoCorners(Point a, Point b)
        {
            A = a;
            B = b;
        }

        public bool IsInside(Point cp, double tolerance)
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

        public Point A { get; }
        public Point B { get; }
    }
}
