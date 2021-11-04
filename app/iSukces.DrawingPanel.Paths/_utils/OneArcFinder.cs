using System.Runtime.CompilerServices;
using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    internal partial class OneArcFinder
    {
        public ArcDefinition CalculateArc()
        {
            var p1 = StartPoint;
            var p2 = EndPoint;
            var v1 = p1 - Cross;
            var v2 = p2 - Cross;
            var l1 = v1.Length;
            var l2 = v2.Length;
            if (l1 < l2)
            {
                var scale = l1 / l2;
                if (double.IsNaN(scale))
                    return null;
                p2 = Cross + v2 * scale;
            }
            else
            {
                var scale = l2 / l1;
                if (double.IsNaN(scale))
                    return null;
                p1 = Cross + v1 * scale;
            }

            return ArcDefinition.Make(p1, StartVector, p2, EndVector);
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

        public Point  Cross       { get; set; }
        public Point  StartPoint  { get; set; }
        public Vector StartVector { get; set; }
        public Point  EndPoint    { get; set; }
        public Vector EndVector   { get; set; }
    }

#if DEBUG
    internal partial class OneArcFinder
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
            var a     = LineEquationNotNormalized.FromPointAndDeltas(StartPoint, StartVector);
            var b     = LineEquationNotNormalized.FromPointAndDeltas(EndPoint, EndVector);
            var cross = LineEquationNotNormalized.Cross(a, b);
            if (cross.HasValue)
            {
                Cross = cross.Value;
                return true;
            }

            return false;
        }
    }
#endif
}
