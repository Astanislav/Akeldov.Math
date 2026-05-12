using System;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Provides angle helpers for floating-point values.
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// Normalizes an angle in radians to the range [0, 2 * pi).
        /// </summary>
        /// <param name="angle">The angle to normalize, in radians.</param>
        /// <returns>The normalized angle in radians.</returns>
        public static float NormalizeAngleRad(this float angle)
        {
            float twoPi = 2f * MathF.PI;
            angle %= twoPi;
            if (angle < 0f) angle += twoPi;
            return angle;
        }

        /// <summary>
        /// Determines whether an angle lies inside the closed arc from the start angle to the end angle.
        /// </summary>
        /// <param name="angle">The normalized angle to test, in radians.</param>
        /// <param name="startAngle">The normalized arc start angle in radians.</param>
        /// <param name="endAngle">The normalized arc end angle in radians.</param>
        /// <returns><see langword="true"/> if the angle is inside the arc; otherwise, <see langword="false"/>.</returns>
        public static bool IsAngleWithinArc(this float angle, float startAngle, float endAngle)
        {
            if (startAngle.AlmostEquals(endAngle))
                return angle.AlmostEquals(startAngle);

            if (startAngle < endAngle)
                return angle >= startAngle && angle <= endAngle;
            else
                return angle >= startAngle || angle <= endAngle;
        }
    }
}
