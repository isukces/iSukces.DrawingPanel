using System;
using Xunit;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=iSukces.Mathematics.Point;
using Vector=iSukces.Mathematics.Vector;
#endif

namespace iSukces.DrawingPanel.Paths.Test;

public class IsAngleBetweenSmallEnoughtBasedOnHTests
{
    [Fact]
    public void T01_Should_calculate()
    {
        var vector1 = new Vector(2, 0);
        var vector2 = new Vector(10, 10);
        var h       = 0.1;

        PathsMathUtils.IsAngleBetweenSmallEnoughtBasedOnH(vector1, vector2, h);
            
        var result = new CodeCopy().Update(vector1, vector2, h);
        var code   = Codemaker.Make(result, nameof(result));

        #region Asserts
        Assert.Equal(2, result.Test_A, 6);
        Assert.Equal(45, result.Reference_AngleBetween, 6);
        Assert.Equal(0.707106781186547, result.Reference_SinAngleBetween, 6);
        Assert.Equal(0.707106781186548, result.Test_AngleBetween, 6);
        Assert.Equal(0.198019801980198, result.Reference_SinusByH, 6);
        Assert.Equal(0.198019801980198, result.Test_SinusByH, 6);
        #endregion
            
    }

    [Fact]
    public void T02_Should_calculate()
    {
        var vector1 = new Vector(2, 0);
        var vector2 = new Vector(10, -10);
        var h       = 0.1;

        PathsMathUtils.IsAngleBetweenSmallEnoughtBasedOnH(vector1, vector2, h);
            
        var result = new CodeCopy().Update(vector1, vector2, h);
        var code   = Codemaker.Make(result, nameof(result));

        #region Asserts
        Assert.Equal(2, result.Test_A, 6);
        Assert.Equal(-45, result.Reference_AngleBetween, 6);
        Assert.Equal(-0.707106781186547, result.Reference_SinAngleBetween, 6);
        Assert.Equal(0.707106781186548, result.Test_AngleBetween, 6);
        Assert.Equal(0.198019801980198, result.Reference_SinusByH, 6);
        Assert.Equal(0.198019801980198, result.Test_SinusByH, 6);
        #endregion

            
    }


    class Codemaker : DpAssertsBuilder
    {
        public static string Make(CodeCopy result, string name)
        {
            var q = new Codemaker();
            return q.Make1(result, name);
        }

        private string Make1(CodeCopy result, string name)
        {
            return Create(() =>
            {
                name += ".";
                Add(result.Test_A, name + nameof(result.Test_A));
                Add(result.Reference_AngleBetween, name + nameof(result.Reference_AngleBetween));
                Add(result.Reference_SinAngleBetween, name + nameof(result.Reference_SinAngleBetween));
                Add(result.Test_AngleBetween, name + nameof(result.Test_AngleBetween));
                    
                Add(result.Reference_SinusByH, name + nameof(result.Reference_SinusByH));
                Add(result.Test_SinusByH, name + nameof(result.Test_SinusByH));
            });
        }
    }

    public class CodeCopy
    {
        public CodeCopy Update(Vector vector1, Vector vector2, double h)
        {
            Reference_AngleBetween    = Vector.AngleBetween(vector1, vector2);
            Reference_SinAngleBetween = Math.Sin(Reference_AngleBetween * (Math.PI / 180));

            {
                var a = vector1.Length;
                Reference_SinusByH = (4 * a * h) / (a * a + h * h*4);
            }

            /*vector1 = new Vector(100, 0);
            h       = 2;*/

            /*
            const double h  = 0.001;
            const double h2 = h * h;
            double       a  = 1000.0 / 2;

            var sin = (2 * a * h) / (h2 + a * a);

            sinus ^2 = (4*A^2*H^2)/(H^2+A^2)^2
            */

            // (2*A*H)/(H^2+A^2)
            var x1 = vector1.X;
            var x2 = vector2.X;
            var y1 = vector1.Y;
            var y2 = vector2.Y;

            HSquare = h * h;
            ASquare = (x1 * x1 + y1 * y1);
            var bSquare = ASquare * 0.25;

            var m = ( HSquare + bSquare);
            m *= m;

            var counter = ASquare * HSquare;
            SinSquareByH = counter / m;

            Cross = x1 * y2 - x2 * y1;

            LengthSquare2 = (x2 * x2 + y2 * y2);
            Sqr           = ASquare * LengthSquare2;

            SinusSquare = (Cross * Cross) / Sqr;
            return this;
            //======================= 
        }

        public double Reference_SinAngleBetween { get; set; }

        public double Reference_AngleBetween { get; set; }

        public double Reference_SinusByH { get; set; }

        public double HSquare       { get; set; }
        public double ASquare       { get; set; }
        public double SinSquareByH  { get; set; }
        public double Cross         { get; set; }
        public double LengthSquare2 { get; set; }
        public double Sqr           { get; set; }
        public double SinusSquare   { get; set; }


        public double Test_SinusByH     => Math.Sqrt(SinSquareByH);
        public double Test_AngleBetween => Math.Sqrt(SinusSquare);
        public double Test_A            => Math.Sqrt(ASquare);
    }
}
