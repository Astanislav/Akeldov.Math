namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a parameterized two-endpoint curve with an explicit traversal direction.
    /// </summary>
    public interface IParameterizedTwoEndpointCurve : ITwoEndpointCurve, IParameterizedCurve
    {
        /// <summary>
        /// Gets the endpoint at the start of the traversal direction.
        /// </summary>
        VectorXY StartPoint { get; }

        /// <summary>
        /// Gets the endpoint at the end of the traversal direction.
        /// </summary>
        VectorXY EndPoint { get; }
    }
}
