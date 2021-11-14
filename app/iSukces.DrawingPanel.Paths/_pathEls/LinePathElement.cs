using System.Runtime.CompilerServices;
using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public class LinePathElement : IPathElement, ILineCollider
    {
        public LinePathElement(Point start, Point end)
        {
            _start  = start;
            _end    = end;
            _vector = _end - _start;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point GetEndPoint() { return _end; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector GetEndVector() { return _vector; }

        public double GetLength() { return _vector.Length; }

        public Point GetNearestPoint(Point point)
        {
            var line = LineEquationNotNormalized.FromPointAndDeltas(_start, _vector);
            return line.GetNearestPoint(point);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point GetStartPoint() { return _start; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector GetStartVector() { return _vector; }

        public bool IsLineCollision(Point hitPoint, double toleranceSquared, out double distanceSquared,
            out Point correctedPoint)
        {
            var line    = LineEquationNotNormalized.FromPointAndDeltas(_start, _vector);
            var counter = line.DistanceNotNormalized(hitPoint);
            counter *= counter;

            var determinant = line.GetDeterminantSquared();
            distanceSquared = counter / determinant;

            if (distanceSquared <= toleranceSquared)
            {
                correctedPoint = line.GetNearestPoint(hitPoint);
                return true;
            }

            correctedPoint = default;
            return false;
        }

        private readonly Point _end;
        private readonly Point _start;
        private readonly Vector _vector;
    }
}
