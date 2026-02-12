namespace iSukces.DrawingPanel.Interfaces;

public sealed class DpKeyEventArgs
{
    public DpKeyEventArgs(bool alt, bool control, string keyCode)
    {
        Alt     = alt;
        Control = control;
        KeyCode = keyCode;
    }

    public bool   Alt     { get; }
    public bool   Control { get; }
    public string KeyCode { get; }

    public bool IsDelete => KeyCode == "Delete";
    public bool IsBack   => KeyCode == "Back";
    public bool IsEscape => KeyCode == "Escape";
}
