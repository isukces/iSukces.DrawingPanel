#nullable disable
#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using JetBrains.Annotations;


namespace iSukces.DrawingPanel.Paths;

public sealed class EverythingOkPathValidator : IPathValidator
{
    private EverythingOkPathValidator()
    {
    }

    public ArcValidationResult ValidateArc(ArcDefinition arc, ArcDestination arcDestination)
    {
        return ArcValidationResult.Ok;
    }

    public LineValidationResult ValidateLine(Vector vector)
    {
        return LineValidationResult.Ok;
    }

    public CircleCrossValidationResult ValidatePointForCircleConnectionValid(PathRay start, PathRay end,
        Point cross)
    {
        return CircleCrossValidationResult.Ok;
    }

    #region properties

    [NotNull]
    public static EverythingOkPathValidator Instance => EverythingOkPathValidatorHolder.SingleIstance;

    #endregion

    private static class EverythingOkPathValidatorHolder
    {
        #region Fields

        public static readonly EverythingOkPathValidator SingleIstance = new();

        #endregion
    }
}
