#if NET5_0
using iSukces.Mathematics.Compatibility;

#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    public sealed class TwoReferencePointsPathCalculator : ReferencePointPathCalculator
    {
        public IPathResult Compute(IPathValidator validator)
        {
            var tmp = ComputeInternal(validator, out var result);
            if (tmp is not null)
                return tmp;
            var line = new LinePathElement(Start.Point, End.Point);
            return new PathResult(Start.Point, End.Point, new[] { line });
        }

        private IPathResult ComputeInternal(IPathValidator validator, out ArcValidationResult result)
        {
            var firstInvalid  = !Reference1.HasValidVector();
            var secondInvalid = !Reference2.HasValidVector();
            validator ??= EverythingOkPathValidator.Instance;

            if (secondInvalid || firstInvalid)
            {
                var refVector            = Reference2.Point - Reference1.Point;
                var lineValidationResult = validator.ValidateLine(refVector);

                if (lineValidationResult != LineValidationResult.Ok)
                {
                    result = ArcValidationResult.NoCrossPoints;
                    return null;
                }

                if (firstInvalid)
                    Reference1 = Reference1.With(refVector);
                if (secondInvalid)
                    Reference2 = Reference2.With(refVector);
            }

            var builder = new PathBuilder(Start.Point, validator);
            builder.AddConnectionAutomatic(Start, Reference1, false);
            builder.AddConnectionAutomatic(Reference1, Reference2, false);
            builder.AddConnectionAutomatic(Reference2, End, true);

            result = ArcValidationResult.Ok;
            return builder.LineToAndCreate(End.Point);
        }

        public override void InitDemo()
        {
            Start      = new PathRay(new Point(-20, 0), new Vector(200, 100));
            End        = new PathRay(new Point(100, 0), new Vector(-100, 100));
            Reference1 = new PathRay(new Point(40, 20), new Vector());
            Reference2 = new PathRay(new Point(60, 20), new Vector());
        }

        public override void SetReferencePoint(Point p, int nr)
        {
            switch (nr)
            {
                case 0:
                    Reference1 = Reference1.With(p);
                    break;
                case 1:
                    Reference2 = Reference2.With(p);
                    break;
            }
        }

        #region properties

        public PathRay Reference1 { get; set; }
        public PathRay Reference2 { get; set; }

        #endregion
    }
}
