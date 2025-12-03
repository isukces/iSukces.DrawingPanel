using Xunit;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif

namespace iSukces.DrawingPanel.Paths.Test;

public class ArcPathSegmentMakerTests : TestBaseClass
{
    private static TestName MakeTitle(int testNumber, string title)
    {
        return new TestName(testNumber, "ArcCollider", title);
    }

    [Fact]
    public void T01_Should_create_line()
    {
        var ref1 = new PathRay(217.25631948011707, 79.465895927052742, 0.52655371522506067, 1.9294406404403024);
        var a = new ArcPathSegmentMaker
        {
            Flags = SegmentFlags.HasEndVector |
                    SegmentFlags.HasEndVector |
                    SegmentFlags.HasReferencePoints |
                    SegmentFlags.BothVectors,
            Validator = new MinimumValuesPathValidator(5, 0.001),
            Point = new ArcPathMakerVertex(224.32697127503582, 105.37474916092907)
                .WithInVector(0.26327685761253034, 0.9647203202201512)
                .WithReferencePoints(ref1),
            PreviousPoint = new ArcPathMakerVertex(209.76091585076492, 52.000632724428755)
                .WithInVector(0.9647203202201492, -0.26327685761253855)
                .WithOutVector(0.26327685761253034, 0.9647203202201512)
        };
        var r    = a.MakeItem();
        var code = new DpAssertsBuilder().Create(r, nameof(r));

        #region Asserts

        AssertEx.Equal(209.760915850765, 52.0006327244288, r.Start);
        AssertEx.Equal(224.326971275036, 105.374749160929, r.End);
        Assert.Single(r.Elements);
        var tmp1 = (LinePathElement)r.Elements[0];
        AssertEx.Equal(209.760915850765, 52.0006327244288, 224.326971275036, 105.374749160929, tmp1, 6);

        #endregion
    }

    [Fact]
    public void T02_Should_create_with_sharp_bend()
    {
        var ref1 = new WayPoint(new PathRay(10, 5, 1, 1.4), new Vector(1, -0.2));
        var a = new ArcPathSegmentMaker
        {
            Flags = SegmentFlags.HasReferencePoints |
                    SegmentFlags.BothVectors,
            Validator = new MinimumValuesPathValidator(5, 0.001),
            PreviousPoint = new ArcPathMakerVertex(5, 5)
                .WithInVector(0, 0)
                .WithOutVector(1, 0.2),
            Point = new ArcPathMakerVertex(17, 12)
                .WithInVector(1.3, 1)
                .WithReferencePoints(ref1),
        };
        var r = a.MakeItem();
        ResultDrawer.Draw(a, r, MakeTitle(12, "sharp bend"));

        // var code = new DpAssertsBuilder().Create(r, nameof(r));
            
        #region Asserts
        AssertEx.Equal(5, 5, r.Start);
        AssertEx.Equal(17, 12, r.End);
        Assert.Equal(3, r.Elements.Count);
        var arc = (ArcDefinition)r.Elements[0];
        Assert.Equal(ArcDirection.Clockwise, arc.Direction);
        Assert.Equal(22.6198649480404, arc.Angle, 6);
        Assert.Equal(12.747548783982, arc.Radius, 6);
        AssertEx.Equal(7.5, -7.5, arc.Center);
        AssertEx.Equal(5, 5, arc.Start);
        AssertEx.Equal(10, 5, arc.End);
        AssertEx.Equal(0.98058067569092, 0.196116135138184, arc.DirectionStart);
        arc = (ArcDefinition)r.Elements[1];
        Assert.Equal(ArcDirection.Clockwise, arc.Direction);
        Assert.Equal(16.8937301791981, arc.Angle, 6);
        Assert.Equal(29.6698912454261, arc.Radius, 6);
        AssertEx.Equal(34.1433835934669, -12.2452739953335, arc.Center);
        AssertEx.Equal(10, 5, arc.Start);
        AssertEx.Equal(16.0533316278905, 11.2717935599158, arc.End);
        AssertEx.Equal(0.581238193719096, 0.813733471206735, arc.DirectionStart);
        var line = (LinePathElement)r.Elements[2];
        AssertEx.Equal(16.0533316278905, 11.2717935599158, 17, 12, line, 6);
        #endregion


    }
}
