using System.Collections.Generic;
using System.Windows;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel.Paths
{
    public interface IPathResult
    {
        Point Start { get; }
        Point End   { get; }

        [NotNull]
        IReadOnlyList<IPathElement> Elements { get; }

        Vector StartVector { get; }

        Vector EndVector { get; }
    }

    public static class PathResultExtensions
    {
        private static bool FindDistanceFromSegmentStart(Point aPoint, IPathResult segment, out double distance)
        {
            var elements = segment?.Elements;
            if (elements is null)
            {
                distance = 0;
                return false;
            }

            var cnt = elements.Count;
            switch (cnt)
            {
                case 0:
                    distance = 0;
                    return false;
                case 1:
                {
                    var element = elements[0];
                    if (element is InvalidPathElement)
                    {
                        distance = 0;
                        return false;
                    }

                    element.DistanceFromElement(aPoint, out distance);
                    return true;
                }
                default:
                {
                    var offset = 0d;
                    var first  = true;
                    distance = 0;
                    var bestDistance = 0d;

                    for (var index = 0; index < cnt; index++)
                    {
                        var element = elements[index];
                        if (element is InvalidPathElement)
                        {
                            offset += element.GetLength();
                            continue;
                        }

                        var currentDistance = element.DistanceFromElement(aPoint, out var distanceFromStart);
                        if (first)
                        {
                            bestDistance = currentDistance;
                            distance     = offset + distanceFromStart;
                            first        = false;
                        }
                        else if (currentDistance < bestDistance)
                        {
                            bestDistance = currentDistance;
                            distance     = offset + distanceFromStart;
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
