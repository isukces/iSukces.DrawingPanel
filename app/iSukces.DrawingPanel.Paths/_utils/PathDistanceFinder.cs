#if COREFX
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
using System;
using System.Runtime.CompilerServices;


namespace iSukces.DrawingPanel.Paths
{
    public static class PathDistanceFinder
    {
        public static PathDistanceFinderResult GetDistanceFromLine(IPathResult path, Point point)
        {
            if (path is null || path.Elements.Count == 0)
                return null;

            return new Finder(path, point).Find();
        }


        private class Finder
        {
            public Finder(IPathResult path, Point point)
            {
                _path  = path;
                _point = point;
            }

            public PathDistanceFinderResult Find()
            {
                _bestSquaredDistance = double.NaN;
                _best                = default;
                _bestElement         = null;
                var start      = 0d;
                var bestOffset = 0d;
                var bestIdx    = -1;
                for (var index = 0; index < _path.Elements.Count; index++)
                {
                    _element = _path.Elements[index];
                    _current = _element.FindClosestPointOnElement(_point);
                    var movement = _current.ClosestPoint - _point;

                    _distSquared = movement.LengthSquared;

                    var isBetter = IsNewSolutionBetter();

                    if (isBetter)
                    {
                        _bestSquaredDistance = _distSquared;
                        _best                = _current;
                        _bestElement         = _element;
                        bestOffset           = start;
                        bestIdx              = index;
                    }

                    start += _element.GetLength();
                }

                if (_best is null)
                    return null;

                return new PathDistanceFinderResult(
                    distanceFromLine: Math.Sqrt(_bestSquaredDistance),
                    location: _best.Location,
                    track: _best.ElementTrack,
                    direction: _best.Direction,
                    sideMovement: _best.SideMovement,
                    closestPoint: _best.ClosestPoint,
                    elementIndex: bestIdx,
                    elementTrackOffset: bestOffset);
            }

            private bool IsNewSolutionBetter()
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                static double GetOver(double track, double length)
                {
                    if (track < 0)
                        return -track;
                    var diff = track - length;
                    return diff > 0 ? diff : 0;
                }

                if (_best is null || double.IsNaN(_bestSquaredDistance))
                    return true;

                if (_best.Location == Three.Above && _current.Location == Three.Below)
                {
                    
                    var a = GetOver(_best.ElementTrack, _bestElement.GetLength());
                    var b = GetOver(_current.ElementTrack, _element.GetLength());
                    if (b < a)
                        return true;
                    return false;
                }
                // compare locations
                //var n = _current.Location;
                // var o = _best.Location;
                // if (o != Three.Inside && n == Three.Inside) return true;
                // compare distance when status of new is equal or better
                if (_distSquared <= _bestSquaredDistance)
                {
                    if (_distSquared < _bestSquaredDistance)
                        //if (o != Three.Inside || n == Three.Inside)
                        return true;
                    return _best.Location != Three.Inside || _current.Location == Three.Inside;
                }

                return false;
            }

            #region Fields

            private readonly IPathResult _path;
            private readonly Point _point;
            private ClosestPointResult _current;
            private ClosestPointResult _best;
            private IPathElement _bestElement;
            private double _distSquared;
            private double _bestSquaredDistance;
            private IPathElement _element;

            #endregion
        }
    }
}
