using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Hexes.Topology
{
    public class HexField
    {
        private readonly VectorXYInt _resolution;

        private Hex[] _hexes;

        public HexField(VectorXYInt resolution, Layout layout, float apothem)
        {
            _resolution = resolution;

            var adjacentIndices = AdjacentIndicesCalculator.Calculate(resolution, layout);
            _hexes = new Hex[resolution.X * resolution.Y];
            for (int y = 0; y < resolution.Y; y++)
            {
                for (int x = 0; x < resolution.X; x++)
                {
                    var index = new VectorXYInt(x, y);
                    var flatIndex = GetFlatIndex(index);

                    _hexes[flatIndex] = new Hex(index, adjacentIndices[flatIndex]);
                }
            }
        }

        public Hex this[VectorXYInt index]
        {
            get
            {
                if (index.X < 0 || index.X >= _resolution.X ||
                    index.Y < 0 || index.Y >= _resolution.Y)
                    throw new IndexOutOfRangeException($"Hex index out of bounds: {index}");

                return _hexes[GetFlatIndex(index)];
            }
        }

        private int GetFlatIndex(VectorXYInt index) => index.Y * _resolution.X + index.X;
    }
}
