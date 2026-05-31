using Akeldov.Math.Hexes.Vectors.QRS;
using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Topology
{
    public static class HexVertexExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Pair<HexEdge> GetAdjacentEdges(this HexVertex hexVertex)
        {
            return GetPointyTopAdjacentEdges(hexVertex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Pair<HexEdge> GetAdjacentEdges(this HexVertex hexVertex, Layout layout)
        {
            switch (layout)
            {
                case Layout.OddR:
                case Layout.EvenR:
                    return GetPointyTopAdjacentEdges(hexVertex);
                case Layout.OddQ:
                case Layout.EvenQ:
                    return GetFlatTopAdjacentEdges(hexVertex);
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Pair<HexEdge> GetPointyTopAdjacentEdges(HexVertex hexVertex)
        {
            var hexVertexIndex = (int)hexVertex;
            var hexEdgeLeftIndex = (hexVertexIndex + 1) % 6;
            var hexEdgeRightIndex = hexVertexIndex;
            return new Pair<HexEdge>((HexEdge)hexEdgeLeftIndex, (HexEdge)hexEdgeRightIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Pair<HexEdge> GetFlatTopAdjacentEdges(HexVertex hexVertex)
        {
            var hexVertexIndex = (int)hexVertex;
            var hexEdgeLeftIndex = hexVertexIndex;
            var hexEdgeRightIndex = (hexVertexIndex + 5) % 6;
            return new Pair<HexEdge>((HexEdge)hexEdgeLeftIndex, (HexEdge)hexEdgeRightIndex);
        }
    }
}
