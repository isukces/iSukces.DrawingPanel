using System.Windows;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public class ZeroReferencePointPathCalculatorTests2
    {
        [Fact]
        public void T01_Line_OneArc_Right_27()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end   = new PathRay(new Point(33.6452056050077, 24.7273817080189), new Vector(1, 0));
            var r     = ZeroReferencePointPathCalculator.Compute(start, end, null);
            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.OneArc, r.Kind);
            Assert.Equal(ArcDirection.Clockwise, r.Arc1.Direction);
            Assert.Equal(26.565051177078, r.Arc1.Angle, 6);
            Assert.Equal(33.6452056050077, r.Arc1.Center.X, 6);
            Assert.Equal(6.97638373975955, r.Arc1.Center.Y, 6);
            Assert.Equal(25.70671797991, r.Arc1.Start.X, 6);
            Assert.Equal(22.853358989955, r.Arc1.Start.Y, 6);
            Assert.Equal(33.6452056050077, r.Arc1.End.X, 6);
            Assert.Equal(24.7273817080189, r.Arc1.End.Y, 6);
            Assert.Equal(200, r.Arc1.StartVector.X, 6);
            Assert.Equal(100, r.Arc1.StartVector.Y, 6);
            Assert.Null(r.Arc2);
        }

        [Fact]
        public void T02_OneArc_Right_27_Line()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end   = new PathRay(new Point(32.3955742033012, 3.01503610336888), new Vector(1, 0));
            var r     = ZeroReferencePointPathCalculator.Compute(start, end, null);
            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.OneArc, r.Kind);
            Assert.Equal(ArcDirection.Clockwise, r.Arc1.Direction);
            Assert.Equal(26.565051177078, r.Arc1.Angle, 6);
            Assert.Equal(-7.22810211151332, r.Arc1.Center.X, 6);
            Assert.Equal(-25.5437957769734, r.Arc1.Center.Y, 6);
            Assert.Equal(-20, r.Arc1.Start.X, 6);
            Assert.Equal(0, r.Arc1.Start.Y, 6);
            Assert.Equal(-7.22810211151332, r.Arc1.End.X, 6);
            Assert.Equal(3.01503610336888, r.Arc1.End.Y, 6);
            Assert.Equal(200, r.Arc1.StartVector.X, 6);
            Assert.Equal(100, r.Arc1.StartVector.Y, 6);
            Assert.Null(r.Arc2);
        }


        [Fact]
        public void T03_TwoArcs_Left_32_Right()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end   = new PathRay(new Point(46.7663353229257, 45.5025037613891), new Vector(1, 0));
            var r     = ZeroReferencePointPathCalculator.Compute(start, end, null);
            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.TwoArcs, r.Kind);
            Assert.Equal(ArcDirection.CounterClockwise, r.Arc1.Direction);
            Assert.Equal(32.3905141231159, r.Arc1.Angle, 6);
            Assert.Equal(-43.5791698455339, r.Arc1.Center.X, 6);
            Assert.Equal(47.1583396910677, r.Arc1.Center.Y, 6);
            Assert.Equal(-20, r.Arc1.Start.X, 6);
            Assert.Equal(0, r.Arc1.Start.Y, 6);
            Assert.Equal(1.59358273869591, r.Arc1.End.X, 6);
            Assert.Equal(19.9681084124149, r.Arc1.End.Y, 6);
            Assert.Equal(200, r.Arc1.StartVector.X, 6);
            Assert.Equal(100, r.Arc1.StartVector.Y, 6);
            Assert.Equal(ArcDirection.Clockwise, r.Arc2.Direction);
            Assert.Equal(58.9555653001939, r.Arc2.Angle, 6);
            Assert.Equal(46.7663353229257, r.Arc2.Center.X, 6);
            Assert.Equal(-7.22212286623783, r.Arc2.Center.Y, 6);
            Assert.Equal(1.59358273869591, r.Arc2.Start.X, 6);
            Assert.Equal(19.9681084124149, r.Arc2.Start.Y, 6);
            Assert.Equal(46.7663353229257, r.Arc2.End.X, 6);
            Assert.Equal(45.5025037613891, r.Arc2.End.Y, 6);
            Assert.Equal(27.1902312786528, r.Arc2.StartVector.X, 6);
            Assert.Equal(45.1727525842298, r.Arc2.StartVector.Y, 6);
        }


        [Fact]
        public void T04_TwoArcs_Right_74_Left_48()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end   = new PathRay(new Point(38.1595425371264, -16.5774819323465), new Vector(1, 0));
            var r     = ZeroReferencePointPathCalculator.Compute(start, end, null);
            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.TwoArcs, r.Kind);
            Assert.Equal(ArcDirection.Clockwise, r.Arc1.Direction);
            Assert.Equal(74.3021669144872, r.Arc1.Angle, 6);
            Assert.Equal(-6.50490416315373, r.Arc1.Center.X, 6);
            Assert.Equal(-26.9901916736926, r.Arc1.Center.Y, 6);
            Assert.Equal(-20, r.Arc1.Start.X, 6);
            Assert.Equal(0, r.Arc1.Start.Y, 6);
            Assert.Equal(15.8273191869864, r.Arc1.End.X, 6);
            Assert.Equal(-6.69586097598817, r.Arc1.End.Y, 6);
            Assert.Equal(200, r.Arc1.StartVector.X, 6);
            Assert.Equal(100, r.Arc1.StartVector.Y, 6);
            Assert.Equal(ArcDirection.CounterClockwise, r.Arc2.Direction);
            Assert.Equal(47.7371157374092, r.Arc2.Angle, 6);
            Assert.Equal(38.1595425371264, r.Arc2.Center.X, 6);
            Assert.Equal(13.5984697217162, r.Arc2.Center.Y, 6);
            Assert.Equal(15.8273191869864, r.Arc2.Start.X, 6);
            Assert.Equal(-6.69586097598817, r.Arc2.Start.Y, 6);
            Assert.Equal(38.1595425371264, r.Arc2.End.X, 6);
            Assert.Equal(-16.5774819323465, r.Arc2.End.Y, 6);
            Assert.Equal(20.2943306977044, r.Arc2.StartVector.X, 6);
            Assert.Equal(-22.3322233501401, r.Arc2.StartVector.Y, 6);
        }


        [Fact]
        public void T05_TwoArcs_Left_85_Right_203()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end = new PathRay(new Point(25.6788054191289, 30.3507230156981),
                new Vector(-1.17316474829017, -49.9862349499677));
            var r = ZeroReferencePointPathCalculator.Compute(start, end, null);
            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.TwoArcs, r.Kind);
            Assert.Equal(ArcDirection.CounterClockwise, r.Arc1.Direction);
            Assert.Equal(84.9240162298895, r.Arc1.Angle, 6);
            Assert.Equal(-28.4641809484453, r.Arc1.Center.X, 6);
            Assert.Equal(16.9283618968905, r.Arc1.Center.Y, 6);
            Assert.Equal(-20, r.Arc1.Start.X, 6);
            Assert.Equal(0, r.Arc1.Start.Y, 6);
            Assert.Equal(-10.8533245119608, r.Arc1.End.X, 6);
            Assert.Equal(23.8615812943749, r.Arc1.End.Y, 6);
            Assert.Equal(200, r.Arc1.StartVector.X, 6);
            Assert.Equal(100, r.Arc1.StartVector.Y, 6);
            Assert.Equal(ArcDirection.Clockwise, r.Arc2.Direction);
            Assert.Equal(202.833538562328, r.Arc2.Angle, 6);
            Assert.Equal(6.75753192452358, r.Arc2.Center.X, 6);
            Assert.Equal(30.7948006918592, r.Arc2.Center.Y, 6);
            Assert.Equal(-10.8533245119608, r.Arc2.Start.X, 6);
            Assert.Equal(23.8615812943749, r.Arc2.Start.Y, 6);
            Assert.Equal(25.6788054191289, r.Arc2.End.X, 6);
            Assert.Equal(30.3507230156981, r.Arc2.End.Y, 6);
            Assert.Equal(-6.93321939748436, r.Arc2.StartVector.X, 6);
            Assert.Equal(17.6108564364844, r.Arc2.StartVector.Y, 6);
        }
    }
}
