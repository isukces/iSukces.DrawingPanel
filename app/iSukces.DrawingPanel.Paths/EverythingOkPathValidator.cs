using JetBrains.Annotations;
#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    public sealed class EverythingOkPathValidator : IPathValidator
    {
        private EverythingOkPathValidator() { }

        public ArcValidationResult ValidateArc(ArcDefinition arc) { return ArcValidationResult.Ok; }

        public LineValidationResult ValidateLine(Vector vector) { return LineValidationResult.Ok; }

        [NotNull]
        public static EverythingOkPathValidator Instance => EverythingOkPathValidatorHolder.SingleIstance;

        private static class EverythingOkPathValidatorHolder
        {
            public static readonly EverythingOkPathValidator SingleIstance = new();
        }
    }
}
