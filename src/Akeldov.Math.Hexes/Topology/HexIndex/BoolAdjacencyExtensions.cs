using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Topology
{
    public static partial class BoolExtensions
    {
        private static readonly VectorXYInt[] RowUnshiftedOffsets = new VectorXYInt[]
        {
            new VectorXYInt(1, 0),
            new VectorXYInt(0, 1),
            new VectorXYInt(-1, 1),
            new VectorXYInt(-1, 0),
            new VectorXYInt(-1, -1),
            new VectorXYInt(0, -1)
        };

        private static readonly VectorXYInt[] RowShiftedOffsets = new VectorXYInt[]
        {
            new VectorXYInt(1, 0),
            new VectorXYInt(1, 1),
            new VectorXYInt(0, 1),
            new VectorXYInt(-1, 0),
            new VectorXYInt(0, -1),
            new VectorXYInt(1, -1)
        };

        private static readonly VectorXYInt[] ColumnUnshiftedOffsets = new VectorXYInt[]
        {
            new VectorXYInt(1, 0),
            new VectorXYInt(0, 1),
            new VectorXYInt(-1, 0),
            new VectorXYInt(-1, -1),
            new VectorXYInt(0, -1),
            new VectorXYInt(1, -1)
        };

        private static readonly VectorXYInt[] ColumnShiftedOffsets = new VectorXYInt[]
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
                    return axisIsEven ? RowUnshiftedOffsets : RowShiftedOffsets;
                case Layout.EvenR:
                    return axisIsEven ? RowShiftedOffsets : RowUnshiftedOffsets;
                case Layout.OddQ:
                    return axisIsEven ? ColumnUnshiftedOffsets : ColumnShiftedOffsets;
                case Layout.EvenQ:
                    return axisIsEven ? ColumnShiftedOffsets : ColumnUnshiftedOffsets;
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
