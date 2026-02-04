using System;
using iSukces.Mathematics;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=iSukces.Mathematics.Point;
using Vector=iSukces.Mathematics.Vector;
#endif


namespace iSukces.DrawingPanel.Paths.Test;

public sealed class ArcDefinitionTrack : IPathTracker
{
    public static IPathTracker Make(ArcDefinition arc)
    {
        var angle  = arc.Angle;
        var angle2 = angle * MathEx.DEGTORAD;
        var radius = arc.Radius;
        var length = angle2 * radius;

        var radiusStart = arc.RadiusStart;
        var dir         = arc.Direction;

        var lengthToAngleFactor = 1 / radius;

        if (dir == ArcDirection.Clockwise)
            lengthToAngleFactor = -lengthToAngleFactor;
        return new ArcDefinitionTrack
        {
            _center              = arc.Center,
            _radius              = arc.Radius,
            _length              = length,
            _startAngle          = Math.Atan2(radiusStart.Y, radiusStart.X),
            _lengthToAngleFactor = lengthToAngleFactor,
            _dir                 = dir
        };
    }

    public double GetLength()
    {
        return _length;
    }

    public TrackInfo GetTrackInfo(double x)
    {
        var angle = _startAngle + _lengthToAngleFactor * x;
        var sin   = Math.Sin(angle);
        if (sin is -1 or 1)
        {
            var point  = new Point(_center.X, _center.Y + _radius * sin);
            var vector = _dir == ArcDirection.CounterClockwise ? new Vector(-sin, 0) : new Vector(sin, -0);
            return new TrackInfo(point, vector);
        }

        var cos = Math.Cos(angle);
        if (cos is -1 or 1)
        {
            var point  = new Point(_center.X + _radius * cos, _center.Y);
            var vector = _dir == ArcDirection.CounterClockwise ? new Vector(0, cos) : new Vector(0, -cos);
            return new TrackInfo(point, vector);
        }

        {
            var point  = new Point(_center.X + _radius * cos, _center.Y + _radius * sin);
            var vector = _dir == ArcDirection.CounterClockwise ? new Vector(-sin, cos) : new Vector(sin, -cos);
            return new TrackInfo(point, vector);
        }
    }

    #region Fields

    private ArcDirection _dir;
    private double _length;
    private double _lengthToAngleFactor;
    private double _startAngle;

    #endregion

#pragma warning disable 414
    private Point _center;
    private double _radius;
#pragma warning restore 414
}
