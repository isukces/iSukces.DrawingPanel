#nullable disable
#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System.Collections.Generic;


namespace iSukces.DrawingPanel.Paths;

/// <summary>
///     Allows moving path parallelly left or right.
/// </summary>
public sealed class VerticesValidator
{
    private VerticesValidator(IReadOnlyList<ArcPathMakerVertex> list)
    {
        _list      = list;
        _listCount = list.Count;
        _exists    = new bool[_listCount];
        _vectors   = new Vector[_listCount];
    }

    public static IReadOnlyList<ArcPathMakerVertex> FillMissingVectors(IReadOnlyList<ArcPathMakerVertex> list)
    {
        if (list is null || list.Count < 2)
            return list;
        var a = new VerticesValidator(list);
        return a.FillMissingVectorsInternal();
    }

    private IReadOnlyList<ArcPathMakerVertex> FillMissingVectorsInternal()
    {
        var resultList = new ArcPathMakerVertex[_listCount];
        for (var index = 0; index < _listCount; index++)
        {
            var src    = _list[index];
            var vertex = src.DeepClone();

            var inZero  = (src.Flags & FlexiPathMakerItem2Flags.HasInVector) == 0;
            var outZero = (src.Flags & FlexiPathMakerItem2Flags.HasOutVector) == 0;

            if (index > 0)
            {
                if (inZero)
                {
                    var v = GetSegmentVector(index);
                    if (!double.IsNaN(v.X))
                        vertex.WithInVector(v);
                }
                else
                    vertex.NormalizeInVector();
            }

            if (index < _listCount - 1)
            {
                if (outZero)
                {
                    var v = GetSegmentVector(index + 1);
                    if (!double.IsNaN(v.X))
                        vertex.WithOutVector(v);
                }
                else
                    vertex.NormalizeOutVector();
            }

            resultList[index] = vertex;
        }

        return resultList;
    }


    private Vector GetSegmentVector(int index)
    {
        // oblicza wektor z keszowaniem
        if (index == 0)
            index = 1;
        if (_exists[index])
            return _vectors[index];
        var p1 = _list[index - 1].Location;
        var p2 = _list[index].Location;
        var v  = p2 - p1;
        v.Normalize();
        _vectors[index] = v;
        _exists[index]  = true;
        return v;
    }

    #region Fields

    private readonly bool[] _exists;
    private readonly IReadOnlyList<ArcPathMakerVertex> _list;
    private readonly int _listCount;
    private readonly Vector[] _vectors;

    #endregion
}
