using System;
using System.Collections.Generic;
using Akeldov.Math.Spatial2D.Fields;
using Akeldov.Math.Spatial2D.Imaging;

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

        /// <summary>
        /// Rasterizes point-source culling selections on the specified raster grid using the default 16-bit RGBA palette.
        /// </summary>
        /// <typeparam name="TPointSource">The point influence source type.</typeparam>
        /// <param name="source">The point influence sources used to color culling selections.</param>
        /// <param name="grid">The raster grid that describes the sampled region.</param>
        /// <param name="culler">The culler used to select sources for each raster cell center.</param>
        /// <returns>A 16-bit RGBA raster showing the culling selection map.</returns>
        public static RGBA16BitRaster RasterizeCullingMap<TPointSource>(
            this IReadOnlyList<TPointSource> source,
            RasterGrid grid,
            IInfluenceSourceCuller<TPointSource> culler)
            where TPointSource : IPointInfluenceSource
        {
            return source.Rasterize(grid, new CullingMapRGBA16BitRasterizer<TPointSource>(culler));
        }

        /// <summary>
        /// Rasterizes point-source culling selections on the specified raster grid using the specified 16-bit RGBA palette.
        /// </summary>
        /// <typeparam name="TPointSource">The point influence source type.</typeparam>
        /// <param name="source">The point influence sources used to color culling selections.</param>
        /// <param name="grid">The raster grid that describes the sampled region.</param>
        /// <param name="culler">The culler used to select sources for each raster cell center.</param>
        /// <param name="sourceColors">
        /// The source color palette. Source indexes beyond this palette wrap around to the beginning.
        /// </param>
        /// <returns>A 16-bit RGBA raster showing the culling selection map.</returns>
        public static RGBA16BitRaster RasterizeCullingMap<TPointSource>(
            this IReadOnlyList<TPointSource> source,
            RasterGrid grid,
            IInfluenceSourceCuller<TPointSource> culler,
            IReadOnlyList<RGBA16BitColor> sourceColors)
            where TPointSource : IPointInfluenceSource
        {
            return source.Rasterize(grid, new CullingMapRGBA16BitRasterizer<TPointSource>(culler, sourceColors));
        }
    }
}
