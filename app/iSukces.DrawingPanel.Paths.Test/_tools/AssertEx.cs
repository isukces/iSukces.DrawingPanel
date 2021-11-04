using System.Windows;
using iSukces.DrawingPanel.Paths;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal class AssertEx : Assert
    {
        public static void Equal(double x, double y, Point point, int decimalPlaces=6)
        {
            Equal(x, point.X, decimalPlaces);
            Equal(y, point.Y, decimalPlaces);
        }

        
        public static void Equal(double x, double y, Vector point, int decimalPlaces=6)
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
    }
}
