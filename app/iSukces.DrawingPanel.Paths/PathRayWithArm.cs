using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace iSukces.DrawingPanel.Paths;

[DebuggerDisplay("{GetCreationCode()}")]
public struct PathRayWithArm
{
    public PathRayWithArm(double pointX, double pointY, double vectorX, double vectorY,
        double armLength = 0)
        : this(new Point(pointX, pointY), new Vector(vectorX, vectorY), armLength)
    {
    }

    [JsonConstructor]
    public PathRayWithArm(Point point, Vector vector, double armLength = 0)
    {
        if (armLength < 0)
            throw new ArgumentOutOfRangeException(nameof(armLength));
        if (armLength > 0)
        {
            vector = vector.GetNormalized();
            if (!vector.IsValidVector())
                throw new ArgumentOutOfRangeException(nameof(vector));
        }

        Vector    = vector;
        Point     = point;
        ArmLength = armLength;
    }


    public static PathRayWithArm operator +(PathRayWithArm a, Vector v)
    {
        return new PathRayWithArm(a.Point + v, a.Vector, a.ArmLength);
    }

    public static implicit operator PathRayWithArm(PathRay ray)
    {
        return new PathRayWithArm(ray.Point, ray.Vector);
    }

    public Point? CrossMeAsBeginWithEnd(PathRayWithArm aEnd)
    {
        return GetMovedRayOutput().Cross(aEnd.GetMovedRayInput());
    }


    internal string GetCreationCode()
    {
        return
            $"new {nameof(PathRayWithArm)}({Point.X.CsCode()}, {Point.Y.CsCode()}, {Vector.X.CsCode()}, {Vector.Y.CsCode()}, {ArmLength.CsCode()})";
    }

    public PathLineEquationNotNormalized GetLine()
    {
        return PathLineEquationNotNormalized.FromPointAndDeltas(Point, Vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PathRay GetMovedRayInput()
    {
        var point = Point;
        if (ArmLength > 0)
            point -= Vector * ArmLength;
        return new PathRay(point, Vector);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PathRay GetMovedRayOutput()
    {
        var point = Point;
        if (ArmLength > 0)
            point += Vector * ArmLength;
        return new PathRay(point, Vector);
    }

    public Vector GetNormalizedVector()
    {
        if (ArmLength > 0)
            return Vector;
        var vector = Vector.GetNormalized();
        return vector;
    }

    public Vector GetPrependicularVector()
    {
        return Vector.GetPrependicularVector();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PathRay GetRay()
    {
        var point = Point;
        return new PathRay(point, Vector);
    }

    public bool HasValidVector()
    {
        return Vector.IsValidVector();
    }


    public PathRayWithArm Normalize()
    {
        if (ArmLength > 0)
            return this;
        var vector = Vector.GetNormalized();
        return new PathRayWithArm(Point, vector, ArmLength);
    }


    public bool ShouldSerializeArmLength()
    {
        return ArmLength != 0d;
    }

    public PathRayWithArm With(Point point)
    {
        return new PathRayWithArm(point, Vector, ArmLength);
    }

    public PathRayWithArm With(Vector vector)
    {
        return new PathRayWithArm(Point, vector, ArmLength);
    }

    public WayPoint WithPoint(Point point)
    {
        var ray = new PathRay(point, Vector);
        return new WayPoint(ray);
    }

    public Vector Vector { get; }

    public Point Point { get; }

    public double ArmLength { get; }
}
