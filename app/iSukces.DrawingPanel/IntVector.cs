using System.Drawing;

namespace iSukces.DrawingPanel
{
    internal struct IntVector
    {
        private readonly int _x;
        private readonly int _y;

        public IntVector(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public static IntVector operator +(IntVector a, IntVector b)
        {
            return new IntVector(a._x + b._x, a._y + b._y);
        }

        public static Point operator +(Point a, IntVector b)
        {
            return new Point(a.X + b._x, a.Y + b._y);
        }

        public static Point operator -(Point a, IntVector b)
        {
            return new Point(a.X - b._x, a.Y - b._y);
        }
    }
}