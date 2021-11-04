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
}
