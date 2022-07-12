#if COREFX
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.DrawingPanel.Interfaces
{
    public struct SnapResult
    {
        public SnapResult(Point point)
        {
            Point = point;    
        }

        public Point Point { get; }
    }
}