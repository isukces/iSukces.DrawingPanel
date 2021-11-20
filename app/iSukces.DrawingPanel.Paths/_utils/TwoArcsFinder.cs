using System;
using System.Diagnostics;
using System.Windows;
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
                arc1 = prec.arc1;
                arc2 = prec.arc2;
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

            bool aIsZero;
            {
                // due to length ov sV=1 and eV=1 if sumX is zero then sumY is also zero and vice versa

                var isSumXZero = sumX == 0;
                var isSumYZero = sumY == 0;
                aIsZero = isSumXZero && isSumYZero;
                if (!aIsZero)
                {
                    if (isSumXZero || isSumYZero)
                    {
                        // exctly one is true and one is false;
                        if (isSumYZero)
                        {
                            var x1 = eV.X;
                            var x2 = sV.X;
                            if (x1 * x2 < 0)
                            {
                                var x = (x1 - x2) * 0.5;
                                sV      = new Vector(-x, sV.Y);
                                eV      = new Vector(x, eV.Y);
                                aIsZero = true;
                                sumX    = 0;
                            }
                        }
                        else
                        {
                            // !isSumYZero == true here
                            if (eV.Y * sV.Y < 0)
                            {
                                var y = (eV.Y - sV.Y) * 0.5;
                                sV      = new Vector(-sV.X, y);
                                eV      = new Vector(eV.X, y);
                                aIsZero = true;
                                sumY    = 0;
                            }
                        }
                    }
                }
            }

            var vx  = sV.X - eV.X;
            var vy  = sV.Y - eV.Y;
            var vy2 = vy * vy;
            var vx2 = vx * vx;

            var a = vy2 + vx2 - 4;
            var b = -2 * (dy * vy + dx * vx);
            var c = dy * dy + dx * dx;

            // a<=0
            //  b any
            // c >0
            // delta alvays >0

            var delta = b * b - 4 * a * c;

#if DEBUG && USE_TINYEXPR
            if (a > -4E-9)
            {
                Debug.Write("");
            }

            var aa = vy2.ToExpr() + vx2 - 4d;
            var bb = -2 * (dy.ToExpr() * vy + dx.ToExpr() * vx).Brackets();
            var cc = dy.ToExpr() * dy + dx.ToExpr() * dx;
            aa = aa.Brackets();
            bb = bb.Brackets();
            var expr      = TinyExpr.Square(aa, bb, cc, new TinyExpr("r"));
            var deltaExpr = TinyExpr.SquareDelta(aa, bb, cc);
            {
                var _sV_X = sV.X.ToExpr();
                var _sV_Y = sV.Y.ToExpr();
                var _dx   = dx.ToExpr();
                var _dy   = dy.ToExpr();

                /*_dx   = new TinyExpr("dx");
                _dy   = new TinyExpr("dx");
                _sV_X = new TinyExpr("sVX");
                _sV_Y = new TinyExpr("sVX");*/

                var sin = new TinyExpr("a");
                var cos = (1d.ToExpr() - sin * sin).Sqrt();
                // sin = 0d.ToExpr();
                // cos = 1d.ToExpr();

                var _eV_X = _sV_X * cos + _sV_Y * sin;
                var _eV_Y = _sV_Y * cos - _sV_X * sin;

                _eV_X = eV.X.ToExpr();
                _eV_Y = eV.Y.ToExpr();
                //_eV_X = _sV_X;
                //_eV_Y = _sV_Y;

                //_eV_X = _eV_X.Brackets().Negate();
                // _eV_Y = _eV_Y.Brackets().Negate();

                _sV_X = _sV_X.BFloat();
                _sV_Y = _sV_Y.BFloat();
                _eV_X = _eV_X.BFloat();
                _eV_Y = _eV_Y.BFloat();

                TinyExpr _vx = (_sV_X.Brackets() - _eV_X.Brackets()).Brackets();
                var      _vy = (_sV_Y.Brackets() - _eV_Y.Brackets()).Brackets();

                var _vxPlot = (_vx * _vx + _vy * vy).Plot("a", -1, 1);

                var _aa = _vx * _vx + _vy * _vy - 4;
                var _bb = -2 * (_dy * _vy + _dx * _vx).Brackets();
                var _cc = _dy * _dy + _dx * _dx;
                _aa = _aa.Brackets();
                _bb = _bb.Brackets();
                _cc = _cc.Brackets();
                // var _expr = TinyExpr.Square(_aa, _bb, _cc, new TinyExpr("r"));

                deltaExpr = TinyExpr.SquareDelta(_aa, _bb, _cc);
                var solve1 = TinyExpr.SquareSolve1(_aa, _bb, _cc);
                var solve2 = TinyExpr.SquareSolve2(_aa, _bb, _cc);
            }
#endif

            double r1, r2;
            {
                if (aIsZero || a == 0)
                    r1 = r2 = -c / b;
                else
                {
                    var m = -2 * a;
                    var d = Math.Sqrt(delta);
                    r1 = (b - d) / m;
                    r2 = (b + d) / m;
                }
            }
            return new TwoArcsFinderPrecompute(this)
            {
                radius1 = r1,
                radius2 = r2,
                sumX    = sumX,
                sumY    = sumY,
                aIsZero = aIsZero
            };
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
