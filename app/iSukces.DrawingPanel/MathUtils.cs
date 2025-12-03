using System;

namespace iSukces.DrawingPanel;

internal sealed class MathUtils
{
    public static double Round05(double value) { return Math.Round(value + 0.5) - 0.5; }

    public static long RoundToLong(double d)
    {
        if (d <= long.MinValue) return long.MinValue;
        if (d >= long.MaxValue) return long.MaxValue;
        return (long)Math.Round(d);
    }
}
