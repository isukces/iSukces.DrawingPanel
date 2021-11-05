using System;
using System.Drawing;
using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel
{
    public partial class DrawingControl : UserControl
    {
        public DrawingControl()
        {
            InitializeComponent();

            const int rullerLeft = 65;
            const int rullerTop  = 25;
            Panel = new DrawingLayersContainer
            {
                Name     = "drawingPanel",
                Location = new Point(rullerLeft, rullerTop),
                Size     = new Size(ClientSize.Width - rullerLeft, ClientSize.Height - rullerTop),
                Anchor   = (AnchorStyles)15
            };
            _horizontalRuler = new Ruler
            {
                Name          = "horizontalRuller",
                RulerAutoSize = true,
                Location      = new Point(rullerLeft, 0),
                Size          = new Size(Panel.Size.Width, rullerTop),
                Anchor        = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                AxisLocation  = AxisLocation.Down
            };
            _verticalRuler = new Ruler
            {
                Name          = "verticalRuller",
                RulerAutoSize = true,
                Location      = new Point(0, rullerTop),
                Size          = new Size(rullerLeft, Panel.Size.Height),
                Anchor        = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top,
                AxisLocation  = AxisLocation.Right
            };

            Controls.Add(Panel);
            Controls.Add(_horizontalRuler);
            Controls.Add(_verticalRuler);
            Panel.DrawingTranformChanged += UpdateRulers;
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

        private void UpdateRulers(object sender, EventArgs e)
        {
            var displayValueOffset = new Point(0, 0);
            // _mouseCapturable.DisplayValueOffset = displayValueOffset;
            if (_underUpdateRenderTransform)
                return;
            try
            {
                var transformation = Panel.CanvasInfo.Transformation;
                var scale          = transformation.Scale;
                var mm             = TickConfigutation.FindForScale(scale);

                var topLeftLogical = transformation.FromCanvas(new System.Windows.Point(0, 0));
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
                {
                    /*AfterRullerUpdate?.Invoke(this, new AfterRullerUpdateEventArgs
                    {
                        HorizontalRulerDimension = _horizontalRuler.Dimension,
                        VerticalRulerDimension   = _verticalRuler.Dimension
                    });*/
                }
            }
            catch
            {
                _horizontalRuler.Visible = _verticalRuler.Visible = false;
            }
        }

        public DrawingLayersContainer Panel { get; }

        public IDpHandlerContainer RootBehaviorContainer => Panel.RootBehaviorContainer;


        private readonly Ruler _horizontalRuler;

        private readonly Ruler _verticalRuler;
#pragma warning disable 649
        private bool _underUpdateRenderTransform;
#pragma warning restore 649
    }
}
