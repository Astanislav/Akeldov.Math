using System;

namespace Akeldov.Math.Spatial2D.Rasterization
{
    /// <summary>
    /// Provides rasterization extension methods.
    /// </summary>
    public static class RasterizationExtensions
    {
        /// <summary>
        /// Rasterizes a source object on the specified raster grid using the specified rasterizer.
        /// </summary>
        /// <typeparam name="TSource">The source object type to rasterize.</typeparam>
        /// <typeparam name="TRaster">The raster artifact type produced by the rasterizer.</typeparam>
        /// <param name="source">The source object to rasterize.</param>
        /// <param name="grid">The raster grid that describes the sampled region.</param>
        /// <param name="rasterizer">The rasterization strategy.</param>
        /// <returns>The raster artifact produced from the source object.</returns>
        public static TRaster Rasterize<TSource, TRaster>(
            this TSource source,
            RasterGrid grid,
            IRasterizer<TSource, TRaster> rasterizer)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (rasterizer == null)
                throw new ArgumentNullException(nameof(rasterizer));

            return rasterizer.Rasterize(source, grid);
        }
    }
}
