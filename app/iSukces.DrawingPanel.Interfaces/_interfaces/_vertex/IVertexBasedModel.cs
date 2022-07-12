namespace iSukces.DrawingPanel.Interfaces
{
    /// <summary>
    /// A model interface that relies on vertexes.
    /// The VertexDragDeltaSuspenResume method allows you to notify  the model to start handling the shifting of a vertex or vertexes.
    /// I suggest that there is simply a rendering freeze in the middle,
    /// as if more than one vertex is changed it may unnecessarily redraw multiple times.
    /// </summary>
    public interface IVertexBasedModel
    {
        void VertexDragDeltaSuspenResume(StartOrStop action);
    }
}
