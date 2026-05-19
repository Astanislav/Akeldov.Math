namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a curve with finite length.
    /// </summary>
    public interface IFiniteCurve : ICurve
    {
        /// <summary>
        /// Gets the curve length.
        /// </summary>
        float Length { get; }
    }
}
