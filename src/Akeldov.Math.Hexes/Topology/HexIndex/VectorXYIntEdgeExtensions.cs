using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Topology
{
    public static partial class VectorXYIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt GetAdjacent(this VectorXYInt index, HexEdge hexEdge, Layout layout)
        {
            var axisIsEven = layout.IsPointyTop()
                ? (index.Y & 1) == 0
                : (index.X & 1) == 0;

            var relativeNeighborIndex = axisIsEven.GetRelativeOffset(hexEdge, layout);
            return index + relativeNeighborIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt GetAdjacent(this VectorXYInt index, SixfoldAngle direction, Layout layout)
        {
            return index.GetAdjacent(direction.ToHexEdge(), layout);
        }

        private static HexEdge ToHexEdge(this SixfoldAngle direction)
        {
            switch (direction)
            {
                case SixfoldAngle.Deg0:
                    return HexEdge.Edge0;
                case SixfoldAngle.Deg60:
                    return HexEdge.Edge1;
                case SixfoldAngle.Deg120:
                    return HexEdge.Edge2;
                case SixfoldAngle.Deg180:
                    return HexEdge.Edge3;
                case SixfoldAngle.Deg240:
                    return HexEdge.Edge4;
                case SixfoldAngle.Deg300:
                    return HexEdge.Edge5;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }
        }
    }
}
