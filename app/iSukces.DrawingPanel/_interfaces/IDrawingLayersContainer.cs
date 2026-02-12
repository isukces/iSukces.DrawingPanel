using System.ComponentModel;

namespace iSukces.DrawingPanel.Interfaces;

public interface IDrawingLayersContainer : IDrawingCanvasInfoProvider
{
    IExtendedObservableCollection<IDrawable> Underlay  { get; }
    IExtendedObservableCollection<IDrawable> Drawables { get; }
    IExtendedObservableCollection<IDrawable> Overlay   { get; }
}

public interface IInitializeableDrawingLayersContainer : IDrawingLayersContainer,
    ISupportInitialize
{
}