using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public class TwoReferencePointsPathCalculatorTests
    {
        private static TestName MakeTitle(int testNumber, string title)
        {
            return new TestName(testNumber, "Two", title);
        }

        [Fact]
        public void T01_Should_create_simple()
        {
            var calc = new TwoReferencePointsPathCalculator
            {
                Start      = new PathRay(-20, 0, 200, 100),
                End        = new PathRay(100, 0, -100, 100),
                Reference1 = new PathRay(40, 20, 0, 0),
                Reference2 = new PathRay(60, 20, 0, 0)
            };
            var r = calc.Compute(validator);
            new ResultDrawerConfig
            {
                Start  = calc.Start,
                End    = calc.End,
                Result = r,
                Title  = MakeTitle(1, "simple"),
            }.With(calc).Draw();

            #region Asserts

            AssertEx.Equal(-20, 0, r.Start);
            AssertEx.Equal(100, 0, r.End);
            Assert.Equal(5, r.Elements.Count);
            AssertEx.Equal(-20, 0, 2.11145618000168, 11.0557280900008, (LinePathElement)r.Elements[0], 6);
            var tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(26.565051177078, tmp1.Angle, 6);
            AssertEx.Equal(40, -64.7213595499958, tmp1.Center);
            AssertEx.Equal(2.11145618000168, 11.0557280900008, tmp1.Start);
            AssertEx.Equal(40, 20, tmp1.End);
            AssertEx.Equal(200, 100, tmp1.StartVector);
            AssertEx.Equal(40, 20, 60, 20, (LinePathElement)r.Elements[2], 6);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45, tmp1.Angle, 6);
            AssertEx.Equal(60, -28.2842712474619, tmp1.Center);
            AssertEx.Equal(60, 20, tmp1.Start);
            AssertEx.Equal(94.142135623731, 5.85786437626905, tmp1.End);
            AssertEx.Equal(20, 0, tmp1.StartVector);
            AssertEx.Equal(94.142135623731, 5.85786437626905, 100, 0, (LinePathElement)r.Elements[4], 6);

            #endregion
        }


        [Fact]
        public void T02_Should_create_left_too_close()
        {
            var calc = new TwoReferencePointsPathCalculator
            {
                Start      = new PathRay(-20, 0, 200, 100),
                End        = new PathRay(100, 0, -100, 100),
                Reference1 = new PathRay(23.02022, 21.05943, 0, 0),
                Reference2 = new PathRay(60, 20, 0, 0)
            };
            var r = calc.Compute(validator);
            new ResultDrawerConfig
            {
                Start  = calc.Start,
                End    = calc.End,
                Result = r,
                Title  = MakeTitle(2, "left to close"),
            }.With(calc).Draw();

            #region Asserts

            AssertEx.Equal(-20, 0, r.Start);
            AssertEx.Equal(100, 0, r.End);
            Assert.Equal(5, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(19.0279734107065, tmp1.Angle, 6);
            AssertEx.Equal(-39.0449960455387, 38.0899920910774, tmp1.Center);
            AssertEx.Equal(-20, 0, tmp1.Start);
            AssertEx.Equal(-8.62215750416518, 8.29049097178948, tmp1.End);
            AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(47.2340367551414, tmp1.Angle, 6);
            AssertEx.Equal(21.8006810372083, -21.5090101474984, tmp1.Center);
            AssertEx.Equal(-8.62215750416518, 8.29049097178948, tmp1.Start);
            AssertEx.Equal(23.02022, 21.05943, tmp1.End);
            AssertEx.Equal(29.7995011192879, 30.4228385413735, tmp1.StartVector);
            AssertEx.Equal(23.02022, 21.05943, 60, 20, (LinePathElement)r.Elements[2], 6);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(43.3589878326431, tmp1.Angle, 6);
            AssertEx.Equal(58.5161581129436, -31.7940274847151, tmp1.Center);
            AssertEx.Equal(60, 20, tmp1.Start);
            AssertEx.Equal(95.1550927988293, 4.84490720117068, tmp1.End);
            AssertEx.Equal(36.97978, -1.05943, tmp1.StartVector);
            AssertEx.Equal(95.1550927988293, 4.84490720117068, 100, 0, (LinePathElement)r.Elements[4], 6);

            #endregion
        }

        [Fact]
        public void T03_Should_create_left_little_out()
        {
            var calc = new TwoReferencePointsPathCalculator
            {
                Start      = new PathRay(-20, 0, 200, 100),
                End        = new PathRay(100, 0, -100, 100),
                Reference1 = new PathRay(-6, 20, 0, 0),
                Reference2 = new PathRay(60, 20, 0, 0)
            };
            var r = calc.Compute(validator);
            new ResultDrawerConfig
            {
                Start  = calc.Start,
                End    = calc.End,
                Result = r,
                Title  = MakeTitle(3, "left little out"),
            }.With(calc).Draw();

            #region Asserts

            AssertEx.Equal(-20, 0, r.Start);
            AssertEx.Equal(100, 0, r.End);
            Assert.Equal(5, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(71.8593968123999, tmp1.Angle, 6);
            AssertEx.Equal(-24.0889269128046, 8.17785382560929, tmp1.Center);
            AssertEx.Equal(-20, 0, tmp1.Start);
            AssertEx.Equal(-15.0444634564023, 9.51736764677488, tmp1.End);
            AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(98.4244479894779, tmp1.Angle, 6);
            AssertEx.Equal(-6, 10.8568814679405, tmp1.Center);
            AssertEx.Equal(-15.0444634564023, 9.51736764677488, tmp1.Start);
            AssertEx.Equal(-6, 20, tmp1.End);
            AssertEx.Equal(-1.33951382116559, 9.04446345640232, tmp1.StartVector);
            AssertEx.Equal(-6, 20, 60, 20, (LinePathElement)r.Elements[2], 6);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45, tmp1.Angle, 6);
            AssertEx.Equal(60, -28.2842712474619, tmp1.Center);
            AssertEx.Equal(60, 20, tmp1.Start);
            AssertEx.Equal(94.142135623731, 5.85786437626905, tmp1.End);
            AssertEx.Equal(66, 0, tmp1.StartVector);
            AssertEx.Equal(94.142135623731, 5.85786437626905, 100, 0, (LinePathElement)r.Elements[4], 6);

            #endregion
        }


        [Fact]
        public void T04_Should_create_big_rotation_angle()
        {
            var calc = new TwoReferencePointsPathCalculator
            {
                Start      = new PathRay(-20, 0, 200, 100),
                End        = new PathRay(100, 0, -100, 100),
                Reference1 = new PathRay(52.23347, -12.60536, 0, 0),
                Reference2 = new PathRay(17.4, 13.16829, 0, 0)
            };

            var r = calc.Compute(validator);
            new ResultDrawerConfig
            {
                Start  = calc.Start,
                End    = calc.End,
                Result = r,
                Title  = MakeTitle(4, "big rotation angle"),
            }.With(calc).Draw();

            #region Asserts

            AssertEx.Equal(-20, 0, r.Start);
            AssertEx.Equal(100, 0, r.End);
            Assert.Equal(5, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(129.041141188484, tmp1.Angle, 6);
            AssertEx.Equal(-9.21331054865222, -21.5733789026956, tmp1.Center);
            AssertEx.Equal(-20, 0, tmp1.Start);
            AssertEx.Equal(14.3369019350688, -26.7840255584071, tmp1.End);
            AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(245.977962749789, tmp1.Angle, 6);
            AssertEx.Equal(37.8871144187898, -31.9946722141186, tmp1.Center);
            AssertEx.Equal(14.3369019350688, -26.7840255584071, tmp1.Start);
            AssertEx.Equal(52.23347, -12.60536, tmp1.End);
            AssertEx.Equal(-5.2106466557115, -23.550212483721, tmp1.StartVector);
            AssertEx.Equal(52.23347, -12.60536, 17.4, 13.16829, (LinePathElement)r.Elements[2], 6);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(188.501872738383, tmp1.Angle, 6);
            AssertEx.Equal(32.0816235714691, 33.0107218723991, tmp1.Center);
            AssertEx.Equal(17.4, 13.16829, tmp1.Start);
            AssertEx.Equal(49.535450849535, 50.464549150465, tmp1.End);
            AssertEx.Equal(-34.83347, 25.77365, tmp1.StartVector);
            AssertEx.Equal(49.535450849535, 50.464549150465, 100, 0, (LinePathElement)r.Elements[4], 6);

            #endregion
        }


        private static readonly MinimumValuesPathValidator validator = new(5, 4);
    }
}

//  var code = new TestMaker().Create(r, nameof(r));
