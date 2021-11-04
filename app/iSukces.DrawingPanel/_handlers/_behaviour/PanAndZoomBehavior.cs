using System;
using System.Drawing;
using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;
using WinPoint = System.Windows.Point;

namespace iSukces.DrawingPanel
{
    public sealed class PanAndZoomBehavior : INewMouseWheelHandler, INewMouseButtonHandler, IDrawingPanelSizeChangedHandler
    {
        public PanAndZoomBehavior(double mouseWheelResponsibility, ZoomInfo zoom)
        {
            _mouseWheelResponsibility = mouseWheelResponsibility;
            Zoom                      = zoom;
        }

        public ZoomInfo Zoom        { get; }
        public Size     DrawingSize { get; set; }

        private Point _lastMouseSeenAt;
 
        private readonly double _mouseWheelResponsibility;

        private sealed class DragMoveContext
        {
            public Point MouseStart { get; set; }

            public WinPoint Start { get; set; }
        }


        public DrawingHandleResult HandleMouseWheel(MouseEventArgs e)
        {
            var toLogicalBefore = ToLogicalLocation(_lastMouseSeenAt);

            var scale = Zoom.Scale * Math.Exp(e.Delta / _mouseWheelResponsibility);
            Zoom.Scale = scale;

            var toLogicalAfter = ToLogicalLocation(_lastMouseSeenAt);
            var deltaCenter    = toLogicalBefore - toLogicalAfter;

            Zoom.Center += deltaCenter;

            //todo: to oznacza całkowite zawłaszczenie rolki
            return DrawingHandleResult.Break;
        }

        public DrawingHandleResult HandleOnMouseMove(MouseEventArgs args)
        {
            _lastMouseSeenAt = args.Location;
            if (_c is null)
                return DrawingHandleResult.ContinueAfterAction;

            if (args.Location == _c.MouseStart)
                return DrawingHandleResult.Break;
            var info      = GetDrawingCanvasInfo();
            var toLogical = info.FromCanvas(args.Location);
            var delta     = toLogical - _c.Start;
            Zoom.Center -= delta;

            return DrawingHandleResult.Break;
        }

        private DragMoveContext _c;

        public DrawingHandleResult HandleOnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                var startPoint = ToLogicalLocation(e.Location);
                _c = new DragMoveContext
                {
                    Start      = startPoint,
                    MouseStart = e.Location
                };
                return DrawingHandleResult.Break;
            }

            return DrawingHandleResult.Continue;
        }

        private WinPoint ToLogicalLocation(Point mouseLocation)
        {
            var info            = GetDrawingCanvasInfo();
            var toLogicalBefore = info.FromCanvas(mouseLocation);
            return toLogicalBefore;
        }

        private FlippedYDrawingToPixelsTransformation GetDrawingCanvasInfo()
        {
            return FlippedYDrawingToPixelsTransformation.Make(DrawingSize, Zoom.Center, Zoom.Scale);
        }

        public DrawingHandleResult HandleOnMouseUp(MouseEventArgs e)
        {
            if (_c is null || e.Button != MouseButtons.Middle)
                return DrawingHandleResult.Continue;
            _c = null;
            return DrawingHandleResult.Break;
        }
    }
}