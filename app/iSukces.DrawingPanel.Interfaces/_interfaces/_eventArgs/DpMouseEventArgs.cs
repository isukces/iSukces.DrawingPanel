using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Interfaces;

public sealed class DpMouseEventArgs
{
    public DpMouseEventArgs(double delta, Point location, DpMouseButtons button)
    {
        Delta        = delta;
        Location     = location;
        Button = button;
    }

    public double Delta    { get; }
    public Point  Location { get; }
    public DpMouseButtons Button { get; }
}