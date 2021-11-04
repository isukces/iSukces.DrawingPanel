using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public static class MathUtils
    {
        public static Point Average(Point a, Point b)
        {
            var x = (a.X + b.X) * 0.5;
            var y = (a.Y + b.Y) * 0.5;
            return new Point(x, y);
        }

        public static Point? GetCrossPoint(Point start, Vector vStart, Point end, Vector vEnd)
        {
            var l1         = LineEquationNotNormalized.FromPointAndDeltas(start, vStart);
            var l2         = LineEquationNotNormalized.FromPointAndDeltas(end, vEnd);
            var crossPoint = l1.CrossWith(l2);
            return crossPoint;
        }
    }
}
