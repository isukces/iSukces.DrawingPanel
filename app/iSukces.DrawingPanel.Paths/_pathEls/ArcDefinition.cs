using System.Runtime.CompilerServices;
using System.Windows;
using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Paths
{
    public sealed class ArcDefinition : IPathElement
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


        public override string ToString() { return $"{Angle:N2}°, r={Radius:N2}"; }

        public void UpdateRadiusVectors()
        {
            RadiusStart = Start - Center;
            RadiusEnd   = End - Center;
        }

        private double Radius => RadiusStart.Length;

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
