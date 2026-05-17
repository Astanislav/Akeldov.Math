using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Topology
{
    public static partial class VectorXYIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt[] GetAdjacents(this VectorXYInt index, Layout layout)
        {
            var axisIsEven = layout.IsPointyTop()
                ? (index.Y & 1) == 0
                : (index.X & 1) == 0;

            var relativeOffsets = axisIsEven.GetRelativeOffsets(layout);
            var adjacents = new VectorXYInt[6];
            for (int i = 0; i < 6; i++)
            {
                adjacents[i] = index + relativeOffsets[i];
            }
            return adjacents;
        }
    }
}
