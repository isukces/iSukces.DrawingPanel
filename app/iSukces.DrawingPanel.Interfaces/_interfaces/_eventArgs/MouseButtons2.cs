// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//
// Modifications Copyright (c) 2017-2026 Internet Sukces Piotr Stęclik.


using System;

namespace iSukces.DrawingPanel.Interfaces;

[Flags]
public enum MouseButtons2
{
    /// <summary>
    ///  The left mouse button was pressed.
    /// </summary>
    Left = 0x00100000,

    /// <summary>
    ///  No mouse button was pressed.
    /// </summary>
    None = 0x00000000,

    /// <summary>
    ///  The right mouse button was pressed.
    /// </summary>
    Right = 0x00200000,

    /// <summary>
    ///  The middle mouse button was pressed.
    /// </summary>
    Middle = 0x00400000,

    XButton1 = 0x00800000,

    XButton2 = 0x01000000,
}
