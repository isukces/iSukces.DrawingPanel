using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using iSukces.DrawingPanel.Paths;

namespace iSukces.DrawingPanel.Benchmark;

[SimpleJob(RunStrategy.Throughput, launchCount: 1)]
[MemoryDiagnoser]
public class PathsBenchmark
{
    [Benchmark]
    public IPathResult ZeroPararell()
    {
        PathRay start = new PathRay(0, 0, 1, 0);
        PathRay end   = new PathRay(10, 1, 1, 0);
        var     a     = ZeroReferencePointPathCalculator.Compute(start, end, null);
        return a;
    }

    [Benchmark]
    public IPathResult ZeroNormal()
    {
        PathRay start = new PathRay(0, 0, 1, 0.1);
        PathRay end   = new PathRay(10, 1, 1, -0.1);
        var     a     = ZeroReferencePointPathCalculator.Compute(start, end, null);
        return a;
    }

}