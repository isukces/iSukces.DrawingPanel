using System;

namespace iSukces.DrawingPanel.Interfaces;

public interface IDrawableBase 
{
    void SetCanvasInfo(DrawingCanvasInfo canvasInfo);
    event EventHandler? Changed;
    bool                Visible { get; }


    /// <summary>
    ///     Private data set by rendering engine. Do not write your data here !
    /// </summary>
    bool PresenterRenderingFlag { get; set; }
}
