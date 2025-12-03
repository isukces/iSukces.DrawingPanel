#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif

namespace iSukces.DrawingPanel.Paths;

public class PathDistanceFinderResult
{
    public PathDistanceFinderResult(double distanceFromLine, Three location, double track,
        Vector direction, double sideMovement, Point closestPoint, int elementIndex,
        double elementTrackOffset)
    {
        DistanceFromLine   = distanceFromLine;
        Location           = location;
        Track              = track;
        Direction          = direction;
        SideMovement       = sideMovement;
        ClosestPoint       = closestPoint;
        ElementIndex       = elementIndex;
        ElementTrackOffset = elementTrackOffset;
    }

    #region properties

    /// <summary>
    ///     Not negative value
    /// </summary>
    public double DistanceFromLine { get; }

    public Three Location { get; }

    /// <summary>
    ///     Can be lower than zero or greater than element length
    /// </summary>
    public double Track { get; }

    /// <summary>
    ///     Direction of path in specified point
    /// </summary>
    public Vector Direction { get; }

    public double SideMovement       { get; }
    public Point  ClosestPoint       { get; }
    public int    ElementIndex       { get; }
    public double ElementTrackOffset { get; }

    public double AbsoluteTrack => ElementTrackOffset + Track;

    #endregion
}
