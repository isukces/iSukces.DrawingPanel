#nullable enable
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel;

public sealed class LiteEllipse : ILiteDrawable
{
    public void Draw(Graphics graphics, DrawingCanvasInfo canvasInfo)
    {
        var r = new DoubleRectangle();
        r.SetXFromCenterAndLength(XCenter, Width);
        r.SetYFromCenterAndLength(YCenter, Height);
        var rect = r.Transform(canvasInfo.Transformation);
        graphics.FillEllipse(Brush, rect);
        graphics.DrawEllipse(new Pen(Stroke, 1), rect);
    }

    public double Width   { get; set; }
    public double Height  { get; set; }
    public double XCenter { get; set; }
    public double YCenter { get; set; }
    public Brush  Brush   { get; set; }
    public Color  Stroke  { get; set; }
}
