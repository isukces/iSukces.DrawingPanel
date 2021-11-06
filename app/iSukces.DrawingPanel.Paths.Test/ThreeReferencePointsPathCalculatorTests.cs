using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public class ThreeReferencePointsPathCalculatorTests
    {
        private static TestName MakeTitle(int testNumber, string title)
        {
            return new TestName(testNumber, "Three", title);
        }

        [Fact]
        public void T01_Should_create_simple()
        {
            var calc = new ThreeReferencePointsPathCalculator
            {
                Start      = new PathRay(-20, 0, 200, 100),
                End        = new PathRay(100, 0, -100, 100),
                Reference1 = new PathRay(40, 20, 1, 0),
                Reference2 = new PathRay(50, 20, 1, 0.1),
                Reference3 = new PathRay(70, 18, 1, 0)
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
            Assert.Equal(8, r.Elements.Count);
            AssertEx.Equal(-20, 0, 2.11145618000168, 11.0557280900008, (LinePathElement)r.Elements[0], 6);
            var tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(26.565051177078, tmp1.Angle, 6);
            AssertEx.Equal(40, -64.7213595499958, tmp1.Center);
            AssertEx.Equal(2.11145618000168, 11.0557280900008, tmp1.Start);
            AssertEx.Equal(40, 20, tmp1.End);
            AssertEx.Equal(200, 100, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(4.03716308472048, tmp1.Angle, 6);
            AssertEx.Equal(40, -21.6127931068827, tmp1.Center);
            AssertEx.Equal(40, 20, tmp1.Start);
            AssertEx.Equal(42.9296861635069, 19.8967418114897, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(9.74775622222014, tmp1.Angle, 6);
            AssertEx.Equal(45.8593723270138, 61.4062767298621, tmp1.Center);
            AssertEx.Equal(42.9296861635069, 19.8967418114897, tmp1.Start);
            AssertEx.Equal(50, 20, tmp1.End);
            AssertEx.Equal(41.5095349183724, -2.92968616350689, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Elements[4];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(20.4470570576543, tmp1.Angle, 6);
            AssertEx.Equal(53.2717997145861, -12.7179971458614, tmp1.Center);
            AssertEx.Equal(50, 20, tmp1.Start);
            AssertEx.Equal(61.6358998572931, 19.0815915214741, tmp1.End);
            AssertEx.Equal(0.995037190209989, 0.0995037190209989, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Elements[5];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(14.7364639201547, tmp1.Angle, 6);
            AssertEx.Equal(70, 50.8811801888096, tmp1.Center);
            AssertEx.Equal(61.6358998572931, 19.0815915214741, tmp1.Start);
            AssertEx.Equal(70, 18, tmp1.End);
            AssertEx.Equal(31.7995886673355, -8.36410014270692, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Elements[6];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45, tmp1.Angle, 6);
            AssertEx.Equal(70, -10.9705627484771, tmp1.Center);
            AssertEx.Equal(70, 18, tmp1.Start);
            AssertEx.Equal(90.4852813742386, 9.51471862576143, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.StartVector);
            AssertEx.Equal(90.4852813742386, 9.51471862576143, 100, 0, (LinePathElement)r.Elements[7], 6);

            #endregion
        }


        private static readonly MinimumValuesPathValidator validator = new(5, 4);
    }
}

//  var code = new TestMaker().Create(r, nameof(r));
