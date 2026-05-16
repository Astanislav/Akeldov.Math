using System;
using System.Collections.Generic;
using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Imaging;
using Akeldov.Math.Spatial2D.Rasterization;

namespace Akeldov.Math.Spatial2D.Contours.Rasterization
{
    /// <summary>
    /// Rasterizes contours into 16-bit grayscale rasters using signed distance-to-contour mapping.
    /// </summary>
    public sealed class ContourSignedDistanceGray16BitRasterizer : IRasterizer<IContour, Gray16BitRaster>
    {
        private readonly Func<float, ushort> _distanceToGrayLevel;

        /// <summary>
        /// Initializes a new contour rasterizer.
        /// </summary>
        /// <param name="distanceToGrayLevel">The function that maps signed distance to the contour to a 16-bit grayscale value. Negative distances are inside the contour; positive distances are outside.</param>
        public ContourSignedDistanceGray16BitRasterizer(Func<float, ushort> distanceToGrayLevel)
        {
            _distanceToGrayLevel = distanceToGrayLevel ?? throw new ArgumentNullException(nameof(distanceToGrayLevel));
        }

        /// <inheritdoc/>
        public Gray16BitRaster Rasterize(IContour source, RasterGrid grid)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            IReadOnlyList<IBoundedParameterizedCurve> curves = GetCurves(source);
            var values = new ushort[grid.Resolution.X, grid.Resolution.Y];
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
                    values[x, y] = _distanceToGrayLevel(signedDistance);
                }
            }

            return new Gray16BitRaster(grid, values);
        }

        private static IReadOnlyList<IBoundedParameterizedCurve> GetCurves(IContour contour)
        {
            IReadOnlyList<IBoundedParameterizedCurve> curves = contour.Curves;
            if (curves == null || curves.Count == 0)
                throw new InvalidOperationException("Contour must expose at least one bounded parameterized curve.");

            return curves;
        }

        private static float GetSignedDistanceToContour(IContour contour, VectorXY point, IReadOnlyList<IBoundedParameterizedCurve> curves)
        {
            float distance = DistanceToContour(point, curves);
            return contour.Contains(point) ? -distance : distance;
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
