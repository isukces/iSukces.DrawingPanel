#nullable disable
using System.Collections.Generic;
using System.Drawing;

namespace iSukces.DrawingPanel;

public static class ThumbsExtensions
{
    public static int FindThumb<T>(this IReadOnlyList<T> thumbs, Point location)
        where T:DrawableThumb
    {
        for (var index = thumbs.Count - 1; index >= 0; index--)
        {
            var thumb    = thumbs[index];
            var isInside = thumb.IsInside(location);
            if (isInside)
                return index;
        }

        return -1;
    }
        
}
