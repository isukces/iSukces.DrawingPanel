namespace iSukces.DrawingPanel.Paths;

public interface ILineCollider
{
    bool IsLineCollision(Point hitPoint, double toleranceSquared, out double distanceSquared,
        out Point correctedPoint);
}
