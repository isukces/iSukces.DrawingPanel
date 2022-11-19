#if COREFX
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using JetBrains.Annotations;


namespace iSukces.DrawingPanel.Paths
{
    public interface ILineGeometrySource
    {
        PathLineEquationNotNormalized GetLineEquation();
    }

    public static class LineSourceExt
    {
        public static Point? CrossWith([CanBeNull] this ILineGeometrySource a, [CanBeNull] ILineGeometrySource b)
        {
            var lineA = a?.GetLineEquation();
            if (lineA == null)
                return null;
            var lineB = b?.GetLineEquation();
            if (lineB == null)
                return null;
            return PathLineEquationNotNormalized.Cross(lineA, lineB);
        }
    }
}
