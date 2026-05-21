using System;
using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Imaging;

namespace Akeldov.Math.Spatial2D.Rasterization
{
    /// <summary>
    /// Rasterizes curves into 8-bit grayscale rasters using distance-to-curve mapping.
    /// </summary>
    public sealed class CurveDistanceGray8BitRasterizer : IRasterizer<ICurve, Gray8BitRaster>
    {
        private readonly Func<float, byte> _distanceToGrayLevel;

        /// <summary>
        /// Initializes a new curve rasterizer.
        /// </summary>
        /// <param name="distanceToGrayLevel">The function that maps distance to the curve to an 8-bit grayscale value.</param>
        public CurveDistanceGray8BitRasterizer(Func<float, byte> distanceToGrayLevel)
        {
            _distanceToGrayLevel = distanceToGrayLevel ?? throw new ArgumentNullException(nameof(distanceToGrayLevel));
        }

        /// <inheritdoc/>
        public Gray8BitRaster Rasterize(ICurve source, RasterGrid grid)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            ValidateGrid(grid);
            var values = new byte[grid.Resolution.X, grid.Resolution.Y];
            VectorXY cellSize = grid.CellSize;
            float firstX = grid.Origin.X + cellSize.X * 0.5f;
            float firstY = grid.Origin.Y + cellSize.Y * 0.5f;

            for (int y = 0; y < grid.Resolution.Y; y++)
            {
                float pointY = firstY + y * cellSize.Y;
                for (int x = 0; x < grid.Resolution.X; x++)
                {
                    VectorXY point = new VectorXY(firstX + x * cellSize.X, pointY);
                    float distance = source.Distance(point);
                    values[x, y] = _distanceToGrayLevel(distance);
                }
            }

            return new Gray8BitRaster(grid, values);
        }

        private static void ValidateGrid(RasterGrid grid)
        {
            if (!grid.Size.IsFinite || grid.Size.X <= 0f || grid.Size.Y <= 0f)
                throw new ArgumentOutOfRangeException(nameof(grid), "Raster grid size components must be finite and positive.");

            if (grid.Resolution.X <= 0 || grid.Resolution.Y <= 0)
                throw new ArgumentOutOfRangeException(nameof(grid), "Raster grid resolution components must be positive.");
        }
    }
}
