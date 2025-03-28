#nullable disable
#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace iSukces.DrawingPanel.Paths;

[DebuggerDisplay("{GetCreationCode()}")]
public class MinimumValuesPathValidator : IPathValidator, IMinRadiusPathValidator
{
    public MinimumValuesPathValidator(double minRadius, double minLineLength)
    {
        _minRadius      = minRadius;
        _minRadius2     = minRadius * minRadius;
        _minLineLength  = minLineLength;
        _minLineLength2 = minLineLength * minLineLength;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsTooSmall(Vector v, double minLengthSquared)
    {
        return v.LengthSquared < minLengthSquared;
    }

    internal string GetCreationCode()
    {
        return $"new MinimumValuesPathValidator({_minRadius.CsCode()}, {_minLineLength.CsCode()})";
    }

    public double GetMinRadius()
    {
        return _minRadius;
    }


    public virtual ArcValidationResult ValidateArc(ArcDefinition arc, ArcDestination arcDestination)
    {
        if (arc.Radius < _minRadius)
            return ArcValidationResult.RadiusTooSmall;
        return ArcValidationResult.Ok;
    }

    public LineValidationResult ValidateLine(Vector vector)
    {
        if (IsTooSmall(vector, _minLineLength2))
            return LineValidationResult.TooShort;
        return LineValidationResult.Ok;
    }

    public virtual CircleCrossValidationResult ValidatePointForCircleConnectionValid(PathRay start, PathRay end,
        Point cross)
    {
        return CircleCrossValidationResult.Ok;
    }

    #region Fields

    private readonly double _minLineLength;
    private readonly double _minLineLength2;
    private readonly double _minRadius;
    private readonly double _minRadius2;

    #endregion
}
