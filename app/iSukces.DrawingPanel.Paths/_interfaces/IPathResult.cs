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
        IReadOnlyList<IPathElement> Arcs { get; }
    }
}
