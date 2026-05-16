namespace Akeldov.Math.Spatial2D.Imaging
{
    /// <summary>
    /// Provides PNG export extension methods for grayscale rasters.
    /// </summary>
    public static class RasterPngExtensions
    {
        /// <summary>
        /// Saves a 16-bit grayscale raster as a 16-bit grayscale PNG file.
        /// </summary>
        /// <param name="raster">The raster to save.</param>
        /// <param name="path">The output PNG file path.</param>
        public static void SaveAsPng(this Gray16BitRaster raster, string path)
        {
            PngEncoder.Save(raster, path);
        }
    }
}
