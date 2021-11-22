using System;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public class ArcPathSegmentMakerTests
    {
        [Fact]
        public void T01_Should_create_line()
        {
            var ref1 = new PathRay(217.25631948011707, 79.465895927052742, 0.52655371522506067, 1.9294406404403024);
            var a = new ArcPathSegmentMaker
            {
                Flags = SegmentFlags.HasEndVector |
                        SegmentFlags.HasEndVector |
                        SegmentFlags.HasReferencePoints |
                        SegmentFlags.BothVectors,
                Validator = new MinimumValuesPathValidator(5, 0.001),
                Point = new ArcPathMakerVertex(224.32697127503582, 105.37474916092907)
                    .WithInVector(0.26327685761253034, 0.9647203202201512)
                    .WithReferencePoints(ref1),
                PreviousPoint = new ArcPathMakerVertex(209.76091585076492, 52.000632724428755)
                    .WithInVector(0.9647203202201492, -0.26327685761253855)
                    .WithOutVector(0.26327685761253034, 0.9647203202201512)
            };
            var r    = a.MakeItem();
            var code = new DpAssertsBuilder().Create(r, nameof(r));
            
            #region Asserts
            AssertEx.Equal(209.760915850765, 52.0006327244288, r.Start);
            AssertEx.Equal(224.326971275036, 105.374749160929, r.End);
            Assert.Single(r.Elements);
            var tmp1 = (LinePathElement)r.Elements[0];
            AssertEx.Equal(209.760915850765, 52.0006327244288, 224.326971275036, 105.374749160929, tmp1, 6);
            #endregion

        }
    }
}
