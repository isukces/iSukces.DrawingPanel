#nullable disable
using System;
using System.Drawing;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel.Interfaces;

public interface IDrawable
{
    void Draw([NotNull] Graphics graphics);
    void SetCanvasInfo([NotNull] DrawingCanvasInfo canvasInfo);
    event EventHandler Changed;
    bool               Visible { get; }


    /// <summary>
    ///     Private data set by rendering engine. Do not write your data here !
    /// </summary>
    bool PresenterRenderingFlag { get; set; }
}
