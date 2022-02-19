using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace iSukces.DrawingPanel.Paths.Test
{
    public partial class PathDistanceFinderTests
    {
        private static IEnumerable<PathDistanceFinderTestData> Data_03()
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
            yield return new PathDistanceFinderTestData(63.283474128770692, -0.77720490366533879, 4.987233097473,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(60.298248407317153, 3.2179072800539537),
                Direction          = new Vector(17.777204903665339, 13.283474128770692),
                SideMovement       = 4.9872330974730019,
                Track              = 0.36937912246704141,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(60.939753871393556, 7.0705212217063256, 2.4305934211844522,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(62.739536665380975, 5.4369465299422846),
                Direction          = new Vector(9.9294787782936744, 10.939753871393556),
                SideMovement       = -2.4305934211844544,
                Track              = 3.6735490353053577,
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
            yield return new PathDistanceFinderTestData(60.608405720847081, -0.84132254153523345, 3.8892051102077607,
                Three.Above)
            {
                ClosestPoint       = new Point(60, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = 3.8413225415352334,
                Track              = 50.608405720847081,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(62.04495063033594, -1.0134415903366945, 4.5043908000478483,
                Three.Below, 1)
            {
                ClosestPoint       = new Point(60, 3),
                Direction          = new Vector(0.813733471206735, 0.58123819371909646),
                SideMovement       = 4.4544751674113474,
                Track              = -0.66872076569473315,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(63.438508804810695, 0.12878178979293331, 4.3645757569386578,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(60.7191998714489, 3.5427062855888485),
                Direction          = new Vector(16.871218210207068, 13.438508804810695),
                SideMovement       = 4.36457575693866,
                Track              = 0.90109053944607,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(61.412639875356, 9.4246587224466829, 3.5066892114661483,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(64.334284942288278, 7.485365209673704),
                Direction          = new Vector(7.5753412775533171, 11.412639875356),
                SideMovement       = -3.5066892114661474,
                Track              = 6.2720229209844991,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(59.130077893530007, 5.1836203798754443, 2.1836203798754443)
            {
                ClosestPoint       = new Point(59.130077893530007, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = -2.1836203798754443,
                Track              = 49.130077893530007,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(58.86265243915971, 2.0418187115574651, 0.95818128844253492)
            {
                ClosestPoint       = new Point(58.86265243915971, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = 0.95818128844253492,
                Track              = 48.86265243915971,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(60.06338708454583, 1.1577831964370873, 1.8433069939152229,
                Three.Above)
            {
                ClosestPoint       = new Point(60, 3),
                Direction          = new Vector(1, 0),
                SideMovement       = 1.8422168035629127,
                Track              = 50.06338708454583,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(61.189295533637754, 1.0703943093730395, 2.2666720070690993,
                Three.Below, 1)
            {
                ClosestPoint       = new Point(60, 3),
                Direction          = new Vector(0.813733471206735, 0.58123819371909646),
                SideMovement       = 2.2614487244639427,
                Track              = -0.15379094333238841,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(68.457728449523572, 15.213071034636206, 2.2446123358069956,
                Three.Above, 1)
            {
                ClosestPoint       = new Point(66.870547845739466, 13.625890430852106),
                Direction          = new Vector(0.19611613513818402, 0.98058067569092011),
                SideMovement       = 1.2450869031217136,
                Track              = 26.592303051226814,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(65.5505192251517, 15.582913341055242, 2.3605961599203678,
                Three.Above, 1)
            {
                ClosestPoint       = new Point(66.870547845739466, 13.625890430852106),
                Direction          = new Vector(0.19611613513818402, 0.98058067569092011),
                SideMovement       = -1.6781983262332225,
                Track              = 26.384812632832393,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(64.340810225261976, 12.045134923443687, 2.0319933559949295,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(66.2613987388808, 11.381556171400668),
                Direction          = new Vector(4.9548650765563131, 14.340810225261976),
                SideMovement       = -2.0319933559949295,
                Track              = 10.630398608470635,
                ElementTrackOffset = 50
            };
            yield return new PathDistanceFinderTestData(68.17669705737309, 11.535614747395059, 1.7756503619596173,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(66.47622566411502, 12.046820428738956),
                Direction          = new Vector(5.4643852526049415, 18.17669705737309),
                SideMovement       = 1.7756503619596238,
                Track              = 11.32953693505471,
                ElementTrackOffset = 50
            };

            #endregion
        }

        public static List<object[]> T03_TestData()
        {
            return MakeTestData(Data_03()).ToList();
        }
    }
}
