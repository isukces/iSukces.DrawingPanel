using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using iSukces.DrawingPanel.Interfaces;
#if COMPATMATH
using WinPoint=iSukces.Mathematics.Compatibility.Point;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using WinPoint=iSukces.Mathematics.Point;
using Vector=iSukces.Mathematics.Vector;
#endif

namespace iSukces.DrawingPanel;

/// <summary>
///     Drawing panel with rullers
/// </summary>
public partial class DrawingControl : UserControl
{
    public DrawingControl()
    {
        InitializeComponent();

        MainPanel = new DrawableContainerControl
        {
            Name   = "drawingPanel",
            Anchor = AnchorStyles.None
        };
        _horizontalRuler = new Ruler
        {
            Name          = "horizontalRuller",
            RulerAutoSize = true,
            Anchor        = AnchorStyles.None,
            AxisLocation  = AxisLocation.Down
        };
        _verticalRuler = new Ruler
        {
            Name          = "verticalRuller",
            RulerAutoSize = true,
            Anchor        = AnchorStyles.None,
            AxisLocation  = AxisLocation.Right
        };

        SuspendLayout();
        Controls.Add(MainPanel);
        Controls.Add(_horizontalRuler);
        Controls.Add(_verticalRuler);
        ResumeLayout();
        MainPanel.DrawingTranformChanged += UpdateRulers;
        UpdateRulers(null, null);
    }


    public static void SetupRuller(ref RulerDimension dim, double scale,
        TickConfigutation tick, double offset, double displayValueOffset, bool isReversed)
    {
        dim.Scale              = scale;
        dim.Major              = tick.Major;
        dim.MinorCount         = tick.MinorCount;
        dim.Offset             = offset;
        dim.DisplayValueOffset = displayValueOffset;
        dim.IsReverse          = isReversed;
    }

    private void UpdateRulers(object? sender, EventArgs e)
    {
        var displayValueOffset = new Point(0, 0);
        // _mouseCapturable.DisplayValueOffset = displayValueOffset;
        if (_underUpdateRenderTransform)
            return;
        try
        {
            var drawingCanvasInfo = MainPanel.CanvasInfo;
            if (drawingCanvasInfo is null)
                return;

            var transformation = drawingCanvasInfo.Transformation;
            var scale          = transformation.Scale;
            var mm             = TickConfigutation.FindForScale(scale);

            var topLeftLogical = transformation.FromCanvas(new WinPoint(0, 0));
            {
                var dim = _horizontalRuler.Dimension;
                SetupRuller(ref dim, scale, mm,
                    -topLeftLogical.X,
                    displayValueOffset.X, false);
                _horizontalRuler.Dimension = dim;
            }
            {
                var dim = _verticalRuler.Dimension;
                SetupRuller(ref dim, scale, mm,
                    -topLeftLogical.Y,
                    displayValueOffset.Y, true);
                _verticalRuler.Dimension = dim;
            }

            _horizontalRuler.Visible = _verticalRuler.Visible = true;
        }
        catch
        {
            _horizontalRuler.Visible = _verticalRuler.Visible = false;
        }
    }

    public override LayoutEngine LayoutEngine
    {
        get
        {
            if (_layoutEngine is null)
            {
                _layoutEngine = new DrawingControlLayoutEngine(this);
            }

            return _layoutEngine;
        }
    }

    public DrawableContainerControl MainPanel { get; }

    public IDpHandlerContainer BehaviorContainer => MainPanel.BehaviorContainer;

    const int rullerLeft = 65;
    const int rullerTop = 25;


    private readonly Ruler _horizontalRuler;

    private readonly Ruler _verticalRuler;

    private sealed class DrawingControlLayoutEngine : LayoutEngine
    {
        public DrawingControlLayoutEngine(DrawingControl owner)
        {
            _owner = owner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Setup(Control control, int x,
            int y, int width,
            int height)
        {
            if (control is null) return;
            var c = new BoundsUpdater(control)
            {
                Width  = width,
                Height = height,
                X      = x,
                Y      = y
            };
            c.Update();
        }

        public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
        {
            // return base.Layout(container, layoutEventArgs);
            var cs       = _owner.ClientSize;
            var csWidth  = cs.Width - rullerLeft;
            var csHeight = cs.Height - rullerTop;

            Setup(_owner.MainPanel, rullerLeft, rullerTop, csWidth, csHeight);
            Setup(_owner._horizontalRuler, rullerLeft, 0, csWidth, rullerTop);
            Setup(_owner._verticalRuler, 0, rullerTop, rullerLeft, csHeight);

            return false;
        }

        private readonly DrawingControl _owner;
    }
#pragma warning disable 649
    private bool _underUpdateRenderTransform;
    private DrawingControlLayoutEngine _layoutEngine;
#pragma warning restore 649
}

