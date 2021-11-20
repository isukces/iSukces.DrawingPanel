using System;
using System.Collections.Generic;
using System.Linq;
#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    public class ArcPathMakerResult
    {
        public ArcPathMakerResult(IReadOnlyList<IPathResult> segments) { Segments = segments; }

        public void GetBendVectors(int index, out Vector inVector, out Vector outVector)
        {
            var outSegment = Segments[index];
            var inSegment  = Segments[index - 1];

            var inSegmentLast   = inSegment.Elements.Last();
            var outSegmentFirst = outSegment.Elements.First();
            inVector = inSegmentLast.GetEndVector();
            outVector = outSegmentFirst.GetStartVector();
        }

        public IReadOnlyList<IPathResult> Segments { get; }

        public static ArcPathMakerResult Empty => new(Array.Empty<IPathResult>());

        public int Count => Segments.Count;
    }
}
