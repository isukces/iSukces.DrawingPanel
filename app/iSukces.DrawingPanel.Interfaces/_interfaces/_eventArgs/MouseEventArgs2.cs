using iSukces.Mathematics;

namespace iSukces.DrawingPanel.Interfaces;

public sealed class MouseEventArgs2
{
    public MouseEventArgs2(double delta, Point location, MouseButtons2 button)
    {
        Delta        = delta;
        Location     = location;
        Button = button;
    }

    public double Delta    { get; }
    public Point  Location { get; }
    public MouseButtons2 Button { get; }
}