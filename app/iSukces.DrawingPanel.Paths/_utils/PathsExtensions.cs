#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
#endif
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;


namespace iSukces.DrawingPanel.Paths;

internal static partial class PathsExtensions
{
    /*public static double AngleMinusY(this Vector v)
    {
        return Angle(new Vector(v.X, -v.Y));
    }*/
    public static double Angle(this Vector v)
    {
        if (v.X.Equals(0d))
            switch (Math.Sign(v.Y))
            {
                case 1:
                    return 90d;
                case -1:
                    return 270d;
                default:
                    return 0;
            }

        if (v.Y.Equals(0d))
            return v.X < 0 ? 180d : 0d;

        var angle = MathEx.Atan2Deg(v);
        if (angle < 0)
            angle += 360;
        return angle;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Conditional("DEBUG")]
    public static void CheckNaNDebug(this double value, string argumentName)
    {
        if (double.IsNaN(value))
            throw new ArgumentException(argumentName);
    }


    internal static string CsCode(this double d)
    {
        return d.ToString("R", CultureInfo.InvariantCulture);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector GetPrependicularVector(this Vector vector)
    {
        return vector.GetPrependicular(false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidVector(this Vector vector)
    {
        var x = vector.X;
        if (double.IsNaN(x))
            return false;
        var y = vector.Y;
        if (double.IsNaN(y))
            return false;
        if (y != 0)
            return true;

        if (x != 0)
            return true;
        return false;
    }

}
