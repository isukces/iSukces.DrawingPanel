#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System;
using System.Runtime.CompilerServices;
using iSukces.Mathematics;


namespace iSukces.DrawingPanel.Paths
{
    public sealed class TwoArcsFinder
    {
        public bool Compute(out ArcDefinition arc1, out ArcDefinition arc2, bool useSmallerRadius)
        {
            var prec = Compute();
            if (prec.UpdateCompute(useSmallerRadius, null))
            {
                arc1 = prec.Arc1;
                arc2 = prec.Arc2;
                return true;
            }

            arc1 = arc2 = null;
            return false;
        }

        internal TwoArcsFinderPrecompute Compute()
        {
            var dx = EndCenterSearch.Point.X - StartCenterSearch.Point.X;
            var dy = EndCenterSearch.Point.Y - StartCenterSearch.Point.Y;

            // when v1 = -v2
            // then (v1-v2).LengthSquared = 4 so "a" ==0
            // due to calculations it sometimes is small but not zero, so calculated radius is not correct
            var sV = StartCenterSearch.Vector;
            var eV = EndCenterSearch.Vector;

            var sumX = sV.X + eV.X;
            var sumY = sV.Y + eV.Y;

            var vx = sV.X - eV.X;
            var vy = sV.Y - eV.Y;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static bool GetAIsZero(in double sumX, in double sumY, in Vector eV, in Vector sV)
            {
                if (sumX == 0)
                    return eV.Y * sV.Y < 0;
                if (sumY == 0)
                    return eV.X * sV.X < 0;
                return false;
            }

            var aIsZero = GetAIsZero(sumX, sumY, eV, sV);

            var a = (vy * vy) + (vx * vx) - 4; // a<=0
            var b = -2 * (dy * vy + dx * vx); //   b any
            var c = dy * dy + dx * dx; // c >= 0

            var delta = b * b - 4 * a * c; // delta alvays >= 0

            var re = new TwoArcsFinderPrecompute(this)
            {
                AIsZero   = aIsZero,
                MaxRadius = double.MaxValue
            };

            if (aIsZero || a == 0)
            {
                re.Radius1 = re.Radius2 = -c / b;
            }
            else
            {
                var m = -2 * a;
                var d = Math.Sqrt(delta);
                re.Radius1 = (b - d) / m;
                re.Radius2 = (b + d) / m;
                re.SumX    = sumX;
                re.SumY    = sumY;
            }

            return re;
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

        #region properties

        public PathRay StartCenterSearch { get; set; }
        public PathRay EndCenterSearch   { get; set; }
        public Vector  EndDirection      { get; set; }
        public Vector  StartDirection    { get; set; }

        #endregion
    }
}
