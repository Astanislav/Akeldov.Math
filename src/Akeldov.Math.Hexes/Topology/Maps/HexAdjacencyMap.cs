using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Topology
{
    public sealed class HexAdjacencyMap : IHexMap<HexAdjacency>
    {
        private readonly HexAdjacency[] _adjacent;

        public HexAdjacencyMap(
            int width,
            int height,
            Layout layout)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException(nameof(width));

            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            Width = width;
            Height = height;
            Layout = layout;
            _adjacent = new HexAdjacency[checked(width * height)];

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

        public int Count => _adjacent.Length;

        public Layout Layout { get; }

        public HexAdjacency[] Adjacent => _adjacent;

        public HexAdjacency this[VectorXYInt index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index.X < 0 || index.X >= Width ||
                    index.Y < 0 || index.Y >= Height)
                    throw new IndexOutOfRangeException($"Hex index out of bounds: {index}");

                return _adjacent[GetFlatIndex(index)];
            }
        }

        public HexAdjacency this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _adjacent[index];
        }

        private int GetFlatIndex(VectorXYInt index) => index.Y * Width + index.X;

        private void FillRowLayoutTopology(bool evenRowsAreShifted)
        {
            for (int y = 0; y < Height; y++)
            {
                var rowStart = y * Width;
                var offsets = HexAdjacencyOffsets.GetRowOffsets(y, evenRowsAreShifted);

                for (int x = 0; x < Width; x++)
                {
                    var flatIndex = rowStart + x;
                    _adjacent[flatIndex] = CreateAdjacency(x, y, flatIndex, offsets);
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
                    var offsets = HexAdjacencyOffsets.GetColumnOffsets(x, evenColumnsAreShifted);
                    var flatIndex = rowStart + x;
                    _adjacent[flatIndex] = CreateAdjacency(x, y, flatIndex, offsets);
                }
            }
        }

        private HexAdjacency CreateAdjacency(
            int x,
            int y,
            int flatIndex,
            sbyte[] offsets)
        {
            var flags = HexAdjacencyFlags.None;

            int adjacent0Index = GetAdjacentFlatIndex(x + offsets[0], y + offsets[1], flatIndex, HexAdjacencyFlags.Adjacent0, ref flags);
            int adjacent1Index = GetAdjacentFlatIndex(x + offsets[2], y + offsets[3], flatIndex, HexAdjacencyFlags.Adjacent1, ref flags);
            int adjacent2Index = GetAdjacentFlatIndex(x + offsets[4], y + offsets[5], flatIndex, HexAdjacencyFlags.Adjacent2, ref flags);
            int adjacent3Index = GetAdjacentFlatIndex(x + offsets[6], y + offsets[7], flatIndex, HexAdjacencyFlags.Adjacent3, ref flags);
            int adjacent4Index = GetAdjacentFlatIndex(x + offsets[8], y + offsets[9], flatIndex, HexAdjacencyFlags.Adjacent4, ref flags);
            int adjacent5Index = GetAdjacentFlatIndex(x + offsets[10], y + offsets[11], flatIndex, HexAdjacencyFlags.Adjacent5, ref flags);

            return new HexAdjacency(
                flags,
                adjacent0Index,
                adjacent1Index,
                adjacent2Index,
                adjacent3Index,
                adjacent4Index,
                adjacent5Index);
        }

        private int GetAdjacentFlatIndex(
            int x,
            int y,
            int fallbackFlatIndex,
            HexAdjacencyFlags flag,
            ref HexAdjacencyFlags flags)
        {
            if ((uint)x >= (uint)Width || (uint)y >= (uint)Height)
                return fallbackFlatIndex;

            flags |= flag;
            return y * Width + x;
        }
    }
}
