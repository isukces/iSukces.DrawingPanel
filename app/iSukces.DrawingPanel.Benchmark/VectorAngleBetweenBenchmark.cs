using System;
using BenchmarkDotNet.Attributes;
using iSukces.DrawingPanel.Paths;
#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Benchmark;

[SimpleJob(8)]
public class VectorAngleBetweenBenchmark
{
    public static double AngleBetweenAlt(Vector vector1, Vector vector2)
    {
        var x1 = vector1.X;
        var x2 = vector2.X;
        var y1 = vector1.Y;
        var y2 = vector2.Y;

        var y = x1 * y2 - x2 * y1;
        var x = x1 * x2 + y1 * y2;
        return Math.Atan2(y, x);
    }

    public static double AngleBetweenAlt2(Vector vector1, Vector vector2)
    {
        var x1 = vector1.X;
        var x2 = vector2.X;
        var y1 = vector1.Y;
        var y2 = vector2.Y;

        var y = x1 * y2 - x2 * y1;
        var x = x1 * x2 + y1 * y2;
        return y / x;
    }


    public static double AngleBetweenAlt3(Vector vector1, Vector vector2)
    {
        var x1 = vector1.X;
        var x2 = vector2.X;
        var y1 = vector1.Y;
        var y2 = vector2.Y;

        var x   = x1 * x2 + y1 * y2;
        var sqr = (x1 * x1 + y1 * y1) * (x2 * x2 + y2 * y2);
        return x / Math.Sqrt(sqr);
    }


    [Benchmark(Description = "IsAngleBetweenSmallEnoughtBasedOnH")]
    public bool IsAngleBetweenSmallEnoughtBasedOnH()
    {
        return PathsMathUtils.IsAngleBetweenSmallEnoughtBasedOnH(_v1, _v2, 0.001);
    }

    [Benchmark(Description = "AngleBetween", Baseline = true)]
    public double Vector_angle_between() { return Vector.AngleBetween(_v1, _v2); }

    [Benchmark(Description = "AngleBetween ALT")]
    public double Vector_angle_between_alt() { return AngleBetweenAlt(_v1, _v2); }

    [Benchmark(Description = "AngleBetween ALT 2")]
    public double Vector_angle_between_alt2() { return AngleBetweenAlt2(_v1, _v2); }


    [Benchmark(Description = "AngleBetween ALT 3")]
    public double Vector_angle_between_alt3() { return AngleBetweenAlt3(_v1, _v2); }


    private readonly Vector _v1 = new(1, 2);
    private readonly Vector _v2 = new(3, 1);
}