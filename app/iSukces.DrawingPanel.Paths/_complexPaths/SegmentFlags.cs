using System;

namespace iSukces.DrawingPanel.Paths
{
    [Flags]
    public enum SegmentFlags
    {
        None = 0,
        HasStartVector = 1,
        HasEndVector = 2,
        HasReferencePoints = 4,


        BothVectors = HasStartVector | HasEndVector
    }
}
