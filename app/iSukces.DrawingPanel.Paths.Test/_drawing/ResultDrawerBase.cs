using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using iSukces.Mathematics;
using Point = System.Windows.Point;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal class ResultDrawerBase
    {
        protected void CreateBitmap()
        {
            var x = XRange.Length / Width;
            var y = YRange.Length / Height;
            Scale = Math.Max(x, y);
            var width  = (XRange.Length / Scale).Round();
            var height = (YRange.Length / Scale).Round();
            Bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            var xLength = width * Scale;
            var yLength = height * Scale;
            XRange = MinMax.FromCenterAndSize(XRange.Center, xLength);
            YRange = MinMax.FromCenterAndSize(YRange.Center, yLength);
        }


        protected void DrawArc(ArcDefinition c)
        {
            if (c is null)
                return;
            DrawCross(c.Center, Color.Chocolate, 1);
            /*DrawCross(c.Start, Color.Cyan, 1);
            DrawCross(c.End, Color.Cyan, 1);*/

            void LineTo(Point p)
            {
                var       a   = Map(c.Center);
                var       b   = Map(p);
                using var pen = new Pen(Color.Chocolate, 1);
                Graph.DrawLine(pen, a.X, a.Y, b.X, b.Y);
            }

            LineTo(c.Start);
            LineTo(c.End);
            {
                var a  = Map(c.Start);
                var b  = Map(c.End);
                var ce = Map2(c.Center);

                var v1 = c.RadiusStart;
                var v2 = c.RadiusEnd;

                var dot1 = Vector.CrossProduct(v1, c.StartVector);
                var dot2 = Vector.CrossProduct(v2, c.EndVector);

                var angle1 = v1.AngleMinusY();
                var angle2 = v2.AngleMinusY();

                var radius = v1.Length / Scale;
                if (v1.Length < ReferencePointPathCalculator.Epsilon)
                    return;
                var r = new RectangleF(
                    (float)(ce.X - radius),
                    (float)(ce.Y - radius), (float)(radius * 2), (float)(radius * 2));
                using var pen = new Pen(Color.Black, 3);
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


        protected void DrawCircleWithVector(PathRay r, bool isShort = false)
        {
            DrawCircleWithVector(r.Point, r.Vector, isShort);
        }

        protected void DrawCircleWithVector(Point point, Vector v, bool isShort = false)
        {
            v.Normalize();
            v *= 50;
            var a = Map(point);

            {
                if (isShort)
                {
                    v =  -v;
                    v *= 50 / v.Length;
                }

                v = new Vector(v.X, -v.Y);

                using var pen = new Pen(Color.LightGreen, isShort ? 2 : 9);
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

            {
                using var pen = new Pen(Color.Fuchsia, 2);
                Graph.DrawEllipse(pen, a.X - Radius, a.Y - Radius, Radius * 2, Radius * 2);
            }
        }

        protected void DrawCross(Point point, Color c, int thickbess)
        {
            var         p     = Map(point);
            using var   pen   = new Pen(c, thickbess);
            const float delta = 8;
            Graph.DrawLine(pen, p.X - delta, p.Y - delta, p.X + delta, p.Y + delta);
            Graph.DrawLine(pen, p.X - delta, p.Y + delta, p.X + delta, p.Y - delta);
        }


        protected void DrawLine(Point startPoint, Point endPoint, Pen pen)
        {
            var a = Map(startPoint);
            var b = Map(endPoint);
            Graph.DrawLine(pen, a.X, a.Y, b.X, b.Y);
        }

        protected void GrayCross(Point point) { DrawCross(point, Color.Indigo, 3); }

        private double Map(double value, MinMax range)
        {
            var m = (value - range.Min) / Scale;
            return m;
        }

        private PointF Map(Point p)
        {
            var x = Map(p.X, XRange);
            var y = Bmp.Height - Map(p.Y, YRange);
            return new PointF((float)x, (float)y);
        }

        private Point Map2(Point p)
        {
            var x = Map(p.X, XRange);
            var y = Bmp.Height - Map(p.Y, YRange);
            return new Point(x, y);
        }


        protected const double AngleDelta = 10;
        protected const int Width = 1200;
        protected const int Height = 800;
        protected const float Radius = 4;

        public double Scale { get; set; }


        protected Bitmap Bmp;
        protected Graphics Graph;
        protected MinMax XRange;
        protected MinMax YRange;
    }
}
