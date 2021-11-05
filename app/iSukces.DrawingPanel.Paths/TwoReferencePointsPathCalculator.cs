using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public sealed class TwoReferencePointsPathCalculator : ReferencePointPathCalculator
    {
        public IPathResult Compute(IPathValidator validator)
        {
            var tmp = ComputeInternal(validator, out var result);
            if (tmp is not null)
                return tmp;
            return InvalidPathElement.MakeInvalid(Start, End, result);
        }

        private IPathResult ComputeInternal(IPathValidator validator, out ArcValidationResult result)
        {
            validator ??= EverythingOkPathValidator.Instance;

            var refVector            = Reference2.Point - Reference1.Point;
            var lineValidationResult = validator.ValidateLine(refVector);

            if (lineValidationResult != LineValidationResult.Ok)
            {
                result = ArcValidationResult.NoCrossPoints;
                return null;
            }

            Reference1 = Reference1.With(refVector);
            Reference2 = Reference2.With(refVector);

            var builder = new PathBuilder(Start.Point, validator);
            builder.AddConnectionAutomatic(Start, Reference1);
            builder.LineTo(Reference2.Point);
            builder.AddConnectionAutomatic(Reference2, End.WithInvertedVector());

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

        public PathRay Reference1 { get; set; }
        public PathRay Reference2 { get; set; }
    }
}
