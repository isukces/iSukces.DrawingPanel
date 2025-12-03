using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Windows;
using iSukces.Mathematics;
using Xunit;
using Xunit.Abstractions;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif

namespace iSukces.DrawingPanel.Paths.Test;

public partial class ArcDefinitionTests
{
    public ArcDefinitionTests(ITestOutputHelper testOutputHelper) { _testOutputHelper = testOutputHelper; }

    private static ArcDefinition Make(double xCenter, double yCenter, double radius, double startAngle,
        double sweepAngle, ArcDirection dir, out string name)
    {
        name = $"{startAngle:F0} {sweepAngle:F0}° {(dir == ArcDirection.CounterClockwise ? "ccw" : "cw")}";

        var startSinCos = MathEx.GetCosSinV(startAngle);
        var r = dir == ArcDirection.Clockwise
            ? new Vector(startSinCos.Y, -startSinCos.X)
            : new Vector(-startSinCos.Y, startSinCos.X);

        if (dir == ArcDirection.Clockwise)
            sweepAngle = -sweepAngle;

        var endAngle  = startAngle + sweepAngle;
        var endSinCos = MathEx.GetCosSinV(endAngle);

        var center = new Point(xCenter, yCenter);

        return new ArcDefinition(center, center + startSinCos * radius, r,
            center + endSinCos * radius);
    }

    private static TestName MakeTitle(int testNumber, string title)
    {
        return new TestName(testNumber, "ArcCollider", title);
    }

    private void Check(int nr, double angleMin, double angleLength, ArcDirection dir,
        Func<double, bool> checkAngle)
    {
        var          a     = Make(3.54, -7.55, 5, angleMin, angleLength, dir, out var name);
        var          title = MakeTitle(nr, name);
        const double toler = 0.2;
        DrawCircle.Draw(a, title, 0.2);
        var deltas = new[] { 0.01, 0.17, 0.19, 0.21 };

        foreach (var delta in deltas)
        {
            foreach (var plus in new[] { -delta, delta })
            {
                var radius = a.Radius + plus;
                var angle  = 0.5;
                while (angle < 360)
                {
                    var testPoint   = a.Center + radius * MathEx.GetCosSinV(angle);
                    var isCollision = a.IsLineCollision(testPoint, toler.Square(), out var distanceSquared, out var corrected);

                    var expectedResult = delta <= toler;
                    if (expectedResult)
                    {
                        if (!checkAngle(angle))
                            expectedResult = false;
                    }

                    var a1 = Math.Round(delta.Square(), 8);
                    var a2 = Math.Round(distanceSquared, 8);
                    if (a1 != a2 || expectedResult != isCollision)
                    {
                        _testOutputHelper.WriteLine($"plus={plus} angle={angle}");
                    }

                    Assert.Equal(expectedResult, isCollision);
                    Assert.Equal(delta.Square(), distanceSquared, 8);

                    if (isCollision)
                    {
                        var p = a.GetNearestPointOnCircle(testPoint);
                        AssertEx.Equal(p.X, p.Y, corrected);
                    }
                    angle++;
                }
            }
        }
    }


    [Fact]
    public void T11_Should_collide_0_90_ccw()
    {
        Check(11, 0, 90, ArcDirection.CounterClockwise, angle =>
        {
            return angle >= 0 && angle <= 90;
        });
    }

    [Fact]
    public void T12_Should_collide_30_105_ccw()
    {
        Check(12, 30, 105, ArcDirection.CounterClockwise, angle =>
        {
            return angle >= 30 && angle <= 135;
        });
    }


    [Fact]
    public void T13_Should_collide_170_200_ccw()
    {
        Check(13, 170, 200, ArcDirection.CounterClockwise, angle =>
        {
            return angle is <= 10 or >= 170;
        });
    }

    [Fact]
    public void T14_Should_collide_280_120_ccw()
    {
        Check(14, 280, 120, ArcDirection.CounterClockwise, angle =>
        {
            return angle is <= 40 or >= 280;
        });
    }


    [Fact]
    public void T21_Should_collide_0_90_cw()
    {
        Check(21, 0, 90, ArcDirection.Clockwise, angle =>
        {
            return angle >= 270;
        });
    }

    [Fact]
    public void T22_Should_collide_30_105_cw()
    {
        const double start = 30;
        const double len   = 105;
        const double end   = start - len + 360;
        Check(22, start, len, ArcDirection.Clockwise, angle =>
        {
            return angle <= start || angle >= end;
        });
    }


    [Fact]
    public void T23_Should_collide_170_200_cw()
    {
        const double start = 170;
        const double len   = 200;
        const double end   = start - len + 360;

        Check(23, start, len, ArcDirection.Clockwise, angle =>
        {
            return angle is <= start or >= end;
        });
    }

    [Fact]
    public void T24_Should_collide_280_120_ccw()
    {
        const double start = 280;
        const double len   = 120;
        const double end   = start - len;

        Check(24, start, len, ArcDirection.Clockwise, angle =>
        {
            return angle <= start && angle >= end;
        });
    }

    private readonly ITestOutputHelper _testOutputHelper;


    class DrawCircle : ResultDrawer
    {
        protected DrawCircle(ResultDrawerConfig cfg)
            : base(cfg)
        {
        }

        public static void Draw(ArcDefinition arc, TestName title, double dist,
            [CallerFilePath] string? path = null)
        {
            var cfg = new ResultDrawerConfig();

            var dir = GetDir(path, "Colliders");
            if (dir is null)
                return;
            cfg.OutputDirectory = dir;
            cfg.Title           = title;
            var self = new DrawCircle(cfg);
            self.Draw3(arc, dist);
        }

        private void Draw3(ArcDefinition arc, double dist)
        {
            const int width = 501;
            var       delta = arc.Radius + dist * 2;
            XRange = MinMax.FromCenterAndSize(arc.Center.X, delta * 2);
            YRange = MinMax.FromCenterAndSize(arc.Center.Y, delta * 2);
            Bmp    = new Bitmap(width, width, PixelFormat.Format24bppRgb);

            {
                for (int x = 0; x < Bmp.Width; x++)
                {
                    var xx = x * XRange.Length / width;
                    xx += XRange.Min;
                    for (int y = 0; y < Bmp.Height; y++)
                    {
                        var yy = (y) * YRange.Length / width;
                        yy = YRange.Max - yy;
                        var ok = arc.IsLineCollision(new Point(xx, yy), dist.Square(), out _, out _);
                        Bmp.SetPixel(x, y, ok ? Color.GreenYellow : Color.LightGray);
                    }
                }
            }
            // SaveBitmapAndDispose();
            Graph = Graphics.FromImage(Bmp);
            SaveAndDispose();
        }
    }
}
