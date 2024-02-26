using System;
using System.Diagnostics;
#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths;

[DebuggerDisplay("{GetCreationCode()}")]
public sealed class ZeroReferencePointPathCalculator : ReferencePointPathCalculator
{
    ZeroReferencePointPathCalculator()
    {
    }

    private static bool Check(ZeroReferencePointPathCalculatorResult result, ArcDefinition one,
        IPathValidator pathValidator)
    {
        if (result is null)
            return false;
        {
            if (pathValidator is not null)
            {
                if (result.Arc1 is not null)
                {
                    var ok = pathValidator.IsOk(result.Arc1, ArcDestination.ZeroReferenceTwoArcs);
                    if (!ok)
                        return false;
                }

                if (result.Arc2 is not null)
                {
                    var ok = pathValidator.IsOk(result.Arc2, ArcDestination.ZeroReferenceTwoArcs);
                    if (!ok)
                        return false;
                }
            }
        }
        if (one is null)
        {
            return true;
        }

        var oneAngle = one.Angle;
        if (result.Arc1.Angle > oneAngle) return false;
        if (result.Arc2.Angle > oneAngle) return false;
        return true;
    }

    public static IPathResult Compute(PathRay start, PathRay end, IPathValidator validator,
        ZeroReferencePointPathCalculatorFlags flags = ZeroReferencePointPathCalculatorFlags.None)
    {
        var x = new ZeroReferencePointPathCalculator
        {
            Start     = start,
            End       = end,
            Validator = validator,
            Flags     = flags
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
        return x.TryTwo(null, true, double.MaxValue);
    }


    private IPathResult Compute()
    {
        Start = Start.Normalize();
        End   = End.Normalize();
        var lStart   = Start.GetLine();
        var lEnd     = End.GetLine();
        var endBegin = (End.Point - Start.Point);

        var endBeginLengthSquared = endBegin.LengthSquared;
        if (endBeginLengthSquared == 0)
        {
            var result = new ZeroReferencePointPathCalculatorResult(ResultKind.Point)
            {
                Start = Start.Point,
                End   = End.Point
            };
            return result;
        }

        var maxRadius = endBeginLengthSquared / (8 * PathCalculationConfig.MaximumSagitta);

        var cross = PathsMathUtils.CrossNormalized(lStart, lEnd, out var determinant);
        {
            var dist = PathCalculationConfig.UseLineWhenDistanceLowerThan;
            {
                var d1 = Math.Abs(lStart.DistanceNotNormalized(End.Point));
                var d2 = Math.Abs(lEnd.DistanceNotNormalized(Start.Point));
                if (d1 < dist && d2 < dist)
                {
                    return new ZeroReferencePointPathCalculatorLineResult(Start.Point, End.Point);
                }

                if (d1 == 0)
                {
                    cross = End.Point;
                }
                else if (d2 == 0)
                {
                    cross = Start.Point;
                }
            }
        }

        // var cross = Start.Cross(End);
        if (cross is null)
        {
            var isSmall =
                PathsMathUtils.IsAngleBetweenSmallEnoughtBasedOnH(endBegin, Start.Vector,
                    PathCalculationConfig.MaximumSagitta);

            if (isSmall)
                return new ZeroReferencePointPathCalculatorLineResult(Start.Point, End.Point);
            return TryTwo(null, false, double.MaxValue);
        }

        if ((Flags & ZeroReferencePointPathCalculatorFlags.DontUseOneArcSolution) == 0)
        {
            var one = TryOne(cross.Value);
            if (Validator.IsOk(one, ArcDestination.ZeroReferenceOneArc))
            {
                if (one.Angle < 180)
                    return new ZeroReferencePointPathCalculatorResult(ResultKind.OneArc)
                    {
                        Arc1  = one,
                        Start = Start.Point,
                        End   = End.Point
                    };
            }
        }

        var a = TryTwo(null, false, maxRadius);
        if (a is null)
        {
            if (determinant.AbsLessThan(PathCalculationConfig.MinimumLinearEquationSystemDeterminantToUseLine))
            {
                return new ZeroReferencePointPathCalculatorLineResult(Start.Point, End.Point);
            }
        }

        return a;
    }

    string GetCreationCode()
    {
        var    a = Start.GetCreationCode();
        var    b = End.GetCreationCode();
        string c;
        if (Validator is null)
            c = "null";
        else if (Validator is MinimumValuesPathValidator minimumValuesPathValidator)
            c = minimumValuesPathValidator.GetCreationCode();
        else
            c = "???";

        return $"new ZeroReferencePointPathCalculator{{ Start = {a}, End = {b}, Validator = {c} }}";
    }

    public override void InitDemo()
    {
        Start = new PathRay(new Point(-20, 0), new Vector(200, 100));
        End   = new PathRay(new Point(100, 20), new Vector(-100, 100));
    }


    public override void SetReferencePoint(Point p, int nr)
    {
    }

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
            var calc  = new OneArcFinder(point); // VALIDATED
            var start = Start.GetMovedRayOutput();
            var end   = End.GetMovedRayInput();
            calc.SetupReverseEnd(start, end);
            return ArcValidationHelper.Validate(Validator, calc) ? calc.CalculateArc() : null;
        }

