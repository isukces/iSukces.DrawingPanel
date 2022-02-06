#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using JetBrains.Annotations;


namespace iSukces.DrawingPanel.Paths
{
    public sealed class EverythingOkPathValidator : IPathValidator
    {
        private EverythingOkPathValidator()
        {
        }

        public CircleCrossValidationResult ValidatePointForCircleConnectionValid(PathRay start, PathRay end, Point cross)
        {
            return CircleCrossValidationResult.Ok;
        }

        public ArcValidationResult ValidateArc(ArcDefinition arc)
        {
            return ArcValidationResult.Ok;
        }

        public LineValidationResult ValidateLine(Vector vector)
        {
            return LineValidationResult.Ok;
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
}
