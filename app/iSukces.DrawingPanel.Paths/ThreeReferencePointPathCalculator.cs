#define _DEEP_DEBUG
using System.Collections.Generic;
using System.Windows;
#if DEBUG && DEEP_DEBUG
using System.Diagnostics;
using Newtonsoft.Json;
#endif

namespace iSukces.DrawingPanel.Paths
{
    public sealed class ThreeReferencePointPathCalculator : ReferencePointPathCalculator
    {
        public IPathResult Compute(IPathValidator validator)
        {
#if DEBUG && DEEP_DEBUG
            {
                var json = JsonConvert.SerializeObject(this);
                Debug.WriteLine(json);
            }
#endif
            var list = new List<ArcDefinition>();

            void Add(PathRay st, PathRay en)
            {
                var s = ZeroReferencePointPathCalculator.Compute(st, en);
#if DEBUG && DEEP_DEBUG
                var code = new TestMaker().GetDebugCode(st, en, s);
                Debug.WriteLine("\r\n\r\n" + code + "\r\n\r\n");
#endif
                if (s is null)
                    return;
                if (s.Arc1 != null) list.Add(s.Arc1);
                if (s.Arc2 != null) list.Add(s.Arc2);
            }

            Add(Start, Reference1);
            Add(Reference1, Reference2);
            Add(Reference2, Reference3);
            Add(Reference3, End.WithInvertedVector());
            return new PathResult(Start.Point, End.Point, list.ToArray());
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
