using System.Collections.Generic;
using WinPoint=iSukces.Mathematics.Point;
using Vector=iSukces.Mathematics.Vector;

namespace iSukces.DrawingPanel.Sample;

internal class DrawingSession
{
    public WinPoint ProcessPoint(WinPoint point, out AlignInfo ai, double scale)
    {
        var delta = 15 / scale;

        var x = new AlignToValueAggregator(point.X);
        var y = new AlignToValueAggregator(point.Y);

        var lastIdx = Points.Count - 2;

        ai = AlignInfo.Empty;

        for (var index = lastIdx; index >= 0; index--)
        {
            var p = Points[index];
            if (x.TryAccept(p.X, delta))
                ai.IdxX = index != lastIdx ? index : -1;

            if (y.TryAccept(p.Y, delta))
                ai.IdxY = index != lastIdx ? index : -1;
        }

        point = new WinPoint(x.Value, y.Value);
        if (x.HasAlignment || y.HasAlignment)
        {
            if (ai.IdxX < 0 && ai.IdxY < 0)
                // ortogonalnie do ostatniego punktu
                point = Snap(point, false);
            return point;
        }

        point = Snap(point, true);
        return point;
    }

    private WinPoint Snap(WinPoint point, bool snapAngle)
    {
        var p2             = Points[^2];
        var v              = point - p2;
        var lengthOriginal = v.Length;
        var lengthDesired  = Math.Round(lengthOriginal);

        if (snapAngle && Points.Count > 2)
        {
            var p1             = Points[^3];
            var vReference     = p2 - p1;
            var angleReference = Math.Atan2(vReference.Y, vReference.X);
            var angle          = Math.Atan2(v.Y, v.X) * 180 / Math.PI;
            angle =  angleReference + 15 * Math.Round((angle - angleReference) / 15);
            angle *= Math.PI / 180;

            v = new Vector(Math.Cos(angle) * lengthDesired, Math.Sin(angle) * lengthDesired);
        }
        else
        {
            v *= lengthDesired / lengthOriginal;
        }

        point = p2 + v;
        return point;
    }

    public List<WinPoint> Points { get; } = [];

    public AlignInfo PointAlign = AlignInfo.Empty;

    public struct AlignInfo
    {
        public int IdxX;
        public int IdxY;

        public static AlignInfo Empty => new AlignInfo { IdxX = -1, IdxY = -1 };

        public int[] GetUnique()
        {
            return IdxX < 0
                ? IdxY < 0
                    ? Array.Empty<int>()
                    : [IdxY]
                : IdxY < 0 || IdxY == IdxX
                    ? [IdxX]
                    : [IdxX, IdxY];
        }
    }
}
