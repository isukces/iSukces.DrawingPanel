#nullable disable
using System;
using System.Drawing;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel;

internal sealed class DrawingColorScheme : IDrawingColorScheme
{
    private static bool IsDarkColor(Color c)
    {
        const int threshHold = 127 * 3;
        var       all        = c.R + c.G + c.B;
        return all < threshHold;
    }

    private static byte MiddleByte(byte start, byte end, double factor)
    {
        if (factor <= 0)
            return start;
        if (factor >= 1)
            return end;
        var c = start * (1 - factor) + end * factor;
        return (byte)Math.Round(c);
    }

    private static Color MiddleColor(Color start, Color end, double factor)
    {
        var r = MiddleByte(start.R, end.R, factor);
        var g = MiddleByte(start.G, end.G, factor);
        var b = MiddleByte(start.B, end.B, factor);
        return Color.FromArgb(r, g, b);
    }

    public void Update(Color backColor)
    {
        Background = backColor;
        IsDark     = IsDarkColor(backColor);
        if (IsDark)
        {
            Pen = Color.White;
        }
        else
        {
            Pen = Color.Black;
        }

        Gray = MiddleColor(backColor, Pen, 50);

        ColorSchemeChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler ColorSchemeChanged;

    public Color Background { get; private set; }
    public Color Pen        { get; private set; }
    public Color Gray       { get; private set; }
    public bool  IsDark     { get; private set; }
}
