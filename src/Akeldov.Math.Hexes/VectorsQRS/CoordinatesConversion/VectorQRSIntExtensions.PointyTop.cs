using Akeldov.Math.Spatial2D;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Vectors.QRS.PointyTop
{
    public static class VectorQRSIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt ToXYIndex(this VectorQRSInt index)
        {
            int x = index.Q + (index.R >= 0 ? index.R / 2 : (index.R - 1) / 2);
            int y = index.R;

            return new VectorXYInt(x, y);
        }
    }
}