#if COMPATMATH
using Point = iSukces.Mathematics.Compatibility.Point;
using Vector = iSukces.Mathematics.Compatibility.Vector;
#else
using Point = System.Windows.Point;
#endif
using System.IO;
using System.Runtime.CompilerServices;

namespace iSukces.DrawingPanel.Paths.Test;

internal sealed class ArcResultDrawerConfig
{
    public void Draw(ArcPathMaker? maker = null, [CallerFilePath] string? path = null)
    {
        ArcResultDrawer.Draw(this, maker, path);
    }

    public FileInfo GetImageFile()
    {
        var fileName = Title.GetFileName();
        var fi       = new FileInfo(Path.Combine(OutputDirectory.FullName, fileName + ".png"));
        return fi;
    }

    public ArcResultDrawerConfig With(TwoReferencePointsPathCalculator calc)
    {
        ExtraDrawingTop = g =>
        {
            g.DrawCircleWithVector(calc.Reference1, true);
            g.DrawCircleWithVector(calc.Reference2, true);
        };
        return this;
    }

    public ArcResultDrawerConfig With(ThreeReferencePointsPathCalculator calc)
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
    public ArcPathMakerResult       Result             { get; set; }
    public TestName                 Title              { get; set; }
    public Action<ArcResultDrawer>  ExtraDrawingBottom { get; set; }
    public Action<ArcResultDrawer>  ExtraDrawingTop    { get; set; }
    public Func<IEnumerable<Point>> ExtraPoints        { get; set; }
    public DirectoryInfo            OutputDirectory    { get; set; }

    public ResultDrawerConfigFlags Flags { get; set; }
}

