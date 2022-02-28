using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace iSukces.DrawingPanel.Paths.Test
{
    public partial class PathDistanceFinderTests
    {
        private static IEnumerable<PathDistanceFinderTestData> Data_05()
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
            yield return new PathDistanceFinderTestData(63.533168016697374, -0.071336099391488883, 4.6143669106560061,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(60.707138724337646, 3.5763947603186725),
                Direction          = new Vector(10.928663900608511, 8.466831983302626),
                SideMovement       = 4.6143669106560061,
                Track              = 0.9123847466839673,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(61.796889054745073, 8.2185447387672639, 3.3199536846918809,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(63.35366048085293, 5.2862154265389627),
                Direction          = new Vector(19.218544738767264, 10.203110945254927),
                SideMovement       = -3.3199536846918818,
                Track              = 4.0670344288714748,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(56.202038692961004, 5.6593611370175045, 2.6593611370175045)
            {
                ClosestPoint       = new Point(56.202038692961004, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = -2.6593611370175045,
                Track              = 46.202038692961004,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(57.440390909646212, 0.34944719548474579, 2.6505528045152542)
            {
                ClosestPoint       = new Point(57.440390909646212, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = 2.6505528045152542,
                Track              = 47.440390909646212,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(61.031475972751807, -2.30648385799719, 5.4058036976520913,
                Three.Above)
            {
                ClosestPoint       = new Point(60, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = 5.30648385799719,
                Track              = 51.031475972751807,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(62.622467274295545, -1.3645184777457642, 5.0917930189016234,
                Three.Below, 1)
            {
                ClosestPoint       = new Point(60, 3),
                Direction          = new Vector(0.75925660236529668, 0.65079137345596838),
                SideMovement       = 5.0204685496559325,
                Track              = -0.84926538211030222,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(73.322291655870487, 12.856375201353783, 5.45390363492448,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(73.020460428432628, 7.4108299789554044),
                Direction          = new Vector(23.856375201353785, -1.3222916558704867),
                SideMovement       = -5.45390363492448,
                Track              = 14.087404891424898,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(73.438551524022046, 1.980595655394481, 5.3790240580809545,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(74.031045002540225, 7.3268888848504865),
                Direction          = new Vector(12.980595655394481, -1.4385515240220457),
                SideMovement       = 5.3790240580809545,
                Track              = 15.101597463826023,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(91.879262337271385, -12.754062464905578, 1.5174089295963855,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(90.367726073750148, -12.620690864933517),
                Direction          = new Vector(-1.7540624649055783, -19.879262337271385),
                SideMovement       = -1.5174089295963853,
                Track              = 43.653260878188647,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(88.890882345643533, -12.916518322167533, 1.4398257278009898,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(90.321528291840977, -13.078846086479341),
                Direction          = new Vector(-1.9165183221675335, -16.890882345643533),
                SideMovement       = 1.4398257278009936,
                Track              = 44.113751337252616,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(88.194539524101486, -15.888646134283963, 2.2754996942707746,
                Three.Above, 1)
            {
                ClosestPoint       = new Point(90.08101426698947, -14.616202853397894),
                Direction          = new Vector(-0.19611613513818402, -0.98058067569092011),
                SideMovement       = 1.6002940196250273,
                Track              = 58.662707520641092,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(91.303202664928847, -16.428171668351929, 2.1856293067269092,
                Three.Above, 1)
            {
                ClosestPoint       = new Point(90.08101426698947, -14.616202853397894),
                Direction          = new Vector(-0.19611613513818402, -0.98058067569092011),
                SideMovement       = -1.5538106460526979,
                Track              = 58.582096832764364,
                ElementTrackOffset = 50
            };

            #endregion
        }

        public static List<object[]> T05_TestData()
        {
            return MakeTestData(Data_05()).ToList();
        }
    }
}
