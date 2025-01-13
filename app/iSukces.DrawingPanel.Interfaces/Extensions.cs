using System;

namespace iSukces.DrawingPanel.Interfaces;

public static class Extensions
{
    public static IExtendedObservableCollection<IDrawable> Get(this IDrawingLayersContainer layersContainer,
        Layer layer)
    {
        switch (layer)
        {
            case Layer.Underlay:
                return layersContainer.Underlay;
            case Layer.Normal:
                return layersContainer.Drawables;
            case Layer.Overlay:
                return layersContainer.Overlay;
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(layer),
                    $"{layer} is not valid {nameof(Layer)} value");
        }
    }
}
