using System.Windows;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel.Paths
{
    public interface IPathValidator
    {
        ArcValidationResult ValidateArc([NotNull] ArcDefinition arc);
        LineValidationResult ValidateLine(Vector vector);
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
        UnableToConstructArc
    }

    public static class PathValidatorExtensions
    {
        public static bool IsOk(this IPathValidator validator, ArcDefinition arc)
        {
            if (arc is null)
                return false;
            if (validator is null)
                return true;
            var result = validator.ValidateArc(arc);
            return result == ArcValidationResult.Ok;
        }
    }
}
