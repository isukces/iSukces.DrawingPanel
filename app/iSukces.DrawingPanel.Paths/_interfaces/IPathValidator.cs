#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using JetBrains.Annotations;


namespace iSukces.DrawingPanel.Paths
{
    public interface IPathValidator
    {
        ArcValidationResult ValidateArc([NotNull] ArcDefinition arc, ArcDestination arcDestination);
        LineValidationResult ValidateLine(Vector vector);
        CircleCrossValidationResult ValidatePointForCircleConnectionValid(PathRay start, PathRay end, Point cross);
    }

    public interface IMinRadiusPathValidator : IPathValidator
    {
        double GetMinRadius();
    }

    public enum LineValidationResult
    {
        Ok,
        TooShort
    }

    public enum ArcValidationResult
    {
        Ok,
        RadiusTooSmall,
        ArcLengthTooSmall,
        NoCrossPoints,
        InvalidDirectionVector,
        UnableToConstructArc,
        ReplaceByLine
    }

    public enum CircleCrossValidationResult
    {
        Ok,
        ForceLine,
        Invalid
    }

    public static class PathValidatorExtensions
    {
        public static bool IsOk(this IPathValidator validator, ArcDefinition arc, ArcDestination arcDestination)
        {
            if (arc is null)
                return false;
            if (validator is null)
                return true;
            var result = validator.ValidateArc(arc, arcDestination);
            return result == ArcValidationResult.Ok;
        }
    }
}
