using System.Runtime.CompilerServices;
using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public class MinimumValuesPathValidator : IPathValidator
    {
        public MinimumValuesPathValidator(double minRadius, double minLineLength)
        {
            _minRadius2     = minRadius * minRadius;
            _minLineLength2 = minLineLength * minLineLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsTooSmall(Vector v, double minLengthSquared) { return v.LengthSquared < minLengthSquared; }


        public ArcValidationResult ValidateArc(ArcDefinition arc)
        {
            if (IsTooSmall(arc.RadiusStart, _minRadius2))
                return ArcValidationResult.RadiusTooSmall;
            return ArcValidationResult.Ok;
        }

        public LineValidationResult ValidateLine(Vector vector)
        {
            if (IsTooSmall(vector, _minLineLength2))
                return LineValidationResult.TooShort;
            return LineValidationResult.Ok;
        }

        private readonly double _minLineLength2;

        private readonly double _minRadius2;
    }
}
