using System;
using Akeldov.Math.Spatial2D.Fields;
using Akeldov.Math.Spatial2D.Imaging;

namespace Akeldov.Math.Spatial2D.Rasterization
{
    /// <summary>
    /// Rasterizes floating-point influence fields into 16-bit RGBA rasters using a heat map color scale.
    /// </summary>
    public sealed class FloatPointInfluenceFieldHeatMapRGBA16BitRasterizer :
        IRasterizer<FloatPointInfluenceField, RGBA16BitRaster>
    {
        private static readonly RGBA16BitColor[] HeatMapStops =
        {
            new RGBA16BitColor(0, 0, ushort.MaxValue, ushort.MaxValue),
            new RGBA16BitColor(0, ushort.MaxValue, ushort.MaxValue, ushort.MaxValue),
            new RGBA16BitColor(0, ushort.MaxValue, 0, ushort.MaxValue),
            new RGBA16BitColor(ushort.MaxValue, ushort.MaxValue, 0, ushort.MaxValue),
            new RGBA16BitColor(ushort.MaxValue, 0, 0, ushort.MaxValue)
        };

        /// <inheritdoc/>
        public RGBA16BitRaster Rasterize(FloatPointInfluenceField source, RasterGrid grid)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            ValidateGrid(grid);
            ValidateRange(source);

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
                    float value = source.Sample(point);
                    values[y * grid.Resolution.X + x] = ToHeatMapColor(Normalize(value, source.Min, source.Max));
                }
            }

            return new RGBA16BitRaster(grid, values);
        }

        /// <summary>
        /// Converts a normalized value to a 16-bit RGBA heat map color.
        /// </summary>
        /// <param name="normalizedValue">The normalized value. Values outside the 0..1 range are clamped.</param>
        /// <returns>A 16-bit RGBA color on the blue-cyan-green-yellow-red heat map scale.</returns>
        public static RGBA16BitColor ToHeatMapColor(float normalizedValue)
        {
            if (float.IsNaN(normalizedValue) || float.IsInfinity(normalizedValue))
                throw new ArgumentOutOfRangeException(nameof(normalizedValue), "Normalized heat map value must be finite.");

            normalizedValue = MathF.Min(MathF.Max(normalizedValue, 0f), 1f);
            float scaled = normalizedValue * (HeatMapStops.Length - 1);
            int index = (int)MathF.Floor(scaled);

            if (index >= HeatMapStops.Length - 1)
                return HeatMapStops[HeatMapStops.Length - 1];

            float amount = scaled - index;
            return Blend(HeatMapStops[index], HeatMapStops[index + 1], amount);
        }

        private static float Normalize(float value, float min, float max)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
                throw new InvalidOperationException("Influence field returned an invalid value. Values must be finite.");

            if (min == max)
                return 0.5f;

            return (value - min) / (max - min);
        }

        private static RGBA16BitColor Blend(RGBA16BitColor from, RGBA16BitColor to, float amount)
        {
            float inverseAmount = 1f - amount;
            return new RGBA16BitColor(
                BlendChannel(from.Red, to.Red, amount, inverseAmount),
                BlendChannel(from.Green, to.Green, amount, inverseAmount),
                BlendChannel(from.Blue, to.Blue, amount, inverseAmount),
                BlendChannel(from.Alpha, to.Alpha, amount, inverseAmount));
        }

        private static ushort BlendChannel(ushort from, ushort to, float amount, float inverseAmount)
        {
            return (ushort)MathF.Round(from * inverseAmount + to * amount);
        }

        private static void ValidateRange(FloatPointInfluenceField source)
        {
            if (float.IsNaN(source.Min) || float.IsInfinity(source.Min) ||
                float.IsNaN(source.Max) || float.IsInfinity(source.Max) ||
                source.Max < source.Min)
            {
                throw new ArgumentException("Influence field range must be finite and ordered.", nameof(source));
            }
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
