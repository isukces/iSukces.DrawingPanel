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
