#nullable disable
using System;
using System.Collections.Generic;
using System.Windows.Input;
using iSukces.DrawingPanel.Interfaces;
#if COMPATMATH
using WinPoint=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using WinPoint=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif

namespace iSukces.DrawingPanel;

public class ThumbLogicBase<TThumb>
    where TThumb : DrawableThumb
{
    protected ThumbLogicBase(IReadOnlyList<TThumb> thumbs, object model, ISnapService snap)
    {
        Thumbs = thumbs;
        Model  = model;
        Snap   = snap;
    }


    public DrawingHandleResult LeftMouseButtonReleased(Action afterThumbDragWithMoveCompleted)
    {
        if (DraggingSession is null)
            return DrawingHandleResult.Continue;
        OnDragCompleted(afterThumbDragWithMoveCompleted);
        return DrawingHandleResult.Break;
    }

    public void MoveOtherThumbsAfterMainMoved(WinPoint newThumbPoint, WinPoint mainThumbCenterBeforeMove, MoveThumbDelegate move)
    {
        var mainThumb          = DraggingSession.Thumb;
        var canMoveOtherThumbs = (mainThumb.Flags & ThumbFlags.MoveExclusively) == 0;
        if (!canMoveOtherThumbs) return;
        var after = newThumbPoint;
        var moved = after - mainThumbCenterBeforeMove;
        for (var idx = 0; idx < Thumbs.Count; idx++)
        {
            var thumb = Thumbs[idx];
            var dtd   = thumb.DraggingContext;
            if (dtd is null)
                continue;
            if (thumb.SameReference(mainThumb))
                continue;
            if ((dtd.Flags & DraggingTimeDataFlags.InvokeLocationMove) == 0) continue;
            var point = thumb.Center;
            point = new WinPoint(point.X + moved.X, point.Y + moved.Y);
            move(thumb, point);
        }
    }

    public virtual void OnDragCompleted(Action afterThumbDragWithMoveCompleted)
    {
        IVertexBasedModel model2 = Model as IVertexBasedModel;
        // using var resumeNotification = ProjectErrorsRecheckNotifierTools.SuspendNotification();
        model2?.VertexDragDeltaSuspenResume(StartOrStop.Start);
        try
        {
            DraggedStartingCounter--;
            if (!DraggingSession.WasMovement)
                UpdateThumbsSelection();
#if DEBUG && LOG
                    PdLog2.Default.Debug(GetType().Name, $"{nameof(DraggedStartingCounter)}={DraggedStartingCounter}");
#endif
            Snap.Reset();
            Snap.SetStartDraggingPoint(null);
            if (DraggingSession.WasMovement)
                afterThumbDragWithMoveCompleted();
        }
        finally
        {
            model2?.VertexDragDeltaSuspenResume(StartOrStop.Stop);
        }

        /*if (DraggingSession.WasMovement)
            if (Model is IEditableByVertexDragging x)
                x.StartStopVisualEditing(StartStop.Stop, Array.Empty<DraggingVerteksInfo>());

        _formsCursorHost.SetCursor(null, FormsCursorHostLevels.DragOverThumb);*/
        DraggingSession = null;
    }


    /// <summary>
    ///     Starts dragging session candidate (just after holding but before mouse moving).
    ///     It's possible that mouse button will be released without moving.
    /// </summary>
    /// <param name="thumb">clicked thumb</param>
    /// <param name="point">clicked point logic coordinates</param>
    public void OnDragInit(TThumb thumb, WinPoint point)
    {
        thumb.DraggingContext = new ThumbDraggingContext(DraggingTimeDataFlags.UnderMouse);
        DraggingSession       = new ThumbDraggingSession(thumb, point);
        DraggedStartingCounter++;
        Snap.SetStartDraggingPoint(DraggingSession.ThumbCenter);
    }

    protected virtual void StartRealDraggingThumb(TThumb thumb)
    {
    }

    public ThumbStartDragging TryStartRealDragging(double scale, WinPoint currentPoint, TThumb thumb)
    {
        if (DraggingSession.WasMovement)
            return ThumbStartDragging.AlreadyStarted;
        var distance          = DraggingSession.MouseStartingPointLogical - currentPoint;
        var deltaPixelSquared = distance.LengthSquared * scale * scale;
        if (!(deltaPixelSquared >= 2))
            return ThumbStartDragging.None;
        UpdateThumbsSelectedOnFirstMovement();
        StartRealDraggingThumb(thumb);
        return ThumbStartDragging.FirstMovementDetected;
    }

    protected virtual void UpdateThumbsSelectedOnFirstMovement()
    {
    }

    protected void UpdateThumbsSelection()
    {
        var draggedThumb = DraggingSession.Thumb;
        draggedThumb.IsSelected = !draggedThumb.IsSelected;
        if (Keyboard.IsKeyDown(Key.LeftShift))
            return;
        for (var index = Thumbs.Count - 1; index >= 0; index--)
        {
            var thumb = Thumbs[index];
            if (thumb.SameReference(draggedThumb))
                continue;
            thumb.IsSelected = false;
        }
    }

    public delegate void MoveThumbDelegate(TThumb thumb, WinPoint newLocation);

    #region properties

    protected IReadOnlyList<TThumb> Thumbs { get; }

    private ISnapService Snap { get; }

    protected object Model { get; }

    public int                  DraggedStartingCounter { get; protected set; }
    public ThumbDraggingSession DraggingSession        { get; protected set; }

    #endregion

    public sealed class ThumbDraggingSession
    {
        public ThumbDraggingSession(TThumb thumb, WinPoint mouseStartingPointLogical)
        {
            Thumb                     = thumb;
            ThumbCenter               = thumb.Center;
            MouseStartingPointLogical = mouseStartingPointLogical;
            MouseOffsetToThumbCenter  = mouseStartingPointLogical - ThumbCenter;
        }

        public void MarkAsMoved()
        {
            WasMovement = true;
        }

        #region properties

        public TThumb   Thumb                     { get; }
        public WinPoint ThumbCenter               { get; }
        public WinPoint MouseStartingPointLogical { get; }
        public Vector   MouseOffsetToThumbCenter  { get; }
        public bool     WasMovement               { get; private set; }

        #endregion
    }
}

public enum ThumbStartDragging
{
    /// <summary>
    ///     Thumb is already dragged
    /// </summary>
    AlreadyStarted,

    /// <summary>
    ///     Mouse was moved far enought since mouse down so we can assume that dragging was started
    /// </summary>
    FirstMovementDetected,

    /// <summary>
    ///     Mouse still not moved far enought
    /// </summary>
    None
}
