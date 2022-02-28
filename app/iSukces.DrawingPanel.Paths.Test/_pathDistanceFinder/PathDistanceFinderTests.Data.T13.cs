using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace iSukces.DrawingPanel.Paths.Test
{
    public partial class PathDistanceFinderTests
    {
        private static IEnumerable<PathDistanceFinderTestData> Data_13()
        {
            #region DATA

            yield return new PathDistanceFinderTestData(1.4696287981167055, 5.323259713440855, 1.3676046712899306)
            {
                ClosestPoint       = new Point(0.11818750525414168, 5.5328998071252471),
                Direction          = new Vector(1.323259713440855, 8.5303712018832947),
                SideMovement       = 1.3676046712899304,
                Track              = 1.5389674886079676,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(-1.2381410973111566, 5.32774377135211, 1.3163032323033104)
            {
                ClosestPoint       = new Point(0.069070378716110881, 5.173301690575026),
                Direction          = new Vector(1.32774377135211, 11.238141097311157),
                SideMovement       = -1.3163032323033104,
                Track              = 1.1760105224603177,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(-1.8727658366181528, 2.0606942055505453, 2.6959523072913067,
                Three.Below)
            {
                ClosestPoint       = new Point(0, 4),
                Direction          = new Vector(0, 1),
                SideMovement       = -1.8727658366181528,
                Track              = -1.9393057944494547,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(1.2784859870911023, 2.7215140129088971, 1.808052222248191,
                Three.Below)
            {
                ClosestPoint       = new Point(0, 4),
                Direction          = new Vector(0, 1),
                SideMovement       = 1.2784859870911023,
                Track              = -1.2784859870911029,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(2.1121506910621659, 7.4649322272008947, 1.3846693586603185)
            {
                ClosestPoint       = new Point(0.8444032651644402, 8.0218215312304011),
                Direction          = new Vector(3.4649322272008947, 7.8878493089378345),
                SideMovement       = 1.384669358660318,
                Track              = 4.1389901089641095,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(-0.44745379773759808, 11.360289454880556, 2.7797946664055484)
            {
                ClosestPoint       = new Point(1.82502217723342, 9.7593174593240271),
                Direction          = new Vector(7.3602894548805562, 10.447453797737598),
                SideMovement       = -2.7797946664055484,
                Track              = 6.13743444540219,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(18.368055732411143, 13.578262852211514, 0, Three.Inside, 1)
            {
                ClosestPoint       = new Point(18.368055732411143, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = 0,
                Track              = 5.49457687674769,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(16.840805674838329, 17.150459965247858, 3.5721971130363439,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(16.840805674838329, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = -3.5721971130363439,
                Track              = 3.9673268191748754,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(9.996421541909827, 17.007006241976846, 3.0070067342254778)
            {
                ClosestPoint       = new Point(9.9972488227589231, 13.999999621551183),
                Direction          = new Vector(13.007006241976846, 0.0035784580901729868),
                SideMovement       = -3.0070067342254774,
                Track              = 15.705212090673186,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(9.50727450692603, 11.221221192071264, 2.7619882622128804)
            {
                ClosestPoint       = new Point(9.3192529786852614, 13.976802267909848),
                Direction          = new Vector(7.2212211920712637, 0.49272549307396929),
                SideMovement       = 2.7619882622128804,
                Track              = 15.026689364802541,
                ElementTrackOffset = 0
            };
            yield return new PathDistanceFinderTestData(12.076254227922032, 7.9057148740412009, 5.7282953547911228,
                Three.Below, 1)
            {
                ClosestPoint       = new Point(12.873478855663453, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = 5.6725479781703134,
                Track              = -0.79722462774142144,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(16.500728217794467, 9.9510134900804985, 3.6272493621310158,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(16.500728217794467, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = 3.6272493621310158,
                Track              = 3.627249362131014,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(18.647362351270811, 13.578262852211514, 0, Three.Inside, 1)
            {
                ClosestPoint       = new Point(18.647362351270811, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = 0,
                Track              = 5.7738834956073575,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(22.773478855663452, 18.957950329943447, 5.3796874777319328,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(22.773478855663452, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = -5.3796874777319328,
                Track              = 9.8999999999999986,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(22.773478855663452, 9.4083325104788589, 4.1699303417326554,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(22.773478855663452, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = 4.1699303417326554,
                Track              = 9.8999999999999986,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(45.254962570775533, 13.578262852211514, 2.3814837151120827,
                Three.Above, 1)
            {
                ClosestPoint       = new Point(42.87347885566345, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = 0,
                Track              = 32.381483715112083,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(44.460659459447555, 15.165443455995618, 2.2446123358069983,
                Three.Above, 1)
            {
                ClosestPoint       = new Point(42.87347885566345, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = -1.5871806037841036,
                Track              = 31.587180603784102,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(41.425925331361491, 15.025816376513475, 1.447553524301961,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(41.425925331361491, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = -1.447553524301961,
                Track              = 28.552446475698037,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(40.289741222293706, 13.578262852211514, 0, Three.Inside, 1)
            {
                ClosestPoint       = new Point(40.289741222293706, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = 0,
                Track              = 27.416262366630253,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(40.809217791625478, 11.440657944822615, 2.1376049073888996,
                Three.Inside, 1)
            {
                ClosestPoint       = new Point(40.809217791625478, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = 2.1376049073888996,
                Track              = 27.935738935962025,
                ElementTrackOffset = 18.622531212727637
            };
            yield return new PathDistanceFinderTestData(44.790144049810266, 11.661597658064698, 2.7105739120908883,
                Three.Above, 1)
            {
                ClosestPoint       = new Point(42.87347885566345, 13.578262852211514),
                Direction          = new Vector(1, 0),
                SideMovement       = 1.916665194146816,
                Track              = 31.916665194146812,
                ElementTrackOffset = 18.622531212727637
            };

            #endregion
        }

        public static List<object[]> T13_TestData()
        {
            return MakeTestData(Data_13()).ToList();
        }
    }
}
