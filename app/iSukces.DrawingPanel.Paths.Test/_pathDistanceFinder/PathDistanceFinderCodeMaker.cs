#nullable disable
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSukces.DrawingPanel.Paths.Test;

public class PathDistanceFinderCodeMaker
{
    public static string Make(IEnumerable<object[]> data, PathResult pathResult)
    {
        var items = data
            .Select(item => (PathDistanceFinderTestData)item[1])
            .OrderBy(a => a.TestPoint.X)
            .ThenBy(a => a.TestPoint.Y)
            .ToArray();
        return Make(items, pathResult);
    }

    public static string Make(IEnumerable<PathDistanceFinderTestData> items, PathResult pathResult)
    {
        var sb = new StringBuilder();
        sb.AppendLine("#region DATA");
        foreach (var item in items)
        {
            var result = PathDistanceFinder.GetDistanceFromLine(pathResult, item.TestPoint);
            var clone  = item.Clone();
            clone.CopyFrom(result);
            var code = clone.GetCsCode();
            sb.AppendLine("yield return " + code + ";");
        }

        sb.AppendLine("#endregion");
        return sb.ToString();
    }

    public static string MakeRandom(PathResult pathResult)
    {
        PathDistanceFinderTestData[] a = PathDistanceFinderTestData.GenerateRandom(pathResult, 6).ToArray();
        return Make(a, pathResult);
    }
}
