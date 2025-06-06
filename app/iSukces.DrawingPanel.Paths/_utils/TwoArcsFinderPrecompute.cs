#nullable disable
#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System;
using JetBrains.Annotations;


namespace iSukces.DrawingPanel.Paths;

/// <summary>
///     Contains precomputed values for solving square equation
/// </summary>
internal class TwoArcsFinderPrecompute
{
    public TwoArcsFinderPrecompute(TwoArcsFinder finder)
        // : this()
    {
        _owner_StartCenterSearch = finder.StartCenterSearch;
        _owner_EndCenterSearch   = finder.EndCenterSearch;
        _owner_StartDirection    = finder.StartDirection;
        _owner_EndDirection      = finder.EndDirection;
    }


    public bool UpdateCompute(bool useSmallerRadius, [CanBeNull] IMinRadiusPathValidator pathValidator)
    {
        var radius = useSmallerRadius ? Radius1 : Radius2;
        if (radius > 0)
        {
            if (radius > MaxRadius)
                return false;
        }
        else
        {
            if (-radius > MaxRadius)
                return false;
        }

        if (pathValidator != null)
        {
            var minRadis = pathValidator.GetMinRadius();
            if (radius * radius < minRadis * radius)
                return false;
        }

        var   center1 = _owner_StartCenterSearch.Get(radius);
        var   center2 = _owner_EndCenterSearch.Get(radius);
        Point cross;
        {
            // c1 = p1 + v1 * radius
            // c2 = p2 + v2 * radius
            // (c1 + c2) / 2 = (p1 + p2) / 2 + (v1 + v2) / 2 * radius
            // sometimes radius is very big and (v1 + v2) is very small or even zero 
            // so { (c1 + c2) / 2 } can have errors 
            cross = PathsMathUtils.Average(_owner_StartCenterSearch.Point, _owner_EndCenterSearch.Point);
            if (!AIsZero)
            {
                var ve   = new Vector(SumX, SumY);
                var move = ve * (radius * 0.5);
                cross += move;
            }
        }

        Arc1 = new ArcDefinition(center1, _owner_StartCenterSearch.Point, _owner_StartDirection,
            cross);

        var arc3 = new ArcDefinition(center2, _owner_EndCenterSearch.Point, _owner_EndDirection,
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

    #region properties

    public double MaxRadius { get; set; }

    #endregion

    #region Fields

    private readonly PathRay _owner_EndCenterSearch;
    private readonly Vector _owner_EndDirection;
    private readonly PathRay _owner_StartCenterSearch;
    private readonly Vector _owner_StartDirection;
    private bool _radiusNotEqual;

    public bool AIsZero;
    public ArcDefinition Arc1;
    public ArcDefinition Arc2;

    public double Radius1;
    public double Radius2;

    public double SumX;
    public double SumY;

    #endregion
}
