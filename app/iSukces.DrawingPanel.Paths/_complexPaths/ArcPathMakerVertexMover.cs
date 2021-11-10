using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Paths
{
    /// <summary>
    ///     Allows moving path parallelly left or right.
    /// </summary>
    public sealed class ArcPathMakerVertexMover
    {
        private ArcPathMakerVertexMover(ArcPathMakerVertex[] list, double movement)
        {
            _count    = list.Length;
            _list     = list;
            _exists   = new bool[_count];
            _vectors  = new Vector[_count];
            _movement = movement;
        }


        public static IReadOnlyList<ArcPathMakerVertex> Move(
            IReadOnlyList<ArcPathMakerVertex> list,
            double movement, bool vectorsAreValid = false, bool canModifyExistingListIfPossible = false)
        {
            if (list is null)
                return null;
            var count = list.Count;
            if (movement.Equals(0d) || count < 2)
                return list;
            if (!vectorsAreValid)
            {
                list                            = VerticesValidator.FillMissingVectors(list);
                canModifyExistingListIfPossible = true;
            }

            ArcPathMakerVertex[] vertices;
            if (canModifyExistingListIfPossible)
            {
                vertices = list as ArcPathMakerVertex[] ?? list.ToArray();
            }
            else
            {
                vertices = new ArcPathMakerVertex[count];
                for (var i = 0; i < vertices.Length; i++)
                    vertices[i] = list[i].DeepClone();
            }

            var mover = new ArcPathMakerVertexMover(vertices, movement);
            return mover.MoveInternal();
        }


        private IReadOnlyList<ArcPathMakerVertex> MoveInternal()
        {
            var previous    = new Point[_count];
            var moveVectors = new Vector[_count];

            for (var index = 0; index < _list.Length; index++)
            {
                previous[index] = _list[index].Location;
            }

            {
                var a             = _list[0];
                var prependicular = a.OutVector.GetPrependicular(false);
                var vector        = prependicular * _movement;
                moveVectors[0] =  vector;
                a.Location     += vector;
            }
            var lastIndex = _list.Length - 1;
            {
                var a             = _list[lastIndex];
                var prependicular = a.InVector.GetPrependicular(false);
                var vector        = prependicular * _movement;
                moveVectors[lastIndex] =  vector;
                a.Location             += vector;
            }

            for (var index = 1; index < lastIndex; index++)
            {
                var vertex = _list[index];
                var v      = vertex.InVector;
                var loc1   = vertex.Location + v.GetPrependicular(false) * _movement;

                var line1 = LineEquationNotNormalized.FromPointAndDeltas(loc1, v);

                v = vertex.OutVector;
                var loc2  = vertex.Location + v.GetPrependicular(false) * _movement;
                var line2 = LineEquationNotNormalized.FromPointAndDeltas(loc2, v);

                var   cross            = LineEquationNotNormalized.Cross(line1, line2);
                var   originalLocation = vertex.Location;
                Point movedLocation;
                if (cross.HasValue)
                {
                    movedLocation = cross.Value;
                }
                else
                {
                    movedLocation = new Point((loc1.X + loc2.X) * 0.5, (loc1.Y + loc2.Y) * 0.5);
                }

                vertex.Location    = movedLocation;
                moveVectors[index] = movedLocation - originalLocation;
            }

            for (var index = 1; index <= lastIndex; index++)
            {
                var vertex = _list[index];
                var refs   = vertex.ReferencePoints;
                if (refs == null)
                    continue;
                var rr = refs as PathRay[] ?? refs.ToArray();
                if (rr.Length == 0)
                    continue;

                var prevSegmentVector = previous[index] - previous[index - 1];
                var prevSegmentLength = prevSegmentVector.Length;
                prevSegmentVector /= prevSegmentLength;

                var newSegmentBegin  = _list[index - 1].Location;
                var newSegmentVector = vertex.Location - newSegmentBegin;
                var newSegmentLength = newSegmentVector.Length;
                newSegmentVector /= newSegmentLength;

                var newSegmentVectorPrependicular = newSegmentVector.GetPrependicular();
                var prevSegmentVector2            = prevSegmentVector * (newSegmentLength / prevSegmentLength);
                for (var i = 0; i < rr.Length; i++)
                {
                    var src                     = rr[i];
                    var v                       = src.Point - previous[index - 1];
                    var vPrependiculatToSegment = Vector.CrossProduct(prevSegmentVector, v);
                    var vAlongSegment           = v * prevSegmentVector2;

                    var vx          = vPrependiculatToSegment * newSegmentVectorPrependicular;
                    var vy          = vAlongSegment * newSegmentVector;
                    var vvv         = vx + vy;
                    var newLocation = newSegmentBegin + vvv;

                    rr[i] = src.WithPoint(newLocation);
                }

                vertex.ReferencePoints = rr;
            }

            return _list;
        }

        private readonly int _count;

        private readonly bool[] _exists;
        private readonly ArcPathMakerVertex[] _list;
        private readonly double _movement;
        private readonly Vector[] _vectors;
    }

    [Flags]
    public enum ArcPathMakerVertexMoverFlags
    {
        None = 0,
        UpdateExistingIfPossible = 1
    }
}