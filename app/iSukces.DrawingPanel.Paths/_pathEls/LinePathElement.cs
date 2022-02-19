using System.Runtime.CompilerServices;
using iSukces.Mathematics;
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

        private LinePathElement(Point start, Point end,
            double length, Vector vector, Vector unitVector)
        {
            _end        = end;
            _length     = length;
            _start      = start;
            _unitVector = unitVector;
            _vector     = vector;
        }

        public static LinePathElement operator +(LinePathElement a, Vector v)
        {
            if (a is null)
                return null;
            return new LinePathElement(a._start + v, a._end + v, a._length, a._vector, a._unitVector);
        }

        public double DistanceFromElement(Point point, out double distanceFromStart, out Vector direction)
        {
            direction = _unitVector;
            var vToBegin = point - _start;
            distanceFromStart = _unitVector * vToBegin;
            if (distanceFromStart <= 0)
            {
                var g = vToBegin.Length;
                distanceFromStart = 0;
                return g < 0 ? -g : g;
            }

            if (distanceFromStart >= _length)
            {
                var g = (_end - point).Length;
                distanceFromStart = _length;
                return g < 0 ? -g : g;
            }

            var dist2 = Vector.CrossProduct(direction, vToBegin);
            return dist2 < 0 ? -dist2 : dist2;
        }

        public ClosestPointResult FindClosestPointOnElement(Point target)
        {
            var vToBegin = target - _start;
            var track    = _unitVector * vToBegin;

            var prep = _unitVector.GetPrependicular(false);
            var side = prep * vToBegin;
            if (track <= 0)
            {
                var l = track < 0 ? Three.Below : Three.Inside;
                return new ClosestPointResult(_start, l, track, side, _unitVector);
            }

            if (track >= _length)
            {
                var l = track > _length ? Three.Above : Three.Inside;
                return new ClosestPointResult(_end, l, track, side, _unitVector);
            }

            return new ClosestPointResult(_start + track * _unitVector, Three.Inside, track, side, _unitVector);
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

        #region properties

        public Vector UnitVector => _unitVector;
        public Point  Start      => _start;
        public Point  End        => _end;
        public double Length     => _length;

        #endregion

        public object Tag { get; set; }

        #region Fields

        private readonly Point _end;
        private readonly double _length;
        private readonly Point _start;
        private readonly Vector _unitVector;
        private readonly Vector _vector;

        #endregion
    }
}
