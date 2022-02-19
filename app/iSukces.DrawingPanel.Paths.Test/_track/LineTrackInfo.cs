using System.Windows;

namespace iSukces.DrawingPanel.Paths.Test
{
    public sealed class LineTrackInfo : IPathTracker
    {
        public LineTrackInfo(Point begin, Point end)
        {
            _begin = begin;
            var v = end - begin;
            _length = v.Length;
            if (_length == 0)
                _direction = new Vector(1, 0);
            else
            {
                v.Normalize();
                if (double.IsNaN(v.X))
                    _direction = new Vector(1, 0);
                else
                    _direction = v;
            }
        }

        public double GetLength()
        {
            return _length;
        }

        public TrackInfo GetTrackInfo(double x)
        {
            var p = _begin + x * _direction;
            return new TrackInfo(p, _direction);
        }

        #region Fields

        private readonly Point _begin;
        private readonly double _length;
        private readonly Vector _direction;

        #endregion
    }
}
