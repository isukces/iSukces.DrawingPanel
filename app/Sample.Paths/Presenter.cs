using System;
using System.Drawing;
using System.Windows;
using iSukces.DrawingPanel.Interfaces;
using iSukces.DrawingPanel.Paths;
using iSukces.Mathematics;
using Point = System.Windows.Point;

namespace Sample.Paths
{
    internal class Presenter
    {
        public void Draw()
        {
            var validator = new MinimumValuesPathValidator(5, 4);

            DrawBuildings();
            DrawCircleWithVector(Calculator.Start);
            DrawCircleWithVector(Calculator.End);

            switch (Calculator)
            {
                case OneReferencePointPathCalculator one:
                {
                    var result = one.Compute(validator);
                    CodeMaker.Dump(one);
                    DrawYellowTriangle();
                    DrawCircle(one.Reference.Point);
                    DrawLineBeginEnd(result is null);
                    if (result is null)
                        return;

                    DrawPathResult(result);
                    break;
                }
                case TwoReferencePointsPathCalculator two:
                {
                    CodeMaker.Dump(two);
                    DrawCircle(two.Reference1.Point);
                    DrawCircle(two.Reference2.Point);
                    DrawLineBeginEnd(false);
                    var result = two.Compute(validator);
                    if (result != null)
                    {
                        DrawPathResult(result);
                    }

                    break;
                }
                case ThreeReferencePointsPathCalculator three:
                {
                    CodeMaker.Dump(three);
                    DrawLineBeginEnd(false);
                    var result = three.Compute(validator);
                    if (result != null)
                        DrawPathResult(result);

                    DrawCircleWithVector(three.Reference1, true);
                    DrawCircleWithVector(three.Reference2, true);
                    DrawCircleWithVector(three.Reference3, true);
                    break;
                }
            }
        }

        private void DrawArc(ArcDefinition c)
        {
            if (c is null)
                return;
            DrawCross(c.Center);

            void LineTo(Point p)
            {
                var       a   = Transformation.ToCanvasF(c.Center);
                var       b   = Transformation.ToCanvasF(p);
                using var pen = new Pen(Color.Cornsilk, 1);
                Graph.DrawLine(pen, a.X, a.Y, b.X, b.Y);
            }

            LineTo(c.Start);
            LineTo(c.End);
            {
                var ce = Transformation.ToCanvas(c.Center);

                var v1 = c.RadiusStart;
                var v2 = c.RadiusEnd;

                var dot1 = Vector.CrossProduct(v1, c.DirectionStart);

                var angle1 = v1.AngleMinusY();
                var angle2 = v2.AngleMinusY();

                var radius = v1.Length * Transformation.Scale;
                if (v1.Length < ReferencePointPathCalculator.Epsilon)
                    return;
                var r = new RectangleF(
                    (float)(ce.X - radius),
                    (float)(ce.Y - radius), (float)(radius * 2), (float)(radius * 2));
                using var pen = new Pen(Color.Gold, 5);
                if (dot1 < 0)
                {
                    var sweep = angle2 - angle1;
                    if (sweep < 0)
                        sweep += 360;
                    Graph.DrawArc(pen, r, (float)angle1, (float)sweep);
                }
                else
                {
                    var sweep = angle1 - angle2;
                    if (sweep < 0)
                        sweep += 360;
                    Graph.DrawArc(pen, r, (float)angle2, (float)sweep);
                }
            }
        }

        private void DrawArcLine(Point? p1, Point? p2)
        {
            if (p1 is null || p2 is null)
                return;
            var       a   = Transformation.ToCanvasF(p1.Value);
            var       b   = Transformation.ToCanvasF(p2.Value);
            using var pen = new Pen(Color.Blue, 5);
            Graph.DrawLine(pen, a.X, a.Y, b.X, b.Y);
        }

        private void DrawBuildings()
        {
            void DrawBuildin(double x1, double x2, double y1, double y2)
            {
                var pa = Transformation.ToCanvasF(x1, y1);
                var pb = Transformation.ToCanvasF(x2, y2);

                var x = Math.Min(pa.X, pb.X);
                var y = Math.Min(pa.Y, pb.Y);
                var w = Math.Abs(pa.X - pb.X);
                var h = Math.Abs(pa.Y - pb.Y);

                Graph.FillRectangle(Brushes.Purple, x, y, w, h);

                using var pen = new Pen(Color.Azure, 1);
                Graph.DrawRectangle(pen, x, y, w, h);
            }

            DrawBuildin(-50, 100, 50, 100);
        }

