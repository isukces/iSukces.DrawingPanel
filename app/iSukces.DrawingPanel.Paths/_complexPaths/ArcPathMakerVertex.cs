using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;

namespace iSukces.DrawingPanel.Paths
{
    public sealed class ArcPathMakerVertex
    {
        public ArcPathMakerVertex() { }

        public ArcPathMakerVertex(Point location) { Location = location; }

        public ArcPathMakerVertex(double x, double y)
            : this(new Point(x, y))
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArcPathMakerVertex WithInVector(double x, double y)
        {
            InVector =  new Vector(x, y);
            Flags    |= FlexiPathMakerItem2Flags.HasInVector;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArcPathMakerVertex WithInVector(double x, double y, double inArmLength)
        {
            InVector    =  new Vector(x, y);
            InArmLength =  inArmLength;
            Flags       |= FlexiPathMakerItem2Flags.HasInVector;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArcPathMakerVertex WithOutVector(double x, double y)
        {
            OutVector =  new Vector(x, y);
            Flags     |= FlexiPathMakerItem2Flags.HasOutVector;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArcPathMakerVertex WithOutVector(double x, double y, double outArmLength)
        {
            OutVector    =  new Vector(x, y);
            OutArmLength =  outArmLength;
            Flags        |= FlexiPathMakerItem2Flags.HasOutVector;
            return this;
        }

        public ArcPathMakerVertex WithRefs(params PathRay[] refs)
        {
            Refs = refs;
            return this;
        }

        public Point Location { get; set; }

        public Vector InVector    { get; set; }
        public double InArmLength { get; set; }

        public Vector OutVector    { get; set; }
        public double OutArmLength { get; set; }

        public FlexiPathMakerItem2Flags Flags { get; set; }
        public IReadOnlyList<PathRay>   Refs  { get; set; }
    }

    [Flags]
    public enum FlexiPathMakerItem2Flags
    {
        None = 0,
        HasInVector = 1,

        /// <summary>
        ///     Wektor wychodzący z kolana w kierunku odbioru jest ustawiony
        /// </summary>
        HasOutVector = 2
    }
}
