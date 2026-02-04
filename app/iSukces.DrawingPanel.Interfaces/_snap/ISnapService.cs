using System;
using Point=iSukces.Mathematics.Point;
using System.Collections.Generic;


namespace iSukces.DrawingPanel.Interfaces;

public interface ISnapService : IDisposable
{
    /// <summary>
    ///     Resets reference objects
    /// </summary>
    void Reset();

    SnapResult Snap(Point point);

    #region properties

    IList<SnapServiceItem> Infos { get; }

    #endregion
}

public static class SnapServiceExtensions
{
    public static void SetStartDraggingPoint(this ISnapService snapService, Point? point)
    {
        var infos = snapService.Infos;
        if (point == null)
        {
            for (var index = infos.Count - 1; index >= 0; index--)
            {
                if (infos[index].Kind != SnapServiceSpecialPointKind.StartDragging) continue;
                infos.RemoveAt(index);
                return;
            }
        }
        else
        {
            var dragInfo = new SnapServiceItem(point.Value, SnapServiceSpecialPointKind.StartDragging);
            for (var index = 0; index < infos.Count; index++)
            {
                if (infos[index].Kind != SnapServiceSpecialPointKind.StartDragging) continue;
                infos[index] = dragInfo;
                return;
            }

            infos.Add(dragInfo);
        }
    }
}
