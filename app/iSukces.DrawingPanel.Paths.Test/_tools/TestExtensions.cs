using System;
using System.Globalization;
using System.Windows;
using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal static class TestExtensions
    {
        public static string ToCs(this double x) { return x.ToString(CultureInfo.InvariantCulture); }
        public static string ToCs(this int x) { return x.ToString(CultureInfo.InvariantCulture); }

        public static string ToCs(this Point x) { return $"new Point({x.X.ToCs()}, {x.Y.ToCs()})"; }

        public static string ToCs(this Vector x) { return $"new Vector({x.X.ToCs()}, {x.Y.ToCs()})"; }

        public static double Angle(double x, double y)
        {
            if (x.Equals(0d))
                switch (Math.Sign(y))
                {
                    case 1:
                        return 90d;
                    case -1:
                        return 270d;
                    default:
                        return 0;
                }

            if (y.Equals(0d))
                return x < 0 ? 180d : 0d;

            var angle = MathEx.Atan2Deg(y, x);
            if (angle < 0)
                angle += 360;
            return angle;
        }

        public static double AngleMinusY(this Vector v) { return Angle(v.X, -v.Y); }
    }
}
