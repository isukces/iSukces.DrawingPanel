using System;
using System.Windows;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel.Paths
{
    /// <summary>
    ///     Contains precomputed values for solving square equation
    /// </summary>
    internal struct TwoArcsFinderPrecompute
    {
        public TwoArcsFinderPrecompute(TwoArcsFinder owner)
            : this()
        {
            _owner = owner; 
            
        }

        public bool UpdateCompute(bool useSmallerRadius, [CanBeNull] IMinRadiusPathValidator pathValidator)
        {
            var radius = useSmallerRadius ? Radius1 : Radius2;

            if (pathValidator != null)
            {
                var minRadis = pathValidator.GetMinRadius();
                if (radius * radius < minRadis * radius)
                    return false;
            }

            var   center1 = _owner.StartCenterSearch.Get(radius);
            var   center2 = _owner.EndCenterSearch.Get(radius);
            Point cross;
            {
                // c1 = p1 + v1 * radius
                // c2 = p2 + v2 * radius
                // (c1 + c2) / 2 = (p1 + p2) / 2 + (v1 + v2) / 2 * radius
                // sometimes radius is very big and (v1 + v2) is very small or even zero 
                // so { (c1 + c2) / 2 } can have errors 
                cross = MathUtils.Average(_owner.StartCenterSearch.Point, _owner.EndCenterSearch.Point);
                if (!AIsZero)
                {
                    var ve   = new Vector(SumX, SumY);
                    var move = ve * (radius * 0.5);
                    cross += move;
                }
            }

            Arc1 = ArcDefinition.FromCenterAndArms(center1, _owner.StartCenterSearch.Point, _owner.StartDirection,
                cross);

            var arc3 = ArcDefinition.FromCenterAndArms(center2, _owner.EndCenterSearch.Point, _owner.EndDirection,
                cross);

            Arc2 = arc3.GetComplementar();

            if (radius < 0)
                radius = -radius;
 
            if (PathCalculationConfig.CheckRadius)
            {
                var arc1Radius = radius - Arc1.Radius;
                if (Math.Abs(arc1Radius) > 0.000001)
                    _radiusNotEqual = true;
                var arc2Radius = radius - Arc2.Radius;
                if (Math.Abs(arc2Radius) > 0.000001)
                    _radiusNotEqual = true;
            }
            else
                _radiusNotEqual = false;
 

            Arc1.UseRadius(radius);
            Arc2.UseRadius(radius);
            return true;
        }

        private readonly TwoArcsFinder _owner;

        public bool AIsZero;

        public double Radius1;
        public double Radius2;

        public double SumX;
        public double SumY;
        public ArcDefinition Arc1;
        public ArcDefinition Arc2;
        private bool _radiusNotEqual;
    }
}
