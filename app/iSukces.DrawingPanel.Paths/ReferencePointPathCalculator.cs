using System.Runtime.CompilerServices;
using System.Windows;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel.Paths
{
    public abstract class ReferencePointPathCalculator : PathBase
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool DotNotPositive(Point p, PathRay ray)
        {
            var vector = p - ray.Point;
            var dot    = vector * ray.Vector;
            return dot <= 0;
        }

        protected static ArcDefinition Make(PathRay a, PathRay b, bool invertVector = false)
        {
            var cross = a.Cross(b);
            if (cross is null)
                return null;
            if (invertVector)
                b = b.WithInvertedVector();
            var c = new OneArcFinder
            {
                Cross = cross.Value
            };
            c.Setup(a, b);
            return c.CalculateArc();
        }


        [NotNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected IPathResult CreateInvalid(ArcValidationResult status)
        {
            return InvalidPathElement.MakeInvalid(Start, End, status);
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
        public const double Epsilon = 1e-8;
        public PathRay Start { get; set; }
        public PathRay End   { get; set; }
    }
}
