using System;

namespace iSukces.DrawingPanel.Interfaces
{
    [Flags]
    public enum ThumbFlags : byte
    {
        None = 0,
        Fixed = 1,
        MoveExclusively = 2
    }
}
