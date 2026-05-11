namespace Akeldov.Math.Spatial2D.Curves
{
    using System;

    /// <summary>
    /// Represents the result of projecting a point onto a curve.
    /// </summary>
    public readonly struct CurveProjection
    {
        /// <summary>
        /// Initializes a new projection result.
        /// </summary>
        /// <param name="point">The projected point on the curve.</param>
        /// <param name="parameter">The curve parameter at the projected point.</param>
        /// <param name="distance">The distance from the original point to the projected point.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="distance"/> is negative, NaN, or infinite.</exception>
        public CurveProjection(VectorXY point, float parameter, float distance)
        {
            if (distance < 0f || float.IsNaN(distance) || float.IsInfinity(distance))
                throw new ArgumentOutOfRangeException(nameof(distance), "Projection distance must be finite and non-negative.");

            Point = point;
            Parameter = parameter;
            Distance = distance;
        }

        /// <summary>
        /// Gets the projected point on the curve.
        /// </summary>
        public VectorXY Point { get; }

        /// <summary>
        /// Gets the curve parameter at the projected point.
        /// </summary>
        public float Parameter { get; }

        /// <summary>
        /// Gets the distance from the original point to the projected point.
        /// </summary>
        public float Distance { get; }
    }
}
