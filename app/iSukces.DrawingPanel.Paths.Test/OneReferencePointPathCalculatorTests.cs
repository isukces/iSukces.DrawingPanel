using System;
using Xunit;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif

namespace iSukces.DrawingPanel.Paths.Test
{
    public class OneReferencePointPathCalculatorTests : TestBaseClass
    {
        private static TestName MakeTitle(int testNumber, string title)
        {
            return new TestName(testNumber, "One", title);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(Epsilon)]
        [InlineData(12.4)]
        [InlineData(50 - Epsilon)]
        [InlineData(50)]
        [InlineData(76)]
        public void T01_Should_create_colinear(double x)
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRayWithArm(0, 0, 13, 0),
                End       = new PathRayWithArm(50, 0, -11, 0),
                Reference = new PathRay(x, 0, 0, 0)
            };
            var r = a.Compute(null);
            if (Math.Abs(x - 12.4) < 0.0001)
                ResultDrawer.Draw(a, r, MakeTitle(1, "colinear"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(50, 0, r.End);
            Assert.Single(r.Elements);
            AssertEx.Equal(0, 0, 50, 0, (LinePathElement)r.Elements[0], 6);

            #endregion
        }

        [Fact]
        public void T02_Should_create_colinear_not_line()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRayWithArm(0, 0, 13, 0),
                End       = new PathRay(50, 0, -11, 0),
                Reference = new PathRay(18, 8, 0, 0)
            };
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(2, "colinear"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(50, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(47.9249779491564, tmp1.Angle, 6);
            AssertEx.Equal(0, 12.125, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(9, 4, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(47.9249779491564, tmp1.Angle, 6);
            AssertEx.Equal(18, -4.125, tmp1.Center);
            AssertEx.Equal(9, 4, tmp1.Start);
            AssertEx.Equal(18, 8, tmp1.End);
            AssertEx.Equal(8.125, 9, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(28.072486935853, tmp1.Angle, 6);
            AssertEx.Equal(18, -26, tmp1.Center);
            AssertEx.Equal(18, 8, tmp1.Start);
            AssertEx.Equal(34, 4, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(28.072486935853, tmp1.Angle, 6);
            AssertEx.Equal(50, 34, tmp1.Center);
            AssertEx.Equal(34, 4, tmp1.Start);
            AssertEx.Equal(50, 0, tmp1.End);
            AssertEx.Equal(30, -16, tmp1.DirectionStart);

            #endregion
        }

        [Fact]
        public void T03_Should_create_too_small_radius_2()
        {
            const double x  = 2;
            const double dx = 6;
            const double dy = 4;
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, dx, dy),
                End       = new PathRay(30, 0, -1, 1),
                Reference = new PathRay(x, 4, 0, 0)
            };
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(3, "too small radius, x=" + x));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(78.6180267593746, tmp1.Angle, 6);
            AssertEx.Equal(-0.856275169326163, 1.28441275398924, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(0.571862415336919, 1.8703703697666, tmp1.End);
            AssertEx.Equal(0.832050294337844, 0.554700196225229, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(112.308094285354, tmp1.Angle, 6);
            AssertEx.Equal(2, 2.45632798554396, tmp1.Center);
            AssertEx.Equal(0.571862415336919, 1.8703703697666, tmp1.Start);
            AssertEx.Equal(2, 4, tmp1.End);
            AssertEx.Equal(-0.585957615777356, 1.42813758466308, tmp1.DirectionStart);
            AssertEx.Equal(2, 4, 20.3431457505076, 4, (LinePathElement)r.Elements[2], 6);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45, tmp1.Angle, 6);
            AssertEx.Equal(20.3431457505076, -9.65685424949238, tmp1.Center);
            AssertEx.Equal(20.3431457505076, 4, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);

            #endregion
        }

        [Fact]
        public void T03_Should_create_too_small_radius_26()
        {
            const double x  = 26;
            const double dx = 6;
            const double dy = 4;
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, dx, dy),
                End       = new PathRay(30, 0, -1, 1),
                Reference = new PathRay(x, 4, 0, 0)
            };
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(3, "too small radius, x=" + x));
            var code = new DpAssertsBuilder().Create(r, nameof(r));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(33.6900675259798, tmp1.Angle, 6);
            AssertEx.Equal(13.211102550928, -19.816653826392, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(13.211102550928, 4, tmp1.End);
            AssertEx.Equal(0.832050294337844, 0.554700196225229, tmp1.DirectionStart);
            var tmp2 = (LinePathElement)r.Elements[1];
            AssertEx.Equal(13.211102550928, 4, 26, 4, tmp2, 6);
            tmp1 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(76.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(26, 0.765881025674692, tmp1.Center);
            AssertEx.Equal(26, 4, tmp1.Start);
            AssertEx.Equal(29.1434337289548, 1.5263742417921, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(31.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(32.2868674579095, 2.28686745790951, tmp1.Center);
            AssertEx.Equal(29.1434337289548, 1.5263742417921, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(0.760493216117408, -3.14343372895475, tmp1.DirectionStart);

            #endregion
        }

        [Fact]
        public void T03_Should_create_too_small_radius_29()
        {
            const double x  = 29;
            const double dx = 6;
            const double dy = 4;
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, dx, dy),
                End       = new PathRay(30, 0, -1, 1),
                Reference = new PathRay(x, 4, 0, 0)
            };
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(3, "too small radius, x=" + x));
            var code = new DpAssertsBuilder().Create(r, nameof(r));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(33.6900675259798, tmp1.Angle, 6);
            AssertEx.Equal(13.211102550928, -19.816653826392, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(13.211102550928, 4, tmp1.End);
            AssertEx.Equal(0.832050294337844, 0.554700196225229, tmp1.DirectionStart);
            var tmp2 = (LinePathElement)r.Elements[1];
            AssertEx.Equal(13.211102550928, 4, 29, 4, tmp2, 6);
            tmp1 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(132.595764207088, tmp1.Angle, 6);
            AssertEx.Equal(29, 2.69313071723601, tmp1.Center);
            AssertEx.Equal(29, 4, tmp1.Start);
            AssertEx.Equal(29.9620480659834, 1.80861342460141, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(87.5957642070876, tmp1.Angle, 6);
            AssertEx.Equal(30.9240961319668, 0.924096131966815, tmp1.Center);
            AssertEx.Equal(29.9620480659834, 1.80861342460141, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(-0.884517292634599, -0.96204806598341, tmp1.DirectionStart);

            #endregion
        }

        [Fact]
        public void T03_Should_create_too_small_radius_6()
        {
            const double x  = 6;
            const double dx = 6;
            const double dy = 4;
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, dx, dy),
                End       = new PathRay(30, 0, -1, 1),
                Reference = new PathRay(x, 4, 0, 0)
            };
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(3, "too small radius, x=" + x));
            var code = new DpAssertsBuilder().Create(r, nameof(r));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(23.6482664310892, tmp1.Angle, 6);
            AssertEx.Equal(-2.9478049570633, 4.42170743559494, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(1.52609752146835, 1.55373823710781, tmp1.End);
            AssertEx.Equal(0.832050294337844, 0.554700196225229, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(57.3383339570689, tmp1.Angle, 6);
            AssertEx.Equal(6, -1.31423096137932, tmp1.Center);
            AssertEx.Equal(1.52609752146835, 1.55373823710781, tmp1.Start);
            AssertEx.Equal(6, 4, tmp1.End);
            AssertEx.Equal(2.86796919848713, 4.47390247853165, tmp1.DirectionStart);
            var tmp2 = (LinePathElement)r.Elements[2];
            AssertEx.Equal(6, 4, 20.3431457505076, 4, tmp2, 6);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45, tmp1.Angle, 6);
            AssertEx.Equal(20.3431457505076, -9.65685424949238, tmp1.Center);
            AssertEx.Equal(20.3431457505076, 4, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);

            #endregion
        }


        [Fact]
        public void T04_Should_create_normal_configuration()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, 1, 1),
                End       = new PathRay(50, 0, -2, 1),
                Reference = new PathRay(13, 1, 0, 0)
            };
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(4, "normal config"));
            var code = new DpAssertsBuilder().Create(r, nameof(r));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(50, 0, r.End);
            Assert.Equal(3, r.Elements.Count);
            var arc = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(45, arc.Angle, 6);
            Assert.Equal(3.41421356237309, arc.Radius, 6);
            AssertEx.Equal(2.41421356237309, -2.41421356237309, arc.Center);
            AssertEx.Equal(0, 0, arc.Start);
            AssertEx.Equal(2.41421356237309, 1, arc.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, arc.DirectionStart);
            var line = (LinePathElement)r.Elements[1];
            AssertEx.Equal(2.41421356237309, 1, 45.7639320225002, 1, line, 6);
            arc = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(26.565051177078, arc.Angle, 6);
            Assert.Equal(9.47213595499958, arc.Radius, 6);
            AssertEx.Equal(45.7639320225002, -8.47213595499958, arc.Center);
            AssertEx.Equal(45.7639320225002, 1, arc.Start);
            AssertEx.Equal(50, 0, arc.End);
            AssertEx.Equal(1, 0, arc.DirectionStart);

