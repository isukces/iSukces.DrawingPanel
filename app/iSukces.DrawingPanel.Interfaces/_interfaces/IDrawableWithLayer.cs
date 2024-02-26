using System.Windows;
#if COMPATMATH
using WinPoint=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using WinPoint=System.Windows.Point;
using Vector=System.Windows.Vector;
#endif

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