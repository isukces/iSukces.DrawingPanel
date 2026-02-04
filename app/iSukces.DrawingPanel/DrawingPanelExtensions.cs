using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using Point=iSukces.Mathematics.Point;
using Vector=iSukces.Mathematics.Vector;

namespace iSukces.DrawingPanel;

public static class DrawingPanelExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double Abs(this double value) { return Math.Abs(value); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMoreHorizontal(this Vector vector) { return vector.X.Abs() > vector.Y.Abs(); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool SameReference<T>(this T a, T b) { return ReferenceEquals(a, b); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string ToInvariantString(this int d) { return d.ToString(CultureInfo.InvariantCulture); }

    internal static string ToInvariantString(this double d) { return d.ToString(CultureInfo.InvariantCulture); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PointF ToPointF(this Point point) { return new PointF((float)point.X, (float)point.Y); }
}
