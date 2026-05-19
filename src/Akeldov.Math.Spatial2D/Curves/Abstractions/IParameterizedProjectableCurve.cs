namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a curve that can project points onto itself and report a curve coordinate for the projection.
    /// </summary>
    public interface IParameterizedProjectableCurve : IProjectableCurve
    {
        /// <summary>
        /// Projects the specified point onto this curve and reports the curve coordinate of the projected point.
        /// </summary>
        /// <param name="point">The finite point to project.</param>
        /// <returns>The projection point, curve coordinate, and distance to this curve.</returns>
        ParameterizedCurveProjection ProjectWithParameter(VectorXY point);
    }
}
