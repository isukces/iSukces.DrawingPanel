using System;

namespace iSukces.DrawingPanel.Interfaces;

public static class Extensions
{
    public static IExtendedObservableCollection<IDrawable> Get(this IDrawingLayersContainer layersContainer,
        Layer layer)
    {
        return layer switch
        {
            Layer.Underlay => layersContainer.Underlay,
            Layer.Normal => layersContainer.Drawables,
            Layer.Overlay => layersContainer.Overlay,
            _ => throw new ArgumentOutOfRangeException(nameof(layer), $"{layer} is not valid {nameof(Layer)} value")
        };
    }
}
