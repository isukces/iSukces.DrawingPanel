using System;
using System.Runtime.CompilerServices;
using iSukces.Mathematics;
using Newtonsoft.Json;
#if COMPATMATH
using iSukces.Mathematics.Compatibility;

#else
using System.Windows;
#endif

namespace iSukces.DrawingPanel.Paths
{
    public struct WayPoint
    {
        private WayPoint(WayPoint a, Vector v)
        {
            Point           = a.Point + v;
            Vector          = a.Vector;
            UseInputVector  = a.UseInputVector;
            InputVector     = a.InputVector;
            InputArmLength  = a.InputArmLength;
            OutputArmLength = a.OutputArmLength;
            Tag             = a.Tag;
        }

        public WayPoint(PathRay ray, bool useInputVector, Vector inputVector, double inputArmLength,
            double outputArmLength)
        {
            switch (inputArmLength)
            {
                case < 0: throw new ArgumentOutOfRangeException(nameof(inputArmLength));
                case > 0:
                    inputVector = inputVector.NormalizeFast();
                    if (!inputVector.IsValidVector())
                        throw new ArgumentException(nameof(inputVector));
                    break;
            }

            switch (outputArmLength)
            {
                case < 0:
                    throw new ArgumentOutOfRangeException(nameof(outputArmLength));
                case > 0:
                    ray = ray.Normalize();
                    if (!ray.Vector.IsValidVector())
                        throw new ArgumentException(nameof(ray));
                    break;
            }

            Point           = ray.Point;
            Vector          = ray.Vector;
            UseInputVector  = useInputVector;
            InputVector     = inputVector;
            InputArmLength  = inputArmLength;
            OutputArmLength = outputArmLength;
            Tag             = null;
        }

        [JsonConstructor]
        public WayPoint(Point point, Vector vector, Vector inputVector, double inputArmLength,
            double outputArmLength)
        {
            switch (inputArmLength)
            {
                case < 0: throw new ArgumentOutOfRangeException(nameof(inputArmLength));
                case > 0:
                    inputVector = inputVector.NormalizeFast();
                    if (!inputVector.IsValidVector())
                        throw new ArgumentException(nameof(inputVector));
                    break;
            }

            switch (outputArmLength)
            {
                case < 0:
                    throw new ArgumentOutOfRangeException(nameof(outputArmLength));
                case > 0:
                    vector = vector.NormalizeFast();
                    if (!vector.IsValidVector())
                        throw new ArgumentException(nameof(vector));
                    break;
            }

            Point           = point;
            Vector          = vector;
            UseInputVector  = inputVector.IsValidVector();
            InputVector     = inputVector;
            InputArmLength  = inputArmLength;
            OutputArmLength = outputArmLength;
            Tag             = null;
        }

        public WayPoint(PathRay ray, bool useInputVector, Vector inputVector)
        {
            Point           = ray.Point;
            Vector          = ray.Vector;
            UseInputVector  = useInputVector && inputVector.IsValidVector();
            InputVector     = inputVector;
            InputArmLength  = 0;
            OutputArmLength = 0;
            Tag             = null;
        }

        public WayPoint(PathRay ray)
        {
            Point           = ray.Point;
            Vector          = ray.Vector;
            UseInputVector  = false;
            InputVector     = default;
            InputArmLength  = 0;
            OutputArmLength = 0;
            Tag             = null;
        }


        public WayPoint(PathRay ray, Vector inputVector)
        {
            Point           = ray.Point;
            Vector          = ray.Vector;
            UseInputVector  = inputVector.IsValidVector();
            InputVector     = inputVector;
            InputArmLength  = 0;
            OutputArmLength = 0;
            Tag             = null;
        }

        public static WayPoint operator +(WayPoint a, Vector v)
        {
            return new WayPoint(a, v);
        }

        public static implicit operator WayPoint(PathRay ray)
        {
            return new WayPoint(ray);
        }


        public bool ShouldSerializeInputArmLength()
        {
            return InputArmLength != 0d;
        }


        public bool ShouldSerializeOutputArmLength()
        {
            return OutputArmLength != 0d;
        }


        public bool ShouldSerializeUseInputVector()
        {
            return UseInputVector;
        }

        #region properties

        public Vector Vector { get; }

        public Point Point { get; }


        public bool UseInputVector { get; }

        public Vector InputVector { get; }

        public double InputArmLength { get; }

        public double OutputArmLength { get; }


        public PathRayWithArm InputRay
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (UseInputVector)
                    return new PathRayWithArm(Point, InputVector, InputArmLength);
                return new PathRayWithArm(Point, Vector, InputArmLength);
            }
        }

        public PathRayWithArm OutputRay
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new PathRayWithArm(Point, Vector, OutputArmLength);
        }

        public object Tag { get; set; }

        #endregion
    }
}
