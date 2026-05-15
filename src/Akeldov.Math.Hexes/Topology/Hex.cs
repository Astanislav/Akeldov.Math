using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Topology
{
    public readonly struct Hex
    {
        private readonly VectorXYInt _index;
        private readonly AdjacentIndices _adjacentsIndexes;

        public Hex(VectorXYInt index, AdjacentIndices adjacentIndices)
        {
            _index = index;
            _adjacentsIndexes = adjacentIndices;
        }

        public VectorXYInt Index => _index;

        public AdjacentIndices AdjacentIndices => _adjacentsIndexes;
    }
}
