using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public class ZeroReferencePointPathCalculatorTests
    {
        [Fact]
        public void T01_Should_compute_point()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end   = new PathRay(0, 0, 200, 0);
            var c     = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
            Assert.Equal(ZeroReferencePointPathCalculator.ResultKind.Point, c.Kind);
            Assert.Null(c.Arc1);
            Assert.Null(c.Arc2);
        }


        [Fact]
        public void T02_Should_compute_line()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end   = new PathRay(200, 0, 100, 0);
            var c     = (ZeroReferencePointPathCalculatorLineResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
        }

        [Fact]
        public void T03_Should_compute_one_circle()
        {
            var start = new PathRay(0, 0, 100, 0);
            var end   = new PathRay(200, 20, 100, 100);
            var c     = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
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
            var end   = new PathRay(10, 20, 100, 100);
            var c     = (ZeroReferencePointPathCalculatorResult)ZeroReferencePointPathCalculator.Compute(start, end, null);
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
        public void T05_Practical()
        {
            const string Json = @"
[
{""Location"":""48.6807545920352,42.764462268262"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0,""ReferencePoints"":null},
{""Location"":""147.159706379571,68.6649958308797"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0,""ReferencePoints"":[{""Vector"":""1,0"",""Point"":""96.13121924784,59.8634246985647""}]},
{""Location"":""210.045000453121,51.5032938674065"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0,""ReferencePoints"":null},
{""Location"":""224.717683004725,105.268122033596"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":0,""ReferencePoints"":null}
]";
            var input = JsonConvert.DeserializeObject<List<ArcPathMakerVertex>>(Json);
            
            var pipePoints = new ArcPathMaker
            {
                Vertices = input
            }.Compute();
        }
    }
}
