#define _LOG
using System;
using System.Drawing;
using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;
using Point = System.Windows.Point;

namespace iSukces.DrawingPanel
{
    public class DrawableThumb : IDrawableWithLayer
    {
        public void Draw(Graphics graphics)
        {
            var point = _canvasInfo.Transformation.ToCanvas(Center);
            var x     = point.X - SizeHalf;
            var y     = point.Y - SizeHalf;
            if (_canvasInfo.IsOutside(x, y, Size, Size))
                return;

            var colorSettings = Colors ?? ThumbColors.DefaultColors;

            var   color = colorSettings.GetFill(IsSelected);
            Brush brush = new SolidBrush(color);
            graphics.FillRectangle(brush, (float)x, (float)y, Size, Size);

            color = colorSettings.GetPen(IsSelected);
            using var pen = new Pen(color, 1);
            graphics.DrawRectangle(pen, (float)x, (float)y, Size, Size);
        }

        public Point GetScreenLocation()
        {
            var point = _canvasInfo.Transformation.ToCanvas(Center);
            return point;
        }

        /*public bool IsInside(Point p)
        {
            var a = _canvasInfo.Transformation.ToCanvas(p);
            var b = _canvasInfo.Transformation.ToCanvas(Center);
            return Math.Abs(a.X - b.X) <= SizeHalf && Math.Abs(a.Y - b.Y) <= SizeHalf;
        }*/

        public bool IsInside(System.Drawing.Point a)
        {
            if (_canvasInfo is null)
                return false; // generalnie to program się wywalił na rzadko
            var b = _canvasInfo.Transformation.ToCanvas(Center);
            return Math.Abs(a.X - b.X) <= SizeHalf && Math.Abs(a.Y - b.Y) <= SizeHalf;
        }


        private void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void SetCanvasInfo(DrawingCanvasInfo canvasInfo)
        {
            _canvasInfo = canvasInfo;
        }

        #region properties

        public Point Center
        {
            get => _center;
            set
            {
                if (Equals(_center, value)) return;
                _center = value;
                OnChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;
                _isSelected = value;
                IsSelectedChanged?.Invoke(this, EventArgs.Empty);
                OnChanged();
            }
        }

        public object      DataContext            { get; set; }
        public int         Index                  { get; set; }
        public bool        DisableDraggingByMouse => (Flags & ThumbFlags.Fixed) != 0;
        public Cursor      Cursor                 { get; set; }
        public ThumbFlags  Flags                  { get; set; }
        public ThumbColors Colors                 { get; set; }

        /// <summary>
        ///     Used only when dragging.
        /// </summary>
        public ThumbDraggingContext DraggingContext { get; set; }

        #endregion

        public event EventHandler Changed;
        public bool               Visible                => true;
        public bool               PresenterRenderingFlag { get; set; }
        public Layer              DrawableLayer          => Layer.Overlay;


        public event EventHandler IsSelectedChanged;

        #region Fields

        private const float Size = 9;
        private const float SizeHalf = Size / 2;
        private DrawingCanvasInfo _canvasInfo;
        private Point _center;
        private bool _isSelected;

        #endregion
    }
}
