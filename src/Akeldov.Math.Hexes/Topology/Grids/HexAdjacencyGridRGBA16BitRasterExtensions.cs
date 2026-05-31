using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Imaging;
using Akeldov.Math.Spatial2D.Rasterization;
using System;

namespace Akeldov.Math.Hexes.Topology
{
    public static class HexAdjacencyGridRGBA16BitRasterExtensions
    {
        public static RGBA16BitRaster ToRGBA16BitRaster(
            this HexAdjacencyGrid grid,
            Func<HexAdjacency, RGBA16BitColor> adjacencyToColor)
        {
            return grid.ToRGBA16BitRaster(adjacencyToColor, default(RGBA16BitColor));
        }

        public static RGBA16BitRaster ToRGBA16BitRaster(
            this HexAdjacencyGrid grid,
            Func<HexAdjacency, RGBA16BitColor> adjacencyToColor,
            RGBA16BitColor emptyColor)
        {
            if (grid == null)
                throw new ArgumentNullException(nameof(grid));

            if (adjacencyToColor == null)
                throw new ArgumentNullException(nameof(adjacencyToColor));

            var values = new RGBA16BitColor[grid.Count];

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = grid.HasHex[i]
                    ? adjacencyToColor(grid.Adjacent[i])
                    : emptyColor;
            }

            return new RGBA16BitRaster(CreateRasterGrid(grid), values);
        }

        public static RGBA16BitRaster ToRGBA16BitRaster(
            this HexAdjacencyGrid grid,
            Func<int, HexAdjacency, RGBA16BitColor> hexToColor)
        {
            return grid.ToRGBA16BitRaster(hexToColor, default(RGBA16BitColor));
        }

        public static RGBA16BitRaster ToRGBA16BitRaster(
            this HexAdjacencyGrid grid,
            Func<int, HexAdjacency, RGBA16BitColor> hexToColor,
            RGBA16BitColor emptyColor)
        {
            if (grid == null)
                throw new ArgumentNullException(nameof(grid));

            if (hexToColor == null)
                throw new ArgumentNullException(nameof(hexToColor));

            var values = new RGBA16BitColor[grid.Count];

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = grid.HasHex[i]
                    ? hexToColor(grid.HexIndices[i], grid.Adjacent[i])
                    : emptyColor;
            }

            return new RGBA16BitRaster(CreateRasterGrid(grid), values);
        }

        private static RasterGrid CreateRasterGrid(HexAdjacencyGrid grid)
        {
            return new RasterGrid(
                (PointXY)grid.Origin,
                grid.Size,
                grid.Resolution);
        }
    }
}
