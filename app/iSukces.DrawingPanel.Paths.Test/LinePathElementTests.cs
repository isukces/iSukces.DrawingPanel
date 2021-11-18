using System.Windows;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
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
            var d    = line.DistanceFromElement(new Point(x, y), out var loc);
            Assert.Equal(dist, d, 10);
            Assert.Equal(locExpected, loc, 10);
        }

        [Theory]
        [InlineData(10, 6, 0.894427190999916, 8.4970583144992)]
        [InlineData(-10, 6, 12.3693168769, 0)]
        public void T05_Should_calculate_DistanceFromElement(double x, double y, double dist, double locExpected)
        {
            var line = new LinePathElement(new Point(2, 3), new Point(12, 8));
            var d    = line.DistanceFromElement(new Point(x, y), out var loc);
            Assert.Equal(dist, d, 10);
            Assert.Equal(locExpected, loc, 10);
        }
    }
}
