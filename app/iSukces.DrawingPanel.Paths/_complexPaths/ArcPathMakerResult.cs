namespace iSukces.DrawingPanel.Paths;

public class ArcPathMakerResult
{
    public ArcPathMakerResult(IReadOnlyList<IPathResult>? segments)
    {
        Segments = segments ?? [];
    }

    public static ArcPathMakerResult? operator +(ArcPathMakerResult? a, Vector v)
    {
        if (a is null)
            return null;
        var r = a.Segments;
        if (r.Count==0)
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
        var inSegmentLast = inSegment.Count > 0 ? inSegment[^1] : null;
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

    public IReadOnlyList<IPathResult> Segments { get; }

    public static ArcPathMakerResult Empty => new([]);

    public int Count => Segments.Count;
}


[Flags]
public enum GetBendVectorsResult
{
    None = 0,
    In = 1,
    Out = 2,
    Both = 3
}
