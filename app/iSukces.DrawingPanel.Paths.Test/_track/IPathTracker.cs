#nullable disable
namespace iSukces.DrawingPanel.Paths.Test;

public interface IPathTracker
{
    double GetLength();
    TrackInfo GetTrackInfo(double x);
}
