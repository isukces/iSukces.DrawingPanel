using System.Windows;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public class ZeroReferencePointPathCalculatorTests : TestBaseClass
    {
        private static TestName MakeTitle(int testNumber, string title)
        {
            return new TestName(testNumber, "Zero", title);
        }

        [Fact]
        public void T01_Line_OneArc_Right_27()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end = new PathRay(new Point(33.6452056050077, 24.7273817080189), new Vector(1, 0));
            var r = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
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
            Assert.Equal(2, r.Elements.Count);
            AssertEx.Equal(-20, 0, 25.7067179799099, 22.853358989955, (LinePathElement)r.Elements[0], 6);
            var tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(26.565051177078, tmp1.Angle, 6);
            AssertEx.Equal(33.6452056050077, 6.97638373975939, tmp1.Center);
            AssertEx.Equal(25.7067179799099, 22.853358989955, tmp1.Start);
            AssertEx.Equal(33.6452056050077, 24.7273817080189, tmp1.End);
            AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp1.DirectionStart);

            #endregion
        }

        [Fact]
        public void T02_OneArc_Right_27_Line()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end = new PathRay(new Point(32.3955742033012, 3.01503610336888), new Vector(1, 0));
            var r = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
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
            Assert.Equal(2, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(26.565051177078, tmp1.Angle, 6);
            AssertEx.Equal(-7.22810211151334, -25.5437957769733, tmp1.Center);
            AssertEx.Equal(-20, 0, tmp1.Start);
            AssertEx.Equal(-7.22810211151334, 3.01503610336888, tmp1.End);
            AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp1.DirectionStart);
            AssertEx.Equal(-7.22810211151334, 3.01503610336888, 32.3955742033012, 3.01503610336888,
                (LinePathElement)r.Elements[1], 6);

            #endregion
        }
        
        [Fact]
        public void T03_TwoArcs_Left_32_Right()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end = new PathRay(new Point(46.7663353229257, 45.5025037613891), new Vector(1, 0));
            var r = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);

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
            Assert.Equal(2, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(32.3905141231159, tmp1.Angle, 6);
            AssertEx.Equal(-43.5791698455339, 47.1583396910677, tmp1.Center);
            AssertEx.Equal(-20, 0, tmp1.Start);
            AssertEx.Equal(1.59358273869592, 19.9681084124149, tmp1.End);
            AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(58.9555653001939, tmp1.Angle, 6);
            AssertEx.Equal(46.7663353229257, -7.22212286623781, tmp1.Center);
            AssertEx.Equal(1.59358273869592, 19.9681084124149, tmp1.Start);
            AssertEx.Equal(46.7663353229257, 45.5025037613891, tmp1.End);
            AssertEx.Equal(27.1902312786528, 45.1727525842298, tmp1.DirectionStart);

            #endregion
        }
        
        [Fact]
        public void T04_TwoArcs_Right_74_Left_48()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end = new PathRay(new Point(38.1595425371264, -16.5774819323465), new Vector(1, 0));
            var r = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);

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
            Assert.Equal(2, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(74.3021669144873, tmp1.Angle, 6);
            AssertEx.Equal(-6.50490416315376, -26.9901916736925, tmp1.Center);
            AssertEx.Equal(-20, 0, tmp1.Start);
            AssertEx.Equal(15.8273191869863, -6.69586097598819, tmp1.End);
            AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(47.7371157374093, tmp1.Angle, 6);
            AssertEx.Equal(38.1595425371264, 13.5984697217161, tmp1.Center);
            AssertEx.Equal(15.8273191869863, -6.69586097598819, tmp1.Start);
            AssertEx.Equal(38.1595425371264, -16.5774819323465, tmp1.End);
            AssertEx.Equal(20.2943306977043, -22.3322233501401, tmp1.DirectionStart);

            #endregion
        }
        
        [Fact]
        public void T05_TwoArcs_Left_85_Right_203()
        {
            var start = new PathRay(new Point(-20, 0), new Vector(200, 100));
            var end = new PathRay(new Point(25.6788054191289, 30.3507230156981),
                new Vector(-1.17316474829017, -49.9862349499677));
            var r = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);

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
            Assert.Equal(2, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(84.9240162298896, tmp1.Angle, 6);
            AssertEx.Equal(-28.4641809484453, 16.9283618968905, tmp1.Center);
            AssertEx.Equal(-20, 0, tmp1.Start);
            AssertEx.Equal(-10.8533245119608, 23.8615812943749, tmp1.End);
            AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(202.833538562328, tmp1.Angle, 6);
            AssertEx.Equal(6.75753192452358, 30.7948006918593, tmp1.Center);
            AssertEx.Equal(-10.8533245119608, 23.8615812943749, tmp1.Start);
            AssertEx.Equal(25.6788054191289, 30.3507230156981, tmp1.End);
            AssertEx.Equal(-6.93321939748438, 17.6108564364844, tmp1.DirectionStart);

            #endregion
        }

        [Fact]
        public void T06_Should_compute_line_from_very_flat_arc()
        {
            var start = new PathRay(48.5777405281936, 43.1561421153959, 0.967110733663976, 0.25435571318908);
            var end   = new PathRay(147.16164261446, 69.084278213229, 0.967110733663976, 0.25435571318908);

            var result =
                (ZeroReferencePointPathCalculatorLineResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            AssertEx.Equal(48.5777405281936, 43.1561421153959, result.Start);
            AssertEx.Equal(147.16164261446, 69.084278213229, result.End);
            Assert.Single(result.Elements);
            var tmp1 = (LinePathElement)result.Elements[0];
            AssertEx.Equal(48.5777405281936, 43.1561421153959, 147.16164261446, 69.084278213229, tmp1, 6);

            #endregion
        }


        [Fact]
        public void T07_Should_compute_point()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end = new PathRay(0, 0, 200, 0);
            var c = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.Point, c.Kind);
            Assert.Null(c.Arc1);
            Assert.Null(c.Arc2);
        }


        [Fact]
        public void T08_Should_compute_line()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end   = new PathRay(200, 0, 100, 0);
            var c = (ZeroReferencePointPathCalculatorLineResult)ZeroReferencePointPathCalculator.Compute(start, end,
                null);
        }

        [Fact]
        public void T09_Should_compute_one_circle()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end = new PathRay(200, 20, 100, 100);
            var c = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
            Assert.NotNull(c.Arc1);
            Assert.Null(c.Arc2);

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.OneArc, c.Kind);
            Assert.Equal(151.7157287525381, c.Arc1.Center.X, 8);
            Assert.Equal(68.284271247461902, c.Arc1.Center.Y, 8);
            Assert.Equal(68.284271247461902, c.Arc1.RadiusStart.Length, 8);
        }

        [Fact]
        public void T10_Should_compute_one_circle()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end = new PathRay(10, 20, 100, 100);
            var c = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
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


        [Fact]
        public void T99a_Should_compute_practical_case()
        {
            PathRay start = new PathRay(48.426398878846122,
                43.73157300192598,
                0.96711073366397649,
                0.25435571318907962);

            PathRay end = new PathRay(110.61711161799218,
                49.932980045672636,
                98.73808832761253,
                25.968687970556957);

            var result =
                (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);

            new ResultDrawerConfig
            {
                Start  = start,
                End    = end,
                Result = result,
                Title  = MakeTitle(1, "practical case A"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();

            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.TwoArcs, result.Kind);
            AssertEx.Equal(48.4263988788461, 43.731573001926, result.Start);
            AssertEx.Equal(110.617111617992, 49.9329800456726, result.End);
            Assert.Equal(2, result.Elements.Count);
            var arc = (ArcDefinition)result.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(18.0818631776401, arc.Angle, 6);
            AssertEx.Equal(73.7175574338968, -52.4304144711244, arc.Center);
            AssertEx.Equal(48.4263988788461, 43.731573001926, arc.Start);
            AssertEx.Equal(79.5217552484192, 46.8322765237993, arc.End);
            AssertEx.Equal(0.967110733663976, 0.25435571318908, arc.DirectionStart);
            arc = (ArcDefinition)result.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(18.0818631776401, arc.Angle, 6);
            AssertEx.Equal(85.3259530629415, 146.094967518723, arc.Center);
            AssertEx.Equal(79.5217552484192, 46.8322765237993, arc.Start);
            AssertEx.Equal(110.617111617992, 49.9329800456726, arc.End);
            AssertEx.Equal(99.2626909949237, -5.80419781452231, arc.DirectionStart);

            #endregion
        }

        [Fact]
        public void T99b_Should_compute_practical_case()
        {
            PathRay start = new PathRay(92.406079312945906, 78.164430288334614, 26.878532429961201,
                0.41011077915496202);

            PathRay end = new PathRay(112.53719286991523, 54.715165044463063, 10.3046717663197, -12.015567941507101);

            var result =
                (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);

            new ResultDrawerConfig
            {
                Start  = start,
                End    = end,
                Result = result,
                Title  = MakeTitle(1, "practical case B"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();

            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.OneArc, result.Kind);
            AssertEx.Equal(92.4060793129459, 78.1644302883346, result.Start);
            AssertEx.Equal(112.537192869915, 54.7151650444631, result.End);
            Assert.Equal(2, result.Elements.Count);
            var tmp1 = (ArcDefinition)result.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(50.2574251944742, tmp1.Angle, 6);
            AssertEx.Equal(92.4067468110961, 78.1206826679612, tmp1.Center);
            AssertEx.Equal(92.4060793129459, 78.1644302883346, tmp1.Start);
            AssertEx.Equal(92.4399586788274, 78.1491654993499, tmp1.End);
            AssertEx.Equal(0.999883618099045, 0.0152561547305979, tmp1.DirectionStart);
            var tmp2 = (LinePathElement)result.Elements[1];
            AssertEx.Equal(92.4399586788274, 78.1491654993499, 112.537192869915, 54.7151650444631, tmp2, 6);

            #endregion
        }

        [Fact]
        public void T99c_Should_compute_practical_case()
        {
            PathRay start = new PathRay(92.406079312945906, 78.164430288334614, 26.878532429961201,
                0.41011077915496202);

            PathRay end = new PathRay(112.53719286991523, 54.715165044463063, 10.3046717663197, -12.015567941507101);

            var result =
                (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(
                    start, end, new MinimumValuesPathValidator(3, 0.001));

            new ResultDrawerConfig
            {
                Start  = start,
                End    = end,
                Result = result,
                Title  = MakeTitle(1, "practical case C"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();

            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.TwoArcs, result.Kind);
            AssertEx.Equal(92.4060793129459, 78.1644302883346, result.Start);
            AssertEx.Equal(112.537192869915, 54.7151650444631, result.End);
            Assert.Equal(2, result.Elements.Count);
            var tmp1 = (ArcDefinition)result.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(85.1568008940344, tmp1.Angle, 6);
            AssertEx.Equal(92.6525221347156, 62.0126446885604, tmp1.Center);
            AssertEx.Equal(92.4060793129459, 78.1644302883346, tmp1.Start);
            AssertEx.Equal(108.725830597366, 63.6218889801372, tmp1.End);
            AssertEx.Equal(0.999883618099045, 0.0152561547305979, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)result.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(34.8993756995524, tmp1.Angle, 6);
            AssertEx.Equal(124.799139060016, 65.2311332717139, tmp1.Center);
            AssertEx.Equal(108.725830597366, 63.6218889801372, tmp1.Start);
            AssertEx.Equal(112.537192869915, 54.7151650444631, tmp1.End);
            AssertEx.Equal(1.60924429157672, -16.0733084626502, tmp1.DirectionStart);

            #endregion
        }

        [Fact]
        public void T99d_Should_compute_practical_case()
        {
            PathRay start = new PathRay(48.577740528193623,
                43.156142115395909,
                0.96711073366397649,
                0.25435571318907962);

            PathRay end = new PathRay(60.253556209091755,
                48.782956081365825,
                0.96710516056661378,
                0.25437690226399112);

            var result =
                (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(
                    start, end, new MinimumValuesPathValidator(3, 0.001));

            new ResultDrawerConfig
            {
                Start  = start,
                End    = end,
                Result = result,
                Title  = MakeTitle(1, "practical case D"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();

            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.TwoArcs, result.Kind);
            AssertEx.Equal(48.5777405281936, 43.1561421153959, result.Start);
            AssertEx.Equal(60.2535562090918, 48.7829560813658, result.End);
            Assert.Equal(2, result.Elements.Count);
            var arc = (ArcDefinition)result.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(21.9892812623252, arc.Angle, 6);
            Assert.Equal(16.9902048760993, arc.Radius, 6);
            AssertEx.Equal(44.2561848497048, 59.5875516182215, arc.Center);
            AssertEx.Equal(48.5777405281936, 43.1561421153959, arc.Start);
            AssertEx.Equal(54.4158283720046, 45.9695964424139, arc.End);
            AssertEx.Equal(0.967110733663976, 0.25435571318908, arc.DirectionStart);
            arc = (ArcDefinition)result.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(21.988025927212, arc.Angle, 6);
            Assert.Equal(16.9902048154375, arc.Radius, 6);
            AssertEx.Equal(64.5754718943044, 32.3516412666062, arc.Center);
            AssertEx.Equal(54.4158283720046, 45.9695964424139, arc.Start);
            AssertEx.Equal(60.2535562090918, 48.7829560813658, arc.End);
            AssertEx.Equal(13.6179551758077, 10.1596435222998, arc.DirectionStart);

            #endregion
        }
    }
}
