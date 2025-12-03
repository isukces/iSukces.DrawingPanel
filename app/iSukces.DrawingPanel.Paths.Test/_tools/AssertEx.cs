using Xunit;
#if COMPATMATH
using WinPoint=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using WinPoint=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif

namespace iSukces.DrawingPanel.Paths.Test;

internal abstract class AssertEx : Assert
{
    public static void Equal(double x, double y, WinPoint point, int decimalPlaces = 6)
    {
        Equal(x, point.X, decimalPlaces);
        Equal(y, point.Y, decimalPlaces);
    }

    public static void Equal(WinPoint ex, WinPoint point, int decimalPlaces = 6)
    {
        Equal(ex.X, point.X, decimalPlaces);
        Equal(ex.Y, point.Y, decimalPlaces);
    }


    public static void Equal(double x, double y, Vector point, int decimalPlaces = 6)
    {
        Equal(x, point.X, decimalPlaces);
        Equal(y, point.Y, decimalPlaces);
    }


    public static void Equal(double x1, double y1, double x2, double y2,
        LinePathElement point, int decimalPlaces)
    {
        Equal(x1, y1, point.GetStartPoint(), decimalPlaces);
        Equal(x2, y2, point.GetEndPoint(), decimalPlaces);
    }
        
    public static void Equal(double x1, double y1, double x2, double y2,
        PathRay point, int decimalPlaces = 6)
    {
        Equal(x1, y1, point.Point, decimalPlaces);
        Equal(x2, y2, point.Vector, decimalPlaces);
    }        
    public static void Equal(double x1, double y1, double x2, double y2,
        double armLength,
        PathRayWithArm point, int decimalPlaces = 6)
    {
        Equal(x1, y1, point.Point, decimalPlaces);
        Equal(x2, y2, point.Vector, decimalPlaces);
        Assert.Equal(armLength, point.ArmLength, decimalPlaces);
    }
}
