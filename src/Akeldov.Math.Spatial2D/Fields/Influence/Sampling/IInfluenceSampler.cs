using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Fields
{
    /// <summary>
    /// Combines influence sources into a sampled value at a two-dimensional point.
    /// </summary>
    /// <remarks>
    /// Samplers implement mathematical interpolation or extrapolation strategies and do not
    /// necessarily clamp their output to a field range. Bounded field implementations such as
    /// <see cref="IFloatField"/> and <see cref="IIntField"/> are responsible for preserving
    /// their public <c>Min</c>/<c>Max</c> contracts.
    /// </remarks>
    /// <typeparam name="TSource">The influence source type.</typeparam>
    /// <typeparam name="TValue">The sampled value type.</typeparam>
    public interface IInfluenceSampler<TSource, TValue>
        where TSource : IInfluenceSource<TValue>
    {
        /// <summary>
        /// Samples a value from the specified influence sources at the specified point.
        /// </summary>
        /// <param name="influenceSources">The influence sources to sample from.</param>
        /// <param name="point">The point to sample.</param>
        /// <returns>The sampled value. The value may be outside a bounded field range.</returns>
        TValue Sample(IReadOnlyList<TSource> influenceSources, VectorXY point);
    }
}
