using System.Collections.Generic;
using iSukces.DrawingPanel.Paths;
using iSukces.DrawingPanel.Paths.Test;
using Newtonsoft.Json;
using Xunit;
#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif

namespace Test.Pd.Common.Geometry
{
    public sealed class ArcPathMakerTests
    {
        static ArcPathMakerTests()
        {
            VariablesDictionary.GetVarName = t =>
            {
                if (t == typeof(ArcDefinition))
                    return "arc";
                if (t == typeof(IPathResult))
                    return "pathResult";
                if (t == typeof(LinePathElement))
                    return "line";
                return null;
            };
        }

        private static TestName MakeTitle(int testNumber, string title)
        {
            return new TestName(testNumber, "PathMaker", title);
        }

        [Fact]
        public void T01_Should_create_empty_path()
        {
            for (var i = -1; i < 2; i++)
            {
                var maker = new ArcPathMaker();
                if (i >= 0)
                    maker.Vertices = new ArcPathMakerVertex[i];
                var r = maker.Compute();
                Assert.Null(r);
            }
        }


        [Fact]
        public void T20_001_Should_create_line()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0),
                    new ArcPathMakerVertex(100, 0)
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(20_001, "Line"),
                Result = result
            }.Draw(maker);

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(100, 0, pathResult.End);
            Assert.Single(pathResult.Elements);
            var line = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(0, 0, 100, 0, line, 6);

            #endregion
        }

        [Fact]
        public void T20_002_Should_create_arc_with_starting_vector()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0).WithOutVector(0, 1),
                    new ArcPathMakerVertex(24, 100)
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(20_002, "Starting vector"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(24, 100, pathResult.End);
            Assert.Equal(2, pathResult.Elements.Count);
            var arc = (ArcDefinition)pathResult.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(23.0276006797085, arc.Angle, 6);
            AssertEx.Equal(182.157302401683, 0, arc.Center);
            AssertEx.Equal(0, 0, arc.Start);
            AssertEx.Equal(14.5149272916306, 71.2552937382106, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(9.53186739891266, arc.Angle, 6);
            AssertEx.Equal(-153.127447818422, 142.510587476421, arc.Center);
            AssertEx.Equal(14.5149272916306, 71.2552937382106, arc.Start);
            AssertEx.Equal(24, 100, arc.End);
            AssertEx.Equal(71.2552937382106, 167.642375110052, arc.DirectionStart);

            #endregion
        }

        [Fact]
        public void T20_003_Should_create_arc_with_ending_vector()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0),
                    new ArcPathMakerVertex(24, 100).WithInVector(0, 1)
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(20_003, "Ending vector"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(24, 100, pathResult.End);
            Assert.Equal(2, pathResult.Elements.Count);
            var arc = (ArcDefinition)pathResult.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(9.53186739891265, arc.Angle, 6);
            AssertEx.Equal(177.127447818422, -42.5105874764212, arc.Center);
            AssertEx.Equal(0, 0, arc.Start);
            AssertEx.Equal(9.48507270836939, 28.7447062617894, arc.End);
            AssertEx.Equal(0.233372952475324, 0.972387301980517, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(23.0276006797085, arc.Angle, 6);
            AssertEx.Equal(-158.157302401683, 100, arc.Center);
            AssertEx.Equal(9.48507270836939, 28.7447062617894, arc.Start);
            AssertEx.Equal(24, 100, arc.End);
            AssertEx.Equal(71.2552937382106, 167.642375110052, arc.DirectionStart);

            #endregion
        }

        [Fact]
        public void T20_004_Should_create_arc_with_both_vectors()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0).WithOutVector(0, 1),
                    new ArcPathMakerVertex(24, 100).WithInVector(0, 1)
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(20_004, "Both vectors"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(24, 100, pathResult.End);
            Assert.Equal(2, pathResult.Elements.Count);
            var arc = (ArcDefinition)pathResult.Elements[0];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(26.9914665615916, arc.Angle, 6);
            AssertEx.Equal(110.166666666667, 0, arc.Center);
            AssertEx.Equal(0, 0, arc.Start);
            AssertEx.Equal(12, 50, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(26.9914665615916, arc.Angle, 6);
            AssertEx.Equal(-86.1666666666667, 100, arc.Center);
            AssertEx.Equal(12, 50, arc.Start);
            AssertEx.Equal(24, 100, arc.End);
            AssertEx.Equal(50, 98.1666666666667, arc.DirectionStart);

            #endregion
        }

        [Fact]
        public void T20_005_Should_create_arc_with_both_vectors_and_arms()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0).WithOutVector(0, 1, 8),
                    new ArcPathMakerVertex(24, 100).WithInVector(0, 1, 23)
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(20_005, "Both vectors and arms"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(24, 100, pathResult.End);
            Assert.Equal(4, pathResult.Elements.Count);
            var line = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(0, 0, 0, 8, line, 6);
            var arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(38.3580160516215, arc.Angle, 6);
            AssertEx.Equal(55.59375, 8, arc.Center);
            AssertEx.Equal(0, 8, arc.Start);
            AssertEx.Equal(12, 42.5, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[2];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(38.3580160516214, arc.Angle, 6);
            AssertEx.Equal(-31.59375, 77, arc.Center);
            AssertEx.Equal(12, 42.5, arc.Start);
            AssertEx.Equal(24, 77, arc.End);
            AssertEx.Equal(34.5, 43.59375, arc.DirectionStart);
            line = (LinePathElement)pathResult.Elements[3];
            AssertEx.Equal(24, 77, 24, 100, line, 6);

            #endregion
        }

        [Fact]
        public void T21_001_Should_create_arc()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0).WithOutVector(0, 1),
                    new ArcPathMakerVertex(24, 100)
                        .WithInVector(0, 1)
                        .WithReferencePoints(new PathRay(3, 50, 0, 0))
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(21_001, "Test"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(24, 100, pathResult.End);
            Assert.Equal(4, pathResult.Elements.Count);
            var line = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(0, 0, 0, 24.6450398678176, line, 6);
            var arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(13.4957332807958, arc.Angle, 6);
            AssertEx.Equal(108.645667217427, 24.6450398678176, arc.Center);
            AssertEx.Equal(0, 24.6450398678176, arc.Start);
            AssertEx.Equal(3, 50, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(26.648817803621, arc.Angle, 6);
            AssertEx.Equal(49.0382984801056, 38.9508083647747, arc.Center);
            AssertEx.Equal(3, 50, arc.Start);
            AssertEx.Equal(12.8463295794419, 69.4754041823873, arc.End);
            AssertEx.Equal(0.233372952475324, 0.972387301980517, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(40.1445510844168, arc.Angle, 6);
            AssertEx.Equal(-23.3456393212218, 100, arc.Center);
            AssertEx.Equal(12.8463295794419, 69.4754041823873, arc.Start);
            AssertEx.Equal(24, 100, arc.End);
            AssertEx.Equal(30.5245958176127, 36.1919689006637, arc.DirectionStart);

            #endregion
        }

        [Fact]
        public void T21_002_Should_create_arc()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0).WithOutVector(0, 1),
                    new ArcPathMakerVertex(24, 100)
                        .WithInVector(1, 1)
                        .WithReferencePoints(new PathRay(3, 50, 0, 0))
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(21_002, "Test"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(24, 100, pathResult.End);
            Assert.Equal(4, pathResult.Elements.Count);
            var line = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(0, 0, 0, 24.6450398678176, line, 6);
            var arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(13.4957332807958, arc.Angle, 6);
            AssertEx.Equal(108.645667217427, 24.6450398678176, arc.Center);
            AssertEx.Equal(0, 24.6450398678176, arc.Start);
            AssertEx.Equal(3, 50, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            line = (LinePathElement)pathResult.Elements[2];
            AssertEx.Equal(3, 50, 8.2495358548225, 71.8730660617604, line, 6);
            arc = (ArcDefinition)pathResult.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(31.5042667192042, arc.Angle, 6);
            AssertEx.Equal(65.9829543856344, 58.0170456143655, arc.Center);
            AssertEx.Equal(8.2495358548225, 71.8730660617604, arc.Start);
            AssertEx.Equal(24, 100, arc.End);
            AssertEx.Equal(0.233372952475324, 0.972387301980517, arc.DirectionStart);

            #endregion
        }

        [Fact]
        public void T21_003_Should_create_arc_with_both_vectors_and_arms()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0).WithOutVector(0, 1, 8),
                    new ArcPathMakerVertex(24, 100).WithInVector(0, 1, 13)
                        .WithReferencePoints(new PathRay(-3, 50, 0, 0))
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(21_003, "Both vectors and arms"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(24, 100, pathResult.End);
            Assert.Equal(6, pathResult.Elements.Count);
            var line = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(0, 0, 0, 8, line, 6);
            var arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(19.1644610646904, arc.Angle, 6);
            AssertEx.Equal(-44.3392850337163, 8, arc.Center);
            AssertEx.Equal(0, 8, arc.Start);
            AssertEx.Equal(-2.45727541086164, 22.5557365753935, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(36.0631097587287, arc.Angle, 6);
            AssertEx.Equal(39.424734211993, 37.1114731507869, arc.Center);
            AssertEx.Equal(-2.45727541086164, 22.5557365753935, arc.Start);
            AssertEx.Equal(-3, 50, arc.End);
            AssertEx.Equal(-14.5557365753935, 41.8820096228546, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(48.0542648883617, arc.Angle, 6);
            AssertEx.Equal(20.2720279459373, 42.9300168265507, arc.Center);
            AssertEx.Equal(-3, 50, arc.Start);
            AssertEx.Equal(9.97488792735364, 64.9650084132753, arc.End);
            AssertEx.Equal(0.290679627319485, 0.956820439926638, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[4];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(64.9529135823999, arc.Angle, 6);
            AssertEx.Equal(-0.322252091230002, 87, arc.Center);
            AssertEx.Equal(9.97488792735364, 64.9650084132753, arc.Start);
            AssertEx.Equal(24, 87, arc.End);
            AssertEx.Equal(22.0349915867246, 10.2971400185836, arc.DirectionStart);
            line = (LinePathElement)pathResult.Elements[5];
            AssertEx.Equal(24, 87, 24, 100, line, 6);

            #endregion
        }

        [Fact]
        public void T22_001_Should_create_arc_with_both_vectors()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0).WithOutVector(0, 1),
                    new ArcPathMakerVertex(24, 100).WithInVector(0, 1)
                        .WithReferencePoints(new PathRay(-4, 45, 0, 0), new PathRay(-4, 55, 0, 0))
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(22_001, "Both vectors"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(24, 100, pathResult.End);
            Assert.Equal(5, pathResult.Elements.Count);
            var arc = (ArcDefinition)pathResult.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(10.1592157200291, arc.Angle, 6);
            AssertEx.Equal(-127.5625, 0, arc.Center);
            AssertEx.Equal(0, 0, arc.Start);
            AssertEx.Equal(-2, 22.5, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(10.1592157200291, arc.Angle, 6);
            AssertEx.Equal(123.5625, 45, arc.Center);
            AssertEx.Equal(-2, 22.5, arc.Start);
            AssertEx.Equal(-4, 45, arc.End);
            AssertEx.Equal(-22.5, 125.5625, arc.DirectionStart);
            var line = (LinePathElement)pathResult.Elements[2];
            AssertEx.Equal(-4, 45, -4, 55, line, 6);
            arc = (ArcDefinition)pathResult.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(63.7815836036914, arc.Angle, 6);
            AssertEx.Equal(21.0803571428571, 55, arc.Center);
            AssertEx.Equal(-4, 55, arc.Start);
            AssertEx.Equal(10, 77.5, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[4];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(63.7815836036914, arc.Angle, 6);
            AssertEx.Equal(-1.08035714285714, 100, arc.Center);
            AssertEx.Equal(10, 77.5, arc.Start);
            AssertEx.Equal(24, 100, arc.End);
            AssertEx.Equal(22.5, 11.0803571428571, arc.DirectionStart);

            #endregion
        }


        [Fact]
        public void T22_002_Should_create_arc_with_both_vectors_and_arms()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0).WithOutVector(0, 1, 8),
                    new ArcPathMakerVertex(24, 100).WithInVector(0, 1, 13)
                        .WithReferencePoints(new PathRay(-4, 45, 0, 0), new PathRay(-4, 55, 0, 0))
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(22_002, "Both vectors and arms"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(24, 100, pathResult.End);
            Assert.Equal(7, pathResult.Elements.Count);
            var line = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(0, 0, 0, 8, line, 6);
            var arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(12.3403501900592, arc.Angle, 6);
            AssertEx.Equal(-86.5625, 8, arc.Center);
            AssertEx.Equal(0, 8, arc.Start);
            AssertEx.Equal(-2, 26.5, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(12.3403501900592, arc.Angle, 6);
            AssertEx.Equal(82.5625, 45, arc.Center);
            AssertEx.Equal(-2, 26.5, arc.Start);
            AssertEx.Equal(-4, 45, arc.End);
            AssertEx.Equal(-18.5, 84.5625, arc.DirectionStart);
            line = (LinePathElement)pathResult.Elements[3];
            AssertEx.Equal(-4, 45, -4, 55, line, 6);
            arc = (ArcDefinition)pathResult.Elements[4];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(82.3718503314193, arc.Angle, 6);
            AssertEx.Equal(12.1428571428571, 55, arc.Center);
            AssertEx.Equal(-4, 55, arc.Start);
            AssertEx.Equal(10, 71, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[5];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(82.3718503314193, arc.Angle, 6);
            AssertEx.Equal(7.85714285714286, 87, arc.Center);
            AssertEx.Equal(10, 71, arc.Start);
            AssertEx.Equal(24, 87, arc.End);
            AssertEx.Equal(16, 2.14285714285714, arc.DirectionStart);
            line = (LinePathElement)pathResult.Elements[6];
            AssertEx.Equal(24, 87, 24, 100, line, 6);

            #endregion
        }


        [Fact]
        public void T23_001_Should_create_arc_with_both_vectors()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0).WithOutVector(0, 1),
                    new ArcPathMakerVertex(24, 100).WithInVector(0, 1)
                        .WithReferencePoints(new PathRay(-4, 30, 0, 1), new PathRay(-4, 46, 0.4, 1),
                            new PathRay(-4, 59, 0, 1))
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(23_001, "Both vectors"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(24, 100, pathResult.End);
            Assert.Equal(8, pathResult.Elements.Count);
            var arc = (ArcDefinition)pathResult.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(15.1892867371829, arc.Angle, 6);
            AssertEx.Equal(-57.25, 0, arc.Center);
            AssertEx.Equal(0, 0, arc.Start);
            AssertEx.Equal(-2, 15, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(15.1892867371829, arc.Angle, 6);
            AssertEx.Equal(53.25, 30, arc.Center);
            AssertEx.Equal(-2, 15, arc.Start);
            AssertEx.Equal(-4, 30, arc.End);
            AssertEx.Equal(-15, 55.25, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[2];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(15.369129137677, arc.Angle, 6);
            AssertEx.Equal(-21.7489076300842, 30, arc.Center);
            AssertEx.Equal(-4, 30, arc.Start);
            AssertEx.Equal(-4.63473030343775, 34.7041105953583, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(37.1705386240288, arc.Angle, 6);
            AssertEx.Equal(12.4794470232087, 39.4082211907165, arc.Center);
            AssertEx.Equal(-4.63473030343775, 34.7041105953583, arc.Start);
            AssertEx.Equal(-4, 46, arc.End);
            AssertEx.Equal(-4.70411059535825, 17.1141773266465, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[4];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(37.1705386240288, arc.Angle, 6);
            AssertEx.Equal(-17.3895507063571, 51.3558202825428, arc.Center);
            AssertEx.Equal(-4, 46, arc.Start);
            AssertEx.Equal(-3.48428162845683, 55.1779101412714, arc.End);
            AssertEx.Equal(0.371390676354104, 0.928476690885259, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[5];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(15.369129137677, arc.Angle, 6);
            AssertEx.Equal(10.4209874494435, 59, arc.Center);
            AssertEx.Equal(-3.48428162845683, 55.1779101412714, arc.Start);
            AssertEx.Equal(-4, 59, arc.End);
            AssertEx.Equal(-3.82208985872858, 13.9052690779003, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[6];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(68.6604343910067, arc.Angle, 6);
            AssertEx.Equal(18.0089285714286, 59, arc.Center);
            AssertEx.Equal(-4, 59, arc.Start);
            AssertEx.Equal(10, 79.5, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[7];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(68.6604343910066, arc.Angle, 6);
            AssertEx.Equal(1.99107142857143, 100, arc.Center);
            AssertEx.Equal(10, 79.5, arc.Start);
            AssertEx.Equal(24, 100, arc.End);
            AssertEx.Equal(20.5, 8.00892857142857, arc.DirectionStart);

            #endregion
        }


        [Fact]
        public void T23_002_Should_create_arc_with_both_vectors_and_arms()
        {
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0).WithOutVector(0, 1, 8),
                    new ArcPathMakerVertex(24, 100).WithInVector(0, 1, 13)
                        .WithReferencePoints(
                            new PathRay(-4, 30, 0, 1),
                            new PathRay(-4, 46, 0.4, 1),
                            new PathRay(-4, 59, 0, 1))
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(23_002, "Both vectors and arms"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(24, 100, pathResult.End);
            Assert.Equal(10, pathResult.Elements.Count);
            var line = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(0, 0, 0, 8, line, 6);
            var arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(20.6096929375321, arc.Angle, 6);
            AssertEx.Equal(-31.25, 8, arc.Center);
            AssertEx.Equal(0, 8, arc.Start);
            AssertEx.Equal(-2, 19, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(20.6096929375321, arc.Angle, 6);
            AssertEx.Equal(27.25, 30, arc.Center);
            AssertEx.Equal(-2, 19, arc.Start);
            AssertEx.Equal(-4, 30, arc.End);
            AssertEx.Equal(-11, 29.25, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(15.369129137677, arc.Angle, 6);
            AssertEx.Equal(-21.7489076300842, 30, arc.Center);
            AssertEx.Equal(-4, 30, arc.Start);
            AssertEx.Equal(-4.63473030343775, 34.7041105953583, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[4];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(37.1705386240288, arc.Angle, 6);
            AssertEx.Equal(12.4794470232087, 39.4082211907165, arc.Center);
            AssertEx.Equal(-4.63473030343775, 34.7041105953583, arc.Start);
            AssertEx.Equal(-4, 46, arc.End);
            AssertEx.Equal(-4.70411059535825, 17.1141773266465, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[5];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(37.1705386240288, arc.Angle, 6);
            AssertEx.Equal(-17.3895507063571, 51.3558202825428, arc.Center);
            AssertEx.Equal(-4, 46, arc.Start);
            AssertEx.Equal(-3.48428162845683, 55.1779101412714, arc.End);
            AssertEx.Equal(0.371390676354104, 0.928476690885259, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[6];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(15.369129137677, arc.Angle, 6);
            AssertEx.Equal(10.4209874494435, 59, arc.Center);
            AssertEx.Equal(-3.48428162845683, 55.1779101412714, arc.Start);
            AssertEx.Equal(-4, 59, arc.End);
            AssertEx.Equal(-3.82208985872858, 13.9052690779003, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[7];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(90, arc.Angle, 6);
            AssertEx.Equal(10, 59, arc.Center);
            AssertEx.Equal(-4, 59, arc.Start);
            AssertEx.Equal(10, 73, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[8];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(90, arc.Angle, 6);
            AssertEx.Equal(10, 87, arc.Center);
            AssertEx.Equal(10, 73, arc.Start);
            AssertEx.Equal(24, 87, arc.End);
            AssertEx.Equal(14, 0, arc.DirectionStart);
            line = (LinePathElement)pathResult.Elements[9];
            AssertEx.Equal(24, 87, 24, 100, line, 6);

            #endregion
        }
        
        
        [Fact]
        public void T24_001_Should_compute_with_4_arms()
        {
            var wayPoint = new WayPoint(
                new Point(25, 40),
                new Vector(1, -1),
                new Vector(1, 1),
                13, 3);
            var maker = new ArcPathMaker
            {
                Vertices = new[]
                {
                    new ArcPathMakerVertex(0, 0).WithOutVector(0, 1, 5),
                    new ArcPathMakerVertex(50, 5).WithInVector(0, -1, 10)
                        .WithReferencePoints(
                            wayPoint)
                             
                }
            };
            var result = maker.Compute();
            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(24_001, "4 arms"),
                Result = result
            }.Draw(maker);
            var code = new DpAssertsBuilder().Create(result, nameof(result));
            
            #region Asserts
            Assert.Single(result.Segments);
            var pathResult = result.Segments[0];
            AssertEx.Equal(0, 0, pathResult.Start);
            AssertEx.Equal(50, 5, pathResult.End);
            Assert.Equal(7, pathResult.Elements.Count);
            var line = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(0, 0, 0, 5, line, 6);
            var arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(45, arc.Angle, 6);
            Assert.Equal(24.142135623731, arc.Radius, 6);
            AssertEx.Equal(24.142135623731, 5, arc.Center);
            AssertEx.Equal(0, 5, arc.Start);
            AssertEx.Equal(7.07106781186548, 22.0710678118655, arc.End);
            AssertEx.Equal(0, 1, arc.DirectionStart);
            line = (LinePathElement)pathResult.Elements[2];
            AssertEx.Equal(7.07106781186548, 22.0710678118655, 25, 40, line, 6);
            line = (LinePathElement)pathResult.Elements[3];
            AssertEx.Equal(25, 40, 27.1213203435596, 37.8786796564404, line, 6);
            arc = (ArcDefinition)pathResult.Elements[4];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(31.3997148099191, arc.Angle, 6);
            Assert.Equal(18.498092996101, arc.Radius, 6);
            AssertEx.Equal(40.2014473401221, 50.9588066530028, arc.Center);
            AssertEx.Equal(27.1213203435596, 37.8786796564404, arc.Start);
            AssertEx.Equal(35.8516771720105, 32.9794033265014, arc.End);
            AssertEx.Equal(0.707106781186547, -0.707106781186547, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[5];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(76.399714809919, arc.Angle, 6);
            Assert.Equal(18.498092996101, arc.Radius, 6);
            AssertEx.Equal(31.501907003899, 15, arc.Center);
            AssertEx.Equal(35.8516771720105, 32.9794033265014, arc.Start);
            AssertEx.Equal(50, 15, arc.End);
            AssertEx.Equal(17.9794033265014, -4.34977016811156, arc.DirectionStart);
            line = (LinePathElement)pathResult.Elements[6];
            AssertEx.Equal(50, 15, 50, 5, line, 6);
            #endregion


        }

        [Fact]
        public void T99a_Should_compute_practical_case()
        {
            const string Json = @"
[
{""Location"":""48.6807545920352,42.764462268262"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0},
{""Location"":""147.159706379571,68.6649958308797"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0},
{""Location"":""210.045000453121,51.5032938674065"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0},
{""Location"":""224.717683004725,105.268122033596"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0}
]";
            var input = JsonConvert.DeserializeObject<List<ArcPathMakerVertex>>(Json);
            var wayPoint =
                JsonConvert.DeserializeObject<PathRay>(
                    @"{""Vector"":""0,0"",""Point"":""96.13121924784,59.8634246985647""}");
            input[1].ReferencePoints = new[] { (WayPoint)wayPoint };
            // 

            var maker = new ArcPathMaker
            {
                Vertices = input
            };
            var result = maker.Compute();

            new ArcResultDrawerConfig
            {
                Title  = MakeTitle(99, "Practical case A"),
                Result = result
            }.Draw(maker);

            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Equal(3, result.Segments.Count);
            var pathResult = result.Segments[0];
            AssertEx.Equal(48.6807545920352, 42.764462268262, pathResult.Start);
            AssertEx.Equal(147.159706379571, 68.6649958308797, pathResult.End);
            Assert.Equal(4, pathResult.Elements.Count);
            var arc = (ArcDefinition)pathResult.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(10.1628320353855, arc.Angle, 6);
            Assert.Equal(142.363671286863, arc.Radius, 6);
            AssertEx.Equal(12.4697414496494, 180.445896853598, arc.Center);
            AssertEx.Equal(48.6807545920352, 42.764462268262, arc.Start);
            AssertEx.Equal(72.4059869199376, 51.3139434834134, arc.End);
            AssertEx.Equal(0.967110733663976, 0.25435571318908, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(10.1628320353855, arc.Angle, 6);
            Assert.Equal(142.363671286863, arc.Radius, 6);
            AssertEx.Equal(132.342232390226, -77.8180098867709, arc.Center);
            AssertEx.Equal(72.4059869199376, 51.3139434834134, arc.Start);
            AssertEx.Equal(96.13121924784, 59.8634246985647, arc.End);
            AssertEx.Equal(129.131953370184, 59.9362454702882, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(9.89825381378978, arc.Angle, 6);
            Assert.Equal(150.055859329634, arc.Radius, 6);
            AssertEx.Equal(134.298784365829, -85.2572075082957, arc.Center);
            AssertEx.Equal(96.13121924784, 59.8634246985647, arc.Start);
            AssertEx.Equal(121.645462813705, 64.2642102647222, arc.End);
            AssertEx.Equal(0.967110733663976, 0.25435571318908, arc.DirectionStart);
            arc = (ArcDefinition)pathResult.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(9.89825381378978, arc.Angle, 6);
            Assert.Equal(150.055859329634, arc.Radius, 6);
            AssertEx.Equal(108.992141261582, 213.78562803774, arc.Center);
            AssertEx.Equal(121.645462813705, 64.2642102647222, arc.Start);
            AssertEx.Equal(147.159706379571, 68.6649958308797, arc.End);
            AssertEx.Equal(149.521417773018, 12.6533215521237, arc.DirectionStart);
            pathResult = result.Segments[1];
            AssertEx.Equal(147.159706379571, 68.6649958308797, pathResult.Start);
            AssertEx.Equal(210.045000453121, 51.5032938674065, pathResult.End);
            Assert.Single(pathResult.Elements);
            var line = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(147.159706379571, 68.6649958308797, 210.045000453121, 51.5032938674065, line, 6);
            pathResult = result.Segments[2];
            AssertEx.Equal(210.045000453121, 51.5032938674065, pathResult.Start);
            AssertEx.Equal(224.717683004725, 105.268122033596, pathResult.End);
            Assert.Single(pathResult.Elements);
            line = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(210.045000453121, 51.5032938674065, 224.717683004725, 105.268122033596, line, 6);

            #endregion
        }
    }
}
