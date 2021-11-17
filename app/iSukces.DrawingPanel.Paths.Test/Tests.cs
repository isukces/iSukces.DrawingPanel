using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public class ZeroReferencePointPathCalculatorTests : TestBaseClass
    {
        [Fact]
        public void T01_Should_compute_point()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end = new PathRay(0, 0, 200, 0);
            var c = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.Point, c.Kind);
            Assert.Null(c.Arc1);
            Assert.Null(c.Arc2);
        }


        [Fact]
        public void T02_Should_compute_line()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end   = new PathRay(200, 0, 100, 0);
            var c = (ZeroReferencePointPathCalculatorLineResult)ZeroReferencePointPathCalculator.Compute(start, end,
                null);
        }

        [Fact]
        public void T03_Should_compute_one_circle()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end = new PathRay(200, 20, 100, 100);
            var c = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
            Assert.NotNull(c.Arc1);
            Assert.Null(c.Arc2);

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.OneArc, c.Kind);
            Assert.Equal(151.7157287525381, c.Arc1.Center.X, 8);
            Assert.Equal(68.284271247461902, c.Arc1.Center.Y, 8);
            Assert.Equal(68.284271247461902, c.Arc1.RadiusStart.Length, 8);
        }

        [Fact]
        public void T04_Should_compute_one_circle()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end = new PathRay(10, 20, 100, 100);
            var c = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
            Assert.NotNull(c.Arc1);
            Assert.NotNull(c.Arc2);

            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.TwoArcs, c.Kind);
            {
                var a = c.Arc1;
                Assert.Equal(0, a.Center.X, 8);
                Assert.Equal(8.4604973937154231, a.Center.Y, 8);
                Assert.Equal(0, a.RadiusStart.X, 8);
                Assert.Equal(-8.4604973937154231, a.RadiusStart.Y, 8);
            }

            {
                var a = c.Arc2;
                Assert.Equal(15.982475079307287, a.Center.X, 8);
                Assert.Equal(14.017524920692713, a.Center.Y, 8);
                Assert.Equal(-7.9912375396536435, a.RadiusStart.X, 8);
                Assert.Equal(-2.778513763488645, a.RadiusStart.Y, 8);
            }
        }

        [Fact]
        public void T05a_Should_compute_practical_case()
        {
            const string Json = @"
[
{""Location"":""48.6807545920352,42.764462268262"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0,""ReferencePoints"":null},
{""Location"":""147.159706379571,68.6649958308797"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0,""ReferencePoints"":[{""Vector"":""1,0"",""Point"":""96.13121924784,59.8634246985647""}]},
{""Location"":""210.045000453121,51.5032938674065"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0,""ReferencePoints"":null},
{""Location"":""224.717683004725,105.268122033596"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0,""ReferencePoints"":null}
]";
            var input = JsonConvert.DeserializeObject<List<ArcPathMakerVertex>>(Json);

            var result = new ArcPathMaker
            {
                Vertices = input
            }.Compute();

            var code = new DpAssertsBuilder().Create(result, nameof(result));

            #region Asserts

            Assert.Equal(3, result.Segments.Count);
            var pathResult = result.Segments[0];
            AssertEx.Equal(48.6807545920352, 42.764462268262, pathResult.Start);
            AssertEx.Equal(147.159706379571, 68.6649958308797, pathResult.End);
            Assert.Equal(4, pathResult.Elements.Count);
            var tmp2 = (ArcDefinition)pathResult.Elements[0];
            Assert.Equal(ArcDirection.CounterClockwise, tmp2.Direction);
            Assert.Equal(10.1628320353855, tmp2.Angle, 6);
            AssertEx.Equal(12.4697414496494, 180.445896853598, tmp2.Center);
            AssertEx.Equal(48.6807545920352, 42.764462268262, tmp2.Start);
            AssertEx.Equal(72.4059869199376, 51.3139434834134, tmp2.End);
            AssertEx.Equal(0.967110733663976, 0.25435571318908, tmp2.StartVector);
            tmp2 = (ArcDefinition)pathResult.Elements[1];
            Assert.Equal(ArcDirection.Clockwise, tmp2.Direction);
            Assert.Equal(10.1628320353855, tmp2.Angle, 6);
            AssertEx.Equal(132.342232390226, -77.8180098867709, tmp2.Center);
            AssertEx.Equal(72.4059869199376, 51.3139434834134, tmp2.Start);
            AssertEx.Equal(96.13121924784, 59.8634246985647, tmp2.End);
            AssertEx.Equal(129.131953370184, 59.9362454702882, tmp2.StartVector);
            tmp2 = (ArcDefinition)pathResult.Elements[2];
            Assert.Equal(ArcDirection.Clockwise, tmp2.Direction);
            Assert.Equal(9.89825381378978, tmp2.Angle, 6);
            AssertEx.Equal(134.298784365829, -85.2572075082957, tmp2.Center);
            AssertEx.Equal(96.13121924784, 59.8634246985647, tmp2.Start);
            AssertEx.Equal(121.645462813705, 64.2642102647222, tmp2.End);
            AssertEx.Equal(0.967110733663976, 0.25435571318908, tmp2.StartVector);
            tmp2 = (ArcDefinition)pathResult.Elements[3];
            Assert.Equal(ArcDirection.CounterClockwise, tmp2.Direction);
            Assert.Equal(9.89825381378978, tmp2.Angle, 6);
            AssertEx.Equal(108.992141261582, 213.78562803774, tmp2.Center);
            AssertEx.Equal(121.645462813705, 64.2642102647222, tmp2.Start);
            AssertEx.Equal(147.159706379571, 68.6649958308797, tmp2.End);
            AssertEx.Equal(149.521417773018, 12.6533215521237, tmp2.StartVector);

            pathResult = result.Segments[1];
            AssertEx.Equal(147.159706379571, 68.6649958308797, pathResult.Start);
            AssertEx.Equal(210.045000453121, 51.5032938674065, pathResult.End);
            Assert.Single(pathResult.Elements);
            var tmp3 = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(147.159706379571, 68.6649958308797, 210.045000453121, 51.5032938674065, tmp3, 6);

            pathResult = result.Segments[2];
            AssertEx.Equal(210.045000453121, 51.5032938674065, pathResult.Start);
            AssertEx.Equal(224.717683004725, 105.268122033596, pathResult.End);
            Assert.Single(pathResult.Elements);
            tmp3 = (LinePathElement)pathResult.Elements[0];
            AssertEx.Equal(210.045000453121, 51.5032938674065, 224.717683004725, 105.268122033596, tmp3, 6);

            #endregion
        }

        [Fact]
        public void T05b_Should_compute_practical_case()
        {
            const string Json = @"{
  ""StartCenterSearch"": {
    ""Vector"": ""0.25435571318907962,-0.96711073366397649"",
    ""Point"": ""48.577740528193623,43.156142115395909""
  },
  ""EndCenterSearch"": {
    ""Vector"": ""-0.25435571318907968,0.96711073366397649"",
    ""Point"": ""96.07998471088365,60.26872286057305""
  },
  ""EndDirection"": ""0.96711073366397649,0.25435571318907968"",
  ""StartDirection"": ""0.96711073366397649,0.25435571318907962""
}";

            var finder = JsonConvert.DeserializeObject<TwoArcsFinder>(Json);

            var isOk = finder.Compute(out var arc1, out var arc2, true);
            Assert.True(isOk);
            var code = new DpAssertsBuilder().Create(arc1, nameof(arc1));

            #region Asserts

            Assert.Equal(ArcDirection.CounterClockwise, arc1.Direction);
            Assert.Equal(10.1520694559206, arc1.Angle, 6);
            AssertEx.Equal(12.2901109624214, 181.128887497343, arc1.Center);
            AssertEx.Equal(48.5777405281936, 43.1561421153959, arc1.Start);
            AssertEx.Equal(72.3288626195387, 51.7124324879845, arc1.End);
            AssertEx.Equal(0.967110733663976, 0.25435571318908, arc1.StartVector);

            #endregion

            code = new DpAssertsBuilder().Create(arc2, nameof(arc2));

            #region Asserts

            Assert.Equal(ArcDirection.Clockwise, arc2.Direction);
            Assert.Equal(10.1520694559206, arc2.Angle, 6);
            AssertEx.Equal(132.367614276656, -77.7040225213743, arc2.Center);
            AssertEx.Equal(72.3288626195387, 51.7124324879845, arc2.Start);
            AssertEx.Equal(96.0799847108837, 60.2687228605731, arc2.End);
            AssertEx.Equal(129.416455009359, 60.0387516571173, arc2.StartVector);

            #endregion
        }




        [Fact]
        public void T05d_Should_compute_practical_case()
        {
            var o = new OneReferencePointPathCalculator
            {
                Start = new PathRay(48.426398878846122,
                    43.73157300192598,
                    0.96711073366397649,
                    0.25435571318907962),
                End = new PathRay(147.16448720645866,
                    69.700260972482937,
                    -0.96711073366397649,
                    -0.25435571318907962),
                Reference = new PathRay(110.61711161799218, 49.932980045672636, 0, 0)
            };

            var result = o.Compute(null);
            var code   = new DpAssertsBuilder().Create(result, nameof(result));

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
            AssertEx.Equal(0.967110733663976, 0.25435571318908, arc.StartVector);
            arc = (ArcDefinition)result.Elements[1];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(18.0818631776401, arc.Angle, 6);
            AssertEx.Equal(85.3259530629415, 146.094967518723, arc.Center);
            AssertEx.Equal(79.5217552484192, 46.8322765237993, arc.Start);
            AssertEx.Equal(110.617111617992, 49.9329800456726, arc.End);
            AssertEx.Equal(99.2626909949237, -5.80419781452231, arc.StartVector);
            arc = (ArcDefinition)result.Elements[2];
            Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
            Assert.Equal(27.3441904745793, arc.Angle, 6);
            AssertEx.Equal(99.4388002168958, 92.4351310872041, arc.Center);
            AssertEx.Equal(110.617111617992, 49.9329800456726, arc.Start);
            AssertEx.Equal(128.890799412225, 59.8166205090778, arc.End);
            AssertEx.Equal(0.967110733663976, 0.25435571318908, arc.StartVector);
            arc = (ArcDefinition)result.Elements[3];
            Assert.Equal(ArcDirection.Clockwise, arc.Direction);
            Assert.Equal(27.3441904745794, arc.Angle, 6);
            AssertEx.Equal(158.342798607555, 27.1981099309515, arc.Center);
            AssertEx.Equal(128.890799412225, 59.8166205090778, arc.Start);
            AssertEx.Equal(147.164487206459, 69.7002609724829, arc.End);
            AssertEx.Equal(32.6185105781263, 29.4519991953296, arc.StartVector);
            #endregion

        }
    }
}
