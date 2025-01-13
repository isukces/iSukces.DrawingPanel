#nullable disable
using System;
using System.Collections.Generic;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif


namespace iSukces.DrawingPanel.Paths;

public class ArcPathMakerResult
{
    public ArcPathMakerResult(IReadOnlyList<IPathResult> segments)
    {
        Segments = segments;
    }

    public static ArcPathMakerResult operator +(ArcPathMakerResult a, Vector v)
    {
        if (a is null)
            return null;
        var r = a.Segments;
        if (r is null || r.Count==0)
            return a;
        var s = new IPathResult[r.Count];
        for (var index = 0; index < r.Count; index++)
        {
            var src = r[index];
            s[index] = src.TranslateResult(v);
        }

        return new ArcPathMakerResult(s);
    }

    public GetBendVectorsResult GetBendVectors(int index, out Vector inVector, out Vector outVector)
    {
        var result = GetBendVectorsResult.None;

        var inSegment = Segments[index - 1]?.Elements;
        if (inSegment is null)
            throw new NullReferenceException($"{nameof(Segments)}[{(index - 1)}]");
        var inSegmentLast = inSegment.Count > 0 ? inSegment[inSegment.Count - 1] : null;
        if (inSegmentLast is null)
        {
            inVector = default;
        }
        else
        {
            inVector = inSegmentLast.GetEndVector();
            result   = GetBendVectorsResult.In;
        }

        var outSegment = Segments[index]?.Elements;
        if (outSegment is null)
            throw new NullReferenceException($"{nameof(Segments)}[{(index - 1)}]");
        var outSegmentFirst = outSegment.Count > 0 ? outSegment[0] : null;
        if (outSegmentFirst is null)
        {
            outVector = default;
        }
        else
        {
            outVector =  outSegmentFirst.GetStartVector();
            result    |= GetBendVectorsResult.Out;
        }

        return result;
    }

    #region properties

    public IReadOnlyList<IPathResult> Segments { get; }

    public static ArcPathMakerResult Empty => new(Array.Empty<IPathResult>());

    public int Count => Segments.Count;

    #endregion
}


[Flags]
public enum GetBendVectorsResult
{
    None = 0,
    In = 1,
    Out = 2,
    Both = 3
}
