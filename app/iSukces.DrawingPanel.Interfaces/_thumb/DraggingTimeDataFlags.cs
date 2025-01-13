#nullable disable
using System;

namespace iSukces.DrawingPanel.Interfaces;

[Flags]
public enum DraggingTimeDataFlags
{
    None = 0,

    /// <summary>
    /// Denotes that vertex is under mouse
    /// </summary>
    UnderMouse = 1,

    Arrange = 2,
    InvokeLocationMove = 4,
    DontUpdateVector = 8,
}
