using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Vectors.XY
{
    public static partial class VectorXYIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXYInt point, float angleRad, VectorXY offset)
        {
            var rotated = point.Rotate(angleRad);
            return rotated + offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXYInt point, float angleRad, VectorXYInt offset)
        {
            var rotated = point.Rotate(angleRad);
            return rotated + offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXYInt point, float scaleFactor, float angleRad, VectorXY offset)
        {
            var scaled = point * scaleFactor;
            var rotated = scaled.Rotate(angleRad);
            return rotated + offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXYInt point, float scaleFactor, float angleRad, VectorXYInt offset)
        {
            var scaled = point * scaleFactor;
            var rotated = scaled.Rotate(angleRad);
            return rotated + offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Rotate(this VectorXYInt point, VectorXYInt pivot, float angleRad)
        {
            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            VectorXY offset = point - pivot;

            float rotatedX = offset.X * cos - offset.Y * sin;
            float rotatedY = offset.X * sin + offset.Y * cos;

            return new VectorXY(rotatedX, rotatedY) + pivot;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Rotate(this VectorXYInt point, VectorXY pivot, float angleRad)
        {
            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            VectorXY offset = point - pivot;

            float rotatedX = offset.X * cos - offset.Y * sin;
            float rotatedY = offset.X * sin + offset.Y * cos;

            return new VectorXY(rotatedX, rotatedY) + pivot;
        }
    }
}