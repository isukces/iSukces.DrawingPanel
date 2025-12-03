using Xunit;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;

#else
using Point=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif

namespace iSukces.DrawingPanel.Paths.Test;

public class LineEquationNotNormalizedTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1.1)]
    [InlineData(2)]
    [InlineData(5.2)]
    public void T01_Should_compute_cross_with_horizontal_line(double y)
    {
        var line = PathLineEquationNotNormalized.From2Points(1, 2, 10, 2);
        var p    = line.GetNearestPoint(new Point(3, y));
        AssertEx.Equal(3, 2, p);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(1.1)]
    [InlineData(2)]
    [InlineData(5.2)]
    public void T02_Should_compute_cross_with_vertical_line(double x)
    {
        var line = PathLineEquationNotNormalized.From2Points(7, -55, 7, 2);
        var p    = line.GetNearestPoint(new Point(x, 3));
        AssertEx.Equal(7, 3, p);
    }
        
        
    [Fact]
    public void T03_Should_compute_cross()
    {
        var line = PathLineEquationNotNormalized.From2Points(1, 2, 7, 14);
        var p    = line.GetNearestPoint(new Point(4, 3));
        AssertEx.Equal(2, 4, p);
    }
}
