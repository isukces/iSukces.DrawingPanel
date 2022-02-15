#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    public interface IPathElement : ILineCollider
    {
        double DistanceFromElement(Point point, out double distanceFromStart, out Vector direction);
        Point GetEndPoint();
        Vector GetEndVector();
        double GetLength();
        Point GetStartPoint();
        Vector GetStartVector();
        
        /// <summary>
        /// Finds closest point on element. It can be point on line, line begin or line end.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        Point FindClosestPointOnElement(Point target);

        #region properties

        object Tag { get; set; }

        #endregion
    }
}
