using iSukces.DrawingPanel.Paths;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public class ZeroReferencePointPathCalculatorTests
    {
        [Fact]
        public void T01_Should_compute_point()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end   = new PathRay(0, 0, 200, 0);
            var c     = ZeroReferencePointPathCalculator.Compute(start, end);
            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.Point, c.Kind);
            Assert.Null(c.Arc1);
            Assert.Null(c.Arc2);
        }


        [Fact]
        public void T02_Should_compute_line()
        {

            var start = new PathRay(0, 0, 100, 0);
            var end   = new PathRay(200, 0, 100, 0);
            var c     = ZeroReferencePointPathCalculator.Compute(start, end);
            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.Line, c.Kind);
            Assert.Null(c.Arc1);
            Assert.Null(c.Arc2);
        }

        [Fact]
        public void T03_Should_compute_one_circle()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end   = new PathRay(200, 20, 100, 100);
            var c = ZeroReferencePointPathCalculator.Compute(start, end);
            Assert.NotNull(c.Arc1);
            Assert.Null(c.Arc2);

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.OneArc, c.Kind);
            Assert.Equal(151.7157287525381, c.Arc1.Center.X, 8);
            Assert.Equal(68.284271247461902, c.Arc1.Center.Y, 8);
            Assert.Equal(68.284271247461902, c.Arc1.RadiusStart.Length, 8);
        }

        [Fact]
        public void T04_Should_compute_one_circle()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end   = new PathRay(10, 20, 100, 100);
            var c     = ZeroReferencePointPathCalculator.Compute(start, end);
            Assert.NotNull(c.Arc1);
            Assert.NotNull(c.Arc2);
            
            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.TwoArcs, c.Kind);
            {
                var a = c.Arc1;
                Assert.Equal(0, a.Center.X, 8);
                Assert.Equal(8.4604973937154231, a.Center.Y, 8);
                Assert.Equal(0, a.RadiusStart.X, 8);
                Assert.Equal(-8.4604973937154231, a.RadiusStart.Y, 8);
            }

            {
                var a = c.Arc2;
                Assert.Equal(15.982475079307287, a.Center.X, 8);
                Assert.Equal(14.017524920692713, a.Center.Y, 8);
                Assert.Equal(-7.9912375396536435, a.RadiusStart.X, 8);
                Assert.Equal(-2.778513763488645, a.RadiusStart.Y, 8);
            }

        }
 
    }
}
