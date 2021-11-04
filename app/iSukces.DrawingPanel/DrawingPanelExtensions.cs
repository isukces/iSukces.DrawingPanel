using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using JetBrains.Annotations;
using Point = System.Windows.Point;


namespace iSukces.DrawingPanel
{
    public static class DrawingPanelExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Abs(this double value)
        {
            return Math.Abs(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsMoreHorizontal(this Vector vector)
        {
            return vector.X.Abs() > vector.Y.Abs();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool SameReference<T>(this T a, T b)
        {
            return ReferenceEquals(a, b);
        }

        [NotNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ToInvariantString(this int d)
        {
            return d.ToString(CultureInfo.InvariantCulture);
        }

        internal static string ToInvariantString(this double d)
        {
            return d.ToString(CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ToPointF(this Point point)
        {
            return new PointF((float)point.X, (float)point.Y);
        }
    }
}