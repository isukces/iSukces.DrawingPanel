using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using iSukces.Mathematics;
using Point = System.Windows.Point;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal static class TestExtensions
    {
        public static double Angle(double x, double y)
        {
            if (x.Equals(0d))
                switch (Math.Sign(y))
                {
                    case 1:
                        return 90d;
                    case -1:
                        return 270d;
                    default:
                        return 0;
                }

            if (y.Equals(0d))
                return x < 0 ? 180d : 0d;

            var angle = MathEx.Atan2Deg(y, x);
            if (angle < 0)
                angle += 360;
            return angle;
        }

        public static double AngleMinusY(this Vector v) { return Angle(v.X, -v.Y); }


        public static bool ListSequenceEqual<TSource>(
            this IList<TSource> first,
            IList<TSource> second)
        {
            if (first is null) return false;
            if (second is null) return false;
            if (first.Count != second.Count) return false;
            for (var i = 0; i < first.Count; i++)
                if (!first[i].Equals(second[i]))
                    return false;
            return true;
        }

        public static PointF ToPointF(this Point point) { return new PointF((float)point.X, (float)point.Y); }

        public static void SaveIfDifferent(this Bitmap bmp, string fileName)
        {
            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            SaveIfDifferent(ms.ToArray(), fileName);
        }

        public static void SaveIfDifferent(string fileName, string content)
        {
            SaveIfDifferent(Encoding.UTF8.GetBytes(content), fileName);
        }

        private static void SaveIfDifferent(this byte[] current, string fileName)
        {
            if (File.Exists(fileName))
            {
                var existing = File.ReadAllBytes(fileName);
                if (current.ListSequenceEqual(existing))
                    return;
            }

            File.WriteAllBytes(fileName, current);
        }


        public static string ToCs(this double x) { return x.ToString(CultureInfo.InvariantCulture); }
        public static string ToCs(this int x) { return x.ToString(CultureInfo.InvariantCulture); }
        public static string ToCs(this Point x) { return $"new Point({x.X.ToCs()}, {x.Y.ToCs()})"; }

        public static string ToCs(this Vector x) { return $"new Vector({x.X.ToCs()}, {x.Y.ToCs()})"; }

        public static string ToFileName(this string s)
        {
            s = s.Replace("°", " ").Replace(":", " ").Replace(" ", "_");
            Rep("__", "_");
            Rep("_+", "+");
            Rep("_,", ",");
            Rep(",_", ",");
            Rep("+_", "+");
            s = s.TrimEnd('_').TrimStart('_');

            s = DigitAfterLetterRegex.Replace(s, m =>
            {
                return m.Groups[1].Value + m.Groups[2].Value;
            });

            return s;

            void Rep(string a, string b)
            {
                while (s.Contains(a))
                {
                    s = s.Replace(a, b);
                }
            }
        }


        private const string DigitAfterLetterFilter = @"([a-z])_(\d)";

        private static readonly Regex DigitAfterLetterRegex =
            new Regex(DigitAfterLetterFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
