using Point=iSukces.Mathematics.Point;

namespace iSukces.DrawingPanel.Paths.Test;

public sealed class DefaultPathValidatorPd
{
    private DefaultPathValidatorPd()
    {
    }

    public IPathValidator GetValidator(int arg)
    {
        return _validator;
    }

    public static DefaultPathValidatorPd Instance => DefaultPathValidatorHolder.SingleIstance;

    private readonly IPathValidator _validator = new PdPathValidator();

    private static class DefaultPathValidatorHolder
    {
        public static readonly DefaultPathValidatorPd SingleIstance = new DefaultPathValidatorPd();
    }

    private sealed class PdPathValidator : MinimumValuesPathValidator
    {
        public override ArcValidationResult ValidateArc(ArcDefinition arc, ArcDestination arcDestination)
        {
            if (arc.Angle >= 180)
                return ArcValidationResult.ReplaceByLine;
            return base.ValidateArc(arc, arcDestination);
        }

        public PdPathValidator()
            : base(5, 0.001)
        {
        }

        public override CircleCrossValidationResult ValidatePointForCircleConnectionValid(PathRay start,
            PathRay end, Point cross)
        {
            var v        = end.Point - start.Point;
            var lSquared = v.LengthSquared;
            if (lSquared < minLength * minLength)
                return CircleCrossValidationResult.ForceLine;

            const double limit        = 10_000;
            const double limitSquared = limit * limit;

            var v1 = cross - start.Point;
            var t1 = v1.LengthSquared / lSquared;
            if (t1 > limitSquared)
                return Reject;

            var v2 = cross - end.Point;
            var t2 = v2.LengthSquared / lSquared;
            if (t2 > limitSquared)
                return Reject;
            return CircleCrossValidationResult.Ok;
        }

        private const double minLength = 0.10; // nie bawię się w łuki na odległości mniejszej niż 10cm

        const CircleCrossValidationResult Reject = CircleCrossValidationResult.Invalid;
    }
}
