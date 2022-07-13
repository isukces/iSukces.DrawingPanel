using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using iSukces.DrawingPanel;

namespace Sample.Paths
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "Paths demo";
            Size = new Size(1200, 800);

            var dp = new DrawingControl
            {
                Location = new Point(LeftPanelWidth, 0),
                Size     = new Size(ClientSize.Width - LeftPanelWidth, ClientSize.Height),
                Anchor   = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            dp.MainPanel.BackColor = Color.Black;

            var behaviorSource = dp.MainPanel.BehaviorSource;
            behaviorSource.KeyboardFrom   = this;
            behaviorSource.MouseWheelFrom = this;
            Controls.Add(dp);

            const int idx = 4;
            {
                _leftPanel.Location = new Point(0, 0);
                _leftPanel.Size     = new Size(LeftPanelWidth, ClientSize.Height);
                _leftPanel.Anchor   = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
                Controls.Add(_leftPanel);

                CreateButton("1 ref point", () => SetPointsCount(1));
                CreateButton(strMaxArc, () => _controller.MaximumArc());
                CreateButton("2 ref point", () => SetPointsCount(2));
                CreateButton("3 ref point", () => SetPointsCount(3));

                var options = new[]
                {
                    "Start point",
                    "Start vector",
                    "End point",
                    "End vector",
                    "Ref point 1",
                    "Ref point 2",
                    "Ref point 3",
                    "Angle 1",
                    "Angle 2",
                    "Angle 3"
                };

                for (var index = 0; index < options.Length; index++)
                {
                    var cb = new RadioButton
                    {
                        Checked  = index == idx,
                        Text     = options[index],
                        AutoSize = true,
                        Tag      = index
                    };
                    cb.CheckedChanged += CheckBoxCheckedChanged;
                    _leftPanel.Controls.Add(cb);
                }
            }
            _controller = new PathsController();
            dp.BehaviorContainer.RegisterHandler(_controller, DrawingHandlerOrders.ElementEditor);
            dp.MainPanel.Drawables.Add(_controller);
            _controller.OptionIndex =  idx;
            VisibleChanged          += OnVisibleChanged;
        }

        private void ArrangeLeftPanel()
        {
            var y = 50;
            foreach (Control c in _leftPanel.Controls)
            {
                if (!c.Visible) continue;
                c.Location =  new Point(10, y);
                y          += c.Height + 5;
            }
        }

        private void CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rb)
            {
                if (rb.Tag is int index)
                {
                    _controller.OptionIndex = index;
                }
            }
        }

        private void CreateButton(string text, Action click)
        {
            var btn = new Button
            {
                Text = text,
            };
            btn.Click += (_, a) =>
            {
                click();
            };
            _leftPanel.Controls.Add(btn);
        }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (!Visible || Disposing)
                return;
            VisibleChanged -= OnVisibleChanged;
            SetPointsCount(1);
        }

        private void SetPointsCount(int nr)
        {
            _controller.SetPointsCount(nr);

            foreach (Control c in _leftPanel.Controls)
            {
                var m = EndsWithRegex.Match(c.Text);
                if (m.Success)
                {
                    var nr1 = int.Parse(m.Groups[1].Value);
                    c.Visible = nr1 <= nr;
                }

                if (c.Text == strMaxArc)
                    c.Visible = nr == 1;
            }

            ArrangeLeftPanel();
        }

        private const string strMaxArc = "Max arc";

        private const string EndsWithFilter = @"(\d+)$";

        private const int LeftPanelWidth = 130;

        private static readonly Regex EndsWithRegex =
            new Regex(EndsWithFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly PathsController _controller;
        private readonly Panel _leftPanel = new();
    }
}
