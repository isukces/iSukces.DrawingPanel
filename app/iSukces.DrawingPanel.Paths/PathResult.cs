#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;


namespace iSukces.DrawingPanel.Paths;

public class PathResult : IPathResult
{
    public PathResult(Point start, Point end, IReadOnlyList<IPathElement>? elements)
    {
        Start    = start;
        End      = end;
        Elements = elements ?? [];
    }

    public PathResult(IReadOnlyList<IPathElement> arcs)
    {
        Start    = arcs[0].GetStartPoint();
        End      = arcs[^1].GetEndPoint();
        Elements = arcs;
    }

    public PathResult(IPathElement element)
    {
        Start    = element.GetStartPoint();
        End      = element.GetEndPoint();
        Elements = [element];
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IReadOnlyList<IPathElement> GetNotNull(IPathElement? a, IPathElement? b)
    {
        if (a is null)
            return b is null
                ? []
                : new[] { b };

        return b is null
            ? new[] { a }
            : new[] { a, b };
    }

    public static PathResult Make(Point start, Point end, IPathElement el1, IPathElement el2)
    {
        return new PathResult(start, end, GetNotNull(el1, el2));
    }

    public static PathResult Make(Point start, Point end, IPathElement? element)
    {
        var elements = element is null
            ? Array.Empty<IPathElement>()
            : new[] { element };
        return new PathResult(start, end, elements);
    }

    [return: NotNullIfNotNull(nameof(src))]
    public static PathResult? operator +(PathResult? src, Vector v)
    {
        if (src is null)
            return null;
        return new PathResult(src.Start + v, src.End + v, MoveUtils.TranslateElementList(src.Elements, v));
    }

    public override string ToString()
    {
        return $"Start: {Start}, End: {End}, Count: {Elements.Count}";
    }

    public Point                       Start    { get; }
    public Point                       End      { get; }
    public IReadOnlyList<IPathElement> Elements { get; }

    public Vector StartVector
    {
        get
        {
            if (Elements.Count == 0)
                return default;
            return Elements[0].GetStartVector();
        }
    }

    public Vector EndVector
    {
        get
        {
            if (Elements.Count == 0)
                return default;
            return Elements[^1].GetEndVector();
        }
    }
}
