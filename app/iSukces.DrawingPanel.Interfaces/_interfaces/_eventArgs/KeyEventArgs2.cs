namespace iSukces.DrawingPanel.Interfaces;

public sealed class KeyEventArgs2
{
    public KeyEventArgs2(bool alt, bool control)
    {
        Alt     = alt;
        Control = control;
    }

    public bool Alt     { get; }
    public bool Control { get; }
}
