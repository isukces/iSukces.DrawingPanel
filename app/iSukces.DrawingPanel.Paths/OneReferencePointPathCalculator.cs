#define _OLD_RESULT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Paths
{
    public sealed class OneReferencePointPathCalculator : ReferencePointPathCalculator
    {
        public IPathResult Compute(IPathValidator validator)
        {
            Reference = Reference.With(End.Point - Start.Point);
            if (Reference.Vector.LengthSquared < LengthEpsilonSquare)
                return CreateResultLine();

            var list = Find();
            if (list.Count == 0)
                throw new NotImplementedException();
            return new PathResult(list);

            IReadOnlyList<IPathElement> Find()
            {
                var ll = new Aggregator
                {
                    CurrentPoint = Start.Point
                };

                var startCrossNullable = Start.Cross(Reference);
                var endCrossNullable   = End.Cross(Reference);

                if (startCrossNullable is null)
                {
                    if (endCrossNullable is null)
                    {
                        var line = LineEquation.Make(Start.Point, Reference.Vector);
                        var dist = line.DistanceNotNormalized(Reference.Point);

                        //if (dist * dist < LengthEpsilonSquare)
                        if (dist == 0)
                        {
                            ll.AddLine(Start.Point, End.Point);
                            return ll.List;
                        }

                        ll.Add(Start, Reference);
                        ll.Add(Reference, End.WithInvertedVector());
                        return ll.List;
                    }

                    ll.Add(Start, Reference);
                }
                else
                {
                    if (endCrossNullable is null)
                    {
                        ll.Add(Reference, End.WithInvertedVector());
                    }
                    else
                    {
                        var cross1 = startCrossNullable.Value;
                        var cross2 = endCrossNullable.Value;
                        var q      = FromTwoCrosses(cross1, cross2, ll, validator);
                        if (q == FromTwoCrossesResult.TwoS)
                        {
                            ll.Add(Start, Reference);
                            ll.Add(Reference, End.WithInvertedVector());
                            return ll.List;
                        }
                    }
                }

                if (ll.List.Count == 0)
                    ll.Add(Start, End.WithInvertedVector());

                /*
                if (endCrossNullable is null)
                {
                    ll.Add(Reference, End.WithInvertedVector());
                }*/

                return ll.List;
            }
        }

        private FromTwoCrossesResult FromTwoCrosses(Point startCross, Point endCross, Aggregator aggregator,
            IPathValidator validator)
        {
            var startIsInvalid = DotNotPositive(startCross, Start);
            var endIsInvalid   = DotNotPositive(endCross, End);
            if (startIsInvalid || endIsInvalid)
            {
                aggregator.AddLine(Start.Point, End.Point);
                return FromTwoCrossesResult.None;
            }

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
                var dot = tmp * vMiddle; // ujemne OK

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
                var dot = tmp * vMiddle; // dodatnie Ok
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
            var isOk    = result1 == ArcValidationResult.Ok && result2 == ArcValidationResult.Ok;

            if (isOk)
            {
                aggregator.ArcTo(arc1);
                aggregator.ArcTo(arc2);
                aggregator.LineTo(End.Point);
                return FromTwoCrossesResult.None;
            }

            return FromTwoCrossesResult.TwoS;
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


        private class Aggregator
        {
            public void Add(PathRay a, PathRay b)
            {
                var x = ZeroReferencePointPathCalculator.Compute(a, b);
                if (x is null)
                {
                    _list.Add(new InvalidPathElement(a, b, ArcValidationResult.UnableToConstructArc));
                    return;
                }

                if (x.Kind == ZeroReferencePointPathCalculator.ResultKind.Line)
                {
                    _list.Add(new LinePathElement(a.Point, b.Point));
                    return;
                }

                ArcTo(x.Arc1);
                ArcTo(x.Arc2);
                LineTo(b.Point);
            }


            public void AddLine(Point startPoint, Point endPoint)
            {
                var linePathElement = new LinePathElement(startPoint, endPoint);
                LineTo(startPoint);
                _list.Add(linePathElement);
                CurrentPoint = endPoint;
            }

            public void ArcTo(ArcDefinition arc)
            {
                if (arc is null)
                    return;
                LineTo(arc.Start);
                _list.Add(arc);
                CurrentPoint = arc.End;
            }

            public void LineTo(Point p)
            {
                var line = p - CurrentPoint;
                if (!(line.LengthSquared > LengthEpsilonSquare)) return;
                _list.Add(new LinePathElement(CurrentPoint, p));
                CurrentPoint = p;
            }

            public IReadOnlyList<IPathElement> List => _list;
            private readonly List<IPathElement> _list = new List<IPathElement>();
            public Point CurrentPoint;
        }
    }
}
