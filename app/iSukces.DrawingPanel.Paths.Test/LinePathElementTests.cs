using Xunit;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=iSukces.Mathematics.Point;
using Vector=iSukces.Mathematics.Vector;
#endif

namespace iSukces.DrawingPanel.Paths.Test;

public class LinePathElementTests
{
    [Fact]
    public void T01_Should_collide()
    {
        var a = new LinePathElement(new Point(2, 3), new Point(12, 13));
        var c = a.IsLineCollision(new Point(7, 8), 0.1.Square(), out var distanceSquared, out var corrected);

        Assert.True(c);
        Assert.Equal(0, distanceSquared);
        AssertEx.Equal(7, 8, corrected);
    }

    [Fact]
    public void T02_Should_collide()
    {
        var a = new LinePathElement(new Point(2, 3), new Point(12, 13));
        var c = a.IsLineCollision(new Point(7 - 0.1, 8), 0.08.Square(), out var distanceSquared, out var corrected);
        Assert.True(c);
        Assert.Equal(5e-3, distanceSquared);
        AssertEx.Equal(6.95, 7.95, corrected);
    }

    [Fact]
    public void T03_Should_collide()
    {
        var a = new LinePathElement(new Point(2, 3), new Point(12, 13));
        var c = a.IsLineCollision(new Point(7 - 0.1, 8), 0.07.Square(), out var distanceSquared, out var corrected);
        Assert.False(c);
        Assert.Equal(5e-3, distanceSquared);
        AssertEx.Equal(0, 0, corrected);
    }

    [Theory]
    [InlineData(0, 6, 3.60555127546399, 0)]
    [InlineData(2, 5, 2, 0)]
    [InlineData(3, 5, 2, 1)]
    [InlineData(11, -5, 8, 9)]
    [InlineData(11, 5, 2, 9)]
    [InlineData(12, 5, 2, 10)]
    [InlineData(13, 5, 2.23606797749979, 10)]
    public void T04_Should_calculate_DistanceFromElement(double x, double y, double dist, double locExpected)
    {
        var line = new LinePathElement(new Point(2, 3), new Point(12, 3));
        var d    = line.DistanceFromElement(new Point(x, y), out var loc, out _);
        Assert.Equal(dist, d, 10);
        Assert.Equal(locExpected, loc, 10);
    }

    [Theory]
    [InlineData(10, 6, 0.894427190999916, 8.4970583144992)]
    [InlineData(-10, 6, 12.3693168769, 0)]
    public void T05_Should_calculate_DistanceFromElement(double x, double y, double dist, double locExpected)
    {
        var line = new LinePathElement(new Point(2, 3), new Point(12, 8));
        var d    = line.DistanceFromElement(new Point(x, y), out var loc, out _);
        Assert.Equal(dist, d, 10);
        Assert.Equal(locExpected, loc, 10);
    }

    [Theory]
    [InlineData(-30, 0, 2, 3)] // begin
    [InlineData(30, 0, 12, 4)] // end
    [InlineData(5, 0, 4.67326732673267, 3.26732673267327)] // middle
    [InlineData(-30, 7, 2, 3)] // begin
    [InlineData(30, 7, 12, 4)] // end
    [InlineData(5, 7, 5.36633663366337, 3.33663366336634)] // middle
    public void T06_Should_find_closest_point(double x, double y, double ex, double ey)
    {
        var line = new LinePathElement(new Point(2, 3), new Point(12, 4));
        var d    = line.FindClosestPointOnElement(new Point(x, y));
        AssertEx.Equal(ex, ey, d.ClosestPoint);
    }
}
