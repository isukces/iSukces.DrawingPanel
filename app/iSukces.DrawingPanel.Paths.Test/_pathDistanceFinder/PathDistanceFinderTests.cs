#define _DO_DRAWINGS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using iSukces.Mathematics;
using Xunit;
using Xunit.Abstractions;

namespace iSukces.DrawingPanel.Paths.Test
{
    public partial class PathDistanceFinderTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PathDistanceFinderTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private static IEnumerable<object[]> MakeTestData<T>(IEnumerable<T> src)
        {
            var index = 0;
            foreach (var i in src)
                yield return new object[] { index++, i };
        }

        [Theory]
        [InlineData(6, 6, Three.Inside, 3)]
        [InlineData(6, 2, Three.Inside, 1)]
        [InlineData(6, 3, Three.Inside, 0)]
        [InlineData(5, 2, Three.Inside, 1)]
        [InlineData(53, 2, Three.Inside, 1)]
        [InlineData(4, 6, Three.Below, 3.16227766016838)]
        [InlineData(60, 6, Three.Above, 7.61577310586391)]
        public void T01_Should_calculate_distance_from_line(double x, double y, Three loc, double distance)
        {
            //var track = Math.Max(0, Math.Min(48, x - 5));
            var track = x - 5;
            var l     = new LinePathElement(new Point(5, 3), new Point(53, 3));
            Assert.Equal(48, l.Length);

            var g = PathDistanceFinder.GetDistanceFromLine(new PathResult(l), new Point(x, y));
            Assert.Equal(distance, g.DistanceFromLine, 8);
            Assert.Equal(0, g.ElementIndex);
            Assert.Equal(loc, g.Location);
            Assert.Equal(3 - y, g.SideMovement);
            Assert.Equal(1, g.Direction.X);
            Assert.Equal(0, g.Direction.Y);
            Assert.Equal(track, g.Track);
            Assert.Equal(0, g.ElementTrackOffset);
        }

        [Theory]
        [MemberData(nameof(T02_TestData))]
        public void T02_Should_calculate_distance_from_two_lines(int testIndex, PathDistanceFinderTestData expected)
        {
            PathResult Prepare()
            {
                var l1    = new LinePathElement(new Point(10, 3), new Point(60, 3));
                var l2    = new LinePathElement(l1.End, l1.End + new Vector(0, 10));
                var lines = new[] { l1, l2 };
                return new PathResult(lines);
            }

            var pathResult = Prepare();
            var actual     = PathDistanceFinder.GetDistanceFromLine(pathResult, expected.TestPoint);

            if (testIndex == 0)
            {
                var code = PathDistanceFinderCodeMaker.MakeRandom(pathResult);

#if DO_DRAWINGS
                //  picture worth a thousand words
                var points = T02_TestData().Select(a => ((PathDistanceFinderTestData)a[1]).TestPoint).ToArray();
                const string name = "DistanceFinder, T02 Point assigment to one of two lines";
                PathElementAssignmetnDrawer.Draw(pathResult, name, points);
#endif
            }

            expected.AssertEqual(actual);
        }


        [Theory]
        [MemberData(nameof(T03_TestData))]
        public void T03_Should_calculate_distance_from_line_arc(int testIndex, PathDistanceFinderTestData expected)
        {
            PathResult Prepare()
            {
                var l1             = new LinePathElement(new Point(10, 3), new Point(60, 3));
                var center         = l1.End + new Vector(-10, 14);
                var end            = center + new Vector(10, -2);
                var directionStart = new Vector(1, 0);

                var l2 = new ArcDefinition(center, l1.End, directionStart, end)
                    .FixStartDirection()
                    .FixEndPoint();
                var lines = new IPathElement[] { l1, l2 };
                return new PathResult(lines);
            }

            var pathResult = Prepare();
            var actual     = PathDistanceFinder.GetDistanceFromLine(pathResult, expected.TestPoint);

            if (testIndex == 0)
            {
                var code = PathDistanceFinderCodeMaker.MakeRandom(pathResult);

#if DO_DRAWINGS
                //  picture worth a thousand words
                var points = T03_TestData().Select(a => ((PathDistanceFinderTestData)a[1]).TestPoint).ToArray();
                const string name = "DistanceFinder, T03 Point assigment to line or arc";
                PathElementAssignmetnDrawer.Draw(pathResult, name, points);
#endif
            }

            expected.AssertEqual(actual);
        }

