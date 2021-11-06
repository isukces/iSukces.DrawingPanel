using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal class DpAssertsBuilder : AssertsBuilder
    {
        private static string Create(PathRay tmp) { return $"new PathRay({tmp.Point.ToCs()}, {tmp.Vector.ToCs()})"; }

        private static string FindName(ZeroReferencePointPathCalculator.Result result)
        {
            if (result is null)
                return "";
            var s = new StringBuilder();
            s.Append(result.Kind);

            void A(ArcDefinition arc)
            {
                if (arc is null) return;
                s.Append("_" + arc.GetDirectionAlternative());
                s.Append("_" + arc.Angle.Round());
            }

            A(result.Arc1);
            A(result.Arc2);

            return s.ToString();
        }

        private void Add(ArcDefinition c, string name)
        {
            if (c is null)
                AssertNull(name);
            else
            {
                name += ".";
                Add(c.Direction, name + nameof(c.Direction));
                Add(c.Angle, name + nameof(c.Angle));
                Add(c.Center, name + nameof(c.Center));
                Add(c.Start, name + nameof(c.Start));
                Add(c.End, name + nameof(c.End));
                Add(c.StartVector, name + nameof(c.StartVector));
            }
        }


        private void Add(ArcDirection n, string name) { AssertEqual("ArcDirection." + n, name); }

        private void Add(ZeroReferencePointPathCalculator.ResultKind n, string name)
        {
            AssertEqual("ZeroReferencePointPathCalculator.ResultKind." + n, name);
        }


        private void Add(Point p, string name) { AssertExEqual(p.X.ToCs(), p.Y.ToCs(), name); }

        private void Add(PathRay p, string name)
        {
            AssertExEqual(p.Point.X.ToCs(), p.Point.Y.ToCs(), p.Vector.X.ToCs(), p.Vector.Y.ToCs(), name);
        }

        private void Add(Vector p, string name) { AssertExEqual(p.X.ToCs(), p.Y.ToCs(), name); }

        private void Add(IPathResult result, string name)
        {
            if (result is null)
            {
                AssertNull(name);
                return;
            }

            name += ".";
            if (result is ZeroReferencePointPathCalculator.Result zero)
            {
                Add(zero.Kind, name + nameof(zero.Kind));
            }

            Add(result.Start, name + nameof(result.Start));
            Add(result.End, name + nameof(result.End));
            Add(result.Elements, name + nameof(result.Elements));
        }

        private void Add(IReadOnlyList<IPathElement> a, string name)
        {
            AssertList(a, name, Add);
        }

        private void Add(InvalidPathElement x, string name)
        {
            name += ".";
            Add(x.Status, name + "." + nameof(x.Status));
            AddIPathElement(x, name);
        }

        private void Add(ArcValidationResult value, string expression)
        {
            AssertEqual(nameof(ArcValidationResult) + "." + value, expression);
        }

        private void Add(IPathElement el, string name)
        {
            switch (el)
            {
                case ArcDefinition arcDefinition:
                    var n = Declare(arcDefinition, name);
                    Add(arcDefinition, n);
                    Release(n);
                    break;
                case InvalidPathElement invalidPathElement:
                    n = Declare(invalidPathElement, name);
                    Add(invalidPathElement, n);
                    Release(n);
                    break;
                case LinePathElement linePathElement:
                    n = "(LinePathElement)" + name;
                    Add(linePathElement, n);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(el));
            }
        }


        private void Add(LinePathElement x, string name)
        {
            var s = x.GetStartPoint();
            var e = x.GetEndPoint();
            var a = new[]
            {
                s.X.ToCs(), s.Y.ToCs(), e.X.ToCs(), e.Y.ToCs(),
                name, "6"
            };
            AssertExEqual(a);
        }

        private void AddIPathElement(IPathElement x, string name)
        {
            name += ".";
            var point = x.GetStartPoint();
            var n     = Declare(point, name + nameof(x.GetStartPoint) + "()");
            Add(point, n);
            Release(n);

            point = x.GetEndPoint();
            n     = Declare(point, name + nameof(x.GetEndPoint) + "()");
            Add(point, n);
            Release(n);
        }


        public string Create(IPathResult result, string name)
        {
            return Create(() =>
            {
                Add(result, name);
            });
        }


        public string GetDebugCode(PathRay st, PathRay en, ZeroReferencePointPathCalculator.Result result)
        {
            var name = FindName(result);
            WriteLine("[Fact]");
            WriteLine("public void Txxx_" + name + "()");
            WriteLine("{");

            /*
            _sb.AppendLine("var c = new ZeroReferencePointPathCalculator();");
            _sb.AppendLine($"c.Start = {Create(st)};");
            _sb.AppendLine($"c.End = {Create(en)};");
            */
            WriteLine($"var r = ZeroReferencePointPathCalculator.Compute({Create(st)}, {Create(en)});");
            if (result is not null)
            {
                Add(result.Kind, "r.Kind");
                Add(result.Arc1, nameof(result.Arc1));
                Add(result.Arc2, nameof(result.Arc2));
            }

            WriteLine("}");
            return Code;
        }
    }
}
