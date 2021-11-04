using System.Drawing;
using System.Drawing.Drawing2D;

namespace iSukces.DrawingPanel
{
    public class SmileDrawable : DrawableBase
    {
        public override void Draw(Graphics graphics)
        {
            using var pen = new Pen(Color.Fuchsia, 3);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var savedTransform = graphics.Transform;
            try
            {
                using var matrix = _canvasInfo.Transformation.GetTransform();
                graphics.Transform = matrix;

                graphics.DrawEllipse(pen, 10, 10, 100, 100);

                graphics.DrawEllipse(pen, 30, 50, 10, 10);
                graphics.DrawEllipse(pen, 80, 50, 10, 10);

                var center = new Point(60, 80);
                var v1     = new IntVector(20, 0);

                graphics.DrawPolygon(pen, new[]
                {
                    center - v1 + new IntVector(-15, -6),
                    center - v1,
                    center + v1,
                    center + v1 + new IntVector(15, -6)
                });
            }
            finally
            {
                graphics.Transform = savedTransform;
                savedTransform.Dispose();
            }
        }
    }
}
