using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using iSukces.Mathematics;
using Point = System.Windows.Point;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal class ResultDrawer : ResultDrawerBase
    {
        private ResultDrawer(ResultDrawerConfig cfg) { _cfg = cfg; }


        public static void Draw(OneReferencePointPathCalculator x, IPathResult r, TestName name,
            [CallerFilePath] string path = null)
        {
            void ExtraDraw(ResultDrawer g) { g.GrayCross(x.Reference.Point); }

            IEnumerable<Point> ExtraPoints()
            {
                yield return x.Start.Point;
                yield return x.End.Point;
                yield return x.Reference.Point;
            }

            // ReSharper disable once ExplicitCallerInfoArgument
            Draw(new ResultDrawerConfig
            {
                Start        = x.Start,
                End          = x.End,
                Result       = r,
                Title        = name,
                ExtraPoints  = ExtraPoints,
                ExtraDrawing = ExtraDraw
            }, path);
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

        private static DirectoryInfo GetDir(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            var directory = new FileInfo(path).Directory;
            while (directory != null)
            {
                var h = Path.Combine(directory.FullName, ".git", "HEAD");
                if (File.Exists(h))
                    return new DirectoryInfo(Path.Combine(directory.FullName, "doc", "testDrawings"));
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

            Graph = Graphics.FromImage(Bmp);
            Graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            {
                Graph.FillRectangle(Brushes.White, 0, 0, Bmp.Width, Bmp.Height);
                DrawCircleWithVector(_cfg.Start);
                if ((_cfg.Flags & ResultDrawerConfigFlags.ReverseEndMarker) != 0)
                    DrawCircleWithVector(_cfg.End.WithInvertedVector());
                else
                    DrawCircleWithVector(_cfg.End);

                _cfg.ExtraDrawing?.Invoke(this);

                // var p = _start.Point;
                foreach (var element in _cfg.Result.Arcs)
                {
                    // DrawLine(p, element.GetStartPoint(), new Pen(Color.Gainsboro, 1));

                    switch (element)
                    {
                        case ArcDefinition arcDefinition:
                            DrawArc(arcDefinition);
                            // throw new NotImplementedException("Drawer");
                            break;
                        case InvalidPathElement invalidPathElement:
                            DrawLine(element.GetStartPoint(), element.GetEndPoint(), new Pen(Color.Red, 1));
                            break;
                        case LinePathElement linePathElement:
                            DrawLine(element.GetStartPoint(), element.GetEndPoint(), new Pen(Color.Blue, 3));
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(element));
                    }

                    // p = element.GetEndPoint();
                }
                // DrawLine(_result.Start, _result.End, new Pen(Color.Crimson, 2));
            }
            var description = _cfg.Title.GetDescription();
            Graph.DrawString(description, new Font("Arial", 10), Brushes.Black, 5, 5);
            Graph.Dispose();
            var fileInfo = _cfg.GetImageFile();
            fileInfo.Directory?.Create();
            Bmp.SaveIfDifferent(fileInfo.FullName);
            Bmp.Dispose();

            MarkdownTools.MakeMarkdownIndex(_cfg.OutputDirectory, TestNamesSorter.Sort);
        }


        private IEnumerable<Point> GetPoints(IPathResult r)
        {
            yield return r.Start;
            yield return r.End;
            foreach (var i in r.Arcs)
            {
                yield return i.GetStartPoint();
                yield return i.GetEndPoint();
                switch (i)
                {
                    case ArcDefinition arcDefinition:
                        foreach (var point in GetPoints(arcDefinition))
                            yield return point;
                        break;
                    case InvalidPathElement:
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


        private void ScanRange()
        {
            XRange = new MinMax();
            YRange = new MinMax();
            foreach (var i in GetPoints(_cfg.Result))
            {
                XRange.Add(i.X);
                YRange.Add(i.Y);
            }

            Grow(ref XRange);
            Grow(ref YRange);
        }

        private readonly ResultDrawerConfig _cfg;
    }
}
