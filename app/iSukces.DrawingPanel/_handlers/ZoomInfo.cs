using System;
using iSukces.DrawingPanel.Interfaces;
using Size = System.Drawing.Size;
#if COMPATMATH
using Point=iSukces.Mathematics.Compatibility.Point;
using Rect=iSukces.Mathematics.Compatibility.Rect;
using Vector=iSukces.Mathematics.Compatibility.Vector;
#else
using Point=System.Windows.Point;
using Rect=System.Windows.Rect;
#endif

namespace iSukces.DrawingPanel;

public class ZoomInfo : DpNotifyPropertyChanged
{
    private static double ValidateScale(double value)
    {
        if (value < MinScale)
            value = MinScale;
        else if (value > MaxScale)
            value = MaxScale;
        return value;
    }

    public void ApplyBounds(Rect bounds, Size size)
    {
        if (bounds.IsEmpty || size.IsEmpty)
            return;

        var scale1 = size.Width / bounds.Width;
        var scale2 = size.Height / bounds.Height;
        Scale  = Math.Min(scale1, scale2);
        Center = new Point(bounds.Left + bounds.Width * 0.5, bounds.Top + bounds.Height * 0.5);
    }

    public DrawingPanelZoomStorageData ToStorageData()
    {
        return new DrawingPanelZoomStorageData
        {
            CenterX = Center.X,
            CenterY = Center.Y,
            Scale   = Scale
        };
    }

    public void TryRestore(IDrawingPanelZoomStorage storage)
    {
        if (storage is null || !storage.TryRead(out var data)) return;
        var center = new Point(data.CenterX, data.CenterY);

        var newScale           = ValidateScale(data.Scale);
        var notifyScaleChanged = !newScale.Equals(_scale);
        _scale = data.Scale;
        Center = center;
        if (notifyScaleChanged)
            OnPropertyChanged(nameof(Scale));
    }

    #region properties

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
            value = ValidateScale(value);
            SetAndNotify(ref _scale, value);
        }
    }

    #endregion

    #region Fields

    private const double HullHdDiagonalInPixels = 2202.90717;


    private const double InchMonitorDiameter = 22;
    public static double MaxScale = PixPerMeter * 5 * 2; // rozmiar rzeczywisty dla ekranu 22 cale
    private const double MilimeterMonitorDiameter = InchMonitorDiameter * 25.4;

    public static double MinScale = 1920 / 20_000.0; // 20 km na 1920 pixeli
    private const double PixPerMeter = 1000 * PixPerMm;
    private const double PixPerMm = HullHdDiagonalInPixels / MilimeterMonitorDiameter;

    private Point _center;
    private double _scale = 1;

    #endregion
}