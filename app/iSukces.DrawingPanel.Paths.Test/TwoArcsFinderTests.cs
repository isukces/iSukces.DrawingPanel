#nullable disable
using Newtonsoft.Json;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test;

public class TwoArcsFinderTests
{
        
    [Fact]
    public void T05b_Should_compute_practical_case()
    {
        const string Json = @"{
  ""StartCenterSearch"": {
    ""Vector"": ""0.25435571318907962,-0.96711073366397649"",
    ""Point"": ""48.577740528193623,43.156142115395909""
  },
  ""EndCenterSearch"": {
    ""Vector"": ""-0.25435571318907968,0.96711073366397649"",
    ""Point"": ""96.07998471088365,60.26872286057305""
  },
  ""EndDirection"": ""0.96711073366397649,0.25435571318907968"",
  ""StartDirection"": ""0.96711073366397649,0.25435571318907962""
}";

        var finder = JsonConvert.DeserializeObject<TwoArcsFinder>(Json);

        var isOk = finder.Compute(out var arc1, out var arc2, true);
        Assert.True(isOk);
        var code = new DpAssertsBuilder().Create(arc1, nameof(arc1));

        #region Asserts

        Assert.Equal(ArcDirection.CounterClockwise, arc1.Direction);
        Assert.Equal(10.1520694559206, arc1.Angle, 6);
        AssertEx.Equal(12.2901109624214, 181.128887497343, arc1.Center);
        AssertEx.Equal(48.5777405281936, 43.1561421153959, arc1.Start);
        AssertEx.Equal(72.3288626195387, 51.7124324879845, arc1.End);
        AssertEx.Equal(0.967110733663976, 0.25435571318908, arc1.DirectionStart);

        #endregion

        code = new DpAssertsBuilder().Create(arc2, nameof(arc2));

        #region Asserts

        Assert.Equal(ArcDirection.Clockwise, arc2.Direction);
        Assert.Equal(10.1520694559206, arc2.Angle, 6);
        AssertEx.Equal(132.367614276656, -77.7040225213743, arc2.Center);
        AssertEx.Equal(72.3288626195387, 51.7124324879845, arc2.Start);
        AssertEx.Equal(96.0799847108837, 60.2687228605731, arc2.End);
        AssertEx.Equal(129.416455009359, 60.0387516571173, arc2.DirectionStart);

        #endregion
    }
}
