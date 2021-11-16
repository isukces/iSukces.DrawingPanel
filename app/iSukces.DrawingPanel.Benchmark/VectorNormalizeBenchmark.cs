using System.Windows;
using BenchmarkDotNet.Attributes;
using iSukces.DrawingPanel.Paths;

namespace iSukces.DrawingPanel.Benchmark
{
    [SimpleJob(targetCount: 8)]
    public class VectorNormalizeBenchmark
    {
        [Benchmark(Description = "Vector normalize", Baseline = true)]
        public object Vector_normalize()
        {
            var vv = new Vector[Iterations];
            for (var i = 0; i < Iterations; i++)
            {
                var v = new Vector(3, 4);
                v.Normalize();
                vv[i] = v;
            }

            return vv;
        }

        [Benchmark(Description = "Vector normalize fast EXT")]
        public object Vector_normalize_fast()
        {
            var vv = new Vector[Iterations];
            for (var i = 0; i < Iterations; i++)
            {
                var v = new Vector(3, 4);
                v     = v.NormalizeFast();
                vv[i] = v;
            }

            return vv;
        }


        const int Iterations = 100;
    }
}