using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Geometry
{
    public readonly struct HexGeometry
    {
        private readonly VectorXYInt _index;
        private readonly AdjacentIndices _adjacentsIndexes;

        private readonly VectorXY _center;

        public HexGeometry(VectorXYInt index, AdjacentIndices adjacentIndices, VectorXY center)
        {
            _index = index;
            _adjacentsIndexes = adjacentIndices;

            _center = center;
        }

        public VectorXYInt Index => _index;

        public AdjacentIndices AdjacentIndices => _adjacentsIndexes;

        public VectorXY Center => _center;
    }
}
