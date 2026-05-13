using Akeldov.Math.Spatial2D;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a two-dimensional curve that can be measured and intersected with rays.
    /// </summary>
    public interface ICurve
    {
        /// <summary>
        /// Returns point intersections between this curve and the specified ray.
        /// </summary>
        /// <param name="ray">The ray to intersect with this curve.</param>
        /// <returns>A new mutable list of intersection points in the forward direction of the ray, owned by the caller.</returns>
        List<VectorXY> GetRayIntersections(Ray ray);

        /// <summary>
        /// Returns the shortest distance from the specified point to this curve.
        /// </summary>
        /// <param name="point">The finite point to measure from.</param>
        /// <returns>The distance to this curve.</returns>
        float Distance(VectorXY point);
    }
}
