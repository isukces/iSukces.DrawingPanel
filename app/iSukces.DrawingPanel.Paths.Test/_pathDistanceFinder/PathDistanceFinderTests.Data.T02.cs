using System.Collections.Generic;
using System.Linq;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif

namespace iSukces.DrawingPanel.Paths.Test;

public partial class PathDistanceFinderTests
{
    private static IEnumerable<PathDistanceFinderTestData> Data_02()
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
        yield return new PathDistanceFinderTestData(62.587768471200988, 5.5877684712009872, 2.5877684712009881,
            Three.Inside, 1)
        {
            ClosestPoint       = new Point(60, 5.5877684712009872),
            Direction          = new Vector(0, 1),
            SideMovement       = 2.5877684712009881,
            Track              = 2.5877684712009872,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(60, 8.5544934833210391, 0, Three.Inside, 1)
        {
            ClosestPoint       = new Point(60, 8.5544934833210391),
            Direction          = new Vector(0, 1),
            SideMovement       = 0,
            Track              = 5.5544934833210391,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(58.452315385148069, 6.6461164614479413, 1.5476846148519314,
            Three.Inside, 1)
        {
            ClosestPoint       = new Point(60, 6.6461164614479413),
            Direction          = new Vector(0, 1),
            SideMovement       = -1.5476846148519314,
            Track              = 3.6461164614479413,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(55.169682276936044, 4.9515750392342, 1.9515750392342)
        {
            ClosestPoint       = new Point(55.169682276936044, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = -1.9515750392342,
            Track              = 45.169682276936044,
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
        yield return new PathDistanceFinderTestData(62.297177416761158, -2.4118108519936827, 5.8791768796179333,
            Three.Above)
        {
            ClosestPoint       = new Point(60, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = 5.4118108519936827,
            Track              = 52.297177416761158,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(65.188274578621758, 0.79770810179456664, 5.6363359293138373,
            Three.Below, 1)
        {
            ClosestPoint       = new Point(60, 3),
            Direction          = new Vector(0, 1),
            SideMovement       = 5.1882745786217583,
            Track              = -2.2022918982054334,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(61.995280362374508, 10.781473068142077, 1.9952803623745083,
            Three.Inside, 1)
        {
            ClosestPoint       = new Point(60, 10.781473068142077),
            Direction          = new Vector(0, 1),
            SideMovement       = 1.9952803623745083,
            Track              = 7.781473068142077,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(60, 11.679687477731928, 0, Three.Inside, 1)
        {
            ClosestPoint       = new Point(60, 11.679687477731928),
            Direction          = new Vector(0, 1),
            SideMovement       = 0,
            Track              = 8.6796874777319282,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(56.536373935706024, 10.896387032650797, 3.463626064293976,
            Three.Inside, 1)
        {
            ClosestPoint       = new Point(60, 10.896387032650797),
            Direction          = new Vector(0, 1),
            SideMovement       = -3.463626064293976,
            Track              = 7.8963870326507966,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(55.822303207618425, 5.2583832022711388, 2.2583832022711388)
        {
            ClosestPoint       = new Point(55.822303207618425, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = -2.2583832022711388,
            Track              = 45.822303207618425,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(57.909778916491852, 1.3757477153226425, 1.6242522846773575)
        {
            ClosestPoint       = new Point(57.909778916491852, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = 1.6242522846773575,
            Track              = 47.909778916491852,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(60.773365962231971, 0.797215248012094, 2.33459966056911,
            Three.Above)
        {
            ClosestPoint       = new Point(60, 3),
            Direction          = new Vector(1, 0),
            SideMovement       = 2.202784751987906,
            Track              = 50.773365962231971,
            ElementTrackOffset = 0
        };
        yield return new PathDistanceFinderTestData(63.194126556999542, 1.3814764248866904, 3.5807908379752256,
            Three.Below, 1)
        {
            ClosestPoint       = new Point(60, 3),
            Direction          = new Vector(0, 1),
            SideMovement       = 3.1941265569995423,
            Track              = -1.6185235751133096,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(61.916665194146816, 11.083334805853184, 1.916665194146816,
            Three.Inside, 1)
        {
            ClosestPoint       = new Point(60, 11.083334805853184),
            Direction          = new Vector(0, 1),
            SideMovement       = 1.916665194146816,
            Track              = 8.083334805853184,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(61.587180603784105, 14.587180603784104, 2.2446123358069983,
            Three.Above, 1)
        {
            ClosestPoint       = new Point(60, 13),
            Direction          = new Vector(0, 1),
            SideMovement       = 1.5871806037841054,
            Track              = 11.587180603784104,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(60, 15.560371330361987, 2.5603713303619866, Three.Above, 1)
        {
            ClosestPoint       = new Point(60, 13),
            Direction          = new Vector(0, 1),
            SideMovement       = 0,
            Track              = 12.560371330361987,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(58.552446475698041, 14.447553524301961, 2.0471498263288037,
            Three.Above, 1)
        {
            ClosestPoint       = new Point(60, 13),
            Direction          = new Vector(0, 1),
            SideMovement       = -1.4475535243019593,
            Track              = 11.447553524301961,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(57.921186879623605, 10.921186879623605, 2.0788131203763953,
            Three.Inside, 1)
        {
            ClosestPoint       = new Point(60, 10.921186879623605),
            Direction          = new Vector(0, 1),
            SideMovement       = -2.0788131203763953,
            Track              = 7.9211868796236047,
            ElementTrackOffset = 50
        };
        yield return new PathDistanceFinderTestData(60, 10.61995262793263, 0, Three.Inside, 1)
        {
            ClosestPoint       = new Point(60, 10.61995262793263),
            Direction          = new Vector(0, 1),
            SideMovement       = 0,
            Track              = 7.61995262793263,
            ElementTrackOffset = 50
        };

        #endregion
    }

    public static List<object[]> T02_TestData()
    {
        return MakeTestData(Data_02()).ToList();
    }
}