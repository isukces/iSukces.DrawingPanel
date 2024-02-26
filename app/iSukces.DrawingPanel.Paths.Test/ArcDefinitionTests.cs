using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using iSukces.Mathematics;
using Xunit;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif


namespace iSukces.DrawingPanel.Paths.Test;

public partial class ArcDefinitionTests
{
    [Theory]
    [InlineData(1, ArcDirection.CounterClockwise)]
    [InlineData(-1, ArcDirection.Clockwise)]
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
        var arc = new ArcDefinition(
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
        var arc = new ArcDefinition(
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
        var drawer = new ResultDrawerBase();

        var arc = new ArcDefinition(new Point(54.8912687238947, 91.7723295871598),
            new Point(29.4845072800563, 0.552564737714022), new Vector(0.963332660388057, -0.268309868304668),
            new Point(146.242441507818, 66.8421820293499));
        var p1 = new Point(30.9478964807321, 4.29722988260286);
        var p2 = new Point(29.7659915108501, 0.0537495135934687);

        void Draw()
        {
            drawer.DrawArc(arc);
            drawer.DrawLine(new Pen(Color.Blue), p1, p2);

            drawer.DrawCross(arc.Start, Color.Red, 2);
            drawer.DrawCross(p2, Color.Red, 2);
            drawer.DrawCross(p1, Color.Gold, 2);
        }

        IEnumerable<Point> RangePoints()
        {
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
            [ArcDefinitionProperties.Direction] =
                ArcDefinitionProperties.RadiusStart | ArcDefinitionProperties.DirectionStart,
            [ArcDefinitionProperties.Angle] = ArcDefinitionProperties.RadiusStart |
                                              ArcDefinitionProperties.RadiusEnd | ArcDefinitionProperties.Direction,
            [ArcDefinitionProperties.Sagitta] = ArcDefinitionProperties.Start | ArcDefinitionProperties.End |
                                                ArcDefinitionProperties.Radius,
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

        ArcDefinitionProperties Dependent(ArcDefinitionProperties property,
            ArcDefinitionProperties starting = ArcDefinitionProperties.None)
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


    [Fact]
    public void T51a_Should_generate_points()
    {
        var v1     = MathEx.GetCosSinV(90 - 20) * 10;
        var v2     = MathEx.GetCosSinV(90 + 20) * 10;
        var center = new Point(7, -3);
        var arc = new ArcDefinition
        {
            Center         = center,
            Start          = center + v1,
            End            = center + v2,
            DirectionStart = v1.GetPrependicular().NormalizeFast()
        };
        arc.UpdateRadiusVectors();

        var points = arc.GetSmallAnglePoints(10);
        foreach (var i in points)
        {
            var dist = Math.Abs((i - center).Length - 10);
        }

        var aaa  = string.Join("\r\n", points.Select(x => x.X + "\t" + x.Y));
        var code = new DpAssertsBuilder().Create(points, nameof(points));

        #region Asserts

        Assert.Equal(11, points.Count);
        var tmp1 = points[0];
        AssertEx.Equal(10.4202014332567, 6.39692620785908, tmp1);
        tmp1 = points[1];
        AssertEx.Equal(9.73616114660535, 6.61838979142597, tmp1);
        tmp1 = points[2];
        AssertEx.Equal(9.05212085995401, 6.78717528075091, tmp1);
        tmp1 = points[3];
        AssertEx.Equal(8.36808057330268, 6.90597574926124, tmp1);
        tmp1 = points[4];
        AssertEx.Equal(7.68404028665134, 6.97657701249471, tmp1);
        tmp1 = points[5];
        AssertEx.Equal(7, 7, tmp1);
        tmp1 = points[6];
        AssertEx.Equal(6.31595971334866, 6.97657701249471, tmp1);
        tmp1 = points[7];
        AssertEx.Equal(5.63191942669732, 6.90597574926124, tmp1);
        tmp1 = points[8];
        AssertEx.Equal(4.94787914004599, 6.78717528075091, tmp1);
        tmp1 = points[9];
        AssertEx.Equal(4.26383885339465, 6.61838979142597, tmp1);
        tmp1 = points[10];
        AssertEx.Equal(3.57979856674331, 6.39692620785909, tmp1);

        #endregion
    }


    [Fact]
    public void T51b_Should_generate_points()
    {
        var v1     = MathEx.GetCosSinV(60) * 10;
        var v2     = MathEx.GetCosSinV(60 + 32) * 10;
        var center = new Point(7, -3);
        var arc = new ArcDefinition
        {
            Center         = center,
            Start          = center + v1,
            End            = center + v2,
            DirectionStart = v1.GetPrependicular().NormalizeFast()
        };
        arc.UpdateRadiusVectors();

        var points = arc.GetSmallAnglePoints(10);
        foreach (var i in points)
        {
            var dist = Math.Abs((i - center).Length - 10);
            Assert.True(dist < 1e-10);
        }

        var aaa  = string.Join("\r\n", points.Select(x => x.X + "\t" + x.Y));
        var code = new DpAssertsBuilder().Create(points, nameof(points));

        #region Asserts

        Assert.Equal(11, points.Count);
        var tmp1 = points[0];
        AssertEx.Equal(12, 5.66025403784439, tmp1);
        tmp1 = points[1];
        AssertEx.Equal(11.4992672388919, 5.9306547527622, tmp1);
        tmp1 = points[2];
        AssertEx.Equal(10.990603629048, 6.16924657078372, tmp1);
        tmp1 = points[3];
        AssertEx.Equal(10.4742687920499, 6.3770707771984, tmp1);
        tmp1 = points[4];
        AssertEx.Equal(9.95043960627201, 6.55483679241783, tmp1);
        tmp1 = points[5];
        AssertEx.Equal(9.41921895599668, 6.70295726275996, tmp1);
        tmp1 = points[6];
        AssertEx.Equal(8.88064061286701, 6.82156763888714, tmp1);
        tmp1 = points[7];
        AssertEx.Equal(8.33467080523989, 6.91053247013703, tmp1);
        tmp1 = points[8];
        AssertEx.Equal(7.78120664883298, 6.96943911019166, tmp1);
        tmp1 = points[9];
        AssertEx.Equal(7.22007126527189, 6.99757813863946, tmp1);
        tmp1 = points[10];
        AssertEx.Equal(6.65100503297499, 6.99390827019096, tmp1);

        #endregion
    }


    [Fact]
    public void T51c_Should_generate_points()
    {
        var v1     = MathEx.GetCosSinV(60) * 10;
        var v2     = MathEx.GetCosSinV(60 - 32) * 10;
        var center = new Point(7, -3);
        var arc = new ArcDefinition
        {
            Center         = center,
            Start          = center + v1,
            End            = center + v2,
            DirectionStart = v1.GetPrependicular(false).NormalizeFast()
        };
        arc.UpdateRadiusVectors();

        var points = arc.GetSmallAnglePoints(10);
        foreach (var i in points)
        {
            var dist = Math.Abs((i - center).Length - 10);
            Assert.True(dist < 1e-10);
        }

        var aaa  = string.Join("\r\n", points.Select(x => x.X + "\t" + x.Y));
        var code = new DpAssertsBuilder().Create(points, nameof(points));

        #region Asserts

        Assert.Equal(11, points.Count);
        var tmp1 = points[0];
        AssertEx.Equal(12, 5.66025403784439, tmp1);
        tmp1 = points[1];
        AssertEx.Equal(12.4845402688744, 5.36180710367655, tmp1);
        tmp1 = points[2];
        AssertEx.Equal(12.9454986493381, 5.04058740458179, tmp1);
        tmp1 = points[3];
        AssertEx.Equal(13.3836471101136, 4.69734042208988, tmp1);
        tmp1 = points[4];
        AssertEx.Equal(13.7995115881121, 4.33257404757224, tmp1);
        tmp1 = points[5];
        AssertEx.Equal(14.1933980033865, 3.94658370458997, tmp1);
        tmp1 = points[6];
        AssertEx.Equal(14.5654067738299, 3.53946636557514, tmp1);
        tmp1 = points[7];
        AssertEx.Equal(14.9154374815493, 3.11112505809569, tmp1);
        tmp1 = points[8];
        AssertEx.Equal(15.2431842064916, 2.6612643585905, tmp1);
        tmp1 = points[9];
        AssertEx.Equal(15.5481210117458, 2.18937637568817, tmp1);
        tmp1 = points[10];
        AssertEx.Equal(15.8294759285893, 1.69471562785891, tmp1);

        #endregion
    }


    [Fact]
    public void T51d_Should_generate_points()
    {
        var v1     = MathEx.GetCosSinV(60) * 10;
        var v2     = MathEx.GetCosSinV(60 - 32) * 10;
        var center = new Point(7, -3);
        var arc = new ArcDefinition
        {
            Center         = center,
            Start          = center + v1,
            End            = center + v2,
            DirectionStart = v1.GetPrependicular(false).NormalizeFast()
        };
        arc.UpdateRadiusVectors();

        var points = arc.GetSmallAnglePoints(13);
        foreach (var i in points)
        {
            var dist = Math.Abs((i - center).Length - 10);
            Assert.True(dist < 1e-10);
        }

        var aaa  = string.Join("\r\n", points.Select(x => x.X + "\t" + x.Y));
        var code = new DpAssertsBuilder().Create(points, nameof(points));

        #region Asserts

        Assert.Equal(14, points.Count);
        var tmp1 = points[0];
        AssertEx.Equal(12, 5.66025403784439, tmp1);
        tmp1 = points[1];
        AssertEx.Equal(12.3748499235409, 5.43273314527455, tmp1);
        tmp1 = points[2];
        AssertEx.Equal(12.7356236155188, 5.19161899389266, tmp1);
        tmp1 = points[3];
        AssertEx.Equal(13.0827140568041, 4.93729108091407, tmp1);
        tmp1 = points[4];
        AssertEx.Equal(13.4164226356856, 4.67004045362614, tmp1);
        tmp1 = points[5];
        AssertEx.Equal(13.7369681986058, 4.39007844958182, tmp1);
        tmp1 = points[6];
        AssertEx.Equal(14.0444933472606, 4.09754277763798, tmp1);
        tmp1 = points[7];
        AssertEx.Equal(14.3390684186905, 3.79250136148525, tmp1);
        tmp1 = points[8];
        AssertEx.Equal(14.6206934128957, 3.47495420112363, tmp1);
        tmp1 = points[9];
        AssertEx.Equal(14.8892979928353, 3.1448333728625, tmp1);
        tmp1 = points[10];
        AssertEx.Equal(15.1447395568137, 2.80200116784496, tmp1);
        tmp1 = points[11];
        AssertEx.Equal(15.3867992583883, 2.4462462485181, tmp1);
        tmp1 = points[12];
        AssertEx.Equal(15.6151757092703, 2.07727756759453, tmp1);
        tmp1 = points[13];
        AssertEx.Equal(15.8294759285893, 1.69471562785891, tmp1);

        #endregion
    }


    [Fact]
    public void T52()
    {
        var a = new ArcDefinition(
            new Point(-32.3137145786995, -170.936449757157),
            new Point(-84.2421496951301, -107.606117740329),
            new Vector(0.77328238112352, 0.634061794341796),
            new Point(-50.1734159092112, -91.0094555975624));
        Assert.Equal(ArcDefinition.ArcFlags.None, a.GetFlags());

        {
            var arc = a.Angle;
            Assert.Equal(ArcDefinition.ArcFlags.HasDirection | ArcDefinition.ArcFlags.HasAngle, a.GetFlags());
        }

        {
            a.Start += new Vector(0, 0.000001);
            Assert.Equal(ArcDefinition.ArcFlags.HasDirection | ArcDefinition.ArcFlags.HasAngle, a.GetFlags());
        }

        {
            a.UpdateRadiusStart();
            Assert.Equal(ArcDefinition.ArcFlags.None, a.GetFlags());
        }
        {
            a.ResetFlags();
            var arc = a.Angle;
            Assert.Equal(ArcDefinition.ArcFlags.HasDirection | ArcDefinition.ArcFlags.HasAngle, a.GetFlags());
        }

        {
            a.ResetFlags();
            var q = a.EndAngle;
            Assert.Equal(ArcDefinition.ArcFlags.HasEndAngle, a.GetFlags());
        }

        {
            a.ResetFlags();
            var q = a.StartAngle;
            Assert.Equal(ArcDefinition.ArcFlags.HasStartAngle, a.GetFlags());
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void T53_Should_find_closest_point_on_arc(bool left)
    {
        var center = new Point(12, 14);

        const int radius = 12;

        Point FindPoint(double x, double y, double radius1 = radius)
        {
            return center + radius1 * new Vector(x, y).NormalizeFast();
        }

        var start = FindPoint(10, 3);
        var end   = FindPoint(-3, 10);
        if (!left)
            (start, end) = (end, start);
        var dirStart = (start - center).NormalizeFast().GetPrependicular(left);
        var arc      = new ArcDefinition(center, start, dirStart, end);

        if (!left)
            (start, end) = (end, start);
        var expected = start;
        for (var r = 11; r <= 13; r++)
        {
            var q = arc.FindClosestPointOnElement(FindPoint(1, 0, r));
            AssertEx.Equal(expected.X, expected.Y, q.ClosestPoint);
        }

        expected = center + new Vector(0, radius);
        for (int r = 11; r <= 13; r++)
        {
            var q = arc.FindClosestPointOnElement(FindPoint(0, 1, r));
            AssertEx.Equal(expected.X, expected.Y, q.ClosestPoint);
        }

        expected = end;
        for (int r = 11; r <= 13; r++)
        {
            var q = arc.FindClosestPointOnElement(FindPoint(-1, 1, r));
            AssertEx.Equal(expected.X, expected.Y, q.ClosestPoint);
        }
    }

    [Fact]
    public void T54_Should_find_closest_point_on_arc()
    {
        var arc = new ArcDefinition(
            new Point(6551548.156720737, 5573124.2815031214),
            new Point(6551146.8467774736, 5573876.2209492614),
            new Vector(0.882218656029713, 0.47083993347328429),
            new Point(6551161.5548050478, 5573883.8876905693));
        var checkPoint = new Point(6551146.7137651928, 5573876.4701760318);
        var result     = arc.FindClosestPointOnElement(checkPoint);
        {
            var toStart = arc.Start - checkPoint;
            var toEnd   = arc.End - checkPoint;
            Assert.True(toStart.Length < toEnd.Length);
        }

        {
            var v = arc.Start - result.ClosestPoint;
            Assert.True(v.Length < 1e-5);
            Assert.True(result.ElementTrack < 1e-5);
        }
        {
            var track = TrackFromPathResult.Make(new[] { arc });
            var back  = track.GetTrackInfo(result.ElementTrack);
            var v     = back.Location - arc.Start;
            Assert.True(v.Length < 1e-5);
        }
    }

    #region Fields

    private const double ArcLength = 7.853981633974483;

    #endregion

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