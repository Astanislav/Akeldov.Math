using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a closed two-dimensional contour made from curves.
    /// </summary>
    public interface IContour
    {
        /// <summary>
        /// Gets the curves that form this contour.
        /// </summary>
        IReadOnlyList<ICurve> Curves { get; }

        /// <summary>
        /// Determines whether the specified point lies inside or on this contour.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns><see langword="true"/> if the point lies inside or on the contour; otherwise, <see langword="false"/>.</returns>
        bool Contains(VectorXY point);
    }
}
