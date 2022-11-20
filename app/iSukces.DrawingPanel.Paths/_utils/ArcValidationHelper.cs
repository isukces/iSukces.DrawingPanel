#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel.Paths
{
    public static class ArcValidationHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CircleCrossValidationResult Validate([CanBeNull] IPathValidator validator, PathRay start,
            PathRay end, Point cross)
        {
            if (validator is null)
                return CircleCrossValidationResult.Ok;
            var result = validator.ValidatePointForCircleConnectionValid(start, end, cross);
            return result;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CircleCrossValidationResult Validate([CanBeNull] IPathValidator validator, PathRay start,
            PathRay end, Point cross, bool reverseEnd)
        {
            if (validator is null)
                return CircleCrossValidationResult.Ok;
            if (reverseEnd)
                end = end.WithInvertedVector();
            var result = validator.ValidatePointForCircleConnectionValid(start, end, cross);
            return result;
        }

        public static bool Validate(IPathValidator validator, OneArcFinder calc)
        {
            var start  = new PathRay(calc.StartPoint, calc.StartVector);
            var end    = new PathRay(calc.EndPoint, calc.EndVector);
            var result = Validate(validator, start, end, calc.Cross);
            return result == CircleCrossValidationResult.Ok;
        }
    }
}
