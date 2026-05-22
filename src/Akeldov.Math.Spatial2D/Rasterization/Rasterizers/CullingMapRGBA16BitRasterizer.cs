using System;
using System.Collections.Generic;
using Akeldov.Math.Spatial2D.Fields;
using Akeldov.Math.Spatial2D.Imaging;

namespace Akeldov.Math.Spatial2D.Rasterization
{
    /// <summary>
    /// Rasterizes point-source culling selections into a 16-bit RGBA raster.
    /// </summary>
    /// <remarks>
    /// Each source is assigned a color by its index in the rasterized source list. For every raster cell,
    /// the configured culler selects relevant sources at the cell center and the rasterizer writes the
    /// average color of the selected sources.
    /// </remarks>
    /// <typeparam name="TPointSource">The point influence source type.</typeparam>
    public sealed class CullingMapRGBA16BitRasterizer<TPointSource> :
        IRasterizer<IReadOnlyList<TPointSource>, RGBA16BitRaster>
        where TPointSource : IPointInfluenceSource
    {
        private static readonly RGBA16BitColor[] DefaultPalette =
        {
            new RGBA16BitColor(0xefef, 0x4444, 0x4444, 0xffff),
            new RGBA16BitColor(0x2222, 0xc5c5, 0x5e5e, 0xffff),
            new RGBA16BitColor(0x3b3b, 0x8282, 0xf6f6, 0xffff),
            new RGBA16BitColor(0xf5f5, 0x9e9e, 0x0b0b, 0xffff),
            new RGBA16BitColor(0xa8a8, 0x5555, 0xf7f7, 0xffff)
        };

        private readonly IInfluenceSourceCuller<TPointSource> _culler;
        private readonly RGBA16BitColor[] _sourceColors;

        /// <summary>
        /// Initializes a new culling map rasterizer with the default source color palette.
        /// </summary>
        /// <param name="culler">The culler used to select sources for each raster cell center.</param>
        public CullingMapRGBA16BitRasterizer(IInfluenceSourceCuller<TPointSource> culler)
            : this(culler, DefaultPalette)
        {
        }

        /// <summary>
        /// Initializes a new culling map rasterizer with the specified source colors.
        /// </summary>
        /// <param name="culler">The culler used to select sources for each raster cell center.</param>
        /// <param name="sourceColors">
        /// The source color palette. Source indexes beyond this palette wrap around to the beginning.
        /// </param>
        public CullingMapRGBA16BitRasterizer(
            IInfluenceSourceCuller<TPointSource> culler,
            IReadOnlyList<RGBA16BitColor> sourceColors)
        {
            _culler = culler ?? throw new ArgumentNullException(nameof(culler));
            _sourceColors = CopySourceColors(sourceColors);
        }

        /// <inheritdoc/>
        public RGBA16BitRaster Rasterize(IReadOnlyList<TPointSource> source, RasterGrid grid)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            ValidateGrid(grid);
            TPointSource[] sources = CopySources(source);
            var values = new RGBA16BitColor[checked(grid.Resolution.X * grid.Resolution.Y)];
            VectorXY cellSize = grid.CellSize;
            float firstX = grid.Origin.X + cellSize.X * 0.5f;
            float firstY = grid.Origin.Y + cellSize.Y * 0.5f;

            for (int y = 0; y < grid.Resolution.Y; y++)
            {
                float pointY = firstY + y * cellSize.Y;
                for (int x = 0; x < grid.Resolution.X; x++)
                {
                    PointXY point = new PointXY(firstX + x * cellSize.X, pointY);
                    IReadOnlyList<TPointSource> selectedSources = Cull(point);
                    values[y * grid.Resolution.X + x] = GetSelectionColor(sources, selectedSources);
                }
            }

            return new RGBA16BitRaster(grid, values);
        }

        private IReadOnlyList<TPointSource> Cull(PointXY point)
        {
            IReadOnlyList<TPointSource> selectedSources = _culler.Cull(point);

            if (selectedSources == null)
                throw new InvalidOperationException(
                    "Influence source culler returned null. Culler implementations must return a non-empty source list.");

            if (selectedSources.Count == 0)
                throw new InvalidOperationException(
                    "Influence source culler returned an empty source list. Culler implementations must return at least one source.");

            return selectedSources;
        }

        private RGBA16BitColor GetSelectionColor(
            IReadOnlyList<TPointSource> sources,
            IReadOnlyList<TPointSource> selectedSources)
        {
            ulong red = 0UL;
            ulong green = 0UL;
            ulong blue = 0UL;
            ulong alpha = 0UL;

            for (int i = 0; i < selectedSources.Count; i++)
            {
                TPointSource selectedSource = selectedSources[i];
                if (selectedSource is null)
                    throw new InvalidOperationException("Influence source culler returned a source list containing null.");

                int sourceIndex = IndexOfSource(sources, selectedSource);
                if (sourceIndex < 0)
                    throw new InvalidOperationException(
                        "Influence source culler returned a source that is not present in the rasterized source list.");

                RGBA16BitColor color = _sourceColors[sourceIndex % _sourceColors.Length];
                red += color.Red;
                green += color.Green;
                blue += color.Blue;
                alpha += color.Alpha;
            }

            ulong halfCount = (ulong)selectedSources.Count / 2UL;
            return new RGBA16BitColor(
                (ushort)((red + halfCount) / (ulong)selectedSources.Count),
                (ushort)((green + halfCount) / (ulong)selectedSources.Count),
                (ushort)((blue + halfCount) / (ulong)selectedSources.Count),
                (ushort)((alpha + halfCount) / (ulong)selectedSources.Count));
        }

        private static int IndexOfSource(IReadOnlyList<TPointSource> sources, TPointSource selectedSource)
        {
            var comparer = EqualityComparer<TPointSource>.Default;

            for (int i = 0; i < sources.Count; i++)
            {
                if (comparer.Equals(sources[i], selectedSource))
                    return i;
            }

            return -1;
        }

        private static TPointSource[] CopySources(IReadOnlyList<TPointSource> source)
        {
            if (source.Count == 0)
                throw new ArgumentException("Culling map source collection must not be empty.", nameof(source));

            var copy = new TPointSource[source.Count];
            for (int i = 0; i < source.Count; i++)
            {
                TPointSource pointSource = source[i];
                if (pointSource is null)
                    throw new ArgumentException("Culling map source collection cannot contain null elements.", nameof(source));

                if (!PointXYValidation.IsFinite(pointSource.Position))
                    throw new ArgumentException("Culling map source positions must be finite.", nameof(source));

                copy[i] = pointSource;
            }

            return copy;
        }

        private static RGBA16BitColor[] CopySourceColors(IReadOnlyList<RGBA16BitColor> sourceColors)
        {
            if (sourceColors == null)
                throw new ArgumentNullException(nameof(sourceColors));

            if (sourceColors.Count == 0)
                throw new ArgumentException("Culling map source color palette must not be empty.", nameof(sourceColors));

            var copy = new RGBA16BitColor[sourceColors.Count];
            for (int i = 0; i < sourceColors.Count; i++)
                copy[i] = sourceColors[i];

            return copy;
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
