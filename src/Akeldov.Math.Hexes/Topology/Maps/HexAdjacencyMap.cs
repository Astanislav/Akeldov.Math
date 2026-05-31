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
            : this(new HexFieldTopologySoA(width, height, layout))
        {
        }

        public HexAdjacencyMap(HexFieldTopologySoA source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            int count = checked(source.Width * source.Height);
            var adjacent = new HexAdjacency[count];

            for (int i = 0; i < count; i++)
            {
                adjacent[i] = new HexAdjacency(
                    source.HasAdjacent[i],
                    source.Adjacent0Index[i],
                    source.Adjacent1Index[i],
                    source.Adjacent2Index[i],
                    source.Adjacent3Index[i],
                    source.Adjacent4Index[i],
                    source.Adjacent5Index[i]);
            }

            Width = source.Width;
            Height = source.Height;
            Layout = source.Layout;
            _adjacent = adjacent;
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
    }
}
