using System.Linq;
using Newtonsoft.Json;
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
    public class PathBuilderTest
    {
        [Fact]
        public void T01_Should_replace_strange_circle_with_line()
        {
            const string a1 = @"{
  ""Vector"": ""0.941303233494752,0.33756217592071"",
  ""Point"": ""30.95071298,39.21376973"",
  ""ArmLength"": 0.0
}";
            const string a2 = @"{
  ""Vector"": ""1.88260646693512,0.675124351993066"",
  ""Point"": ""63.2545531,51.08514872""
}
";
            var start      = JsonConvert.DeserializeObject<PathRayWithArm>(a1);
            var reference1 = JsonConvert.DeserializeObject<PathRayWithArm>(a2);

            var pb = new PathBuilder(start.Point, new MyValidator(CircleCrossValidationResult.ForceLine));
            pb.AddConnectionAutomatic(start, reference1, false);
            var list = pb.List;
            Assert.Single(list);
            Assert.True(list.Single() is LinePathElement);
        }

        [Fact]
        public void T02_Should_replace_strange_circle_with_line()
        {
            const string a1 = @"{
  ""Vector"": ""0.941303233494752,0.33756217592071"",
  ""Point"": ""30.95071298,39.21376973"",
  ""ArmLength"": 0.0
}";
            const string a2 = @"{
  ""Vector"": ""1.88260646693512,0.675124351993066"",
  ""Point"": ""63.2545531,51.08514872""
}
";
            var start      = JsonConvert.DeserializeObject<PathRayWithArm>(a1);
            var reference1 = JsonConvert.DeserializeObject<PathRayWithArm>(a2);

            var pb = new PathBuilder(start.Point, new MyValidator(CircleCrossValidationResult.Invalid));
            pb.AddConnectionAutomatic(start, reference1, false);
            var list = pb.List;
            Assert.Equal(2, list.Count);
            var e1 = new ArcDefinition(new Point(-339.270603461039, 1071.58809106537),
                new Point(30.95071298, 39.21376973), new Vector(0.941303233494752, 0.33756217592071),
                new Point(47.1026330815796, 45.149459239911));
            var a = (ArcDefinition)list[0];
            Assert.Equal(e1.Radius, a.Radius, 9);
            AssertEx.Equal(e1.Start, a.Start, 9);
            AssertEx.Equal(e1.End, a.End, 9);
            
            
            e1 = new ArcDefinition(new Point(433.475869624198, -981.289172585547),
                new Point(47.1026330815796, 45.149459239911), new Vector(1026.43863182546, 386.373236542618),
                new Point(63.2545531, 51.08514872));
            a = (ArcDefinition)list[1];
            Assert.Equal(e1.Radius, a.Radius, 9);
            AssertEx.Equal(e1.Start, a.Start, 9);
            AssertEx.Equal(e1.End, a.End, 9);
        }

        public class MyValidator : IPathValidator
        {
            private readonly double _maxArc;

            public MyValidator(CircleCrossValidationResult reject, double maxArc = 370)
            {
                _maxArc = maxArc;
                Reject       = reject;
            }

            public ArcValidationResult ValidateArc(ArcDefinition arc, ArcDestination arcDestination)
            {
                if (arc.Angle > 350)
                {
                    if (arc.Sagitta < 1e-3)
                        return ArcValidationResult.ReplaceByLine;
                }

                if (arcDestination.IsOne)
                    return ArcValidationResult.Ok;
                if (arc.Angle>_maxArc)
                    return ArcValidationResult.ReplaceByLine;

                return ArcValidationResult.Ok;
            }

            public LineValidationResult ValidateLine(Vector vector)
            {
                return LineValidationResult.Ok;
            }

            public CircleCrossValidationResult ValidatePointForCircleConnectionValid(PathRay start, PathRay end,
                Point cross)
            {
                var v = end.Point - start.Point;
                var l = v.Length;
                if (l < 1e-6)
                    return CircleCrossValidationResult.ForceLine;

                const double limit = 10_000;

                var v1 = cross - start.Point;
                var t1 = v1.Length / l;
                if (t1 > limit)
                    return Reject;

                var v2 = cross - end.Point;
                var t2 = v2.Length / l;
                return t2 > limit ? Reject : CircleCrossValidationResult.Ok;
            }

            #region properties

            public CircleCrossValidationResult Reject { get; }

            #endregion
        }
    }
}
