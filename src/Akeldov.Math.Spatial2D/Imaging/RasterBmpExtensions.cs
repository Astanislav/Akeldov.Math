namespace Akeldov.Math.Spatial2D.Imaging
{
    /// <summary>
    /// Provides BMP export extension methods for grayscale rasters.
    /// </summary>
    public static class RasterBmpExtensions
    {
        /// <summary>
        /// Saves an 8-bit grayscale raster as an indexed grayscale BMP file.
        /// </summary>
        /// <param name="raster">The raster to save.</param>
        /// <param name="path">The output BMP file path.</param>
        public static void SaveAsBmp(this Gray8BitRaster raster, string path)
        {
            BmpEncoder.Save(raster, path);
        }
    }
}
