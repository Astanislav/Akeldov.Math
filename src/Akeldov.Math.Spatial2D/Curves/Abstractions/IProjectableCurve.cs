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
        /// <param name="point">The finite point to project.</param>
        /// <returns>The projection point and distance to this curve.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="point"/> has a non-finite coordinate.</exception>
        CurveProjection Project(VectorXY point);
    }
}
