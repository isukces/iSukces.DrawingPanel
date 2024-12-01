using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.ComponentModel;

namespace iSukces.DrawingPanel;

/// <summary>
///     A ruler control which displays ruler in pixels.
///     In order to use it vertically, change the <see cref="AxisLocation">Marks</see> property to <c>Up</c> and rotate it
///     ninety
///     degrees.
/// </summary>
/// <remarks>
///     Rewritten by: Sebestyen Murancsik
///     Contributions from Raf Lenfers
///     <seealso>http://visualizationtools.net/default/wpf-ruler/</seealso>
/// </remarks>
public sealed partial class Ruler : Control
{
    public Ruler()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    }

    public static bool IsHorizontalRuller(AxisLocation axisLocation)
    {
        return axisLocation is AxisLocation.Up or AxisLocation.Down;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PointF NewPointF(double x, double y) { return new PointF((float)x, (float)y); }

    protected override void OnClientSizeChanged(EventArgs e)
    {
        base.OnClientSizeChanged(e);

        Invalidate();
    }


    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var graphics = e.Graphics;

        var axisLocation      = AxisLocation;
        var isHorizontalRuler = IsHorizontalRuller(axisLocation);

        PointF MkPoint(double x, double y)
        {
            return isHorizontalRuler ? new PointF((float)x, (float)y) : new PointF((float)y, (float)x);
        }

        var dim = Dimension;

        var rullerWidth  = isHorizontalRuler ? ClientSize.Height : ClientSize.Width;
        var rullerLength = Length;

        var lengthTimesZoom = rullerLength * Dimension.Scale;

        var majorTickLength = 12; //rullerWidth - tickMarginForText;

        var minorCount    = Math.Max(1, dim.MinorCount);
        var dimCalculator = dim.GetCalculator();
        var range         = dimCalculator.GetTickRange(0, lengthTimesZoom);

        if (range is null)
            return;
        {
            // _textManager.FontName  = FontFamily?.Source;
            _textManager.TextBrush = _textBrush;
            var textHeight =
                _textManager.TextHeight; //axisLocation == AxisLocation.Left ? -1 : CalcTextHeight(typeFace);
            var c = new ValueToLabelConverter(dim.Major);
            for (var nr = range.Minimum; nr <= range.Maximum; nr++)
            {
                var value = dimCalculator.GetValue(nr);
                var x     = MathUtils.Round05(value.DrawPosition);
                if (x > lengthTimesZoom || x < 0) continue;
                var    isBig = minorCount < 2 || nr % minorCount == 0;
                double startHeight, endHeight;
                if (axisLocation == AxisLocation.Up || axisLocation == AxisLocation.Left)
                {
                    startHeight = 0;
                    endHeight   = isBig ? majorTickLength : majorTickLength / 2;
                }
                else
                {
                    startHeight = rullerWidth;
                    endHeight   = isBig ? majorTickLength : majorTickLength * 0.5;
                    endHeight   = rullerWidth - endHeight;
                }

                graphics.DrawLine(isBig ? _thinPen : _thinPen2,
                    MkPoint(x, startHeight), MkPoint(x, endHeight));

                if (!isBig) continue;
                PointF p;
                switch (axisLocation)
                {
                    case AxisLocation.Up:
                        p = NewPointF(x, rullerWidth - textHeight);
                        break;
                    case AxisLocation.Down:
                        p = NewPointF(x, rullerWidth - majorTickLength - textHeight - 1);
                        break;
                    case AxisLocation.Left:
                        p = NewPointF(tickMarginForText, x);
                        break;
                    case AxisLocation.Right:
                        p = NewPointF(rullerWidth - majorTickLength - textHeight / 2 - 3, x);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var savedTransform = graphics.Transform;
                try
                {
                    var text = c.ValueToText(value);
                    var m    = graphics.MeasureString(text, Font);
                    p = new PointF(p.X - m.Width * 0.5f, p.Y);

                    if (isHorizontalRuler)
                        p = new PointF(p.X - m.Width * 0.5f, p.Y);
                    else
                        p = new PointF(Size.Width - m.Width, p.Y - m.Height - 1);
                    using var stringFormat = new StringFormat();
                    graphics.DrawString(text, Font, Brushes.Black, p, stringFormat);
                }
                finally
                {
                    graphics.Transform = savedTransform;
                    savedTransform.Dispose();
                }
            }
        }
    }

    private void SetAndInvalidate<T>(ref T field, T value)
    {
        if (field.Equals(value))
            return;
        field = value;
        Invalidate();
    }


    private const int tickMarginForText = 13;

    /// <summary>
    ///     Gets or sets where the marks are shown in the ruler.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AxisLocation AxisLocation
    {
        get => _axisLocation;
        set => SetAndInvalidate(ref _axisLocation, value);
    }

    /// <summary>
    ///     Gets or sets the zoom factor for the ruler. The default value is 1.0.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public RulerDimension Dimension
    {
        get => _dimenison;
        set => SetAndInvalidate(ref _dimenison, value);
    }


    /*[Localizability(LocalizationCategory.Font)]
    public FontFamily FontFamily
    {
        get { return (FontFamily)GetValue(FontFamilyProperty); }
        set
        {
            SetValue(FontFamilyProperty, value);
            InvalidateVisual();
        }
    }
    */


    /// <summary>
    ///     Gets or sets the length of the ruler. If the <see cref="RulerAutoSize" /> property is set to false (default) this
    ///     is a fixed length. Otherwise the length is calculated based on the actual width of the ruler.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public double Length
    {
        get => RulerAutoSize
            ? (IsHorizontalRuller(AxisLocation) ? ClientSize.Width : ClientSize.Height) / Dimension.Scale
            : _length;
        set => SetAndInvalidate(ref _length, value);
    }

    /// <summary>
    ///     Gets or sets the AutoSize behavior of the ruler.
    ///     false (default): the lenght of the ruler results from the <see cref="Length" /> property. If the window size is
    ///     changed, e.g. wider
    ///     than the rulers length, free space is shown at the end of the ruler. No rescaling is done.
    ///     true				 : the length of the ruler is always adjusted to its actual width. This ensures that the ruler is shown
    ///     for the actual width of the window.
    /// </summary>
    /// <summary>
    ///     Gets or sets the AutoSize behavior of the ruler.
    ///     false (default): the lenght of the ruler results from the <see cref="Length" /> property. If the window size is
    ///     changed, e.g. wider
    ///     than the rulers length, free space is shown at the end of the ruler. No rescaling is done.
    ///     true				 : the length of the ruler is always adjusted to its actual width. This ensures that the ruler is shown
    ///     for the actual width of the window.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool RulerAutoSize
    {
        get => _rulerAutoSize;
        set => SetAndInvalidate(ref _rulerAutoSize, value);
    }

    private readonly Brush _textBrush = Brushes.DimGray;

    private readonly RullerTextManager _textManager = new RullerTextManager();

    private readonly Pen _thinPen = new Pen(Color.Black, 1);
    private readonly Pen _thinPen2 = new Pen(Color.FromArgb(100, Color.Black), 1);
    private AxisLocation _axisLocation = AxisLocation.Up;
    private RulerDimension _dimenison;
    private double _length = 20;
    private bool _rulerAutoSize;
}