using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using iSukces.Mathematics;
using Newtonsoft.Json;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public sealed class PathDistanceFinderTestData
    {
        public PathDistanceFinderTestData(double x, double y, double distanceFromLine = double.NaN,
            Three locationRelatedToElement = Three.Inside, int index = 0)
        {
            TestPoint                = new Point(x, y);
            DistanceFromLine         = distanceFromLine;
            LocationRelatedToElement = locationRelatedToElement;
            Index                    = index;
        }

        public static IEnumerable<PathDistanceFinderTestData> GenerateRandom(PathResult pathResult, double maxDistance)
        {
            var elements    = pathResult.Elements;
            var track1      = TrackFromPathResult.Make(pathResult);
            var totalLength = track1.GetLength();

            TrackInfo Find(double track)
            {
                if (track < 0)
                {
                    var pathElement = elements.First();
                    var point       = pathElement.GetStartPoint();
                    var vector      = pathElement.GetStartVector().NormalizeFast();
                    point += vector * track;
                    return new TrackInfo(point, vector);
                }

                if (track > totalLength)
                {
                    track -= totalLength;
                    var pathElement = elements.Last();
                    var point       = pathElement.GetEndPoint();
                    var vector      = pathElement.GetEndVector().NormalizeFast();
                    point += vector * track;
                    return new TrackInfo(point, vector);
                }

                return track1.GetTrackInfo(track);
            }

            var rnd = new Random(1234);

            double Rand(double min, double max)
            {
                return rnd.NextDouble() * (max - min) + min;
            }

            var sink = new List<PathDistanceFinderTestData>();

            void Part(double centerX, double maxDistance1)
            {
                var center = Find(centerX);
                var angles = new List<double>();
                for (var i = 0; i < 360; i++)
                    angles.Add(i * MathEx.DEGTORAD);

                angles.Sort();

                var g1 = Enumerable.Range(0, 360)
                    .Select(i =>
                    {
                        var angle = i * MathEx.DEGTORAD;
                        var s     = Math.Sin(angle);
                        var c     = Math.Cos(angle);
                        var p     = center.Location + new Vector(c, s) * maxDistance1 * Rand(0.6, 1);
                        return TestingItem.Make(angle, p, pathResult);
                    }).ToArray();

                var g = g1.OrderBy(x => x.Angle).GroupBy(a => a.ConfigString).ToArray();

                foreach (var i in g)
                {
                    var bla    = i.ToArray();
                    var middle = bla.Length / 2;
                    sink.Add(bla[middle].Data);
                }
            }

            var offset = 0d;
            for (var i = 0; i < elements.Count; i++)
            {
                var isFirstOrLast = i == 0;
                var maxDistance1  = isFirstOrLast ? maxDistance * 0.5 : maxDistance;
                Part(offset, maxDistance1);
                var length = elements[i].GetLength();
                Part(offset + length * 0.33, maxDistance1);
                offset += length;
            }

            Part(offset, maxDistance * 0.5);
            return sink;
        }

        public void AssertEqual(PathDistanceFinderResult actual)
        {
            Assert.True(DistanceFromLine >= 0);
            Assert.Equal(DistanceFromLine, actual.DistanceFromLine, 8);
            Assert.Equal(LocationRelatedToElement, actual.Location);
            Assert.Equal(SideMovement, actual.SideMovement);
            Assert.Equal(Direction.X, actual.Direction.X);
            Assert.Equal(Direction.Y, actual.Direction.Y);

            Assert.Equal(ClosestPoint.X, actual.ClosestPoint.X);
            Assert.Equal(ClosestPoint.Y, actual.ClosestPoint.Y);
            Assert.Equal(Track, actual.Track);
            Assert.Equal(ElementTrackOffset, actual.ElementTrackOffset);
            Assert.Equal(Index, actual.ElementIndex);
        }

        public PathDistanceFinderTestData Clone()
        {
            return (PathDistanceFinderTestData)MemberwiseClone();
        }

        public void CopyFrom(PathDistanceFinderResult result)
        {
            ClosestPoint             = result.ClosestPoint;
            Direction                = result.Direction;
            Index                    = result.ElementIndex;
            Track                    = result.Track;
            SideMovement             = result.SideMovement;
            ElementTrackOffset       = result.ElementTrackOffset;
            DistanceFromLine         = result.DistanceFromLine;
            ElementTrackOffset       = result.ElementTrackOffset;
            LocationRelatedToElement = result.Location;
        }

        public string GetCsCode()
        {
            var constructorArgs = new[]
            {
                TestPoint.X.ToCs(), TestPoint.Y.ToCs(), DistanceFromLine.ToCs(),
                nameof(Three) + "." + LocationRelatedToElement,
                Index.ToCs()
            };

            var sb = new StringBuilder($"new {nameof(PathDistanceFinderTestData)}(");
            sb.Append(string.Join(", ", constructorArgs));
            sb.AppendLine("){");
            sb.AppendLine($"{nameof(ClosestPoint)} = new Point({ClosestPoint.X.ToCs()}, {ClosestPoint.Y.ToCs()}),");
            sb.AppendLine($"{nameof(Direction)} = new Vector({Direction.X.ToCs()}, {Direction.Y.ToCs()}),");
            sb.AppendLine($"{nameof(SideMovement)} = {SideMovement.ToCs()},");
            sb.AppendLine($"{nameof(Track)} = {Track.ToCs()},");
            sb.AppendLine($"{nameof(ElementTrackOffset)} = {ElementTrackOffset.ToCs()}");
            sb.Append("}");
            return sb.ToString();
        }

        #region properties

        public Point  TestPoint                { get; }
        public Point  ClosestPoint             { get; set; }
        public Vector Direction                { get; set; }
        public Three  LocationRelatedToElement { get; set; } = Three.Inside;
        public double DistanceFromLine         { get; set; }
        public int    Index                    { get; set; }
        public double SideMovement             { get; set; } = double.NaN;
        public double Track                    { get; set; } = double.NaN;
        public double ElementTrackOffset       { get; set; } = double.NaN;

        #endregion

        public class TestingItem
        {
            public TestingItem(double angle, string configString, PathDistanceFinderTestData data)
            {
                Angle        = angle;
                ConfigString = configString;
                Data         = data;
            }

            public static TestingItem Make(double angle, Point p, IPathResult pathResult)
            {
                var ti  = new PathDistanceFinderTestData(p.X, p.Y);
                var tmp = PathDistanceFinder.GetDistanceFromLine(pathResult, p);
                ti.CopyFrom(tmp);

                var obj = new object[]
                {
                    ti.LocationRelatedToElement,
                    ti.Index,
                    Math.Sign(ti.SideMovement)
                };
                var json = JsonConvert.SerializeObject(obj);

                return new TestingItem(angle, json, ti);
            }

            public override string ToString()
            {
                return $"Angle={Angle}, ConfigString={ConfigString}, Data={Data}";
            }

            #region properties

            public double Angle { get; }

            public string ConfigString { get; }

            public PathDistanceFinderTestData Data { get; }

            #endregion
        }
    }
}
