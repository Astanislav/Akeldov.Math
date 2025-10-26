using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Vectors.XY
{
    public static partial class VectorXYExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Clamp(this VectorXY source, VectorXY min, VectorXY max)
        {
            if (max.X < min.X)
                throw new ArgumentException($"Cannot clamp value: min.X ({min.X}) must be less than or equal to max.X ({max.X}).");

            if (max.Y < min.Y)
                throw new ArgumentException($"Cannot clamp value: min.Y ({min.Y}) must be less than or equal to max.Y ({max.Y}).");

            var x = MathF.Min(MathF.Max(source.X, min.X), max.X);
            var y = MathF.Min(MathF.Max(source.Y, min.Y), max.Y);

            return new VectorXY(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY ClampMin(this VectorXY source, VectorXY min)
        {
            var x = MathF.Max(source.X, min.X);
            var y = MathF.Max(source.Y, min.Y);

            return new VectorXY(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY ClampMax(this VectorXY source, VectorXY max)
        {
            var x = MathF.Min(source.X, max.X);
            var y = MathF.Min(source.Y, max.Y);

            return new VectorXY(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY ClampMin(this VectorXY source, VectorXYInt min)
        {
            var x = MathF.Max(source.X, min.X);
            var y = MathF.Max(source.Y, min.Y);

            return new VectorXY(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY ClampMax(this VectorXY source, VectorXYInt max)
        {
            var x = MathF.Min(source.X, max.X);
            var y = MathF.Min(source.Y, max.Y);

            return new VectorXY(x, y);
        }
    }
}
