namespace Akeldov.Math.Spatial2D
{
    /// <summary>
    /// Represents an object that can measure distance to a two-dimensional point.
    /// </summary>
    public interface IDistanceMeasurable
    {
        /// <summary>
        /// Returns the distance from this object to the specified point.
        /// </summary>
        /// <param name="point">The finite point to measure to.</param>
        /// <returns>The distance to the point.</returns>
        float Distance(VectorXY point);
    }
}
