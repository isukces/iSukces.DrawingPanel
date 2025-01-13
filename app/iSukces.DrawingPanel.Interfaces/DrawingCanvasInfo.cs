#nullable disable
using System.Drawing;
using System.Runtime.CompilerServices;

namespace iSukces.DrawingPanel.Interfaces;

public sealed class DrawingCanvasInfo
{
    public DrawingCanvasInfo(IDrawingToPixelsTransformation transformation, Rectangle drawingRectangle)
    {
        Transformation   = transformation;
        DrawingRectangle = drawingRectangle;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOutside(double x, double y, double sizeX, double sizeY)
    {
        var rect = DrawingRectangle;
        if (x > rect.Right || y > rect.Bottom)
            return true;

        x += sizeX;
        if (x < rect.Left)
            return true;

        y += sizeY;
        if (y < rect.Top)
            return true;
        return false;
    }

    public IDrawingToPixelsTransformation Transformation { get; }

    public Rectangle DrawingRectangle { get; }

}
