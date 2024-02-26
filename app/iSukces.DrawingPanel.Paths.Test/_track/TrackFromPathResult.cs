using System;
using System.Collections.Generic;

namespace iSukces.DrawingPanel.Paths.Test;

public sealed class TrackFromPathResult : IPathTracker
{
    private TrackFromPathResult(IReadOnlyList<IPathElement> elements)
    {
        var count = elements.Count;
        _trackers       = new IPathTracker[count];
        _startOfTracker = new double[count];

        var start = 0d;
        for (var i = 0; i < _startOfTracker.Length; i++)
        {
            var src = elements[i];
            _startOfTracker[i] = start;
            var track = ConvertPathTracker(src);

            var length = track.GetLength();
            _trackers[i] =  track;
            start        += length;
        }

        _length = start;
    }

    public static IPathTracker ConvertPathTracker(IPathElement src)
    {
        IPathTracker track;
        switch (src)
        {
            case ArcDefinition arcDefinition:
                track = ArcDefinitionTrack.Make(arcDefinition);
                break;
            case LinePathElement:
                var startPoint = src.GetStartPoint();
                var endPoint   = src.GetEndPoint();
                track = new LineTrackInfo(startPoint, endPoint);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(src));
        }

        return track;
    }

    public static IPathTracker Make(IPathResult result)
    {
        return Make(result.Elements);
    }

    public static IPathTracker Make(IReadOnlyList<IPathElement> elements)
    {
        var tmp    = new TrackFromPathResult(elements);
        var tracks = tmp._trackers;
        if (tracks.Length == 1)
            return tracks[0];
        return tmp;
    }

    public double GetLength()
    {
        return _length;
    }

    public TrackInfo GetTrackInfo(double x)
    {
        for (var i = _startOfTracker.Length - 1; i > 0; i--)
        {
            var start = _startOfTracker[i];
            if (start <= x)
                return _trackers[i].GetTrackInfo(x - start);
        }

        return _trackers[0].GetTrackInfo(x);
    }

    #region Fields

    private readonly double _length;

    private readonly double[] _startOfTracker;
    private readonly IPathTracker[] _trackers;

    #endregion
}