#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace iSukces.DrawingPanel.Paths
{
    [DebuggerDisplay("{GetCreationCode()}")]
    public class MinimumValuesPathValidator : IPathValidator, IMinRadiusPathValidator
    {
        public MinimumValuesPathValidator(double minRadius, double minLineLength)
        {
            _minRadius      = minRadius;
            _minRadius2     = minRadius * minRadius;
            _minLineLength  = minLineLength;
            _minLineLength2 = minLineLength * minLineLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsTooSmall(Vector v, double minLengthSquared) { return v.LengthSquared < minLengthSquared; }

        internal string GetCreationCode()
        {
            return $"new MinimumValuesPathValidator({_minRadius.Str()}, {_minLineLength.Str()})";
        }

        public double GetMinRadius() { return _minRadius; }


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

        private readonly double _minLineLength;
        private readonly double _minLineLength2;
        private readonly double _minRadius;
        private readonly double _minRadius2;
    }
}
