﻿using System;
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

            var vx  = StartCenterSearch.Vector.X - EndCenterSearch.Vector.X;
            var vy  = StartCenterSearch.Vector.Y - EndCenterSearch.Vector.Y;
            var vy2 = vy * vy;
            var vx2 = vx * vx;

            var a = vy2 + vx2 - 4;
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

            double radius;
            {
                if (a == 0)
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

            var center1 = StartCenterSearch.Get(radius);
            var center2 = EndCenterSearch.Get(radius);
            var cross   = MathUtils.Average(center1, center2);

            arc1 = ArcDefinition.FromCenterAndArms(center1, StartCenterSearch.Point, StartDirection, cross);
            arc2 = ArcDefinition.FromCenterAndArms(center2, cross, arc1.EndVector, EndCenterSearch.Point);
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
