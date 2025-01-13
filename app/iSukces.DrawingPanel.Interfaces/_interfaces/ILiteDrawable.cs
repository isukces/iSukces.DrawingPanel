#nullable disable
using System.Drawing;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel.Interfaces;

public interface ILiteDrawable
{
    void Draw([NotNull] Graphics graphics, [NotNull] DrawingCanvasInfo canvasInfo);
}
