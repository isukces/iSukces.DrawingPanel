using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public sealed class InvalidPathElement : IPathElement
    {
        public InvalidPathElement(PathRay start, PathRay end, ArcValidationResult status)
        {
            Status  = status;
            _start  = start;
            _end    = end;
            _vector = end.Point - _start.Point;
        }

        public static IPathResult MakeInvalid(PathRay start, PathRay end, ArcValidationResult status)
        {
            return new PathResult(new InvalidPathElement(start, end, status));
        }

        public Point GetEndPoint() { return _end.Point; }
        public Vector GetEndVector() { return _end.Vector; }

        public double GetLength() { return _vector.Length; }

        public Point GetStartPoint() { return _start.Point; }

        public Vector GetStartVector() { return _start.Vector; }

        public bool IsLineCollision(Point hitPoint, double toleranceSquared, out double distanceSquared)
        {
            var line    = LineEquationNotNormalized.FromPointAndDeltas(_start.Point, _vector);
            var counter = line.DistanceNotNormalized(hitPoint);
            counter *= counter;

            var determinant = line.Get_determinant_squared();
            distanceSquared = counter / determinant;

            return distanceSquared <= toleranceSquared;
        }

        public ArcValidationResult Status { get; }

        private readonly PathRay _end;
        private readonly PathRay _start;
        private readonly Vector _vector;
    }
}
