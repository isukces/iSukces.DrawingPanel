using System;
using System.Windows;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel.Paths
{
    /// <summary>
    ///     Contains precomputed values for solving square equation
    /// </summary>
    internal class TwoArcsFinderPrecompute
    {
        public TwoArcsFinderPrecompute(TwoArcsFinder owner) { _owner = owner; }

        public bool UpdateCompute(bool useSmallerRadius, [CanBeNull] IMinRadiusPathValidator pathValidator)
        {
            double radius = useSmallerRadius ? radius1 : radius2;

            if (pathValidator != null)
            {
                var minRadis = pathValidator.GetMinRadius();
                if (radius < 0)
                {
                    if (-radius < minRadis)
                        return false;
                }
                else
                {
                    if (radius < minRadis)
                        return false;
                }
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
                if (!aIsZero)
                {
                    var ve   = new Vector(sumX, sumY);
                    var move = ve * (radius * 0.5);
                    cross += move;
                }
            }

            this.arc1 = ArcDefinition.FromCenterAndArms(center1, _owner.StartCenterSearch.Point, _owner.StartDirection,
                cross);

            var arc3 = ArcDefinition.FromCenterAndArms(center2, _owner.EndCenterSearch.Point, _owner.EndDirection,
                cross);

            this.arc2 = arc3.GetComplementar();

            if (radius < 0)
                radius = -radius;
 
            if (PathCalculationConfig.CheckRadius)
            {
                var arc1Radius = radius - arc1.Radius;
                if (Math.Abs(arc1Radius) > 0.000001)
                    radiusNotEqual = true;
                    //throw new Exception("Radius is not equal");
                var arc2Radius = radius - arc2.Radius;
                if (Math.Abs(arc2Radius) > 0.000001)
                    radiusNotEqual = true;
                    // throw new Exception("Radius is not equal");
            }
            else
                this.radiusNotEqual = false;
 

            arc1.UseRadius(radius);
            arc2.UseRadius(radius);
            return true;
        }

        private readonly TwoArcsFinder _owner;

        public bool aIsZero;

        public double radius1;
        public double radius2;

        public double sumX;
        public double sumY;
        public ArcDefinition arc1;
        public ArcDefinition arc2;
        private bool radiusNotEqual;
    }
}
