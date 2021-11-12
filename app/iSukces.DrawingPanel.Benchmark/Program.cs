﻿using BenchmarkDotNet.Running;

namespace iSukces.DrawingPanel.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var s2 = BenchmarkRunner.Run<MathOperationsBenchmark>();
            //var s2 = BenchmarkRunner.Run<VectorNormalizeBenchmark>();
            //var s2 = BenchmarkRunner.Run<VectorAngleBetweenBenchmark>();
        }
    }
}
