using Newtonsoft.Json;
using Xunit;
using Vector=iSukces.Mathematics.Vector;

namespace iSukces.DrawingPanel.Paths.Test;

public partial class ArcPathMakerTests
{
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
        input[1].ReferencePoints = [(WayPoint)wayPoint];
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

    [Fact]
    public void T99b_Should_compute_practical_case()
    {
        const string Json = @"
[{""Location"":""6551522.34418533,5574146.66044663"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""-0.255467483913962,0.966817648092271"",""OutArmLength"":0.0,""Flags"":2,""ReferencePoints"":null},
{""Location"":""6551521.37233942,5574150.33840111"",""InVector"":""-0.255467483913962,0.966817648092271"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":1,
""ReferencePoints"":[{""Vector"":""0.123898409794026,1.99615860693746"",""Point"":""6551521.1209083,5574148.69763847"",""UseInputVector"":false,""InputVector"":""0,0"",""InputArmLength"":0.0,""OutputArmLength"":0.0,""InputRay"":{""Vector"":""0.123898409794026,1.99615860693746"",""Point"":""6551521.1209083,5574148.69763847"",""ArmLength"":0.0},""OutputRay"":{""Vector"":""0.123898409794026,1.99615860693746"",""Point"":""6551521.1209083,5574148.69763847"",""ArmLength"":0.0},""Tag"":{""OriginalTag"":{""Point"":{""$id"":""1"",""Uid"":""635305f6283544509409194818840029"",""Location"":""6551521.1209083,5574148.69763847""},""Direction"":""0.123898409794026,1.99615860693746""},""LeftRightMove"":0.0}}]}]";
        var input = JsonConvert.DeserializeObject<List<ArcPathMakerVertex>>(Json);

        var offset = new Vector(6551522.34418533, 5574146.66044663);
        input = ArcPathMakerVertex.Move(input, -offset);

        /*var wayPoint =
            JsonConvert.DeserializeObject<PathRay>(
                @"{""Vector"":""0,0"",""Point"":""96.13121924784,59.8634246985647""}");
        input[1].ReferencePoints = new[] { (WayPoint)wayPoint };*/
        // 

        var maker = new ArcPathMaker
        {
            Vertices = input
        };
        var result = maker.Compute();

        new ArcResultDrawerConfig
        {
            Title  = MakeTitle(99, "Practical case B"),
            Result = result
        }.Draw(maker);

        var code = new DpAssertsBuilder().Create(result, nameof(result));

        #region Asserts

        Assert.Single(result.Segments);
        var pathResult = result.Segments[0];
        AssertEx.Equal(0, 0, pathResult.Start);
        AssertEx.Equal(-0.97184590995311737, 3.6779544800519943, pathResult.End);
        Assert.Equal(4, pathResult.Elements.Count);
        var arc = (ArcDefinition)pathResult.Elements[0];
        Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
        Assert.Equal(43.0468080103188, arc.Angle, 6);
        Assert.Equal(1.35834334760244, arc.Radius, 6);
        AssertEx.Equal(-1.3132703206307716, -0.34701255730326264, arc.Center);
        AssertEx.Equal(0, 0, arc.Start);
        AssertEx.Equal(-0.59040648398285644, 0.80301549611215561, arc.End);
        AssertEx.Equal(-0.25546748391396151, 0.96681764809227089, arc.DirectionStart);
        arc = (ArcDefinition)pathResult.Elements[1];
        Assert.Equal(ArcDirection.Clockwise, arc.Direction);
        Assert.Equal(61.3997977267447, arc.Angle, 6);
        Assert.Equal(1.35834334760244, arc.Radius, 6);
        AssertEx.Equal(0.13245735266505898, 1.9530435495275738, arc.Center);
        AssertEx.Equal(-0.59040648398285644, 0.80301549611215561, arc.Start);
        AssertEx.Equal(-1.223277029581368, 2.0371918398886919, arc.End);
        AssertEx.Equal(-1.1500280534154181, 0.72286383664791543, arc.DirectionStart);
        arc = (ArcDefinition)pathResult.Elements[2];
        Assert.Equal(ArcDirection.Clockwise, arc.Direction);
        Assert.Equal(22.1308519699424, arc.Angle, 6);
        Assert.Equal(1.54746931330053, arc.Radius, 6);
        AssertEx.Equal(0.32122006477686305, 1.9413273463271972, arc.Center);
        AssertEx.Equal(-1.223277029581368, 2.0371918398886919, arc.Start);
        AssertEx.Equal(-1.073373243578218, 2.6119768672381194, arc.End);
        AssertEx.Equal(0.06194920489701309, 0.99807930346873142, arc.DirectionStart);
        arc = (ArcDefinition)pathResult.Elements[3];
        Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
        Assert.Equal(40.4838416863683, arc.Angle, 6);
        Assert.Equal(1.54746931330053, arc.Radius, 6);
        AssertEx.Equal(-2.4679665519332992, 3.2826263881490414, arc.Center);
        AssertEx.Equal(-1.073373243578218, 2.6119768672381194, arc.Start);
        AssertEx.Equal(-0.97184590995311737, 3.6779544800519943, arc.End);
        AssertEx.Equal(0.670649520910922, 1.3945933083550812, arc.DirectionStart);

        #endregion

        result = result + offset;
        code   = new DpAssertsBuilder().Create(result, nameof(result));

        #region Asserts

        Assert.Single(result.Segments);
        pathResult = result.Segments[0];
        AssertEx.Equal(6551522.34418533, 5574146.66044663, pathResult.Start);
        AssertEx.Equal(6551521.37233942, 5574150.33840111, pathResult.End);
        Assert.Equal(4, pathResult.Elements.Count);
        arc = (ArcDefinition)pathResult.Elements[0];
        Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
        Assert.Equal(43.0468080103188, arc.Angle, 6);
        Assert.Equal(1.35834334760244, arc.Radius, 6);
        AssertEx.Equal(6551521.03091501, 5574146.3134340728, arc.Center);
        AssertEx.Equal(6551522.34418533, 5574146.66044663, arc.Start);
        AssertEx.Equal(6551521.753778846, 5574147.4634621255, arc.End);
        AssertEx.Equal(-0.25546748391396151, 0.96681764809227089, arc.DirectionStart);
        arc = (ArcDefinition)pathResult.Elements[1];
        Assert.Equal(ArcDirection.Clockwise, arc.Direction);
        Assert.Equal(61.3997977267447, arc.Angle, 6);
        Assert.Equal(1.35834334760244, arc.Radius, 6);
        AssertEx.Equal(6551522.4766426822, 5574148.6134901792, arc.Center);
        AssertEx.Equal(6551521.753778846, 5574147.4634621255, arc.Start);
        AssertEx.Equal(6551521.1209083, 5574148.69763847, arc.End);
        AssertEx.Equal(-1.1500280534154181, 0.72286383664791543, arc.DirectionStart);
        arc = (ArcDefinition)pathResult.Elements[2];
        Assert.Equal(ArcDirection.Clockwise, arc.Direction);
        Assert.Equal(22.1308519699424, arc.Angle, 6);
        Assert.Equal(1.54746931330053, arc.Radius, 6);
        AssertEx.Equal(6551522.6654053945, 5574148.6017739763, arc.Center);
        AssertEx.Equal(6551521.1209083, 5574148.69763847, arc.Start);
        AssertEx.Equal(6551521.2708120868, 5574149.2724234974, arc.End);
        AssertEx.Equal(0.06194920489701309, 0.99807930346873142, arc.DirectionStart);
        arc = (ArcDefinition)pathResult.Elements[3];
        Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
        Assert.Equal(40.4838416863683, arc.Angle, 6);
        Assert.Equal(1.54746931330053, arc.Radius, 6);
        AssertEx.Equal(6551519.8762187781, 5574149.9430730185, arc.Center);
        AssertEx.Equal(6551521.2708120868, 5574149.2724234974, arc.Start);
        AssertEx.Equal(6551521.37233942, 5574150.33840111, arc.End);
        AssertEx.Equal(0.670649520910922, 1.3945933083550812, arc.DirectionStart);

        #endregion
    }

    [Fact]
    public void T99c_Should_compute_practical_case()
    {
        const string Json =
            @"[{""Location"":""6551522.34418533,5574146.66044663"",""InVector"":""0,0"",""InArmLength"":0.0,""OutVector"":""-0.255467483913962,0.966817648092271"",""OutArmLength"":0.0,""Flags"":2,""ReferencePoints"":null},{""Location"":""6551520.96855378,5574157.63177922"",""InVector"":""0.797106533311307,2.02114687021822"",""InArmLength"":0.0,""OutVector"":""0,0"",""OutArmLength"":0.0,""Flags"":1,""ReferencePoints"":[{""Vector"":""-0.966157735325396,1.93225387018174"",""Point"":""6551519.63539108,5574151.63526837"",""UseInputVector"":false,""InputVector"":""0,0"",""InputArmLength"":0.0,""OutputArmLength"":0.0,""InputRay"":{""Vector"":""-0.966157735325396,1.93225387018174"",""Point"":""6551519.63539108,5574151.63526837"",""ArmLength"":0.0},""OutputRay"":{""Vector"":""-0.966157735325396,1.93225387018174"",""Point"":""6551519.63539108,5574151.63526837"",""ArmLength"":0.0},""Tag"":{""OriginalTag"":{""Point"":{""$id"":""1"",""Uid"":""06fdaab10e9a49549bf6ba16f647485b"",""Location"":""6551519.63539108,5574151.63526837""},""Direction"":""-0.966157735325396,1.93225387018174""},""LeftRightMove"":0.0}}]}]";
        var input = JsonConvert.DeserializeObject<List<ArcPathMakerVertex>>(Json);

        var offset = new Vector(6551522.34418533, 5574146.66044663);
        input = ArcPathMakerVertex.Move(input, -offset);


        var maker = new ArcPathMaker
        {
            Vertices         = input,
            GetPathValidator = DefaultPathValidatorPd.Instance.GetValidator
        };
        var result = maker.Compute();

        new ArcResultDrawerConfig
        {
            Title  = MakeTitle(99, "Practical case C"),
            Result = result
        }.Draw(maker);

        var r = new[]
        {
            result,
            result + offset
        };

        var code = new DpAssertsBuilder().Create(result, nameof(result));
 
        #region Asserts
        Assert.Single(result.Segments);
        var pathResult = result.Segments[0];
        AssertEx.Equal(0, 0, pathResult.Start);
        AssertEx.Equal(-1.3756315503269434, 10.971332590095699, pathResult.End);
        Assert.Equal(3, pathResult.Elements.Count);
        var arc = (ArcDefinition)pathResult.Elements[0];
        Assert.Equal(ArcDirection.CounterClockwise, arc.Direction);
        Assert.Equal(36.5916433167106, arc.Angle, 6);
        Assert.Equal(6.13345000374479, arc.Radius, 6);
        AssertEx.Equal(-6.0857985515523678, -0.763062866584416, arc.Center);
        AssertEx.Equal(0, 0, arc.Start);
        AssertEx.Equal(-1.6543516611586613, 3.477395529162072, arc.End);
        AssertEx.Equal(-0.12441005732801715, 0.99223088927710734, arc.DirectionStart);
        arc = (ArcDefinition)pathResult.Elements[1];
        Assert.Equal(ArcDirection.Clockwise, arc.Direction);
        Assert.Equal(17.1725501286194, arc.Angle, 6);
        Assert.Equal(6.13345000374479, arc.Radius, 6);
        AssertEx.Equal(2.7770952292350453, 7.71785392490856, arc.Center);
        AssertEx.Equal(-1.6543516611586613, 3.477395529162072, arc.Start);
        AssertEx.Equal(-2.7087942501530051, 4.9748217398300767, arc.End);
        AssertEx.Equal(-4.2404583957464883, 4.4314468903937065, arc.DirectionStart);
        var line = (LinePathElement)pathResult.Elements[2];
        AssertEx.Equal(-2.7087942501530051, 4.9748217398300767, -1.3756315503269434, 10.971332590095699, line, 6);
        #endregion

    }
}
