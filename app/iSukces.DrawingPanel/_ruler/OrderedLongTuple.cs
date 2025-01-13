#nullable disable
namespace iSukces.DrawingPanel;

public sealed class OrderedLongTuple
{
    public OrderedLongTuple(long minimum, long maximum)
    {
        if (minimum <= maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }
        else
        {
            Minimum = maximum;
            Maximum = minimum;
        }
    }

    public override string ToString() { return $"{Minimum} ... {Maximum}"; }

    public long Minimum { get; }

    public long Maximum { get; }
}
