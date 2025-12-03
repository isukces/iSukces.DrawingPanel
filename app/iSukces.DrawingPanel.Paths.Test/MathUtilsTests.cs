using System;
using Xunit;
using Xunit.Abstractions;

namespace iSukces.DrawingPanel.Paths.Test;

public class MathUtilsTests
{
    public MathUtilsTests(ITestOutputHelper testOutputHelper) { _testOutputHelper = testOutputHelper; }


    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(180)]
    [InlineData(320)]
    [InlineData(359.99999999)]
    public void T01_ShouldNormalizeAngles(double src)
    {
        const int decimals = 10;
        var       res      = PathsMathUtils.NormalizeAngleDeg(src);
        Assert.Equal(src, res, decimals);
        res = PathsMathUtils.NormalizeAngleDeg(src + 360);
        Assert.Equal(src, res, decimals);
        res = PathsMathUtils.NormalizeAngleDeg(src + 360 * 2);
        Assert.Equal(src, res, decimals);
        res = PathsMathUtils.NormalizeAngleDeg(src - 360);
        Assert.Equal(src, res, decimals);
        res = PathsMathUtils.NormalizeAngleDeg(src - 360 * 2);
        Assert.Equal(src, res, decimals);
    }

    [Fact]
    public void T02_should_calculate_0_10()
    {
        const double min   = 0;
        const double max   = 10;
        const double check = (max + min) / 2 + 180;

        for (var i = 0; i < 360; i++)
        {
            var expected = GetExpected(i);
            var q        = PathsMathUtils.IsAngleInRegion(i, min, max);
            if (expected != q)
                _testOutputHelper.WriteLine("angle=" + i);
            Assert.Equal(expected, q);
            q = PathsMathUtils.IsAngleInRegion(i - 360, min, max);
            Assert.Equal(expected, q);
            q = PathsMathUtils.IsAngleInRegion(i + 360, min, max);
            Assert.Equal(expected, q);
        }

        Three GetExpected(int angle)
        {
            if (angle <= max)
                return Three.Inside;
            if (angle <= check)
                return Three.Above;
            return Three.Below;
        }
    }


    [Fact]
    public void T03_should_calculate_183_383()
    {
        double min   = 183;
        double max   = 183 + 200;
        double check = (max + min) / 2 + 180 - 360;

        for (var i = 0; i < 360; i++)
        {
            var expected = GetExpected(i);
            var q        = PathsMathUtils.IsAngleInRegion(i, min, max);
            if (expected != q)
                _testOutputHelper.WriteLine("angle=" + i);
            Assert.Equal(expected, q);
            q = PathsMathUtils.IsAngleInRegion(i - 360, min, max);
            Assert.Equal(expected, q);
            q = PathsMathUtils.IsAngleInRegion(i + 360, min, max);
            Assert.Equal(expected, q);
        }

        Three GetExpected(int i)
        {
            if (i <= (max - 360))
                return Three.Inside;
            if (i >= min)
                return Three.Inside;
            if (i <= check)
                return Three.Above;
            return Three.Below;
        }
    }


    [Theory]
    [InlineData(-1.4210854715202004E-14, 0)]
    [InlineData(-2.8421709430404E-14, 0)]
    [InlineData(-2.8421709430405E-14, 359.99999999999994)]
    public void T04_Should_Normalize_specific_angles(double src, double expected)
    {
        const int decimals = 10;
        var       res      = PathsMathUtils.NormalizeAngleDeg(src);
        Assert.Equal(expected, res, decimals);
    }

    private readonly ITestOutputHelper _testOutputHelper;
}
