using System.Drawing;
using System.Drawing.Drawing2D;
#if COREFX
using WinPoint=iSukces.Mathematics.Compatibility.Point;
#else
using WinPoint=System.Windows.Point;
#endif


namespace iSukces.DrawingPanel.Interfaces
{
    public class FlippedYDrawingToPixelsTransformation : IDrawingToPixelsTransformation
    {
        public FlippedYDrawingToPixelsTransformation(double scale, double deltaX, double deltaY)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
            Scale  = scale;
        }

        public static FlippedYDrawingToPixelsTransformation Make(Size viewPortSize, WinPoint center, double scale)
        {
            var screenCenter = new WinPoint(viewPortSize.Width * 0.5, viewPortSize.Height * 0.5);

            var dx = screenCenter.X - center.X * scale;
            var dy = screenCenter.Y + center.Y * scale;
            return new FlippedYDrawingToPixelsTransformation(scale, dx, dy);
        }


        public WinPoint FromCanvas(WinPoint point)
        {
            var x = (point.X - DeltaX) / Scale;
            var y = (DeltaY - point.Y) / Scale;
            return new WinPoint(x, y);
        }

        public WinPoint FromCanvas(Point point)
        {
            var x = (point.X - DeltaX) / Scale;
            var y = (DeltaY - point.Y) / Scale;
            return new WinPoint(x, y);
        }

        public Matrix GetTransform()
        {
            var fScale = (float)Scale;
            var m = new Matrix(fScale, 0,
                0, -fScale,
                (float)DeltaX, (float)DeltaY);
            return m;
        }

        public WinPoint ToCanvas(WinPoint point)
        {
            var x = DeltaX + point.X * Scale;
            var y = DeltaY - point.Y * Scale;
            return new WinPoint(x, y);
        }

        public WinPoint ToCanvas(double pointX, double pointY)
        {
            var x = DeltaX + pointX * Scale;
            var y = DeltaY - pointY * Scale;
            return new WinPoint(x, y);
        }

        public PointF ToCanvasF(double pointX, double pointY)
        {
            var x = DeltaX + pointX * Scale;
            var y = DeltaY - pointY * Scale;
            return new PointF((float)x, (float)y);
        }


        public PointF ToCanvasF(WinPoint point)
        {
            var x = DeltaX + point.X * Scale;
            var y = DeltaY - point.Y * Scale;
            return new PointF((float)x, (float)y);
        }

        public override string ToString() { return $"DeltaX={DeltaX}, DeltaY={DeltaY}, Scale={Scale}"; }

        public double Scale { get; }

        public static readonly FlippedYDrawingToPixelsTransformation Empty =
            new FlippedYDrawingToPixelsTransformation(1, 0, 0);

        public readonly double DeltaX;
        public readonly double DeltaY;
    }
}
