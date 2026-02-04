using System.Drawing;
using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;
using WinPoint=iSukces.Mathematics.Point;

namespace iSukces.DrawingPanel;

public sealed class PanAndZoomBehavior : IDpMouseWheelHandler, IDpMouseButtonHandler,
    IDpSizeChangedHandler
{
    public PanAndZoomBehavior(double mouseWheelResponsibility, ZoomInfo zoom)
    {
        _mouseWheelResponsibility = mouseWheelResponsibility;
        Zoom                      = zoom;
    }

    private FlippedYDrawingToPixelsTransformation GetDrawingCanvasInfo()
    {
        return FlippedYDrawingToPixelsTransformation.Make(DrawingSize, Zoom.Center, Zoom.Scale);
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

    public DrawingHandleResult HandleOnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Middle)
        {
            var startPoint = ToLogicalLocation(e.Location);
            _dragMoveContext = new DragMoveContext
            {
                Start      = startPoint,
                MouseStart = e.Location
            };
            return DrawingHandleResult.Break;
        }

        return DrawingHandleResult.Continue;
    }

    public DrawingHandleResult HandleOnMouseMove(MouseEventArgs args)
    {
        _lastMouseSeenAt = args.Location;
        if (_dragMoveContext is null)
            return DrawingHandleResult.ContinueAfterAction;

        if (args.Location == _dragMoveContext.MouseStart)
            return DrawingHandleResult.Break;
        var info      = GetDrawingCanvasInfo();
        var toLogical = info.FromCanvas(args.Location);
        var delta     = toLogical - _dragMoveContext.Start;
        Zoom.Center -= delta;

        return DrawingHandleResult.Break;
    }

    public DrawingHandleResult HandleOnMouseUp(MouseEventArgs e)
    {
        if (_dragMoveContext is null || e.Button != MouseButtons.Middle)
            return DrawingHandleResult.Continue;
        _dragMoveContext = null;
        return DrawingHandleResult.Break;
    }

    private WinPoint ToLogicalLocation(Point mouseLocation)
    {
        var info            = GetDrawingCanvasInfo();
        var toLogicalBefore = info.FromCanvas(mouseLocation);
        return toLogicalBefore;
    }

    public ZoomInfo Zoom        { get; }
    public Size     DrawingSize { get; set; }

    private readonly double _mouseWheelResponsibility;

    private DragMoveContext? _dragMoveContext;

    private Point _lastMouseSeenAt;

    private sealed class DragMoveContext
    {
        public Point MouseStart { get; set; }

        public WinPoint Start { get; set; }
    }
}
