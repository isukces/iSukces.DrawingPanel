using System;
using System.Collections.Generic;
using Xunit;

namespace iSukces.DrawingPanel.Paths.Test
{
    public class ArcPathMakerVertexMoverTests:TestBaseClass
    {
        
        [Fact]
        public void T11_Should_move_null_path()
        {
            var r    = ArcPathMakerVertexMover.Move(null, 1);
            Assert.Null(r);
        }

        [Fact]
        public void T12_Should_move_empty_path()
        {
            var list = Array.Empty<ArcPathMakerVertex>();
            var r    = ArcPathMakerVertexMover.Move(list, 1);
            Assert.True(ReferenceEquals(list, r));
        }

        [Fact]
        public void T13_Should_move_one_point_path()
        {
            var list = new[] { new ArcPathMakerVertex(1, 2) };
            var r    = ArcPathMakerVertexMover.Move(list, 1);
            Assert.True(ReferenceEquals(list, r));
        }


        [Fact]
        public void T13_Should_move_two_point_path()
        {
            var list = new[] { new ArcPathMakerVertex(1, 2), new ArcPathMakerVertex(4, 2) };
            var r    = ArcPathMakerVertexMover.Move(list, 1);
            var code = new DpAssertsBuilder().Create(r, nameof(r));
            #region Asserts
            Assert.Equal(2, r.Count);
            var tmp1 = r[0];
            AssertEx.Equal(1, 1, tmp1.Location);
            AssertEx.Equal(0, 0, tmp1.InVector);
            AssertEx.Equal(1, 0, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasOutVector, tmp1.Flags);
            tmp1 = r[1];
            AssertEx.Equal(4, 1, tmp1.Location);
            AssertEx.Equal(1, 0, tmp1.InVector);
            AssertEx.Equal(0, 0, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasInVector, tmp1.Flags);
            #endregion
        }
        [Fact]
        public void T14_Should_move_three_point_path()
        {
            var list = new[]
            {
                new ArcPathMakerVertex(1, 2),
                new ArcPathMakerVertex(4, 2),
                new ArcPathMakerVertex(4, 22)
            };
            var r    = ArcPathMakerVertexMover.Move(list, 1);
            var code = new DpAssertsBuilder().Create(r, nameof(r));
            #region Asserts
            Assert.Equal(3, r.Count);
            var tmp1 = r[0];
            AssertEx.Equal(1, 1, tmp1.Location);
            AssertEx.Equal(0, 0, tmp1.InVector);
            AssertEx.Equal(1, 0, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasOutVector, tmp1.Flags);
            tmp1 = r[1];
            AssertEx.Equal(5, 1, tmp1.Location);
            AssertEx.Equal(1, 0, tmp1.InVector);
            AssertEx.Equal(0, 1, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasBothVectors, tmp1.Flags);
            tmp1 = r[2];
            AssertEx.Equal(5, 22, tmp1.Location);
            AssertEx.Equal(0, 1, tmp1.InVector);
            AssertEx.Equal(0, 0, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasInVector, tmp1.Flags);
            #endregion

        }
        
        [Fact]
        public void T15_Should_move_two_point_path_with_reference()
        {
            var list = new[]
            {
                new ArcPathMakerVertex(5, 2),
                new ArcPathMakerVertex(5, 22).WithReferencePoints(new PathRay(6, 12))
            };
            var r    = ArcPathMakerVertexMover.Move(list, 1);
            var code = new DpAssertsBuilder().Create(r, nameof(r));
            
            #region Asserts
            Assert.Equal(2, r.Count);
            var tmp1 = r[0];
            AssertEx.Equal(6, 2, tmp1.Location);
            AssertEx.Equal(0, 0, tmp1.InVector);
            AssertEx.Equal(0, 1, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasOutVector, tmp1.Flags);
            Assert.Null(tmp1.ReferencePoints);
            tmp1 = r[1];
            AssertEx.Equal(6, 22, tmp1.Location);
            AssertEx.Equal(0, 1, tmp1.InVector);
            AssertEx.Equal(0, 0, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasInVector, tmp1.Flags);
            Assert.Single(tmp1.ReferencePoints);
            var waypoint = tmp1.ReferencePoints[0];
            AssertEx.Equal(7, 12, 0, 0, 0, waypoint.OutputRay);
            Assert.False(waypoint.UseInputVector);
            AssertEx.Equal(0, 0, waypoint.InputVector);
            #endregion



        }
        
        [Fact]
        public void T16_Should_move_three_point_path_with_reference()
        {
            var list = new[]
            {
                new ArcPathMakerVertex(1, 2),
                new ArcPathMakerVertex(5, 2),
                new ArcPathMakerVertex(5, 22).WithReferencePoints(new PathRay(6, 12))
            };
            var r    = ArcPathMakerVertexMover.Move(list, 1);
            var code = new DpAssertsBuilder().Create(r, nameof(r));
            
            #region Asserts
            Assert.Equal(3, r.Count);
            var tmp1 = r[0];
            AssertEx.Equal(1, 1, tmp1.Location);
            AssertEx.Equal(0, 0, tmp1.InVector);
            AssertEx.Equal(1, 0, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasOutVector, tmp1.Flags);
            Assert.Null(tmp1.ReferencePoints);
            tmp1 = r[1];
            AssertEx.Equal(6, 1, tmp1.Location);
            AssertEx.Equal(1, 0, tmp1.InVector);
            AssertEx.Equal(0, 1, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasBothVectors, tmp1.Flags);
            Assert.Null(tmp1.ReferencePoints);
            tmp1 = r[2];
            AssertEx.Equal(6, 22, tmp1.Location);
            AssertEx.Equal(0, 1, tmp1.InVector);
            AssertEx.Equal(0, 0, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasInVector, tmp1.Flags);
            Assert.Single(tmp1.ReferencePoints);
            var waypoint = tmp1.ReferencePoints[0];
            AssertEx.Equal(7, 11.5, 0, 0, 0, waypoint.OutputRay);
            Assert.False(waypoint.UseInputVector);
            AssertEx.Equal(0, 0, waypoint.InputVector);
            #endregion

        }
        
        [Fact]
        public void T17_Should_move_three_point()
        {
            var list = new[]
            {
                new ArcPathMakerVertex(1, 2),
                new ArcPathMakerVertex(5, 2),
                new ArcPathMakerVertex(5, 22)
            };
            var r    = ArcPathMakerVertexMover.Move(list, segmentIndex =>
            {
                return segmentIndex * 2 + 2;
            });
            var code = new DpAssertsBuilder().Create(r, nameof(r));
            #region Asserts
            Assert.Equal(3, r.Count);
            var tmp1 = r[0];
            AssertEx.Equal(1, 0, tmp1.Location);
            AssertEx.Equal(0, 0, tmp1.InVector);
            AssertEx.Equal(1, 0, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasOutVector, tmp1.Flags);
            Assert.Null(tmp1.ReferencePoints);
            tmp1 = r[1];
            AssertEx.Equal(9, 0, tmp1.Location);
            AssertEx.Equal(1, 0, tmp1.InVector);
            AssertEx.Equal(0, 1, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasBothVectors, tmp1.Flags);
            Assert.Null(tmp1.ReferencePoints);
            tmp1 = r[2];
            AssertEx.Equal(9, 22, tmp1.Location);
            AssertEx.Equal(0, 1, tmp1.InVector);
            AssertEx.Equal(0, 0, tmp1.OutVector);
            Assert.Equal(FlexiPathMakerItem2Flags.HasInVector, tmp1.Flags);
            Assert.Null(tmp1.ReferencePoints);
            #endregion

            
        }
    }
}
