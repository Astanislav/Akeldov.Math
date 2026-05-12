namespace Akeldov.Math.Spatial2D
{
    /// <summary>
    /// Represents an object with a two-dimensional center position.
    /// </summary>
    public interface IHasPosition2D
    {
        /// <summary>
        /// Gets the object's position.
        /// </summary>
        VectorXY Position { get; }
    }
}
