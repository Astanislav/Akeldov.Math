using System;
using System.Collections.Generic;
using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Imaging;

namespace Akeldov.Math.Spatial2D.Rasterization
{
    /// <summary>
    /// Rasterizes contours into 8-bit grayscale rasters using signed distance-to-contour mapping.
    /// </summary>
    public sealed class ContourSignedDistanceGray8BitRasterizer : IRasterizer<IContour, Gray8BitRaster>
    {
        private readonly Func<float, byte> _signedDistanceToGrayLevel;

        /// <summary>
        /// Initializes a new contour rasterizer.
        /// </summary>
        /// <param name="signedDistanceToGrayLevel">The function that maps signed distance to the contour to an 8-bit grayscale value. Negative distances are inside the contour; positive distances are outside.</param>
        public ContourSignedDistanceGray8BitRasterizer(Func<float, byte> signedDistanceToGrayLevel)
        {
            _signedDistanceToGrayLevel = signedDistanceToGrayLevel ?? throw new ArgumentNullException(nameof(signedDistanceToGrayLevel));
        }

        /// <inheritdoc/>
        public Gray8BitRaster Rasterize(IContour source, RasterGrid grid)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            ValidateGrid(grid);
            IReadOnlyList<IBoundedParameterizedCurve> curves = GetCurves(source);
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
                    float signedDistance = GetSignedDistanceToContour(source, point, curves);
                    values[x, y] = _signedDistanceToGrayLevel(signedDistance);
                }
            }

            return new Gray8BitRaster(grid, values);
        }

        private static IReadOnlyList<IBoundedParameterizedCurve> GetCurves(IContour contour)
        {
            IReadOnlyList<IBoundedParameterizedCurve> curves = contour.Curves;
            if (curves == null || curves.Count == 0)
                throw new InvalidOperationException("Contour must expose at least one bounded parameterized curve.");

            return curves;
        }

        private static void ValidateGrid(RasterGrid grid)
        {
            if (!grid.Size.IsFinite || grid.Size.X <= 0f || grid.Size.Y <= 0f)
                throw new ArgumentOutOfRangeException(nameof(grid), "Raster grid size components must be finite and positive.");

            if (grid.Resolution.X <= 0 || grid.Resolution.Y <= 0)
                throw new ArgumentOutOfRangeException(nameof(grid), "Raster grid resolution components must be positive.");
        }

        private static float GetSignedDistanceToContour(IContour contour, VectorXY point, IReadOnlyList<IBoundedParameterizedCurve> curves)
        {
            float distance = DistanceToContour(point, curves);
            return contour.Encloses(point) ? -distance : distance;
        }

        private static float DistanceToContour(VectorXY point, IReadOnlyList<IBoundedParameterizedCurve> curves)
        {
            float minDistance = float.MaxValue;

            for (int i = 0; i < curves.Count; i++)
            {
                IBoundedParameterizedCurve curve = curves[i];
                if (curve == null)
                    throw new InvalidOperationException("Contour curves must not contain null values.");

                float distance = curve.Distance(point);
                if (distance < minDistance)
                    minDistance = distance;
            }

            return minDistance;
        }
    }
}
