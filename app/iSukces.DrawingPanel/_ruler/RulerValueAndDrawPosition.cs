namespace iSukces.DrawingPanel
{
    public struct RulerValueAndDrawPosition
    {
        public RulerValueAndDrawPosition(double displayValue, double drawPosition)
        {
            DisplayValue = displayValue;
            DrawPosition = drawPosition;
        }

        public override string ToString()
        {
            return $"DisplayValue={DisplayValue}, DrawPosition={DrawPosition}";
        }

        public double DisplayValue { get; }
        public double DrawPosition { get; }
    }
}