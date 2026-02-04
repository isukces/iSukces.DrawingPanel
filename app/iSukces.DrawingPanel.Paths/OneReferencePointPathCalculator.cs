#define _OLD_RESULT

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace iSukces.DrawingPanel.Paths;

public sealed class OneReferencePointPathCalculator : ReferencePointPathCalculator
{
    internal void AppendData(IDictionary dictionary)
    {
        dictionary.Set(nameof(Start), Start);
        dictionary.Set(nameof(End), End);
        dictionary.Set(nameof(Reference), Reference);
    }

    private void CalculateCrosses(out Point? startCross, out Point? endCross)
    {
        var dist      = PathCalculationConfig.UseLineWhenDistanceLowerThan;
        var reference = Reference;

        var l = reference.GetLine();
        {
            var sMoved = Start.GetMovedRayOutput();
            var l1     = sMoved.GetLine();

            var d1 = Math.Abs(l1.DistanceNotNormalized(reference.Point));
            var d2 = Math.Abs(l.DistanceNotNormalized(sMoved.Point));
            if (d1 < dist && d2 < dist)
                startCross = null;
            else
                startCross = PathsMathUtils.CrossNormalized(l, l1);
        }
        {
            var eMoved = End.GetMovedRayInput();
            var l2     = eMoved.GetLine();

            var d1 = Math.Abs(l2.DistanceNotNormalized(reference.Point));
            var d2 = Math.Abs(l.DistanceNotNormalized(eMoved.Point));
            if (d1 < dist && d2 < dist)
                endCross = null;
            else
                endCross = PathsMathUtils.CrossNormalized(l, l2);
        }
    }

    public IPathResult Compute(IPathValidator? validator)
    {
        if (!Reference.HasValidVector())
        {
            var vector = End.Point - Start.Point;
            if (vector.LengthSquared < LengthEpsilonSquare)
                return CreateResultLine();

            Reference = Reference.With(vector);
        }

        Reference = Reference.Normalize();
        Start     = Start.Normalize();
        End       = End.Normalize();

        var list = FindPathElements();
        if (list.Count == 0)
        {
            var exception = new NotImplementedException("No path elements found");
            AppendData(exception.Data);
            throw exception;
        }

        return new PathResult(list);

        IReadOnlyList<IPathElement> FindPathElements()
        {
            var builder = new PathBuilder(Start.Point, validator);
            var idx     = 0;

            CalculateCrosses(out var startCrossNullable, out var endCrossNullable);

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
                        var start  = Start.GetMovedRayOutput();
                        var middle = Reference;
                        var end    = End.GetMovedRayInput().WithInvertedVector();

                        builder.AddFlexiFromPararell(start, middle, validator);
                        builder.LineTo(middle.Point);
                        builder.MarkEndWayPoint(ref idx, middle);

                        builder.AddFlexiFromPararell(middle, end, validator);
                        builder.LineTo(End.Point);
                        return builder.List;
                    }
                }

                {
                    var start  = Start.GetMovedRayOutput();
                    var middle = Reference;
                    var end    = End.GetMovedRayInput();

                    builder.AddFlexiFromPararell(start, middle, validator);
                    builder.MarkEndWayPoint(ref idx, middle);
                    builder.AddFlexi(middle, end, true);
                }
            }
            else
            {
                if (endCrossNullable is null)
                {
                    var start  = Start.GetMovedRayOutput();
                    var middle = Reference;
                    var end    = End.GetMovedRayInput().WithInvertedVector();

                    builder.AddFlexi(start, middle);
                    builder.MarkEndWayPoint(ref idx, middle);
                    builder.AddFlexiFromPararell(middle, end, validator);
                }
                else
                {
                    var cross1 = startCrossNullable.Value;
                    var cross2 = endCrossNullable.Value;
                    var q      = FromTwoCrosses(cross1, cross2, builder, validator);
                    if (q == FromTwoCrossesResult.TwoS)
                        return FlexiSShape();
                }
            }

            if (builder.List.Count == 0)
                builder.AddLine(Start.Point, End.Point);
            return builder.List;

            IReadOnlyList<IPathElement> FlexiSShape()
            {
                var middle = Reference;
                builder.Clear();
                builder.AddFlexi(Start, middle);
                builder.MarkEndWayPoint(ref idx, middle);

                var endd = new PathRayWithArm(End.Point, -End.Vector, End.ArmLength);
                builder.AddFlexi(middle, endd);
                return builder.List;
            }
        }
    }

    private FromTwoCrossesResult FromTwoCrosses(Point startCross, Point endCross,
        PathBuilder aggregator,
        IPathValidator? validator)
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
        var xStart     = Start.Vector.GetNormalized();
        var xEnd       = End.Vector.GetNormalized();

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

        var result1 = validator.ValidateArc(arc1, ArcDestination.OneReferenceTwoArcs);
        var result2 = validator.ValidateArc(arc2, ArcDestination.OneReferenceTwoArcs);
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
            aggregator.AddFlexi(Reference, End, true);
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
        var l1 = PathLineEquationNotNormalized.FromPointAndDeltas(Start.Point, Start.Vector);
        var l2 = PathLineEquationNotNormalized.FromPointAndDeltas(End.Point, End.Vector);
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

    private enum FromTwoCrossesResult
    {
        None,
        TwoS
    }
}
