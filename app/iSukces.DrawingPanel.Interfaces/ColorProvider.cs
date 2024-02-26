using System;
using System.Drawing;

namespace iSukces.DrawingPanel.Interfaces;

public interface IDrawingColorScheme
{
    event EventHandler ColorSchemeChanged;
    Color              Background { get; }
    Color              Pen        { get; }
    Color              Gray       { get; }

    bool IsDark { get; }
}