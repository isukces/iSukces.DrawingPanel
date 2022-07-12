using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel
{
    /// <summary>
    ///     Used when thumb is dragged
    /// </summary>
    public sealed class ThumbDraggingContext
    {
         
        public ThumbDraggingContext(DraggingTimeDataFlags flags)
        {
            Flags = flags;
        }

        public DraggingTimeDataFlags Flags { get; }
    }
}
