using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Hexes.Topology
{
    public static partial class VectorXYIntExtensions
    {
        private static readonly VectorXYInt[] RowUnshiftedOffsets = new VectorXYInt[]
        {
            new VectorXYInt(1, 0),
            new VectorXYInt(0, 1),
            new VectorXYInt(-1, 1),
            new VectorXYInt(-1, 0),
            new VectorXYInt(-1, -1),
            new VectorXYInt(0, -1)
        };

        private static readonly VectorXYInt[] RowShiftedOffsets = new VectorXYInt[]
        {
            new VectorXYInt(1, 0),
            new VectorXYInt(1, 1),
            new VectorXYInt(0, 1),
            new VectorXYInt(-1, 0),
            new VectorXYInt(0, -1),
            new VectorXYInt(1, -1)
        };

        private static readonly VectorXYInt[] ColumnUnshiftedOffsets = new VectorXYInt[]
        {
            new VectorXYInt(0, 1),
            new VectorXYInt(1, 0),
            new VectorXYInt(1, -1),
            new VectorXYInt(0, -1),
            new VectorXYInt(-1, -1),
            new VectorXYInt(-1, 0)
        };

        private static readonly VectorXYInt[] ColumnShiftedOffsets = new VectorXYInt[]
        {
            new VectorXYInt(0, 1),
            new VectorXYInt(1, 1),
            new VectorXYInt(1, 0),
            new VectorXYInt(0, -1),
            new VectorXYInt(-1, 0),
            new VectorXYInt(-1, 1)
        };

        public static HexMap<AdjacentIndices> ToAdjacentIndicesMap(this VectorXYInt resolution, Layout layout)
        {
            var adjacentIndices = AdjacentIndicesCalculator.Calculate(resolution, layout);
            return new HexMap<AdjacentIndices>(resolution, adjacentIndices);
        }

        public static HexFieldTopology ToHexHieldTopology(this VectorXYInt resolution, Layout layout)
        {
            if (resolution.X < 0 || resolution.Y < 0)
                throw new ArgumentOutOfRangeException(nameof(resolution));

            var count = checked(resolution.X * resolution.Y);
            var adjacentMasks = new byte[count];
            var adjacent0Index = new int[count];
            var adjacent1Index = new int[count];
            var adjacent2Index = new int[count];
            var adjacent3Index = new int[count];
            var adjacent4Index = new int[count];
            var adjacent5Index = new int[count];

            switch (layout)
            {
                case Layout.OddR:
                    FillRowLayoutTopology(resolution, false, adjacentMasks, adjacent0Index, adjacent1Index, adjacent2Index, adjacent3Index, adjacent4Index, adjacent5Index);
                    break;
                case Layout.EvenR:
                    FillRowLayoutTopology(resolution, true, adjacentMasks, adjacent0Index, adjacent1Index, adjacent2Index, adjacent3Index, adjacent4Index, adjacent5Index);
                    break;
                case Layout.OddQ:
                    FillColumnLayoutTopology(resolution, false, adjacentMasks, adjacent0Index, adjacent1Index, adjacent2Index, adjacent3Index, adjacent4Index, adjacent5Index);
                    break;
                case Layout.EvenQ:
                    FillColumnLayoutTopology(resolution, true, adjacentMasks, adjacent0Index, adjacent1Index, adjacent2Index, adjacent3Index, adjacent4Index, adjacent5Index);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }

            return new HexFieldTopology(
                resolution.X,
                resolution.Y,
                adjacentMasks,
                adjacent0Index,
                adjacent1Index,
                adjacent2Index,
                adjacent3Index,
                adjacent4Index,
                adjacent5Index);
        }

        private static void FillRowLayoutTopology(
            VectorXYInt resolution,
            bool evenRowsAreShifted,
            byte[] adjacentMasks,
            int[] adjacent0Index,
            int[] adjacent1Index,
            int[] adjacent2Index,
            int[] adjacent3Index,
            int[] adjacent4Index,
            int[] adjacent5Index)
        {
            for (int y = 0; y < resolution.Y; y++)
            {
                var rowStart = y * resolution.X;
                var rowIsShifted = ((y & 1) == 0) == evenRowsAreShifted;
                var offsets = rowIsShifted ? RowShiftedOffsets : RowUnshiftedOffsets;

                for (int x = 0; x < resolution.X; x++)
                {
                    FillTopologyCell(
                        resolution,
                        x,
                        y,
                        rowStart + x,
                        offsets,
                        adjacentMasks,
                        adjacent0Index,
                        adjacent1Index,
                        adjacent2Index,
                        adjacent3Index,
                        adjacent4Index,
                        adjacent5Index);
                }
            }
        }

        private static void FillColumnLayoutTopology(
            VectorXYInt resolution,
            bool evenColumnsAreShifted,
            byte[] adjacentMasks,
            int[] adjacent0Index,
            int[] adjacent1Index,
            int[] adjacent2Index,
            int[] adjacent3Index,
            int[] adjacent4Index,
            int[] adjacent5Index)
        {
            for (int y = 0; y < resolution.Y; y++)
            {
                var rowStart = y * resolution.X;

                for (int x = 0; x < resolution.X; x++)
                {
                    var columnIsShifted = ((x & 1) == 0) == evenColumnsAreShifted;
                    var offsets = columnIsShifted ? ColumnShiftedOffsets : ColumnUnshiftedOffsets;

                    FillTopologyCell(
                        resolution,
                        x,
                        y,
                        rowStart + x,
                        offsets,
                        adjacentMasks,
                        adjacent0Index,
                        adjacent1Index,
                        adjacent2Index,
                        adjacent3Index,
                        adjacent4Index,
                        adjacent5Index);
                }
            }
        }

        private static void FillTopologyCell(
            VectorXYInt resolution,
            int x,
            int y,
            int flatIndex,
            VectorXYInt[] offsets,
            byte[] adjacentMasks,
            int[] adjacent0Index,
            int[] adjacent1Index,
            int[] adjacent2Index,
            int[] adjacent3Index,
            int[] adjacent4Index,
            int[] adjacent5Index)
        {
            var candidate0X = x + offsets[0].X;
            var candidate0Y = y + offsets[0].Y;
            var candidate1X = x + offsets[1].X;
            var candidate1Y = y + offsets[1].Y;
            var candidate2X = x + offsets[2].X;
            var candidate2Y = y + offsets[2].Y;
            var candidate3X = x + offsets[3].X;
            var candidate3Y = y + offsets[3].Y;
            var candidate4X = x + offsets[4].X;
            var candidate4Y = y + offsets[4].Y;
            var candidate5X = x + offsets[5].X;
            var candidate5Y = y + offsets[5].Y;

            var mask = (byte)0;

            adjacent0Index[flatIndex] = GetAdjacentFlatIndex(resolution, candidate0X, candidate0Y, flatIndex, 0, ref mask);
            adjacent1Index[flatIndex] = GetAdjacentFlatIndex(resolution, candidate1X, candidate1Y, flatIndex, 1, ref mask);
            adjacent2Index[flatIndex] = GetAdjacentFlatIndex(resolution, candidate2X, candidate2Y, flatIndex, 2, ref mask);
            adjacent3Index[flatIndex] = GetAdjacentFlatIndex(resolution, candidate3X, candidate3Y, flatIndex, 3, ref mask);
            adjacent4Index[flatIndex] = GetAdjacentFlatIndex(resolution, candidate4X, candidate4Y, flatIndex, 4, ref mask);
            adjacent5Index[flatIndex] = GetAdjacentFlatIndex(resolution, candidate5X, candidate5Y, flatIndex, 5, ref mask);

            adjacentMasks[flatIndex] = mask;
        }

        private static int GetAdjacentFlatIndex(VectorXYInt resolution, int x, int y, int fallbackFlatIndex, int edge, ref byte mask)
        {
            if ((uint)x >= (uint)resolution.X || (uint)y >= (uint)resolution.Y)
                return fallbackFlatIndex;

            mask |= (byte)(1 << edge);
            return y * resolution.X + x;
        }
    }
}