            #endregion
        }


        [Fact]
        public void T04a_Should_create_normal_configuration()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, 1, 1),
                End       = new PathRay(50, 0, -2, 1),
                Reference = new PathRay(13, 4, 1, 0.1)
            };
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(4, "normal config, rotated ref"));
            var code = new DpAssertsBuilder().Create(r, nameof(r));

            #region Asserts

            AssertEx.Equal(4.44089209850063E-16, 4.44089209850063E-16, r.Start);
            AssertEx.Equal(50, -1.77635683940025E-15, r.End);
            Assert.Equal(3, r.Elements.Count);
            var arc = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(39.2894068625004, arc.Angle, 6);
            Assert.Equal(11.8853668094486, arc.Radius, 6);
            AssertEx.Equal(8.40422346785063, -8.40422346785063, arc.Center);
            AssertEx.Equal(4.44089209850063E-16, 4.44089209850063E-16, arc.Start);
            AssertEx.Equal(7.22158526838175, 3.42215852683818, arc.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, arc.DirectionStart);
            var line = (LinePathElement)r.Elements[1];
            AssertEx.Equal(7.22158526838175, 3.42215852683818, 22.889770716363, 4.9889770716363, line, 6);
            arc = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(32.2756443145776, arc.Angle, 6);
            Assert.Equal(49.5872886473176, arc.Radius, 6);
            AssertEx.Equal(27.8238903529388, -44.3522192941223, arc.Center);
            AssertEx.Equal(22.889770716363, 4.9889770716363, arc.Start);
            AssertEx.Equal(50, -1.77635683940025E-15, arc.End);
            AssertEx.Equal(0.995037190209989, 0.0995037190209989, arc.DirectionStart);

            #endregion
        }


        [Fact]
        public void T05_Should_create_normal_configuration_with_min_radius_left()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, 1, 1),
                End       = new PathRay(50, 0, -2, 1),
                Reference = new PathRay(13, 1, 0, 0)
            };
            var r = a.Compute(new MinimumValuesPathValidator(4, 1));
            ResultDrawer.Draw(a, r, MakeTitle(5, "normal config, min radius=4, left"));
            var code = new DpAssertsBuilder().Create(r, nameof(r));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(50, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(69.1806326720388, tmp1.Angle, 6);
            AssertEx.Equal(6.02251873596345, -6.02251873596345, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(9.51125936798172, 1.74730447004106, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(24.1806326720388, tmp1.Angle, 6);
            AssertEx.Equal(13, 9.51712767604558, tmp1.Center);
            AssertEx.Equal(9.51125936798172, 1.74730447004106, tmp1.Start);
            AssertEx.Equal(13, 1, tmp1.End);
            AssertEx.Equal(7.76982320600451, -3.48874063201828, tmp1.DirectionStart);
            AssertEx.Equal(13, 1, 45.7639320225002, 1, (LinePathElement)r.Elements[2], 6);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(26.565051177078, tmp1.Angle, 6);
            AssertEx.Equal(45.7639320225002, -8.47213595499958, tmp1.Center);
            AssertEx.Equal(45.7639320225002, 1, tmp1.Start);
            AssertEx.Equal(50, 0, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);

            #endregion
        }


        [Fact]
        public void T05_Should_create_normal_configuration_with_min_radius_right()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, 1, 1),
                End       = new PathRay(50, 0, -2, 1),
                Reference = new PathRay(47.5, 1, 0, 0)
            };
            var r = a.Compute(new MinimumValuesPathValidator(4, 1));
            ResultDrawer.Draw(a, r, MakeTitle(5, "normal config, min radius=4, right"));
            var code = new DpAssertsBuilder().Create(r, nameof(r));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(50, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(74.3867633280261, tmp1.Angle, 6);
            Assert.Equal(28.131282373654, tmp1.Radius, 6);
            AssertEx.Equal(19.8918205298843, -19.8918205298843, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(33.6959102649422, 4.61973092188483, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(29.3867633280261, tmp1.Angle, 6);
            Assert.Equal(28.131282373654, tmp1.Radius, 6);
            AssertEx.Equal(47.5, 29.131282373654, tmp1.Center);
            AssertEx.Equal(33.6959102649422, 4.61973092188483, tmp1.Start);
            AssertEx.Equal(47.5, 1, tmp1.End);
            AssertEx.Equal(24.5115514517692, -13.8040897350578, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(353.936768191595, tmp1.Angle, 6);
            Assert.Equal(10.5949406013085, tmp1.Radius, 6);
            AssertEx.Equal(47.5, 11.5949406013085, tmp1.Center);
            AssertEx.Equal(47.5, 1, tmp1.Start);
            AssertEx.Equal(46.3808992597902, 1.05926882023459, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(20.5018193686729, tmp1.Angle, 6);
            Assert.Equal(10.5949406013085, tmp1.Radius, 6);
            AssertEx.Equal(45.2617985195804, -9.47640296083929, tmp1.Center);
            AssertEx.Equal(46.3808992597902, 1.05926882023459, tmp1.Start);
            AssertEx.Equal(50, 0, tmp1.End);
            AssertEx.Equal(10.5356717810739, -1.11910074020982, tmp1.DirectionStart);

            #endregion
        }


        [Fact]
        public void T06_Should_create_high()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, 1, 1),
                End       = new PathRay(30, 0, -2, 1),
                Reference = new PathRay(13, 20, 0, 0)
            };

            var cross = a.Start.GetMovedRayOutput().Cross(a.End.GetMovedRayInput()).Value;
            a.Reference = new PathRay(cross + new Vector(0, -0.02), default(Vector));
            var r = a.Compute(new MinimumValuesPathValidator(3, 0.5));
            ResultDrawer.Draw(a, r, MakeTitle(6, "high"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(31.3034673717492, tmp1.Angle, 6);
            Assert.Equal(8.0904831824097, tmp1.Radius, 6);
            AssertEx.Equal(-5.72083552135762, 5.72083552135762, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(2.13958223932119, 3.80517616947396, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(76.3034673717492, tmp1.Angle, 6);
            Assert.Equal(8.0904831824097, tmp1.Radius, 6);
            AssertEx.Equal(10, 1.8895168175903, tmp1.Center);
            AssertEx.Equal(2.13958223932119, 3.80517616947396, tmp1.Start);
            AssertEx.Equal(10, 9.98, tmp1.End);
            AssertEx.Equal(1.91565935188366, 7.86041776067881, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45.1866493554084, tmp1.Angle, 6);
            Assert.Equal(20.5846141238144, tmp1.Radius, 6);
            AssertEx.Equal(10, -10.6046141238144, tmp1.Center);
            AssertEx.Equal(10, 9.98, tmp1.Start);
            AssertEx.Equal(24.6028596471451, 3.90341223238305, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(18.6215981783304, tmp1.Angle, 6);
            Assert.Equal(20.5846141238144, tmp1.Radius, 6);
            AssertEx.Equal(39.2057192942903, 18.4114385885805, tmp1.Center);
            AssertEx.Equal(24.6028596471451, 3.90341223238305, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(14.5080263561975, -14.6028596471451, tmp1.DirectionStart);

            #endregion
        }

        [Fact]
        public void T07_Should_create_high_top()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, 1, 1),
                End       = new PathRay(30, 0, -1, 1),
                Reference = new PathRay(13, 20, 0, 0)
            };

            var cross = a.Start.CrossMeAsBeginWithEnd(a.End).Value;
            a.Reference = new PathRay(cross, default(Vector));
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(7, "high top"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(31.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(-8.57575296716065, 8.57575296716065, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(3.21212351641967, 5.72390340672037, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(76.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(15, 2.87205384628009, tmp1.Center);
            AssertEx.Equal(3.21212351641967, 5.72390340672037, tmp1.Start);
            AssertEx.Equal(15, 15, tmp1.End);
            AssertEx.Equal(2.85184956044028, 11.7878764835803, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(76.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(15, 2.87205384628009, tmp1.Center);
            AssertEx.Equal(15, 15, tmp1.Start);
            AssertEx.Equal(26.7878764835803, 5.72390340672037, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(31.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(38.5757529671607, 8.57575296716065, tmp1.Center);
            AssertEx.Equal(26.7878764835803, 5.72390340672037, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(2.85184956044028, -11.7878764835803, tmp1.DirectionStart);

            #endregion
        }

        [Fact]
        public void T08_Should_create_too_high()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start = new PathRay(0, 0, 1, 1),
                End   = new PathRay(30, 0, -2, 1),
            };
            var cross = a.Start.CrossMeAsBeginWithEnd(a.End).Value;
            a.Reference = new PathRay(cross + new Vector(0, 1), default(Vector));

            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(8, "too high"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(36.0301611959271, tmp1.Angle, 6);
            AssertEx.Equal(-5.57464243621784, 5.57464243621784, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(2.21267878189108, 4.34545374876899, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(81.0301611959271, tmp1.Angle, 6);
            AssertEx.Equal(10, 3.11626506132013, tmp1.Center);
            AssertEx.Equal(2.21267878189108, 4.34545374876899, tmp1.Start);
            AssertEx.Equal(10, 11, tmp1.End);
            AssertEx.Equal(1.22918868744886, 7.78732121810892, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(49.1380265027694, tmp1.Angle, 6);
            AssertEx.Equal(10, -7.77295997565143, tmp1.Center);
            AssertEx.Equal(10, 11, tmp1.Start);
            AssertEx.Equal(24.1977614644439, 4.50904294106216, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(22.5729753256914, tmp1.Angle, 6);
            AssertEx.Equal(38.3955229288879, 16.7910458577758, tmp1.Center);
            AssertEx.Equal(24.1977614644439, 4.50904294106216, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(12.2820029167136, -14.1977614644439, tmp1.DirectionStart);

            #endregion
        }


        [Fact]
        public void T09_Should_create_too_high_right()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start = new PathRay(0, 0, 1, 1),
                End   = new PathRay(30, 0, -2, 1),
            };
            var cross = a.Start.CrossMeAsBeginWithEnd(a.End).Value;
            a.Reference = new PathRay(cross + new Vector(20, 1), default(Vector));

            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(9, "too high right"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45, tmp1.Angle, 6);
            AssertEx.Equal(26.556349186104, -26.556349186104, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(26.556349186104, 11, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.DirectionStart);
            AssertEx.Equal(26.556349186104, 11, 30, 11, (LinePathElement)r.Elements[1], 6);
            tmp1 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(167.079033618416, tmp1.Angle, 6);
            AssertEx.Equal(30, 8.13823822959867, tmp1.Center);
            AssertEx.Equal(30, 11, tmp1.Start);
            AssertEx.Equal(30.6399093854028, 5.34893788560484, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(140.513982441338, tmp1.Angle, 6);
            AssertEx.Equal(31.2798187708055, 2.55963754161101, tmp1.Center);
            AssertEx.Equal(30.6399093854028, 5.34893788560484, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(-2.78930034399383, -0.639909385402753, tmp1.DirectionStart);

            #endregion
        }


        [Fact]
        public void T10_Should_create_too_high_left()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start = new PathRay(0, 0, 1, 1),
                End   = new PathRay(30, 0, -2, 1),
            };
            var cross = a.Start.CrossMeAsBeginWithEnd(a.End).Value;
            a.Reference = new PathRay(cross + new Vector(-7.2, 1), default(Vector));

            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(10, "too high left"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(87.1333501934733, tmp1.Angle, 6);
            AssertEx.Equal(-2.55120522668932, 2.55120522668932, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(0.12439738665534, 4.97162809735408, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(132.133350193473, tmp1.Angle, 6);
            AssertEx.Equal(2.8, 7.39205096801884, tmp1.Center);
            AssertEx.Equal(0.12439738665534, 4.97162809735408, tmp1.Start);
            AssertEx.Equal(2.8, 11, tmp1.End);
            AssertEx.Equal(-2.42042287066476, 2.67560261334466, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(26.565051177078, tmp1.Angle, 6);
            AssertEx.Equal(2.8, -11.0275534829989, tmp1.Center);
            AssertEx.Equal(2.8, 11, tmp1.Start);
            AssertEx.Equal(12.6510213931996, 8.67448930340022, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);
            AssertEx.Equal(12.6510213931996, 8.67448930340022, 30, 0, (LinePathElement)r.Elements[3], 6);

            #endregion
        }


        [Fact]
        public void T11_001_Should_create_below()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, 1, 1),
                End       = new PathRay(50, 0, -2, 1),
                Reference = new PathRay(13, -2, 0, 0)
            };
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(11_001, "below"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(50, 0, r.End);
            Assert.Equal(4, r.Elements.Count);
            var tmp1 = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(91.5730144980679, tmp1.Angle, 6);
            AssertEx.Equal(4.25650626615298, -4.25650626615298, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(8.62825313307649, -0.118448688116687, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(46.5730144980679, tmp1.Angle, 6);
            AssertEx.Equal(13, 4.0196088899196, tmp1.Center);
            AssertEx.Equal(8.62825313307649, -0.118448688116687, tmp1.Start);
            AssertEx.Equal(13, -2, tmp1.End);
            AssertEx.Equal(4.13805757803629, -4.37174686692351, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(24.0647556287283, tmp1.Angle, 6);
            AssertEx.Equal(13, 27.301097445316, tmp1.Center);
            AssertEx.Equal(13, -2, tmp1.Start);
            AssertEx.Equal(24.9480754296928, 0.546699582043599, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.DirectionStart);
            tmp1 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(50.6298068058063, tmp1.Angle, 6);
            AssertEx.Equal(36.8961508593856, -26.2076982812288, tmp1.Center);
            AssertEx.Equal(24.9480754296928, 0.546699582043599, tmp1.Start);
            AssertEx.Equal(50, 0, tmp1.End);
            AssertEx.Equal(26.7543978632724, 11.9480754296928, tmp1.DirectionStart);

            #endregion
        }


        [Fact]
        public void T11_002_Should_create_below_stating_arm()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRayWithArm(0, 0, 1, 1, 3),
                End       = new PathRayWithArm(50, 0, -2, 1),
                Reference = new PathRay(13, -2, 0, 0)
            };
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(11_002, "below, starting arm"));
            var code = new DpAssertsBuilder().Create(r, nameof(r));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(50, 0, r.End);
            Assert.Equal(5, r.Elements.Count);
            var tmp1 = (LinePathElement)r.Elements[0];
            AssertEx.Equal(0, 0, 2.12132034355964, 2.12132034355964, tmp1, 6);
            var tmp2 = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp2.Direction);
            Assert.Equal(113.454547289735, tmp2.Angle, 6);
            Assert.Equal(4.2373022595867, tmp2.Radius, 6);
            AssertEx.Equal(5.11754550525048, -0.874904818131196, tmp2.Center);
            AssertEx.Equal(2.12132034355964, 2.12132034355964, tmp2.Start);
            AssertEx.Equal(9.05877275262524, 0.681198720727754, tmp2.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp2.DirectionStart);
            tmp2 = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.CounterClockwise, tmp2.Direction);
            Assert.Equal(68.4545472897351, tmp2.Angle, 6);
            Assert.Equal(4.2373022595867, tmp2.Radius, 6);
            AssertEx.Equal(13, 2.2373022595867, tmp2.Center);
            AssertEx.Equal(9.05877275262524, 0.681198720727754, tmp2.Start);
            AssertEx.Equal(13, -2, tmp2.End);
            AssertEx.Equal(1.55610353885895, -3.94122724737476, tmp2.DirectionStart);
            tmp2 = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp2.Direction);
            Assert.Equal(24.0647556287283, tmp2.Angle, 6);
            Assert.Equal(29.301097445316, tmp2.Radius, 6);
            AssertEx.Equal(13, 27.301097445316, tmp2.Center);
            AssertEx.Equal(13, -2, tmp2.Start);
            AssertEx.Equal(24.9480754296928, 0.546699582043599, tmp2.End);
            AssertEx.Equal(1, 0, tmp2.DirectionStart);
            tmp2 = (ArcDefinition)r.Elements[4];
            Assert.Equal(ArcDirection.Clockwise, tmp2.Direction);
            Assert.Equal(50.6298068058063, tmp2.Angle, 6);
            Assert.Equal(29.301097445316, tmp2.Radius, 6);
            AssertEx.Equal(36.8961508593856, -26.2076982812288, tmp2.Center);
            AssertEx.Equal(24.9480754296928, 0.546699582043599, tmp2.Start);
            AssertEx.Equal(50, 0, tmp2.End);
            AssertEx.Equal(26.7543978632724, 11.9480754296928, tmp2.DirectionStart);

            #endregion
        }


        [Fact]
        public void T11_003_Should_create_below_end_arm()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRayWithArm(0, 0, 1, 1),
                End       = new PathRayWithArm(50, 0, -2, 1, 5),
                Reference = new PathRay(13, -2, 0, 0)
            };
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(11_003, "below, ending arm"));
            var code = new DpAssertsBuilder().Create(r, nameof(r));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(50, 0, r.End);
            Assert.Equal(5, r.Elements.Count);
            var arc = (ArcDefinition)r.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(91.5730144980679, arc.Angle, 6);
            Assert.Equal(6.0196088899196, arc.Radius, 6);
            AssertEx.Equal(4.25650626615298, -4.25650626615298, arc.Center);
            AssertEx.Equal(0, 0, arc.Start);
            AssertEx.Equal(8.62825313307649, -0.118448688116687, arc.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, arc.DirectionStart);
            arc = (ArcDefinition)r.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(46.5730144980679, arc.Angle, 6);
            Assert.Equal(6.0196088899196, arc.Radius, 6);
            AssertEx.Equal(13, 4.0196088899196, arc.Center);
            AssertEx.Equal(8.62825313307649, -0.118448688116687, arc.Start);
            AssertEx.Equal(13, -2, arc.End);
            AssertEx.Equal(4.13805757803629, -4.37174686692351, arc.DirectionStart);
            arc = (ArcDefinition)r.Elements[2];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(31.8582989376357, arc.Angle, 6);
            Assert.Equal(21.6440585760206, arc.Radius, 6);
            AssertEx.Equal(13, 19.6440585760206, arc.Center);
            AssertEx.Equal(13, -2, arc.Start);
            AssertEx.Equal(24.4241733940033, 1.26054601976632, arc.End);
            AssertEx.Equal(1, 0, arc.DirectionStart);
            arc = (ArcDefinition)r.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(58.4233501147137, arc.Angle, 6);
            Assert.Equal(21.6440585760206, arc.Radius, 6);
            AssertEx.Equal(35.8483467880065, -17.122966536488, arc.Center);
            AssertEx.Equal(24.4241733940033, 1.26054601976632, arc.Start);
            AssertEx.Equal(45.5278640450004, 2.23606797749979, arc.End);
            AssertEx.Equal(18.3835125562543, 11.4241733940033, arc.DirectionStart);
            var line = (LinePathElement)r.Elements[4];
            AssertEx.Equal(45.5278640450004, 2.23606797749979, 50, 0, line, 6);

            #endregion
        }

        [Fact]
        public void T99a_Should_compute_practical_case()
        {
            var o = new OneReferencePointPathCalculator
            {
                Start = new PathRay(48.426398878846122, 43.73157300192598,
                    0.96711073366397649, 0.25435571318907962),
                End = new PathRay(147.16448720645866, 69.700260972482937,
                    -0.96711073366397649, -0.25435571318907962),
                Reference = new PathRay(110.61711161799218, 49.932980045672636, 0, 0)
            };

            var result = o.Compute(null);
            new ResultDrawerConfig
            {
                Start  = o.Start,
                End    = o.End,
                Result = result,
                Title  = MakeTitle(99, "practical case A"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            AssertEx.Equal(48.4263988788461, 43.731573001926, result.Start);
            AssertEx.Equal(147.164487206459, 69.7002609724829, result.End);
            Assert.Equal(4, result.Elements.Count);
            var arc = (ArcDefinition)result.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(18.0818631776401, arc.Angle, 6);
            AssertEx.Equal(73.7175574338968, -52.4304144711244, arc.Center);
            AssertEx.Equal(48.4263988788461, 43.731573001926, arc.Start);
            AssertEx.Equal(79.5217552484192, 46.8322765237993, arc.End);
            AssertEx.Equal(0.967110733663976, 0.25435571318908, arc.DirectionStart);
            arc = (ArcDefinition)result.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(18.0818631776401, arc.Angle, 6);
            AssertEx.Equal(85.3259530629415, 146.094967518723, arc.Center);
            AssertEx.Equal(79.5217552484192, 46.8322765237993, arc.Start);
            AssertEx.Equal(110.617111617992, 49.9329800456726, arc.End);
            AssertEx.Equal(99.2626909949237, -5.80419781452231, arc.DirectionStart);
            arc = (ArcDefinition)result.Elements[2];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(27.3441904745793, arc.Angle, 6);
            AssertEx.Equal(99.4388002168958, 92.4351310872041, arc.Center);
            AssertEx.Equal(110.617111617992, 49.9329800456726, arc.Start);
            AssertEx.Equal(128.890799412225, 59.8166205090778, arc.End);
            AssertEx.Equal(0.967110733663976, 0.25435571318908, arc.DirectionStart);
            arc = (ArcDefinition)result.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(27.3441904745794, arc.Angle, 6);
            AssertEx.Equal(158.342798607555, 27.1981099309515, arc.Center);
            AssertEx.Equal(128.890799412225, 59.8166205090778, arc.Start);
            AssertEx.Equal(147.164487206459, 69.7002609724829, arc.End);
            AssertEx.Equal(32.6185105781263, 29.4519991953296, arc.DirectionStart);

            #endregion
        }

        [Fact]
        public void T99b_Should_compute_practical_case()
        {
            var o = new OneReferencePointPathCalculator
            {
                Start = new PathRayWithArm(264.09476926, 305.02877797,
                    0.23123524718396984, -0.97289786743510154),
                End = new PathRayWithArm(279.02984002, 243.09038861,
                    -0.22161353894684818, 0.975134574997448),
                Reference = new PathRay(272.35123586, 273.68909084,
                    0.23123524718396984, -0.97289786743510154)
            };

            var result = o.Compute(null);
            new ResultDrawerConfig
            {
                Start  = o.Start,
                End    = o.End,
                Result = result,
                Title  = MakeTitle(99, "practical case B"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            AssertEx.Equal(264.09476926, 305.02877797, result.Start);
            AssertEx.Equal(279.02984002, 243.09038861, result.End);
            Assert.Equal(4, result.Elements.Count);
            var arc = (ArcDefinition)result.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(2.77890364387448, arc.Angle, 6);
            Assert.Equal(334.139447582511, arc.Radius, 6);
            AssertEx.Equal(589.17832523896811, 382.29359572565716, arc.Center);
            AssertEx.Equal(264.09476926, 305.02877797, arc.Start);
            AssertEx.Equal(268.22300256, 289.358934405, arc.End);
            AssertEx.Equal(0.23123524718396984, -0.97289786743510154, arc.DirectionStart);
            arc = (ArcDefinition)result.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(2.77890364387446, arc.Angle, 6);
            Assert.Equal(334.139447582511, arc.Radius, 6);
            AssertEx.Equal(-52.732320118968119, 196.42427308434287, arc.Center);
            AssertEx.Equal(268.22300256, 289.358934405, arc.Start);
            AssertEx.Equal(272.35123586, 273.68909084, arc.End);
            AssertEx.Equal(92.934661320657142, -320.95532267896812, arc.DirectionStart);
            arc = (ArcDefinition)result.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(1.8816595895608, arc.Angle, 6);
            Assert.Equal(561.255004627575, arc.Radius, 6);
            AssertEx.Equal(-273.69256122944626, 143.90715111170246, arc.Center);
            AssertEx.Equal(272.35123586, 273.68909084, arc.Start);
            AssertEx.Equal(276.31821959662767, 255.68962377442458, arc.End);
            AssertEx.Equal(0.23123524718396984, -0.97289786743510154, arc.DirectionStart);
            arc = (ArcDefinition)result.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(1.31567436386978, arc.Angle, 6);
            Assert.Equal(561.255004627575, arc.Radius, 6);
            AssertEx.Equal(826.32900042270148, 367.47209643714666, arc.Center);
            AssertEx.Equal(276.31821959662767, 255.68962377442458, arc.Start);
            AssertEx.Equal(279.02984002, 243.09038861, arc.End);
            AssertEx.Equal(111.78247266272209, -550.01078082607387, arc.DirectionStart);

            #endregion
        }

        [Fact]
        public void T99b_Should_compute_practical_case_REV()
        {
            var o = new OneReferencePointPathCalculator
            {
                End = new PathRayWithArm(264.09476926, 305.02877797,
                    0.23123524718396984, -0.97289786743510154),
                Start = new PathRayWithArm(279.02984002, 243.09038861,
                    -0.22161353894684818, 0.975134574997448),
                Reference = new PathRay(272.35123586, 273.68909084,
                    -0.23123524718396984, 0.97289786743510154)
            };

            var result = o.Compute(null);
            new ResultDrawerConfig
            {
                Start  = o.Start,
                End    = o.End,
                Result = result,
                Title  = MakeTitle(99, "practical case B REV"),
                Flags  = ResultDrawerConfigFlags.ReverseEndMarker
            }.Draw();
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            AssertEx.Equal(279.02984002, 243.09038861, result.Start);
            AssertEx.Equal(264.09476926, 305.02877797, result.End);
            Assert.Equal(4, result.Elements.Count);
            var arc = (ArcDefinition)result.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(1.31567436386979, arc.Angle, 6);
            Assert.Equal(561.255004627575, arc.Radius, 6);
            AssertEx.Equal(826.32900042270148, 367.47209643714666, arc.Center);
            AssertEx.Equal(279.02984002, 243.09038861, arc.Start);
            AssertEx.Equal(276.31821959662767, 255.68962377442458, arc.End);
            AssertEx.Equal(-0.22161353894684818, 0.975134574997448, arc.DirectionStart);
            arc = (ArcDefinition)result.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(1.88165958956077, arc.Angle, 6);
            Assert.Equal(561.255004627575, arc.Radius, 6);
            AssertEx.Equal(-273.69256122944626, 143.90715111170246, arc.Center);
            AssertEx.Equal(276.31821959662767, 255.68962377442458, arc.Start);
            AssertEx.Equal(272.35123586, 273.68909084, arc.End);
            AssertEx.Equal(-111.78247266272211, 550.01078082607387, arc.DirectionStart);
            arc = (ArcDefinition)result.Elements[2];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(2.77890364387448, arc.Angle, 6);
            Assert.Equal(334.139447582511, arc.Radius, 6);
            AssertEx.Equal(-52.732320118968119, 196.42427308434287, arc.Center);
            AssertEx.Equal(272.35123586, 273.68909084, arc.Start);
            AssertEx.Equal(268.22300256, 289.358934405, arc.End);
            AssertEx.Equal(-0.23123524718396984, 0.97289786743510154, arc.DirectionStart);
            arc = (ArcDefinition)result.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(2.77890364387446, arc.Angle, 6);
            Assert.Equal(334.139447582511, arc.Radius, 6);
            AssertEx.Equal(589.17832523896811, 382.29359572565716, arc.Center);
            AssertEx.Equal(268.22300256, 289.358934405, arc.Start);
            AssertEx.Equal(264.09476926, 305.02877797, arc.End);
            AssertEx.Equal(-92.934661320657142, 320.95532267896812, arc.DirectionStart);

            #endregion
        }

        #region Fields

        private const double Epsilon = 1e-6;

        #endregion
    }
}
//  var code = new TestMaker().Create(r, nameof(r));
