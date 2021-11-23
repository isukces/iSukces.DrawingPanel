#define _OLD_RESULT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using iSukces.Mathematics;
#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    public sealed class OneReferencePointPathCalculator : ReferencePointPathCalculator
    {
        public IPathResult Compute(IPathValidator validator)
        {
            if (!Reference.HasValidVector())
            {
                var vector = End.Point - Start.Point;
                if (vector.LengthSquared < LengthEpsilonSquare)
                    return CreateResultLine();

                Reference = Reference.With(vector);
            }

            Reference = Reference.Normalize();

            Start = Start.Normalize();
            End   = End.Normalize();


            var list = Find();
            if (list.Count == 0)
                throw new NotImplementedException();
            return new PathResult(list);

            IReadOnlyList<IPathElement> Find()
            {
                var builder = new PathBuilder(Start.Point, validator);

                IReadOnlyList<IPathElement> FlexiS()
                {
                    builder.Clear();
                    builder.AddFlexi(Start, Reference);
                    var endInverted = End.WithInvertedVector();
                    builder.AddFlexi(Reference, endInverted);
                    return builder.List;
                }

                var l1 = Start.GetLine();
                var l2 = End.GetLine();
                var l  = Reference.GetLine();


                Point? startCrossNullable, endCrossNullable;
                {
                    var dist = PathCalculationConfig.UseLineWhenDistanceLowerThan;
                    {
                        var d1 = Math.Abs(l1.DistanceNotNormalized(Reference.Point));
                        var d2 = Math.Abs(l.DistanceNotNormalized(Start.Point));
                        if (d1 < dist && d2 < dist)
                            startCrossNullable = null;
                        else
                            startCrossNullable = PathsMathUtils.CrossNormalized(l, l1);
                    }
                    {
                        var d1 = Math.Abs(l2.DistanceNotNormalized(Reference.Point));
                        var d2 = Math.Abs(l.DistanceNotNormalized(End.Point));
                        if (d1 < dist && d2 < dist)
                            endCrossNullable = null;
                        else
                            endCrossNullable = PathsMathUtils.CrossNormalized(l, l1);
                    }
                }


                if (startCrossNullable is null)
                {
                    if (endCrossNullable is null)
                    {
                        var line = LineEquation.Make(Start.Point, Reference.Vector);
                        var dist = line.DistanceNotNormalized(Reference.Point);

                        if (dist == 0)
                        {
                            builder.AddLine(Start.Point, End.Point);
                            return builder.List;
                        }

                        {
                            builder.AddFlexiFromPararell(Start, Reference, validator);
                            var endInverted = End.WithInvertedVector();
                            builder.AddFlexiFromPararell(Reference, endInverted, validator);
                            return builder.List;
                        }
                    }

                    builder.AddFlexiFromPararell(Start, Reference, validator);
                }
                else
                {
                    if (endCrossNullable is null)
                    {
                        var endInverted = End.WithInvertedVector();
                        builder.AddFlexiFromPararell(Reference, endInverted, validator);
                    }
                    else
                    {
                        var cross1 = startCrossNullable.Value;
                        var cross2 = endCrossNullable.Value;
                        var q      = FromTwoCrosses(cross1, cross2, builder, validator);
                        if (q == FromTwoCrossesResult.TwoS)
                            return FlexiS();
                    }
                }

                if (builder.List.Count == 0)
                    builder.AddLine(Start.Point, End.Point);
                // return FlexiS();

                return builder.List;
            }
        }

        private FromTwoCrossesResult FromTwoCrosses(Point startCross, Point endCross,
            PathBuilder aggregator,
            IPathValidator validator)
        {
            var startIsInvalid = DotNotPositive(startCross, Start);
            if (startIsInvalid)
                return FromTwoCrossesResult.TwoS;
            var endIsInvalid = DotNotPositive(endCross, End);
            if (endIsInvalid)
                return FromTwoCrossesResult.TwoS;

            var vMiddle = endCross - startCross;
            {
                var dot = vMiddle * (End.Point - Start.Point);
                if (dot <= 0)
                    return FromTwoCrossesResult.TwoS;
            }

            var vStart = startCross - Start.Point;
            var vEnd   = End.Point - endCross;

            double startLength;
            {
                var tmp = startCross - Reference.Point;
                var dot = tmp * vMiddle; // negative is OK

                startLength = vStart.Length;
                if (dot < 0)
                    startLength = Math.Min(startLength, tmp.Length);
                else
                    return FromTwoCrossesResult.TwoS;
            }
            var    middleLength = vMiddle.Length;
            double endLength;
            {
                var tmp = endCross - Reference.Point;
                var dot = tmp * vMiddle; // positive is Ok
                endLength = vEnd.Length;
                if (dot > 0)
                    endLength = Math.Min(endLength, tmp.Length);
                else
                    return FromTwoCrossesResult.TwoS;
            }

            var xReference = Reference.GetNormalizedVector();
            var xStart     = Start.Vector.GetNormalizedVector();
            var xEnd       = End.Vector.GetNormalizedVector();

            var brakuje = startLength + endLength - middleLength;
            if (brakuje > 0)
            {
                brakuje     *= 0.5;
                startLength -= brakuje;
                endLength   -= brakuje;
            }

            if (Math.Abs(endLength) < Epsilon || Math.Abs(startLength) < Epsilon)
                Debug.Write("");

            var arc1 = ArcDefinition.Make(
                startCross - xStart * startLength, Start.Vector,
                startCross + xReference * startLength, -Reference.Vector
            );
            if (arc1 is null)
                return FromTwoCrossesResult.None;

            var arc2 = ArcDefinition.Make(
                endCross - xReference * endLength, Reference.Vector,
                endCross - xEnd * endLength, End.Vector
            );
            if (arc2 is null)
                return FromTwoCrossesResult.None;

            if (validator is null)
                validator = new MinimumValuesPathValidator(Epsilon, 0);

            var result1 = validator.ValidateArc(arc1);
            var result2 = validator.ValidateArc(arc2);
            var ok1     = result1 == ArcValidationResult.Ok;
            var ok2     = result2 == ArcValidationResult.Ok;
            {
                var bothOk = ok1 && ok2;
                if (bothOk)
                {
                    aggregator.ArcTo(arc1);
                    aggregator.ArcTo(arc2);
                    aggregator.LineTo(End.Point);
                    return FromTwoCrossesResult.None;
                }
            }
            {
                var bothInvalid = !ok1 && !ok2;
                if (bothInvalid)
                    return FromTwoCrossesResult.TwoS;
            }
            if (ok1)
            {
                aggregator.ArcTo(arc1);
                aggregator.AddFlexi(Reference, End.WithInvertedVector());
                aggregator.LineTo(End.Point);
                return FromTwoCrossesResult.None;
            }

            aggregator.AddFlexi(Start, Reference);
            aggregator.ArcTo(arc2);
            aggregator.LineTo(End.Point);
            return FromTwoCrossesResult.None;
        }

        public Point? Get()
        {
            var l1 = LineEquationNotNormalized.FromPointAndDeltas(Start.Point, Start.Vector);
            var l2 = LineEquationNotNormalized.FromPointAndDeltas(End.Point, End.Vector);
            return l1.CrossWith(l2);
        }

        public override void InitDemo()
        {
            Start     = new PathRay(new Point(-20, 0), new Vector(200, 100));
            End       = new PathRay(new Point(100, 0), new Vector(-100, 100));
            Reference = new PathRay(new Point(50, 20), new Vector());
        }

        public void MaximumArc()
        {
            var a = Get();
            if (a is null)
                return;
            var extraPoint = a.Value;

            var v1 = Start.Point - extraPoint;
            var v2 = End.Point - extraPoint;
            var l1 = v1.Length;
            var l2 = v2.Length;
            var l  = Math.Min(l1, l2);

            v1 *= l / l1;
            v2 *= l / l2;

            var ps = extraPoint + v1;
            var pe = extraPoint + v2;

            var c = ArcDefinition.Make(
                ps, Start.Vector,
                pe, End.Vector
            );
            if (c is null)
                return;
            var radius = (c.Center - ps).Length;
            var vDir   = extraPoint - c.Center;
            var v      = Reference.GetPrependicularVector();
            var dot    = vDir * v;
            if (dot < 0)
                v = -v;
            v         *= radius / v.Length;
            Reference =  Reference.With(c.Center + v);
        }

        public override void SetReferencePoint(Point p, int nr)
        {
            if (nr == 0)
                Reference = Reference.With(p);
        }

        public PathRay Reference { get; set; }

        public enum FromTwoCrossesResult
        {
            None,
            TwoS
        }
    }
}
