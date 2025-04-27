namespace iSukces.DrawingPanel;

public struct TickConfigutation
{
    public TickConfigutation(double major, int minorCount)
    {
        Major      = major;
        MinorCount = minorCount;
    }

    public static TickConfigutation FindForScale(double scale1)
    {
        var       possible    = new[] { 1, 2, 5 };
        var       possible2   = new[] { 5, 4, 5 };
        const int minDistance = 90;

        var scale      = (decimal)scale1;
        var baseFactor = 100m;
        while (true)
        {
            if (baseFactor * scale < minDistance)
                break;
            baseFactor *= 0.1m;
        }

        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < possible.Length; j++)
            {
                var distance = baseFactor * possible[j];
                if (distance * scale >= minDistance)
                    return new TickConfigutation((double)distance, possible2[j]);
            }

            baseFactor *= 10;
        }

        return new TickConfigutation(100, 10);
    }

    public override string ToString()
    {
        return $"Major={Major}, MinorCount={MinorCount}";
    }

    public double Major      { get; }
    public int    MinorCount { get; }
}
