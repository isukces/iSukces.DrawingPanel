#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System.Collections.Generic;
using JetBrains.Annotations;


namespace iSukces.DrawingPanel.Paths
{
    public interface IPathResult
    {
        #region properties

        Point Start { get; }
        Point End   { get; }

        [NotNull]
        IReadOnlyList<IPathElement> Elements { get; }

        Vector StartVector { get; }

        Vector EndVector { get; }

        #endregion
    }

    public static class PathResultExtensions
    {
        public static bool FindDistanceFromSegmentStart(this IPathResult segment, Point aPoint,
            out double distance, out Vector direction)
        {
            var elements = segment?.Elements;
            if (elements is null)
            {
                distance  = 0;
                direction = default;
                return false;
            }

            var cnt = elements.Count;
            switch (cnt)
            {
                case 0:
                    distance  = 0;
                    direction = default;
                    return false;
                case 1:
                {
                    var element = elements[0];
                    element.DistanceFromElement(aPoint, out distance, out direction);
                    return true;
                }
                default:
                {
                    var offset = 0d;
                    var first  = true;
                    distance = 0;
                    var bestDistance = 0d;
                    direction = default;

                    for (var index = 0; index < cnt; index++)
                    {
                        var element = elements[index];

                        var currentDistance =
                            element.DistanceFromElement(aPoint, out var distanceFromStart, out var newDirection);
                        if (first)
                        {
                            bestDistance = currentDistance;
                            distance     = offset + distanceFromStart;
                            direction    = newDirection;
                            first        = false;
                        }
                        else if (currentDistance < bestDistance)
                        {
                            bestDistance = currentDistance;
                            distance     = offset + distanceFromStart;
                            direction    = newDirection;
                        }

                        offset += element.GetLength();
                    }

                    return !first;
                }
            }
        }

        public static double GetAllElementsLength(this IPathResult path)
        {
            var result   = 0d;
            var elements = path.Elements;
            for (var index = 0; index < elements.Count; index++)
            {
                var element = elements[index];
                result += element.GetLength();
            }

            return result;
        }
    }
}
