using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal sealed class ResultDrawerConfig
    {
        public void Draw([CallerFilePath] string path = null) { ResultDrawer.Draw(this, path); }

        public FileInfo GetImageFile()
        {
            var fileName = Title.GetFileName();
            var fi       = new FileInfo(Path.Combine(OutputDirectory.FullName, fileName + ".png"));
            return fi;
        }

        public ResultDrawerConfig With(TwoReferencePointsPathCalculator calc)
        {
            ExtraDrawingTop = g =>
            {
                g.DrawCircleWithVector(calc.Reference1, true);
                g.DrawCircleWithVector(calc.Reference2, true);
            };
            return this;
        }

        public ResultDrawerConfig With(ThreeReferencePointsPathCalculator calc)
        {
            ExtraDrawingTop = g =>
            {
                g.DrawCircleWithVector(calc.Reference1, true);
                g.DrawCircleWithVector(calc.Reference2, true);
                g.DrawCircleWithVector(calc.Reference3, true);
            };
            ExtraPoints = () =>
            {
                return new[] { calc.Reference1.Point, calc.Reference2.Point, calc.Reference3.Point };
            };
            return this;
        }

        public PathRay                  Start              { get; set; }
        public PathRay                  End                { get; set; }
        public IPathResult              Result             { get; set; }
        public TestName                 Title              { get; set; }
        public Action<ResultDrawer>     ExtraDrawingBottom { get; set; }
        public Action<ResultDrawer>     ExtraDrawingTop    { get; set; }
        public Func<IEnumerable<Point>> ExtraPoints        { get; set; }
        public DirectoryInfo            OutputDirectory    { get; set; }

        public ResultDrawerConfigFlags Flags { get; set; }
    }

    [Flags]
    public enum ResultDrawerConfigFlags
    {
        None = 0,
        ReverseEndMarker = 1,
    }
}
