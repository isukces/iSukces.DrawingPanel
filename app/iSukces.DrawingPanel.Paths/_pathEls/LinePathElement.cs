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

        public Point GetEndPoint() { return _end; }

        public Vector GetEndVector() { return _vector; }

        public double GetLength() { return _vector.Length; }

        public Point GetStartPoint() { return _start; }

        public Vector GetStartVector() { return _vector; }

        public bool IsLineCollision(Point hitPoint, double toleranceSquared, out double distanceSquared)
        {
            var line    = LineEquationNotNormalized.FromPointAndDeltas(_start, _vector);
            var counter = line.DistanceNotNormalized(hitPoint);
            counter *= counter;

            var determinant = line.Get_determinant_squared();
            distanceSquared = counter / determinant;

            return distanceSquared <= toleranceSquared;
        }

        private readonly Point _end;
        private readonly Point _start;
        private readonly Vector _vector;
    }
}
