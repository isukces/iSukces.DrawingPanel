using System.Runtime.CompilerServices;
#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    public class MinimumValuesPathValidator : IPathValidator, IMinRadiusPathValidator
    {
        public MinimumValuesPathValidator(double minRadius, double minLineLength)
        {
            _minRadius      = minRadius;
            _minRadius2     = minRadius * minRadius;
            _minLineLength2 = minLineLength * minLineLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsTooSmall(Vector v, double minLengthSquared)
        {
            return v.LengthSquared < minLengthSquared;
        }


        public ArcValidationResult ValidateArc(ArcDefinition arc)
        {
            if (arc.Radius < _minRadius)
                return ArcValidationResult.RadiusTooSmall;
            return ArcValidationResult.Ok;
        }

        public LineValidationResult ValidateLine(Vector vector)
        {
            if (IsTooSmall(vector, _minLineLength2))
                return LineValidationResult.TooShort;
            return LineValidationResult.Ok;
        }

        public double GetMinRadius()
        {
            return _minRadius;
        }

        private readonly double _minLineLength2;

        private readonly double _minRadius2;
        private readonly double _minRadius;
    }
}
