#if COREFX
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif

namespace iSukces.DrawingPanel.Paths
{
    public static class PathDebug
    {
        public static string CsCode(this ArcDefinition x, string name = "")
        {
            var center = x.Center.CsCode();
            var start  = x.Start.CsCode();
            var dir    = x.DirectionStart.CsCode();
            var end    = x.End.CsCode();
            var code   = $"new ArcDefinition({center}, {start}, {dir}, {end})";
            return string.IsNullOrEmpty(name) ? code : $"var {name} = {code};\r\n";
        }

        public static string CsCode(this Point p, string name = "")
        {
            var code = $"new Point({p.X.CsCode()},{p.Y.CsCode()})";
            return string.IsNullOrEmpty(name) ? code : $"var {name} = {code};\r\n";
        }

        public static string CsCode(this Vector p, string name = "")
        {
            var code = $"new Vector({p.X.CsCode()},{p.Y.CsCode()})";
            return string.IsNullOrEmpty(name) ? code : $"var {name} = {code};\r\n";
        }
    }
}
