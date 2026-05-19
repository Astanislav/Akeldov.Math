namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a parameterized curve that can project points onto itself and report a curve coordinate for each projection.
    /// </summary>
    public interface IParameterizedProjectableCurve : IParameterizedCurve, IProjectableCurve
    {
        /// <summary>
        /// Projects the specified point onto this curve and reports the curve coordinate of the projected point.
        /// </summary>
        /// <param name="point">The finite point to project.</param>
        /// <returns>
        /// The projection point, its curve coordinate in the same coordinate system as <see cref="IParameterizedCurve.GetPoint"/>,
        /// and the distance to this curve.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="point"/> has a non-finite coordinate.</exception>
        ParameterizedCurveProjection ProjectWithParameter(VectorXY point);
    }
}
