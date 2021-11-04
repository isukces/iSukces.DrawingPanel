using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using JetBrains.Annotations;
using Size = System.Drawing.Size;

namespace iSukces.DrawingPanel
{
    public class ZoomInfo : NpcBase
    {
        public void ApplyBounds(Rect bounds, Size size)
        {
            if (bounds.IsEmpty || size.IsEmpty)
                return;

            var scale1 = size.Width / bounds.Width;
            var scale2 = size.Height / bounds.Height;
            Scale  = Math.Min(scale1, scale2);
            Center = new Point(bounds.Left + bounds.Width * 0.5, bounds.Top + bounds.Height * 0.5);
        }

        private const double HullHdDiagonalInPixels = 2202.90717;


        private const double InchMonitorDiameter = 22;
        private const double MaxScale = PixPerMeter * 5 * 2; // rozmiar rzeczywisty dla ekranu 22 cale
        private const double MilimeterMonitorDiameter = InchMonitorDiameter * 25.4;

        private const double MinScale = 1920 / 20_000.0; // 20 km na 1920 pixeli
        private const double PixPerMeter = 1000 * PixPerMm;
        private const double PixPerMm = HullHdDiagonalInPixels / MilimeterMonitorDiameter;

        public Point Center
        {
            get => _center;
            set => SetAndNotify(ref _center, value);
        }

        public double Scale
        {
            get => _scale;
            set
            {
                if (value < MinScale)
                    value = MinScale;
                else if (value > MaxScale)
                    value = MaxScale;
                SetAndNotify(ref _scale, value);
            }
        }

        private Point _center;
        private double _scale = 1;
    }

    public class NpcBase : INotifyPropertyChanged
    {
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetAndNotify<T>(ref T backField, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (Equals(backField, value))
                return false;
            backField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
