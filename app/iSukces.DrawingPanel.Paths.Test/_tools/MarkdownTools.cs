using System;
using System.IO;
using System.Linq;
using System.Text;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal class MarkdownTools
    {
        public static void MakeMarkdownIndex(DirectoryInfo dir, Comparison<FileInfo> comparision)
        {
            var files = dir.GetFiles("*.png").ToList();
            files.Sort(comparision);
            var sb = new StringBuilder();

            sb.AppendLine("# Test output");
            foreach (var file in files)
            {
                var uri = "https://github.com/isukces/iSukces.DrawingPanel/blob/main/doc/testDrawings/"
                          + file.Name + "?raw=true";
                var code = "![](" + uri + ")";
                sb.AppendLine(code);
            }

            var fileName = Path.Combine(dir.FullName, "ReadMe.md");
            TestExtensions.SaveIfDifferent(fileName, sb.ToString());
        }
    }
}
