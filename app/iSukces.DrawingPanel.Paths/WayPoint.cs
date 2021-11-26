using System.Runtime.CompilerServices;
#if NET5_0
using iSukces.Mathematics.Compatibility;

#else
using System.Windows;
#endif

namespace iSukces.DrawingPanel.Paths
{
    public struct WayPoint
    {
        public WayPoint(PathRay ray, bool useInputVector, Vector inputVector)
        {
            Ray            = ray;
            UseInputVector = useInputVector && inputVector.IsValidVector();
            InputVector    = inputVector;
        }

        public WayPoint(PathRay ray, Vector inputVector)
        {
            Ray            = ray;
            UseInputVector = inputVector.IsValidVector();
            InputVector    = inputVector;
        }

        public PathRay Ray { get; }

        public bool UseInputVector { get; }

        public Vector InputVector { get; }

        public static implicit operator WayPoint(PathRay ray)
        {
            return new WayPoint(ray, false, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathRay GetInputRay()
        {
            if (UseInputVector)
                return new PathRay(Ray.Point, InputVector);
            return Ray;
        }
    }
}
