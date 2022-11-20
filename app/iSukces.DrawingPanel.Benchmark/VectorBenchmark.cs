using System;
using BenchmarkDotNet.Attributes;
using iSukces.DrawingPanel.Paths;

#if COMPATMATH
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Benchmark
{
    [SimpleJob(targetCount: 8)]
    // [MemoryDiagnoser]
    public class VectorBenchmark
    {
 
        [Benchmark(Description = "Minus")]
        public Vector Minus() { return _p2 - _p1; }

        [Benchmark(Description = "None point")]
        public Point None_Point() { return _p2; }


        [Benchmark(Description = "None vector")]
        public Vector None_Vector() { return _v1; }

        [Benchmark(Description = "AngleBetween")]
        public double Vector_angle_between() { return Vector.AngleBetween(_v1, _v2); }


        [Benchmark(Description = "Vector/Length")]
        public Vector Vector_div_length()
        {
            var v = _p2 - _p1;
            return v / v.Length;
        }


        [Benchmark(Description = "Vector Length")]
        public double Vector_lenght() { return _v1.Length; }


        [Benchmark(Description = "Vector Length squared")]
        public double Vector_lenght_squared() { return _v1.LengthSquared; }



        [Benchmark(Description = "Vector normalize like normal 1")]
        public Vector Vector_normalize_like_normal_1()
        {
            var v = _p2 - _p1;
            v /= Math.Max(Math.Abs(v.X), Math.Abs(v.Y));
            var vLength = v.Length;
            return new Vector(v.X / vLength, v.Y / vLength);
        }

        [Benchmark(Description = "Vector normalize like normal 2")]
        public Vector Vector_normalize_like_normal_2()
        {
            var v = _p2 - _p1;
            v /= Math.Max(Math.Abs(v.X), Math.Abs(v.Y));
            var vLength = Math.Sqrt(v.X * v.X + v.Y * v.Y);
            return new Vector(v.X / vLength, v.Y / vLength);
        }

        [Benchmark(Description = "Vector normalize like normal 3")]
        public Vector Vector_normalize_like_normal_3()
        {
            var v = _p2 - _p1;

            var x = v.X;
            var y = v.Y;

            var xMinus = x < 0;
            var yMinus = y < 0;

            var absX = xMinus ? -x : x;
            var absY = yMinus ? -y : y;

            if (absX > absY)
            {
                y /= absX;
                x = xMinus ? -1 : 1;
                var vLength = Math.Sqrt(1 + y * y);
                return new Vector(x / vLength, y / vLength);
            }
            else
            {
                x /= absY;
                y = yMinus ? -1 : 1;
                var vLength = Math.Sqrt(x * x + 1);
                return new Vector(x / vLength, y / vLength);
            }
        }

        [Benchmark(Description = "Vector normalize like normal 4")]
        public Vector Vector_normalize_like_normal_4()
        {
            var v = _p2 - _p1;

            var x = v.X;
            var y = v.Y;
            FastVector.Normalize(ref x, ref y);
            return new Vector(x, y);
        }
        
 
        [Benchmark(Description = "FastVector normalize")]
        public FastVector FastVector_normalize()
        {
            var v = new FastVector(1, 2);
            return v.Normalize();
        }

 
        
        [Benchmark(Description = "FastVector create")]
        public FastVector FastVector_create()
        {
            var v = new FastVector(1, 2);
            return v;
        }

        
  
 
        private readonly Point _p1 = new(1, 2);
        private readonly Point _p2 = new(3, 5);

        private readonly Vector _v1 = new(1, 2);
        private readonly Vector _v2 = new(3, 1);
 


        const int Iterations = 100;


      
    }
}
