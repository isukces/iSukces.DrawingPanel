using System.Collections.Generic;
using System.ComponentModel;
#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    [ImmutableObject(true)]
    public sealed class ZeroReferencePointPathCalculatorLineResult : IPathResult
    {
        public ZeroReferencePointPathCalculatorLineResult(Point start, Point end)
        {
            _line = new LinePathElement(start, end);
        }

        public double GetLength() { return _line.GetLength(); }

        public override string ToString() { return $"Line {Start} .. {End}"; }

        public Point Start => _line.GetStartPoint();
        public Point End   => _line.GetEndPoint();

        public IReadOnlyList<IPathElement> Elements
        {
            get
            {
                return new[]
                {
                    _line
                };
            }
        }

        public Vector StartVector => _line.GetStartVector();

        public Vector EndVector => _line.GetEndVector();
        private readonly LinePathElement _line;
    }
}
