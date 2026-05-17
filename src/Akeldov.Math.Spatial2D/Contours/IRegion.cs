using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Contours
{
    /// <summary>
    /// Represents a filled two-dimensional region defined by one or more contours.
    /// </summary>
    public interface IRegion
    {
        /// <summary>
        /// Gets the contours that define this region.
        /// </summary>
        IReadOnlyList<IContour> Contours { get; }

        /// <summary>
        /// Gets the fill rule used to interpret the contours.
        /// </summary>
        FillRule FillRule { get; }

        /// <summary>
        /// Determines whether the specified point lies inside or on this region.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns><see langword="true"/> if the point lies inside or on the region; otherwise, <see langword="false"/>.</returns>
        bool Contains(VectorXY point);
    }
}
