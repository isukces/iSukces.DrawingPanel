using System;
using System.Runtime.CompilerServices;
using System.Windows;
using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Paths
{
    public sealed class ArcDefinition : IPathElement, ILineCollider
    {
        public static ArcDefinition FromCenterAndArms(Point center, Point start, Vector startV, Point end)
        {
            var arc = new ArcDefinition
            {
                Center      = center,
                Start       = start,
                StartVector = startV,
                End         = end,
                RadiusStart = start - center,
                RadiusEnd   = end - center
            };
            return arc;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArcDefinition Make(PathRay a, Point b, Vector vb) { return Make(a.Point, a.Vector, b, vb); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArcDefinition Make(PathRay a, PathRay b) { return Make(a.Point, a.Vector, b.Point, b.Vector); }

        public static ArcDefinition Make(Point a, Vector va, Point b, Vector vb)
        {
            var l1     = LineEquationNotNormalized.FromPointAndDeltas(a, va.GetPrependicularVector());
            var l2     = LineEquationNotNormalized.FromPointAndDeltas(b, vb.GetPrependicularVector());
            var center = l1.CrossWith(l2);
            if (center is null)
                return null;
            /*
            var ttt = center.Value - a;
            if (ttt.Length < 1e-10)
                Debug.Write("");
            */
            var c = new ArcDefinition
            {
                Center      = center.Value,
                Start       = a,
                End         = b,
                StartVector = va,
            };
            c.RadiusStart = c.Start - c.Center;
            c.RadiusEnd   = c.End - c.Center;
            return c;
        }

        public string GetDirectionAlternative()
        {
            var isRight = Direction == ArcDirection.Clockwise;
            return isRight ? "Right" : "Left";
        }

        Point IPathElement.GetEndPoint() { return End; }

        Vector IPathElement.GetEndVector() { return EndVector; }

        public double GetLength() { return Radius * Angle * MathEx.DEGTORAD; }

        Point IPathElement.GetStartPoint() { return Start; }

        Vector IPathElement.GetStartVector() { return StartVector; }

        public bool IsLineCollision(Point hitPoint, double toleranceSquared, out double distanceSquared)
        {
          
            var toCenter = hitPoint - Center;

            var x1 = RadiusStart.X;
            var y1 = RadiusStart.Y;
            var x2 = toCenter.X;
            var y2 = toCenter.Y;

            var y1Sq = y1 * y1;
            var x1Sq = x1 * x1;
            var y2Sq = y2 * y2;
            var x2Sq = x2 * x2;

            var tmpSqrt     = (y1Sq + x1Sq) * (y2Sq + x2Sq);
            var distSquared = -2 * Math.Sqrt(tmpSqrt) + y2Sq + y1Sq + x2Sq + x1Sq;

            if (distSquared <= toleranceSquared)
            {
                var angle = Vector.AngleBetween(toCenter, RadiusStart);
                if (Direction==ArcDirection.CounterClockwise)
                    angle = -angle;
                if (angle<0)
                    angle += 360;
                distanceSquared = distSquared;
                return angle >= 0 && angle <= Angle;
            }

            distanceSquared = distSquared;
            return false;
        }

        public override string ToString() { return $"{Angle:N2}°, r={Radius:N2}"; }

        public void UpdateRadiusVectors()
        {
            RadiusStart = Start - Center;
            RadiusEnd   = End - Center;
        }

        public double Radius => RadiusStart.Length;

        public Vector RadiusEnd { get; set; }

        public Vector RadiusStart { get; set; }

        public Vector EndVector
        {
            get
            {
                if (Direction == ArcDirection.CounterClockwise) // MathematicalPlus
                    return new Vector(-RadiusEnd.Y, RadiusEnd.X);
                return new Vector(RadiusEnd.Y, -RadiusEnd.X);
            }
        }

        public Vector StartVector { get; set; }

        public Point End { get; set; }

        public Point Start { get; set; }

        public Point Center { get; set; }

        public double Angle
        {
            get
            {
                var angle = Vector.AngleBetween(RadiusStart, RadiusEnd);
                switch (angle)
                {
                    case 0: return 0;
                    case < 0:
                        angle += 360;
                        break;
                }

                if (Direction == ArcDirection.Clockwise)
                    angle = 360 - angle;

                return angle;
            }
        }

        public ArcDirection Direction
        {
            get
            {
                var dotStart = Vector.CrossProduct(RadiusStart, StartVector);
                if (dotStart < 0)
                    return ArcDirection.Clockwise; // MathematicalMinus
                return ArcDirection.CounterClockwise; // MathematicalPlus;
            }
        }
    }
}
