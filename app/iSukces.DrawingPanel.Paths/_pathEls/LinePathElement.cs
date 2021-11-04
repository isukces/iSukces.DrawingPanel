using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public class LinePathElement : IPathElement
    {
        public LinePathElement(Point start, Point end)
        {
            _start  = start;
            _end    = end;
            _vector = _end - _start;
        }

        public Point GetEndPoint() { return _end; }

        public Vector GetEndVector() { return _vector; }

        public Point GetStartPoint() { return _start; }

        public Vector GetStartVector() { return _vector; }

        private readonly Point _end;

        private readonly Point _start;
        private readonly Vector _vector;
    }
}
