using System;
using System.Windows;
using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Paths
{
    internal sealed class TwoArcsFinder
    {
        public bool Compute(out ArcDefinition arc1, out ArcDefinition arc2, bool useSmaller)
        {
            var dx = EndCenterSearch.Point.X - StartCenterSearch.Point.X;
            var dy = EndCenterSearch.Point.Y - StartCenterSearch.Point.Y;

            
            // when v1 = -v2
            // then (v1-v2).LengthSquared = 4 so "a" ==0
            // due to calculations it sometimes is small but not zero, so calculated radius is not correct
            var sumX     = StartCenterSearch.Vector.X + EndCenterSearch.Vector.X;
            var sumY     = StartCenterSearch.Vector.Y + EndCenterSearch.Vector.Y;
            var aIsZero = sumX == 0 && sumY == 0;
            
            var vx  = StartCenterSearch.Vector.X - EndCenterSearch.Vector.X;
            var vy  = StartCenterSearch.Vector.Y - EndCenterSearch.Vector.Y;
            var vy2 = vy * vy;
            var vx2 = vx * vx;

            var a =  vy2 + vx2 - 4;
            var b = -2 * (dy * vy + dx * vx);
            var c = dy * dy + dx * dx;

            var delta = b * b - 4 * a * c;
            if (delta < 0)
            {
                // może jakieś inne rozwiązanie ?????
                arc1 = null;
                arc2 = null;
                return false;
            }
            #if DEBUG && useexpr
            var aa   = vy2.X() + vx2 - 4;
            var bb   = -2 * (dy.X() * vy + dx.X() * vx).Brackets();
            var cc   = dy.X() * dy + dx.X() * dx;
            aa = aa.Brackets();
            bb = bb.Brackets();
            var expr = aa * aa * new TinyExpr("r") + bb * new TinyExpr("r") + cc; 
            #endif

            double radius;
            {
                if (aIsZero || a == 0)
                    radius = -c / b;
                else
                {
                    var m  = -2 * a;
                    var d  = Math.Sqrt(delta);
                    var r1 = (b - d) / m;
                    var r2 = (b + d) / m;
                    if (Math.Abs(r1) > Math.Abs(r2))
                        radius = useSmaller ? r2 : r1;
                    else
                        radius = useSmaller ? r1 : r2;
                }
            }

            var   center1 = StartCenterSearch.Get(radius);
            var   center2 = EndCenterSearch.Get(radius);
            Point cross;
            {
                // c1 = p1 + v1 * radius
                // c2 = p2 + v2 * radius
                // (c1 + c2) / 2 = (p1 + p2) / 2 + (v1 + v2) / 2 * radius
                // sometimes radius is very big and (v1 + v2) is very small or even zero 
                // so { (c1 + c2) / 2 } can have errors 
                cross = MathUtils.Average(StartCenterSearch.Point, EndCenterSearch.Point);
                var ve   = StartCenterSearch.Vector + EndCenterSearch.Vector;
                var move = ve * (radius * 0.5);
                cross += move;
            }

            arc1 = ArcDefinition.FromCenterAndArms(center1, StartCenterSearch.Point, StartDirection, cross);
            arc2 = ArcDefinition.FromCenterAndArms(center2, cross, arc1.EndVector, EndCenterSearch.Point);
            
            var arc3 = ArcDefinition.FromCenterAndArms(center2, EndCenterSearch.Point, EndDirection, cross);

            arc2.RadiusStart = arc3.RadiusEnd;
            arc2.RadiusEnd   = arc3.RadiusStart;

            arc2.StartVector = arc3.EndVector;
            // arc2.EndVector   = arc3.StartVector;

            arc2.Start = arc3.End;
            arc2.End = arc3.Start;
            
            
            if (radius < 0)
                radius = -radius;
            {
                var arc1Radius = radius - arc1.Radius;
                if (Math.Abs(arc1Radius) > 0.000001)
                    throw new Exception("Radius is not equal");
                var arc2Radius = radius - arc2.Radius;
                if (Math.Abs(arc2Radius) > 0.000001)
                    throw new Exception("Radius is not equal");
            }
            arc1.UseRadius(radius);
            arc2.UseRadius(radius);
            return true;
        }

        public void Normalize()
        {
            StartCenterSearch = StartCenterSearch.Normalize();
            EndCenterSearch   = EndCenterSearch.Normalize();
        }

        public void UpdateFromPoints(PathRay startRay, PathRay endRay, bool reverseEnd)
        {
            StartDirection = startRay.Vector;
            EndDirection   = endRay.Vector;

            var startSearcVector = StartDirection.GetPrependicular(false);
            var endSearchVector  = EndDirection.GetPrependicular(reverseEnd);

            StartCenterSearch = startRay.With(startSearcVector);
            EndCenterSearch   = endRay.With(endSearchVector);
        }

        public PathRay StartCenterSearch { get; set; }
        public PathRay EndCenterSearch   { get; set; }
        public Vector  EndDirection      { get; set; }
        public Vector  StartDirection    { get; set; }
    }
}
