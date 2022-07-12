using System.Drawing;

namespace iSukces.DrawingPanel
{
    public sealed class ThumbColors
    {
        public ThumbColors(Color selectedFill, Color fill, Color selectedPen, Color pen)
        {
            SelectedFill = selectedFill;
            Fill         = fill;
            SelectedPen  = selectedPen;
            Pen          = pen;
        }

        public Color GetFill(bool isSelected)
        {
            var color = isSelected ? SelectedFill : Fill;
            return color;
        }

        public Color GetPen(bool isSelected)
        {
            var color = isSelected ? SelectedPen : Pen;
            return color;
        }

        public Color SelectedFill { get; }
        public Color Fill         { get; }
        public Color SelectedPen  { get; }
        public Color Pen          { get; }

        public static ThumbColors DefaultColors = new ThumbColors(
            Color.Yellow,
            Color.FromArgb(0, 127, 255),
            Color.LightPink,
            Color.FromArgb(111, 111, 111)
        );


        public static ThumbColors RotateColors = new ThumbColors(
            Color.FromArgb(255, 216, 178),
            Color.FromArgb(255, 127, 0),
            Color.LightPink,
            Color.FromArgb(111, 111, 111)
        );
    }
}
