using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Topology
{
    public static partial class VectorXYIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triplet<VectorXYInt> GetAdjacentTriplet(this VectorXYInt hexIndex, HexVertex hexVertex, Layout layout)
        {
            var (leftEdge, rightEdge) = hexVertex.GetAdjacentEdges();
            var leftIndex = hexIndex.GetAdjacent(leftEdge, layout);
            var rightIndex = hexIndex.GetAdjacent(rightEdge, layout);
            return new Triplet<VectorXYInt>(hexIndex, leftIndex, rightIndex);
        }
    }
}
