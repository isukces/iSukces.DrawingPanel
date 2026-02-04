using System.Collections.Generic;
using System.Linq;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=iSukces.Mathematics.Point;
using Vector=iSukces.Mathematics.Vector;
#endif

namespace iSukces.DrawingPanel.Paths.Test;

public partial class PathDistanceFinderTests
{
    private static IEnumerable<PathDistanceFinderTestData> Data_04()
    {
        #region DATA

        yield return new PathDistanceFinderTestData(12.278897175229572, 3, 0)
        {
            ClosestPoint       = new Point(12.278897175229572, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = 0,
            Track              = 2.2788971752295719,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(11.392334612880683, 4.4418046985778048, 1.4418046985778048)
        {
            ClosestPoint       = new Point(11.392334612880683, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = -1.4418046985778048,
            Track              = 1.3923346128806831,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(8.4966951120541179, 4.4517246554019447, 2.0898397214197733, Three.Below)
        {
            ClosestPoint       = new Point(10, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = -1.4517246554019447,
            Track              = -1.5033048879458821,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(8.6182785723653517, 1.6182785723653517, 1.9540491823824346, Three.Below)
        {
            ClosestPoint       = new Point(10, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = 1.3817214276346483,
            Track              = -1.3817214276346483,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(11.278485987091102, 1.7215140129088971, 1.2784859870911029)
        {
            ClosestPoint       = new Point(11.278485987091102, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = 1.2784859870911029,
            Track              = 1.2784859870911021,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(28.689036368201037, 3, 0)
        {
            ClosestPoint       = new Point(28.689036368201037, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = 0,
            Track              = 18.689036368201037,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(26.450176814863063, 5.8543683648174092, 2.8543683648174092)
        {
            ClosestPoint       = new Point(26.450176814863063, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = -2.8543683648174092,
            Track              = 16.450176814863063,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(26.5, 0.78029005768722381, 2.2197099423127762)
        {
            ClosestPoint       = new Point(26.5, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = 2.2197099423127762,
            Track              = 16.5,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(64.279316892978613, 6.7199534219792536, 5.5826389658121434, Three.Inside, 1)
        {
            ClosestPoint       = new Point(60.926775053848552, 2.2560626258814711),
            Direction          = new Vector(10.280046578020746, -7.7206831070213866),
            SideMovement       = -5.5826389658121442,
            Track              = 1.1886311439556634,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(62.112017976824205, 6.658122442358346, 4.2240359536484062, Three.Below, 1)
        {
            ClosestPoint       = new Point(60, 3),
            Direction          = new Vector(0.75925660236529668, -0.65079137345596838),
            SideMovement       = -4.151936696522359,
            Track              = -0.77711093531451558,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(60.781597292910043, 7.4326585175431337, 4.5010394201153137, Three.Above)
        {
            ClosestPoint       = new Point(60, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = -4.4326585175431337,
            Track              = 50.781597292910043,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(55.8161168334292, 7.1838831665708023, 4.1838831665708023)
        {
            ClosestPoint       = new Point(55.8161168334292, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = -4.1838831665708023,
            Track              = 45.8161168334292,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(55.337580899270662, -0.38744576006437459, 3.3874457600643746)
        {
            ClosestPoint       = new Point(55.337580899270662, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = 3.3874457600643746,
            Track              = 45.337580899270662,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(61.417646973811777, -1.1171457620483798, 2.5422639048627991, Three.Inside, 1)
        {
            ClosestPoint       = new Point(62.699887178173441, 1.0780685373508399),
            Direction          = new Vector(18.117145762048381, -10.582353026188223),
            SideMovement       = 2.5422639048628,
            Track              = 3.3185698332688736,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(70.9519395225704, -6.7334624907536087, 5.3175032771482584, Three.Inside, 1)
        {
            ClosestPoint       = new Point(71.186529777704749, -1.4211363981009306),
            Direction          = new Vector(23.73346249075361, -1.0480604774296012),
            SideMovement       = 5.3175032771482584,
            Track              = 12.252688512390481,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(71.602532907491721, 3.9448828975262318, 5.3779227090385033, Three.Inside, 1)
        {
            ClosestPoint       = new Point(71.438876211814119, -1.4305490990998928),
            Direction          = new Vector(13.055117102473769, -0.39746709250827905),
            SideMovement       = -5.3779227090385042,
            Track              = 12.505212409097053,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(91.668194870773576, 14.970977750386224, 2.2446123358070085, Three.Above, 1)
        {
            ClosestPoint       = new Point(90.08101426698947, 13.383797146602106),
            Direction          = new Vector(0.19611613513818402, 0.98058067569092011),
            SideMovement       = 1.2450869031217102,
            Track              = 40.258312050715595,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(88.7609856464017, 15.34082005680526, 2.3605961599203824, Three.Above, 1)
        {
            ClosestPoint       = new Point(90.08101426698947, 13.383797146602106),
            Direction          = new Vector(0.19611613513818402, 0.98058067569092011),
            SideMovement       = -1.6781983262332261,
            Track              = 40.050821632321174,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(87.55127664651198, 11.803041639193705, 2.042425182585196, Three.Inside, 1)
        {
            ClosestPoint       = new Point(89.488397487881443, 11.155690519328886),
            Direction          = new Vector(5.1969583608062955, 15.55127664651198),
            SideMovement       = -2.0424251825851911,
            Track              = 36.083606969099868,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(91.3871634786231, 11.293521463145076, 1.7704634069883776, Three.Inside, 1)
        {
            ClosestPoint       = new Point(89.688745673120209, 11.793439089795365),
            Direction          = new Vector(5.7064785368549238, 19.387163478623094),
            SideMovement       = 1.7704634069883731,
            Track              = 36.752121430976779,
            ElementTrackOffset = 50
        };

        #endregion
    }

    public static List<object[]> T04_TestData()
    {
        return MakeTestData(Data_04()).ToList();
    }
}
