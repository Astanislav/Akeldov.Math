using Akeldov.Math.Spatial2D;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Vectors.QRS.FlatTop
{
    public static class VectorQRSIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt ToXYIndex(this VectorQRSInt index)
        {
            int x = index.Q;
            int y = index.R + (index.Q >= 0 ? index.Q / 2 : (index.Q - 1) / 2);

            return new VectorXYInt(x, y);
        }
    }
}