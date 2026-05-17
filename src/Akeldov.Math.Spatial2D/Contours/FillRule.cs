namespace Akeldov.Math.Spatial2D.Contours
{
    /// <summary>
    /// Defines how multiple contours are converted into a filled region.
    /// </summary>
    public enum FillRule
    {
        /// <summary>
        /// A point is inside the region when it is inside an odd number of contours.
        /// </summary>
        EvenOdd = 0
    }
}
