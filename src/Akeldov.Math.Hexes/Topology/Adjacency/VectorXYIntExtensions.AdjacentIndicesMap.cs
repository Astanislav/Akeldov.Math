using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Topology
{
    public static partial class VectorXYIntExtensions
    {
        public static HexMap<AdjacentIndices> ToAdjacentIndicesMap(this VectorXYInt resolution, Layout layout)
        {
            var adjacentIndices = AdjacentIndicesCalculator.Calculate(resolution, layout);
            return new HexMap<AdjacentIndices>(resolution, adjacentIndices);
        }
    }
}
