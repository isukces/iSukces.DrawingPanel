using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public sealed class ZeroReferencePointPathCalculator : ReferencePointPathCalculator
    {
        private ZeroReferencePointPathCalculator() { }

        private static bool Check(Result result, ArcDefinition one)
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

        public static Result Compute(PathRay start, PathRay end, IPathValidator validator)
        {
            var x = new ZeroReferencePointPathCalculator
            {
                Start     = start,
                End       = end,
                Validator = validator
            };
            return x.Compute();
        }


        private Result Compute()
        {
            var cross = Start.Cross(End);
            if (cross is null)
            {
                var a = (End.Point - Start.Point);
                if (a.LengthSquared == 0)
                {
                    var result = new Result(ResultKind.Point)
                    {
                        Start = Start.Point,
                        End   = End.Point
                    };
                    return result;
                }

                var dot = Vector.CrossProduct(Start.Vector, a);
                if (dot == 0)
                    return new Result(ResultKind.Line)
                    {
                        Start = Start.Point,
                        End   = End.Point
                    };
                return TryTwo(null);
            }

            var one = TryOne(cross.Value);
            if (IsOk(one))
            {
                if (one.Angle < 180)
                    return new Result(ResultKind.OneArc)
                    {
                        Arc1  = one,
                        Start = Start.Point,
                        End   = End.Point
                    };
            }

            return TryTwo(null);
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

        private Result TryTwo(ArcDefinition one)
        {
            var toCompare = one;
            if (toCompare != null)
                if (toCompare.RadiusStart.Length <= 3)
                    toCompare = null;

            Start = Start.Normalize();
            End   = End.Normalize();
            var    minLength  = double.MaxValue;
            Result bestResult = null;

            void CheckAndAdd(Result r)
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

            CheckAndAdd(TryTwoArcsSolution(false, false));
            CheckAndAdd(TryTwoArcsSolution(true, false));
            CheckAndAdd(TryTwoArcsSolution(false, true));
            CheckAndAdd(TryTwoArcsSolution(true, true));

            if (bestResult != null)
                return bestResult;
            if (one != null)
                return new Result(ResultKind.OneArc)
                {
                    Arc1  = one,
                    Start = Start.Point,
                    End   = End.Point
                };

            return null;
        }

        private Result TryTwoArcsSolution(bool reverseEnd, bool useSmallerRadius)
        {
            var finder = new TwoArcsFinder();
            finder.UpdateFromPoints(Start, End, reverseEnd);
            // finder.Normalize();
            var isOk = finder.Compute(out var arc1, out var arc2, useSmallerRadius);
            if (!isOk) return null;

            var dot = Start.Vector * arc1.StartVector;
            if (dot <= 0) return null;
            dot = End.Vector * arc2.EndVector;
            if (dot <= 0) return null;
            return new Result(ResultKind.TwoArcs)
            {
                Arc1  = arc1,
                Arc2  = arc2,
                Start = Start.Point,
                End   = End.Point
            };
        }

        public IPathValidator Validator { get; set; }


        public enum ResultKind
        {
            /// <summary>
            ///     Początek i koniec jest jednym punktem
            /// </summary>
            Point,

            /// <summary>
            ///     Pocz-koniec jest poprawną linią
            /// </summary>
            Line,

            OneArc,
            TwoArcs
        }


        public sealed class Result : IPathResult
        {
            public Result(ResultKind kind) { Kind = kind; }

            public double GetLength(Point s, Point e)
            {
                double d = 0;
                Point  p = s;

                void AddArc(ArcDefinition a)
                {
                    if (a is null)
                        return;
                    var v = a.Start - p;
                    d += v.Length;
                    d += a.GetLength();
                    p =  a.End;
                }

                {
                    AddArc(Arc1);
                    AddArc(Arc2);
                    var v = e - p;
                    d += v.Length;
                    return d;
                }
            }

            public double GetLength()
            {
                var r = 0d;
                if (Arc1 is not null)
                    r += Arc1.GetLength();
                if (Arc2 is not null)
                {
                    r += Arc2.GetLength();
                    if (Arc1 is not null)
                    {
                        r += (Arc1.End - Arc2.Start).Length;
                    }
                }

                return r;
            }

            public override string ToString()
            {
                switch (Kind)
                {
                    case ResultKind.OneArc: return $"{Arc1}";
                    case ResultKind.TwoArcs: return $"{Arc1} {Arc2}";
                    case ResultKind.Point: return "Point";
                    case ResultKind.Line: return "Line";
                    default: return "Unknown";
                }
            }

            public Point Start { get; set; }
            public Point End   { get; set; }

            public IReadOnlyList<IPathElement> Elements
            {
                get
                {
                    switch (Kind)
                    {
                        case ResultKind.Line:
                            return new[]
                            {
                                new LinePathElement(Start, End)
                            };
                        case ResultKind.Point: return Array.Empty<IPathElement>();
                        case ResultKind.OneArc:
                        case ResultKind.TwoArcs:
                            var builder = new PathBuilder(Start);
                            builder.ArcTo(Arc1);
                            builder.ArcTo(Arc2);
                            builder.LineTo(End);
                            return builder.List;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }
            }

            public ResultKind    Kind { get; }
            public ArcDefinition Arc1 { get; set; }
            public ArcDefinition Arc2 { get; set; }
        }
    }
}
