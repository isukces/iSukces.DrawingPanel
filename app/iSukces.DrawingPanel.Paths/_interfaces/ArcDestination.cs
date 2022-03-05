using System;

namespace iSukces.DrawingPanel.Paths
{
    /// <summary>
    ///     Helps IPathValidator to make better decisions
    /// </summary>
    public struct ArcDestination : IEquatable<ArcDestination>
    {
        private readonly int _value;

        private ArcDestination(int value) => _value = value;

        public bool Equals(ArcDestination other) => _value == other._value;

        public override bool Equals(object obj) => obj is ArcDestination other && Equals(other);

        public override int GetHashCode() => _value;

        public static bool operator ==(ArcDestination left, ArcDestination right) => left.Equals(right);

        public static bool operator !=(ArcDestination left, ArcDestination right) => !left.Equals(right);

        private const int num_Unknown = 0;
        private const int num_ZeroReferenceOneArc = 1;
        private const int num_ZeroReferenceTwoArcs = 2;
        private const int num_OneReferenceTwoArcs = 12;
        
        private const int num_AppendArcToList = 99;

        public override string ToString()
        {
            return _value switch
            {
                num_Unknown => nameof(Unknown),
                num_ZeroReferenceOneArc => nameof(ZeroReferenceOneArc),
                num_ZeroReferenceTwoArcs => nameof(ZeroReferenceTwoArcs),
                num_OneReferenceTwoArcs => nameof(OneReferenceTwoArcs),
                num_AppendArcToList => nameof(AppendArcToList),
                _ => "???"
            };
        }

        public bool IsOne => _value == num_ZeroReferenceOneArc;

        public bool IsPair => _value is num_ZeroReferenceTwoArcs or num_OneReferenceTwoArcs;

        public static ArcDestination Unknown = new(num_Unknown);
        public static ArcDestination ZeroReferenceOneArc = new(num_ZeroReferenceOneArc);
        public static ArcDestination ZeroReferenceTwoArcs = new(num_ZeroReferenceTwoArcs);
        public static ArcDestination OneReferenceTwoArcs = new(num_OneReferenceTwoArcs);
        public static ArcDestination AppendArcToList = new(num_AppendArcToList);
    }
}
