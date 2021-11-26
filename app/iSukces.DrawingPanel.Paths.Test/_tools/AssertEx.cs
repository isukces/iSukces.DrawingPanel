using System.Windows;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal class AssertEx : Assert
    {
        public static void Equal(double x, double y, Point point, int decimalPlaces = 6)
        {
            Equal(x, point.X, decimalPlaces);
            Equal(y, point.Y, decimalPlaces);
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
}
