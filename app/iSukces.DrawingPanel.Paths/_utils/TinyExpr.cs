namespace iSukces.DrawingPanel.Paths
{
    internal class TinyExpr
    {
        public TinyExpr(double value) { _code = value.Str(); }

        public TinyExpr(string code) { _code = code; }


        public static TinyExpr operator +(TinyExpr a, TinyExpr b) { return new TinyExpr($"{a} + {b}"); }


        public static TinyExpr operator +(TinyExpr a, double b)
        {
            var bb = new TinyExpr(b);
            return a + bb;
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

        public static TinyExpr operator -(TinyExpr a, TinyExpr b) { return new TinyExpr($"{a} - {b}"); }

        public static TinyExpr operator -(TinyExpr a, double b)
        {
            var bb = new TinyExpr(b);
            return a - bb;
        }


        public TinyExpr Brackets() { return new TinyExpr("(" + _code + ")"); }

        public override string ToString() { return _code; }

        private readonly string _code;
    }
}
