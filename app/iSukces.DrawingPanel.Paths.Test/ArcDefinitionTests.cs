using System.Windows;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public partial class ArcDefinitionTests
    {
        [Theory]
        [InlineData(1, ArcDirection.CounterClockwise)] // MathematicalPlus__
        [InlineData(-1, ArcDirection.Clockwise)] // MathematicalMinus__
        public void T01_Should_find_direction(double yOfStart, ArcDirection expected)
        {
            var center = new Point(100, 200);
            var arc = new ArcDefinition
            {
                Center      = center,
                Start       = center + new Vector(50, 0),
                StartVector = new Vector(0, yOfStart),
                End         = center + new Vector(0, 50)
            };
            arc.UpdateRadiusVectors();
            Assert.Equal(expected, arc.Direction);
        }


        [Theory]
        [InlineData(1, 50, 0, 0, 50)]
        [InlineData(-1, 50, 0, 0, -50)]
        [InlineData(1, -50, 0, 0, -50)]
        [InlineData(-1, -50, 0, 0, 50)]
        [InlineData(1, 0, 50, -50, 0)]
        public void T02_Should_find_end_vector(double yOfStart, double x, double y, double expectedX,
            double expectedY)
        {
            var v = new Vector(x, y);
            v.Normalize();
            v *= 50;
            var center = new Point(100, 200);
            var arc = new ArcDefinition
            {
                Center      = center,
                Start       = center + new Vector(50, 0),
                StartVector = new Vector(0, yOfStart),
                End         = center + v
            };
            arc.UpdateRadiusVectors();

            var end = arc.EndVector;
            Assert.Equal(expectedX, end.X, 6);
            Assert.Equal(expectedY, end.Y, 6);
        }


        [Theory]
        [InlineData(1, 50, 0, 0)]
        [InlineData(1, 50, 50, 45)]
        [InlineData(1, 0, 50, 90)]
        [InlineData(1, -50, 50, 135)]
        [InlineData(1, -50, 0, 180)]
        [InlineData(1, -50, -50, 225)]
        [InlineData(1, 0, -50, 270)]
        [InlineData(1, 50, -50, 315)]
        [InlineData(-1, 50, 0, 0)]
        [InlineData(-1, 50, 50, 315)]
        [InlineData(-1, 0, 50, 270)]
        [InlineData(-1, -50, 50, 225)]
        [InlineData(-1, -50, 0, 180)]
        [InlineData(-1, -50, -50, 135)]
        [InlineData(-1, 0, -50, 90)]
        [InlineData(-1, 50, -50, 45)]
        public void T03_Should_find_angle(double yOfStart, double x, double y, double expected)
        {
            var v = new Vector(x, y);
            v.Normalize();
            v *= 50;
            var center = new Point(100, 200);
            var arc = new ArcDefinition
            {
                Center      = center,
                Start       = center + new Vector(50, 0),
                StartVector = new Vector(0, yOfStart),
                End         = center + v
            };
            arc.UpdateRadiusVectors();
            Assert.Equal(expected, arc.Angle);
        }

    }
}
