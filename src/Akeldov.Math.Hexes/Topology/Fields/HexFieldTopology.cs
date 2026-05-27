using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Hexes.Topology
{
    public class HexFieldTopology
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

        public HexFieldTopology(
            int width,
            int height,
            Layout layout)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException(nameof(width));

            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            var count = checked(width * height);

            Width = width;
            Height = height;
            Layout = layout;
            HasAdjacent = new byte[count];
            Adjacent0Index = new int[count];
            Adjacent1Index = new int[count];
            Adjacent2Index = new int[count];
            Adjacent3Index = new int[count];
            Adjacent4Index = new int[count];
            Adjacent5Index = new int[count];

            switch (layout)
            {
                case Layout.OddR:
                    FillRowLayoutTopology(false);
                    break;
                case Layout.EvenR:
                    FillRowLayoutTopology(true);
                    break;
                case Layout.OddQ:
                    FillColumnLayoutTopology(false);
                    break;
                case Layout.EvenQ:
                    FillColumnLayoutTopology(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }
        }

        public int Width { get; }
        public int Height { get; }
        public Layout Layout { get; }
        public byte[] HasAdjacent { get; }
        public int[] Adjacent0Index { get; }
        public int[] Adjacent1Index { get; }
        public int[] Adjacent2Index { get; }
        public int[] Adjacent3Index { get; }
        public int[] Adjacent4Index { get; }
        public int[] Adjacent5Index { get; }

        private void FillRowLayoutTopology(bool evenRowsAreShifted)
        {
            for (int y = 0; y < Height; y++)
            {
                var rowStart = y * Width;
                var rowIsShifted = ((y & 1) == 0) == evenRowsAreShifted;
                var offsets = rowIsShifted ? RowShiftedOffsets : RowUnshiftedOffsets;

                for (int x = 0; x < Width; x++)
                {
                    FillTopologyCell(
                        x,
                        y,
                        rowStart + x,
                        offsets);
                }
            }
        }

        private void FillColumnLayoutTopology(bool evenColumnsAreShifted)
        {
            for (int y = 0; y < Height; y++)
            {
                var rowStart = y * Width;

                for (int x = 0; x < Width; x++)
                {
                    var columnIsShifted = ((x & 1) == 0) == evenColumnsAreShifted;
                    var offsets = columnIsShifted ? ColumnShiftedOffsets : ColumnUnshiftedOffsets;

                    FillTopologyCell(
                        x,
                        y,
                        rowStart + x,
                        offsets);
                }
            }
        }

        private void FillTopologyCell(
            int x,
            int y,
            int flatIndex,
            VectorXYInt[] offsets)
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

            Adjacent0Index[flatIndex] = GetAdjacentFlatIndex(candidate0X, candidate0Y, flatIndex, 0, ref mask);
            Adjacent1Index[flatIndex] = GetAdjacentFlatIndex(candidate1X, candidate1Y, flatIndex, 1, ref mask);
            Adjacent2Index[flatIndex] = GetAdjacentFlatIndex(candidate2X, candidate2Y, flatIndex, 2, ref mask);
            Adjacent3Index[flatIndex] = GetAdjacentFlatIndex(candidate3X, candidate3Y, flatIndex, 3, ref mask);
            Adjacent4Index[flatIndex] = GetAdjacentFlatIndex(candidate4X, candidate4Y, flatIndex, 4, ref mask);
            Adjacent5Index[flatIndex] = GetAdjacentFlatIndex(candidate5X, candidate5Y, flatIndex, 5, ref mask);

            HasAdjacent[flatIndex] = mask;
        }

        private int GetAdjacentFlatIndex(int x, int y, int fallbackFlatIndex, int edge, ref byte mask)
        {
            if ((uint)x >= (uint)Width || (uint)y >= (uint)Height)
                return fallbackFlatIndex;

            mask |= (byte)(1 << edge);
            return y * Width + x;
        }
    }
}
