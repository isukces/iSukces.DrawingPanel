﻿using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;
using WinPoint = System.Windows.Point;

namespace iSukces.DrawingPanel.Sample
{
    public partial class PolilineDrawer : DrawableBase, INewMouseButtonHandler
    {
        private static void DrawDimension(Graphics graphics, WinPoint a, WinPoint b,
            IDrawingToPixelsTransformation transf)
        {
            using var pen = new Pen(Color.Wheat, 1);

            var main          = b - a;
            var l             = main.Length;
            var prependicular = new Vector(main.Y, -main.X);
            var flipped       = main.X > 0;
            if (flipped)
                prependicular = -prependicular;
            prependicular *= 1 / (l * transf.Scale);

            var aAx = transf.ToCanvasF(a);
            var bAx = transf.ToCanvasF(b);
            {
                const int extLinePlus = 5;
                const int extLine     = 15;
                {
                    var prepLong = prependicular * (extLinePlus + extLine);
                    graphics.DrawLine(pen, aAx, transf.ToCanvasF(a + prepLong));
                    graphics.DrawLine(pen, bAx, transf.ToCanvasF(b + prepLong));
                }
                {
                    var prepShort     = prependicular * extLine;
                    var makeLongerLen = flipped ? extLinePlus : -extLinePlus;
                    var pararell      = new Vector(prependicular.Y * makeLongerLen, -prependicular.X * makeLongerLen);
                    graphics.DrawLine(pen,
                        transf.ToCanvasF(a + prepShort - pararell),
                        transf.ToCanvasF(b + prepShort + pararell)
                    );
                }
            }
            {
                // draw text
                var font  = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
                var brush = new SolidBrush(pen.Color);
                var point = transf.ToCanvasF(b);
                var text  = $"{l:N2}m";

                var textSize = graphics.MeasureString(text, font);
                if (main.X < 0)
                {
                    point = new PointF(point.X - 12 - textSize.Width, point.Y - 20);
                }
                else
                {
                    point = new PointF(point.X + 5, point.Y - 20);
                }

                var rect = new RectangleF(point.X - 2, point.Y - 2, textSize.Width + 4, textSize.Height + 4);
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(160, Color.Black)), rect);
                graphics.DrawString(text, font, brush, point);
            }
        }

        public override void Draw(Graphics graphics)
        {
            if (_session is null)
                return;

            var src       = _session.Points;
            var lastPoint = src[src.Count - 1];
            var pointsF   = src.MapToArray(a => new PointF((float)a.X, (float)a.Y));
            var m         = _canvasInfo.Transformation.GetTransform();
            m.TransformPoints(pointsF);
            m.Dispose();

            using var pen = new Pen(Color.Chartreuse, 3);
            graphics.DrawLines(pen, pointsF);
            // ==========
            DrawDimension(graphics, src[src.Count - 2], lastPoint, _canvasInfo.Transformation);

            DrawAlignToLines(graphics, pointsF);
        }

        private void DrawAlignToLines(Graphics graphics, PointF[] pointsF)
        {
            var pp       = _session.PointAlign.GetUnique();
            var ppLength = pp.Length;
            if (ppLength == 0)
                return;
            using var pen = new Pen(Color.Orange, 1);
            for (var index = ppLength - 1; index >= 0; index--)
            {
                var p      = pp[index];
                var transf = _canvasInfo.Transformation;
                var begin  = transf.ToCanvasF(_session.Points[p]);
                var end    = pointsF[pointsF.Length - 1];
                graphics.DrawLine(pen, begin, end);
            }
        }
    }

    partial class PolilineDrawer
    {
        public DrawingHandleResult HandleOnMouseDown(MouseEventArgs e)
        {
            return DrawingHandleResult.Continue;
        }

        public DrawingHandleResult HandleOnMouseMove(MouseEventArgs args)
        {
            if (_session is null)
                return DrawingHandleResult.Continue;
            var point = _canvasInfo.Transformation.FromCanvas(args.Location);
            point = _session.ProcessPoint(point, out _session.PointAlign, _canvasInfo.Transformation.Scale);
            _session.Points[_session.Points.Count - 1] = point;
            OnChanged();
            return DrawingHandleResult.Break;
        }

        public DrawingHandleResult HandleOnMouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return DrawingHandleResult.Continue;

            var point = _canvasInfo.Transformation.FromCanvas(e.Location);
            if (_session is null)
            {
                _session = new DrawingSession();
                _session.Points.Add(point);
            }

            _session.Points.Add(point);
            OnChanged();
            return DrawingHandleResult.Break;
        }

        private DrawingSession _session;
    }
}