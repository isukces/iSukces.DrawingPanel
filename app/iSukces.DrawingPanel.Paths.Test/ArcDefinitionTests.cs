using System;
using System.Windows;
using iSukces.Mathematics;
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
                DirectionStart = new Vector(0, yOfStart),
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
                DirectionStart = new Vector(0, yOfStart),
                End         = center + v
            };
            arc.UpdateRadiusVectors();

            var end = arc.DirectionEnd;
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
                DirectionStart = new Vector(0, yOfStart),
                End         = center + v
            };
            arc.UpdateRadiusVectors();
            Assert.Equal(expected, arc.Angle);
        }


        [Fact]
        public void T04_Should_calculate_Chord()
        {
            var center = new Point(100, 200);
            var arc = new ArcDefinition
            {
                Center      = center,
                Start       = center + new Vector(50, 0),
                DirectionStart = new Vector(0, 0),
                End         = center + new Vector(0, 50)
            };
            arc.UpdateRadiusVectors();
            Assert.Equal(70.710678118654755, arc.Chord);
        }


        [Fact]
        public void T05_Should_calculate_Sagitta()
        {
            var center = new Point(100, 200);
            var arc = new ArcDefinition
            {
                Center      = center,
                Start       = center + new Vector(50, 0),
                DirectionStart = new Vector(0, 0),
                End         = center + new Vector(0, 50)
            };
            arc.UpdateRadiusVectors();
            Assert.Equal(14.644660940672622, arc.Sagitta);
        }


        [Fact]
        public void T06_Should_calculate_Sagitta()
        {
            var center = new Point(100, 200);
            var arc = new ArcDefinition
            {
                Center      = center,
                Start       = center + new Vector(50, 0),
                DirectionStart = new Vector(0, 0),
                End         = center + new Vector(0, 50)
            };
            arc.UpdateRadiusVectors();
            Assert.Equal(14.644660940672622, arc.Sagitta);
        }


        [Theory]
        [InlineData(15, 20, 0, 0)]
        [InlineData(15 + 2, 20, 2, 0)]
        [InlineData(15 + 2, 19, 2.23606797749979, 0)]
        [InlineData(15 + 2, 21, 2.0710678118654755, 0.70948527302082)]
        [InlineData(15 + 10, 20 + 10, 13.0277563773199, 2.94001301773784)]
        [InlineData(15, 20 + 10, 6.18033988749895, 5.53574358897045)]
        [InlineData(10 - 1, 20 + 10, 5.09901951359278, ArcLength)]
        [InlineData(10 - 2, 20 + 10, 5.3851648071345, ArcLength)]
        [InlineData(-20, 20, 30.4138126514911, ArcLength)]
        [InlineData(-20, 25, 30, ArcLength)]
        public void T07_Should_calculate_DistanceFromElement(double x, double y, double dist, double locExpected)
        {
            var arc = ArcDefinition.FromCenterAndArms(
                new Point(10, 20),
                new Point(15, 20),
                new Vector(0, 1),
                new Point(10, 25));
            var point = new Point(x, y);
            var d     = arc.DistanceFromElement(point, out var loc, out var direction);
            Assert.Equal(dist, d, 10);
            Assert.Equal(locExpected, loc, 10);

            var angle = locExpected / arc.Radius;
            direction = direction.NormalizeFast().GetPrependicularVector();
            Assert.Equal(Math.Sin(angle), direction.Y, 5);
            Assert.Equal(Math.Cos(angle), direction.X, 5);
        }

        [Theory]
        [InlineData(15, 20, 0, 0)]
        [InlineData(10, -30, 45, ArcLength)]
        [InlineData(16, 20, 1, 0)]
        [InlineData(16, 21, 1.4142135623731, 0)]
        [InlineData(10, 15, 0, ArcLength)]
        [InlineData(10, 12.6, 2.4, ArcLength)]
        [InlineData(10, 17.4, 2.4, ArcLength)]
        [InlineData(9.9, 17.4, 2.40208242989286, ArcLength)]
        [InlineData(10+7, 20-7, 4.89949493661167, ArcLength/2)]
        public void T08_Should_calculate_DistanceFromElement(double x, double y, double dist, double locExpected)
        {
            var arc = ArcDefinition.FromCenterAndArms(
                new Point(10, 20),
                new Point(15, 20),
                new Vector(0, -1),
                new Point(10, 15));
            var point = new Point(x, y);
            var d     = arc.DistanceFromElement(point, out var loc, out var direction);
            Assert.Equal(dist, d, 10);
            Assert.Equal(locExpected, loc, 10);
            
            
            var angle = -locExpected / arc.Radius;
            direction = direction.NormalizeFast().GetPrependicular(true);
            Assert.Equal(Math.Sin(angle), direction.Y, 5);
            Assert.Equal(Math.Cos(angle), direction.X, 5);
        }


        private const double ArcLength = 7.853981633974483;
    }
}
