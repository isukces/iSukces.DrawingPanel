using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using iSukces.Mathematics;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel.Paths
{
    internal static class PathsExtensions
    {
        /*public static double AngleMinusY(this Vector v)
        {
            return Angle(new Vector(v.X, -v.Y));
        }*/
        public static double Angle(this Vector v)
        {
            if (v.X.Equals(0d))
                switch (Math.Sign(v.Y))
                {
                    case 1:
                        return 90d;
                    case -1:
                        return 270d;
                    default:
                        return 0;
                }

            if (v.Y.Equals(0d))
                return v.X < 0 ? 180d : 0d;

            var angle = MathEx.Atan2Deg(v);
            if (angle < 0)
                angle += 360;
            return angle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("DEBUG")]
        public static void CheckNaNDebug(this double value, string argumentName)
        {
            if (double.IsNaN(value))
                throw new ArgumentException(argumentName);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector GetNormalizedVector(this Vector vvvv)
        {
            vvvv.Normalize();
            return vvvv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector GetPrependicularVector(this Vector vector) { return vector.GetPrependicular(false); }

        /*
        public static Vector GetPrependicularVector(this Vector vector, double multiple)
        {
            return new Vector(vector.Y * multiple, -vector.X * multiple);
        }
        */

        public static Vector GetPrependicularVector(this Vector vector, Vector direction)
        {
            var result = new Vector(vector.Y, -vector.X);
            var dot    = result * direction;
            if (dot < 0)
                return -result;
            return result;
        }


        [NotNull]
        public static IPathElement Validate(this ArcDefinition arc, IPathValidator validator,
            in PathRay start, in PathRay end)
        {
            if (arc is null)
                return new InvalidPathElement(start, end, ArcValidationResult.UnableToConstructArc);
            if (validator is null)
            {
                return arc;
            }

            var r = validator.ValidateArc(arc);
            if (r == ArcValidationResult.Ok)
                return arc;
            return new InvalidPathElement(start, end, r);
        }
        
        internal static string Str(this double d)
        {
            return d.ToString(CultureInfo.InvariantCulture);
        }
        
        
        internal static TinyExpr X(this double d)
        {
            return new TinyExpr(d);
        }
    }
    
}
