#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths;

public interface IPathElement : ILineCollider
{
    double DistanceFromElement(Point point, out double distanceFromStart, out Vector direction);

    /// <summary>
    ///     Finds closest point on element. It can be point on line, line begin or line end.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    ClosestPointResult FindClosestPointOnElement(Point target);

    Point GetEndPoint();
    Vector GetEndVector();
    double GetLength();
    Point GetStartPoint();
    Vector GetStartVector();

    object? Tag { get; set; }

    /// <summary>
    /// An intermediate point at the end of a section of the path
    /// </summary>
    WayPoint? EndWayPoint { get; set; }
}

public class ClosestPointResult
{
    public ClosestPointResult(Point closestPoint, Three location, double elementTrack, double sideMovement,
        Vector direction)
    {
        ClosestPoint = closestPoint;
        Location     = location;
        ElementTrack = elementTrack;
        SideMovement = sideMovement;
        Direction    = direction;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="closestPoint"></param>
    /// <param name="location"></param>
    /// <param name="elementTrack"></param>
    public ClosestPointResult(Point closestPoint, Three location, double elementTrack)
    {
        ClosestPoint = closestPoint;
        Location     = location;
        ElementTrack = elementTrack;
    }


    public Point  ClosestPoint { get; }
    public Three  Location     { get; }
    public double ElementTrack { get; }
    public double SideMovement { get; }
    public Vector Direction    { get; }
}
