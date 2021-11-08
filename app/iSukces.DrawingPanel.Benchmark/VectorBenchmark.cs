using System;
using System.Windows;
using BenchmarkDotNet.Attributes;

namespace iSukces.DrawingPanel.Benchmark
{
    
    [SimpleJob(targetCount: 10)]
    public class VectorBenchmark
    {
        [Benchmark(Description = "Minus")]
        public Vector Minus() { return _p2 - _p1; }

        [Benchmark(Description = "None point")]
        public Point None_Point() { return _p2; }


        [Benchmark(Description = "None vector")]
        public Vector None_Vector() { return _v1; }


        [Benchmark(Description = "Vector Length")]
        public double Vector_lenght() { return _v1.Length; }


        [Benchmark(Description = "Vector Length squared")]
        public double Vector_lenght_squared() { return _v1.LengthSquared; }

        [Benchmark(Description = "Vector normalize")]
        public Vector Vector_normalize()
        {
            var v = _p2 - _p1;
            v.Normalize();
            return v;
        }

        
        [Benchmark(Description = "Vector/Length")]
        public Vector Vector_div_length()
        {
            var v = _p2 - _p1;
            return v / v.Length;
        }

        [Benchmark(Description = "Vector normalize like normal")]
        public Vector Vector_normalize_like_normal()
        {
            var v = _p2 - _p1;
            v /= Math.Max(Math.Abs(v.X), Math.Abs(v.Y));
            var vLength = v.Length;
            return new Vector(v.X / vLength, v.Y / vLength);
        }

        private readonly Point _p1 = new(1, 2);
        private readonly Point _p2 = new(3, 5);
        private readonly Vector _v1 = new(1, 2);
    }
}
