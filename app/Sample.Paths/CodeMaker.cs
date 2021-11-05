using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using iSukces.DrawingPanel.Paths;

namespace Sample.Paths
{
    public class CodeMaker
    {
        public static void Dump(OneReferencePointPathCalculator x)
        {
            var a     = new CodeMaker();
            var props = new Props();
            props.Add("Start", Get(x.Start));
            props.Add("End", Get(x.End));
            props.Add("Reference", GetWithNoVector(x.Reference));

            a.Open<OneReferencePointPathCalculator>("calc");
            a.WriteLine(props);
            a.WriteLine("};");
            Debug.WriteLine(a.ToString());
        }

        public static void Dump(ThreeReferencePointsPathCalculator x)
        {
            var a     = new CodeMaker();
            var props = new Props();
            props.Add("Start", Get(x.Start));
            props.Add("End", Get(x.End));
            props.Add("Reference1", Get(x.Reference1));
            props.Add("Reference2", Get(x.Reference2));
            props.Add("Reference3", Get(x.Reference3));

            a.Open<ThreeReferencePointsPathCalculator>("calc");
            a.WriteLine(props);
            a.WriteLine("};");
            Debug.WriteLine(a.ToString());
        }


        public static void Dump(TwoReferencePointsPathCalculator x)
        {
            var a     = new CodeMaker();
            var props = new Props();
            props.Add("Start", Get(x.Start));
            props.Add("End", Get(x.End));
            props.Add("Reference1", GetWithNoVector(x.Reference1));
            props.Add("Reference2", GetWithNoVector(x.Reference2));

            a.Open<TwoReferencePointsPathCalculator>("calc");
            a.WriteLine(props);
            a.WriteLine("};");
            Debug.WriteLine(a.ToString());
        }

        private static string Get(PathRay ray)
        {
            var doubles = new[]
            {
                Math.Round(ray.Point.X, 5),
                Math.Round(ray.Point.Y, 5),
                ray.Vector.X,
                ray.Vector.Y
            };
            var strings = doubles.Select(x => x.ToInv());
            var args    = string.Join(", ", strings);
            return $"new PathRay({args})";
        }

        private static string GetWithNoVector(PathRay ray)
        {
            ray = ray.With(new Vector(0, 0));
            return Get(ray);
        }

        private void Open<T>(string varName)
        {
            var name = typeof(T).Name;
            WriteLine($"var {varName} = new {name} {{");
        }

        public override string ToString() { return _sb.ToString(); }

        private void WriteLine(Props props)
        {
            var lines = props.GetLines();
            foreach (var i in lines)
                WriteLine(i);
        }

        private void WriteLine(string txt) { _sb.AppendLine(txt); }

        private readonly StringBuilder _sb = new();

        private sealed class Props
        {
            public void Add(string name, string value) { _props.Add(Tuple.Create(name, value)); }

            public IEnumerable<string> GetLines()
            {
                if (_props.Count == 0)
                    yield break;
                var maxLen = _props.Max(x => x.Item1.Length) + 1;
                for (var index = 0; index < _props.Count; index++)
                {
                    var p    = _props[index];
                    var name = p.Item1;
                    if (name.Length < maxLen)
                        name += new string(' ', maxLen - name.Length);
                    var code = $"    {name} = {p.Item2}";
                    if (index < _props.Count - 1)
                        code += ",";
                    yield return code;
                }
            }

            private readonly List<Tuple<string, string>> _props = new();
        }
    }
}
