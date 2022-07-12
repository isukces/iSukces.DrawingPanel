using System.Drawing;
using iSukces.DrawingPanel.Interfaces;
using Point = System.Windows.Point;

namespace iSukces.DrawingPanel
{
    public sealed class DrawableText : DrawableBase, IDrawableWithLayer, IDrawableCollider
    {
        public DrawableText(Layer drawableLayer = Layer.Normal)
        {
            DrawableLayer = drawableLayer;
            _primitive.Changed += (a, b) =>
            {
                OnChanged();
            };
        }

        public override void Draw(Graphics graphics) { _primitive.Draw(graphics, CanvasInfo); }

        public bool IsInside(Point logicPoint, double tolerance)
        {
            // var transformation = _canvasInfo.Transformation;
            //var canvasPoint                   = transformation.ToCanvas(logicPoint);
            //tolerance *= transformation.Scale; 
            return _primitive.IsInside(logicPoint, tolerance);
        }


        public Layer DrawableLayer { get; }

        public string Text
        {
            get => _primitive.Text;
            set => _primitive.Text = value;
        }

        public Point Point
        {
            get => _primitive.Point;
            set => _primitive.Point = value;
        }

        public HorizontalDrawableTextAlignment HorizontalAlignment
        {
            get => _primitive.HorizontalAlignment;
            set => _primitive.HorizontalAlignment = value;
        }

        public VerticalDrawableTextAlignment VerticalAlignment
        {
            get => _primitive.VerticalAlignment;
            set => _primitive.VerticalAlignment = value;
        }

        public double FontSize
        {
            get => _primitive.FontSize;
            set => _primitive.FontSize = value;
        }

        public Brush FontBrush
        {
            get => _primitive.FontBrush;
            set => _primitive.FontBrush = value;
        }

        public object Tag { get; set; }

        public double Angle
        {
            get => _primitive.Angle;
            set => _primitive.Angle = value;
        }

        private readonly LiteDrawableText _primitive = new();
    }
}
