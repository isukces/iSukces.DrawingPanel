#if COMPATMATH
using Point = iSukces.Mathematics.Compatibility.Point;
using Vector = iSukces.Mathematics.Compatibility.Vector;
#else
using Point = System.Windows.Point;
#endif
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.CompilerServices;
using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Paths.Test;

internal class ArcResultDrawer : ResultDrawerBase
{
    private ArcResultDrawer(ArcResultDrawerConfig cfg, ArcPathMaker maker)
    {
        _cfg   = cfg;
        _maker = maker;
    }
    
    public static void Draw(ArcResultDrawerConfig cfg, ArcPathMaker maker = null,
        [CallerFilePath] string path = null)
    {
        if (cfg.Result is null)
            return;
        var dir = GetDir(path);
        if (dir is null)
            return;
        cfg.OutputDirectory = dir;
        var p = new ArcResultDrawer(cfg, maker);
        p.DrawInternal();
    }

    internal static DirectoryInfo GetDir(string path)
    {
        if (string.IsNullOrEmpty(path))
            return null;
        var directory = new FileInfo(path).Directory;
        while (directory != null)
        {
            var h = Path.Combine(directory.FullName, ".git", "HEAD");
            if (File.Exists(h))
                return new DirectoryInfo(Path.Combine(directory.FullName, "doc", "testDrawings", "paths"));
            directory = directory.Parent;
        }

        return null;
    }

    private static IEnumerable<Point> GetPoints(ArcDefinition arc)
    {
        yield return arc.Center;
    }
    
    private static IEnumerable<Point> GetPoints(IPathResult r)
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
                case LinePathElement line:
                    yield return line.GetStartPoint();
                    yield return line.GetEndPoint();
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(i));
            }
        }
    }

    internal static void Grow(ref MinMax range)
    {
        //range.Grow(1);
        var size = range.Length + Math.Max(4, range.Length * 0.20);
        range = MinMax.FromCenterAndSize(range.Center, size);
    }

    private void Draw(ArcPathMakerVertex i)
    {
        var a = Map(i.Location);
        Graph.FillEllipse(Brushes.Crimson, a.X - Radius, a.Y - Radius, Radius * 2, Radius * 2);
        if ((i.Flags & FlexiPathMakerItem2Flags.HasInVector) != 0)
        {
            DrawArrow(i.Location, i.InVector, false);
        }

        if ((i.Flags & FlexiPathMakerItem2Flags.HasOutVector) != 0)
        {
            DrawArrow(i.Location, i.OutVector, true);
        }

        var refs = i.ReferencePoints;
        if (refs == null || refs.Count == 0) return;
        var drawArrows = refs.Count > 2;
        for (var ii = 0; ii < refs.Count; ii++)
        {
            var refPoint = refs[ii].OutputRay;
            a = Map(refPoint.Point);
            Graph.FillEllipse(Brushes.Olive, a.X - Radius, a.Y - Radius, Radius * 2, Radius * 2);
            if (drawArrows)
                DrawArrow(refPoint.Point, refPoint.Vector, true);
        }
    }

    private void DrawInternal()
    {
        if (_cfg.Title is null)
            throw new ArgumentNullException(nameof(_cfg.Title));
        ScanRange();

        CreateBitmap();

        Graph               = Graphics.FromImage(Bmp);
        Graph.SmoothingMode = SmoothingMode.HighQuality;
        {
            Graph.FillRectangle(Brushes.White, 0, 0, Bmp.Width, Bmp.Height);
            DrawCircleWithVector(_cfg.Start);
            if ((_cfg.Flags & ResultDrawerConfigFlags.ReverseEndMarker) != 0)
                DrawCircleWithVector(_cfg.End.WithInvertedVector());
            else
                DrawCircleWithVector(_cfg.End);

            _cfg.ExtraDrawingBottom?.Invoke(this);

            // var p = _start.Point;
            foreach (var s in _cfg.Result.Segments)
            foreach (var element in s.Elements)
            {
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
            }

            _cfg.ExtraDrawingTop?.Invoke(this);
        }

        if (_maker != null)
        {
            foreach (var i in _maker.Vertices)
            {
                Draw(i);
            }
        }

        var description = _cfg.Title.GetDescription();
        Graph.DrawString(description, new Font("Arial", 10), Brushes.Black, 5, 5);
        Graph.Dispose();
        var fileInfo = _cfg.GetImageFile();
        fileInfo.Directory?.Create();
        Bmp.SaveIfDifferent(fileInfo.FullName);
        Bmp.Dispose();
    }

    private IEnumerable<Point> GetPoints(ArcPathMakerResult arc)
    {
        if (_cfg.ExtraPoints != null)
        {
            var a = _cfg.ExtraPoints().ToArray();
            foreach (var i in a)
                yield return i;
        }

        foreach (var i in arc.Segments)
        {
            foreach (var q in GetPoints(i))
            {
                yield return q;
            }
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

    private readonly ArcResultDrawerConfig _cfg;
    private readonly ArcPathMaker _maker;
}
