using System;
using Akeldov.Math.Spatial2D.Rasterization;

namespace Akeldov.Math.Spatial2D.Imaging
{
    /// <summary>
    /// Stores 16-bit grayscale raster values on a raster grid.
    /// </summary>
    public sealed class Gray16BitRaster
    {
        /// <summary>
        /// Initializes a new 16-bit grayscale raster.
        /// </summary>
        /// <param name="grid">The raster grid that describes the image coordinates.</param>
        /// <param name="values">The grayscale values indexed as [x, y].</param>
        public Gray16BitRaster(RasterGrid grid, ushort[,] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (values.GetLength(0) != grid.Resolution.X || values.GetLength(1) != grid.Resolution.Y)
                throw new ArgumentException("Grayscale raster dimensions must match the raster grid resolution.", nameof(values));

            Grid = grid;
            Values = values;
        }

        /// <summary>
        /// Gets the raster grid that describes the image coordinates.
        /// </summary>
        public RasterGrid Grid { get; }

        /// <summary>
        /// Gets the grayscale values indexed as [x, y].
        /// </summary>
        public ushort[,] Values { get; }

        /// <summary>
        /// Gets the raster width in pixels.
        /// </summary>
        public int Width => Values.GetLength(0);

        /// <summary>
        /// Gets the raster height in pixels.
        /// </summary>
        public int Height => Values.GetLength(1);
    }
}
