#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System.Runtime.CompilerServices;
using JetBrains.Annotations;


namespace iSukces.DrawingPanel.Paths
{
    public abstract class ReferencePointPathCalculator : PathBase
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool DotNotPositive(in Point p, in PathRay ray)
        {
            var vector = p - ray.Point;
            var dot    = vector * ray.Vector;
            return dot <= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool DotNotPositive(in Point p, in PathRayWithArm ray)
        {
            var vector = p - ray.Point;
            var dot    = vector * ray.Vector;
            return dot <= 0;
        }

        protected static ArcDefinition MakeNotValidated(PathRay a, PathRay b, bool invertVector = false)
        {
            var cross = a.Cross(b);
            if (cross is null)
                return null;
            if (invertVector)
                b = b.WithInvertedVector();
            var c = new OneArcFinder(cross.Value); // NOT VALIDATED
            c.Setup(a, b);
            return c.CalculateArc();
        }


        [NotNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected IPathResult CreateResult(params IPathElement[] elements)
        {
            return new PathResult(Start.Point, End.Point, elements);
        }

        [NotNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected IPathResult CreateResultLine()
        {
            var startPoint = Start.Point;
            var endPoint   = End.Point;
            var el         = new LinePathElement(startPoint, endPoint);
            return new PathResult(el);
        }


        public abstract void InitDemo();
        public abstract void SetReferencePoint(Point p, int nr);

        #region properties

        public PathRayWithArm Start { get; set; }
        public PathRayWithArm End   { get; set; }

        #endregion

        #region Fields

        public const double Epsilon = 1e-8;

        #endregion
    }
}
