using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Spatial2D
{
    /// <summary>
    /// Provides affine transformation helpers for <see cref="VectorXYInt"/>.
    /// </summary>
    public static partial class VectorXYIntExtensions
    {
        /// <summary>
        /// Rotates an integer point around the origin and then applies an offset.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <param name="angleRad">The rotation angle in radians.</param>
        /// <param name="offset">The translation offset.</param>
        /// <returns>The transformed point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXYInt point, float angleRad, VectorXY offset)
        {
            var rotated = point.Rotate(angleRad);
            return rotated + offset;
        }

        /// <summary>
        /// Rotates an integer point around the origin and then applies an integer offset.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <param name="angleRad">The rotation angle in radians.</param>
        /// <param name="offset">The translation offset.</param>
        /// <returns>The transformed point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXYInt point, float angleRad, VectorXYInt offset)
        {
            var rotated = point.Rotate(angleRad);
            return rotated + offset;
        }

        /// <summary>
        /// Scales an integer point, rotates it around the origin, and then applies an offset.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <param name="scaleFactor">The uniform scale factor.</param>
        /// <param name="angleRad">The rotation angle in radians.</param>
        /// <param name="offset">The translation offset.</param>
        /// <returns>The transformed point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXYInt point, float scaleFactor, float angleRad, VectorXY offset)
        {
            var scaled = point * scaleFactor;
            var rotated = scaled.Rotate(angleRad);
            return rotated + offset;
        }

        /// <summary>
        /// Scales an integer point, rotates it around the origin, and then applies an integer offset.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <param name="scaleFactor">The uniform scale factor.</param>
        /// <param name="angleRad">The rotation angle in radians.</param>
        /// <param name="offset">The translation offset.</param>
        /// <returns>The transformed point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXYInt point, float scaleFactor, float angleRad, VectorXYInt offset)
        {
            var scaled = point * scaleFactor;
            var rotated = scaled.Rotate(angleRad);
            return rotated + offset;
        }

        /// <summary>
        /// Rotates an integer point around the specified integer pivot.
        /// </summary>
        /// <param name="point">The point to rotate.</param>
        /// <param name="pivot">The pivot point.</param>
        /// <param name="angleRad">The rotation angle in radians.</param>
        /// <returns>The rotated point.</returns>
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

        /// <summary>
        /// Rotates an integer point around the specified floating-point pivot.
        /// </summary>
        /// <param name="point">The point to rotate.</param>
        /// <param name="pivot">The pivot point.</param>
        /// <param name="angleRad">The rotation angle in radians.</param>
        /// <returns>The rotated point.</returns>
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
