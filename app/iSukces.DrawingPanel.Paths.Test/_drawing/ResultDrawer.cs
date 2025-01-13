#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
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

internal class ResultDrawer : ResultDrawerBase
{
    protected ResultDrawer(ResultDrawerConfig cfg) { _cfg = cfg; }


    public static void Draw(OneReferencePointPathCalculator x, IPathResult r, TestName name,
        [CallerFilePath] string path = null)
    {
        // ReSharper disable once ExplicitCallerInfoArgument
        Draw(new ResultDrawerConfig
        {
            Start              = x.Start,
            End                = x.End,
            Result             = r,
            Title              = name,
            ExtraPoints        = ExtraPoints,
            ExtraDrawingBottom = ExtraDraw
        }, path);
        return;

        void ExtraDraw(ResultDrawer g)
        {
            g.DrawCircleWithVector(x.Reference, true);
            ExtraDraw2(g);
        }

        IEnumerable<Point> ExtraPoints()
        {
            yield return x.Start.Point;
            yield return x.End.Point;
            yield return x.Reference.Point;
        }

        void ExtraDraw2(ResultDrawer g)
        {
            var crossNullable = x.Start.CrossMeAsBeginWithEnd(x.End);
            if (!crossNullable.HasValue) return;
            var cross = crossNullable.Value;
            var dot   = (cross - x.Start.Point) * x.Start.Vector;
            if (dot < 0)
                return;
            dot = (cross - x.End.Point) * x.End.Vector;
            if (dot < 0)
                return;

            using var pen = new Pen(Color.Aqua, 1)
            {
                DashStyle = DashStyle.DashDot
            };
            var p1 = g.Map(x.Start.Point);
            var p2 = g.Map(x.End.Point);
            var p3 = g.Map(cross);
            g.Graph.DrawPolygon(pen, new[] { p1, p2, p3, p1 });
        }
    }

    public static void Draw(ArcPathSegmentMaker x, IPathResult r, TestName name,
        [CallerFilePath] string path = null)
    {
        var start = x.PreviousPoint;
        var end   = x.Point;

        // ReSharper disable once ExplicitCallerInfoArgument
        Draw(new ResultDrawerConfig
        {
            Start              = new PathRay(start.Location, start.OutVector),
            End                = new PathRay(end.Location, -end.InVector),
            Result             = r,
            Title              = name,
            ExtraPoints        = ExtraPoints,
            ExtraDrawingBottom = ExtraDraw
        }, path);
        return;

        void ExtraDraw2(ResultDrawer g)
        {
            /*var crossNullable = x.Start.Cross(x.End);
            if (!crossNullable.HasValue) return;
            var cross = crossNullable.Value;
            var dot   = (cross - x.Start.Point) * x.Start.Vector;
            if (dot < 0)
                return;
            dot = (cross - x.End.Point) * x.End.Vector;
            if (dot < 0)
                return;

            using var pen = new Pen(Color.Aqua, 1)
            {
                DashStyle = DashStyle.DashDot
            };
            var p1 = g.Map(x.Start.Point);
            var p2 = g.Map(x.End.Point);
            var p3 = g.Map(cross);
            g.Graph.DrawPolygon(pen, new[] { p1, p2, p3, p1 });*/
        }

        void ExtraDraw(ResultDrawer g)
        {
            foreach (var i in x.Point.ReferencePoints)
            {
                g.DrawCircleWithVector(i.OutputRay, true);
                // g.DrawCircleWithVector(x.Reference, true);
            }
                  
            ExtraDraw2(g);
        }

        IEnumerable<Point> ExtraPoints()
        {
            yield return start.Location;
            yield return end.Location;
            foreach (var i in x.Point.ReferencePoints)
            {
                yield return i.OutputRay.Point;
            }
        }
    }

    public static void Draw(ResultDrawerConfig cfg, [CallerFilePath] string path = null)
    {
        if (cfg.Result is null)
            return;
        var dir = GetDir(path);
        if (dir is null)
            return;
        cfg.OutputDirectory = dir;
        var p = new ResultDrawer(cfg);
        p.DrawInternal();
    }

    protected static DirectoryInfo GetDir(string path, string additional = null)
    {
        if (string.IsNullOrEmpty(path))
            return null;
        var directory = new FileInfo(path).Directory;
        while (directory != null)
        {
            var h = Path.Combine(directory.FullName, ".git", "HEAD");
            if (File.Exists(h))
            {
                var combine = Path.Combine(directory.FullName, "doc", "testDrawings");
                if (additional != null)
                    combine = Path.Combine(combine, additional);
                return new DirectoryInfo(combine);
            }

            directory = directory.Parent;
        }

        return null;
    }

