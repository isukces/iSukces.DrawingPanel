using WinPoint=iSukces.Mathematics.Point;

namespace iSukces.DrawingPanel.Interfaces;

public interface IDrawableWithLayer : IDrawable
{
    Layer DrawableLayer { get; }
}

public enum Layer
{
    Underlay,
    Normal,
    Overlay
}

public interface IDrawableCollider
{
    bool IsInside(WinPoint logicPoint, double tolerance);
}
