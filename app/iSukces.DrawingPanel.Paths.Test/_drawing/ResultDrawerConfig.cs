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
            ExtraDrawing = g =>
            {
                g.GrayCross(calc.Reference1.Point);
                g.GrayCross(calc.Reference2.Point);
            };
            return this;
        }

        public ResultDrawerConfig WithReverseEndMarker()
        {
            Flags |= ResultDrawerConfigFlags.ReverseEndMarker;
            return this;
        }

        public PathRay                  Start           { get; set; }
        public PathRay                  End             { get; set; }
        public IPathResult              Result          { get; set; }
        public TestName                 Title           { get; set; }
        public Action<ResultDrawer>     ExtraDrawing    { get; set; }
        public Func<IEnumerable<Point>> ExtraPoints     { get; set; }
        public DirectoryInfo            OutputDirectory { get; set; }

        public ResultDrawerConfigFlags Flags { get; set; }
    }

    [Flags]
    public enum ResultDrawerConfigFlags
    {
        None = 0,
        ReverseEndMarker = 1,
    }
}
