using System;
using System.Runtime.CompilerServices;
using iSukces.Mathematics;
#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Paths
{
    public sealed class ArcDefinition : IPathElement, ILineCollider
    {
        public static ArcDefinition FromCenterAndArms(Point center, Point start, Vector startV, Point end)
        {
            var arc = new ArcDefinition
            {
                Center      = center,
                Start       = start,
                StartVector = startV,
                End         = end,
                RadiusStart = start - center,
                RadiusEnd   = end - center
            };
            return arc;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArcDefinition Make(PathRay a, Point b, Vector vb) { return Make(a.Point, a.Vector, b, vb); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArcDefinition Make(PathRay a, PathRay b) { return Make(a.Point, a.Vector, b.Point, b.Vector); }

        public static ArcDefinition Make(Point a, Vector va, Point b, Vector vb)
        {
            var l1     = LineEquationNotNormalized.FromPointAndDeltas(a, va.GetPrependicularVector());
            var l2     = LineEquationNotNormalized.FromPointAndDeltas(b, vb.GetPrependicularVector());
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
                Center      = center.Value,
                Start       = a,
                End         = b,
                StartVector = va,
            };
            c.RadiusStart = c.Start - c.Center;
            c.RadiusEnd   = c.End - c.Center;
            return c;
        }

        public double DistanceFromElement(Point point, out double distanceFromStart)
        {
            var v          = point - Center;
            var a          = v.Angle();
            var sweepAngle = Angle;
            if (Direction == ArcDirection.CounterClockwise)
            {
                var angleMinusStart = a - StartAngle;
                var isInside        = PathsMathUtils.IsAngleInRegion(angleMinusStart, sweepAngle);
                switch (isInside)
                {
                    case Three.Below:
                        distanceFromStart = 0;
                        return (point - Start).Length;
                    case Three.Above:
                        distanceFromStart = sweepAngle * PathsMathUtils.DegreesToRadians * Radius;
                        return (point - End).Length;
                    default:
                        var vLength = v.Length;

                        distanceFromStart = angleMinusStart * PathsMathUtils.DegreesToRadians * Radius;

                        var dist = vLength - Radius;
                        if (dist < 0)
                            dist = -dist;
                        return dist;
                }
            }
            else
            {
                var min = StartAngle - sweepAngle;
                min = PathsMathUtils.NormalizeAngleDeg(min);

                var angleMinusMinAngle = a - min;
                if (angleMinusMinAngle < 0)
                    angleMinusMinAngle += 360;
                var isInside = PathsMathUtils.IsAngleInRegion(angleMinusMinAngle, sweepAngle);
                switch (isInside)
                {
                    case Three.Below:
                        distanceFromStart = sweepAngle * PathsMathUtils.DegreesToRadians * Radius;
                        return (point - End).Length;
                    case Three.Above:
                        distanceFromStart = 0;
                        return (point - Start).Length;
                    default:
                        var vLength = v.Length;

                        distanceFromStart = (sweepAngle - angleMinusMinAngle) * PathsMathUtils.DegreesToRadians * Radius;

                        var dist = vLength - Radius;
                        if (dist < 0)
                            dist = -dist;
                        return dist;
                }
            }
        }

        public ArcDefinition GetComplementar()
        {
            const ArcFlags mask = ArcFlags.HasRadius
                                  | ArcFlags.HasDirection
                                  | ArcFlags.IsCounterClockwise
                                  | ArcFlags.HasChord;
            var result = new ArcDefinition
            {
                Center       = Center,
                RadiusStart  = RadiusEnd,
                RadiusEnd    = RadiusStart,
                StartVector  = EndVector,
                Start        = End,
                End          = Start,
                _flags       = _flags & mask,
                _radius      = _radius,
                _chordCached = _chordCached
            };
            return result;
        }


        public string GetDirectionAlternative()
        {
            var isRight = Direction == ArcDirection.Clockwise;
            return isRight ? "Right" : "Left";
        }

        Point IPathElement.GetEndPoint() { return End; }

        Vector IPathElement.GetEndVector() { return EndVector; }

        public double GetLength() { return Radius * Angle * MathEx.DEGTORAD; }


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

        Point IPathElement.GetStartPoint() { return Start; }

        Vector IPathElement.GetStartVector() { return StartVector; }

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

        public override string ToString() { return $"{Angle:N2}°, r={Radius:N2}"; }

        public void UpdateRadiusVectors()
        {
            RadiusStart = Start - Center;
            RadiusEnd   = End - Center;
        }


        /// <summary>
        ///     Use this value as radius. It's useful when center and/or start point is calculated from known radius
        /// </summary>
        /// <param name="radius"></param>
        public void UseRadius(double radius)
        {
            _flags  |= ArcFlags.HasRadius;
            _radius =  radius;
        }

        public double Radius
        {
            get
            {
                if ((_flags & ArcFlags.HasRadius) != 0)
                    return _radius;
                _flags  |= ArcFlags.HasRadius;
                _radius =  RadiusStart.Length;
                return _radius;
            }
        }

        public Vector RadiusEnd
        {
            get => _radiusEnd;
            set
            {
                const ArcFlags flags = ~(ArcFlags.HasDirection | ArcFlags.HasAngle);
                _radiusEnd =  value;
                _flags     &= flags;
            }
        }

        public Vector RadiusStart
        {
            get => _radiusStart;
            set
            {
                const ArcFlags arcFlags = ~(ArcFlags.HasDirection
                                            | ArcFlags.HasAngle
                                            | ArcFlags.HasRadius
                                            | ArcFlags.HasStartAngle);
                _radiusStart =  value;
                _flags       &= arcFlags;
            }
        }

        public Vector EndVector
        {
            get
            {
                if (Direction == ArcDirection.CounterClockwise) // MathematicalPlus
                    return new Vector(-RadiusEnd.Y, RadiusEnd.X);
                return new Vector(RadiusEnd.Y, -RadiusEnd.X);
            }
        }

        public Vector StartVector { get; set; }


        public Point Start
        {
            get => _start;
            set
            {
                _start = value;
                const ArcFlags arcFlags = ~(ArcFlags.HasChord);
                _flags &= arcFlags;
            }
        }

        public Point End
        {
            get => _end;
            set
            {
                _end = value;
                const ArcFlags arcFlags = ~(ArcFlags.HasChord);
                _flags &= arcFlags;
            }
        }

        public Point Center { get; set; }

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

                var dotStart = Vector.CrossProduct(RadiusStart, StartVector);
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

        private double _angleCached;
        private double _chordCached;
        private Point _end;
        private ArcFlags _flags;
        private double _radius;
        private Vector _radiusEnd;
        private Vector _radiusStart;
        private double _sagittaCached;
        private Point _start;
        private double _startAngleCached;

        [Flags]
        private enum ArcFlags : byte
        {
            None = 0,
            HasDirection = 1,
            HasAngle = 2,
            IsCounterClockwise = 4,
            HasRadius = 8,
            HasChord = 16,
            HasSagitta = 32,
            HasStartAngle = 64
        }
    }
}
