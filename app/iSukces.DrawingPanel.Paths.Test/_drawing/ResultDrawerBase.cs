#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using iSukces.Mathematics;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif

namespace iSukces.DrawingPanel.Paths.Test;

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

    public void DrawArc(ArcDefinition c)
    {
        if (c is null)
            return;
        DrawCross(c.Center, Color.Chocolate, 1);
        /*DrawCross(c.Start, Color.Cyan, 1);
        DrawCross(c.End, Color.Cyan, 1);*/

        LineTo(c.Start);
        LineTo(c.End);
        {
            var a  = Map(c.Start);
            var b  = Map(c.End);
            var ce = Map2(c.Center);

            var v1 = c.RadiusStart;
            var v2 = c.RadiusEnd;

            var dot1 = Vector.CrossProduct(v1, c.DirectionStart);
            var dot2 = Vector.CrossProduct(v2, c.DirectionEnd);

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
                try
                {
                    Graph.DrawArc(pen, r, (float)angle1, (float)sweep);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                var sweep = angle1 - angle2;
                if (sweep < 0)
                    sweep += 360;
                try
                {
                    Graph.DrawArc(pen, r, (float)angle2, (float)sweep);
                }
                catch
                {
                }
            }
        }
        return;

        void LineTo(Point p)
        {
            var       a   = Map(c.Center);
            var       b   = Map(p);
            using var pen = new Pen(Color.Chocolate, 1);
            try
            {
                Graph.DrawLine(pen, a.X, a.Y, b.X, b.Y);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    protected void DrawArrow(Point point, Vector v, bool arrowStart, Color? color = null,
        double ahl = arrowHeadLength, float linethickness = 2)
    {
        var factor = ahl / arrowHeadLength;
        var b      = Map(point).ToPoint();
        v = new Vector(v.X, -v.Y);
        v.Normalize();
        if (!arrowStart)
            b -= v * ahl;

        var c  = color ?? Color.Fuchsia;
        var br = new SolidBrush(c);

        var cs = SimpleCoordinateSystem2D.FromPointAndVector(b, v);
        var p1 = cs.Transform(new Point(ahl - 15 * factor, 4)).ToPointF();
        var p3 = cs.Transform(new Point(ahl - 15 * factor, -4)).ToPointF();
        var p2 = cs.Transform(new Point(ahl, 0)).ToPointF();
        Graph.FillPolygon(br, new[] { p1, p2, p3 });

        var       p4  = cs.Transform(new Point(0, 0)).ToPointF();
        using var pen = new Pen(c, linethickness);
        Graph.DrawLine(pen, p2, p4);
    }

    public void DrawCircleWithVector(PathRayWithArm r, bool isShort = false)
    {
        DrawCircleWithVector(r.Point, r.Vector, isShort);
    }

    public void DrawCircleWithVector(PathRay r, bool isShort = false)
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
                v *= arrowHeadLength / v.Length;
            }

            v = new Vector(v.X, -v.Y);
            var v1 = v;
            if (isShort)
            {
                v1 *= (arrowHeadLength - arrowHeadSize) / arrowHeadLength;
            }

            var       lineColor = isShort ? Color.Fuchsia : Color.LightGreen;
            using var pen       = new Pen(lineColor, isShort ? 2 : 9);
            var       bx        = a.X - (float)v1.X;
            var       by        = a.Y - (float)v1.Y;
            Graph.DrawLine(pen, a.X, a.Y, bx, by);
            if (isShort)
            {
                bx = a.X - (float)v.X;
                by = a.Y - (float)v.Y;

                var cs = SimpleCoordinateSystem2D.FromPointAndVector(new Point(bx, by), v);
                var p1 = cs.Transform(new Point(15, 4)).ToPointF();
                var p3 = cs.Transform(new Point(15, -4)).ToPointF();
                var p2 = cs.Transform(new Point(0, 0)).ToPointF();

                Graph.FillPolygon(Brushes.Fuchsia, new[] { p1, p2, p3 });
            }
        }

        {
            using var pen = new Pen(Color.Fuchsia, 2);
            Graph.DrawEllipse(pen, a.X - Radius, a.Y - Radius, Radius * 2, Radius * 2);
        }
    }


    public void DrawCross(Point point, Color c, int thickness)
    {
        var         p     = Map(point);
        using var   pen   = new Pen(c, thickness);
        const float delta = 8;
        Graph.DrawLine(pen, p.X - delta, p.Y - delta, p.X + delta, p.Y + delta);
        Graph.DrawLine(pen, p.X - delta, p.Y + delta, p.X + delta, p.Y - delta);
    }

    public void DrawCustom(string description,
        Func<IEnumerable<Point>> range,
        Action drawAction, [CallerFilePath] string path = null)
    {
        var dir = ArcResultDrawer.GetDir(path);
        if (dir is null)
            return;
        {
            XRange = new MinMax();
            YRange = new MinMax();
            foreach (var i in range())
            {
                XRange.Add(i.X);
                YRange.Add(i.Y);
            }

            ArcResultDrawer.Grow(ref XRange);
            ArcResultDrawer.Grow(ref YRange);
        }

        CreateBitmap();

        Graph               = Graphics.FromImage(Bmp);
        Graph.SmoothingMode = SmoothingMode.HighQuality;
        Graph.FillRectangle(Brushes.White, 0, 0, Bmp.Width, Bmp.Height);

        drawAction();
        Graph.DrawString(description, new Font("Arial", 10), Brushes.Black, 5, 5);
        Graph.Dispose();
        var fileInfo = new FileInfo(Path.Combine(dir.FullName, description.ToFileName() + ".png"));
        fileInfo.Directory?.Create();
        Bmp.SaveIfDifferent(fileInfo.FullName);
        Bmp.Dispose();
    }

    public void DrawLine(Pen pen, Point a, Point b)
    {
        var p1 = Map(a);
        var p2 = Map(b);
        Graph.DrawLine(pen, p1, p2);
    }


    protected void DrawLine(Point startPoint, Point endPoint, Pen pen)
    {
        var a = Map(startPoint);
        var b = Map(endPoint);
        Graph.DrawLine(pen, a.X, a.Y, b.X, b.Y);
    }


    public void GrayCross(double x, double y)
    {
        GrayCross(new Point(x, y));
    }

    public void GrayCross(Point point)
    {
        DrawCross(point, Color.Indigo, 3);
    }

    public PointF Map(Point p)
    {
        var x = Map(p.X, XRange);
        var y = Bmp.Height - Map(p.Y, YRange);
        return new PointF((float)x, (float)y);
    }


    private double Map(double value, MinMax range)
    {
        var m = (value - range.Min) / Scale;
        return m;
    }


    private Point Map2(Point p)
    {
        var x = Map(p.X, XRange);
        var y = Bmp.Height - Map(p.Y, YRange);
        return new Point(x, y);
    }

    public Point MapRev(Point p)
    {
        var x = MapRev(p.X, XRange);
        var y = MapRev(Bmp.Height - p.Y, YRange);
        return new Point(x, y);
    }

    private double MapRev(double value, MinMax range)
    {
        return value * Scale + range.Min;
    }

    #region properties

    public double Scale { get; set; }

    #endregion

    #region Fields

    const double arrowHeadLength = 50;
    const double arrowHeadSize = 15;


    protected const double AngleDelta = 10;
    protected const int Width = 1200;
    protected const int Height = 800;
    protected const float Radius = 4;


    protected Bitmap Bmp;
    protected Graphics Graph;
    protected MinMax XRange;
    protected MinMax YRange;

    #endregion
}
