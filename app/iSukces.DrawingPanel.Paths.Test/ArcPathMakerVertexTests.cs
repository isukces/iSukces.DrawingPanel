using Xunit;

namespace iSukces.DrawingPanel.Paths.Test;

public class ArcPathMakerVertexTests : TestBaseClass
{
    [Fact]
    public void T01_Should_deep_clone()
    {
        var src  = new ArcPathMakerVertex(1, 2).WithReferencePoints(new PathRay(3, 1));
        var r    = src.DeepClone();
        var code = new DpAssertsBuilder().Create(r, nameof(r));

        #region Asserts
        AssertEx.Equal(1, 2, r.Location);
        AssertEx.Equal(0, 0, r.InVector);
        AssertEx.Equal(0, 0, r.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.None, r.Flags);
        Assert.Single(r.ReferencePoints);
        var ray = r.ReferencePoints[0];
            
            
        #region Asserts
        AssertEx.Equal(1, 2, r.Location);
        AssertEx.Equal(0, 0, r.InVector);
        AssertEx.Equal(0, 0, r.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.None, r.Flags);
        Assert.Single(r.ReferencePoints);
        var tmp1 = r.ReferencePoints[0];
        AssertEx.Equal(3, 1, 0, 0, 0, tmp1.OutputRay);
        Assert.False(tmp1.UseInputVector);
        AssertEx.Equal(0, 0, tmp1.InputVector);
        #endregion

        #endregion


    }
}
