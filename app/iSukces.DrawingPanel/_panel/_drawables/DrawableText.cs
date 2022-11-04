﻿using System.Drawing;
using iSukces.DrawingPanel.Interfaces;
using Point = System.Windows.Point;

namespace iSukces.DrawingPanel
{
    public class DrawableText : DrawableBase, IDrawableWithLayer, IDrawableCollider
    {
        public DrawableText(Layer drawableLayer = Layer.Normal)
        {
            DrawableLayer = drawableLayer;
            _primitive    = new LiteDrawableText();
            _primitive.Changed += (a, b) =>
            {
                OnChanged();
            };
        }
        
        protected DrawableText(LiteDrawableText primitive, Layer drawableLayer = Layer.Normal)
        {
            DrawableLayer = drawableLayer;
            _primitive    = primitive ?? new LiteDrawableText();
            _primitive.Changed += (a, b) =>
            {
                OnChanged();
            };
        }


        public override void Draw(Graphics graphics)
        {
            _primitive.Draw(graphics, CanvasInfo);
        }

        public bool IsInside(Point logicPoint, double tolerance)
        {
            return _primitive.IsInside(logicPoint, tolerance);
        }

        #region properties

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

        #endregion


        public Layer DrawableLayer { get; }

        #region Fields

        private readonly LiteDrawableText _primitive;

        #endregion
    }
}