        if (dot1 < 0 && dot2 < 0)
        {
            if (l1 > l2)
            {
                var vector = v2 * (l1 / l2);
                var p      = point + vector;

                {
                    var calc = new OneArcFinder(point) // VALIDATED
                    {
                        StartPoint  = Start.Point,
                        StartVector = Start.Vector,
                        EndPoint    = p,
                        EndVector   = End.Vector
                    };
                    return ArcValidationHelper.Validate(Validator, calc) ? calc.CalculateArc() : null;
                }
            }
            else
            {
                var vector = v1 * (l2 / l1);
                var p      = point - vector;

                var calc = new OneArcFinder(point) // VALIDATED
                {
                    StartPoint  = p,
                    StartVector = Start.Vector,
                    EndPoint    = End.Point,
                    EndVector   = End.Vector
                };
                return ArcValidationHelper.Validate(Validator, calc) ? calc.CalculateArc() : null;
            }
        }

        if (dot1 < 0 && dot2 >= 0)
        {
            var vector = v2 * (l1 / l2);
            var p      = point - vector;

            var calc = new OneArcFinder(point) // VALIDATED
            {
                StartPoint  = Start.Point,
                StartVector = Start.Vector,
                EndPoint    = p,
                EndVector   = End.Vector
            };
            return ArcValidationHelper.Validate(Validator, calc) ? calc.CalculateArc() : null;
        }

        return null;
    }

    private ZeroReferencePointPathCalculatorResult TryTwo(ArcDefinition one, bool normalize, double maxRadius)
    {
        if (normalize)
        {
            Start = Start.Normalize();
            End   = End.Normalize();
        }

        var minLength  = double.MaxValue;
        var bestResult = (ZeroReferencePointPathCalculatorResult)null;

        void CheckAndAdd(ZeroReferencePointPathCalculatorResult r)
        {
            if (r is null)
                return;
            if (!Check(r, one, Validator))
                return;

            var l = r.GetLength(Start.Point, End.Point);
            if (l < minLength)
            {
                minLength  = l;
                bestResult = r;
            }
        }

        var s = Start.GetMovedRayOutput();
        var e = End.GetMovedRayInput();

        var finder = new TwoArcsFinder();
        finder.UpdateFromPoints(s, e, false);
        var prec = finder.Compute();
        prec.MaxRadius = maxRadius;

        var sol1 = TryTwoArcsSolution(prec, false);
        CheckAndAdd(sol1);
        if (!prec.AIsZero)
        {
            var sol2 = TryTwoArcsSolution(prec, true);
            CheckAndAdd(sol2);
        }

        //finder = new TwoArcsFinder();
        finder.UpdateFromPoints(s, e, true);
        prec           = finder.Compute();
        prec.MaxRadius = maxRadius;

        var sol3 = TryTwoArcsSolution(prec, false);
        CheckAndAdd(sol3);
        if (!prec.AIsZero)
        {
            var sol4 = TryTwoArcsSolution(prec, true);
            CheckAndAdd(sol4);
        }

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
        TwoArcsFinderPrecompute precompute, bool useSmallerRadius)
    {
        var isOk = precompute.UpdateCompute(useSmallerRadius, Validator as IMinRadiusPathValidator);
        if (!isOk) return null;

        var arc1 = precompute.Arc1;
        var dot  = Start.Vector * arc1.DirectionStart;
        if (dot <= 0) return null;

        var arc2 = precompute.Arc2;
        dot = End.Vector * arc2.DirectionEnd;
        if (dot <= 0) return null;

        dot = arc1.DirectionEnd * arc2.DirectionStart;
        if (dot <= 0) return null;

        return new ZeroReferencePointPathCalculatorResult(ResultKind.TwoArcs)
        {
            Arc1  = arc1,
            Arc2  = arc2,
            Start = Start.Point,
            End   = End.Point
        };
    }

    #region properties

    public IPathValidator                        Validator { get; set; }
    public ZeroReferencePointPathCalculatorFlags Flags     { get; set; }


#if DEBUG && USE_TINYEXPR
    public string DebugCreate
    {
        get
        {
            var a = PathsExtensions.DebugCreate(Start.GetRay(), "start") + "\r\n";
            var b = PathsExtensions.DebugCreate(End.GetRay(), "end") + "\r\n";
            var c = $"var g = {nameof(ZeroReferencePointPathCalculator)}.Compute(start, end, null)";
            return a + b + c;
        }
    }
#endif

    #endregion


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

[Flags]
public enum ZeroReferencePointPathCalculatorFlags
{
    None = 0,

    /// <summary>
    ///     Don't try to find points connection with one arc
    /// </summary>
    DontUseOneArcSolution = 1
}