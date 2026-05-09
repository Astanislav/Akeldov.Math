namespace Akeldov.Math.Spatial2D.Fields
{
    /// <summary>
    /// Represents the contribution of an influence source at a sampled point.
    /// </summary>
    /// <typeparam name="TValue">The value type contributed by the source.</typeparam>
    public readonly struct InfluenceSample<TValue>
    {
        /// <summary>
        /// Initializes a new influence sample.
        /// </summary>
        /// <param name="value">The source value at the sampled point.</param>
        /// <param name="point">The point on the influence source used for the sample.</param>
        /// <param name="distance">The distance from the requested point to <paramref name="point"/>.</param>
        /// <param name="power">The source power at <paramref name="point"/>.</param>
        public InfluenceSample(TValue value, VectorXY point, float distance, float power)
        {
            Value = value;
            Point = point;
            Distance = distance;
            Power = power;
        }

        /// <summary>
        /// Gets the value contributed by the influence source.
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Gets the point on the influence source used for this sample.
        /// </summary>
        public VectorXY Point { get; }

        /// <summary>
        /// Gets the distance from the requested point to <see cref="Point"/>.
        /// </summary>
        public float Distance { get; }

        /// <summary>
        /// Gets the source power at <see cref="Point"/>.
        /// </summary>
        public float Power { get; }
    }
}
