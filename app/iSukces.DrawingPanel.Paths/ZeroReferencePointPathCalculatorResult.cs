using System;
using System.Collections.Generic;
#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    public sealed class ZeroReferencePointPathCalculatorResult : IPathResult
    {
        public ZeroReferencePointPathCalculatorResult(ZeroReferencePointPathCalculator.ResultKind kind) { Kind = kind; }

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
                case ZeroReferencePointPathCalculator.ResultKind.OneArc: return $"{Arc1}";
                case ZeroReferencePointPathCalculator.ResultKind.TwoArcs: return $"{Arc1} {Arc2}";
                case ZeroReferencePointPathCalculator.ResultKind.Point: return "Point";
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
                    case ZeroReferencePointPathCalculator.ResultKind.Point: return Array.Empty<IPathElement>();
                    case ZeroReferencePointPathCalculator.ResultKind.OneArc:
                    case ZeroReferencePointPathCalculator.ResultKind.TwoArcs:
                        var builder = new PathBuilder(Start);
                        builder.ArcTo(Arc1);
                        builder.ArcTo(Arc2);
                        builder.LineTo(End);
                        return builder.List;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Vector StartVector
        {
            get
            {
                switch (Kind)
                {
                    case ZeroReferencePointPathCalculator.ResultKind.Point:
                        return default;
                    case ZeroReferencePointPathCalculator.ResultKind.OneArc:
                    case ZeroReferencePointPathCalculator.ResultKind.TwoArcs:
                        var line = Arc1.Start - Start;
                        if (line.LengthSquared < PathBase.LengthEpsilonSquare)
                            return Arc1.StartVector;
                        return line;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Vector EndVector
        {
            get
            {
                switch (Kind)
                {
                    case ZeroReferencePointPathCalculator.ResultKind.Point:
                        return default;
                    case ZeroReferencePointPathCalculator.ResultKind.OneArc:
                        var line = End - Arc1.End;
                        if (line.LengthSquared < PathBase.LengthEpsilonSquare)
                            return Arc1.EndVector;
                        return line;
                    case ZeroReferencePointPathCalculator.ResultKind.TwoArcs:
                        line = End - Arc2.End;
                        if (line.LengthSquared < PathBase.LengthEpsilonSquare)
                            return Arc2.EndVector;
                        return line;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public ZeroReferencePointPathCalculator.ResultKind Kind { get; }
        public ArcDefinition                               Arc1 { get; set; }
        public ArcDefinition                               Arc2 { get; set; }
    }
}
