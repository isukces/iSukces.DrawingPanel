using System;
using System.Collections.Generic;

namespace iSukces.DrawingPanel.Paths;

public sealed class ArcPathMaker
{
    public static void Add(PathBuilder pm, IPathResult? result)
    {
        if (result is null)
            return;
        foreach (var i in result.Elements)
        {
            switch (i)
            {
                case ArcDefinition arcDefinition:
                    pm.ArcTo(arcDefinition);
                    break;
                case LinePathElement linePathElement:
                    pm.LineTo(linePathElement.GetStartPoint());
                    pm.LineTo(linePathElement.GetEndPoint());
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(i));
            }
        }
    }

    public ArcPathMakerResult? Compute()
    {
        var vertices = Vertices ?? [];
        if (vertices.Count < 2)
            return null;
        Init();
        var sm = new ArcPathSegmentMaker
        {
            PreviousPoint = vertices[0]
        };
        for (var idx = 1; idx < vertices.Count; idx++)
        {
            sm.Point     = vertices[idx];
            sm.Validator = GetPathValidator?.Invoke(idx);
            sm.Flags     = _segmentFlags[idx];
            var result = sm.MakeItem();
            _pathResults.Add(result);
            sm.PreviousPoint = sm.Point;
        }

        return new ArcPathMakerResult(_pathResults);
    }

    private void Init()
    {
        var vertices = Vertices ?? [];
        _segmentFlags = new SegmentFlags[vertices.Count];
        var prevPoint = vertices[0];
        for (var index = 1; index < vertices.Count; index++)
        {
            var point = vertices[index];
            var flags = SegmentFlags.None;
            {
                if ((prevPoint.Flags & FlexiPathMakerItem2Flags.HasOutVector) != 0)
                    flags |= SegmentFlags.HasStartVector;

                if ((point.Flags & FlexiPathMakerItem2Flags.HasInVector) != 0)
                    flags |= SegmentFlags.HasEndVector;
                if (point.ReferencePoints != null && point.ReferencePoints.Count > 0)
                    flags |= SegmentFlags.HasReferencePoints;
            }
            _segmentFlags[index] = flags;

            prevPoint = point;
        }
    }

    public Func<int, IPathValidator>? GetPathValidator { get; set; }

    public IReadOnlyList<ArcPathMakerVertex>? Vertices { get; set; }

    private readonly List<IPathResult> _pathResults = new();
    private SegmentFlags[] _segmentFlags = null!;
}

[Flags]
public enum NormalizationFlags
{
    None = 0,
    NormalizeStartVector = 1,
    NormalizeEndVector = 2,
}
