using System;
using BenchmarkDotNet.Attributes;

namespace iSukces.DrawingPanel.Benchmark
{
    [SimpleJob(targetCount: 10/*, invocationCount:1_0000_000*/)]
    public class MathOperationsBenchmark
    {
        [Benchmark(Description = "Sin")]
        public double TestSin()
        {
            return Math.Sin(X);
        }
        
        [Benchmark(Description = "Cos")]
        public double TestCos()
        {
            return Math.Cos(X);
        }
        
        
        [Benchmark(Description = "aCos")]
        public double TestACos()
        {
            return Math.Acos(X);
        }

        [Benchmark(Description = "SinDEG")]
        public double TestSinDeg()
        {
            const double mul = Math.PI / 180;
            return Math.Sin(X * mul);
        }

        [Benchmark(Description = "SinDEG alt")]
        public double TestSinDeg2()
        {
            const double div = 180 / Math.PI;
            return Math.Sin(X / div);
        }

        [Benchmark(Description = "Sqrt")]
        public double TestSqrt()
        {
            return Math.Sqrt(X);
        }
        
        [Benchmark(Description = "Atan2")]
        public double Atan2()
        {
            return Math.Atan2(X, Y);
        }

        
        [Benchmark(Description = "Exp")]
        public double TestExp()
        {
            return Math.Exp(X);
        }

        
        [Benchmark(Description = "Log")]
        public double TestLog()
        {
            return Math.Log(X);
        }
        [Benchmark(Description = "Log10")]
        public double TestLog10()
        {
            return Math.Log10(X);
        }

        [Params(0.5)]
        public double X { get; set; }
        
        [Params(0.8)]
        public double Y { get; set; }
    }
}
