#nullable disable
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test;

public class VerticesValidatorTests
{
    [Fact]
    public void T01_Should_fill_missing_vectors()
    {
        var list = new[] { new ArcPathMakerVertex(1, 2), new ArcPathMakerVertex(20, 3) };
        var r    = VerticesValidator.FillMissingVectors(list);

        var code = new DpAssertsBuilder().Create(r, nameof(r));

        #region Asserts

        Assert.Equal(2, r.Count);
        var tmp1 = r[0];
        AssertEx.Equal(1, 2, tmp1.Location);
        AssertEx.Equal(0, 0, tmp1.InVector);
        AssertEx.Equal(0.99861782933251, 0.0525588331227637, tmp1.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.HasOutVector, tmp1.Flags);
        tmp1 = r[1];
        AssertEx.Equal(20, 3, tmp1.Location);
        AssertEx.Equal(0.99861782933251, 0.0525588331227637, tmp1.InVector);
        AssertEx.Equal(0, 0, tmp1.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.HasInVector, tmp1.Flags);

        #endregion
    }


    [Fact]
    public void T02_Should_fill_missing_vectors_and_normalize_existing()
    {
        var list = new[]
        {
            new ArcPathMakerVertex(1, 2), new ArcPathMakerVertex(20, 3).WithInVector(20, 5)
        };
        var r = VerticesValidator.FillMissingVectors(list);

        var code = new DpAssertsBuilder().Create(r, nameof(r));

        #region Asserts

        Assert.Equal(2, r.Count);
        var tmp1 = r[0];
        AssertEx.Equal(1, 2, tmp1.Location);
        AssertEx.Equal(0, 0, tmp1.InVector);
        AssertEx.Equal(0.99861782933251, 0.0525588331227637, tmp1.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.HasOutVector, tmp1.Flags);
        tmp1 = r[1];
        AssertEx.Equal(20, 3, tmp1.Location);
        AssertEx.Equal(0.970142500145332, 0.242535625036333, tmp1.InVector);
        AssertEx.Equal(0, 0, tmp1.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.HasInVector, tmp1.Flags);

        #endregion
    }

    [Fact]
    public void T03_Should_fill_missing_vectors()
    {
        var list = new[]
            { new ArcPathMakerVertex(1, 2), new ArcPathMakerVertex(20, 3), new ArcPathMakerVertex(20, 42) };
        var r = VerticesValidator.FillMissingVectors(list);

        var code = new DpAssertsBuilder().Create(r, nameof(r));

        #region Asserts
        Assert.Equal(3, r.Count);
        var tmp1 = r[0];
        AssertEx.Equal(1, 2, tmp1.Location);
        AssertEx.Equal(0, 0, tmp1.InVector);
        AssertEx.Equal(0.99861782933251, 0.0525588331227637, tmp1.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.HasOutVector, tmp1.Flags);
        tmp1 = r[1];
        AssertEx.Equal(20, 3, tmp1.Location);
        AssertEx.Equal(0.99861782933251, 0.0525588331227637, tmp1.InVector);
        AssertEx.Equal(0, 1, tmp1.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.HasBothVectors, tmp1.Flags);
        tmp1 = r[2];
        AssertEx.Equal(20, 42, tmp1.Location);
        AssertEx.Equal(0, 1, tmp1.InVector);
        AssertEx.Equal(0, 0, tmp1.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.HasInVector, tmp1.Flags);
        #endregion

    }
    [Fact]
    public void T04_Should_fill_missing_vectors()
    {
        var list = new[]
            { new ArcPathMakerVertex(1, 2), new ArcPathMakerVertex(20, 2), new ArcPathMakerVertex(20, 42) };
        var r = VerticesValidator.FillMissingVectors(list);

        var code = new DpAssertsBuilder().Create(r, nameof(r));

        #region Asserts

        Assert.Equal(3, r.Count);
        var tmp1 = r[0];
        AssertEx.Equal(1, 2, tmp1.Location);
        AssertEx.Equal(0, 0, tmp1.InVector);
        AssertEx.Equal(1, 0, tmp1.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.HasOutVector, tmp1.Flags);
        tmp1 = r[1];
        AssertEx.Equal(20, 2, tmp1.Location);
        AssertEx.Equal(1, 0, tmp1.InVector);
        AssertEx.Equal(0, 1, tmp1.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.HasBothVectors, tmp1.Flags);
        tmp1 = r[2];
        AssertEx.Equal(20, 42, tmp1.Location);
        AssertEx.Equal(0, 1, tmp1.InVector);
        AssertEx.Equal(0, 0, tmp1.OutVector);
        Assert.Equal(FlexiPathMakerItem2Flags.HasInVector, tmp1.Flags);

        #endregion
    }
}
