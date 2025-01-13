#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace iSukces.DrawingPanel;

internal sealed class BoundsUpdater
{
    public BoundsUpdater(Control control)
    {
        _control = control;
        X        = control.Left;
        Y        = control.Top;
        Width    = control.Width;
        Height   = control.Height;
    }

    private BoundsSpecified GetItemsToChange(out Size expectedSize)
    {
        var expectedWidth  = Width;
        var expectedHeight = Height;
        {
            var limit = _control.MinimumSize;
            if (expectedWidth < limit.Width)
                expectedWidth = limit.Width;
            if (expectedHeight < limit.Height)
                expectedHeight = limit.Height;
        }
        {
            var limit = _control.MaximumSize;
            if (limit.Width > 0 && expectedWidth > limit.Width)
                expectedWidth = limit.Width;
            if (limit.Height > 0 && expectedHeight > limit.Height)
                expectedHeight = limit.Height;
        }

        expectedSize = new Size(expectedWidth, expectedHeight);
        var location  = _control.Location;
        var specified = BoundsSpecified.None;
        if (location.X != X)
            specified |= BoundsSpecified.X;
        if (location.Y != Y)
            specified |= BoundsSpecified.Y;

        var size = _control.Size;
        if (size.Width != expectedSize.Width)
            specified |= BoundsSpecified.Width;
        if (size.Height != expectedSize.Height)
            specified |= BoundsSpecified.Height;
        return specified;
    }

    public override string ToString()
    {
        return $"Update {_control} ";
    }

    public void Update()
    {
        var specified = GetItemsToChange(out var expectedSize);

        if (specified == BoundsSpecified.None)
            return;

        _control.SetBounds(X, Y, expectedSize.Width, expectedSize.Height, specified);
    }

    #region properties

    public int X      { get; set; }
    public int Y      { get; set; }
    public int Width  { get; set; }
    public int Height { get; set; }

    #endregion

    #region Fields

    private readonly Control _control;

    #endregion
}
