using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Topology
{
    public static class HexVertexExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Pair<HexEdge> GetAdjacentEdges(this HexVertex hexVertex)
        {
            var hexVertexIndex = (int)hexVertex;
            var hexEdgeLeftIndex = (hexVertexIndex + 1) % 6;
            var hexEdgeRightIndex = hexVertexIndex;
            return new Pair<HexEdge>((HexEdge)hexEdgeLeftIndex, (HexEdge)hexEdgeRightIndex);
        }
    }
}
