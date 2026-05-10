namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a curve that can project points onto itself.
    /// </summary>
    public interface IProjectableCurve : ICurve
    {
        /// <summary>
        /// Projects the specified point onto this curve.
        /// </summary>
        /// <param name="point">The point to project.</param>
        /// <returns>The projection point, curve parameter, and distance to this curve.</returns>
        CurveProjection Project(VectorXY point);
    }
}
