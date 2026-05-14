using Akeldov.Math.Spatial2D;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Vectors.QRS.PointyTop
{
    public static partial class VectorXYIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorQRSInt ToQRSIndex(this VectorXYInt index)
        {
            return new VectorQRSInt(index.X - (index.Y >= 0 ? index.Y / 2 : (index.Y - 1) / 2), index.Y);
        }
    }
}