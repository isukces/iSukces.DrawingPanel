using System;
using System.Windows;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public class OneReferencePointPathCalculatorTests
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
                Start     = new PathRay(0, 0, 13, 0),
                End       = new PathRay(50, 0, -11, 0),
                Reference = new PathRay(x, 0, 0, 0)
            };
            var r = a.Compute(null);
            if (Math.Abs(x - 12.4) < 0.0001)
                ResultDrawer.Draw(a, r, MakeTitle(1, "colinear"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(50, 0, r.End);
            Assert.Single(r.Arcs);
            AssertEx.Equal(0, 0, 50, 0, (LinePathElement)r.Arcs[0], 6);

            #endregion
        }

        [Fact]
        public void T02_Should_create_colinear_not_line()
        {
            var a = new OneReferencePointPathCalculator
            {
                Start     = new PathRay(0, 0, 13, 0),
                End       = new PathRay(50, 0, -11, 0),
                Reference = new PathRay(18, 8, 0, 0)
            };
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(2, "colinear"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(50, 0, r.End);
            Assert.Equal(4, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(47.9249779491564, tmp1.Angle, 6);
            AssertEx.Equal(0, 12.125, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(9, 4, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(47.9249779491564, tmp1.Angle, 6);
            AssertEx.Equal(18, -4.125, tmp1.Center);
            AssertEx.Equal(9, 4, tmp1.Start);
            AssertEx.Equal(18, 8, tmp1.End);
            AssertEx.Equal(8.125, 9, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(28.072486935853, tmp1.Angle, 6);
            AssertEx.Equal(18, -26, tmp1.Center);
            AssertEx.Equal(18, 8, tmp1.Start);
            AssertEx.Equal(34, 4, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(28.072486935853, tmp1.Angle, 6);
            AssertEx.Equal(50, 34, tmp1.Center);
            AssertEx.Equal(34, 4, tmp1.Start);
            AssertEx.Equal(50, 0, tmp1.End);
            AssertEx.Equal(30, -16, tmp1.StartVector);

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
            Assert.Equal(4, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(78.6180267593746, tmp1.Angle, 6);
            AssertEx.Equal(-0.856275169326163, 1.28441275398924, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(0.571862415336919, 1.8703703697666, tmp1.End);
            AssertEx.Equal(0.832050294337844, 0.554700196225229, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(112.308094285354, tmp1.Angle, 6);
            AssertEx.Equal(2, 2.45632798554396, tmp1.Center);
            AssertEx.Equal(0.571862415336919, 1.8703703697666, tmp1.Start);
            AssertEx.Equal(2, 4, tmp1.End);
            AssertEx.Equal(-0.585957615777356, 1.42813758466308, tmp1.StartVector);
            AssertEx.Equal(2, 4, 20.3431457505076, 4, (LinePathElement)r.Arcs[2], 6);
            tmp1 = (ArcDefinition)r.Arcs[3];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45, tmp1.Angle, 6);
            AssertEx.Equal(20.3431457505076, -9.65685424949238, tmp1.Center);
            AssertEx.Equal(20.3431457505076, 4, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(30, 0, tmp1.StartVector);

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

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(33.6900675259798, tmp1.Angle, 6);
            AssertEx.Equal(13.211102550928, -19.816653826392, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(13.211102550928, 4, tmp1.End);
            AssertEx.Equal(6, 4, tmp1.StartVector);
            AssertEx.Equal(13.211102550928, 4, 26, 4, (LinePathElement)r.Arcs[1], 6);
            tmp1 = (ArcDefinition)r.Arcs[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(76.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(26, 0.765881025674692, tmp1.Center);
            AssertEx.Equal(26, 4, tmp1.Start);
            AssertEx.Equal(29.1434337289548, 1.5263742417921, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(31.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(32.2868674579095, 2.28686745790951, tmp1.Center);
            AssertEx.Equal(29.1434337289548, 1.5263742417921, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(0.760493216117408, -3.14343372895475, tmp1.StartVector);

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

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(33.6900675259798, tmp1.Angle, 6);
            AssertEx.Equal(13.211102550928, -19.816653826392, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(13.211102550928, 4, tmp1.End);
            AssertEx.Equal(6, 4, tmp1.StartVector);
            AssertEx.Equal(13.211102550928, 4, 29, 4, (LinePathElement)r.Arcs[1], 6);
            tmp1 = (ArcDefinition)r.Arcs[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(132.595764207088, tmp1.Angle, 6);
            AssertEx.Equal(29, 2.69313071723601, tmp1.Center);
            AssertEx.Equal(29, 4, tmp1.Start);
            AssertEx.Equal(29.9620480659834, 1.80861342460141, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(87.5957642070877, tmp1.Angle, 6);
            AssertEx.Equal(30.9240961319668, 0.924096131966815, tmp1.Center);
            AssertEx.Equal(29.9620480659834, 1.80861342460141, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(-0.884517292634599, -0.96204806598341, tmp1.StartVector);

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

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(23.6482664310892, tmp1.Angle, 6);
            AssertEx.Equal(-2.94780495706329, 4.42170743559494, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(1.52609752146835, 1.55373823710781, tmp1.End);
            AssertEx.Equal(0.832050294337844, 0.554700196225229, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(57.338333957069, tmp1.Angle, 6);
            AssertEx.Equal(6, -1.31423096137931, tmp1.Center);
            AssertEx.Equal(1.52609752146835, 1.55373823710781, tmp1.Start);
            AssertEx.Equal(6, 4, tmp1.End);
            AssertEx.Equal(2.86796919848713, 4.47390247853165, tmp1.StartVector);
            AssertEx.Equal(6, 4, 20.3431457505076, 4, (LinePathElement)r.Arcs[2], 6);
            tmp1 = (ArcDefinition)r.Arcs[3];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45, tmp1.Angle, 6);
            AssertEx.Equal(20.3431457505076, -9.65685424949238, tmp1.Center);
            AssertEx.Equal(20.3431457505076, 4, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(30, 0, tmp1.StartVector);

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

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(50, 0, r.End);
            Assert.Equal(3, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45, tmp1.Angle, 6);
            AssertEx.Equal(2.41421356237309, -2.41421356237309, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(2.41421356237309, 1, tmp1.End);
            AssertEx.Equal(1, 1, tmp1.StartVector);
            AssertEx.Equal(2.41421356237309, 1, 45.7639320225002, 1, (LinePathElement)r.Arcs[1], 6);
            tmp1 = (ArcDefinition)r.Arcs[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(26.565051177078, tmp1.Angle, 6);
            AssertEx.Equal(45.7639320225002, -8.47213595499958, tmp1.Center);
            AssertEx.Equal(45.7639320225002, 1, tmp1.Start);
            AssertEx.Equal(50, 0, tmp1.End);
            AssertEx.Equal(50, 0, tmp1.StartVector);

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

            var cross = a.Start.Cross(a.End).Value;
            a.Reference = new PathRay(cross + new Vector(0, -0.02), default(Vector));
            var r = a.Compute(new MinimumValuesPathValidator(3, 0.5));
            ResultDrawer.Draw(a, r, MakeTitle(6, "high"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(31.3034673717492, tmp1.Angle, 6);
            AssertEx.Equal(-5.72083552135762, 5.72083552135762, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(2.13958223932119, 3.80517616947396, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(76.3034673717492, tmp1.Angle, 6);
            AssertEx.Equal(10, 1.8895168175903, tmp1.Center);
            AssertEx.Equal(2.13958223932119, 3.80517616947396, tmp1.Start);
            AssertEx.Equal(10, 9.98, tmp1.End);
            AssertEx.Equal(1.91565935188366, 7.86041776067881, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45.1866493554084, tmp1.Angle, 6);
            AssertEx.Equal(10, -10.6046141238144, tmp1.Center);
            AssertEx.Equal(10, 9.98, tmp1.Start);
            AssertEx.Equal(24.6028596471451, 3.90341223238305, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(18.6215981783304, tmp1.Angle, 6);
            AssertEx.Equal(39.2057192942903, 18.4114385885805, tmp1.Center);
            AssertEx.Equal(24.6028596471451, 3.90341223238305, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(14.5080263561975, -14.6028596471451, tmp1.StartVector);

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

            var cross = a.Start.Cross(a.End).Value;
            a.Reference = new PathRay(cross, default(Vector));
            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(7, "high top"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(31.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(-8.57575296716065, 8.57575296716065, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(3.21212351641967, 5.72390340672037, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(76.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(15, 2.87205384628009, tmp1.Center);
            AssertEx.Equal(3.21212351641967, 5.72390340672037, tmp1.Start);
            AssertEx.Equal(15, 15, tmp1.End);
            AssertEx.Equal(2.85184956044028, 11.7878764835803, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(76.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(15, 2.87205384628009, tmp1.Center);
            AssertEx.Equal(15, 15, tmp1.Start);
            AssertEx.Equal(26.7878764835803, 5.72390340672037, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(31.399714809919, tmp1.Angle, 6);
            AssertEx.Equal(38.5757529671607, 8.57575296716065, tmp1.Center);
            AssertEx.Equal(26.7878764835803, 5.72390340672037, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(2.85184956044028, -11.7878764835803, tmp1.StartVector);

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
            var cross = a.Start.Cross(a.End).Value;
            a.Reference = new PathRay(cross + new Vector(0, 1), default(Vector));

            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(8, "too high"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(36.0301611959271, tmp1.Angle, 6);
            AssertEx.Equal(-5.57464243621784, 5.57464243621784, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(2.21267878189108, 4.34545374876899, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(81.0301611959271, tmp1.Angle, 6);
            AssertEx.Equal(10, 3.11626506132013, tmp1.Center);
            AssertEx.Equal(2.21267878189108, 4.34545374876899, tmp1.Start);
            AssertEx.Equal(10, 11, tmp1.End);
            AssertEx.Equal(1.22918868744886, 7.78732121810892, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(49.1380265027694, tmp1.Angle, 6);
            AssertEx.Equal(10, -7.77295997565143, tmp1.Center);
            AssertEx.Equal(10, 11, tmp1.Start);
            AssertEx.Equal(24.1977614644439, 4.50904294106216, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(22.5729753256914, tmp1.Angle, 6);
            AssertEx.Equal(38.3955229288879, 16.7910458577758, tmp1.Center);
            AssertEx.Equal(24.1977614644439, 4.50904294106216, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(12.2820029167136, -14.1977614644439, tmp1.StartVector);

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
            var cross = a.Start.Cross(a.End).Value;
            a.Reference = new PathRay(cross + new Vector(20, 1), default(Vector));

            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(9, "too high right"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(45, tmp1.Angle, 6);
            AssertEx.Equal(26.556349186104, -26.556349186104, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(26.556349186104, 11, tmp1.End);
            AssertEx.Equal(1, 1, tmp1.StartVector);
            AssertEx.Equal(26.556349186104, 11, 30, 11, (LinePathElement)r.Arcs[1], 6);
            tmp1 = (ArcDefinition)r.Arcs[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(167.079033618416, tmp1.Angle, 6);
            AssertEx.Equal(30, 8.13823822959867, tmp1.Center);
            AssertEx.Equal(30, 11, tmp1.Start);
            AssertEx.Equal(30.6399093854028, 5.34893788560484, tmp1.End);
            AssertEx.Equal(1, 0, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(140.513982441338, tmp1.Angle, 6);
            AssertEx.Equal(31.2798187708055, 2.55963754161101, tmp1.Center);
            AssertEx.Equal(30.6399093854028, 5.34893788560484, tmp1.Start);
            AssertEx.Equal(30, 0, tmp1.End);
            AssertEx.Equal(-2.78930034399383, -0.639909385402753, tmp1.StartVector);

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
            var cross = a.Start.Cross(a.End).Value;
            a.Reference = new PathRay(cross + new Vector(-7.2, 1), default(Vector));

            var r = a.Compute(null);
            ResultDrawer.Draw(a, r, MakeTitle(10, "too high left"));

            #region Asserts

            AssertEx.Equal(0, 0, r.Start);
            AssertEx.Equal(30, 0, r.End);
            Assert.Equal(4, r.Arcs.Count);
            var tmp1 = (ArcDefinition)r.Arcs[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp1.Direction);
            Assert.Equal(87.1333501934733, tmp1.Angle, 6);
            AssertEx.Equal(-2.55120522668932, 2.55120522668932, tmp1.Center);
            AssertEx.Equal(0, 0, tmp1.Start);
            AssertEx.Equal(0.12439738665534, 4.97162809735408, tmp1.End);
            AssertEx.Equal(0.707106781186547, 0.707106781186547, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[1];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(132.133350193473, tmp1.Angle, 6);
            AssertEx.Equal(2.8, 7.39205096801884, tmp1.Center);
            AssertEx.Equal(0.12439738665534, 4.97162809735408, tmp1.Start);
            AssertEx.Equal(2.8, 11, tmp1.End);
            AssertEx.Equal(-2.42042287066476, 2.67560261334466, tmp1.StartVector);
            tmp1 = (ArcDefinition)r.Arcs[2];
            Assert.Equal(ArcDirection.Clockwise, tmp1.Direction);
            Assert.Equal(26.565051177078, tmp1.Angle, 6);
            AssertEx.Equal(2.8, -11.0275534829989, tmp1.Center);
            AssertEx.Equal(2.8, 11, tmp1.Start);
            AssertEx.Equal(12.6510213931996, 8.67448930340022, tmp1.End);
            AssertEx.Equal(30, 0, tmp1.StartVector);
            AssertEx.Equal(12.6510213931996, 8.67448930340022, 30, 0, (LinePathElement)r.Arcs[3], 6);

            #endregion
        }


        private const double Epsilon = 1e-6;
    }
}
//  var code = new TestMaker().Create(r, nameof(r));
