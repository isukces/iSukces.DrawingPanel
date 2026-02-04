using System.Runtime.CompilerServices;

namespace iSukces.DrawingPanel.Paths;

internal sealed class ArcPathSegmentMaker
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IPathResult Handle0()
    {
        IPathResult ComputeCommon()
        {
            var pathResult = ZeroReferencePointPathCalculator.Compute(_start, _end, Validator);
            if (pathResult is null)
            {
                return new ZeroReferencePointPathCalculatorLineResult(_start.Point, _end.Point);
            }

            return pathResult;
        }

        if (_inArmLengthPlus || _outArmLengthPlus)
        {
            var builder = new PathBuilder(_start.Point, Validator);
            NormalizeVectorsAndMovePoints();
            var r = ComputeCommon();
            ArcPathMaker.Add(builder, r);
            return builder.LineToAndCreate(Point.Location);
        }
        else
        {
            var r = ComputeCommon();
            return r;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IPathResult? Handle1()
    {
        var refs     = Point.ReferencePoints!;
        var wayPoint = refs[0];
        if (wayPoint.UseInputVector)
            return Handle3AndMore();
        var reference = wayPoint.OutputRay;
        if (reference.ArmLength > 0)
            return Handle3AndMore();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IPathResult ComputeCommon()
        {
            var calculator = new OneReferencePointPathCalculator
            {
                Start     = _start,
                End       = _end.WithInvertedVector(),
                Reference = reference.GetRay()
            };
            var pathResult = calculator.Compute(Validator);
            if (pathResult is null)
            {
                var ex = new NotImplementedException(nameof(OneReferencePointPathCalculator) + " gives not result");
                calculator.AppendData(ex.Data);
                ex.Data.AddDebug();
                throw ex;
            }

            return pathResult;
        }

        if (_inArmLengthPlus || _outArmLengthPlus)
        {
            var builder = new PathBuilder(_start.Point, Validator);
            NormalizeVectorsAndMovePoints();
            var r = ComputeCommon();
            ArcPathMaker.Add(builder, r);
            return builder.LineToAndCreate(Point.Location);
        }

        {
            var r = ComputeCommon();
            return r;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IPathResult? Handle2()
    {
        var refs = Point.ReferencePoints!;
        var wp0  = refs[0];
        if (wp0.UseInputVector || wp0.OutputArmLength > 0)
            return Handle3AndMore();

        var wp1 = refs[1];
        if (wp1.UseInputVector || wp1.OutputArmLength > 0)
            return Handle3AndMore();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IPathResult ComputeCommon()
        {
            var ref1 = wp0.OutputRay;
            var ref2 = wp1.OutputRay;
            var calculator = new TwoReferencePointsPathCalculator
            {
                Start      = _start,
                End        = _end.WithInvertedVector(),
                Reference1 = new PathRay(ref1.Point, ref1.Vector),
                Reference2 = new PathRay(ref2.Point, ref2.Vector)
            };
            var pathResult = calculator.Compute(Validator);
            if (pathResult is null)
            {
                var ex = new NotImplementedException(nameof(TwoReferencePointsPathCalculator) + " gives not result");
                calculator.AppendData(ex.Data);
                ex.Data.AddDebug();
                throw ex;
            }

            return pathResult;
        }

        if (_inArmLengthPlus || _outArmLengthPlus)
        {
            var builder = new PathBuilder(_start.Point, Validator);
            NormalizeVectorsAndMovePoints();
            var r = ComputeCommon();
            ArcPathMaker.Add(builder, r);
            return builder.LineToAndCreate(Point.Location);
        }
        else
        {
            var r = ComputeCommon();
            return r;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IPathResult? Handle3AndMore()
    {
        var refs    = Point.ReferencePoints!;
        var builder = new PathBuilder(_start.Point, Validator);

        if (_inArmLengthPlus || _outArmLengthPlus)
            NormalizeVectorsAndMovePoints();

        var            idx = 0;
        PathRayWithArm last;
        {
            var wayPoint = refs[0];
            last = wayPoint.OutputRay;
            var start = new PathRayWithArm(PreviousPoint.Location, _startVector, _outArmLength);
            builder.AddFlexi(start, wayPoint.InputRay);
            builder.MarkEndWayPoint(ref idx, wayPoint);
        }

        for (var index = 1; index < refs.Count; index++)
        {
            var wayPoint = refs[index];
            builder.AddFlexi(last, wayPoint.InputRay);
            last = wayPoint.OutputRay;
            builder.MarkEndWayPoint(ref idx, wayPoint);
        }

        {
            var end = new PathRayWithArm(_end.Point, _end.Vector);
            builder.AddFlexi(last, end);
        }
        return builder.LineToAndCreate(Point.Location);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IPathResult? MakeItem()
    {
        var refs = Point.ReferencePoints;

        _startVector = PreviousPoint.OutVector;
        _endVector   = Point.InVector;

        _outArmLength     = PreviousPoint.OutArmLength;
        _inArmLength      = Point.InArmLength;
        _outArmLengthPlus = _outArmLength > 0;
        _inArmLengthPlus  = _inArmLength > 0;

        _normalizationFlags = NormalizationFlags.NormalizeEndVector | NormalizationFlags.NormalizeStartVector;

        if ((Flags & SegmentFlags.BothVectors) != SegmentFlags.BothVectors)
        {
            var dir = Point.Location - PreviousPoint.Location;
            dir = dir.GetNormalized();
            if ((Flags & SegmentFlags.HasStartVector) == 0)
            {
                _normalizationFlags &= ~NormalizationFlags.NormalizeStartVector;
                _startVector        =  dir;
            }

            if ((Flags & SegmentFlags.HasEndVector) == 0)
            {
                _normalizationFlags &= ~NormalizationFlags.NormalizeEndVector;
                _endVector          =  dir;
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
                _startVector = _startVector.GetNormalized();
            }

            _start = _start.With(_start.Point + _startVector * _outArmLength);
        }

        if (_inArmLengthPlus)
        {
            const NormalizationFlags mask = NormalizationFlags.NormalizeEndVector;
            if ((_normalizationFlags & mask) != 0)
            {
                _normalizationFlags &= ~mask;
                _endVector = _endVector.GetNormalized();
            }

            _end = _end.With(_end.Point - _endVector * _inArmLength);
        }
    }

    #region Fields

    private PathRay _end;
    private Vector _endVector;
    private double _inArmLength;
    private bool _inArmLengthPlus;
    private NormalizationFlags _normalizationFlags;
    private double _outArmLength;
    private bool _outArmLengthPlus;
    private PathRay _start;
    private Vector _startVector;


    public SegmentFlags Flags;
    public ArcPathMakerVertex Point;
    public ArcPathMakerVertex PreviousPoint;
    public IPathValidator? Validator;

    #endregion
}

/*
internal sealed class PdPathBuilder : PathBuilder
{
    public PdPathBuilder(Point currentPoint, IPathValidator? validator = null)
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
    }#1#
    public void Mark(ref int idx, WayPoint wayPoint)
    {
        throw new NotImplementedException();
    }
}
*/
