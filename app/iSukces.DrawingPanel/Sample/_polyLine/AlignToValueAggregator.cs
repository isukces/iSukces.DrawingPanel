using System;

namespace iSukces.DrawingPanel.Sample;

internal struct AlignToValueAggregator
{
    public AlignToValueAggregator(double value)
    {
        Value        = value;
        _starting    = value;
        _best        = double.MaxValue;
        HasAlignment = false;
    }

    public bool HasAlignment { get; private set; }

    private double _best;

    private readonly double _starting;
    public double Value { get; set; }

    public bool TryAccept(double value, double delta)
    {
        var currentDelta = Math.Abs(value - _starting);
        if (currentDelta > delta || currentDelta > _best)
            return false;
        _best = currentDelta;
        Value = value;
        return HasAlignment = true;
    }
}