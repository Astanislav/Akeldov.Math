namespace Akeldov.Math.Spatial2D
{
    /// <summary>
    /// Provides tolerance constants used by two-dimensional geometry operations.
    /// </summary>
    public static class GeometryConstants
    {
        /// <summary>
        /// The default absolute tolerance for geometry comparisons.
        /// </summary>
        public const float GeometryEpsilon = 1e-6f;

        /// <summary>
        /// The squared default absolute tolerance for squared-distance comparisons.
        /// </summary>
        public const float GeometryEpsilonSquared = GeometryEpsilon * GeometryEpsilon;
    }
}
