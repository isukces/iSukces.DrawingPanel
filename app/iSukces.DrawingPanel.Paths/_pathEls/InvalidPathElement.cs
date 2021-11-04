using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public sealed class InvalidPathElement : IPathElement
    {
        public InvalidPathElement(PathRay start, PathRay end, ArcValidationResult status)
        {
            Status = status;
            _start = start;
            _end   = end;
        }

        public static IPathResult MakeInvalid(PathRay start, PathRay end, ArcValidationResult status)
        {
            return new PathResult(new InvalidPathElement(start, end, status));
        }

        public Point GetEndPoint() { return _end.Point; }

        public Point GetStartPoint() { return _start.Point; }
        public Vector GetEndVector() { return _end.Vector; }

        public Vector GetStartVector() { return _start.Vector;}

        public ArcValidationResult Status { get; }

        private readonly PathRay _end;
        private readonly PathRay _start;
    }
}
