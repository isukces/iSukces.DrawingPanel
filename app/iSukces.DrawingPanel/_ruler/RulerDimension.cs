namespace iSukces.DrawingPanel;

public struct RulerDimension : IEquatable<RulerDimension>
{
    public static bool operator ==(RulerDimension left, RulerDimension right) { return left.Equals(right); }

    public static bool operator !=(RulerDimension left, RulerDimension right) { return !left.Equals(right); }

    public bool Equals(RulerDimension other)
    {
        return Major.Equals(other.Major)
               && MinorCount == other.MinorCount
               && Scale.Equals(other.Scale) &&
               Offset.Equals(other.Offset);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return obj is RulerDimension dimension && Equals(dimension);
    }

    public RulerDimensionCalculator GetCalculator()
    {
        var scale = Scale;
        if (IsReverse)
            scale = -scale;
        return new RulerDimensionCalculator(scale, Major, MinorCount, Offset, DisplayValueOffset);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Major.GetHashCode();
            hashCode = (hashCode * 397) ^ MinorCount;
            hashCode = (hashCode * 397) ^ Scale.GetHashCode();
            hashCode = (hashCode * 397) ^ Offset.GetHashCode();
            return hashCode;
        }
    }


    public double Major { get; set; }

    public int MinorCount
    {
        get => _minorCountMinus1 + 1;
        set => _minorCountMinus1 = Math.Max(value - 1, 0);
    }

    public double Scale
    {
        get => _scaleMinus1 + 1;
        set
        {
            if (value <= 0)
                value = 1;
            _scaleMinus1 = value - 1;
        }
    }

    public double Offset { get; set; }

    public bool IsReverse { get; set; }

    /// <summary>
    ///     Wartość dodawana do wyświetlanych etykiet
    /// </summary>
    public double DisplayValueOffset { get; set; }

    public static RulerDimension Default = new RulerDimension
    {
        Major      = 100,
        MinorCount = 10
    };

    private double _scaleMinus1;
    private int _minorCountMinus1;
}
