using System.Collections.Generic;
using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Contours
{
    /// <summary>
    /// Represents a closed two-dimensional contour made from bounded parameterized curves.
    /// </summary>
    public interface IContour
    {
        /// <summary>
        /// Gets the bounded parameterized curves that form this contour.
        /// </summary>
        IReadOnlyList<IBoundedParameterizedCurve> Curves { get; }

        /// <summary>
        /// Determines whether the specified point lies inside or on this contour.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns><see langword="true"/> if the point lies inside or on the contour; otherwise, <see langword="false"/>.</returns>
        bool Contains(VectorXY point);
    }
}
