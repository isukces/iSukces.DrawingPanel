using System.Runtime.CompilerServices;
#if NET5_0
using iSukces.Mathematics.Compatibility;

#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    public class LinePathElement : IPathElement, ILineCollider
    {
        public LinePathElement(Point start, Point end)
        {
            _start      = start;
            _end        = end;
            _vector     = _end - _start;
            _unitVector = _vector.NormalizeFast(out _length);
        }


        public double DistanceFromElement(Point point, out double distanceFromStart, out Vector direction)
        {
            var v = _vector.NormalizeFast();
            direction = _unitVector;
            var vToBegin = point - _start;
            var loc      = v * vToBegin;
            if (loc <= 0)
            {
                var g = vToBegin.Length;
                distanceFromStart = 0;
                return g < 0 ? -g : g;
            }

            var endLoc = v * _vector; // == _vector.Length
            if (loc >= endLoc)
            {
                var g = (_end - point).Length;
                distanceFromStart = _length;
                return g < 0 ? -g : g;
            }

            var l    = PathLineEquationNotNormalized.FromPointAndDeltas(_start, v);
            var dist = l.DistanceNotNormalized(point);
            distanceFromStart = loc;
            return dist < 0 ? -dist : dist;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point GetEndPoint()
        {
            return _end;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector GetEndVector()
        {
            return _unitVector;
        }

        public double GetLength()
        {
            return _length;
        }

        public Point GetNearestPoint(Point point)
        {
            var line = PathLineEquationNotNormalized.FromPointAndDeltas(_start, _vector);
            return line.GetNearestPoint(point);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point GetStartPoint()
        {
            return _start;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector GetStartVector()
        {
            return _unitVector;
        }

        public bool IsLineCollision(Point hitPoint, double toleranceSquared, out double distanceSquared,
            out Point correctedPoint)
        {
            var line    = PathLineEquationNotNormalized.FromPointAndDeltas(_start, _vector);
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

        public LinePathElement WithEndPoint(Point newEnd)
        {
            return new LinePathElement(_start, newEnd);
        }

        public LinePathElement WithStartPoint(Point newStart)
        {
            return new LinePathElement(newStart, _end);
        }

        public object Tag { get; set; }

        public Vector UnitVector => _unitVector;
        public Point  Start      => _start;
        public Point  End        => _end;
        public double Length     => _length;

        private readonly Point _end;
        private readonly double _length;
        private readonly Point _start;
        private readonly Vector _unitVector;
        private readonly Vector _vector;
    }
}
