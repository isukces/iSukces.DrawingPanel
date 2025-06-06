#nullable disable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using iSukces.Mathematics;
using Newtonsoft.Json;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif


namespace iSukces.DrawingPanel.Paths;

[DebuggerDisplay("{GetCreationCode()}")]
public struct PathRay
{
    public static PathRay operator +(PathRay a, Vector v)
    {
        return new PathRay(a.Point + v, a.Vector);
    }

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

    public override string ToString()
    {
        return $"{Point} => {Vector}";
    }

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
        var l2         = PathLineEquationNotNormalized.FromPointAndDeltas(p, v);
        var crossPoint = l1.CrossWith(l2);
        return crossPoint;
    }

    internal string GetCreationCode()
    {
        return $"new PathRay({Point.X.CsCode()}, {Point.Y.CsCode()}, {Vector.X.CsCode()}, {Vector.Y.CsCode()})";
    }

    public Vector Vector { get; }

    public Point Point { get; }

    public PathLineEquationNotNormalized GetLine()
    {
        return PathLineEquationNotNormalized.FromPointAndDeltas(Point, Vector);
    }

    public PathRay With(Point point)
    {
        return new PathRay(point, Vector);
    }

    public PathRay With(Vector vector)
    {
        return new PathRay(Point, vector);
    }

    public Vector GetNormalizedVector()
    {
        return Vector.GetNormalized();
    }

    public Vector GetPrependicularVector()
    {
        return Vector.GetPrependicularVector();
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PathRay WithInvertedVector()
    {
        return new PathRay(Point, -Vector);
    }

    public PathRay WithEnd(Point end, double length)
    {
        var vector = end - Point;
        vector *= length / vector.Length;
        return new PathRay(Point, vector);
    }

    public PathRay Normalize()
    {
        var v = Vector;
        v.Normalize();
        return new PathRay(Point, v);
    }

    public Point GetPoint(double d)
    {
        return Point + d * Vector;
    }

    public Point Get(double distance)
    {
        return Point + Vector * distance;
    }

    public PathRay WithPoint(Point point)
    {
        return new PathRay(point, Vector);
    }

    public bool HasValidVector()
    {
        return Vector.IsValidVector();
    }
}
