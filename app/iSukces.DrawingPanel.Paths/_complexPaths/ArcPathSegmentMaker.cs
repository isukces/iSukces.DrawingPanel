using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    internal sealed  class ArcPathSegmentMaker
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IPathResult Handle0()
        {
            if (_inArmLengthPlus || _outArmLengthPlus)
            {
                var builder = new PathBuilder(_start.Point, Validator);
                NormalizeVectorsAndMovePoints();

                var r = ZeroReferencePointPathCalculator.Compute(_start, _end, Validator);
                if (r is null)
                    throw new NotImplementedException();
                ArcPathMaker.Add(builder, r);
                return builder.LineToAndCreate(Point.Location);
            }
            else
            {
                var r = ZeroReferencePointPathCalculator.Compute(_start, _end, Validator);
                if (r is null)
                    throw new NotImplementedException();
                return r;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IPathResult Handle1()
        {
            var refs = Point.ReferencePoints;
            if (_inArmLengthPlus || _outArmLengthPlus)
            {
                var builder = new PathBuilder(_start.Point, Validator);
                NormalizeVectorsAndMovePoints();

                var r = new OneReferencePointPathCalculator
                {
                    Start     = _start,
                    End       = _end.WithInvertedVector(),
                    Reference = refs[0]
                }.Compute(Validator);
                if (r is null)
                    throw new NotImplementedException();
                ArcPathMaker.Add(builder, r);
                return builder.LineToAndCreate(Point.Location);
            }
            else
            {
                var r = new OneReferencePointPathCalculator
                {
                    Start     = _start,
                    End       = _end.WithInvertedVector(),
                    Reference = refs[0]
                }.Compute(Validator);
                if (r is null)
                    throw new NotImplementedException();
                return r;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IPathResult Handle2()
        {
            var refs = Point.ReferencePoints;
            if (_inArmLengthPlus || _outArmLengthPlus)
            {
                var builder = new PathBuilder(_start.Point, Validator);
                NormalizeVectorsAndMovePoints();

                var r = new TwoReferencePointsPathCalculator
                {
                    Start      = _start,
                    End        = _end.WithInvertedVector(),
                    Reference1 = refs[0],
                    Reference2 = refs[1]
                }.Compute(Validator);
                if (r is null)
                    throw new NotImplementedException();
                ArcPathMaker.Add(builder, r);
                return builder.LineToAndCreate(Point.Location);
            }
            else
            {
                var r = new TwoReferencePointsPathCalculator
                {
                    Start      = _start,
                    End        = _end.WithInvertedVector(),
                    Reference1 = refs[0],
                    Reference2 = refs[1]
                }.Compute(Validator);
                if (r is null)
                    throw new NotImplementedException();
                return r;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IPathResult Handle3AndMore()
        {
            var refs    = Point.ReferencePoints;
            var builder = new PdPathBuilder(_start.Point, Validator);

           

            if (_inArmLengthPlus || _outArmLengthPlus)
            {
                NormalizeVectorsAndMovePoints();
            }

            var last = refs[0];
            builder.AddFlexi(_start, last);

            for (var index = 1; index < refs.Count; index++)
            {
                var t = refs[index];
                builder.AddFlexi(last, t);
                last = t;
            }

            builder.AddFlexi(last, _end);
            return builder.LineToAndCreate(Point.Location);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IPathResult MakeItem()
        {
            var refs = Point.ReferencePoints;

            _startVector = PreviousPoint.OutVector;
            _endVector   = Point.InVector;

            _outArmLength     = PreviousPoint.OutArmLength;
            _inArmLength      = Point.InArmLength;
            _outArmLengthPlus = _outArmLength > 0;
            _inArmLengthPlus  = _inArmLength > 0;
            _anyArm           = _outArmLengthPlus || _inArmLengthPlus;

            _normalizationFlags = NormalizationFlags.NormalizeEndVector | NormalizationFlags.NormalizeStartVector;

            if ((Flags & SegmentFlags.BothVectors) != SegmentFlags.BothVectors)
            {
                var dir = Point.Location - PreviousPoint.Location;
                if (_anyArm)
                    dir.Normalize();
                if ((Flags & SegmentFlags.HasStartVector) == 0)
                {
                    _normalizationFlags &= ~NormalizationFlags.NormalizeStartVector;
                    _startVector       =  dir;
                }

                if ((Flags & SegmentFlags.HasEndVector) == 0)
                {
                    _normalizationFlags &= ~NormalizationFlags.NormalizeEndVector;
                    _endVector         =  dir;
                }
            }

            _start = new PathRay(PreviousPoint.Location, _startVector);
            _end   = new PathRay(Point.Location, _endVector);

            var cnt = refs?.Count ?? 0;

            return cnt switch
            {
                0 => Handle0(),
                1 => Handle1(),
                2 => Handle2(),
                _ => Handle3AndMore()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void NormalizeVectorsAndMovePoints()
        {
            if (_outArmLengthPlus)
            {
                const NormalizationFlags mask = NormalizationFlags.NormalizeStartVector;
                if ((_normalizationFlags & mask) != 0)
                {
                    _normalizationFlags &= ~mask;
                    _startVector.Normalize();
                }

                _start = _start.With(_start.Point + _startVector * _outArmLength);
            }

            if (_inArmLengthPlus)
            {
                const NormalizationFlags mask = NormalizationFlags.NormalizeEndVector;
                if ((_normalizationFlags & mask) != 0)
                {
                    _normalizationFlags &= ~mask;
                    _endVector.Normalize();
                }

                _end = _end.With(_end.Point - _endVector * _inArmLength);
            }
        }


        private bool _anyArm;
        private PathRay _end;
        private Vector _endVector;
        private double _inArmLength;
        private bool _inArmLengthPlus;
        private double _outArmLength;
        private bool _outArmLengthPlus;
        private PathRay _start;
        private Vector _startVector;
        private NormalizationFlags _normalizationFlags;


        public SegmentFlags Flags;
        public ArcPathMakerVertex Point;
        public ArcPathMakerVertex PreviousPoint;
        public IPathValidator Validator;
    }

    internal sealed class PdPathBuilder : PathBuilder
    {
        public PdPathBuilder(Point currentPoint, IPathValidator validator = null)
            : base(currentPoint, validator)
        {
        }
        
        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(PathRay st, PathRay en)
        {
            var s = ZeroReferencePointPathCalculator.Compute(st, en, Validator);
            if (s is null)
                return;
            ArcTo(s.Arc1);
            ArcTo(s.Arc2);
        }*/
    }
}
