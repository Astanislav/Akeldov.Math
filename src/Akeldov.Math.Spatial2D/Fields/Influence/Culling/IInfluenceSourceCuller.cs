using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Fields
{
    /// <summary>
    /// Selects the influence sources that should be considered when sampling a point.
    /// </summary>
    /// <remarks>
    /// Implementations must return a non-null list containing at least one source. If no source can
    /// be selected by the primary culling strategy, the culler is responsible for applying an
    /// explicit fallback, such as returning the nearest source.
    /// </remarks>
    /// <typeparam name="TInfluenceSource">The influence source type.</typeparam>
    public interface IInfluenceSourceCuller<TInfluenceSource>
        where TInfluenceSource : IInfluenceSource
    {
        /// <summary>
        /// Returns the subset of influence sources relevant to the specified point.
        /// </summary>
        /// <param name="point">The point being sampled.</param>
        /// <returns>
        /// The selected influence sources. The returned list must be non-null and contain at least
        /// one source.
        /// </returns>
        List<TInfluenceSource> Cull(VectorXY point);
    }
}
