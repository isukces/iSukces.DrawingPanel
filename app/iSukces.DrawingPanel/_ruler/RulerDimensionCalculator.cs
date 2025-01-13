#nullable disable
namespace iSukces.DrawingPanel;

/// <summary>
///     Powstaje z RulerDimension i ma przygotowane wartości pośrednie przyspieszające obliczenia
/// </summary>
public struct RulerDimensionCalculator
{
    public RulerDimensionCalculator(double scale, double major, int minorCount, double offset,
        double displayValueOffset)
    {
        _scale              = scale;
        _offset             = offset;
        _displayValueOffset = displayValueOffset;
        Minor               = major / minorCount;
    }

    /// <summary>
    ///     Dla zakresu na kontrolce odtwarzamy nr kresek
    /// </summary>
    /// <param name="min">w pixelach</param>
    /// <param name="max">w pixelach</param>
    /// <returns></returns>
    public OrderedLongTuple GetTickRange(double min, double max)
    {
        if (max < min) return null;
        try
        {
            var scalledItem = 1 / (Minor * _scale);
            var tmp         = _displayValueOffset * _scale - _offset * _scale;
            min += tmp;
            max += tmp;
            var aMin = MathUtils.RoundToLong(min * scalledItem);
            var aMax = MathUtils.RoundToLong(max * scalledItem);
            return new OrderedLongTuple(aMin, aMax);
        }
        catch
        {
            return null; // nie udało się policzyć
        }
    }

    public RulerValueAndDrawPosition GetValue(long nr)
    {
        var value               = nr * Minor;
        var valueToDrawPosition = ValueToDrawPosition(value - _displayValueOffset);
        return new RulerValueAndDrawPosition(value, valueToDrawPosition);
    }

    public double ValueToDrawPosition(double value) { return (value + _offset) * _scale; }

    /// <summary>
    ///     Wartość dodawana do wyświetlanych etykiet
    /// </summary>
    private readonly double _displayValueOffset;

    private readonly double _scale;
    private readonly double _offset;
    public readonly double Minor;
}
