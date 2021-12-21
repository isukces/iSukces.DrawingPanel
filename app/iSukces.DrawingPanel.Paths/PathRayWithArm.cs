using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using iSukces.Mathematics;
#if NET5_0
using iSukces.Mathematics.Compatibility;

#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    [DebuggerDisplay("{GetCreationCode()}")]
    public struct PathRayWithArm
    {
        public static implicit operator PathRayWithArm(PathRay ray) { return new PathRayWithArm(ray.Point, ray.Vector); }

        public PathRayWithArm(double pointX, double pointY, double vectorX, double vectorY,
            double armLength = 0)
            : this(new Point(pointX, pointY), new Vector(vectorX, vectorY), armLength)
        {
        }

        public PathRayWithArm(Point point, Vector vector, double armLength = 0)
        {
            if (armLength < 0)
                throw new ArgumentOutOfRangeException(nameof(armLength));
            if (armLength > 0)
            {
                vector = vector.NormalizeFast();
                if (!vector.IsValidVector())
                    throw new ArgumentOutOfRangeException(nameof(vector));
            }

            Vector    = vector;
            Point     = point;
            ArmLength = armLength;
        }

        public Vector Vector { get; }

        public Point Point { get; }

        public double ArmLength { get; }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathRay GetMovedRayOutput()
        {
            var point = Point;
            if (ArmLength > 0)
                point += Vector * ArmLength;
            return new PathRay(point, Vector);
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
        public PathRay GetRay()
        {
            var point = Point;
            return new PathRay(point, Vector);
        }

        public PathRayWithArm With(Point point) { return new PathRayWithArm(point, Vector, ArmLength); }
        public PathRayWithArm With(Vector vector) { return new PathRayWithArm(Point, vector, ArmLength); }

        public bool HasValidVector() { return Vector.IsValidVector(); }

        public WayPoint WithPoint(Point point)
        {
            var ray = new PathRay(point, Vector);
            return new WayPoint(ray);
        }


        internal string GetCreationCode()
        {
            return
                $"new {nameof(PathRayWithArm)}({Point.X.CsCode()}, {Point.Y.CsCode()}, {Vector.X.CsCode()}, {Vector.Y.CsCode()}, {ArmLength.CsCode()})";
        }


        public PathRayWithArm Normalize()
        {
            if (ArmLength > 0)
                return this;
            var vector = Vector.NormalizeFast();
            return new PathRayWithArm(Point, vector, ArmLength);
        }

        public Vector GetNormalizedVector()
        {
            if (ArmLength > 0)
                return Vector;
            var vector = Vector.NormalizeFast();
            return vector;
        }

        public PathLineEquationNotNormalized GetLine()
        {
            return PathLineEquationNotNormalized.FromPointAndDeltas(Point, Vector);
        }

        public Vector GetPrependicularVector() { return Vector.GetPrependicularVector(); }

        public Point? CrossMeAsBeginWithEnd(PathRayWithArm aEnd) { return GetMovedRayOutput().Cross(aEnd.GetMovedRayInput()); }
    }
}
