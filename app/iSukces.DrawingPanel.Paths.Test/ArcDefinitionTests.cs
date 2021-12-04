using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using iSukces.Mathematics;
using Xunit;
using Point = System.Windows.Point;

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
                Center         = center,
                Start          = center + new Vector(50, 0),
                DirectionStart = new Vector(0, yOfStart),
                End            = center + new Vector(0, 50)
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
                Center         = center,
                Start          = center + new Vector(50, 0),
                DirectionStart = new Vector(0, yOfStart),
                End            = center + v
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
                Center         = center,
                Start          = center + new Vector(50, 0),
                DirectionStart = new Vector(0, yOfStart),
                End            = center + v
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
                Center         = center,
                Start          = center + new Vector(50, 0),
                DirectionStart = new Vector(0, 0),
                End            = center + new Vector(0, 50)
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
                Center         = center,
                Start          = center + new Vector(50, 0),
                DirectionStart = new Vector(0, 0),
                End            = center + new Vector(0, 50)
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
                Center         = center,
                Start          = center + new Vector(50, 0),
                DirectionStart = new Vector(0, 0),
                End            = center + new Vector(0, 50)
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
        [InlineData(10 + 7, 20 - 7, 4.89949493661167, ArcLength / 2)]
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
            direction = direction.NormalizeFast().GetPrependicular();
            Assert.Equal(Math.Sin(angle), direction.Y, 5);
            Assert.Equal(Math.Cos(angle), direction.X, 5);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void T31_should(int checkCase)
        {
            var a = new ArcDefinition
            {
                Center         = new Point(33.426090800987062, -100.2953991565369),
                Start          = new Point(-23.082641561567051, -27.910853525241059),
                End            = new Point(4.7312421257509243, -13.063725213082051),
                RadiusEnd      = new Vector(-28.694848675236138, 87.231673943454851),
                RadiusStart    = new Vector(-56.508732362554113, 72.38454563129585),
                DirectionStart = new Vector(0.78824459418468384, 0.61536205581642989),
            };
            a.UseRadius(91.830056514585252);
            Assert.Equal(19.769618298603007, a.Angle, 10);
            if (checkCase == 0)
            {
                var result = a.DistanceFromElement(a.Start, out var dist, out var dir);
                Assert.Equal(0, dist, 10);
                Assert.Equal(0, result, 10);
            }
            else
            {
                var result = a.DistanceFromElement(a.End, out var dist, out var dir);
                Assert.Equal(31.685495529700802, dist, 10);
                Assert.Equal(0, result, 10);
            }
        }

        [Fact]
        public void T40_Should_draw_arcs_and_line()
        {
            // just make drawing for special situation
            var drawer = new ResultDrawerBase();

            var arc = ArcDefinition.FromCenterAndArms(new Point(54.8912687238947, 91.7723295871598),
                new Point(29.4845072800563, 0.552564737714022), new Vector(0.963332660388057, -0.268309868304668),
                new Point(146.242441507818, 66.8421820293499));
            var p1 = new Point(30.9478964807321, 4.29722988260286);
            var p2 = new Point(29.7659915108501, 0.0537495135934687);

            void Draw()
            {
                drawer.DrawArc(arc);
                drawer.DrawLine(new Pen(Color.Blue), p1, p2);

                /*
                drawer.DrawCircleWithVector(big.GetStartRay(), true);                
                drawer.DrawCircleWithVector(small.GetEndRay(), true);
                drawer.DrawLine(new Pen(Color.Blue), small.Center, big.Center);
                drawer.GrayCross(cc);*/
                drawer.DrawCross(arc.Start, Color.Red, 2);
                drawer.DrawCross(p2, Color.Red, 2);
                drawer.DrawCross(p1, Color.Gold, 2);
            }

            IEnumerable<Point> RangePoints()
            {
                // yield return arc.Center;    
                //yield return arc.Start;    
                yield return arc.Start;
                yield return p1;
                yield return p2;
            }

            drawer.DrawCustom("ArcDefinition T40 drawing", RangePoints, Draw);
        }


        [Theory]
        [InlineData(ArcDefinitionProperties.DirectionStart, ArcDefinition.ChangedDirectionStart)]
        [InlineData(ArcDefinitionProperties.RadiusEnd, ArcDefinition.ChangedRadiusEnd)]
        [InlineData(ArcDefinitionProperties.RadiusStart, ArcDefinition.ChangedRadiusStart)]
        [InlineData(ArcDefinitionProperties.Start, ArcDefinition.ChangedStart)]
        [InlineData(ArcDefinitionProperties.End, ArcDefinition.ChangedEnd)]
        [InlineData(ArcDefinitionProperties.Radius, ArcDefinition.ChangedRadius)]
        internal void T41_should_have_proper_flags(ArcDefinitionProperties x1, ArcDefinition.ArcFlags d)
        {
            var a = new Dictionary<ArcDefinitionProperties, ArcDefinitionProperties>
            {
                [ArcDefinitionProperties.Direction]  = ArcDefinitionProperties.RadiusStart | ArcDefinitionProperties.DirectionStart,
                [ArcDefinitionProperties.Angle]      = ArcDefinitionProperties.RadiusStart | ArcDefinitionProperties.RadiusEnd | ArcDefinitionProperties.Direction,
                [ArcDefinitionProperties.Sagitta]    = ArcDefinitionProperties.Start | ArcDefinitionProperties.End | ArcDefinitionProperties.Radius,
                [ArcDefinitionProperties.Chord]      = ArcDefinitionProperties.Start | ArcDefinitionProperties.End,
                [ArcDefinitionProperties.StartAngle] = ArcDefinitionProperties.RadiusStart,
                [ArcDefinitionProperties.EndAngle]   = ArcDefinitionProperties.RadiusEnd,
                [ArcDefinitionProperties.Radius]     = ArcDefinitionProperties.RadiusStart
            };
            var all = GetAll();

            ArcDefinitionProperties[] GetAll()
            {
                var list   = new List<ArcDefinitionProperties>();
                var number = 1;
                while (number <= 2048)
                {
                    list.Add((ArcDefinitionProperties)number);
                    number *= 2;
                }

                return list.ToArray();
            }

            ArcDefinitionProperties Dependent(ArcDefinitionProperties property, ArcDefinitionProperties starting = ArcDefinitionProperties.None)
            {
                var result = starting;
                foreach (var i in a)
                {
                    if (i.Value.HasFlag(property))
                        result |= i.Key;
                }

                while (result != starting)
                {
                    starting = result;
                    foreach (var x in all)
                        if (starting.HasFlag(x))
                            result = Dependent(x, result);
                }

                return result;
            }

            ArcDefinition.ArcFlags Convert(ArcDefinitionProperties s)
            {
                var tmp = ArcDefinition.ArcFlags.None;
                if ((s & ArcDefinitionProperties.Direction) != 0)
                {
                    tmp |= ArcDefinition.ArcFlags.HasDirection;
                    s   &= ~ArcDefinitionProperties.Direction;
                }

                if ((s & ArcDefinitionProperties.Angle) != 0)
                {
                    tmp |= ArcDefinition.ArcFlags.HasAngle;
                    s   &= ~ArcDefinitionProperties.Angle;
                }

                if ((s & ArcDefinitionProperties.Sagitta) != 0)
                {
                    tmp |= ArcDefinition.ArcFlags.HasSagitta;
                    s   &= ~ArcDefinitionProperties.Sagitta;
                }

                if ((s & ArcDefinitionProperties.Radius) != 0)
                {
                    tmp |= ArcDefinition.ArcFlags.HasRadius;
                    s   &= ~ArcDefinitionProperties.Radius;
                }

                if ((s & ArcDefinitionProperties.StartAngle) != 0)
                {
                    tmp |= ArcDefinition.ArcFlags.HasStartAngle;
                    s   &= ~ArcDefinitionProperties.StartAngle;
                }

                if ((s & ArcDefinitionProperties.EndAngle) != 0)
                {
                    tmp |= ArcDefinition.ArcFlags.HasEndAngle;
                    s   &= ~ArcDefinitionProperties.EndAngle;
                }       
                if ((s & ArcDefinitionProperties.Chord) != 0)
                {
                    tmp |= ArcDefinition.ArcFlags.HasChord;
                    s   &= ~ArcDefinitionProperties.Chord;
                }

                Assert.Equal(ArcDefinitionProperties.None, s);
                return tmp;
            }

            {
                var f        = Dependent(x1);
                var expected = Convert(f);
                var arcFlags = ~d;
                Assert.Equal(expected, arcFlags);
            }
        }


        private const double ArcLength = 7.853981633974483;

        [Flags]
        internal enum ArcDefinitionProperties
        {
            None = 0,
            Direction = 1,
            RadiusStart = 2,
            DirectionStart = 4,
            Angle = 8,
            RadiusEnd = 16,
            Sagitta = 32,
            End = 64,
            Start = 128,
            Radius = 256,
            Chord = 512,
            StartAngle = 1024,
            EndAngle = 2048
        }
    }
}
