#if DEBUG && USE_TINYEXPR
namespace iSukces.DrawingPanel.Paths
{
    internal class TinyExpr
    {
        public TinyExpr(double value)
        {
            _code = value.CsCode();
        }

        public TinyExpr(string code)
        {
            _code = code;
        }

        public static TinyExpr operator +(TinyExpr a, TinyExpr b)
        {
            return new TinyExpr($"{a} + {b}");
        }


        public static TinyExpr operator +(TinyExpr a, double b)
        {
            var bb = new TinyExpr(b);
            return a + bb;
        }

        public static TinyExpr operator /(TinyExpr a, TinyExpr b)
        {
            return new TinyExpr(a._code + " / " + b._code);
        }

        public static TinyExpr operator *(TinyExpr a, TinyExpr b)
        {
            if (a._code == b._code)
                return new TinyExpr($"({a})^2");
            return new TinyExpr($"{a} * {b}");
        }

        public static TinyExpr operator *(TinyExpr a, double b)
        {
            var bb = new TinyExpr(b);
            return a * bb;
        }

        public static TinyExpr operator *(double b, TinyExpr a)
        {
            var bb = new TinyExpr(b);
            return bb * a;
        }

        public static TinyExpr operator -(TinyExpr a, TinyExpr b)
        {
            return new TinyExpr($"{a} - {b}");
        }

        public static TinyExpr operator -(TinyExpr a, double b)
        {
            return new TinyExpr($"{a} - {b.ToExpr()}");
        }


        public static TinyExpr operator -(TinyExpr a)
        {
            a = a.Brackets();
            return new TinyExpr($"-{a}");
        }

        public static TinyExpr Square(TinyExpr a, TinyExpr b, TinyExpr c, TinyExpr expr)
        {
            a = a.Brackets();
            b = b.Brackets();
            c = c.Brackets();
            return a * a * expr * expr + b * expr + c;
        }

        public static TinyExpr SquareDelta(TinyExpr a, TinyExpr b, TinyExpr c)
        {
            a = a.Brackets();
            b = b.Brackets();
            c = c.Brackets();
            return b * b - 4 * a * c;
        }

        public static TinyExpr SquareSolve1(TinyExpr a, TinyExpr b, TinyExpr c)
        {
            TinyExpr deltaExpr = SquareDelta(a, b, c);
            var      licznik   = -(b.Brackets()) + deltaExpr.Sqrt();
            var      mianownik = 2 * a.Brackets();
            return licznik.Brackets() / mianownik.Brackets();
        }

        public static TinyExpr SquareSolve2(TinyExpr a, TinyExpr b, TinyExpr c)
        {
            TinyExpr deltaExpr = SquareDelta(a, b, c);
            var      licznik   = -(b.Brackets()) - deltaExpr.Sqrt();
            var      mianownik = 2 * a.Brackets();
            licznik   = licznik.Brackets();
            mianownik = mianownik.Brackets();
            return licznik / mianownik;
        }

        public TinyExpr BFloat()
        {
            return new TinyExpr("bfloat(" + _code + ")");
        }


        public TinyExpr Brackets()
        {
            return new TinyExpr("(" + _code + ")");
        }

        public TinyExpr Negate()
        {
            return new TinyExpr("-" + _code);
        }

        public TinyExpr Plot(string varName, double min, double max)
        {
            var code = "plot2d([" + this + "], [" + varName + "," + min.ToExpr() + "," + max.ToExpr() + "])";
            return new TinyExpr(code);
        }

        public TinyExpr Sqrt()
        {
            return new TinyExpr("sqrt" + Brackets()._code);
        }

        public override string ToString()
        {
            return _code;
        }

        #region Fields

        private readonly string _code;

        #endregion
    }


    internal static partial class PathsExtensions
    {
        internal static TinyExpr ToExpr(this double d)
        {
            return new TinyExpr(d);
        }

        public static string DebugCreate(PathRay start, string name)
        {
            var p = start.Point;
            var v = start.Vector;
            return $"{name} = new {nameof(PathRay)}({p.X.ToExpr()}, {p.Y.ToExpr()}, {v.X.ToExpr()}, {v.Y.ToExpr()});";
        }

    }
}
#endif
