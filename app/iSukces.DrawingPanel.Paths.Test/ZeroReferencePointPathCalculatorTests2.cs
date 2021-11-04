using System.Windows;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public class ZeroReferencePointPathCalculatorTests2
    {
        private static TestName MakeTitle(int testNumber, string title)
        {
            return new TestName(testNumber, "Zero", title);
        }

        [Fact]
        public void T01_Line_OneArc_Right_27()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end   = new PathRay(new Point(33.6452056050077, 24.7273817080189), new Vector(1, 0));
            var r     = ZeroReferencePointPathCalculator.Compute(start, end, null);
            new ResultDrawerConfig
            {
                Start  = start,
                End    = end,
                Result = r,
                Title  = MakeTitle(1, "line+arc right 27°"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();

            #region Asserts

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.OneArc, r.Kind);
            AssertEx.Equal(-20, 0, r.Start);
            AssertEx.Equal(33.6452056050077, 24.7273817080189, r.End);
            Assert.Equal(2, r.Arcs.Count);
            AssertEx.Equal(-20, 0, 25.7067179799099, 22.853358989955, (LinePathElement)r.Arcs[0], 6);
            var tmp1 = (ArcDefinition)r.Arcs[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(26.565051177078, tmp1.Angle, 6);
            AssertEx.Equal(33.6452056050077, 6.97638373975939, tmp1.Center);
            AssertEx.Equal(25.7067179799099, 22.853358989955, tmp1.Start);
            AssertEx.Equal(33.6452056050077, 24.7273817080189, tmp1.End);
            AssertEx.Equal(200, 100, tmp1.StartVector);

            #endregion
        }

        [Fact]
        public void T02_OneArc_Right_27_Line()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end   = new PathRay(new Point(32.3955742033012, 3.01503610336888), new Vector(1, 0));
            var r     = ZeroReferencePointPathCalculator.Compute(start, end, null);
            new ResultDrawerConfig
            {
                Start  = start,
                End    = end,
                Result = r,
                Title  = MakeTitle(2, "arc right 27°+line"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();

            #region Asserts

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.OneArc, r.Kind);
            AssertEx.Equal(-20, 0, r.Start);
            AssertEx.Equal(32.3955742033012, 3.01503610336888, r.End);
            Assert.Equal(2, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(26.565051177078, tmp1.Angle, 6);
            AssertEx.Equal(-7.22810211151334, -25.5437957769733, tmp1.Center);
            AssertEx.Equal(-20, 0, tmp1.Start);
            AssertEx.Equal(-7.22810211151334, 3.01503610336888, tmp1.End);
            AssertEx.Equal(200, 100, tmp1.StartVector);
            AssertEx.Equal(-7.22810211151334, 3.01503610336888, 32.3955742033012, 3.01503610336888,
                (LinePathElement)r.Arcs[1], 6);

            #endregion
        }


        [Fact]
        public void T03_TwoArcs_Left_32_Right()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end   = new PathRay(new Point(46.7663353229257, 45.5025037613891), new Vector(1, 0));
            var r     = ZeroReferencePointPathCalculator.Compute(start, end, null);

            new ResultDrawerConfig
            {
                Start  = start,
                End    = end,
                Result = r,
                Title  = MakeTitle(3, "two arcs: left 32°+right 59°"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();

            #region Asserts

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.TwoArcs, r.Kind);
            AssertEx.Equal(-20, 0, r.Start);
            AssertEx.Equal(46.7663353229257, 45.5025037613891, r.End);
            Assert.Equal(2, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(32.3905141231159, tmp1.Angle, 6);
            AssertEx.Equal(-43.5791698455339, 47.1583396910677, tmp1.Center);
            AssertEx.Equal(-20, 0, tmp1.Start);
            AssertEx.Equal(1.59358273869592, 19.9681084124149, tmp1.End);
            AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(58.9555653001939, tmp1.Angle, 6);
            AssertEx.Equal(46.7663353229257, -7.22212286623781, tmp1.Center);
            AssertEx.Equal(1.59358273869592, 19.9681084124149, tmp1.Start);
            AssertEx.Equal(46.7663353229257, 45.5025037613891, tmp1.End);
            AssertEx.Equal(27.1902312786528, 45.1727525842298, tmp1.StartVector);

            #endregion
        }


        [Fact]
        public void T04_TwoArcs_Right_74_Left_48()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end   = new PathRay(new Point(38.1595425371264, -16.5774819323465), new Vector(1, 0));
            var r     = ZeroReferencePointPathCalculator.Compute(start, end, null);

            new ResultDrawerConfig
            {
                Start  = start,
                End    = end,
                Result = r,
                Title  = MakeTitle(4, "two arcs: right 74° + left 48°"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();

            #region Asserts

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.TwoArcs, r.Kind);
            AssertEx.Equal(-20, 0, r.Start);
            AssertEx.Equal(38.1595425371264, -16.5774819323465, r.End);
            Assert.Equal(2, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(74.3021669144873, tmp1.Angle, 6);
            AssertEx.Equal(-6.50490416315376, -26.9901916736925, tmp1.Center);
            AssertEx.Equal(-20, 0, tmp1.Start);
            AssertEx.Equal(15.8273191869863, -6.69586097598819, tmp1.End);
            AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[1];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(47.7371157374093, tmp1.Angle, 6);
            AssertEx.Equal(38.1595425371264, 13.5984697217161, tmp1.Center);
            AssertEx.Equal(15.8273191869863, -6.69586097598819, tmp1.Start);
            AssertEx.Equal(38.1595425371264, -16.5774819323465, tmp1.End);
            AssertEx.Equal(20.2943306977043, -22.3322233501401, tmp1.StartVector);

            #endregion
        }


        [Fact]
        public void T05_TwoArcs_Left_85_Right_203()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end = new PathRay(new Point(25.6788054191289, 30.3507230156981),
                new Vector(-1.17316474829017, -49.9862349499677));
            var r = ZeroReferencePointPathCalculator.Compute(start, end, null);

            new ResultDrawerConfig
            {
                Start  = start,
                End    = end,
                Result = r,
                Title  = MakeTitle(5, "two arcs: left 85° + right 203°"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();

            #region Asserts

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.TwoArcs, r.Kind);
            AssertEx.Equal(-20, 0, r.Start);
            AssertEx.Equal(25.6788054191289, 30.3507230156981, r.End);
            Assert.Equal(2, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(84.9240162298896, tmp1.Angle, 6);
            AssertEx.Equal(-28.4641809484453, 16.9283618968905, tmp1.Center);
            AssertEx.Equal(-20, 0, tmp1.Start);
            AssertEx.Equal(-10.8533245119608, 23.8615812943749, tmp1.End);
            AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(202.833538562328, tmp1.Angle, 6);
            AssertEx.Equal(6.75753192452358, 30.7948006918593, tmp1.Center);
            AssertEx.Equal(-10.8533245119608, 23.8615812943749, tmp1.Start);
            AssertEx.Equal(25.6788054191289, 30.3507230156981, tmp1.End);
            AssertEx.Equal(-6.93321939748438, 17.6108564364844, tmp1.StartVector);

            #endregion
        }
    }
}