        [Theory]
        [MemberData(nameof(T04_TestData))]
        public void T04_Should_calculate_distance_from_line_arc(int testIndex, PathDistanceFinderTestData expected)
        {
            // if (testIndex != 0) return;
            PathResult Prepare()
            {
                var l1             = new LinePathElement(new Point(10, 3), new Point(60, 3));
                var center         = l1.End + new Vector(12, 14);
                var end            = center + new Vector(10, -2);
                var directionStart = new Vector(1, 0);

                var l2 = new ArcDefinition(center, l1.End, directionStart, end)
                    .FixStartDirection()
                    .FixEndPoint();
                var lines = new IPathElement[] { l1, l2 };
                return new PathResult(lines);
            }

            var pathResult = Prepare();
            var actual     = PathDistanceFinder.GetDistanceFromLine(pathResult, expected.TestPoint);

            if (testIndex == 0)
            {
                var code = PathDistanceFinderCodeMaker.MakeRandom(pathResult);

#if DO_DRAWINGS
                //  picture worth a thousand words
                var points = T04_TestData().Select(a => ((PathDistanceFinderTestData)a[1]).TestPoint).ToArray();
                const string name = "DistanceFinder, T04 Point assigment to line or arc";
                PathElementAssignmetnDrawer.Draw(pathResult, name, points);
#endif
            }

            expected.AssertEqual(actual);
        }

        [Theory]
        [MemberData(nameof(T05_TestData))]
        public void T05_Should_calculate_distance_from_line_arc(int testIndex, PathDistanceFinderTestData expected)
        {
            // if (testIndex != 0) return;
            PathResult Prepare()
            {
                var l1             = new LinePathElement(new Point(10, 3), new Point(60, 3));
                var center         = l1.End + new Vector(12, -14);
                var end            = center + new Vector(10, -2);
                var directionStart = new Vector(1, 0);

                var l2 = new ArcDefinition(center, l1.End, directionStart, end)
                    .FixStartDirection()
                    .FixEndPoint();
                var lines = new IPathElement[] { l1, l2 };
                return new PathResult(lines);
            }

            var pathResult = Prepare();
            var actual     = PathDistanceFinder.GetDistanceFromLine(pathResult, expected.TestPoint);

            if (testIndex == 0)
            {
                var code = PathDistanceFinderCodeMaker.MakeRandom(pathResult);

#if DO_DRAWINGS
                //  picture worth a thousand words
                var points = T05_TestData().Select(a => ((PathDistanceFinderTestData)a[1]).TestPoint).ToArray();
                const string name = "DistanceFinder, T05 Point assigment to line or arc";
                PathElementAssignmetnDrawer.Draw(pathResult, name, points);
#endif
            }

            expected.AssertEqual(actual);
        }


        [Theory]
        [MemberData(nameof(T06_TestData))]
        public void T06_Should_calculate_distance_from_line_arc(int testIndex, PathDistanceFinderTestData expected)
        {
            // if (testIndex != 0) return;
            PathResult Prepare()
            {
                var l1             = new LinePathElement(new Point(10, 3), new Point(60, 3));
                var center         = l1.End + new Vector(-12, -14);
                var end            = center + new Vector(10, -2);
                var directionStart = new Vector(1, 0);

                var l2 = new ArcDefinition(center, l1.End, directionStart, end)
                    .FixStartDirection()
                    .FixEndPoint();
                var lines = new IPathElement[] { l1, l2 };
                return new PathResult(lines);
            }

            var pathResult = Prepare();
            var actual     = PathDistanceFinder.GetDistanceFromLine(pathResult, expected.TestPoint);

            if (testIndex == 0)
            {
                var code = PathDistanceFinderCodeMaker.MakeRandom(pathResult);

#if DO_DRAWINGS
                //  picture worth a thousand words
                var points = T06_TestData().Select(a => ((PathDistanceFinderTestData)a[1]).TestPoint).ToArray();
                const string name = "DistanceFinder, T06 Point assigment to line or arc";
                PathElementAssignmetnDrawer.Draw(pathResult, name, points);
#endif
            }

            expected.AssertEqual(actual);
        }


        [Theory]
        [MemberData(nameof(T13_TestData))]
        public void T13_Should_calculate_distance_from_arc_line(int testIndex, PathDistanceFinderTestData expected)
        {
            _testOutputHelper.WriteLine(testIndex.ToString());
            PathResult Prepare()
            {

                var center = new Point(10, 4);

                var startRadius = new Vector(-10, 0);
                var start  = center + startRadius;
                var end    = center + new Vector(3, 10);


                var l2 = new ArcDefinition(center, start, startRadius.GetPrependicular(false), end)
                    .FixStartDirection()
                    .FixEndPoint();
                var l1    = new LinePathElement(l2.End, l2.End + new Vector(30, 0));
                var lines = new IPathElement[] { l2, l1 };
                return new PathResult(lines);
            }

            var pathResult = Prepare();

            if (testIndex == 0)
            {
#if DO_DRAWINGS
                //  picture worth a thousand words
                var points = T13_TestData().Select(a => ((PathDistanceFinderTestData)a[1]).TestPoint).ToArray();
                const string name = "DistanceFinder, T13 Point assigment to arc or line";
                PathElementAssignmetnDrawer.Draw(pathResult, name, points);
#endif
                var code = PathDistanceFinderCodeMaker.MakeRandom(pathResult);
            }

            var actual = PathDistanceFinder.GetDistanceFromLine(pathResult, expected.TestPoint);
            expected.AssertEqual(actual);
        }
    }
}
