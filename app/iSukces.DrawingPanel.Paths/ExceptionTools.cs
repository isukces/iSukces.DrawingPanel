using System.Collections;
#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif

namespace iSukces.DrawingPanel.Paths
{
    internal static class ExceptionTools
    {
        public static void Set(this IDictionary dictionary, string name, PathRayWithArm value)
        {
            var name2 = name + ".";
            dictionary.Set(name2 + nameof(value.Point), value.Point);
            dictionary.Set(name2 + nameof(value.Vector), value.Vector);
            dictionary.Set(name2 + nameof(value.ArmLength), value.ArmLength);
        }

        public static void Set(this IDictionary dictionary, string name, Point value)
        {
            dictionary[name] = value.ToString();
        }

        public static void Set(this IDictionary dictionary, string name, Vector value)
        {
            dictionary[name] = value.ToString();
        }

        public static void Set(this IDictionary dictionary, string name, double value)
        {
            dictionary[name] = value.ToString();
        }
    }
}
