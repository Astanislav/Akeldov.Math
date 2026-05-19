namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a curve with two endpoints.
    /// </summary>
    /// <remarks>
    /// <see cref="EndpointA"/> and <see cref="EndpointB"/> identify the two boundary points of the curve.
    /// They do not imply traversal direction. Use <see cref="IParameterizedTwoEndpointCurve"/> when start
    /// and end points are part of the contract.
    /// </remarks>
    public interface ITwoEndpointCurve : ICurve
    {
        /// <summary>
        /// Gets one endpoint.
        /// </summary>
        VectorXY EndpointA { get; }

        /// <summary>
        /// Gets the other endpoint.
        /// </summary>
        VectorXY EndpointB { get; }
    }
}
