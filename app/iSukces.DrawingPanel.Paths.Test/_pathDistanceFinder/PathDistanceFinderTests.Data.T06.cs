using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace iSukces.DrawingPanel.Paths.Test
{
    public partial class PathDistanceFinderTests
    {
        private static IEnumerable<PathDistanceFinderTestData> Data_06()
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
            yield return new PathDistanceFinderTestData(8.4966951120541179, 4.4517246554019447, 2.0898397214197733,
                Three.Below)
            {
                ClosestPoint       = new Point(10, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = -1.4517246554019447,
                Track              = -1.5033048879458821,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(8.6182785723653517, 1.6182785723653517, 1.9540491823824346,
                Three.Below)
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
            yield return new PathDistanceFinderTestData(63.59396454417427, 7.1343832682905362, 5.4780092941480758,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(60.02229870243734, 2.9808559791370959),
                Direction          = new Vector(18.134383268290534, -15.59396454417427),
                SideMovement       = -5.4780092941480767,
                Track              = 0.029389213123815046,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(62.112017976824205, 6.6581224423583478, 4.224035953648408,
                Three.Below, 1)
            {
                ClosestPoint       = new Point(60, 3),
                Direction          = new Vector(0.75925660236529668, -0.65079137345596838),
                SideMovement       = -4.1519366965223607,
                Track              = -0.77711093531451647,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(60.781597292910043, 7.4326585175431354, 4.5010394201153154,
                Three.Above)
            {
                ClosestPoint       = new Point(60, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = -4.4326585175431354,
                Track              = 50.781597292910043,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(55.8161168334292, 7.1838831665708041, 4.1838831665708041)
            {
                ClosestPoint       = new Point(55.8161168334292, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = -4.1838831665708041,
                Track              = 45.8161168334292,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(56.119611923697974, 0.38264519388048557, 2.6173548061195144)
            {
                ClosestPoint       = new Point(56.119611923697974, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = 2.6173548061195144,
                Track              = 46.119611923697974,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(61.034051841372985, -2.8643994085208391, 3.0743726989287294,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(63.642074832172185, -1.2365223949288664),
                Direction          = new Vector(8.1356005914791609, -13.034051841372985),
                SideMovement       = 3.0743726989287286,
                Track              = 5.6084384850949762,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(63.391410923066339, 3.5681241926505756, 2.75349876826166,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(61.39164422856431, 1.6753250394441634),
                Direction          = new Vector(14.568124192650576, -15.391410923066339),
                SideMovement       = -2.753498768261661,
                Track              = 1.9221816077824987,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(59.888916935888787, -4.5706297016935924, 4.9230559040753317,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(64.219314965308669, -2.2288072614886612),
                Direction          = new Vector(6.4293702983064076, -11.888916935888787),
                SideMovement       = 4.9230559040753352,
                Track              = 6.7565940961470883,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(58.900069806070221, 0.45271164962684285, 2.5472883503731572)
            {
                ClosestPoint       = new Point(58.900069806070221, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = 2.5472883503731572,
                Track              = 48.900069806070221,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(67.879262337271385, -12.754062464905584, 1.5174089295963855,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(66.367726073750148, -12.620690864933522),
                Direction          = new Vector(-1.7540624649055836, -19.879262337271385),
                SideMovement       = -1.5174089295963853,
                Track              = 17.520415200241526,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(64.890882345643533, -12.916518322167539, 1.43982572780099,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(66.321528291840977, -13.078846086479349),
                Direction          = new Vector(-1.9165183221675388, -16.890882345643533),
                SideMovement       = 1.4398257278009936,
                Track              = 17.980905659305474,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(64.194539524101486, -15.888646134283968, 2.2754996942707777,
                Three.Above, 1)
            {
                ClosestPoint       = new Point(66.08101426698947, -14.616202853397894),
                Direction          = new Vector(-0.19611613513818402, -0.98058067569092011),
                SideMovement       = 1.6002940196250264,
                Track              = 37.236649086010374,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(67.303202664928847, -16.428171668351936, 2.1856293067269155,
                Three.Above, 1)
            {
                ClosestPoint       = new Point(66.08101426698947, -14.616202853397894),
                Direction          = new Vector(-0.19611613513818402, -0.98058067569092011),
                SideMovement       = -1.5538106460526993,
                Track              = 37.156038398133653,
                ElementTrackOffset = 50
            };

            #endregion
        }

        public static List<object[]> T06_TestData()
        {
            return MakeTestData(Data_06()).ToList();
        }
    }
}
