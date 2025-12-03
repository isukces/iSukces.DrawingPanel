#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System.Runtime.CompilerServices;

namespace iSukces.DrawingPanel.Paths;

public partial class OneArcFinder
{
    public OneArcFinder(Point cross)
    {
        Cross = cross;
    }

    public ArcDefinition? CalculateArc()
    {
        var start              = StartPoint;
        var end                = EndPoint;
        var startToCross       = start - Cross;
        var endToCross         = end - Cross;
        var startToCrossLength = startToCross.Length;
        var endToCrossLength   = endToCross.Length;

        bool usePointsFurtherFromCrossPoint;
        {
            var a = startToCross * StartVector;
            var b = endToCross * EndVector;
            usePointsFurtherFromCrossPoint = a > 0 && b < 0;
        }
        if (usePointsFurtherFromCrossPoint ^ startToCrossLength < endToCrossLength)
        {
            var scale = startToCrossLength / endToCrossLength;
            if (double.IsNaN(scale))
                return null;
            end = Cross + endToCross * scale;
        }
        else
        {
            var scale = endToCrossLength / startToCrossLength;
            if (double.IsNaN(scale))
                return null;
            start = Cross + startToCross * scale;
        }

        return ArcDefinition.Make(start, StartVector, end, EndVector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Setup(PathRay start, PathRay end)
    {
        StartPoint  = start.Point;
        StartVector = start.Vector;
        EndPoint    = end.Point;
        EndVector   = end.Vector;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetupReverseEnd(PathRay start, PathRay end)
    {
        StartPoint  = start.Point;
        StartVector = start.Vector;
        EndPoint    = end.Point;
        EndVector   = -end.Vector;
    }

    #region properties

    public Point  Cross       { get; set; }
    public Point  StartPoint  { get; set; }
    public Vector StartVector { get; set; }
    public Point  EndPoint    { get; set; }
    public Vector EndVector   { get; set; }

    #endregion
}

#if DEBUG
public partial class OneArcFinder
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Setup(PathRay start, Point endPoint, Vector endVector)
    {
        StartPoint  = start.Point;
        StartVector = start.Vector;
        EndPoint    = endPoint;
        EndVector   = endVector;
    }

    public bool UpdateCross()
    {
        var a     = PathLineEquationNotNormalized.FromPointAndDeltas(StartPoint, StartVector);
        var b     = PathLineEquationNotNormalized.FromPointAndDeltas(EndPoint, EndVector);
        var cross = PathLineEquationNotNormalized.Cross(a, b);
        if (cross.HasValue)
        {
            Cross = cross.Value;
            return true;
        }

        return false;
    }
}
#endif
