using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using iSukces.Mathematics;
#if COMPATMATH
using iSukces.Mathematics.Compatibility;

#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths;

[DebuggerDisplay("{GetDebuggerDisplay()}")]
public sealed class ArcDefinition : IPathElement, ILineCollider
{
    public ArcDefinition()
    {
    }

    public ArcDefinition(Point center, Point start, Vector directionStart, Point end)
    {
        Center         = center;
        Start          = start;
        DirectionStart = directionStart;
        End            = end;
        RadiusStart    = start - center;
        RadiusEnd      = end - center;
    }

    public ArcDefinition(Point center, Point start, Vector directionStart, Point end,
        double radius)
    {
        Center         = center;
        Start          = start;
        DirectionStart = directionStart;
        End            = end;
        RadiusStart    = start - center;
        RadiusEnd      = end - center;
        UseRadius(radius);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ArcDefinition? Make(PathRay a, Point b, Vector vb)
    {
        return Make(a.Point, a.Vector, b, vb);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ArcDefinition? Make(PathRay a, PathRay b)
    {
        return Make(a.Point, a.Vector, b.Point, b.Vector);
    }

    public static ArcDefinition? Make(Point a, Vector va, Point b, Vector vb)
    {
        var l1     = PathLineEquationNotNormalized.FromPointAndDeltas(a, va.GetPrependicularVector());
        var l2     = PathLineEquationNotNormalized.FromPointAndDeltas(b, vb.GetPrependicularVector());
        var center = l1.CrossWith(l2);
        if (center is null)
            return null;
        /*
        var ttt = center.Value - a;
        if (ttt.Length < 1e-10)
            Debug.Write("");
        */
        var c = new ArcDefinition
        {
            Center         = center.Value,
            Start          = a,
            End            = b,
            DirectionStart = va
        };
        c.RadiusStart = c.Start - c.Center;
        c.RadiusEnd   = c.End - c.Center;
        return c;
    }


    public static ArcDefinition operator +(ArcDefinition? a, Vector v)
    {
        if (a is null)
            return null;
        return a.GetMoved(v);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector DirectionVectorFromRadius(Vector radiusVector)
    {
        return radiusVector.GetPrependicular(Direction == ArcDirection.CounterClockwise);
    }

    public double DistanceFromElement(Point point, out double distanceFromStart, out Vector direction)
    {
        var v          = point - Center;
        var sweepAngle = Angle;
        var isInside   = FindAngleLocation(v, out var trackAngle);
        if (Direction == ArcDirection.CounterClockwise)
        {
            switch (isInside)
            {
                case Three.Below:
                    distanceFromStart = 0;
                    direction         = DirectionStart;
                    return (point - Start).Length;
                case Three.Above:
                    distanceFromStart = sweepAngle * PathsMathUtils.DegreesToRadians * Radius;
                    direction         = DirectionEnd;
                    return (point - End).Length;
                default:
                    var vLength = v.Length;
                    var radius  = Radius;

                    distanceFromStart = trackAngle * PathsMathUtils.DegreesToRadians * radius;
                    var dist = vLength - radius;
                    if (dist < 0)
                        dist = -dist;
                    direction = v.GetPrependicular();
                    return dist;
            }
        }

        switch (isInside)
        {
            case Three.Above:
                distanceFromStart = sweepAngle * PathsMathUtils.DegreesToRadians * Radius;
                direction         = DirectionEnd;
                return (point - End).Length;
            case Three.Below:
                distanceFromStart = 0;
                direction         = DirectionStart;
                return (point - Start).Length;
            default:
                var vLength = v.Length;
                var radius  = Radius;
                distanceFromStart =
                    (/*sweepAngle -*/ trackAngle) * PathsMathUtils.DegreesToRadians * radius;

                var dist = vLength - radius;
                if (dist < 0)
                    dist = -dist;
                direction = v.GetPrependicular(false);
                return dist;
        }
    }

    private Three FindAngleLocation(Vector centerToPoint, out double trackAngle)
    {
        var vectorAngle = centerToPoint.Angle();
        var sweepAngle  = Angle;
        if (Direction == ArcDirection.CounterClockwise)
        {
            trackAngle = vectorAngle - StartAngle;
            var isInside = PathsMathUtils.IsAngleInRegion(trackAngle, sweepAngle);
            return isInside;
        }
        else
        {
            /*var min = EndAngle; // StartAngle - sweepAngle;
            min        = PathsMathUtils.NormalizeAngleDeg(min);
            trackAngle = vectorAngle - min;
            */
            trackAngle = StartAngle - vectorAngle;
            if (trackAngle < 0)
            {
                trackAngle += 360;
                // when angleMinusMinAngle is small negative i.e.-1.4210854715202004E-14
                // its possible that angleMinusMinAngle+360 equals 360
                if (trackAngle >= 360)
                    trackAngle -= 360;
            }

            var isInside = PathsMathUtils.IsAngleInRegion(trackAngle, sweepAngle);
            return isInside;
        }
    }


    public ClosestPointResult FindClosestPointOnElement(Point target)
    {
        var v        = target - Center;
        var isInside = FindAngleLocation(v, out var trackAngle);

        if (isInside == Three.Inside)
        {
            var    radius = Radius - v.Length;
            var    qPoint = target + radius * v.GetNormalized();
            double track  = trackAngle * Radius * MathEx.DEGTORAD;
#if DEBUG
            if (track < 0)
                throw new Exception("track<0");
            if (track > GetLength())
                throw new Exception("track>GetLength()");

#endif
            var side = v.Length - Radius;
            if (Direction == ArcDirection.Clockwise)
                side = -side;
            return new ClosestPointResult(qPoint, isInside, track, side, DirectionVectorFromRadius(v));
        }

        var v1 = target - Start;
        var v2 = target - End;
        if (v1.LengthSquared < v2.LengthSquared)
        {
            var versor = DirectionStart;
            versor.Normalize();
            var track  = versor * v1;
#if DEBUG
            if (track > 0)
                throw new Exception("track>0");
#endif
            var side = versor.GetPrependicular(false) * v1;
            return new ClosestPointResult(Start, Three.Below, track, side, versor);
        }
        else
        {
            var versor = DirectionEnd;
            versor.Normalize();
            var track  = versor * v2;
#if DEBUG
            if (track < 0)
                throw new Exception("track<0");
#endif
            var side   = versor.GetPrependicular(false) * v2;
            var length = GetLength();
            return new ClosestPointResult(End, Three.Above, track + length, side, versor);
        }
    }

    public ArcDefinition FixEndPoint()
    {
        var d    = (End - Center);
        var l    = Radius - d.Length;
        var unit = d;
        unit.Normalize();
        End += l * unit;
#if DEBUG
        d = (End - Center);
        l = Math.Abs(d.Length - Radius);
        if (l > 1e-8)
            throw new Exception("Invalid end point");
#endif
        return this;
    }

    public ArcDefinition FixStartDirection()
    {
        var dir = DirectionVectorFromRadius(RadiusStart);
        dir.Normalize();
#if DEBUG
        if (dir * DirectionStart < 0)
            throw new Exception("Invalid direction");
#endif
        DirectionStart = dir;
        return this;
    }

    public ArcDefinition GetComplementar()
    {
        const ArcFlags mask = ArcFlags.HasRadius
                              | ArcFlags.HasDirection
                              | ArcFlags.IsCounterClockwise
                              | ArcFlags.HasChord;
        var result = new ArcDefinition
        {
            Center         = Center,
            RadiusStart    = RadiusEnd,
            RadiusEnd      = RadiusStart,
            DirectionStart = DirectionEnd,
            Start          = End,
            End            = Start,
            _flags         = _flags & mask,
            _radius        = _radius,
            _chordCached   = _chordCached
        };
        return result;
    }

    internal string GetDebuggerDisplay()
    {
        return this.CsCode();
    }

    public string GetDirectionAlternative()
    {
        var isRight = Direction == ArcDirection.Clockwise;
        return isRight ? "Right" : "Left";
    }

    Point IPathElement.GetEndPoint()
    {
        return End;
    }

    public PathRay GetEndRay()
    {
        return new PathRay(End, DirectionEnd);
    }

    Vector IPathElement.GetEndVector()
    {
        return DirectionEnd;
    }

    internal ArcFlags GetFlags()
    {
        return _flags;
    }

    public double GetLength()
    {
        return Radius * Angle * MathEx.DEGTORAD;
    }

    private ArcDefinition GetMoved(Vector v)
    {
        var clone = (ArcDefinition)MemberwiseClone();
        clone.Center += v;
        clone._start += v;
        clone._end   += v;
        return clone;
    }

    public Point GetNearestPointOnCircle(Point point)
    {
        var v = point - Center;
        v.Normalize();
        var res = Center + v * Radius;
        return res;
    }

    public Point GetNearestPointOnCircleFast(Point point)
    {
        var v          = point - Center;
        var lenSquared = v.LengthSquared;
        if (lenSquared == 0)
            return Start;
        var radiusSquared = RadiusStart.LengthSquared;
        var res           = Center + v * Math.Sqrt(radiusSquared / lenSquared);
        return res;
    }

    public Point[] GetPointsOnArc(int segmentsCount)
    {
        var p = new Point[segmentsCount + 1];
        p[0]             = Start;
        p[segmentsCount] = End;
        var stepsAngle = Angle / segmentsCount;
        if (Direction == ArcDirection.Clockwise)
            stepsAngle = -stepsAngle;
        for (int i = segmentsCount - 1; i > 0; i--)
        {
            var an = StartAngle + stepsAngle * i;
            MathEx.GetSinCos(an, out var sin, out var cos);
            p[i] = Center + new Vector(cos * Radius, sin * Radius);
        }

        return p;
    }


    /// <summary>
    ///     Works for angle less or equal to 180degrees
    /// </summary>
    /// <param name="segmentsCount"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public IReadOnlyList<Point> GetSmallAnglePoints(int segmentsCount)
    {
        if (segmentsCount < 1)
            throw new ArgumentException(nameof(segmentsCount) + " must be greater than 0", nameof(segmentsCount));
        var pointsCount = segmentsCount + 1;
        var result      = new Point[pointsCount];

        var v      = End - Start;
        var middle = new Point((_start.X + _end.X) * 0.5, (_start.Y + _end.Y) * 0.5);
        var chord  = v.Length;

        var factor        = chord / segmentsCount;
        var chordHalf     = chord * 0.5;
        var radius        = Radius;
        var radiusSquared = radius * radius;

        var tmp = radiusSquared - chordHalf * chordHalf;
        var a   = tmp <= 0 ? 0 : -Math.Sqrt(tmp);

        var xOne = v;
        xOne.Normalize();
        var yOne = xOne.GetPrependicular(Direction == ArcDirection.Clockwise);

        for (var idx = segmentsCount / 2; idx >= 1; idx--)
        {
            var x = idx * factor - chordHalf;

            var underSqrt       = radiusSquared - x * x;
            var h               = underSqrt <= 0 ? a : (a + Math.Sqrt(underSqrt));
            var p1              = middle + yOne * h;
            var plusMinusVector = xOne * x;
            result[idx]                 = p1 + plusMinusVector;
            result[segmentsCount - idx] = p1 - plusMinusVector;
        }

        result[0]             = _start;
        result[segmentsCount] = _end;
        return result;
    }

    Point IPathElement.GetStartPoint()
    {
        return Start;
    }

    public PathRay GetStartRay()
    {
        return new PathRay(Start, DirectionStart);
    }

    Vector IPathElement.GetStartVector()
    {
        return DirectionStart;
    }

    public bool IsLineCollision(Point hitPoint, double toleranceSquared, out double distanceSquared,
        out Point correctedPoint)
    {
        var toCenter = hitPoint - Center;

        var x1 = RadiusStart.X;
        var y1 = RadiusStart.Y;
        var x2 = toCenter.X;
        var y2 = toCenter.Y;

        var y1Sq = y1 * y1;
        var x1Sq = x1 * x1;
        var y2Sq = y2 * y2;
        var x2Sq = x2 * x2;

        var tmpSqrt     = (y1Sq + x1Sq) * (y2Sq + x2Sq);
        var distSquared = -2 * Math.Sqrt(tmpSqrt) + y2Sq + y1Sq + x2Sq + x1Sq;

        distanceSquared = distSquared;
        if (distSquared <= toleranceSquared)
        {
            var angle = Vector.AngleBetween(toCenter, RadiusStart);
            if (Direction == ArcDirection.CounterClockwise)
                angle = -angle;
            if (angle < 0)
                angle += 360;

            if (angle >= 0 && angle <= Angle)
            {
                // todo: maybe it can be faster
                correctedPoint = GetNearestPointOnCircleFast(hitPoint);
                return true;
            }
        }

        correctedPoint = default;
        return false;
    }


    public void MoveStartAndUpdateVectors(Point start)
    {
        var leftHand = Direction == ArcDirection.CounterClockwise;
        Start = start;
        UpdateRadiusStart();
        DirectionStart = RadiusStart.GetPrependicular(leftHand).GetNormalized();
    }

    internal void ResetFlags()
    {
        _flags = ArcFlags.None;
    }

    public override string ToString()
    {
        return $"{Angle:N2}°, r={Radius:N2}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UpdateRadiusEnd()
    {
        RadiusEnd = End - Center;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UpdateRadiusStart()
    {
        RadiusStart = Start - Center;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UpdateRadiusVectors()
    {
        var center = Center;
        var start  = Start - center;
        var end    = End - center;
        RadiusStart = start;
        RadiusEnd   = end;
    }


    /// <summary>
    ///     Use this value as radius. It's useful when center and/or start point is calculated from known radius
    /// </summary>
    /// <param name="radius"></param>
    public void UseRadius(double radius)
    {
        _flags |= ArcFlags.HasRadius;
        if (_radius.Equals(radius))
            return;
        _radius =  radius;
        _flags  &= ~ChangedRadius;
    }

    #region properties

    public Vector RadiusEnd
    {
        get => _radiusEnd;
        set
        {
            _radiusEnd =  value;
            _flags     &= ChangedRadiusEnd;
        }
    }

    public Vector RadiusStart
    {
        get => _radiusStart;
        set
        {
            _radiusStart =  value;
            _flags       &= ChangedRadiusStart;
        }
    }

    public Vector DirectionEnd
    {
        get
        {
            if (Direction == ArcDirection.CounterClockwise) // MathematicalPlus
                return new Vector(-RadiusEnd.Y, RadiusEnd.X);
            return new Vector(RadiusEnd.Y, -RadiusEnd.X);
        }
    }

    public Vector DirectionStart
    {
        get => _directionStart;
        set
        {
            _directionStart =  value;
            _flags          &= ChangedDirectionStart;
        }
    }

    public Point Start
    {
        get => _start;
        set
        {
            _start =  value;
            _flags &= ChangedStart;
        }
    }

    public Point End
    {
        get => _end;
        set
        {
            _end   =  value;
            _flags &= ChangedEnd;
        }
    }

    public Point Center { get; set; }

    public double Radius
    {
        get
        {
            if ((_flags & ArcFlags.HasRadius) != 0)
                return _radius;
            _flags  |= ArcFlags.HasRadius;
            _radius =  RadiusStart.Length;
            _flags  &= ~ChangedRadius;
            return _radius;
        }
    }


    public double Angle
    {
        get
        {
            if ((_flags & ArcFlags.HasAngle) != 0)
            {
                return _angleCached;
            }

            _flags |= ArcFlags.HasAngle;

            var angle = Vector.AngleBetween(RadiusStart, RadiusEnd);
            switch (angle)
            {
                case 0: return 0;
                case < 0:
                    angle += 360;
                    break;
            }

            if (Direction == ArcDirection.Clockwise)
                angle = 360 - angle;

            _angleCached = angle;
            return angle;
        }
    }

    public ArcDirection Direction
    {
        get
        {
            if ((_flags & ArcFlags.HasDirection) != 0)
            {
                return (_flags & ArcFlags.IsCounterClockwise) == 0
                    ? ArcDirection.Clockwise
                    : ArcDirection.CounterClockwise;
            }

            _flags |= ArcFlags.HasDirection;

            var dotStart = Vector.CrossProduct(RadiusStart, DirectionStart);
            if (dotStart < 0)
            {
                //_direction= ArcDirection.Clockwise; // MathematicalMinus
                _flags &= ~ArcFlags.IsCounterClockwise;
            }
            else
            {
                //_direction= ArcDirection.CounterClockwise; // MathematicalPlus;
                _flags |= ArcFlags.IsCounterClockwise;
            }

            return (_flags & ArcFlags.IsCounterClockwise) == 0
                ? ArcDirection.Clockwise
                : ArcDirection.CounterClockwise;
        }
    }


    /// <summary>
    ///     see https://en.wikipedia.org/wiki/Sagitta_(geometry)
    /// </summary>
    public double Sagitta
    {
        get
        {
            if ((_flags & ArcFlags.HasSagitta) != 0)
                return _sagittaCached;
            _flags |= ArcFlags.HasSagitta;

            var r   = Radius;
            var lsq = (End - Start).LengthSquared;
            _sagittaCached = r - Math.Sqrt(r * r - lsq * 0.25);
            return _sagittaCached;
        }
    }


    /// <summary>
    ///     see https://en.wikipedia.org/wiki/Chord_(geometry)
    /// </summary>
    public double Chord
    {
        get
        {
            if ((_flags & ArcFlags.HasChord) != 0)
                return _chordCached;
            _flags       |= ArcFlags.HasChord;
            _chordCached =  (End - Start).Length;
            return _chordCached;
        }
    }


    public double StartAngle
    {
        get
        {
            if ((_flags & ArcFlags.HasStartAngle) != 0)
                return _startAngleCached;
            _flags            |= ArcFlags.HasStartAngle;
            _startAngleCached =  RadiusStart.Angle();
            return _startAngleCached;
        }
    }

    public double EndAngle
    {
        get
        {
            if ((_flags & ArcFlags.HasEndAngle) != 0)
                return _endAngleCached;
            _flags          |= ArcFlags.HasEndAngle;
            _endAngleCached =  RadiusEnd.Angle();
            return _endAngleCached;
        }
    }

    #endregion

    public object Tag { get; set; }

    #region Fields

    internal const ArcFlags ChangedStart = ~(ArcFlags.HasChord | ArcFlags.HasSagitta);

    internal const ArcFlags ChangedEnd = ~(ArcFlags.HasChord | ArcFlags.HasSagitta);

    internal const ArcFlags ChangedRadius = ~ArcFlags.HasSagitta;


    internal const ArcFlags ChangedRadiusStart = ~(ArcFlags.HasDirection
                                                   | ArcFlags.HasAngle
                                                   | ArcFlags.HasRadius
                                                   | ArcFlags.HasSagitta
                                                   | ArcFlags.HasStartAngle);

    internal const ArcFlags ChangedRadiusEnd = ~(ArcFlags.HasAngle
                                                 | ArcFlags.HasEndAngle);

    internal const ArcFlags ChangedDirectionStart = ~(ArcFlags.HasDirection
                                                      | ArcFlags.HasAngle);


    private double _angleCached;
    private double _chordCached;
    private Vector _directionStart;
    private Point _end;
    private double _endAngleCached;
    private ArcFlags _flags;
    private double _radius;
    private Vector _radiusEnd;
    private Vector _radiusStart;
    private double _sagittaCached;
    private Point _start;
    private double _startAngleCached;

    #endregion

    [Flags]
    internal enum ArcFlags : byte
    {
        None = 0,
        HasDirection = 1,
        HasAngle = 2,
        IsCounterClockwise = 4,
        HasRadius = 8,
        HasChord = 16,
        HasSagitta = 32,
        HasStartAngle = 64,
        HasEndAngle = 128
    }
}
