using System.Windows;

namespace iSukces.DrawingPanel.Paths.Test
{
    public readonly struct TrackInfo
    {
        public TrackInfo(Point location, Vector direction)
        {
            Location  = location;
            Direction = direction;
        }

        public override string ToString()
        {
            return $"TrackInfo {Location} -> {Direction}";
        }

        public Point  Location  { get; }
        public Vector Direction { get; }
    }
}
