#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths;

public class PathResult : IPathResult
{
    public PathResult(Point start, Point end, IReadOnlyList<IPathElement> arcs)
    {
        Start    = start;
        End      = end;
        Elements = arcs ?? Array.Empty<IPathElement>();
    }

    public PathResult(IReadOnlyList<IPathElement> arcs)
    {
        Start    = arcs[0].GetStartPoint();
        End      = arcs.Last().GetEndPoint();
        Elements = arcs;
    }

    public PathResult([NotNull] IPathElement element)
    {
        Start    = element.GetStartPoint();
        End      = element.GetEndPoint();
        Elements = new[] { element };
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IReadOnlyList<IPathElement> GetNotNull(IPathElement a, IPathElement b)
    {
        if (a is null)
            return b is null
                ? Array.Empty<IPathElement>()
                : new[] { b };

        return b is null
            ? new[] { a }
            : new[] { a, b };
    }

    public static PathResult Make(Point start, Point end, IPathElement el1, IPathElement el2)
    {
        return new PathResult(start, end, GetNotNull(el1, el2));
    }

    public static PathResult Make(Point start, Point end, IPathElement element)
    {
        var elements = element is null
            ? Array.Empty<IPathElement>()
            : new[] { element };
        return new PathResult(start, end, elements);
    }

    public static PathResult operator +(PathResult src, Vector v)
    {
        if (src is null)
            return null;
        return new PathResult(src.Start + v, src.End + v, MoveUtils.TranslateElementList(src.Elements, v));
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
            return Elements[Elements.Count - 1].GetEndVector();
        }
    }
}
