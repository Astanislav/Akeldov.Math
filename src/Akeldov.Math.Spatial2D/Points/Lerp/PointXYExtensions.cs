using System;

namespace Akeldov.Math.Spatial2D
{
    /// <summary>
    /// Provides extension methods for <see cref="PointXY"/>.
    /// </summary>
    public static partial class PointXYExtensions
    {
        /// <summary>
        /// Linearly interpolates from this point to the specified target point.
        /// </summary>
        /// <param name="source">The interpolation start point.</param>
        /// <param name="target">The interpolation target point.</param>
        /// <param name="t">The interpolation parameter.</param>
        /// <returns>The interpolated point.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="t"/> is NaN.</exception>
        public static PointXY LerpTo(this PointXY source, PointXY target, float t)
        {
            if (float.IsNaN(t))
                throw new ArgumentOutOfRangeException(nameof(t), "Interpolation parameter must not be NaN.");

            return new PointXY(
                source.X + (target.X - source.X) * t,
                source.Y + (target.Y - source.Y) * t);
        }
    }
}
