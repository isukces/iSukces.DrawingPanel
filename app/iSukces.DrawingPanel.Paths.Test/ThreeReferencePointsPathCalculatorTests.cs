using Xunit;

namespace iSukces.DrawingPanel.Paths.Test;

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
            Start      = new PathRayWithArm(-20, 0, 200, 100),
            End        = new PathRayWithArm(100, 0, -100, 100),
            Reference1 = new PathRay(40, 20, 1, 0),
            Reference2 = new PathRay(50, 20, 1, 0.1),
            Reference3 = new PathRay(70, 18, 1, 0)
        };
        var result = calc.Compute(validator);
        new ResultDrawerConfig
        {
            Start  = calc.Start,
            End    = calc.End,
            Result = result,
            Title  = MakeTitle(1, "simple"),
        }.With(calc).Draw();

        var code = new DpAssertsBuilder().Create(result, nameof(result));

        #region Asserts

        AssertEx.Equal(-20, 0, result.Start);
        AssertEx.Equal(100, 0, result.End);
        Assert.Equal(8, result.Elements.Count);
        var tmp1 = (LinePathElement)result.Elements[0];
        AssertEx.Equal(-20, 0, 2.11145618000168, 11.0557280900008, tmp1, 6);
        var tmp2 = (ArcDefinition)result.Elements[1];
        Assert.Equal(ArcDirection.Clockwise, tmp2.Direction);
        Assert.Equal(26.565051177078, tmp2.Angle, 6);
        Assert.Equal(84.7213595499958, tmp2.Radius, 6);
        AssertEx.Equal(40, -64.7213595499958, tmp2.Center);
        AssertEx.Equal(2.11145618000168, 11.0557280900008, tmp2.Start);
        AssertEx.Equal(40, 20, tmp2.End);
        AssertEx.Equal(0.894427190999916, 0.447213595499958, tmp2.DirectionStart);
        tmp2 = (ArcDefinition)result.Elements[2];
        Assert.Equal(ArcDirection.Clockwise, tmp2.Direction);
        Assert.Equal(4.03716308472048, tmp2.Angle, 6);
        Assert.Equal(41.6127931068827, tmp2.Radius, 6);
        AssertEx.Equal(40, -21.6127931068827, tmp2.Center);
        AssertEx.Equal(40, 20, tmp2.Start);
        AssertEx.Equal(42.9296861635069, 19.8967418114897, tmp2.End);
        AssertEx.Equal(1, 0, tmp2.DirectionStart);
        tmp2 = (ArcDefinition)result.Elements[3];
        Assert.Equal(ArcDirection.CounterClockwise, tmp2.Direction);
        Assert.Equal(9.74775622222014, tmp2.Angle, 6);
        Assert.Equal(41.6127931068827, tmp2.Radius, 6);
        AssertEx.Equal(45.8593723270138, 61.4062767298621, tmp2.Center);
        AssertEx.Equal(42.9296861635069, 19.8967418114897, tmp2.Start);
        AssertEx.Equal(50, 20, tmp2.End);
        AssertEx.Equal(41.5095349183724, -2.92968616350689, tmp2.DirectionStart);
        tmp2 = (ArcDefinition)result.Elements[4];
        Assert.Equal(ArcDirection.Clockwise, tmp2.Direction);
        Assert.Equal(20.4470570576543, tmp2.Angle, 6);
        Assert.Equal(32.8811801888096, tmp2.Radius, 6);
        AssertEx.Equal(53.2717997145861, -12.7179971458614, tmp2.Center);
        AssertEx.Equal(50, 20, tmp2.Start);
        AssertEx.Equal(61.6358998572931, 19.0815915214741, tmp2.End);
        AssertEx.Equal(0.995037190209989, 0.0995037190209989, tmp2.DirectionStart);
        tmp2 = (ArcDefinition)result.Elements[5];
        Assert.Equal(ArcDirection.CounterClockwise, tmp2.Direction);
        Assert.Equal(14.7364639201547, tmp2.Angle, 6);
        Assert.Equal(32.8811801888095, tmp2.Radius, 6);
        AssertEx.Equal(70, 50.8811801888096, tmp2.Center);
        AssertEx.Equal(61.6358998572931, 19.0815915214741, tmp2.Start);
        AssertEx.Equal(70, 18, tmp2.End);
        AssertEx.Equal(31.7995886673355, -8.36410014270693, tmp2.DirectionStart);
        tmp2 = (ArcDefinition)result.Elements[6];
        Assert.Equal(ArcDirection.Clockwise, tmp2.Direction);
        Assert.Equal(45, tmp2.Angle, 6);
        Assert.Equal(28.9705627484771, tmp2.Radius, 6);
        AssertEx.Equal(70, -10.9705627484771, tmp2.Center);
        AssertEx.Equal(70, 18, tmp2.Start);
        AssertEx.Equal(90.4852813742386, 9.51471862576144, tmp2.End);
        AssertEx.Equal(1, 0, tmp2.DirectionStart);
        tmp1 = (LinePathElement)result.Elements[7];
        AssertEx.Equal(90.4852813742386, 9.51471862576144, 100, 0, tmp1, 6);

        #endregion
    }


    private static readonly MinimumValuesPathValidator validator = new(5, 4);
}

//  var code = new TestMaker().Create(r, nameof(r));
