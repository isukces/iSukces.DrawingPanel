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
    }
}