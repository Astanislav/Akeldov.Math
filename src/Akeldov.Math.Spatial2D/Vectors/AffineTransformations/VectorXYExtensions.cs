using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Spatial2D
{
    /// <summary>
    /// Provides affine transformation helpers for <see cref="VectorXY"/>.
    /// </summary>
    public static partial class VectorXYExtensions
    {
        /// <summary>
        /// Rotates a point around the origin and then applies an offset.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <param name="angle">The rotation angle in radians.</param>
        /// <param name="offset">The translation offset.</param>
        /// <returns>The transformed point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXY point, float angle, VectorXY offset)
        {
            var rotated = point.Rotate(angle);
            return rotated + offset;
        }

        /// <summary>
        /// Rotates a point around the origin and then applies an integer offset.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <param name="angle">The rotation angle in radians.</param>
        /// <param name="offset">The translation offset.</param>
        /// <returns>The transformed point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXY point, float angle, VectorXYInt offset)
        {
            var rotated = point.Rotate(angle);
            return rotated + offset;
        }

        /// <summary>
        /// Scales a point, rotates it around the origin, and then applies an offset.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <param name="scaleFactor">The uniform scale factor.</param>
        /// <param name="angle">The rotation angle in radians.</param>
        /// <param name="offset">The translation offset.</param>
        /// <returns>The transformed point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXY point, float scaleFactor, float angle, VectorXY offset)
        {
            var scaled = point * scaleFactor;
            var rotated = scaled.Rotate(angle);
            return rotated + offset;
        }

        /// <summary>
        /// Scales a point, rotates it around the origin, and then applies an integer offset.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <param name="scaleFactor">The uniform scale factor.</param>
        /// <param name="angle">The rotation angle in radians.</param>
        /// <param name="offset">The translation offset.</param>
        /// <returns>The transformed point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Transform(this VectorXY point, float scaleFactor, float angle, VectorXYInt offset)
        {
            var scaled = point * scaleFactor;
            var rotated = scaled.Rotate(angle);
            return rotated + offset;
        }

        /// <summary>
        /// Rotates a point around the specified pivot.
        /// </summary>
        /// <param name="point">The point to rotate.</param>
        /// <param name="pivot">The pivot point.</param>
        /// <param name="angle">The rotation angle in radians.</param>
        /// <returns>The rotated point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Rotate(this VectorXY point, VectorXY pivot, float angle)
        {
            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);

            VectorXY offset = point - pivot;

            float rotatedX = offset.X * cos - offset.Y * sin;
            float rotatedY = offset.X * sin + offset.Y * cos;

            return new VectorXY(rotatedX, rotatedY) + pivot;
        }

        /// <summary>
        /// Rotates a point around the specified integer pivot.
        /// </summary>
        /// <param name="point">The point to rotate.</param>
        /// <param name="pivot">The pivot point.</param>
        /// <param name="angle">The rotation angle in radians.</param>
        /// <returns>The rotated point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Rotate(this VectorXY point, VectorXYInt pivot, float angle)
        {
            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);

            VectorXY offset = point - pivot;

            float rotatedX = offset.X * cos - offset.Y * sin;
            float rotatedY = offset.X * sin + offset.Y * cos;

            return new VectorXY(rotatedX, rotatedY) + pivot;
        }
    }
}
