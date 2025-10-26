using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Vectors.XY
{
    public static partial class VectorXYIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt Clamp(this VectorXYInt source, VectorXYInt min, VectorXYInt max)
        {
            if (max.X < min.X)
                throw new ArgumentException($"Cannot clamp value: min.X ({min.X}) must be less than or equal to max.X ({max.X}).");

            if (max.Y < min.Y)
                throw new ArgumentException($"Cannot clamp value: min.Y ({min.Y}) must be less than or equal to max.Y ({max.Y}).");

            var x = System.Math.Min(System.Math.Max(source.X, min.X), max.X);
            var y = System.Math.Min(System.Math.Max(source.Y, min.Y), max.Y);

            return new VectorXYInt(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt ClampMin(this VectorXYInt source, VectorXYInt min)
        {
            var x = System.Math.Max(source.X, min.X);
            var y = System.Math.Max(source.Y, min.Y);

            return new VectorXYInt(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt ClampMax(this VectorXYInt source, VectorXYInt max)
        {
            var x = System.Math.Min(source.X, max.X);
            var y = System.Math.Min(source.Y, max.Y);

            return new VectorXYInt(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt ClampMin(this VectorXYInt source, int minX, int minY)
        {
            var x = System.Math.Max(source.X, minX);
            var y = System.Math.Max(source.Y, minY);

            return new VectorXYInt(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt ClampMax(this VectorXYInt source, int maxX, int maxY)
        {
            var x = System.Math.Min(source.X, maxX);
            var y = System.Math.Min(source.Y, maxY);

            return new VectorXYInt(x, y);
        }
    }
}