        private void DrawCircle(Point s)
        {
            var       a   = Transformation.ToCanvasF(s);
            using var pen = new Pen(Color.Fuchsia, 2);
            Graph.DrawEllipse(pen, a.X - Radius, a.Y - Radius, Radius * 2, Radius * 2);
        }

        private void DrawCircleWithVector(PathRay r, bool isShort = false)
        {
            DrawCircleWithVector(r.Point, r.Vector, isShort);
        }

        private void DrawCircleWithVector(Point s, Vector v, bool isShort = false)
        {
            var a = Transformation.ToCanvasF(s);
            {
                using var pen = new Pen(Color.Fuchsia, 2);
                Graph.DrawEllipse(pen, a.X - Radius, a.Y - Radius, Radius * 2, Radius * 2);
            }

            {
                if (isShort)
                {
                    v =  -v;
                    v *= 50 / v.Length;
                }

                v = new Vector(v.X, -v.Y);

                using var pen = new Pen(Color.LightGreen, isShort ? 2 : 5);
                var       bx  = a.X - (float)v.X;
                var       by  = a.Y - (float)v.Y;
                Graph.DrawLine(pen, a.X, a.Y, bx, by);
                if (isShort)
                {
                    var cs = SimpleCoordinateSystem2D.FromPointAndVector(new Point(bx, by), v);
                    var p1 = cs.Transform(new Point(15, 4));
                    var p3 = cs.Transform(new Point(15, -4));
                    var p2 = cs.Transform(new Point(0, 0));
                    Graph.DrawLine(pen, (float)p1.X, (float)p1.Y, (float)p2.X, (float)p2.Y);
                    Graph.DrawLine(pen, (float)p3.X, (float)p3.Y, (float)p2.X, (float)p2.Y);
                    Graph.DrawLine(pen, (float)p3.X, (float)p3.Y, (float)p1.X, (float)p1.Y);
                }
            }
        }

        private void DrawCross(Point s)
        {
            var       a   = Transformation.ToCanvasF(s);
            using var pen = new Pen(Color.Gainsboro, 1);
            Graph.DrawLine(pen, a.X - Radius, a.Y, a.X + Radius, a.Y);
            Graph.DrawLine(pen, a.X, a.Y - Radius, a.X, a.Y + Radius);
        }

        private void DrawLine(Point startPoint, Point endPoint, Pen pen)
        {
            var a = Transformation.ToCanvasF(startPoint);
            var b = Transformation.ToCanvasF(endPoint);
            Graph.DrawLine(pen, a.X, a.Y, b.X, b.Y);
        }


        private void DrawLineBeginEnd(bool isError)
        {
            using var pen = new Pen(isError ? Color.Red : Color.Green, 1);
            DrawLine(Calculator.Start.Point, Calculator.End.Point, pen);
        }

        private void DrawPathResult(IPathResult r)
        {
            var p = r.Elements;
            if (p.Count == 0)
            {
                DrawArcLine(r.Start, r.End);
                return;
            }

            foreach (var i in p)
            {
                switch (i)
                {
                    case ArcDefinition def:
                        DrawArc(def);
                        break;
                    case InvalidPathElement invalidPathElement:
                        break;
                    case LinePathElement linePathElement:
                        DrawArcLine(linePathElement.GetStartPoint(), linePathElement.GetEndPoint());
                        break;

                    default: throw new NotImplementedException();
                }
            }
        }

        private void DrawYellowTriangle()
        {
            if (Calculator is OneReferencePointPathCalculator one)
            {
                var cross = one.Get();
                if (cross is null)
                    return;
                var brush = new SolidBrush(Color.FromArgb(40, Color.Yellow));
                Graph.FillPolygon(brush, new[]
                {
                    Transformation.ToCanvasF(Calculator.Start.Point),
                    Transformation.ToCanvasF(Calculator.End.Point),
                    Transformation.ToCanvasF(cross.Value)
                });
                brush.Dispose();
            }
        }

        private const float Radius = 4;
        public Graphics                       Graph          { get; set; }
        public ReferencePointPathCalculator   Calculator     { get; set; }
        public IDrawingToPixelsTransformation Transformation { get; set; }
    }
}
