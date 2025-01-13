#nullable disable
using System;
using System.IO;

namespace iSukces.DrawingPanel.Paths.Test;

internal static class TestNamesSorter
{
    private static int GetGroup(string name)
    {
        if (name.StartsWith("Zero", StringComparison.OrdinalIgnoreCase))
            return 0;
        if (name.StartsWith("One", StringComparison.OrdinalIgnoreCase))
            return 1;
        if (name.StartsWith("Two", StringComparison.OrdinalIgnoreCase))
            return 2;
        if (name.StartsWith("Three", StringComparison.OrdinalIgnoreCase))
            return 3;
        return 99;
    }

    public static int Sort(FileInfo x, FileInfo y) { return Sort(x.Name, y.Name); }

    private static int Sort(string x, string y)
    {
        var xg = GetGroup(x);
        var yg = GetGroup(y);
        var c  = xg.CompareTo(yg);
        if (c != 0)
            return c;
        c = string.Compare(x, y, StringComparison.OrdinalIgnoreCase);

        return c;
    }
}
