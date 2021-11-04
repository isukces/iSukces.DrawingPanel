using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal class TestMaker
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


        private void Add(double n, string name) { AssertEqual(n.ToCs(), name, "6"); }

        private void Add(int n, string name) { AssertEqual(n.ToCs(), name); }

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
            Add(result.Start, name + nameof(result.Start));
            Add(result.End, name + nameof(result.End));
            Add(result.Arcs, name + nameof(result.Arcs));
        }

        private void Add(IReadOnlyList<IPathElement> a, string name)
        {
            if (a is null)
            {
                AssertNull(name);
                return;
            }

            if (a.Count == 1)
                AssertSingle(name);
            else
                Add(a.Count, name + "." + nameof(a.Count));
            for (var index = 0; index < a.Count; index++)
            {
                var name2 = $"{name}[{index.ToCs()}]";
                Add(a[index], name2);
            }
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
                    _variables.Release(n);
                    break;
                case InvalidPathElement invalidPathElement:
                    n = Declare(invalidPathElement, name);
                    Add(invalidPathElement, n);
                    _variables.Release(n);
                    break;
                case LinePathElement linePathElement:
                    n = "(LinePathElement)" + name;
                    Add(linePathElement, n);
                    // _variables.Release(n);
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
            _variables.Release(n);

            point = x.GetEndPoint();
            n     = Declare(point, name + nameof(x.GetEndPoint) + "()");
            Add(point, n);
            _variables.Release(n);
        }

        private void Assert(string method, params string[] x)
        {
            var a = string.Join(", ", x);
            WriteLine($"Assert.{method}({a});");
        }


        private void AssertEqual(params string[] x) { Assert("Equal", x); }

        private void AssertExEqual(params string[] a) { WriteLine("AssertEx.Equal(" + string.Join(", ", a) + ");"); }

        private void AssertNull(string name) { Assert("Null", name); }

        private void AssertSingle(string name) { Assert("Single", name); }

        public string Create(IPathResult result, string name)
        {
            _sb = new StringBuilder();
            WriteLine("#region Asserts");
            Add(result, name);
            WriteLine("#endregion");
            return _sb.ToString();
        }

        private string Declare<T>(T info, string expression)
        {
            var n  = _variables.GetName(typeof(T), out var first);
            var ex = n + " = (" + typeof(T).Name + ")" + expression + ";";
            if (first)
                ex = "var " + ex;
            WriteLine(ex);
            return n;
        }

        public string GetDebugCode(PathRay st, PathRay en, ZeroReferencePointPathCalculator.Result result)
        {
            _sb = new StringBuilder();
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
            return _sb.ToString();
        }

        private void WriteLine(string x) { _sb.AppendLine(x); }

        private readonly Variables _variables = new();

        private StringBuilder _sb;

        private class Variables
        {
            public string GetName(Type t, out bool first)
            {
                foreach (var i in _list)
                {
                    if (i.Busy)
                        continue;
                    if (i.Typ != t) continue;
                    i.Busy = true;
                    first  = false;
                    return i.Name;
                }

                var nr = _list.Count + 1;
                var info = new Info
                {
                    Busy = true,
                    Name = "tmp" + nr.ToCs(),
                    Typ  = t
                };
                _list.Add(info);
                first = true;
                return info.Name;
            }

            public void Release(string s)
            {
                foreach (var i in _list)
                {
                    if (i.Name == s)
                    {
                        i.Busy = false;
                        return;
                    }
                }
            }

            private readonly List<Info> _list = new();

            private class Info
            {
                public bool   Busy { get; set; }
                public Type   Typ  { get; set; }
                public string Name { get; set; }
            }
        }
    }
}
