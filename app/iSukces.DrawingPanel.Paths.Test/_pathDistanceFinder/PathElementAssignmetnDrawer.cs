using System;
using System.Collections.Generic;
using System.Drawing;
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

internal sealed class PathElementAssignmetnDrawer : ResultDrawerBase
{
    public static void Draw(PathResult pathResult, string name,
        ICollection<Point>? extra_points = null,
        [CallerFilePath] string? path = null)
    {
        new PathElementAssignmetnDrawer
        {
            Kind = DrawinKind.Assign
        }.DrawInternal(pathResult, name + ", Assign", extra_points, path);
        new PathElementAssignmetnDrawer
        {
            Kind = DrawinKind.Movement
        }.DrawInternal(pathResult, name + ", Movement", extra_points, path);
    }

    private static Color FindColorFromSide(PathDistanceFinderResult a)
    {
        var mix    = a.SideMovement < 0 ? Color.Blue : Color.Red;
        var factor = Math.Abs(a.SideMovement) * 0.02;
        if (factor > 1)
            factor = 1;
        return Mix(Color.DarkGray, mix, factor);
    }

    private static Color Mix(Color baseColor, Color mix, double factor)
    {
        var r = (byte)(baseColor.R * (1 - factor) + mix.R * factor);
        var g = (byte)(baseColor.G * (1 - factor) + mix.G * factor);
        var b = (byte)(baseColor.B * (1 - factor) + mix.B * factor);
        return Color.FromArgb(r, g, b);
    }

    void DrawInternal(PathResult pathResult, string name,
        ICollection<Point>? extraPoints = null,
        [CallerFilePath] string? path = null)
    {
        _pathResult = pathResult;

        IEnumerable<Point> GetPoints1()
        {
            foreach (var i in _pathResult.Elements)
            {
                yield return i.GetEndPoint();
                yield return i.GetStartPoint();
            }
        }

        var xRange = GetPoints1().GetMinMax(a => a.X);
        var yRange = GetPoints1().GetMinMax(a => a.Y);
        xRange.Grow(20);
        yRange.Grow(20);

        IEnumerable<Point> GetPoints()
        {
            return
            [
                new(xRange.Min, yRange.Min),
                new(xRange.Max, yRange.Max)
            ];
        }

        void DrawInternal()
        {
            for (int x = 0; x < Bmp.Width; x++)
            {
                for (int y = 0; y < Bmp.Height; y++)
                {
                    var bmpPoint = new Point(x, y);
                    var p        = MapRev(bmpPoint);
                    var a        = PathDistanceFinder.GetDistanceFromLine(_pathResult, p);
                    var color =
                        Kind == DrawinKind.Assign
                            ? FindColorFromLocation(a)
                            : FindColorFromSide(a);
                    Bmp.SetPixel(x, y, color);
                }
            }

            foreach (var i in _pathResult.Elements)
            {
                switch (i)
                {
                    case ArcDefinition arcDefinition:
                        DrawArc(arcDefinition);
                        break;
                    case LinePathElement linePathElement:
                        DrawLine(new Pen(Color.Black, 3),
                            linePathElement.Start,
                            linePathElement.End);
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(i));
                }
            }

            if (extraPoints is not null)
                foreach (var i in extraPoints)
                    DrawCross(i, Color.Black, 1);

            for (int x = 0; x < Bmp.Width; x++)
            {
                for (int y = 0; y < Bmp.Height; y++)
                {
                    var bmpPoint = new Point(x, y);
                    var p        = MapRev(bmpPoint);
                    var a        = PathDistanceFinder.GetDistanceFromLine(_pathResult, p);
                    if (x % 50 == 0 && y % 50 == 0)
                    {
                        //var direction = a.Direction * 100 * Scale;
                        //var pp2       = p + direction;
                        DrawArrow(p, a.Direction, true, Color.Blue, 15, 1);
                    }
                }
            }
        }

        DrawCustom(name, GetPoints, DrawInternal, path);
    }

    private Color FindColorFromLocation(PathDistanceFinderResult result)
    {
        var color = colors[result.ElementIndex];
        if (result.Location == Three.Inside) return color;

        bool  big;
        Color mix;
        if (result.Location == Three.Below)
        {
            mix = Color.White;
            big = result.ElementIndex <= 0;
        }
        else
        {
            mix = Color.Black;
            big = result.ElementIndex == _pathResult.Elements.Count - 1;
        }

        return Mix(color, mix, big ? 0.3 : 0.05);
    }

    #region properties

    DrawinKind Kind { get; set; }

    #endregion

    #region Fields

    static readonly Color[] colors =
    [
        Color.IndianRed, Color.Gray
    ];

    private PathResult _pathResult;

    #endregion

    enum DrawinKind
    {
        Assign,
        Movement
    }
}
