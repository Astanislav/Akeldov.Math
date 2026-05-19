namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a finite parameterized curve with explicit start and end points.
    /// </summary>
    public interface IBoundedParameterizedCurve : IFiniteCurve, IParameterizedProjectableCurve
    {
        /// <summary>
        /// Gets the start point.
        /// </summary>
        VectorXY StartPoint { get; }

        /// <summary>
        /// Gets the end point.
        /// </summary>
        VectorXY EndPoint { get; }
    }
}
