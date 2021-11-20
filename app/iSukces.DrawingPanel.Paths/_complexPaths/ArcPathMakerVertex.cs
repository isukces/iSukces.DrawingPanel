using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if NET5_0
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


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

        private static IReadOnlyList<PathRay> DeepClone(IReadOnlyList<PathRay> refs)
        {
            if (refs is null)
                return null;
            if (refs.Count == 0)
                return Array.Empty<PathRay>();
            var result = new PathRay[refs.Count];
            switch (refs)
            {
                case PathRay[] refsArray:
                    Array.Copy(refsArray, result, refsArray.Length);
                    break;
                case ICollection<PathRay> collection: 
                    collection.CopyTo(result, 0);
                    break;
                default:
                {
                    for (var i = 0; i < result.Length; i++)
                        result[i] = refs[i];
                    break;
                }
            }
            return result;
        }

        public ArcPathMakerVertex DeepClone()
        {
            var vertex = new ArcPathMakerVertex
            {
                _inVector       = _inVector,
                _outVector      = _outVector,
                InArmLength     = InArmLength,
                OutArmLength    = OutArmLength,
                Location        = Location,
                Flags           = Flags,
                ReferencePoints = DeepClone(ReferencePoints)
            };
            return vertex;
        }

        public void NormalizeInVector()
        {
            if ((Flags & FlexiPathMakerItem2Flags.HasInVector) != 0)
                _inVector.Normalize();
        }

        public void NormalizeOutVector()
        {
            if ((Flags & FlexiPathMakerItem2Flags.HasOutVector) != 0)
                _outVector.Normalize();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArcPathMakerVertex WithInVector(double x, double y) { return WithInVector(new Vector(x, y)); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArcPathMakerVertex WithInVector(Vector inVector)
        {
            _inVector = inVector;
            if (inVector.IsZero())
                Flags &= ~FlexiPathMakerItem2Flags.HasInVector;
            else
                Flags |= FlexiPathMakerItem2Flags.HasInVector;
            Flags |= FlexiPathMakerItem2Flags.HasInVector;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArcPathMakerVertex WithInVector(double x, double y, double inArmLength)
        {
            InArmLength = inArmLength;
            return WithInVector(new Vector(x, y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArcPathMakerVertex WithOutVector(Vector outVector)
        {
            _outVector = outVector;
            if (outVector.IsZero())
                Flags &= ~FlexiPathMakerItem2Flags.HasOutVector;
            else
                Flags |= FlexiPathMakerItem2Flags.HasOutVector;
            return this;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArcPathMakerVertex WithOutVector(double x, double y) { return WithOutVector(new Vector(x, y)); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArcPathMakerVertex WithOutVector(double x, double y, double outArmLength)
        {
            OutArmLength = outArmLength;
            return WithOutVector(new Vector(x, y));
        }

        public ArcPathMakerVertex WithReferencePoints(params PathRay[] refs)
        {
            ReferencePoints = refs;
            return this;
        }

        public Point Location { get; set; }

        public Vector InVector => _inVector;

        public double InArmLength { get; set; }

        public Vector OutVector => _outVector;

        public double OutArmLength { get; set; }

        public FlexiPathMakerItem2Flags Flags           { get; private set; }
        public IReadOnlyList<PathRay>   ReferencePoints { get; set; }
        private Vector _inVector;
        private Vector _outVector;
    }

    [Flags]
    public enum FlexiPathMakerItem2Flags
    {
        None = 0,
        HasInVector = 1,

        /// <summary>
        ///     Wektor wychodzący z kolana w kierunku odbioru jest ustawiony
        /// </summary>
        HasOutVector = 2,

        HasBothVectors = HasInVector | HasOutVector,
    }
}
