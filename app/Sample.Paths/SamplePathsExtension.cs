using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using iSukces.Mathematics;
using Vector = iSukces.Mathematics.Vector;

namespace Sample.Paths;

public static class SamplePathsExtension
{
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

    public static double AngleMinusY(this Vector v)
    {
        return Angle(v.X, -v.Y);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string ToInv(this double d)
    {
        return d.ToString(CultureInfo.InvariantCulture);
    }
}
