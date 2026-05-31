using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Topology
{
    public static partial class BoolExtensions
    {
        private static readonly VectorXYInt[] ColumnUnshiftedEdgeOffsets = new VectorXYInt[]
        {
            new VectorXYInt(1, 0),
            new VectorXYInt(0, 1),
            new VectorXYInt(-1, 0),
            new VectorXYInt(-1, -1),
            new VectorXYInt(0, -1),
            new VectorXYInt(1, -1)
        };

        private static readonly VectorXYInt[] ColumnShiftedEdgeOffsets = new VectorXYInt[]
        {
            new VectorXYInt(1, 1),
            new VectorXYInt(0, 1),
            new VectorXYInt(-1, 1),
            new VectorXYInt(-1, 0),
            new VectorXYInt(0, -1),
            new VectorXYInt(1, 0)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt[] GetRelativeOffsets(this bool axisIsEven, Layout layout)
        {
            switch (layout)
            {
                case Layout.OddR:
                    return HexAdjacencyOffsets.GetRowVectorOffsets(axisIsEven, false);
                case Layout.EvenR:
                    return HexAdjacencyOffsets.GetRowVectorOffsets(axisIsEven, true);
                case Layout.OddQ:
                    return axisIsEven ? ColumnUnshiftedEdgeOffsets : ColumnShiftedEdgeOffsets;
                case Layout.EvenQ:
                    return axisIsEven ? ColumnShiftedEdgeOffsets : ColumnUnshiftedEdgeOffsets;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt GetRelativeOffset(this bool axisIsEven, HexEdge hexEdge, Layout layout)
        {
            return axisIsEven.GetRelativeOffsets(layout)[(int)hexEdge];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt GetRelativeOffset(this bool axisIsEven, int hexEdge, Layout layout)
        {
            return axisIsEven.GetRelativeOffsets(layout)[hexEdge];
        }
    }
}
