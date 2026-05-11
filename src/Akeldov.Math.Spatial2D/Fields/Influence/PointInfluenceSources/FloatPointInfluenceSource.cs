using System;

namespace Akeldov.Math.Spatial2D.Fields
{
    /// <summary>
    /// Represents a point influence source that contributes a floating-point value.
    /// </summary>
    public class FloatPointInfluenceSource : IPointInfluenceSource<float>
    {
        /// <summary>
        /// Initializes a new floating-point influence source.
        /// </summary>
        /// <param name="power">The source power used by influence samplers.</param>
        /// <param name="center">The source position.</param>
        /// <param name="value">The value contributed by this source.</param>
        public FloatPointInfluenceSource(float power, VectorXY center, float value)
        {
            if (power < 0f || float.IsNaN(power))
                throw new ArgumentOutOfRangeException(nameof(power), "Influence source power must be non-negative and not NaN.");

            if (float.IsNaN(value))
                throw new ArgumentOutOfRangeException(nameof(value), "Influence source value must not be NaN.");

            Power = power;
            Center = center;
            Value = value;
        }

        /// <summary>
        /// Gets the source power used by influence samplers.
        /// </summary>
        public float Power { get; }

        /// <summary>
        /// Gets the source position.
        /// </summary>
        public VectorXY Center { get; }

        /// <summary>
        /// Gets the value contributed by this source.
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Returns the distance from this source to the specified point.
        /// </summary>
        /// <param name="point">The point to measure to.</param>
        /// <returns>The Euclidean distance from the source center to the point.</returns>
        public float Distance(VectorXY point)
        {
            return Center.Distance(point);
        }

        /// <summary>
        /// Gets the influence contribution of this source for the specified point.
        /// </summary>
        /// <param name="point">The point being sampled.</param>
        /// <returns>The value, source point, distance, and power used by influence samplers.</returns>
        public InfluenceSample<float> GetInfluence(VectorXY point)
        {
            return new InfluenceSample<float>(Value, Center, Distance(point), Power);
        }
    }
}
