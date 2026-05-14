using Akeldov.Math.Spatial2D;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Vectors.QRS
{
    public static partial class VectorXYIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorQRSInt ToQRSIndex(this VectorXYInt index, Layout layout)
        {
            if (layout.IsPointyTop())
            {
                return new VectorQRSInt(index.X - (index.Y >= 0 ? index.Y / 2 : (index.Y - 1) / 2), index.Y);
            }
            else
            {
                return new VectorQRSInt(index.X, index.Y - (index.X >= 0 ? index.X / 2 : (index.X - 1) / 2));
            }
        }
    }
}