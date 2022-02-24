using System.Windows;

namespace iSukces.DrawingPanel.Interfaces
{
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
        bool IsInside(Point logicPoint, double tolerance);
    }
}
