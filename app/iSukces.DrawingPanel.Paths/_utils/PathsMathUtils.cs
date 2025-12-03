#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System;
using System.Runtime.CompilerServices;


namespace iSukces.DrawingPanel.Paths;

public static class PathsMathUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AbsGreaterThan(this double value, double limit)
    {
        if (value < 0)
            value = -value;
        return value > limit;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AbsLessThan(this double value, double limit)
    {
        if (value < 0)
            value = -value;
        return value < limit;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point Average(Point a, Point b)
    {
        var x = (a.X + b.X) * 0.5;
        var y = (a.Y + b.Y) * 0.5;
        return new Point(x, y);
    }

    /// <summary>
    ///     Assume thath a and b are normalized in both line equations
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static Point? CrossNormalized(PathLineEquationNotNormalized p1, PathLineEquationNotNormalized p2)
    {
        // var epsilon = 5.5511151231257827E-17
        const double epsilon = 1E-16;

        var a1 = p1.A;
        var b1 = p1.B;
        var a2 = p2.A;
        var b2 = p2.B;

        var determinant = a1 * b2 - a2 * b1;
        if (determinant > 0)
        {
            if (determinant < epsilon)
                return null;
        }
        else
        {
            if (determinant > -epsilon)
                return null;
        }

        var c1 = p1.C;
        var c2 = p2.C;

        var dx = b1 * c2 - b2 * c1;
        var dy = a2 * c1 - a1 * c2;

        var x = dx / determinant;
        var y = dy / determinant;
        return new Point(x, y);
    }

    /// <summary>
    ///     Assume thath a and b are normalized in both line equations
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="determinant"></param>
    /// <returns></returns>
    public static Point? CrossNormalized(PathLineEquationNotNormalized p1, PathLineEquationNotNormalized p2,
        out double determinant)
    {
        // var epsilon = 5.5511151231257827E-17
        const double epsilon = 1E-16;

        var a1 = p1.A;
        var b1 = p1.B;
        var a2 = p2.A;
        var b2 = p2.B;

        determinant = a1 * b2 - a2 * b1;
        if (determinant > 0)
        {
            if (determinant < epsilon)
                return null;
        }
        else
        {
            if (determinant > -epsilon)
                return null;
        }

        var c1 = p1.C;
        var c2 = p2.C;

        var dx = b1 * c2 - b2 * c1;
        var dy = a2 * c1 - a1 * c2;

        var x = dx / determinant;
        var y = dy / determinant;
        return new Point(x, y);
    }

    public static Point? GetCrossPoint(Point start, Vector vStart, Point end, Vector vEnd)
    {
        var l1         = PathLineEquationNotNormalized.FromPointAndDeltas(start, vStart);
        var l2         = PathLineEquationNotNormalized.FromPointAndDeltas(end, vEnd);
        var crossPoint = l1.CrossWith(l2);
        return crossPoint;
    }

    public static Vector GetNormalized(this Vector v)
    {
        v.Normalize();
        return v;
    }


    public static bool IsAngleBetweenSmallEnoughtBasedOnH(Vector vector1, Vector vector2, double h)
    {
        /*
        see :
        https://raw.githubusercontent.com/isukces/iSukces.DrawingPanel/main/doc/vector_for_arc_compare.jpg
        */

        var x1 = vector1.X;
        var x2 = vector2.X;
        var y1 = vector1.Y;
        var y2 = vector2.Y;

        var hSquare = h * h;
        var aSquare = (x1 * x1 + y1 * y1);
        var bSquare = aSquare * 0.25;

        var m = (hSquare + bSquare);
        m *= m;

        var counter      = aSquare * hSquare;
        var sinSquareByH = counter / m;

        var cross         = x1 * y2 - x2 * y1;
        var lengthSquare2 = (x2 * x2 + y2 * y2);
        var sqr           = aSquare * lengthSquare2;
        var sinusSquare   = (cross * cross) / sqr;

        //======================= 

        return sinusSquare < sinSquareByH;
    }


    /// <summary>
    ///     Assume min &lt;=max and min is &lt;0,360>, angle is &lt;0,360> and max is &lt;0,720>
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static Three IsAngleInRegion(double angle, double min, double max)
    {
        return IsAngleInRegion(angle - min, max - min);
    }

    /// <param name="angle"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Three IsAngleInRegion(double angle, double max)
    {
        angle = NormalizeAngleDeg(angle);
        if (angle <= max)
            return Three.Inside;
        var special = max * 0.5 + 180;
        if (angle <= special)
            return Three.Above;
        return Three.Below;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this Vector v)
    {
        return v.X.Equals(0d) && v.Y.Equals(0d);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double NormalizeAngleDeg(double angle)
    {
        const double full = 360;

        if (angle is >= 0 and < full)
            return angle;
        var cycles    = Math.Floor(angle / full);
        var candidate = angle - cycles * full;

        // fixes rounding errors
        if (candidate >= 360)
            return candidate - 360;
        return candidate;
    }

    #region Fields

    public const double DegreesToRadians =
        0.017453292519943295769236907684886127134428718885417254560971914401710091146034494;

    #endregion
}

public enum Three : byte
{
    Below,
    Inside,
    Above
}
