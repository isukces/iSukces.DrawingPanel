using System;
using System.Collections.Generic;
#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif

namespace iSukces.DrawingPanel.Paths;

public static class MoveUtils
{
    private static IPathElement TranslateElement(this IPathElement element, Vector vector)
    {
        switch (element)
        {
            case null:
                return null;
            case ArcDefinition arcDefinition:
                return arcDefinition + vector;
            case LinePathElement linePathElement:
                return linePathElement + vector;
            default: throw new ArgumentOutOfRangeException(nameof(element));
        }
    }

    public static IReadOnlyList<IPathElement> TranslateElementList(IReadOnlyList<IPathElement> src, Vector vector)
    {
        if (src is null)
            return null;
        if (src.Count == 0)
            return Array.Empty<IPathElement>();
        var result = new IPathElement[src.Count];
        for (var index = 0; index < src.Count; index++)
        {
            var element = src[index];
            element       = TranslateElement(element, vector);
            result[index] = element;
        }

        return result;
    }

    public static IPathResult TranslateResult(this IPathResult src, Vector vector)
    {
        switch (src)
        {
            case null:
                return null;
            case PathResult r1: return r1 + vector;
            case ZeroReferencePointPathCalculatorLineResult r2: return r2 + vector;
            case ZeroReferencePointPathCalculatorResult r3: return r3 + vector;
            default: throw new ArgumentOutOfRangeException(nameof(src));
        }
    }
}