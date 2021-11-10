﻿#define _DEEP_DEBUG
using System.Windows;
#if DEBUG && DEEP_DEBUG
using System.Diagnostics;
using Newtonsoft.Json;
#endif

namespace iSukces.DrawingPanel.Paths
{
    public sealed class ThreeReferencePointsPathCalculator : ReferencePointPathCalculator
    {
        public IPathResult Compute(IPathValidator validator)
        {
            var builder = new PathBuilder(Start.Point);

            void Add(PathRay st, PathRay en)
            {
                var s = ZeroReferencePointPathCalculator.Compute(st, en, validator);
                if (s is null)
                    return;
                builder.ArcTo(s.Arc1);
                builder.ArcTo(s.Arc2);
            }

            Add(Start, Reference1);
            Add(Reference1, Reference2);
            Add(Reference2, Reference3);
            Add(Reference3, End.WithInvertedVector());
            return builder.LineToAndCreate(End.Point);
        }

        public override void InitDemo()
        {
            Start      = new PathRay(new Point(-20, 0), new Vector(200, 100));
            End        = new PathRay(new Point(100, 0), new Vector(-100, 100));
            Reference1 = new PathRay(new Point(40, 20), new Vector(1, 0));
            Reference2 = new PathRay(new Point(50, 20), new Vector(1, 0.1));
            Reference3 = new PathRay(new Point(70, 18), new Vector(1, 0));
        }

        public override void SetReferencePoint(Point p, int nr)
        {
            const int x = 50;
            switch (nr)
            {
                case 0:
                    Reference1 = Reference1.With(p);
                    break;
                case 1:
                    Reference2 = Reference2.With(p);
                    break;
                case 2:
                    Reference3 = Reference3.With(p);
                    break;

                case 100:
                    Reference1 = Reference1.WithEnd(p, x);
                    break;
                case 101:
                    Reference2 = Reference2.WithEnd(p, x);
                    break;
                case 102:
                    Reference3 = Reference3.WithEnd(p, x);
                    break;
            }
        }

        public PathRay Reference1 { get; set; }
        public PathRay Reference2 { get; set; }
        public PathRay Reference3 { get; set; }
    }
}