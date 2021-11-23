using System.Runtime.CompilerServices;
using Newtonsoft.Json;
#if NET5_0
using iSukces.Mathematics.Compatibility;

#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    public struct PathRay
    {
        [JsonConstructor]
        public PathRay(Point point, Vector vector)
        {
            Point  = point;
            Vector = vector;
        }

        public PathRay(double x, double y, double vx, double vy)
            : this(new Point(x, y), new Vector(vx, vy))
        {
        }

        public PathRay(double x, double y)
            : this(new Point(x, y), default(Vector))
        {
        }

        public PathRay(Point point, Point endPoint)
        {
            Point  = point;
            Vector = endPoint - point;
            Vector.Normalize();
        }

        public override string ToString() { return $"{Point} => {Vector}"; }

        public Point? Cross(PathRay other)
        {
            var l1         = GetLine();
            var l2         = other.GetLine();
            var crossPoint = l1.CrossWith(l2);
            return crossPoint;
        }

        public Point? Cross(Point p, Vector v)
        {
            var l1         = GetLine();
            var l2         = LineEquationNotNormalized.FromPointAndDeltas(p, v);
            var crossPoint = l1.CrossWith(l2);
            return crossPoint;
        }

        public Vector Vector { get; }

        public Point Point { get; }


        public LineEquationNotNormalized GetLine()
        {
            return LineEquationNotNormalized.FromPointAndDeltas(Point, Vector);
        }

        public PathRay With(Point point) { return new PathRay(point, Vector); }

        public PathRay With(Vector vector) { return new PathRay(Point, vector); }

        public Vector GetNormalizedVector() { return Vector.GetNormalizedVector(); }

        public Vector GetPrependicularVector() { return Vector.GetPrependicularVector(); }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathRay WithInvertedVector() { return new PathRay(Point, -Vector); }

        public PathRay WithEnd(Point end, double length)
        {
            var vector = end - Point;
            vector *= length / vector.Length;
            return new PathRay(Point, vector);
        }

        public PathRay Normalize()
        {
            var v = Vector;
            v = v.NormalizeFast();
            return new PathRay(Point, v);
        }

        public Point GetPoint(double d) { return Point + d * Vector; }

        public Point Get(double distance) { return Point + Vector * distance; }

        public PathRay WithPoint(Point point) { return new PathRay(point, Vector); }

        public bool HasValidVector()
        {
            var vector = Vector;
            var x      = vector.X;
            if (double.IsNaN(x))
                return false;
            var y = vector.Y;
            if (double.IsNaN(y))
                return false;
            if (y != 0)
                return true;

            if (x != 0)
                return true;
            return false;
        }
    }
}
