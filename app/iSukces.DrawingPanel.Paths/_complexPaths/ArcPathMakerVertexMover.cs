#if COREFX
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Paths
{
    /// <summary>
    ///     Allows moving path parallelly left or right.
    /// </summary>
    public sealed class ArcPathMakerVertexMover
    {
        private ArcPathMakerVertexMover(ArcPathMakerVertex[] list,
            GetMovementBySegmentIndexDelegate movementBySegmentIndex)
        {
            _count                  = list.Length;
            _list                   = list;
            _exists                 = new bool[_count];
            _vectors                = new Vector[_count];
            _movementBySegmentIndex = movementBySegmentIndex;
        }

        public static IReadOnlyList<ArcPathMakerVertex> Move(
            IReadOnlyList<ArcPathMakerVertex> list,
            GetMovementBySegmentIndexDelegate movement,
            bool vectorsAreValid = false,
            bool canModifyExistingListIfPossible = false)
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


        public static IReadOnlyList<ArcPathMakerVertex> Move(
            IReadOnlyList<ArcPathMakerVertex> list,
            double movement,
            bool vectorsAreValid = false,
            bool canModifyExistingListIfPossible = false)
        {
            if (list is null)
                return null;
            return Move(list, pointIdx =>
                {
                    return movement;
                },
                vectorsAreValid, canModifyExistingListIfPossible);
        }


        private IReadOnlyList<ArcPathMakerVertex> MoveInternal()
        {
            var previous    = new Point[_count];
            var moveVectors = new Vector[_count];

            for (var index = 0; index < _list.Length; index++)
            {
                previous[index] = _list[index].Location;
            }

            var movement0 = _movementBySegmentIndex(0);
            {
                var a             = _list[0];
                var prependicular = a.OutVector.GetPrependicular(false);
                var vector        = prependicular * movement0;
                moveVectors[0] =  vector;
                a.Location     += vector;
            }
            var lastIndex = _list.Length - 1;
            {
                var a             = _list[lastIndex];
                var prependicular = a.InVector.GetPrependicular(false);
                var movementLast  = _movementBySegmentIndex(lastIndex - 1);
                var vector        = prependicular * movementLast;
                moveVectors[lastIndex] =  vector;
                a.Location             += vector;
            }

            var movement = movement0;
            for (var index = 1; index < lastIndex; index++)
            {
                // var movement = _movementBySegmentIndex(index-1);
                var vertex = _list[index];
                var v      = vertex.InVector;
                var loc1   = vertex.Location + v.GetPrependicular(false) * movement;

                var line1 = PathLineEquationNotNormalized.FromPointAndDeltas(loc1, v);

                movement = _movementBySegmentIndex(index);
                v        = vertex.OutVector;
                var loc2  = vertex.Location + v.GetPrependicular(false) * movement;
                var line2 = PathLineEquationNotNormalized.FromPointAndDeltas(loc2, v);

                var   cross            = PathLineEquationNotNormalized.Cross(line1, line2);
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
                var rr = refs as WayPoint[] ?? refs.ToArray();
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

                    rr[i] = src.OutputRay.WithPoint(newLocation);
                }

                vertex.ReferencePoints = rr;
            }

            return _list;
        }

        #region Fields

        private readonly int _count;

        private readonly bool[] _exists;
        private readonly ArcPathMakerVertex[] _list;
        private readonly GetMovementBySegmentIndexDelegate _movementBySegmentIndex;
        private readonly Vector[] _vectors;

        #endregion
    }

    [Flags]
    public enum ArcPathMakerVertexMoverFlags
    {
        None = 0,
        UpdateExistingIfPossible = 1
    }

    public delegate double GetMovementBySegmentIndexDelegate(int segmentIndex);
}
