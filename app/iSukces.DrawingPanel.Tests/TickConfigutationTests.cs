using System.Globalization;

namespace iSukces.DrawingPanel.Tests;

public class TickConfigutationTests
{
    [Fact]
    public void T01_Should_find_TickConfigutation()
    {
        var tmp      = TickConfigutation.FindForScale(25388.858450714157);
        var majorStr = tmp.Major.ToString(CultureInfo.InvariantCulture);
        Assert.Equal("0.005", majorStr);
    }
}
