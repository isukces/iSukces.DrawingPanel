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
        private ResultDrawer(FileInfo fi, IPathResult result, PathRay start, PathRay end,
            Action<ResultDrawer> action, Func<IEnumerable<Point>> extraPoints)
        {
            _fi          = fi;
            _result      = result;
            _start       = start;
            _end         = end;
            _action      = action;
            _extraPoints = extraPoints;
        }

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
            Draw(x.Start, x.End, r, name,
                ExtraDraw, ExtraPoints, path);
        }

        public static void Draw(PathRay start, PathRay end, IPathResult r, TestName title,
            Action<ResultDrawer> action = null,
            Func<IEnumerable<Point>> extraPoints = null,
            [CallerFilePath] string path = null)
        {
            if (r is null)
                return;
            var dir = GetDir(path);
            if (dir is null)
                return;
            var name2 = title.GetFileName();
            var fi    = new FileInfo(Path.Combine(dir.FullName, name2 + ".png"));
            //if (fi.Exists) return;
            var p = new ResultDrawer(fi, r, start, end, action, extraPoints)
            {
                Title = title.GetDescription()
            };
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
            {
                Graph.FillRectangle(Brushes.White, 0, 0, Bmp.Width, Bmp.Height);
                DrawCircleWithVector(_start);
                DrawCircleWithVector(_end);

                _action?.Invoke(this);

                // var p = _start.Point;
                foreach (var element in _result.Arcs)
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
            Graph.DrawString(Title, new Font("Arial", 10), Brushes.Black, 5, 5);
            Graph.Dispose();
            _fi.Directory?.Create();
            TestExtensions.SaveIfDifferent(Bmp, _fi.FullName);

            Bmp.Dispose();
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

            if (_extraPoints != null)
            {
                var a = _extraPoints().ToArray();
                foreach (var i in a)
                    yield return i;
            }
        }


        private void ScanRange()
        {
            XRange = new MinMax();
            YRange = new MinMax();
            foreach (var i in GetPoints(_result))
            {
                XRange.Add(i.X);
                YRange.Add(i.Y);
            }

            Grow(ref XRange);
            Grow(ref YRange);
        }

        public string Title { get; set; }

        private readonly Action<ResultDrawer> _action;


        private readonly PathRay _end;
        private readonly Func<IEnumerable<Point>> _extraPoints;

        private readonly FileInfo _fi;
        private readonly IPathResult _result;
        private readonly PathRay _start;
    }
}
