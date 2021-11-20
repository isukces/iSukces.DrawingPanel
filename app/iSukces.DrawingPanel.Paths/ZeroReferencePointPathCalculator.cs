using System.Runtime.CompilerServices;
using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public sealed class ZeroReferencePointPathCalculator : ReferencePointPathCalculator
    {
        ZeroReferencePointPathCalculator() { }

        private static bool Check(ZeroReferencePointPathCalculatorResult result, ArcDefinition one)
        {
            if (result is null)
                return false;
            if (one is null)
                return true;
            var oneAngle = one.Angle;
            if (result.Arc1.Angle > oneAngle) return false;
            if (result.Arc2.Angle > oneAngle) return false;
            return true;
        }

        public static IPathResult Compute(PathRay start, PathRay end, IPathValidator validator)
        {
            var x = new ZeroReferencePointPathCalculator
            {
                Start     = start,
                End       = end,
                Validator = validator
            };
            return x.Compute();
        }

        public static IPathResult ComputeFromPararell(PathRay start, PathRay end, IPathValidator validator)
        {
            var x = new ZeroReferencePointPathCalculator
            {
                Start     = start,
                End       = end,
                Validator = validator
            };
            return x.TryTwo(null, true);
        }


        private IPathResult Compute()
        {
            Start = Start.Normalize();
            End   = End.Normalize();
            var l1    = Start.GetLine();
            var l2    = End.GetLine();
            var cross = MathUtils.CrossNormalized(l1, l2);
            // var cross = Start.Cross(End);
            if (cross is null)
            {
                var a = (End.Point - Start.Point);
                if (a.LengthSquared == 0)
                {
                    var result = new ZeroReferencePointPathCalculatorResult(ResultKind.Point)
                    {
                        Start = Start.Point,
                        End   = End.Point
                    };
                    return result;
                }

                var isSmall =
                    MathUtils.IsAngleBetweenSmallEnoughtBasedOnH(a, Start.Vector, PathCalculationConfig.MaximumSagitta);

                if (isSmall)
                    return new ZeroReferencePointPathCalculatorLineResult(Start.Point, End.Point);
                return TryTwo(null, false);
            }
            else
            {
                var a = (End.Point - Start.Point);
                if (a.LengthSquared == 0)
                {
                    var result = new ZeroReferencePointPathCalculatorResult(ResultKind.Point)
                    {
                        Start = Start.Point,
                        End   = End.Point
                    };
                    return result;
                }

                var startVector = Start.Vector.NormalizeFast();
                var endVector   = End.Vector.NormalizeFast();
                var aa          = a.NormalizeFast();

                var isSmall =
                    MathUtils.IsAngleBetweenSmallEnoughtBasedOnH(a, Start.Vector, PathCalculationConfig.MaximumSagitta);
            }

            var one = TryOne(cross.Value);
            if (IsOk(one))
            {
                if (one.Angle < 180)
                    return new ZeroReferencePointPathCalculatorResult(ResultKind.OneArc)
                    {
                        Arc1  = one,
                        Start = Start.Point,
                        End   = End.Point
                    };
            }

            return TryTwo(null, false);
        }

        public override void InitDemo()
        {
            Start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            End   = new PathRay(new Point(100, 20), new Vector(-100, 100));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsOk(ArcDefinition arc) { return Validator.IsOk(arc); }

        public override void SetReferencePoint(Point p, int nr) { }

        private ArcDefinition TryOne(Point point)
        {
            var v1   = point - Start.Point;
            var dot1 = v1 * Start.Vector;

            var v2   = End.Point - point;
            var dot2 = v2 * End.Vector;

            var l1 = v1.Length;
            var l2 = v2.Length;

            if (dot1 > 0 && dot2 > 0)
            {
                var calc = new OneArcFinder
                {
                    Cross = point,
                };
                calc.SetupReverseEnd(Start, End);
                return calc.CalculateArc();
            }

            if (dot1 < 0 && dot2 < 0)
            {
                if (l1 > l2)
                {
                    var vector = v2 * (l1 / l2);
                    var p      = point + vector;

                    var calc = new OneArcFinder
                    {
                        Cross       = point,
                        StartPoint  = Start.Point,
                        StartVector = Start.Vector,
                        EndPoint    = p,
                        EndVector   = End.Vector
                    };
                    return calc.CalculateArc();
                }
                else
                {
                    var vector = v1 * (l2 / l1);
                    var p      = point - vector;

                    var calc = new OneArcFinder
                    {
                        Cross       = point,
                        StartPoint  = p,
                        StartVector = Start.Vector,
                        EndPoint    = End.Point,
                        EndVector   = End.Vector
                    };
                    return calc.CalculateArc();
                }

                /*
                {
                    var calc = new ClassX
                    {
                        Cross = cross.Value,
                    };
                    calc.SetupReverseEnd(Start, End);
                    return calc.CalculateArc();
                }*/
            }

            if (dot1 < 0 && dot2 >= 0)
            {
                var vector = v2 * (l1 / l2);
                var p      = point - vector;

                var calc = new OneArcFinder
                {
                    Cross       = point,
                    StartPoint  = Start.Point,
                    StartVector = Start.Vector,
                    EndPoint    = p,
                    EndVector   = End.Vector
                };
                return calc.CalculateArc();
            }

            return null;
        }

        private ZeroReferencePointPathCalculatorResult TryTwo(ArcDefinition one, bool normalize)
        {
            var toCompare = one;
            if (toCompare != null)
                if (toCompare.RadiusStart.Length <= 3) // todo: why 3?
                    toCompare = null;

            if (normalize)
            {
                Start = Start.Normalize();
                End   = End.Normalize();
            }

            var                                    minLength  = double.MaxValue;
            ZeroReferencePointPathCalculatorResult bestResult = null;

            void CheckAndAdd(ZeroReferencePointPathCalculatorResult r)
            {
                if (r is null)
                    return;
                if (!Check(r, toCompare))
                    return;

                var l = r.GetLength(Start.Point, End.Point);
                if (l < minLength)
                {
                    minLength  = l;
                    bestResult = r;
                }
            }
            
            var finder = new TwoArcsFinder();
            finder.UpdateFromPoints(Start, End, false);
            var prec = finder.Compute();

            var sol1 = TryTwoArcsSolution(prec, false);
            CheckAndAdd(sol1);
            var sol2 = TryTwoArcsSolution(prec, true);
            CheckAndAdd(sol2);
            
            
            //finder = new TwoArcsFinder();
            finder.UpdateFromPoints(Start, End, true);
            prec = finder.Compute();

            var sol3 = TryTwoArcsSolution(prec, false);
            CheckAndAdd(sol3);
            var sol4 = TryTwoArcsSolution(prec, true);
            CheckAndAdd(sol4);

            if (bestResult != null)
                return bestResult;
            if (one != null)
                return new ZeroReferencePointPathCalculatorResult(ResultKind.OneArc)
                {
                    Arc1  = one,
                    Start = Start.Point,
                    End   = End.Point
                };

            return null;
        }

        private ZeroReferencePointPathCalculatorResult TryTwoArcsSolution(
            TwoArcsFinderPrecompute precompute,
            bool useSmallerRadius)
        {
            var isOk =precompute.UpdateCompute(useSmallerRadius, Validator as IMinRadiusPathValidator);
            if (!isOk) return null;
            /*var finder = new TwoArcsFinder();
            finder.UpdateFromPoints(Start, End, reverseEnd);
            // finder.Normalize();
            var isOk = finder.Compute(out var arc1, out var arc2, useSmallerRadius);
            if (!isOk) return null;*/

            var arc1 = precompute.arc1;
            var dot = Start.Vector * arc1.StartVector;
            if (dot <= 0) return null;
            
            var arc2 = precompute.arc2;
            dot = End.Vector * arc2.EndVector;
            if (dot <= 0) return null;
            
            dot = arc1.EndVector * arc2.StartVector;
            if (dot <= 0) return null;
            
            return new ZeroReferencePointPathCalculatorResult(ResultKind.TwoArcs)
            {
                Arc1  = arc1,
                Arc2  = arc2,
                Start = Start.Point,
                End   = End.Point
            };
        }

        public IPathValidator Validator { get; set; }

        
#if DEBUG && USE_TINYEXPR        
        public string DebugCreate
        {
            get
            {
                var a = PathsExtensions.DebugCreate(Start, "start") + "\r\n";
                var b = PathsExtensions.DebugCreate(End, "end") + "\r\n";
                var c = $"var g = {nameof(ZeroReferencePointPathCalculator)}.Compute(start, end, null)";
                return a + b + c;
            }
        }
#endif


        public enum ResultKind
        {
            /// <summary>
            ///     Początek i koniec jest jednym punktem
            /// </summary>
            Point,


            OneArc,
            TwoArcs
        }
    }
}