    private static IEnumerable<Point> GetPoints(ArcDefinition arc)
    {
        yield return arc.Center;

        /*var plus              = arc.Direction == ArcDirection.CounterClockwise;
        var arcAngle          = arc.Angle;
        var angle             = AngleDelta;
        var startAngle        = MathEx.Atan2Deg(arc.RadiusStart);
        var radiusStartLength = arc.RadiusStart.Length;
        while (angle <= arcAngle)
        {
            var angleDeg = plus ? startAngle + angle : startAngle - angle;
            var sc       = MathEx.GetSinCosV(angleDeg);
            var point    = arc.Center + sc * radiusStartLength;
            yield return point;
            angle += AngleDelta;
        }

        yield return arc.Center;*/
    }

    private static void Grow(ref MinMax range)
    {
        //range.Grow(1);
        var size = range.Length + Math.Max(4, range.Length * 0.07);
        range = MinMax.FromCenterAndSize(range.Center, size);
    }


    private void DrawInternal()
    {
        ScanRange();

        CreateBitmap();

        Graph               = Graphics.FromImage(Bmp);
        Graph.SmoothingMode = SmoothingMode.HighQuality;
        {
            Graph.FillRectangle(Brushes.White, 0, 0, Bmp.Width, Bmp.Height);
            DrawCircleWithVector(_cfg.Start);
            if ((_cfg.Flags & ResultDrawerConfigFlags.ReverseEndMarker) != 0)
                DrawCircleWithVector(_cfg.End.GetMovedRayInput().WithInvertedVector());
            else
                DrawCircleWithVector(_cfg.End);

            _cfg.ExtraDrawingBottom?.Invoke(this);

            // var p = _start.Point;
            foreach (var element in _cfg.Result.Elements)
            {
                // DrawLine(p, element.GetStartPoint(), new Pen(Color.Gainsboro, 1));

                switch (element)
                {
                    case ArcDefinition arcDefinition:
                        DrawArc(arcDefinition);
                        // throw new NotImplementedException("Drawer");
                        break;
                    case LinePathElement linePathElement:
                        DrawLine(element.GetStartPoint(), element.GetEndPoint(), new Pen(Color.Blue, 3));
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(element));
                }

                // p = element.GetEndPoint();
            }

            // DrawLine(_result.Start, _result.End, new Pen(Color.Crimson, 2));
            _cfg.ExtraDrawingTop?.Invoke(this);
        }
        SaveAndDispose();
    }


    private IEnumerable<Point> GetPoints(IPathResult r)
    {
        yield return r.Start;
        yield return r.End;
        foreach (var i in r.Elements)
        {
            yield return i.GetStartPoint();
            yield return i.GetEndPoint();
            switch (i)
            {
                case ArcDefinition arcDefinition:
                    foreach (var point in GetPoints(arcDefinition))
                        yield return point;
                    break;
                case LinePathElement: break;
                default: throw new ArgumentOutOfRangeException(nameof(i));
            }
        }

        if (_cfg.ExtraPoints != null)
        {
            var a = _cfg.ExtraPoints().ToArray();
            foreach (var i in a)
                yield return i;
        }
    }

    protected void SaveAndDispose()
    {
        var description = _cfg.Title.GetDescription();
        Graph.DrawString(description, new Font("Arial", 10), Brushes.Black, 5, 5);
        Graph.Dispose();
        SaveBitmapAndDispose();
    }

    protected void SaveBitmapAndDispose()
    {
        var fileInfo = _cfg.GetImageFile();
        fileInfo.Directory?.Create();
        Bmp.SaveIfDifferent(fileInfo.FullName);
        Bmp.Dispose();

        MarkdownTools.MakeMarkdownIndex(_cfg.OutputDirectory, TestNamesSorter.Sort);
    }


    private void ScanRange()
    {
        XRange = new MinMax();
        YRange = new MinMax();
        foreach (var i in scan())
        {
            var x = i.X;
            if (Math.Abs(x) < 1e5)
                XRange.Add(x);

            x = i.Y;
            if (Math.Abs(x) < 1e5)
                YRange.Add(x);
        }

        Grow(ref XRange);
        Grow(ref YRange);
        return;

        IEnumerable<Point> scan()
        {
            foreach (var i in GetPoints(_cfg.Result))
            {
                yield return i;
            }

            yield return _cfg.Start.Point;
            yield return _cfg.End.Point;
        }
    }

    private readonly ResultDrawerConfig _cfg;
}
