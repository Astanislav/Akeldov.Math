using System;
using Akeldov.Math.Spatial2D.Rasterization;

namespace Akeldov.Math.Spatial2D.Imaging
{
    /// <summary>
    /// Stores mutable 16-bit RGBA raster values on a raster grid.
    /// </summary>
    public sealed class RGBA16BitRaster
    {
        /// <summary>
        /// Initializes a new 16-bit RGBA raster and stores the supplied channel buffers by reference.
        /// </summary>
        /// <param name="grid">The raster grid that describes the image coordinates.</param>
        /// <param name="redValues">The mutable red channel buffer indexed as [x, y].</param>
        /// <param name="greenValues">The mutable green channel buffer indexed as [x, y].</param>
        /// <param name="blueValues">The mutable blue channel buffer indexed as [x, y].</param>
        /// <param name="alphaValues">The mutable alpha channel buffer indexed as [x, y].</param>
        public RGBA16BitRaster(
            RasterGrid grid,
            ushort[,] redValues,
            ushort[,] greenValues,
            ushort[,] blueValues,
            ushort[,] alphaValues)
        {
            if (redValues == null)
                throw new ArgumentNullException(nameof(redValues));

            if (greenValues == null)
                throw new ArgumentNullException(nameof(greenValues));

            if (blueValues == null)
                throw new ArgumentNullException(nameof(blueValues));

            if (alphaValues == null)
                throw new ArgumentNullException(nameof(alphaValues));

            ValidateChannelDimensions(grid, redValues, nameof(redValues));
            ValidateChannelDimensions(grid, greenValues, nameof(greenValues));
            ValidateChannelDimensions(grid, blueValues, nameof(blueValues));
            ValidateChannelDimensions(grid, alphaValues, nameof(alphaValues));

            Grid = grid;
            RedValues = redValues;
            GreenValues = greenValues;
            BlueValues = blueValues;
            AlphaValues = alphaValues;
        }

        /// <summary>
        /// Gets the raster grid that describes the image coordinates.
        /// </summary>
        public RasterGrid Grid { get; }

        /// <summary>
        /// Gets the mutable red channel buffer indexed as [x, y].
        /// </summary>
        /// <remarks>
        /// The raster stores the supplied array by reference for performance. Changes to this array are reflected in the raster.
        /// Use <see cref="Clone"/> when a detached snapshot is needed.
        /// </remarks>
        public ushort[,] RedValues { get; }

        /// <summary>
        /// Gets the mutable green channel buffer indexed as [x, y].
        /// </summary>
        /// <remarks>
        /// The raster stores the supplied array by reference for performance. Changes to this array are reflected in the raster.
        /// Use <see cref="Clone"/> when a detached snapshot is needed.
        /// </remarks>
        public ushort[,] GreenValues { get; }

        /// <summary>
        /// Gets the mutable blue channel buffer indexed as [x, y].
        /// </summary>
        /// <remarks>
        /// The raster stores the supplied array by reference for performance. Changes to this array are reflected in the raster.
        /// Use <see cref="Clone"/> when a detached snapshot is needed.
        /// </remarks>
        public ushort[,] BlueValues { get; }

        /// <summary>
        /// Gets the mutable alpha channel buffer indexed as [x, y].
        /// </summary>
        /// <remarks>
        /// The raster stores the supplied array by reference for performance. Changes to this array are reflected in the raster.
        /// Use <see cref="Clone"/> when a detached snapshot is needed.
        /// </remarks>
        public ushort[,] AlphaValues { get; }

        /// <summary>
        /// Gets the raster width in pixels.
        /// </summary>
        public int Width => RedValues.GetLength(0);

        /// <summary>
        /// Gets the raster height in pixels.
        /// </summary>
        public int Height => RedValues.GetLength(1);

        /// <summary>
        /// Creates a detached copy of this raster and its channel buffers.
        /// </summary>
        /// <returns>A raster snapshot with independent mutable channel buffers.</returns>
        public RGBA16BitRaster Clone()
        {
            return new RGBA16BitRaster(
                Grid,
                (ushort[,])RedValues.Clone(),
                (ushort[,])GreenValues.Clone(),
                (ushort[,])BlueValues.Clone(),
                (ushort[,])AlphaValues.Clone());
        }

        private static void ValidateChannelDimensions(RasterGrid grid, ushort[,] values, string parameterName)
        {
            if (values.GetLength(0) != grid.Resolution.X || values.GetLength(1) != grid.Resolution.Y)
                throw new ArgumentException("RGBA raster channel dimensions must match the raster grid resolution.", parameterName);
        }
    }
}
