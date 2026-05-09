using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Spatial2D
{
    public static partial class VectorXYIntExtensions
    {
        /// <summary>
        /// Rotates an integer vector around the origin by the specified angle.
        /// </summary>
        /// <param name="point">The vector to rotate.</param>
        /// <param name="angleRad">The rotation angle in radians.</param>
        /// <returns>The rotated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Rotate(this VectorXYInt point, float angleRad)
        {
            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            float x = point.X * cos - point.Y * sin;
            float y = point.X * sin + point.Y * cos;

            return new VectorXY(x, y);
        }
    }
}
