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
        IReadOnlyList<IFinitePath> Curves { get; }

        /// <summary>
        /// Determines whether the specified point lies inside or on this closed contour.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="geometryEpsilon">The geometry comparison tolerance in world coordinate units.</param>
        /// <returns><see langword="true"/> if the point lies inside or on the closed contour; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="geometryEpsilon"/> is negative, NaN, or infinite.
        /// </exception>
        bool Encloses(VectorXY point, float geometryEpsilon = GeometryConstants.GeometryEpsilon);
    }
}
